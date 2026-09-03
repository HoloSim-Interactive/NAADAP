# Requirements Traceability & Verification Matrix (RTVM)

<!--
Owned by the Systems Engineer. Don't enter line items against a
[PROPOSED] item in docs/PROJECT_DEFINITION.md — wait for it to become
[CONFIRMED]. See systems-engineer.md for the escalation and handoff
rules this document participates in.
-->

## ID scheme

The category blocks below are a starting point — adjust them to fit
the project, not a fixed requirement:

| Category | Prefix | Range |
| --- | --- | --- |
| UI | UI | 001–099 |
| Data in | DATA-IN | 100–199 |
| Core algorithm / processing | CORE | 200–299 |
| Data out | DATA-OUT | 300–399 |
| Output | OUT | 400–499 |
| Non-functional | NFR | 500–599 |
| Deliverable | DELIV | 900–999 |

Companion schemes: `SN-<n>` for stakeholder needs (defined in
`docs/PROJECT_DEFINITION.md`), `TP-<nnn>` for test procedures.

`UI` is deliberately near-empty for this project — SN-5/Scope
explicitly excludes an interactive end-user application beyond the
required visualization/metrics presentation (batch analysis + report
deliverable). The single UI item below is the container's operational
entrypoint, not an application UI.

`DELIV` items are populated with real line items in this project
(rather than narrative-only coverage in `docs/SDD.md`) at Product
Manager's explicit direction on this issue — they're still typically
verified by Inspection/Demonstration, not a runtime functional test.

## Verification vocabulary

Test / Demonstration / Analysis / Inspection. `DELIV` items are
typically verified by inspection and specified in `docs/SDD.md`'s
build/toolchain conventions rather than by a runtime test.

## Status vocabulary

Draft → Approved → In Implementation → In Test → Verified, plus
Blocked / Withdrawn.

## Reference scale for test data

Several test procedures below need a concrete document-set size.
Rather than invent one, this RTVM reuses the size the client already
gave us: SN-1's PGIL-predetermined evaluation group of **20
documents**. Test procedures that need a "representative document
set" use N=20 unless a specific procedure calls out a different,
smaller set for a narrower check (e.g. a 5–6 document smoke test).

## Requirements

| Req ID | Requirement | Stakeholder Need(s) | Verification Method | Status | Commit(s) |
| --- | --- | --- | --- | --- | --- |
| UI-001 | Container entrypoint: a single invocation, pointed at an input document directory and an output directory, runs the full pipeline end-to-end with no interactive prompts. | SN-1, SN-5 | Test | Verified | 6ec62ff |
| DATA-IN-100 | Ingest a heterogeneous document set (SOW, PWS, CDRL, sources-sought notice, open-source text e.g. Congressional testimony) in common formats (PDF, DOCX, plain text) into a normalized internal record per document: extracted text, document-type tag, source filename, and date if present in the source. | SN-1, SN-6 | Test | Verified | cd6a9cc |
| DATA-IN-110 | A malformed/unsupported/corrupt input file is flagged and skipped without terminating the batch run; the run report lists every skipped file and the reason. | SN-1, SN-5 | Test | Verified | cd6a9cc |
| DATA-IN-120 | Ingestion is format- and clustering-component-extensible: adding a new document type or a new clustering component does not require modifying the ingestion core, only adding a new handler per the extension points documented in DELIV-940. | SN-4, SN-5 | Demonstration | Verified | cd6a9cc |
| [CORE-200](#rtvm-core-200) | Non-LLM clustering/requirement-extraction algorithm groups documents (or requirement statements within them) by shared acquisition-requirement content. | SN-1, SN-6 | Test | Verified | 0b69958 |
| CORE-210 | Reproducibility: given a fixed input set, the pipeline returns the same top-5 candidate contract-vehicle list in ≥95% of runs. | SN-1, SN-2 | Test | Verified | 0b69958 |
| CORE-220 | Runtime: a representative N=20 document set completes, start to candidate-list output, within 30 minutes on the baseline (1 core / 2GB RAM) hardware tier. | SN-1, SN-2 | Test | Approved | |
| CORE-230 | The pipeline completes successfully (no crash, no OOM) at each of the three defined resource tiers: 1c/2GB, 4c/8GB, 8c/16GB, against the same N=20 reference set. | SN-2 | Test | Approved | |
| CORE-240 | The core clustering/recommendation code path makes zero calls to any LLM API or LLM client library — verified by inspection of the module's dependencies and network call sites, not just by absence of a config flag. | SN-2 | Inspection | Verified | 0b69958 |
| CORE-250 | Where an LLM is used at all (summarization/interpretation/visualization step only), a single run's token spend is <50,000 tokens, and every model/microservice call target is on the USN-approved allowlist configured for IL4 operation. | SN-2, SN-3 | Test | Verified | 57945c8 |
| CORE-260 | At least one alternative clustering strategy (e.g. LLM-assisted or retrieval/RAG-based) is implemented and run against the same validation set as the chosen non-LLM core, with accuracy, runtime, and cost recorded for both, to substantiate the non-LLM core's selection in the algorithm documentation (DELIV-970). Not a second production path — a documented, rejected comparison. | SN-1, SN-2 | Analysis | Verified | 57945c8 |
| DATA-OUT-300 | Internal result representation: a ranked candidate contract-vehicle list per run, each candidate carrying a numeric confidence/score and the set of source documents that contributed to it. | SN-1, SN-6 | Test | Verified | 6ec62ff |
| DATA-OUT-310 | ~~If a database or persistent store is used, each run's result set is persisted in a documented schema (DELIV-950) and is re-readable/auditable without re-executing the pipeline.~~ **Withdrawn (SDD issue, 2026-09-03): no database is used — see `docs/SDD.md` Data Architecture. OUT-440's file-based manifest bundle satisfies the re-readable/auditable need instead.** | SN-6 | Test | Withdrawn | |
| OUT-400 | A visualization of the analysis method (e.g. pipeline stages / cluster structure diagram) is produced per run. | SN-6 | Test | Verified | 6ec62ff |
| OUT-410 | A visualization of the results (ranked candidate list / cluster-to-vehicle mapping) is produced per run. | SN-6 | Test | Verified | 6ec62ff |
| OUT-420 | A single summary performance metric (e.g. precision@5 or F1 against the validation ground truth) is produced per run, alongside the raw counts it's computed from. | SN-1, SN-6 | Test | Verified | 6ec62ff |
| OUT-430 | A written validation-methodology description (test corpus, ground-truth derivation, metric definitions) accompanies every run's output or ships as a static submission document. | SN-5, SN-6 | Inspection | Verified | 6ec62ff |
| OUT-440 | A single reviewable output bundle per run collects the candidate list (DATA-OUT-300), both visualizations (OUT-400/410), the summary metric (OUT-420), and a pointer to the validation methodology (OUT-430), indexed by one manifest file. | SN-1, SN-5, SN-6 | Test | Verified | 6ec62ff |
| NFR-500 | The Docker image bundles every runtime dependency; no dependency is fetched over the network at container run time. | SN-3 | Test | Approved | |
| NFR-510 | In default (no-LLM / LLM-disabled) operation, the container makes zero outbound network connections. When the optional LLM step is enabled, outbound connections are limited to the configured USN-approved allowlist only. | SN-3 | Test | Approved | |
| NFR-520 | Horizontal replicability: running N independent, fully stateless full-replica instances of the container against the same input set, each in isolation, reproduces the same top-5 candidate result (per CORE-210's ≥95% reproducibility bar) independently in each replica — not a single logical run sharded/partitioned across replicas. **Resolved in the SDD issue (2026-09-03), see `docs/SDD.md` Data Architecture — flagged to Solutions Architect for review; revisit before implementation if they disagree.** | SN-2 | Test | Approved | |
| NFR-530 | The deployment configuration never provisions more than 8 cores / 16GB RAM for the pipeline (hard ceiling — the challenge scores zero above this tier). | SN-2 | Inspection | Approved | |
| DELIV-900 | Full C# source code (not just compiled binaries) is included in the submission package, buildable from a clean clone. | SN-4 | Inspection | Approved | |
| DELIV-910 | The generated `.sln`/`.csproj` opens directly in Visual Studio on Windows with no conversion step; the core project(s) target plain `net9.0` (no `net9.0-windows`, WPF, or WinForms). | SN-4 | Demonstration | Approved | |
| DELIV-920 | Every NuGet/third-party dependency beyond the .NET 9 BCL has an explicit, on-file justification; nothing is added without one. | SN-4 | Inspection | Approved | |
| DELIV-930 | Build/run documentation takes a reader with no prior project exposure from a fresh clone to a completed run producing the OUT-440 bundle, using only that documentation: prerequisites, Docker build steps, expected input format/location with ≥1 worked example, launch command, and a description of correct output. | SN-5 | Demonstration | Approved | |
| DELIV-940 | Maintainer-extension documentation gives a concrete walkthrough — files to add/edit, interface to implement — for both adding a new input document type and adding a new clustering/algorithm component. | SN-4, SN-5 | Inspection | Approved | |
| DELIV-950 | ~~Database schema and ETL process are documented in `docs/SDD.md`'s Data Architecture section, conditional on the implementation actually using a database (see DATA-OUT-310).~~ **Withdrawn (SDD issue, 2026-09-03): no database is used — see `docs/SDD.md` Data Architecture.** | SN-6 | Inspection | Withdrawn | |
| DELIV-960 | The SETR documentation-artifact list (per NAVAIR Instruction 4355.19D, researched below) is reconciled against this project's `docs/RTVM.md` / `docs/SDD.md` / `docs/IMPLEMENTATION_PLAN.md` pipeline artifacts in `docs/SDD.md`, naming which pipeline document (or which review event) satisfies which SETR artifact, for at least the reviews reachable before the 2026-09-22 Submissions Deadline. | SN-4 | Inspection | Approved | |
| DELIV-970 | Algorithm documentation, dependency documentation, and deployment instructions each exist as discrete, individually locatable submission artifacts (not merged into one narrative document). | SN-5 | Inspection | Approved | |

## Test Procedures

<!-- TP-<nnn>, one per verifiable requirement, with concrete test
     input values and expected output — not just "it works." -->

- **TP-001** (UI-001): Run `docker run <image> --input /data/in
  --output /data/out` against a directory of 5 valid documents.
  Expected: process exits 0; `/data/out` contains the OUT-440 bundle;
  no interactive prompt is emitted (verify by running with stdin
  closed/`</dev/null`).

- **TP-100** (DATA-IN-100): Input: 6 documents drawn from the public
  corpus identified below — 2 SOW, 1 PWS, 1 CDRL, 1 sources-sought
  notice, 1 open-source text sample — in a mix of PDF and DOCX.
  Expected: 6 normalized records emitted, each with non-empty
  extracted text, a `doc_type` tag matching the source category, and a
  `source_filename` field; no crash.

- **TP-110** (DATA-IN-110): Input: the TP-100 batch plus 1 corrupted
  PDF (truncated file). Expected: 6 records processed successfully,
  exit code 0, run report lists the 7th file as skipped with a
  human-readable reason (e.g. "unable to parse: truncated PDF
  stream").

- **TP-120** (DATA-IN-120): Demonstration — a reviewer adds one new
  document-type handler and one new clustering-component
  implementation following DELIV-940's walkthrough, without editing
  the ingestion core's existing handler dispatch logic. Expected: both
  extensions register and run via the documented extension point only.

- **TP-200** (CORE-200): Input: a synthetic 10-document set
  constructed so that documents {A, C, E} share one requirement theme
  (e.g. "flight-line engineering services") and {B, D} share another,
  with {F..J} as unrelated distractors. Expected: cluster assignment
  places {A, C, E} in one cluster and {B, D} in a distinct cluster,
  with the assignment documented alongside the similarity/score
  threshold used.

- **TP-210** (CORE-210): Input: the fixed N=20 reference set (see
  "Reference scale" above). Run the full pipeline 20 times. Expected:
  the top-5 candidate list is identical across ≥19 of the 20 runs
  (≥95%).

- **TP-220** (CORE-220): Input: the N=20 reference set. Run inside a
  container capped at 1 core / 2GB RAM. Expected: wall-clock time from
  invocation to candidate-list output ≤30 minutes.

- **TP-230** (CORE-230): Input: the N=20 reference set, run three
  times under cgroup limits of 1c/2GB, 4c/8GB, and 8c/16GB
  respectively. Expected: all three runs complete successfully (exit
  0, no OOM kill) and produce the OUT-440 bundle.

- **TP-240** (CORE-240) — Inspection: review the core
  clustering/recommendation module's package/dependency manifest and
  outbound-call sites. Expected: zero references to any LLM SDK,
  client library, or HTTP call to a model-serving endpoint anywhere in
  that code path.

- **TP-250** (CORE-250): Input: one run with the optional LLM
  summarization step enabled. Expected: the run log records total
  token usage <50,000; a network-call audit shows every outbound
  connection target matches the configured USN-approved allowlist and
  none other.

- **TP-260** (CORE-260) — Analysis: run both the chosen non-LLM core
  and the alternative (LLM-assisted/RAG) approach against the same
  N=20 reference set with known ground truth. Expected: a comparison
  table recording accuracy (e.g. precision@5), runtime, and
  token/compute cost for both approaches is published in the algorithm
  documentation, substantiating the non-LLM core's selection.

- **TP-300** (DATA-OUT-300): Input: the CORE-200 10-document test set.
  Expected: the output data structure contains a ranked list of ≥1
  candidate vehicle; each entry has a numeric score and a non-empty
  list of contributing source documents.

- **TP-310** (DATA-OUT-310) — **Withdrawn, SDD issue 2026-09-03: no
  database is used (see `docs/SDD.md` Data Architecture).** Superseded
  by TP-440's manifest-based re-readability check.

- **TP-400 / TP-410** (OUT-400/410): Input: the CORE-200 test set.
  Expected: two distinct visualization artifacts are written to the
  run's output directory — one depicting the clustering method/
  pipeline, one depicting the ranked candidate list or cluster-to-
  vehicle mapping.

- **TP-420** (OUT-420): Input: the N=20 reference set with a defined
  ground-truth candidate list. Expected: the report includes one
  headline metric (e.g. "precision@5: 0.80") plus the raw
  correct/incorrect counts it's computed from.

- **TP-430** (OUT-430) — Inspection: confirm the run's output bundle
  (or static submission docs) includes a validation-methodology
  section naming the test corpus, how ground truth was derived, and
  the metric's definition.

- **TP-440** (OUT-440): Run the pipeline once end-to-end on the N=20
  set. Expected: a single output directory contains DATA-OUT-300's
  candidate list, both OUT-400/410 visualizations, OUT-420's metric,
  and a pointer to OUT-430's methodology doc, all referenced from one
  manifest file.

- **TP-500** (NFR-500): Build the Docker image, then disable network
  access entirely and run the full pipeline inside the built
  container. Expected: run completes with no dependency-fetch
  failures.

- **TP-510** (NFR-510): Run the pipeline (LLM step disabled) inside a
  network namespace with all egress blocked except loopback. Expected:
  run completes successfully; a connection-attempt log shows zero
  outbound attempts. Repeat with the LLM step enabled and an
  allowlist-only egress rule: run completes; log shows connections
  only to the allowlisted endpoint(s).

- **TP-520** (NFR-520): Input: the N=20 reference set. Launch 5
  independent, isolated instances of the built container concurrently,
  each pointed at the same input directory and its own output
  directory (no shared volume, no coordination between instances).
  Expected: each of the 5 instances independently produces a top-5
  candidate list matching the single-instance-run baseline (from
  TP-210) in ≥95% of a repeated-run sample per instance — i.e. no
  instance's result diverges from the others due to concurrent
  execution.

- **TP-530** (NFR-530) — Inspection: review the deployment
  configuration (Docker/orchestration resource limits) and confirm no
  profile requests more than 8 cores / 16GB RAM.

- **TP-900** (DELIV-900) — Inspection: from a clean clone, confirm
  `.csproj`/`.sln` and all `.cs` sources needed to build the submitted
  binaries are present in the package (not gitignored / not build-
  artifacts-only).

- **TP-910** (DELIV-910) — Demonstration: open the generated `.sln`
  in Visual Studio on Windows (or `dotnet build` on a Windows runner)
  with zero modifications. Expected: build succeeds. See SDD Open
  Item on *when* this gates — this test procedure runs once at
  consolidation, not per feature (see systems-engineer.md guidance on
  target-platform verification timing, to be decided in the SDD
  issue).

- **TP-920** (DELIV-920) — Inspection: diff `.csproj`
  `<PackageReference>` entries against the dependency-justification
  list in `docs/SDD.md`. Expected: every entry has a matching,
  on-file reason.

- **TP-930** (DELIV-930) — Demonstration: a reviewer with no prior
  project exposure follows only the submitted documentation, on a
  clean machine/container, from `git clone` to a completed run
  producing the OUT-440 bundle.

- **TP-940** (DELIV-940) — Inspection: confirm the maintainer docs
  contain a concrete, followable walkthrough for both extension
  points named in DATA-IN-120.

- **TP-950** (DELIV-950) — **Withdrawn, SDD issue 2026-09-03: no
  database is used** — `docs/SDD.md`'s Data Architecture section
  documents this decision and the file-based alternative instead.

- **TP-960** (DELIV-960) — Inspection: confirm `docs/SDD.md` contains
  a mapping table correlating each SETR review/artifact this project
  targets to the pipeline document or event that satisfies it.

- **TP-970** (DELIV-970) — Inspection: confirm algorithm
  documentation, dependency documentation, and deployment instructions
  exist as three separately named, separately locatable documents or
  document sections.

## Research notes (Product Manager's two assigned research tasks)

### 1. Public SOW/PWS/CDRL-style document corpus

The client-suggested example (NSWC Dahlgren "Leading Edge",
`navsea.navy.mil/...`) is **not reachable from this pipeline's build
environment** — both `navsea.navy.mil` and `navair.navy.mil` returned
HTTP 403 (Akamai edge block, consistent with `.mil` bot/network-origin
filtering, not a content problem). This is an environment constraint,
not a judgment that the source is wrong — if Product Manager or the
client can reach it from a permitted network, it remains a fine source
to fold in later.

From within this environment, **SAM.gov's public opportunities
search/API** (`sam.gov`, `https://sam.gov/api/prod/sgs/v1/search/`) is
reachable (HTTP 200, live data confirmed by a direct query during this
research) and is a directly usable substitute: it hosts real,
currently and historically posted DoD solicitations — including Navy/
NAVAIR ones — with attached SOW/PWS/CDRL documents, sources-sought
notices, and amendments, in PDF/DOCX. This satisfies "genuine,
realistic public-source SOW/PWS/CDRL-style documents" (Scope, In
scope for MVP) without needing GFI access. Recommendation: build the
MVP validation corpus from SAM.gov solicitation attachments (filtered
to Navy/NAVAIR-issuing offices and to PWS/SOW/CDRL-carrying
solicitation types), supplemented with publicly posted Congressional
testimony for the open-source-text category. This is a corpus-sourcing
decision, not an architecture one — flagging to Product Manager for
awareness, not blocking on it, since it doesn't change any RTVM item
above.

### 2. US Navy SETR process — documentation-artifact list

Verified via a reachable public source (AcqNotes' "Systems Engineering
Technical Review Process" and "Major Reviews Overview" pages, both
citing **NAVAIR Instruction 4355.19D** by name — the same instruction
`docs/PROJECT_DEFINITION.md` SN-4 points at). The standard technical-
review sequence: Alternative Systems Review (ASR), System Requirements
Review (SRR), System Functional Review (SFR), Preliminary Design
Review (PDR), Critical Design Review (CDR), Test Readiness Review
(TRR), System Verification Review (SVR) / Functional Configuration
Audit (FCA), Production Readiness Review (PRR), Physical Configuration
Audit (PCA), In-Service Review (ISR) — plus Initial Technical Review
(ITR), Technology Readiness Assessment (TRA), and Integrated Baseline
Review (IBR) earlier in the sequence.

Reconciliation against this project's own pipeline artifacts (which
ones map directly, which are additional, whether PDR/CDR are documents
or review events) is an architecture/documentation-structure decision
that belongs in `docs/SDD.md` — tracked as DELIV-960 above, to be
resolved during the SDD issue.

**Open item for Product Manager — "PEDDAL":** `docs/PROJECT_DEFINITION.md`
SN-4 names "PEDDAL" as one of the SETR article examples
("SDD, PDR, CDR, PEDDAL, etc."). None of the sources checked here
(AcqNotes' SETR pages, both of which cite NAVAIR Instruction 4355.19D
directly) list a review or artifact by that name or acronym. This
reads as a genuine "what does the client mean" question rather than a
"how do we structure it" one — per the standing instruction on this
issue, flagging it back to Product Manager rather than guessing at an
expansion. Not blocking DELIV-960, which can proceed against the
verified review list above and fold in PEDDAL once clarified.

## Open Items

- ~~**NFR-520 (horizontal replicability semantics)**~~ — **Resolved in
  the SDD issue (2026-09-03).** Interpretation (b) — independent,
  fully stateless full-replica runs — was chosen; see `docs/SDD.md`
  Data Architecture for the full rationale. Flagged to Solutions
  Architect for review; revisit before implementation if they
  disagree. NFR-520 and TP-520 above are now Approved/written on that
  basis, not Draft.
- **DELIV-960 (SETR mapping)** — resolved in `docs/SDD.md`'s new "SETR
  Documentation Mapping" section for every review reachable before
  2026-09-22. "PEDDAL" (SN-4) remains an open question back to Product
  Manager — not blocking.
