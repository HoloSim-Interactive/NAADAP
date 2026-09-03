# Implementation Plan

<!--
Owned by the Systems Engineer, built in collaboration with Solutions
Architect's docs/PROJECT_DEFINITION.md. Sequences the build so the
most critical MVP items come first.
-->

## Why a single linear order

`docs/PROJECT_DEFINITION.md` describes one MVP with a hard submission
deadline (2026-09-22), not a product expected to grow through
distinct phases with independently varying complexity/UI/documentation
rigor — the three-axis phase table in the template below does not
apply here and has been omitted. The pipeline itself is also
inherently sequential (each stage's output is the next stage's input —
see `docs/SDD.md`'s Data Architecture run-scoped data flow), so a
single priority-ordered sequence is both the simplest plan and an
accurate reflection of the real dependency graph, not a
simplification of it.

There is also no UI Designer or Scene Developer track: `docs/SDD.md`'s
"Why no use case diagram / ICD" section confirms this is a batch
analysis + report deliverable with one actor (an operator or CI job
invoking the container). Every downstream issue's owner is Software
Engineer.

## Build Sequence

1. **Generate Code Base** (#5) — scaffolds the `Naadap.sln` layout from
   `docs/SDD.md` Coding Standards, with the Core/Alternative/LlmStep
   assembly separation in place from the start (load-bearing for
   CORE-240's inspection). No dependencies; everything else gates on
   this.
2. **Validation Corpus & Test Fixtures** (#6) — pulls the Product
   Manager's SAM.gov recommendation into a concrete, checked-in
   fixture set (smoke subset, corrupted-file case, synthetic CORE-200
   set, and the full N=20 reference set with derived ground truth).
   Not an RTVM item itself, but a prerequisite several test procedures
   need — called out explicitly here rather than assembled ad hoc
   inside a feature issue. Runs largely in parallel with Ingestion.
3. **DATA-IN-100/110/120 — Ingestion & Normalization** (#7) — needed
   before anything downstream has real records to work with. Can start
   once fixture curation is underway (Start-Start on #6); only needs
   the full N=20 set once tests run.
4. **CORE-200/210/220/230/240 — Core Clustering Engine** (#8) — the
   heart of the algorithm and the biggest technical-risk item;
   sequenced as early as its real prerequisites (Ingestion's output,
   the completed reference set) allow.
5. **UI-001, DATA-OUT-300, OUT-400/410/420/430/440 — Ranking &
   Output Bundle** (#9) — depends on Core producing real cluster
   output; this is also where the pipeline first runs genuinely
   end-to-end, so UI-001 is verified here rather than at scaffolding
   time.
6. **CORE-250/260 — Optional LLM Step & Alternative-Approach
   Comparison** (#10) — deliberately sequenced *after* the non-LLM
   core and its output/metric machinery are stable, since CORE-260's
   purpose is comparing against the already-working core and CORE-250
   is optional/non-core.
7. **NFR-500/510/520/530 — Docker, Network Isolation, Replicability,
   Resource Ceiling** (#11) — verified once the pipeline exists
   end-to-end including the optional LLM step's allowlist config.
   NFR-520 is expected to require no new code (a corollary of
   CORE-210's determinism plus the system's lack of shared mutable
   state, per `docs/SDD.md`) — this issue is largely a verification
   pass, not a build task.
8. **DELIV-900/910/920/930/940/960/970 — Deliverable Documentation &
   Windows/VS Consolidation** (#12) — last in the plan. DELIV-910's
   Visual Studio/Windows check runs exactly once here, per
   `docs/SDD.md`'s explicit target-platform-verification decision,
   immediately before the 2026-09-22 submission — not gated per
   feature. DELIV-960's SETR mapping and DELIV-970's document split are
   largely already satisfied by the pipeline's own docs; this issue
   confirms rather than rewrites them.

DATA-OUT-310 and DELIV-950 are Withdrawn (no database — see
`docs/SDD.md` Data Architecture) and appear in no issue above.

**Priority rationale:** Solutions Architect's feasibility read places
Core Clustering (step 4) as the highest technical risk, so it is
sequenced as early as Ingestion's real output dependency allows, ahead
of anything optional. Product Manager's client-value read places the
non-LLM core and its output/validation artifacts (steps 4–5) ahead of
the optional LLM step and alternative-approach comparison (step 6),
matching the client's explicit instruction that the LLM step is
"lower-risk logic," not a shortcut around the clustering work, and
that CORE-260 exists to substantiate — after the fact — a decision
already made in favor of the non-LLM core. Both inputs agree on this
order; **flagged to Solutions Architect and Product Manager for
after-the-fact review** rather than blocked on it beforehand, since
neither the technical-dependency chain nor the value ordering was in
any tension once laid out — see the closing comment on this issue for
the explicit flag.

## Sequence Diagram

```mermaid
graph TD
    A["#5 Generate Code Base"] --> B["#6 Validation Corpus & Test Fixtures"]
    A --> C["#7 DATA-IN-100/110/120\nIngestion & Normalization"]
    B -. Start-Start .-> C
    C --> D["#8 CORE-200/210/220/230/240\nCore Clustering Engine"]
    B --> D
    D --> E["#9 UI-001, DATA-OUT-300, OUT-400/410/420/430/440\nRanking & Output Bundle"]
    E --> F["#10 CORE-250/260\nOptional LLM Step & Alternative-Approach Comparison"]
    F --> G["#11 NFR-500/510/520/530\nDocker / Network / Replicability / Resource Ceiling"]
    G --> H["#12 DELIV-900/910/920/930/940/960/970\nDeliverable Docs & Windows/VS Consolidation"]
```

## Downstream issues created

| Issue | Title | RTVM IDs | Owner | Dependencies |
| --- | --- | --- | --- | --- |
| #5 | Generate Code Base | — | Software Engineer | none |
| #6 | Validation Corpus & Test Fixtures | — (supports TP-100/110/200/210/220/230/420/260) | Software Engineer | Finish-Start: #5 |
| #7 | [RTVM-100] Document ingestion and normalization | DATA-IN-100/110/120 | Software Engineer | Finish-Start: #5; Start-Start: #6 |
| #8 | [RTVM-200] Core clustering engine, reproducibility, and performance | CORE-200/210/220/230/240 | Software Engineer | Finish-Start: #7, #6 |
| #9 | [RTVM-300] Ranking, visualization, metrics, and output bundle | UI-001, DATA-OUT-300, OUT-400/410/420/430/440 | Software Engineer | Finish-Start: #8 |
| #10 | [RTVM-250] Optional LLM summarization step and alternative-approach comparison | CORE-250/260 | Software Engineer | Finish-Start: #9 |
| #11 | [RTVM-500] Docker packaging, network isolation, replicability, and resource ceiling | NFR-500/510/520/530 | Software Engineer | Finish-Start: #10 |
| #12 | [RTVM-900] Deliverable documentation and Windows/Visual Studio consolidation check | DELIV-900/910/920/930/940/960/970 | Software Engineer | Finish-Start: #11 |

Every RTVM item in `docs/RTVM.md` that is not Withdrawn traces to
exactly one issue above.
