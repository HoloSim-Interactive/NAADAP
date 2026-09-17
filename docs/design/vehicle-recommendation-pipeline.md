# <a id="vrp-title"></a>Vehicle recommendation: build-time fit, ship-frozen pipeline

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

## <a id="vrp-what-this-replaces"></a>What this replaces

Today `Naadap.Output/VehicleRecommender.cs` ranks clusters by an internal
cohesion score and labels them with top TF-IDF terms. There is no vehicle
knowledge base anywhere in the codebase, so the pipeline cannot name a real
vehicle; it can only emit a well-formed ranked list of things it has no
principled way to identify. The judges score membership in PGIL's list of
20 correct vehicle candidates. A cluster labeled "aircraft, maintenance,
support" is not a candidate. This design closes that gap.

## <a id="vrp-the-pipeline"></a>The pipeline

### <a id="vrp-build-offline-outside-the-container-once-per-release"></a>Build (offline, outside the container, once per release)

1. **Pull the historical record.** USAspending bulk archive,
   `https://files.usaspending.gov/award_data_archive/`, split by top-tier
   agency: `FY{yyyy}_097_Contracts_Full_{yyyymmdd}.zip` is DoD (Navy is
   inside it, sub-agency 1700). FY2025 is 1.04 GB compressed, ~8.6 GB
   across five CSVs, 297 columns; locate with `?prefix=FY2025_097`
   because the bucket listing truncates. No key, no rate limit, fully
   offline once downloaded; stream the zip members rather than extract.
   Do not use the FPDS ATOM feed — it retires in FY2026. (Corrected
   2026-09-15 from a live pull; the earlier "one `_All_` file per FY"
   description was stale.)
2. **Filter to orders under vehicles.** Keep rows where
   `awarding_sub_agency_code == 1700`, `parent_award_id_piid` is non-null
   (the Referenced IDV PIID — this is the label), and
   `awarding_office_code` is in the NAVAIR set: N00019, N00421, N68335,
   N61340, N68936, N68520. Rows are transactions, so dedupe on
   `award_id_piid` to count orders. Widen to all DON offices for a
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

### <a id="vrp-ship"></a>Ship

The container carries only frozen artifacts: `β`, vocabulary, knowledge
base, LF registry, manifest. No optimizer, no fitting code, no network
client. Every file's hash is recorded and checked at startup.

### <a id="vrp-run-inside-the-container-per-document-set"></a>Run (inside the container, per document set)

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

## <a id="vrp-decisions-and-their-basis"></a>Decisions and their basis

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

## <a id="vrp-the-was-it-right-channel"></a>The "was it right" channel

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

### <a id="vrp-who-this-channel-serves"></a>Who this channel serves

The routing recommendation serves the CO who has never handled this
requirement — a common starting point, a short list of directions that
passed the gates and have precedent, and the reasons the others were
ruled out. **The outcome channel is what serves the experienced CO**, and
that is the harder and stronger case to make. An experienced CO already
knows where to go. What they cannot know from experience is that where
they habitually go has, for this kind of work, produced twice the PALT
or two terminations for default since FY22 — because no colleague will
say so and the CO's own successes are what they remember. The outcome
proxies say so from the record. That is the experienced CO's reason to
use a tool that looks as if it was built for the novice, and it is why
this channel carries the follow-on OTA narrative rather than the routing
channel. The client's full navigation analogy, with both audiences, is
recorded in
<a href="../../.claude/skills/gov-acquisition-sme/references/challenge-brief.md">challenge-brief.md</a>.

## <a id="vrp-gates-before-any-code"></a>Gates before any code

Ordered. Each gates the next.

| Gate | What | Why it gates | Effort |
| --- | --- | --- | --- |
| <a id="vrp-g1"></a>**G1** | Premise check: pull one fiscal year of the bulk archive, count NAVAIR-office rows with non-null `parent_award_id_piid`, confirm the catalog's vehicles appear as parent PIIDs, sample the descriptions | Nobody has pulled a row. If NAVAIR-only counts are thin, or the vehicles do not appear, or descriptions are unusable, the whole design changes. Research B: "do it before building anything." | half a day |
| <a id="vrp-g2"></a>**G2** | Research pass 7: outcome linkage | The "was it right" channel cannot be designed without it, and it is central to positioning | one agent run |
| <a id="vrp-g3"></a>**G3** | Vet the 50 derived requirements (93 verdicts outstanding) | Project rule: nothing built against unvetted items. The derivation already flagged that CORE-286 (abstention) has negative expected value under the published rubric — a client decision, not an engineering one | resume `wf_fe14ec90-697` |
| <a id="vrp-g4"></a>**G4** ✅ 2026-09-17 | Fix the singleton-cohesion inversion in `VehicleRecommender.ComputeCohesion` (done: singletons score 0.0; core precision@5 unchanged at 0.60; alternative rose 0.40 → 0.80, see `docs/ALGORITHM_COMPARISON.md`) | 11 of 20 reference documents are singletons scored at 1.0; the derivation found this blocks verification of the entire grey-area block (CORE-280..289) | small, but it is a scoring-requirement change |
| <a id="vrp-g5"></a>**G5** ✅ 2026-09-17 | Vehicle KB schema and initial curation (done: `scripts/kb/`, `docs/KB_SCHEMA.md`, 484 rows, 87.3% coverage; matcher and evidence record in `Naadap.Output`) | Step 3 of Run needs it; today it does not exist. Start from the ~15 NAVAIR/DON vehicles in contract-vehicles.md with provenance columns from day one | one to two days |
| <a id="vrp-g6"></a>**G6** ✅ 2026-09-17 | Accept the design into the SDD (done: SDD block diagram and data architecture amended; fitted β stays Increment 2, lookup table shipped in v1) | Solutions Architect owns the SDD; Systems Engineer owns the RTVM. This document is a proposal to both | review cycle |

### <a id="vrp-rtvm-amendments-riding-with-g3"></a>RTVM amendments riding with G3

Agreed with the client on 2026-09-15 from the Critical Technical Criteria
and Benefits review; deliberately **not** hand-edited into Verified rows
ahead of the vet pass. They go to the Systems Engineer with the vetted
requirements.

| Item | Amendment | Why |
| --- | --- | --- |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a> (Withdrawn) | Reopen **narrowly**: document the vehicle knowledge base's schema and the build-time FPDS → KB pipeline as schema-and-ETL documentation, even though no database is used | The no-database decision predates the KB. A versioned, schema-validated table fed by an extract-transform-load pipeline is what an evaluator who wrote "ETL" into the criteria will recognize. Cheap insurance; removes a Demo Day question. |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> (Verified) | Change the named metric from "precision@5 or F1 against validation ground truth" to **MRR and Recall@5**, labeled **"agreement with historical practice"** | One correct vehicle per requirement, no graded relevance, so NDCG/F1 machinery is inert; and a confounded model scores *better* on held-out accuracy by learning office habit, so "accuracy" overclaims. See `docs/research/km-capture/C-learning-from-decisions.md`. |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-920" target="_blank">DELIV-920</a> (Verified) | Add a **license** column to the dependency-justification table and make license compatibility with permanent Government access part of the justification | Packaging a dependency in Docker does nothing about its license. The research found vendorable-but-unusable tools (non-commercial, patent-encumbered, GPL-3.0). "Fits in the container" is necessary, not sufficient. |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-970" target="_blank">DELIV-970</a> (Verified) | The algorithm documentation traces each claimed benefit to a named DoD acquisition-reform instrument by memo or instruction number (table in `challenge-brief.md`, Benefits section) | The sponsor wrote "directly support DoD acquisition reform efforts." An evaluator who wrote that will look for the citations. Verification: inspection — every benefit claim cites at least one instrument. |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a> / TP-520 (Verified) | Extend TP-520 to **demonstrate improved performance** under replication — N replicas processing N document sets with a wall-clock measurement showing throughput scales — or revisit the independent-full-replica interpretation | The rubric grants the 10 Replicability points only if "replication must demonstrably improve performance." The SDD's interpretation (independent stateless replicas reproducing the same top 5) satisfies "without affecting results" but does not, on a literal reading, improve anything. Throughput is the defensible reading and it has to be shown, not asserted. Flagged to the Solutions Architect. |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> (Approved) | No text change; add a **design target** note that the practical Demo Day budget is minutes, not thirty, because the timed run may occur inside the 30-minute presentation | The rubric's 30-minute ceiling is the scoring bar. The Phase 3 text puts the live run inside a 30-minute presentation with Q&A. Tech Grove question 4 asks; design as if the answer is "same window." |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-960" target="_blank">DELIV-960</a> (Verified) | Replace the NAVAIRINST 4355.19D citation with 4355.19E and point the requirement at the SEMP's tailored review sequence (`docs/setr/SEMP.md` §3.2.13) | 19D was superseded by 19E in 2015; the E revision adds RBR and FCA and the Handbook's §6.4 tailoring the program follows. The SDD's mapping table carries a superseding note; the requirement text should say what the program actually reconciles against. |

## <a id="vrp-timeline"></a>Timeline

Today is 2026-09-15. Submission deadline is **2026-09-22** on the
client-directed worst-case plan, 2026-10-02 on the listing's TIMELINE
section. Final Demo Day 2026-11-09 or 2026-11-19.

**The full design above is not a seven-day build with SETR rigor.** It is
a new subsystem (knowledge base), a new fitted model, a new build
pipeline, and an extraction layer the codebase does not have. Being honest
about that is the point of writing it down.

**Decided by the client, 2026-09-15 — a two-version plan.**

| | Version 1 | Version 2 |
| --- | --- | --- |
| **Deliverable** | Phase 2 "initial technical package," due 22 Sep (worst case) | Phase 3 Demo Day materials, due 12 Nov; the follow-on OTA basis |
| **Purpose** | Illustrates the strategy and demonstrates the technical capability | The strategy realized on the sponsor's own data |
| **Trigger** | Phase 1 approval (GFI and portal access) | Semifinalist selection, 26 Oct |
| **Data** | Public only: SAM.gov documents, FPDS bulk archive, hand-curated KB | GFI for the *document* side; FPDS for the *label* side |
| **Content** | Vehicle KB hand-curated from the catalog; hard constraints as deterministic rules; scope matching by TF-IDF cosine against vehicle scope text; office affinity as a P(vehicle \| office) **lookup table** counted from FPDS; the singleton fix; the full evidence record including eliminations | Conditional logit β fitted on FPDS replacing the lookup table; labeling functions as validated features; extraction and thresholds re-tuned on GFI; the outcome-proxy "was it right" channel; MRR/Recall@5 against FPDS holdouts |
| **What the algorithm documentation says** | Describes v2 as the roadmap — the fitted model, why it is fit offline, what GFI enables — so the strategy is visible before it is built | Describes what was measured |

Two clarifications the plan depends on:

- **β is fit on FPDS, not on GFI.** The label — which vehicle absorbed
  which requirement — is public and available now. GFI tunes the
  document side: vocabulary, section conventions, PMA and DoDAAC usage,
  the classifier, the similarity threshold. Consequence: G1 is unblocked
  today, and if the premise check goes well the fitted β can move into
  v1. The lookup table is the fallback, not the plan.
- **GFI arrives with Phase 1 approval, before Phase 2 is due** — but
  possibly with no usable days before the deadline. v1 must score on
  public documents. By the v2 deadline GFI will have been in hand for
  weeks; that is where re-tuning belongs.

Whether v1 as scoped lands by the 22nd with G1, G4, G5 and G6 ahead of
it remains the open scheduling question; G3 (vetting) and G2 (outcome
research) are v2 gates and need not block v1.

## <a id="vrp-open-questions-for-the-client"></a>Open questions for the client

1. ~~**Prediction granularity.**~~ Decided 2026-09-17 (client): any of the three forms scores; new-vehicle entries count when expected. Keep all views. Original text: The rubric scores "each data point with a
   correct prediction" against a group of 20 but never defines whether a
   prediction is a vehicle name, a document-to-vehicle pairing, or a
   cluster-to-vehicle pairing. These produce different outputs and
   different scores. Worth asking Tech Grove alongside the existing date
   conflict.
2. ~~**Abstain or guess.**~~ Decided 2026-09-17 (client): record sub-floor candidates in a separate "below evidence floor" section; never suppress. Re-derive CORE-286 at G3 on that basis. Original text: CORE-286 as derived would suppress candidates
   below an evidence floor. Under a rubric with 2 points per correct
   prediction and no penalty for wrong ones, abstaining has strictly
   negative expected value. Defensibility and score point opposite ways.
   Recommended resolution: emit sub-floor candidates in a clearly
   separated "below evidence floor" section rather than suppress them.
3. ~~**v1/v2 split.**~~ Decided 2026-09-15 — see Timeline.
4. **GFI access.** Phase 1 questionnaire in submission as of 2026-09-17. As of 2026-09-03 Phase 1 pre-screening had not been
   submitted. GFI vocabulary will change the knowledge base; the plan
   should assume re-tuning the day it arrives.
