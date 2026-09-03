# Project Definition

<!--
Owned by the Product Manager. Every item below is tagged
[CONFIRMED] (stated directly by the client) or [PROPOSED] (a
recommended default, not yet a decision). Nothing may be built
against a [PROPOSED] item — flip it to [CONFIRMED] once the client
has actually responded, before handing off to the Systems Engineer.
-->

## Mission Statement

[CONFIRMED] Build an algorithmic recommender system that analyzes
NAVAIR/NAWCAD procurement documentation — statements of work (SOWs),
performance work statements (PWSs), CDRLs, sources sought notices, and
open-source material such as Congressional testimony — to identify
common acquisition requirements across otherwise disparate contracts,
and to recommend candidate strategic contract vehicles for
consolidation. The system is being built as HoloSim's entry to the
NAVAIR/NAWCAD NAADAP prize challenge (issue #1 body is the full
official challenge announcement). "NAADAP" is this project's internal
shorthand for the challenge; use it in place of the challenge's full
title in all repository content, which is public.

## Value

[CONFIRMED] For NAVAIR: reduces duplicated contracting effort,
increases buying power, and speeds acquisition by surfacing which
existing (or new) strategic vehicles can absorb a given requirement
instead of every requirement owner standing up their own contract.

[CONFIRMED] For HoloSim: a compliant, competitive Phase 2 submission
that scores well against the challenge's published judging criteria
(50% robustness — runtime, replicability, compute cost, LLM cost; 50%
validation — accuracy against a PGIL-defined correct set), with a shot
at the $150K prize pool and follow-on OTA/CRADA opportunities.

## Stakeholders and Needs

| Need ID | Stakeholder | Description & Rationale |
| --- | --- | --- |
| SN-1 | Challenge judges / NAVAIR technical evaluators (PGIL) | [CONFIRMED] Solution must process a document set and return a candidate list of contract vehicles within 30 minutes; must reproduce the same top-5 results ≥95% of the time; is scored 2 points per correct prediction (within a PGIL-predetermined group of 20) on the initial technical evaluation, plus a live Demo Day identification of 5 manually-selected candidates. |
| SN-2 | Challenge judges — resource/cost scoring | [CONFIRMED] Score rewards low compute footprint (best at 1 core/2GB RAM, degrading at 4c/8GB and 8c/16GB, zero above that), horizontal replicability without changing results, and either no LLM use or low LLM token spend (<50k tokens/retrieval) if one is used. |
| SN-3 | NAVAIR IL4 deployment environment | [CONFIRMED] Must run inside a Docker container with all dependencies bundled, be deployable to a U.S. Government-owned/operated IL4-accredited cloud, and must not call any external service except USN-approved models/microservices — no outbound calls to industry-hosted models. |
| SN-4 | HoloSim engineering team (build/maintain) | [CONFIRMED] Source code (C#) is a must-have deliverable; final IDE target is Microsoft Visual Studio, but that is a packaging step, not a port — Ubuntu + .NET 9.0 SDK for interim development. Avoid 3rd-party plugins/software unless explicitly necessary. SDLC follows MBSE; team process follows Agile Systems Engineering practices; documentation follows the US Navy SETR review process (SDD, PDR, CDR, PEDDAL, etc. as articles/deliveries). |
| SN-5 | Government evaluators running the submission | [CONFIRMED] Must be able to build and run the packaged solution from the submitted materials alone (challenge explicitly forbids "just a link to a website") and interpret its output: algorithm documentation, deployment instructions, dependency documentation, and a description of validation methodology are required Phase 2 submission artifacts, at build/run doc rigor sufficient for a first-time reader to go from fresh clone to a running result (see Deliverable Requirements). |
| SN-6 | NAVAIR — analysis transparency | [CONFIRMED] A visual representation of both the analysis method and the results is a required deliverable, as is a summary metric describing algorithm performance. |

## MVP Definition

- **Target platform:** [CONFIRMED] Docker container, built to run in a
  U.S. Government IL4-accredited cloud environment; developed on
  Ubuntu with the .NET 9.0 SDK for the build during development.
  [CONFIRMED] The Visual Studio deliverable is a packaging step, not a
  conversion — a `.sln`/`.csproj` produced on Ubuntu opens directly in
  Visual Studio on Windows, no translation phase. Keep the core on
  plain `net9.0` (no `net9.0-windows`, WPF, WinForms) so this stays
  true; Systems Engineer should not budget schedule for a "convert to
  VS" phase.
- **Timeline (worst case, client-confirmed 2026-09-03):** [CONFIRMED]
  The challenge announcement text is internally inconsistent on dates
  (see issue #1 body: summary box vs. TIMELINE section disagree). Per
  client instruction, plan against the worst case — Submissions
  Deadline **2026-09-22**, Final Demo Day **2026-11-09** — until the
  client gets an authoritative answer from Tech Grove. Do not plan
  against the later (Oct 2 / Nov 19) dates.
- **Language / stack:** [CONFIRMED] C# (.NET). Third-party/NuGet
  dependencies are to be avoided unless explicitly necessary — no
  outbound calls to non-USN-approved external services or models.
  [CONFIRMED, client 2026-09-03] Core requirement-clustering algorithm
  is built without an LLM — this both maximizes the "LLM cost" score
  category and matches the challenge's own stated goal of moving
  *beyond* retrieval/RAG into cluster analysis, which the client wants
  the solution to lean into heavily. An LLM is used, if at all, only
  for the optional stochastic summarization/interpretation/
  visualization step the rules explicitly carve out as allowed to
  vary — and only once that use is shown to be sound, lower-risk logic
  (not a shortcut around the clustering work). The client also wants
  alternative (e.g. LLM-assisted or retrieval-based) approaches
  demonstrated and their shortcomings documented through rigorous
  testing, rather than simply omitted — this supports the "why not
  LLM-first" narrative required for the submission's algorithm
  documentation.
- **Output format and delivery:** [CONFIRMED] A candidate list of
  contract vehicles for consolidation, a visualization of the analysis
  method and of the results, and a summary performance/validation
  metric — delivered per Phase 2 requirements as: algorithm
  documentation, complete codebase, Docker container, packaging/
  deployment instructions, database schema + ETL docs (if a database
  is used), and a description of validation methodology.

## Scope

### In scope for MVP

- Ingest the described document population: SOWs, PWSs, CDRLs, sources
  sought notices, and open-source text (e.g. Congressional testimony).
- Analyze/cluster document content to identify common requirements
  across documents.
- Recommend a candidate list of contract vehicles for consolidation,
  reproducible (same top-5) ≥95% of the time.
- Produce a visualization of both the analysis method and the results.
- Produce a summary metric describing algorithm performance, and a
  written description of the validation methodology.
- Package as a single Docker container with all dependencies bundled,
  capable of processing a document set and returning candidates within
  30 minutes on the target hardware tier(s).
- Author the submission-required documentation: algorithm
  documentation, dependency documentation, deployment instructions.
- [CONFIRMED, client 2026-09-03] Develop and validate against genuine,
  realistic SOW/PWS/CDRL-style documents sourced from public
  references (e.g. NAVSEA/NSWC Dahlgren "Leading Edge" publications,
  https://www.navsea.navy.mil/Home/Warfare-Centers/NSWC-Dahlgren/Resources/Leading-Edge/I-I-Leading-Edge/Washington02/)
  — not fabricated/synthetic documents. **Flagged to Systems
  Engineer:** research and identify a concrete public-document corpus
  of this kind to develop and test against.
- [CONFIRMED, client 2026-09-03] Alongside the no-LLM-in-core-clustering
  approach, demonstrate at least one alternative strategy (e.g.
  retrieval/RAG- or LLM-assisted clustering) and document its
  shortcomings through rigorous testing — this supports the rationale
  for the chosen approach in the required algorithm documentation, it
  is not scope creep into building a second production path.

### Explicitly out of scope

- Visual Studio solution/project conversion — deferred to final
  packaging, per client instruction.
- Any interactive end-user application UI beyond what's needed to
  present the required visualization and metrics (this is a batch
  analysis + report deliverable, not an interactive product).
- Horizontal multi-container replicability as a day-one feature; will
  be validated once the core algorithm is stable, not built in
  parallel with it (still required before submission for the
  Replicability score, just not MVP-first).
- Phase 1 Pre-Screening submission and Phase 2 GFI (real procurement
  document corpus) access. [CONFIRMED, client 2026-09-03] As of
  2026-09-03, HoloSim has not yet submitted the Phase 1 Pre-Screening
  Questionnaire and does not have Phase 2 GFI access. Development
  proceeds now, ahead of both, against real public-source documents
  (see In Scope above) rather than waiting.
- LLM-based components beyond the optional summarization/
  interpretation/visualization step (see MVP Definition — Language /
  stack).

## Deliverable Requirements

- [CONFIRMED] Full C# source code is a must-have deliverable.
- [CONFIRMED] Final deliverable must open as a Microsoft Visual Studio
  project/solution; that conversion is explicitly deferred until the
  final stage of the project rather than maintained throughout.
- [CONFIRMED] Development proceeds on Ubuntu using the .NET 9.0 SDK
  (`dotnet build` / `dotnet test` over `.csproj`). NOTE — CORRECTION
  (client, 2026-09-03): the kickoff comment said "CMake"; C# does not
  use CMake, its build system is MSBuild driven by the `dotnet` CLI.
  No CMake anywhere in this project.
- [CONFIRMED] The Visual Studio deliverable is NOT a later port. A
  `.sln`/`.csproj` generated on Ubuntu opens directly in Visual Studio
  on Windows — same files, no conversion step. The only real trap is a
  Windows-only target framework, so the core stays on plain `net9.0`
  and avoids WPF/WinForms. This is also required by the IL4 Linux
  Docker constraint. Do not budget schedule for a "convert to VS"
  phase; it does not exist.
- [CONFIRMED] Avoid requiring third-party plugins or software unless
  explicitly necessary; minimize NuGet/external dependencies.
- [CONFIRMED] SDLC follows MBSE (Model-Based Systems Engineering).
- [CONFIRMED] Project management/team communication follows Agile
  Systems Engineering industry best practices.
- [CONFIRMED] Documentation follows the US Navy Systems Engineering
  Technical Review (SETR) process and CDRL artifact conventions, at
  the same rigor as the challenge-mandated algorithm docs, deployment
  instructions, and dependency docs, developed in line with this
  project's normal documentation workflow — just with more rigor and
  possibly additional artifacts, not a separate process.
  [CONFIRMED, client 2026-09-03] **Flagged to Systems Engineer:**
  research the SETR process and produce the list of its documentation
  artifacts as explicit requirements, placed across the development
  timeline. Not every SETR artifact needs to be produced — but, at
  minimum, produce the ones that correspond to what this pipeline
  already generates (RTVM, SDD, etc.), named and formatted per the
  standard found in that research, and reconcile them with this
  repo's existing `docs/SDD.md` / `docs/RTVM.md` /
  `docs/IMPLEMENTATION_PLAN.md` artifacts (which map directly, which
  are additional, whether PDR/CDR are documents or review *events*).
- [CONFIRMED] Must be packaged as a Docker container bundling all
  external dependencies; must not access any external service outside
  USN-approved models/microservices when running in an IL4
  environment.
- [CONFIRMED] Deliverables must include: database schema and ETL
  process documentation (if a database is used), a visual
  representation of the analysis method, a visual representation of
  results, an algorithm performance summary metric, a description of
  the validation methodology, and documentation of external
  dependencies — these are Phase 2 submission requirements, not
  optional extras.
- [CONFIRMED, client 2026-09-03] Build/run documentation sufficient
  for a government evaluator who has never seen this project to go
  from the submitted package to a running result using only that
  documentation — covering prerequisites, how to build the Docker
  image, the expected input document format/location with at least
  one working example, how to launch it, and what correct output
  looks like. Given the client also wants this maintainable in Visual
  Studio, this should also note where a maintainer adds a new document
  type or clustering component. Confirmed as part of the client's
  instruction to develop documentation "in line with our workflow ...
  only ... with more rigor" — this is that workflow's standard
  build/run doc bar, applied at SETR/CDRL rigor.
