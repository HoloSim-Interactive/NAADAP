# Vehicle recommendation: build-time fit, ship-frozen pipeline

**Status: PROPOSED.** Not yet in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank">docs/SDD.md</a>
and not yet backed by approved RTVM items. The requirements this design
would satisfy (KB-600..630, CORE-270..289, DATA-IN-130..175,
DATA-OUT-320..385, OUT-450..490, NFR-540..570) were derived on 2026-09-15
and are preserved in
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/requirements-derivation/DERIVED-REQUIREMENTS.md" target="_blank">docs/requirements-derivation/</a>
but only 7 of 100 adversarial vet verdicts have been run. **Nothing here
may be built against until those are vetted and the design is accepted
into the SDD** — that is the project's own rule
(<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank">RTVM</a>
header: no line items against a [PROPOSED] item).

This document exists so the pipeline stops living in conversation. It
records what was decided, why, with the research each decision rests on,
and what has to happen before anyone writes code.

## What this replaces

Today `Naadap.Output/VehicleRecommender.cs` ranks clusters by an internal
cohesion score and labels them with top TF-IDF terms. There is no vehicle
knowledge base anywhere in the codebase, so the pipeline cannot name a real
vehicle; it can only emit a well-formed ranked list of things it has no
principled way to identify. The judges score membership in PGIL's list of
20 correct vehicle candidates. A cluster labeled "aircraft, maintenance,
support" is not a candidate. This design closes that gap.

## The pipeline

### Build (offline, outside the container, once per release)

1. **Pull the historical record.** USAspending bulk archive,
   `https://files.usaspending.gov/award_data_archive/`, monthly
   `FY{yyyy}_All_Contracts_Full_{yyyymmdd}.zip`, ~1.2 GB per fiscal year,
   CSV inside, no key, no rate limit. Fully offline once downloaded. Do
   not use the FPDS ATOM feed — it retires in FY2026.
2. **Filter to orders under vehicles.** Keep rows where
   `parent_award_id_piid` is non-null (the Referenced IDV PIID — this is
   the label) and `awarding_office_code` is in the NAVAIR set: N00019,
   N00421, N68335, N61340, N68936, N68520. Widen to all DON offices for a
   second, larger fit if the NAVAIR-only count is thin (see Gate 1).
3. **Join each order to its parent vehicle.** `POST /api/v2/idvs/awards/`
   (no key) returns the IDV's own attributes: `last_date_to_order`,
   ceiling, `idv_type`, awarding office. This populates the vehicle
   knowledge base and gives each historical order its eligible choice set.
4. **Build the design matrix.** Per order: PSC, NAICS, issuing office
   DoDAAC, set-aside type, dollar band, character n-grams over the
   250-character `award_description`, and **every SME labeling function's
   output as a binary feature**. Per order, the choice set is the vehicles
   that office could order from on that award date, per the knowledge
   base's ordering-period and eligibility fields — hard constraints
   applied *before* the model sees the row.
5. **Fit conditional logit.** McFadden's model: utility
   `U_j = βᵀx_j`, probability `exp(U_j) / Σ_k exp(U_k)` over the eligible
   set. Include issuing-office covariates and an explicit incumbency
   indicator. Add a small ridge penalty to guarantee strict concavity. Fit
   by Newton-Raphson from β = 0 with fixed summation order. Log-likelihood
   is globally concave, so the fit is initialization-independent and
   bit-reproducible.
6. **Emit and attest.** `β`, the feature vocabulary, the vehicle knowledge
   base (TSV/JSONL with PROV-named provenance columns), the labeling
   function registry, and a `context.jsonld`. Canonicalize (frozen column
   order, sorted rows), SHA-256 each file, record hashes in a manifest.

### Ship

The container carries only frozen artifacts: `β`, vocabulary, knowledge
base, LF registry, manifest. No optimizer, no fitting code, no network
client. Every file's hash is recorded and checked at startup.

### Run (inside the container, per document set)

1. **Extract** the same features from each SOW/PWS/sources-sought that the
   build extracted from FPDS: PSC and NAICS (present or predicted), office
   DoDAAC, set-aside, contract-type hints, incumbent contract numbers,
   character n-grams over the task statements after boilerplate stripping.
   Run every labeling function; record which fired and on what span.
2. **Cluster** (existing CORE-200 path, with the singleton-cohesion fix).
3. **Filter** the vehicle set per cluster by hard constraints — ordering
   period open past the intended performance period, office eligible,
   contract type permitted, ceiling headroom. A failed constraint removes
   the vehicle; it never lowers a score.
4. **Score** the surviving vehicles: `βᵀx_j`, softmax, rank. Record the
   margin between adjacent ranks and the tie group.
5. **Assemble the evidence record** per candidate: the top coefficient ×
   feature contributions, the labeling functions that fired with spans,
   the k nearest historical FPDS orders under that vehicle (PIID, office,
   obligation, date), the constraint results, the FAR 7.107 inputs
   (aggregate value, count of separate current contracts covering the
   cluster's members), and the small-business impact note.
6. **Emit** the output record per
   <a href="../../.claude/skills/gov-acquisition-sme/references/vehicle-matching.md">vehicle-matching.md</a>,
   labeled as decision support, with the named approval authority the
   evidence feeds.

Runtime is pure arithmetic and retrieval. Determinism is a property of an
evaluator, not an optimizer.

## Decisions and their basis

| Decision | Basis |
| --- | --- |
| Conditional logit, not a weak-supervision label model | FPDS already carries the label (Referenced IDV PIID); a latent-variable estimator over labeled data solves a problem we do not have. Research B (weak supervision) reached this conclusion independently and called it "the cheapest check and most likely to change the plan." Research C (learning from decisions) recommended conditional logit as primary. |
| Not inverse RL, not behavioral cloning | At horizon 1, MaxEnt IRL reduces algebraically to conditional logit — same probability, same gradient. Research C, with the reduction shown. |
| Office covariates + incumbency coefficient, not inverse propensity weighting | Choice-set confounding (Tomlinson, Ugander & Benson, KDD '21): offices have different eligible menus *and* different habits, so both conditions for unconfounded choice data fail. IPW needs a propensity over a choice set FPDS does not record. Regression controls are the remedy the paper recommends. Research C. |
| Freeze all learning at build time; ship hashed artifacts | Three research passes converged independently: B ("fit offline, ship μ̂ as a checksummed artifact"), E ("freeze the model; determinism becomes a property of an evaluator"), D ("versioned, hashed, schema-validated tabular KB"). Caveat: all six prompts carried the same constraints paragraph, so this is corroboration, not six independent votes. |
| Tabular KB with PROV provenance columns, not RDF/OWL | Research D: the data is a few dozen typed rows; RDF canonicalization has no verified .NET implementation; OWL reasoning is unbounded; dotNetRDF pulls three dependencies into a project whose zero-dependency rule is enforced by TP-920. Escape hatch: JSON Lines + `context.jsonld` *is* JSON-LD if SPARQL is ever needed. |
| Labeling functions are features, not label substitutes | Research B: the LF abstraction is the right SME-capture artifact (deterministic, citable, versioned); the label model is not. As features in the logit, each LF's coefficient measures whether the expert's rule actually predicted historical choice — knowledge capture with automatic validation. |
| Hard constraints filter, never score | Research E (CBR): criticality (disqualifying) and predictive strength (ranking) must not collapse into one distance. Also vehicle-matching.md's existing rule. |
| Metrics: MRR and Recall@5; label them "agreement with historical practice" | Research C: one positive per query, no graded relevance, so NDCG's machinery is inert. A confounded model gets *better* held-out accuracy by learning the confound, so held-out accuracy is not evidence of good recommendation. <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> currently says "precision@5 or F1 against validation ground truth" and needs amending. |
| ML.NET FastTree as benchmark only; never ML.NET LightGBM | Research C read the source: FastTree is real LambdaMART with no default stochasticity, pure managed, MIT. LightGBM's ML.NET options surface lacks the `Deterministic` field LightGBM's own docs require. ML.NET does not ship conditional logit; `LbfgsMaximumEntropy` is a different model. |

## The "was it right" channel

**This is positioning-critical and it is a research gap.** FPDS records
what was chosen. It has no field for whether the choice was correct. A
model fit to FPDS learns historical practice, including every default and
rut in it. The submission narrative — and the follow-on OTA narrative
especially — is far stronger if the tool can say not only "this is what
NAVAIR usually does" but "and here is where doing that went badly."

What exists:

- **CPARS** (FAR 42.15) is the direct outcome signal: government-authored
  ratings on Quality, Schedule, Cost Control, Management, Small Business
  Utilization, Regulatory Compliance. **It is source-selection sensitive,
  FOIA-exempt, and not public.** Plan on not having it. If it arrives as
  GFI, it changes the design; do not wait for it.
- **Public proxies, each partial**, all derivable from FPDS/SAM.gov/GAO:
  - Termination for default vs. convenience (FPDS reason-for-modification)
  - Bridge contracts — short extensions because the follow-on was not
    ready; DoDI 5000.74 makes these reportable
  - PALT — SAM.gov solicitation date to FPDS award date, per vehicle, per
    work type; the challenge's own efficiency argument rests on this
  - Re-compete migration — the next iteration of a requirement moved to a
    different vehicle
  - Sustained GAO protests citing the solicitation
  - Small-business participation before vs. after consolidation

**Design commitment:** outcome proxies are a **second, separate evidence
channel** — a warning light on a recommendation, never the label. "This
vehicle scores highest on historical practice, *and* orders of this type
on it show 2.3× the PALT and two terminations for default since FY22" is a
sentence a contracting officer can act on. Do not fold proxies into `β`;
they answer a different question and would contaminate the choice model
with survivorship effects.

**The epistemic limit, stated for the record:** "was it the right
vehicle" is a counterfactual. Nobody observes the outcome on the vehicle
not chosen. No dataset dissolves that. The honest claim is "recommends in
agreement with historical practice, and flags where that practice has
measurably underperformed" — not "recommends the optimal vehicle."

**Action:** a seventh research pass on outcome linkage — CPARS access
rules, FPDS modification/termination field semantics, GAO protest data
structure, PALT derivation — before this channel is designed. None of the
six passes run so far covered it.

## Gates before any code

Ordered. Each gates the next.

| Gate | What | Why it gates | Effort |
| --- | --- | --- | --- |
| **G1** | Premise check: pull one fiscal year of the bulk archive, count NAVAIR-office rows with non-null `parent_award_id_piid`, confirm the catalog's vehicles appear as parent PIIDs, sample the descriptions | Nobody has pulled a row. If NAVAIR-only counts are thin, or the vehicles do not appear, or descriptions are unusable, the whole design changes. Research B: "do it before building anything." | half a day |
| **G2** | Research pass 7: outcome linkage | The "was it right" channel cannot be designed without it, and it is central to positioning | one agent run |
| **G3** | Vet the 50 derived requirements (93 verdicts outstanding) | Project rule: nothing built against unvetted items. The derivation already flagged that CORE-286 (abstention) has negative expected value under the published rubric — a client decision, not an engineering one | resume `wf_fe14ec90-697` |
| **G4** | Fix the singleton-cohesion inversion in `VehicleRecommender.ComputeCohesion` | 11 of 20 reference documents are singletons scored at 1.0; the derivation found this blocks verification of the entire grey-area block (CORE-280..289) | small, but it is a scoring-requirement change |
| **G5** | Vehicle KB schema and initial curation | Step 3 of Run needs it; today it does not exist. Start from the ~15 NAVAIR/DON vehicles in contract-vehicles.md with provenance columns from day one | one to two days |
| **G6** | Accept the design into the SDD | Solutions Architect owns the SDD; Systems Engineer owns the RTVM. This document is a proposal to both | review cycle |

## Timeline

Today is 2026-09-15. Submission deadline is **2026-09-22** on the
client-directed worst-case plan, 2026-10-02 on the listing's TIMELINE
section. Final Demo Day 2026-11-09 or 2026-11-19.

**The full design above is not a seven-day build with SETR rigor.** It is
a new subsystem (knowledge base), a new fitted model, a new build
pipeline, and an extraction layer the codebase does not have. Being honest
about that is the point of writing it down.

A defensible split:

- **v1, submission:** vehicle KB hand-curated from the catalog; hard
  constraints as deterministic rules; scope matching by TF-IDF cosine
  between cluster task statements and vehicle scope text; office affinity
  as a *lookup table* of P(vehicle | office) counted from FPDS rather than
  a fitted β; the singleton fix; the evidence record. This names real
  vehicles with citable evidence and needs no fitting code in the
  container. It is what makes the output scoreable against PGIL's list.
- **v2, post-submission / Demo Day / OTA:** the conditional logit fit
  replacing the lookup table; labeling functions as features; the
  outcome-proxy channel; MRR/Recall@5 against FPDS holdouts.

Whether v1 alone is achievable by the 22nd with G1, G3, G4, G5 and G6
ahead of it is a client scheduling decision, not an engineering one.

## Open questions for the client

1. **Prediction granularity.** The rubric scores "each data point with a
   correct prediction" against a group of 20 but never defines whether a
   prediction is a vehicle name, a document-to-vehicle pairing, or a
   cluster-to-vehicle pairing. These produce different outputs and
   different scores. Worth asking Tech Grove alongside the existing date
   conflict.
2. **Abstain or guess.** CORE-286 as derived would suppress candidates
   below an evidence floor. Under a rubric with 2 points per correct
   prediction and no penalty for wrong ones, abstaining has strictly
   negative expected value. Defensibility and score point opposite ways.
   Recommended resolution: emit sub-floor candidates in a clearly
   separated "below evidence floor" section rather than suppress them.
3. **v1/v2 split.** Above.
4. **GFI access.** As of 2026-09-03 Phase 1 pre-screening had not been
   submitted. GFI vocabulary will change the knowledge base; the plan
   should assume re-tuning the day it arrives.
