# The NAADAP prize challenge, as the judges will score it

Source of truth: the official Tech Grove listing
(<a href="https://centralfloridatechgrove.org/advanced-acquisition-documentation-analysis-prize-challenge/" target="_blank">centralfloridatechgrove.org</a>),
reproduced verbatim in <a href="https://github.com/HoloSim-Interactive/NAADAP/issues/1" target="_blank">issue #1</a>.
Do not name the challenge's full title in repo content; "NAADAP" is the
internal shorthand (the repo is public).

## What the sponsor actually wants (problem statement, condensed)

NAVAIR "executes tens of thousands of procurement actions annually" and
already uses tokenization/embedding RAG. They want to "evolve beyond simple
document retrieval" to **cluster analysis** that groups procurement documents
by content and returns "a candidate list of contract vehicles for
consolidation". The algorithm must "produce candidate identification and
recommendations for strategic vehicles", including "consolidating similar
requirements into larger vehicles **or identifying new strategic contracting
vehicles** based on future capabilities."

Their own definition of the target object:

> Strategic Contract Vehicles are enterprise scale contracts that allow a
> large amount of requirement owners to procure items/commodities under the
> same base contract using streamlined procedures. While strategic vehicles
> can encompass vast requirements, they must maintain the ability to levy
> consistent and performance scope requirements to vendors.

Their motivating example: a squadron ("VX-XX") needs flight-line engineering
services and "has to establish their own contract", although many locations
buy the same service. Today "the process relies on an individual with
knowledge of existing systems, requirements, and contracting vehicles."
**That individual is the persona this skill encodes.**

### The navigation analogy (client's framing, 2026-09-15)

The client's reading of that paragraph, agreed as the positioning
statement for the algorithm documentation and Demo Day:

> Currently, experienced COs "know where to go" for requirements they've
> handled before, but COs confronted with requirements they have not
> defined or fulfilled in the past must spend time figuring out a heading
> without a clearly defined target destination. The algorithmic
> methodology will represent a common starting point from which COs will
> be able to select from a short list of reliable directions towards
> fulfilment, **and the reasons the others were ruled out.**

Three things the wording does deliberately. **The CO selects; the system
does not pick** — that is the reserved-decision boundary in
ai-boundaries.md, and it matches the sponsor's own "identify contracts
suitable to be used." **"Reliable" is earned, not asserted**: a direction
is reliable because it passed the hard-constraint gates (ordering period
open past the period of performance, office eligible, contract type
permitted, ceiling headroom) and has precedent (prior orders of this kind
on this vehicle in FPDS). The documentation should define it that way.
**The ruled-out directions are part of the product**: "SeaPort-NxG was
eliminated because its ordering period closes before your period of
performance" is the piece of tacit knowledge the novice did not have.
The eliminations record (CORE-272 in the derived set) is not a diagnostic
by-product; it is what makes "common starting point" a map rather than
an arrow.

**The experienced CO benefits too, and differently — document this,
because it is the harder sell and the stronger one.** For a requirement
they have handled before, the short list is a second opinion: confirmation
that the habitual direction still passes the gates (the ordering period
has not closed; the office is still eligible; nothing in the requirement
has drifted to a contract type the vehicle forbids) and a check that no
newer vehicle has appeared that would serve better. That alone is worth
something — the SeaPort-NxG PIEE transition and the NAWCTSD MAC awards
are exactly the kind of change a CO who "knows where to go" does not
necessarily know about. But the larger benefit arrives with the outcome
channel. FPDS records where experienced COs have gone; the outcome
proxies (terminations for default, bridge contracts, PALT, re-compete
migration) record where going there has underperformed. The tool can
tell an experienced CO that their habitual direction has, for this kind
of work, produced twice the cycle time of the alternative since FY22.
No colleague will tell them that. The record will. That is the
experienced CO's reason to use a tool that looks as if it was built for
the novice — and it is the same evidence the follow-on OTA narrative
rests on. See the "was it right" section of
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank">docs/design/vehicle-recommendation-pipeline.md</a>.

## Benefits (the sponsor's own words, and what they cite)

> The primary benefits include increased buying power, reduction of
> duplication significant process efficiencies, speed of acquisition, and
> cost savings that directly support DoD acquisition reform efforts.

The original omits a comma or conjunction between "duplication" and
"significant"; this skill reads it as two items. Note the direction of
the last clause: the benefits *support* reform — they do not define it.
"DoD acquisition reform efforts" in 2026 is a specific, citable set of
instruments, and this sentence is the sponsor naming what the algorithm
documentation should cite:

| Benefit | FAR 7.107 category | Reform instrument | Quantifiable from public data? |
| --- | --- | --- | --- |
| Increased buying power | Cost savings / price reduction (the *mechanism*) | OMB M-19-13 / M-22-03 category management; Better Buying Power lineage | Mostly no — needs pricing; CALC+ labor-rate ceilings are the only public hook |
| Reduced duplication | Administrative cost savings (7.107-3's ≥10% test) | EO 14240 / OMB M-25-31 consolidation mandate | Yes — count of separate current contracts collapsed |
| Process efficiency | Better terms; approvals avoided (7.107-1 inheritance, 5.202(a)(6) no synopsis, no new J&A) | OMB M-25-26 "Revolutionary FAR Overhaul"; DoDI 5000.74 Seven Steps | Partly — approvals avoided are countable |
| Speed of acquisition | Reduced acquisition cycle time | DoDI 5000.74 Step 3 market research; NAVAIR's own 18–24-month → 4-month MAC rationale | Yes — PALT from SAM.gov solicitation date to FPDS award date |
| Cost savings | Cost savings (the *outcome*) | All of the above | Partly — contract count, bridge avoidance, PALT-derived staff time |

Buying power and cost savings are related but distinct (mechanism vs.
outcome); process efficiency and speed are related but distinct (fewer
steps vs. fewer days). The sponsor lists all five separately; keep them
separate in any output record. The vehicle-definition paragraph adds a
sixth, "maximize contract ceilings," which is a DoD portfolio-management
goal rather than a FAR category: knowledge-base headroom feeds both a
benefit (utilization) and a hard constraint (sufficient headroom), and
those are different checks.

## Scoring rubric (exact)

**Gate before any scoring:** *"Only complete submissions that satisfy all
submission requirements will be evaluated."* An incomplete package is not
scored at all — every one of the twelve Phase 2 deliverables (below) must
be present. This is not a point deduction; it is a zero.

| Component | Points | Rule |
| --- | --- | --- |
| Runtime | 15 | "Users will be presented with a dataset of **additional** documents. The documents must be processed and candidates for recommendation must be provided in 30 minutes. This is a time requirement, not a validation requirement. **Validation will be performed live by the candidate on demo day.**" 0 if it does not run in 30 minutes. |
| Replicability | 10 | "The solution can be distributed across multiple containers without affecting the results. **Replication must demonstrably improve performance.**" 0 "if container cannot be replicated without affecting results." See the scoring risk noted below. |
| Compute cost | 15 / 10 / 5 / 0 | 1 core + 2 GB / 4 c + 8 GB / 8 c + 16 GB / more |
| LLM cost | 10 / 5 / 0 | No LLM / LLM under 50k tokens per retrieval / more |
| Initial technical evaluation | 40 | "Each data point in the dataset with a correct prediction will be granted two percentage points. Correctness is defined as a result being within a group of 20 results predetermined as correct by the **Procurement Group Innovation Lab (PGIL)** on the validation set." |
| Live Demo Day | 10 | "During the live demo day, the researcher will be given a **validation set similar to the test set** to test their solution against. Solutions will be evaluated... by their ability to identify five manually identified candidates from the validation set." |

The validation half is split by who performs it: "Validation will be
performed in the initial evaluation by the government technical team, and
the remaining validation will be performed during the live demo day."

Reading the validation half as an acquisition professional:

- **20 data points, 2 points each.** PGIL has a list of 20 correct answers.
  A "prediction" is judged by membership in that list. The list is almost
  certainly expressed in acquisition vocabulary (vehicle names, contract
  numbers, PSC/functional areas), not in cluster IDs or keyword slugs.
- **"Five manually identified candidates"** on Demo Day means five specific
  documents or requirement groups a human SME flagged as consolidation
  candidates. The tool must surface *those*, live, in front of the people
  who picked them.
- The rubric never rewards cluster purity or a precision@k the entrant
  defined. It rewards agreement with human procurement judgment. Design
  metrics that proxy *that*.
- **There are at least three distinct document sets, and the solution
  never trains on the ones it is scored on.** GFI (development, after
  Phase 1 approval); the initial-evaluation "dataset of additional
  documents" (40 points, government team); and the Demo Day "validation
  set similar to the test set" handed over live (10 points). "Additional"
  and "similar to" both say the scored sets are unseen. Anything tuned to
  the GFI corpus specifically — vocabulary, thresholds, the 0.35 cutoff —
  has to generalize, and the validation methodology should hold out
  documents the same way.
- **The Demo Day run happens inside the presentation window.** The
  robustness text says the 30-minute processing test is "performed live
  by the candidate on demo day," and the Phase 3 text says presentation,
  live demo, and Q&A "must be completed within 30 minutes." Read literally,
  a pipeline that uses its full 30-minute allowance consumes the entire
  presentation. CORE-220's 30-minute ceiling is the *scoring* bar; the
  *practical* bar for Demo Day is a run measured in minutes, on a fresh
  set, with the presenter narrating over it. Tech Grove question 4 asks
  whether the timed run is separate from the presentation; design as if
  it is not.
- **Scoring risk on Replicability — flag for the Systems Engineer and
  Solutions Architect.** The rubric awards the 10 points only if
  "replication must demonstrably improve performance." NFR-520 was resolved
  in the SDD as *independent, fully stateless full-replica runs* — N
  containers each doing the whole job and each reproducing the same top 5.
  That satisfies "without affecting the results." It does not on its face
  *demonstrably improve performance*: N replicas processing one document
  set finish no faster than one. The defensible reading is **throughput** —
  N replicas process N document sets in the time one processes one — and
  TP-520 should be extended to demonstrate exactly that, with a wall-clock
  measurement, or the interpretation revisited. As written, the 10 points
  are at risk on a literal reading.

## Critical technical criteria (verbatim, condensed)

- Deploy in a U.S. Government-owned/operated cloud accredited for **IL4**.
- "The core analysis and identification methodology must ensure repeatable
  results (produces same top 5 results 95% of the time). The final
  summarization component (e.g., creating a common visualization) may be
  stochastic and leverage a Large Language Model (LLM)."
- Database schema and ETL documentation if a database is used.
- Visual representation of the analysis method **and** of the results.
- A summary metric for algorithm performance.
- Docker container with all dependencies; "cannot access external services
  outside of USN-approved models or microservices within an IL4 environment
  (i.e., no external industry-hosted custom models)."

## Phases, dates, and the date conflict

The listing's summary box and its TIMELINE section disagree. The client
directed planning against the earlier set until Tech Grove answers.

| Milestone | Summary box | TIMELINE section |
| --- | --- | --- |
| Launch | 30 Jul 2026 | 30 Jul 2026 |
| Submission deadline | **22 Sep 2026** | 2 Oct 2026 |
| Semi-finalist selection / notification | (none) | 26 Oct 2026 |
| Demo Day materials due | (none) | 12 Nov 2026 |
| Final Demo Day | **9 Nov 2026** | 19 Nov 2026 |

Phase 1 is a Pre-Screening Questionnaire (eligibility, team, cybersecurity,
GFI access). Only approved participants receive **Government Furnished
Information (GFI)** and the submission portal. **Status 2026-09-15: the
client is submitting Phase 1 as quickly as possible, targeting completion
within five days.** Until approval, all development is against public
SAM.gov documents. When GFI arrives, expect NAVAIR-internal documents whose
vocabulary (PMA numbers, DoDAACs, NAWCAD department codes, vehicle names)
matters for vehicle matching.

**GFI timing risk.** Phase 2 access is gated on Phase 1 approval, and
approval takes an unknown time after submission. Against the 22 Sep
worst-case deadline that leaves zero or negative days between GFI access
and submission; against 2 Oct, perhaps a week. The earlier plan to "re-tune
against GFI immediately" cannot be a Phase 2 dependency. The Phase 2
package must score on public documents; GFI re-tuning is Phase 3 work. The
challenge's own wording supports that split — Phase 2 is the "**initial**
technical package," semifinalists advance, and Demo Day materials are due
in November.

**Phase 2 submission package — twelve items, all required** (the
completeness gate above makes any omission a zero, not a deduction):
1. Algorithm documentation
2. Complete codebase
3. Docker container
4. Code packages and deployment instructions
5. Database schema, if a database is used
6. ETL process documentation, if applicable
7. Visual representation of the analysis method
8. Visual representation of the results
9. Algorithm performance summary metrics
10. Description of validation methodology
11. Documentation of external dependencies
12. Required technical and supporting documentation

Plus three constraints on the package as a whole: code "fully
reproducible" (runtime — CORE-210 — and, once the build-time fit exists,
build reproducibility — need 6); "utilize Docker"; "capable of operating
in an IL4 environment"; and "must NOT be a link to a website." Items 5
and 6 are conditional and were withdrawn as not applicable; the vehicle
knowledge base and its FPDS build pipeline reopen them narrowly (design
doc, G3 amendments). A committed reference-run output bundle should
accompany items 7–9 so an evaluator sees them without building first.

Phase 3: a PowerPoint "included and available to judges," and a
presentation "including live demo, Questions/Answers" completed within
30 minutes. See the Demo Day timing note under the rubric.

## Questions for Tech Grove

Open as of 2026-09-15. Each changes the design or the scoring strategy
depending on the answer; none should be guessed at.

1. **The date conflict** (above). Which set governs — the summary box
   (22 Sep / 9 Nov) or the TIMELINE section (2 Oct / 19 Nov)?
2. **Prediction granularity.** The rubric awards two points per "data
   point in the dataset with a correct prediction," judged against a
   group of 20 PGIL predetermined. It never defines what one prediction
   is: a vehicle name, a document-to-vehicle pairing, or a
   cluster-to-vehicle pairing. These produce materially different outputs
   and different scores. Related: do "new strategic vehicle indicated"
   entries count as predictions, given the list of 20 is of vehicles?
3. **"Recommending common requirements."** The Problem Statement's first
   sentence asks for "evaluating, identifying, and recommending common
   requirements." Two readings: (a) recommend *that* a set of requirements
   be treated as common — the cluster with its consolidation evidence; or
   (b) recommend *a* common requirement — draft the consolidated PWS
   language the merged requirement would carry. (a) is captured. (b) is
   a significant addition and sits on the reserved-decision boundary (the
   tool may draft a candidate for human review; it may not originate the
   requirement). Which is intended? Related: the closing sentence, "identify
   contracts suitable to be used *as* strategic vehicles" — is that the
   ordinary routing case, or a third output mode identifying existing
   contracts that could be promoted to strategic-vehicle status?
4. **Is the 30-minute processing test separate from the 30-minute
   presentation?** The robustness criterion says the timed run is
   "performed live by the candidate on demo day"; the Phase 3 criterion
   says presentation, live demo, and Q&A complete within 30 minutes. If
   those are the same 30 minutes, the effective runtime budget is a few
   minutes, not thirty. Also: does "replication must demonstrably improve
   performance" mean throughput across document sets, or latency on one?

## Rules that constrain behavior

- Participants must be U.S. citizens, 18+; no federal employees or support
  service contractors; no federal funds.
- Submissions are federal agency records subject to FOIA; protective
  markings are permitted but not dispositive.
- Government and partners get permanent access to submitted materials;
  participants retain IP and publishing rights.
- Follow-on may come via FAR contracts, OTA (10 USC 4021/4022), prizes
  (10 USC 4025, 15 USC 3719). A win is also a pathway to an OTA prototype.
