---
name: naadap-implementation-plan
description: NAADAP's Implementation Plan (issue #4) build sequence and the 8 downstream issues (#5-#12) it created — single linear order, one issue per pipeline stage.
metadata:
  type: project
---

Decisions made in the Implementation Plan issue (#4, 2026-09-03):

- **Single linear build order**, no 3-axis phase table — this is one
  MVP with a hard 2026-09-22 deadline, not a multi-phase product. See
  [[naadap-sdd-decisions]] for the architecture this sequence rests on.
- **Issues created, in dependency order:** #5 Generate Code Base (no
  deps) → #6 Validation Corpus & Test Fixtures (FS #5) → #7 [RTVM-100]
  Ingestion (FS #5, SS #6) → #8 [RTVM-200] Core Clustering (FS #7, #6)
  → #9 [RTVM-300] Ranking & Output Bundle incl. UI-001 (FS #8) → #10
  [RTVM-250] Optional LLM step + CORE-260 alt-comparison (FS #9) → #11
  [RTVM-500] NFR Docker/network/replicability/resource (FS #10) → #12
  [RTVM-900] Deliverable docs + one-time Windows/VS consolidation
  check (FS #11).
- **Grouping rationale:** one issue per SDD block-diagram component
  (Ingestion / Core / Output / Alt+LlmStep / NFR / Deliverable-docs)
  rather than one issue per individual RTVM ID — the pipeline is
  strictly sequential stage-to-stage (each stage's output is the
  next's input), so finer splitting inside a stage wouldn't add real
  concurrency, only serial hops. If a future project's stages *can*
  progress independently, don't default to this coarse a grouping —
  it specifically fit NAADAP's single-process, no-fan-out pipeline
  shape.
- **UI-001 deliberately verified in the Output issue (#9), not
  Generate Code Base (#5)** — the "runs end-to-end with no prompts"
  requirement is only meaningfully testable once every stage exists;
  #5 only scaffolds CLI arg parsing.
- **Validation Corpus (#6) is its own issue, not folded into Ingestion
  or Core** — Product Manager's SAM.gov corpus recommendation needed
  an explicit owner and ground-truth-derivation deliverable of its
  own; several test procedures (TP-100/110/200/210/220/230/420/260)
  depend on it. Gave Ingestion (#7) a Start-Start (not Finish-Start) on
  it since Ingestion only needs the small smoke subset, not the full
  N=20 set, to start building.
- **Sequencing wasn't blocked on live Solutions Architect/Product
  Manager replies** — no comments from either appeared on the SDD
  issue (#3) by the time this ran. Proceeded on the self-handoff's
  already-reasoned order (feasibility: Core is highest-risk, sequence
  early; value: non-LLM core + output ahead of optional LLM step,
  matching the client's explicit "LLM step is lower-risk logic, not a
  shortcut" instruction in `docs/PROJECT_DEFINITION.md`) and flagged
  it for after-the-fact review in the closing comment, same pattern as
  NFR-520's "flag, don't block" resolution in the SDD issue.
