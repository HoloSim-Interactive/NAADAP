---
name: naadap-sdd-decisions
description: Key architecture decisions made in NAADAP's SDD issue (#3, 2026-09-03) — no DB, NFR-520 semantics, target-platform verification timing, no UI/ICD. Reference before writing IMPLEMENTATION_PLAN.md or answering downstream architecture queries.
metadata:
  type: project
---

Decisions recorded in `docs/SDD.md` during the SDD issue (#3,
2026-09-03), carried forward from the RTVM issue's open items:

- **No database.** All output is file-based (OUT-440 manifest bundle).
  DATA-OUT-310 and DELIV-950 withdrawn in `docs/RTVM.md` as a result —
  don't resurrect a DB-schema doc expectation for this project.
- **NFR-520 (horizontal replicability) resolved as interpretation (b):
  independent, fully stateless full-replica runs**, not (a) sharded
  partitioning of one logical run. Rationale: sharding scope is
  explicitly excluded from MVP and there's no throughput problem to
  solve; the system already has zero shared mutable state, so N
  independent replicas trivially satisfy it as a corollary of
  CORE-210's determinism guarantee. Flagged to Solutions Architect for
  review but not blocked on their reply — revisit if they push back
  before implementation starts.
- **Target-platform verification (Windows/VS, DELIV-910/TP-910) gates
  once, at a consolidation phase, not per feature.** Every
  `[RTVM-014]`-style issue builds/tests on Ubuntu only. Don't add a
  per-feature Windows/VS check task to any feature issue in the
  Implementation Plan — plan one consolidation issue near the end
  instead.
- **No UI Designer track.** SN-5/Scope confirmed batch analysis +
  report deliverable, no interactive UI — so the Implementation Plan
  should not create a parallel `agent:ui-designer` issue for this
  project, and no ICD was written (only one actor, no second system to
  build against).
- **Assembly separation is load-bearing for CORE-240's inspection.**
  `Naadap.Core` (and everything it references) must contain zero
  LLM/HTTP-client dependencies; `Naadap.Alternative` (CORE-260) and
  `Naadap.LlmStep` (CORE-250) are separate projects that Core never
  references. Software Engineer should scaffold this separation in the
  Generate Code Base issue, not retrofit it later.
- **SETR mapping (DELIV-960)** is done in `docs/SDD.md`'s "SETR
  Documentation Mapping" table — PDR/CDR are treated as *documents*
  (this repo's SDD/Implementation Plan), other reviews as *events*
  (issue hand-offs). "PEDDAL" is still unresolved — see
  [[naadap-setr-reference]] — don't guess at it if it resurfaces.
