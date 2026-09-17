# <a id="svd1-title"></a>Software Version Description — NAADAP Increment 1

| Field | Value |
| --- | --- |
| Data item | Contract Data Requirements List (CDRL) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md#cdrl-a015" target="_blank">A015</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-IPSC-81442/" target="_blank">DI-IPSC-81442A</a> Software Version Description |
| Version described | Increment 1 submission, tag `v1.0.156` (commit `0f5b2b3`): <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/VERSION" target="_blank"><code>VERSION</code></a> 1.0, last Continuous Integration / Continuous Delivery (CI/CD) tag `v1.0.82`, candidate commit recorded in <a href="#svd1-3-1" target="_blank">§3.1</a> at Physical Configuration Audit 1 (PCA-1) |
| Knowledge base | `2026-09-17.1` |
| Prepared by | Systems Engineer, 2026-09-17; finalized at PCA-1 |
| Classification | UNCLASSIFIED; public repository content |

## <a id="svd1-1"></a>1. Scope

### <a id="svd1-1-1"></a>1.1 Identification

NAADAP (the project's public shorthand for its entry to the NAVAIR/NAWCAD
prize challenge), software solution <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a>, six assemblies
(`Naadap.Ingestion`, `Naadap.Core`, `Naadap.Output`, `Naadap.Cli`,
`Naadap.LlmStep`, `Naadap.Alternative`), all version 1.0, release tag
`v1.0.<build>` where `<build>` is the git commit count on `main` at the
tag. The version this document describes is the Increment 1 submission
candidate, tag `v1.0.156` at commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/0f5b2b3" target="_blank"><code>0f5b2b3</code></a> (code identical to the commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a> accepted at SVR-1; every later commit is documentation). BUILD 156 is the commit count of the full history on `main` at the tag, consistent with the earlier tags (`v1.0.82` at 82).

### <a id="svd1-1-2"></a>1.2 System overview

A single-process batch tool that reads acquisition documents, groups them
by shared requirement content, and for each group lists the strategic
contract vehicles from a shipped knowledge base that could absorb the
group, with evidence and eliminations. Sponsor: NAVAIR/NAWCAD Procurement
Group Innovation Lab. Developer and support agency: HoloSim Interactive.
Operating site: a Government-operated cloud accredited at Impact Level 4,
as a Docker container; development on Ubuntu with the .NET 9 Software Development Kit (SDK). Full
overview: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> §System overview.

### <a id="svd1-1-3"></a>1.3 Document overview

This document inventories what the Increment 1 release contains, what
changed since the last tagged build, the site-specific data (none), the
documents that accompany it, how to install and verify it, and the known
limitations. It contains no controlled or personal information.

## <a id="svd1-2"></a>2. Referenced documents

| Document | Where |
| --- | --- |
| Software Design Description (<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-IPSC-81435/" target="_blank">DI-IPSC-81435B</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> |
| Requirements and test procedures (<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-IPSC-81431/" target="_blank">DI-IPSC-81431A</a>/81433A/81438A/81439A) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a> |
| Systems Engineering Management Plan (<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785B</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md" target="_blank"><code>docs/setr/SEMP.md</code></a> |
| Knowledge-base schema and Extract, Transform, Load (ETL) (reopened <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/KB_SCHEMA.md" target="_blank"><code>docs/KB_SCHEMA.md</code></a> |
| Deployment instructions | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> |
| Dependency documentation | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a> |
| Maintainer's guide | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/MAINTAINER_GUIDE.md" target="_blank"><code>docs/MAINTAINER_GUIDE.md</code></a> |
| Algorithm comparison | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank"><code>docs/ALGORITHM_COMPARISON.md</code></a> |
| Software Test Report for this version | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/STR-increment-1.md" target="_blank"><code>docs/setr/STR-increment-1.md</code></a> |

## <a id="svd1-3"></a>3. Version description

### <a id="svd1-3-1"></a>3.1 Inventory of materials released

| Item | Identification | Notes |
| --- | --- | --- |
| Source repository | `https://github.com/HoloSim-Interactive/NAADAP`, `main`, tag `v1.0.156`, commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/0f5b2b3" target="_blank"><code>0f5b2b3</code></a> (PCA-1, 2026-09-17) | Massachusetts Institute of Technology (MIT) license (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/LICENSE" target="_blank"><code>LICENSE</code></a>); public; no duplication restriction |
| Container image | `naadap:svr1`, built by `docker build .` at commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/4127c5a" target="_blank"><code>4127c5a</code></a> on 2026-09-17 (Docker Desktop 29.8.0). Image manifest `sha256:58f4295851e8618ada400659ea0b2f3d8e7f5e2a2ed51360821d84fdedb49c9e`; image config `sha256:eafe57f1501d17087fe59c58d59dee372de4079b4e18176397bf88218ba3af6d`. These two are reproducible across rebuilds of the same commit; the manifest-list digest reported as the image Identifier (ID) by `docker image inspect` (`sha256:87ec5f0f…` on the second build) changes per build because BuildKit's provenance attestation is regenerated, and is not used as the identifier. Re-recording against the tagged commit is RFA-PCA1-1 (Principal's build from a clean clone of `v1.0.156`); the digests of that build are entered in the PCA-1 record and here when reported. | Base images `mcr.microsoft.com/dotnet/sdk:9.0@sha256:20387c66…` (build stage) and `mcr.microsoft.com/dotnet/runtime:9.0@sha256:647b8b6d…` (runtime) |
| Documentation set | The twelve Phase 2 package items listed in Systems Engineering Management Plan (SEMP) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-1-1" target="_blank">§3.1.1</a> | All in the repository |
| Reference-run bundle | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/reference-run/" target="_blank"><code>docs/reference-run/</code></a> | Output of the tagged commit on the reference set |

Security and privacy: no item contains controlled or personal data. No
media handling precautions apply; everything is a git repository and a
container image.

### <a id="svd1-3-2"></a>3.2 Inventory of software contents

| File set | Contents |
| --- | --- |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a>; `src/Naadap.*/*.csproj`; `src/**/*.cs` | Six assemblies; 66 C# source files at this revision (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> §Software units SU-01 to SU-13) |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Output/Resources/kb/" target="_blank"><code>src/Naadap.Output/Resources/kb/</code></a> | Knowledge base `2026-09-17.1`: `kb_version`, `vehicles.jsonl` (484 records), `families.jsonl` (11), `office_affinity.tsv` (528 rows), `sources.jsonl` (6), `manifest.sha256`, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/README.md" target="_blank"><code>README.md</code></a> |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a> | Embedded into `Naadap.Output` and copied into every output bundle |
| `tests/Naadap.*.Tests/`; <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/tests/fixtures/" target="_blank"><code>tests/fixtures/</code></a> | Six test assemblies (90 test methods at this revision); smoke, synthetic, and reference-20 fixture sets with ground truth |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Dockerfile" target="_blank"><code>Dockerfile</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docker-compose.yml" target="_blank"><code>docker-compose.yml</code></a> | Multi-stage image build; resource-tier profiles |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/scripts/kb/" target="_blank"><code>scripts/kb/</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/scripts/tp/" target="_blank"><code>scripts/tp/</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/g1_extract.py" target="_blank"><code>docs/research/g1-fpds/g1_extract.py</code></a> | Knowledge-base build, test-procedure runner, Federal Procurement Data System (FPDS) extraction; not part of the runtime |
| Third-party packages | `PdfPig` 0.1.16 (Apache 2.0), `DocumentFormat.OpenXml` 3.5.1 (MIT), ingestion only; test packages `xunit` 2.9.2, `xunit.runner.visualstudio` 2.8.2, `Microsoft.NET.Test.Sdk` 17.12.0, `coverlet.collector` 6.0.2 |

### <a id="svd1-3-3"></a>3.3 Changes installed

Changes since tag `v1.0.82` (the last CI/CD tag before the Systems Engineering Technical Review (SETR) package),
by SEMP <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-10" target="_blank">§3.2.10</a> change class. Problem or change references are the
gates and issues in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> and
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md" target="_blank"><code>docs/setr/SEMP.md</code></a>.

| Class | Change | Commit | Reference | Effect on operation and interfaces |
| --- | --- | --- | --- | --- |
| I | Single-document clusters score 0.0 instead of 1.0 (`VehicleRecommender.SingletonScore`) | <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/f530b54" target="_blank"><code>f530b54</code></a> | Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a>; SEMP <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-r-4" target="_blank">R-4</a> | Cluster ranking changes: singletons sort after multi-document clusters. Metric unchanged on the reference set (0.60). Manifest shape unchanged. |
| I | Vehicle knowledge base added as an embedded, hashed artifact; `VehicleKnowledgeBase`, `VehicleMatcher`, `VehicleRankingWriter`; `RunManifest` gains `vehicleRecommendations`, `vehicleRankingPath`, `knowledgeBaseVersion`, `knowledgeBaseManifestSha256`; results visualization gains a per-cluster vehicle section; new output file `vehicle-ranking.tsv`; new exit code 2 | <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/7783c39" target="_blank"><code>7783c39</code></a> | Gates <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a>; reopened <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a> | Output bundle gains one file and four manifest fields (additive; existing fields unchanged). Run time on the reference set rises from under one second of compute to about six seconds wall-clock including process start. |
| II | Documentation: SEMP and SETR records, Data Item Description (DID) conformance maps, Knowledge Base (KB) schema, README and deployment updates, reference-run bundle | multiple | CDRL <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md#cdrl-a001" target="_blank">A001</a>–<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md#cdrl-a020" target="_blank">A020</a> | None on operation |
| II | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/scripts/tp/run_resource_tests.py" target="_blank"><code>scripts/tp/run_resource_tests.py</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/scripts/kb/" target="_blank"><code>scripts/kb/</code></a>, `g1_extract.py` | <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/a2885d9" target="_blank"><code>a2885d9</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/7783c39" target="_blank"><code>7783c39</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/deb5e7b" target="_blank"><code>deb5e7b</code></a> | Test Procedure (TP) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>/230/520; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a>; Issue <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-i-6" target="_blank">I-6</a> | None on operation; not shipped in the image |

### <a id="svd1-3-4"></a>3.4 Adaptation data

None. The software carries no site-specific configuration. The only
run-time settings are the two directory paths and, when the optional Large Language Model (LLM)
step is enabled, the allowlist and endpoint environment variables
described in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a>; those are the Government's to set and
are not part of the release.

### <a id="svd1-3-5"></a>3.5 Related documents

Every document listed in <a href="#svd1-2" target="_blank">§2</a>, plus the SETR review records under
<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a>, the data item list <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md" target="_blank"><code>docs/setr/CDRL.md</code></a>, and the
research and derivation records under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/research/" target="_blank"><code>docs/research/</code></a> and
<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/requirements-derivation/" target="_blank"><code>docs/requirements-derivation/</code></a>. All are in the repository; none is
delivered separately.

### <a id="svd1-3-6"></a>3.6 Installation instructions

a. Install: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> ("Building the image", "Resource tiers",
"Volumes"); `docker build -t naadap .` from a clean clone of the tagged
commit, then `docker run` with the input and output mounts.
b. Other changes required: none. No site data, no external service.
c. Precautions: run with `--network none` unless the optional LLM step is
deliberately enabled; the image opens no port and reads no credentials.
d. Verification of correct installation: run the container on
<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/tests/fixtures/smoke/" target="_blank"><code>tests/fixtures/smoke/</code></a> and confirm exit code 0, a `manifest.json` with
six candidates and one skipped file (`corrupted-truncated.pdf`); run it on
<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/tests/fixtures/reference-20/" target="_blank"><code>tests/fixtures/reference-20/</code></a> and confirm `summaryMetric.value` = 0.6
and `knowledgeBaseVersion` = `2026-09-17.1`. `dotnet test Naadap.sln`
from the clone must report every test passed.
e. Point of contact: HoloSim Interactive, through the repository's issue
tracker.

### <a id="svd1-3-7"></a>3.7 Possible problems and known errors

| Item | Recognition | Handling |
| --- | --- | --- |
| Knowledge base is Naval Air Systems Command (NAVAIR) NAVAIR-scoped and FY2025-only | Documents from other commands produce vehicle candidates from NAVAIR vehicles only; Naval Facilities Engineering Systems Command (NAVFAC), Naval Sea Systems Command (NAVSEA), and Naval Information Warfare Systems Command (NAVWAR) vehicles other than SeaPort-NxG are absent | Expected for Increment 1; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/KB_SCHEMA.md" target="_blank"><code>docs/KB_SCHEMA.md</code></a> "Coverage and limits". Increment 2 widens the extract and adds FY2022–FY2024. |
| Requesting-office detection depends on a Department of Defense Activity Address Code (DoDAAC) or a known office name in the documents | `requestingOffices` empty in the manifest; affinity channel 0; office constraint not evaluated | Add the DoDAAC to the document set's cover text, or accept lexical-only ranking. |
| Parent-IDV ordering-period end is a proxy | `provenance.overrides.ordering_period_end` note in the knowledge base row | Not used to eliminate; Increment 2 retrieves last-date-to-order. |
| Large document sets produce large `manifest.json` and `vehicle-ranking.tsv` (eliminations and full ranking per cluster) | File sizes scale with clusters × knowledge-base rows | Acceptable at the challenge's scale (twenty documents: about 0.6 MB each). A compact mode is an Increment 2 item. |
| Windows/Visual Studio open of the solution (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a>) | `windows-verification.yml` workflow result | Recorded at System Verification Review 1 (SVR-1) from the hosted workflow; the solution targets plain `net9.0` throughout. |
| .NET 9 is a standard-term-support release with end of support in November 2026 | — | Runtime is bundled in the image; migration to .NET 10 Long-Term Support (LTS) is the first recommended sustainment action (SEMP <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-8-4" target="_blank">§3.2.8.4</a>). |

## <a id="svd1-4"></a>4. Notes

Abbreviations used in this document, each spelled out at its first use in the body (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-11" target="_blank">SEMP §3.2.11</a>, documentation conventions). The program's global list is <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-appendix-a" target="_blank">SEMP Appendix A</a>. The image digests of the tagged build are entered on RFA-PCA1-1 closure.

| Abbreviation | Expansion |
| --- | --- |
| CDRL | Contract Data Requirements List |
| CI/CD | Continuous Integration / Continuous Delivery |
| DID | Data Item Description |
| DoDAAC | Department of Defense Activity Address Code |
| ETL | Extract, Transform, Load |
| FPDS | Federal Procurement Data System |
| ID | Identifier |
| IDV | Indefinite-Delivery Vehicle |
| IL4 | Impact Level 4 |
| KB | Knowledge Base |
| LLM | Large Language Model |
| LTS | Long-Term Support |
| MIT | Massachusetts Institute of Technology |
| NAVAIR | Naval Air Systems Command |
| NAVFAC | Naval Facilities Engineering Systems Command |
| NAVSEA | Naval Sea Systems Command |
| NAVWAR | Naval Information Warfare Systems Command |
| PCA | Physical Configuration Audit |
| SDK | Software Development Kit |
| SEMP | Systems Engineering Management Plan |
| SETR | Systems Engineering Technical Review |
| SVR | System Verification Review |
| TP | Test Procedure |
