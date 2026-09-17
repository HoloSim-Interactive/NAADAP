# Software Version Description — NAADAP Increment 1

| Field | Value |
| --- | --- |
| Data item | CDRL A015, DI-IPSC-81442A Software Version Description |
| Version described | Increment 1 submission candidate: `VERSION` 1.0, last CI/CD tag `v1.0.82`, candidate commit recorded in §3.1 at PCA-1 |
| Knowledge base | `2026-09-17.1` |
| Prepared by | Systems Engineer, 2026-09-17; finalized at PCA-1 |
| Classification | UNCLASSIFIED; public repository content |

## 1. Scope

### 1.1 Identification

NAADAP (the project's public shorthand for its entry to the NAVAIR/NAWCAD
prize challenge), software solution `Naadap.sln`, six assemblies
(`Naadap.Ingestion`, `Naadap.Core`, `Naadap.Output`, `Naadap.Cli`,
`Naadap.LlmStep`, `Naadap.Alternative`), all version 1.0, release tag
`v1.0.<build>` where `<build>` is the git commit count on `main` at the
tag. The version this document describes is the Increment 1 submission
candidate; the tag and commit are entered in §3.1 when PCA-1 exits.

### 1.2 System overview

A single-process batch tool that reads acquisition documents, groups them
by shared requirement content, and for each group lists the strategic
contract vehicles from a shipped knowledge base that could absorb the
group, with evidence and eliminations. Sponsor: NAVAIR/NAWCAD Procurement
Group Innovation Lab. Developer and support agency: HoloSim Interactive.
Operating site: a Government-operated cloud accredited at Impact Level 4,
as a Docker container; development on Ubuntu with the .NET 9 SDK. Full
overview: `docs/SDD.md` §System overview.

### 1.3 Document overview

This document inventories what the Increment 1 release contains, what
changed since the last tagged build, the site-specific data (none), the
documents that accompany it, how to install and verify it, and the known
limitations. It contains no controlled or personal information.

## 2. Referenced documents

| Document | Where |
| --- | --- |
| Software Design Description (DI-IPSC-81435B) | `docs/SDD.md` |
| Requirements and test procedures (DI-IPSC-81431A/81433A/81438A/81439A) | `docs/RTVM.md`, `docs/VALIDATION_METHODOLOGY.md` |
| Systems Engineering Management Plan (DI-SESS-81785B) | `docs/setr/SEMP.md` |
| Knowledge-base schema and ETL (reopened DELIV-950) | `docs/KB_SCHEMA.md` |
| Deployment instructions | `docs/DEPLOYMENT.md` |
| Dependency documentation | `docs/DEPENDENCIES.md` |
| Maintainer's guide | `docs/MAINTAINER_GUIDE.md` |
| Algorithm comparison | `docs/ALGORITHM_COMPARISON.md` |
| Software Test Report for this version | `docs/setr/STR-increment-1.md` |

## 3. Version description

### 3.1 Inventory of materials released

| Item | Identification | Notes |
| --- | --- | --- |
| Source repository | `https://github.com/HoloSim-Interactive/NAADAP`, branch and commit as tagged at PCA-1: tag ______ commit ______ | MIT license (`LICENSE`); public; no duplication restriction |
| Container image | `naadap:<tag>`, built by `docker build .` from the tagged commit; image digest recorded at PCA-1: ______ | Base images `mcr.microsoft.com/dotnet/sdk:9.0` (build stage) and `mcr.microsoft.com/dotnet/runtime:9.0` (runtime) |
| Documentation set | The twelve Phase 2 package items, `docs/setr/SEMP.md` Table under §3.1.1 | All in the repository |
| Reference-run bundle | `docs/reference-run/` | Output of the tagged commit on the reference set |

Security and privacy: no item contains controlled or personal data. No
media handling precautions apply; everything is a git repository and a
container image.

### 3.2 Inventory of software contents

| File set | Contents |
| --- | --- |
| `Naadap.sln`; `src/Naadap.*/*.csproj`; `src/**/*.cs` | Six assemblies; 66 C# source files at this revision (`docs/SDD.md` §Software units SU-01 to SU-13) |
| `src/Naadap.Output/Resources/kb/` | Knowledge base `2026-09-17.1`: `kb_version`, `vehicles.jsonl` (484 records), `families.jsonl` (11), `office_affinity.tsv` (528 rows), `sources.jsonl` (6), `manifest.sha256`, `README.md` |
| `docs/VALIDATION_METHODOLOGY.md` | Embedded into `Naadap.Output` and copied into every output bundle |
| `tests/Naadap.*.Tests/`; `tests/fixtures/` | Six test assemblies (90 test methods at this revision); smoke, synthetic, and reference-20 fixture sets with ground truth |
| `Dockerfile`, `docker-compose.yml` | Multi-stage image build; resource-tier profiles |
| `scripts/kb/`, `scripts/tp/`, `docs/research/g1-fpds/g1_extract.py` | Knowledge-base build, test-procedure runner, FPDS extraction; not part of the runtime |
| Third-party packages | `PdfPig` 0.1.16 (Apache 2.0), `DocumentFormat.OpenXml` 3.5.1 (MIT), ingestion only; test packages `xunit` 2.9.2, `xunit.runner.visualstudio` 2.8.2, `Microsoft.NET.Test.Sdk` 17.12.0, `coverlet.collector` 6.0.2 |

### 3.3 Changes installed

Changes since tag `v1.0.82` (the last CI/CD tag before the SETR package),
by SEMP §3.2.10 change class. Problem or change references are the
gates and issues in `docs/design/vehicle-recommendation-pipeline.md` and
`docs/setr/SEMP.md`.

| Class | Change | Commit | Reference | Effect on operation and interfaces |
| --- | --- | --- | --- | --- |
| I | Single-document clusters score 0.0 instead of 1.0 (`VehicleRecommender.SingletonScore`) | `f530b54` | Gate G4; SEMP R-4 | Cluster ranking changes: singletons sort after multi-document clusters. Metric unchanged on the reference set (0.60). Manifest shape unchanged. |
| I | Vehicle knowledge base added as an embedded, hashed artifact; `VehicleKnowledgeBase`, `VehicleMatcher`, `VehicleRankingWriter`; `RunManifest` gains `vehicleRecommendations`, `vehicleRankingPath`, `knowledgeBaseVersion`, `knowledgeBaseManifestSha256`; results visualization gains a per-cluster vehicle section; new output file `vehicle-ranking.tsv`; new exit code 2 | `7783c39` | Gates G5, G6; reopened DELIV-950 | Output bundle gains one file and four manifest fields (additive; existing fields unchanged). Run time on the reference set rises from under one second of compute to about six seconds wall-clock including process start. |
| II | Documentation: SEMP and SETR records, DID conformance maps, KB schema, README and deployment updates, reference-run bundle | multiple | CDRL A001–A020 | None on operation |
| II | `scripts/tp/run_resource_tests.py`, `scripts/kb/`, `g1_extract.py` | `a2885d9`, `7783c39`, `deb5e7b` | TP-220/230/520; G5; Issue I-6 | None on operation; not shipped in the image |

### 3.4 Adaptation data

None. The software carries no site-specific configuration. The only
run-time settings are the two directory paths and, when the optional LLM
step is enabled, the allowlist and endpoint environment variables
described in `docs/DEPLOYMENT.md`; those are the Government's to set and
are not part of the release.

### 3.5 Related documents

Every document listed in §2, plus the SETR review records under
`docs/setr/reviews/`, the data item list `docs/setr/CDRL.md`, and the
research and derivation records under `docs/research/` and
`docs/requirements-derivation/`. All are in the repository; none is
delivered separately.

### 3.6 Installation instructions

a. Install: `docs/DEPLOYMENT.md` ("Building the image", "Resource tiers",
"Volumes"); `docker build -t naadap .` from a clean clone of the tagged
commit, then `docker run` with the input and output mounts.
b. Other changes required: none. No site data, no external service.
c. Precautions: run with `--network none` unless the optional LLM step is
deliberately enabled; the image opens no port and reads no credentials.
d. Verification of correct installation: run the container on
`tests/fixtures/smoke/` and confirm exit code 0, a `manifest.json` with
six candidates and one skipped file (`corrupted-truncated.pdf`); run it on
`tests/fixtures/reference-20/` and confirm `summaryMetric.value` = 0.6
and `knowledgeBaseVersion` = `2026-09-17.1`. `dotnet test Naadap.sln`
from the clone must report every test passed.
e. Point of contact: HoloSim Interactive, through the repository's issue
tracker.

### 3.7 Possible problems and known errors

| Item | Recognition | Handling |
| --- | --- | --- |
| Knowledge base is NAVAIR-scoped and FY2025-only | Documents from other commands produce vehicle candidates from NAVAIR vehicles only; NAVFAC, NAVSEA, and NAVWAR vehicles other than SeaPort-NxG are absent | Expected for Increment 1; `docs/KB_SCHEMA.md` "Coverage and limits". Increment 2 widens the extract and adds FY2022–FY2024. |
| Requesting-office detection depends on a DoDAAC or a known office name in the documents | `requestingOffices` empty in the manifest; affinity channel 0; office constraint not evaluated | Add the DoDAAC to the document set's cover text, or accept lexical-only ranking. |
| Parent-IDV ordering-period end is a proxy | `provenance.overrides.ordering_period_end` note in the knowledge base row | Not used to eliminate; Increment 2 retrieves last-date-to-order. |
| Large document sets produce large `manifest.json` and `vehicle-ranking.tsv` (eliminations and full ranking per cluster) | File sizes scale with clusters × knowledge-base rows | Acceptable at the challenge's scale (twenty documents: about 0.6 MB each). A compact mode is an Increment 2 item. |
| Windows/Visual Studio open of the solution (DELIV-910) | `windows-verification.yml` workflow result | Recorded at SVR-1 from the hosted workflow; the solution targets plain `net9.0` throughout. |
| .NET 9 is a standard-term-support release with end of support in November 2026 | — | Runtime is bundled in the image; migration to .NET 10 LTS is the first recommended sustainment action (SEMP §3.2.8.4). |

## 4. Notes

CDRL, contract data requirements list (here, the program's data item
list); DID, data item description; DoDAAC, Department of Defense Activity
Address Code; IDV, indefinite-delivery vehicle; IL4, Impact Level 4; LLM,
large language model; LTS, long-term support; PCA, physical configuration
audit; SVR, system verification review. Blanks marked ______ are completed
at PCA-1 when the submission tag and image digest exist.
