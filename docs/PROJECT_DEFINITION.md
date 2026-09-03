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
| SN-5 | Government evaluators running the submission | [CONFIRMED, scope of docs still open — see Open Questions] Must be able to build and run the packaged solution from the submitted materials alone (challenge explicitly forbids "just a link to a website") and interpret its output: algorithm documentation, deployment instructions, dependency documentation, and a description of validation methodology are required Phase 2 submission artifacts. |
| SN-6 | NAVAIR — analysis transparency | [CONFIRMED] A visual representation of both the analysis method and the results is a required deliverable, as is a summary metric describing algorithm performance. |

## MVP Definition

- **Target platform:** [CONFIRMED] Docker container, built to run in a
  U.S. Government IL4-accredited cloud environment; developed on
  Ubuntu with the .NET 9.0 SDK for the build during development.
  [PROPOSED] Final Visual Studio solution conversion is treated as a
  late-stage packaging task, not something the MVP needs to carry
  throughout development — confirm this reading matches the client's
  intent for "does not need to be created until the very last minute."
- **Language / stack:** [CONFIRMED] C# (.NET). Third-party/NuGet
  dependencies are to be avoided unless explicitly necessary — no
  outbound calls to non-USN-approved external services or models.
  [PROPOSED — see Open Questions] Core requirement-clustering
  algorithm is built without an LLM (maximizes the "LLM cost" score
  category and matches the challenge's stated goal of moving *beyond*
  retrieval/RAG into cluster analysis); an LLM is used, if at all,
  only for the optional stochastic summarization/visualization
  component the rules explicitly carve out as allowed to vary.
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

### Explicitly out of scope

- Visual Studio solution/project conversion — deferred to final
  packaging, per client instruction.
- Any interactive end-user application UI beyond what's needed to
  present the required visualization and metrics (this is a batch
  analysis + report deliverable, not an interactive product — confirm
  in Open Questions).
- Horizontal multi-container replicability as a day-one feature; will
  be validated once the core algorithm is stable, not built in
  parallel with it (still required before submission for the
  Replicability score, just not MVP-first).
- Training/validating against real Government Furnished Information
  (GFI) before Phase 2 access is granted — MVP development proceeds
  against representative/synthetic documents of the same described
  types until real GFI is available.
- LLM-based components beyond the optional summarization/visualization
  step, pending client confirmation of LLM strategy (see Open
  Questions).

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
  Technical Review (SETR) process, with deliveries of the SETR
  article set (SDD, PDR, CDR, PEDDAL, etc.). **Flagged to Systems
  Engineer:** this repo's pipeline already produces its own
  `docs/SDD.md` / `docs/RTVM.md` / `docs/IMPLEMENTATION_PLAN.md`
  artifacts — reconciling those with the SETR article set (which
  ones map directly, which are additional, whether PDR/CDR are
  documents or review *events*) is a documentation/build-tooling
  decision for Systems Engineer to make, not something resolved here.
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
- [PROPOSED] Build/run documentation sufficient for a government
  evaluator who has never seen this project to go from the submitted
  package to a running result using only that documentation —
  covering prerequisites, how to build the Docker image, the expected
  input document format/location with at least one working example,
  how to launch it, and what correct output looks like. Given the
  client also wants this maintainable in Visual Studio, this should
  also note where a maintainer adds a new document type or clustering
  component. **Needs client confirmation** — see Open Questions.
