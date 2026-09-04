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

## Scoring rubric (exact)

| Component | Points | Rule |
| --- | --- | --- |
| Runtime | 15 | Process a new document set and return candidates within 30 minutes; 0 otherwise. "A time requirement, not a validation requirement." |
| Replicability | 10 | Solution "can be distributed across multiple containers without affecting the results. Replication must demonstrably improve performance." |
| Compute cost | 15 / 10 / 5 / 0 | 1 core + 2 GB / 4 c + 8 GB / 8 c + 16 GB / more |
| LLM cost | 10 / 5 / 0 | No LLM / LLM under 50k tokens per retrieval / more |
| Initial technical evaluation | 40 | "Each data point in the dataset with a correct prediction will be granted two percentage points. Correctness is defined as a result being within a group of 20 results predetermined as correct by the **Procurement Group Innovation Lab (PGIL)** on the validation set." |
| Live Demo Day | 10 | "Ability to identify five manually identified candidates from the validation set." |

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
Information (GFI)** and the submission portal. As of 2026-09-03 HoloSim had
not submitted Phase 1 and had no GFI, so all development is against public
SAM.gov documents. When GFI arrives, expect NAVAIR-internal documents whose
vocabulary (PMA numbers, DoDAACs, NAWCAD department codes, vehicle names)
matters for vehicle matching. Plan to re-tune against GFI immediately.

Phase 2 submission package (all required): algorithm documentation,
complete codebase, Docker container, packages and deployment instructions,
database schema and ETL docs if applicable, method visualization, results
visualization, performance summary metrics, validation methodology,
external-dependency documentation. "Must not consist solely of a link to a
website." Phase 3: PowerPoint, live demo, Q&A, all within 30 minutes.

## Rules that constrain behavior

- Participants must be U.S. citizens, 18+; no federal employees or support
  service contractors; no federal funds.
- Submissions are federal agency records subject to FOIA; protective
  markings are permitted but not dispositive.
- Government and partners get permanent access to submitted materials;
  participants retain IP and publishing rights.
- Follow-on may come via FAR contracts, OTA (10 USC 4021/4022), prizes
  (10 USC 4025, 15 USC 3719). A win is also a pathway to an OTA prototype.
