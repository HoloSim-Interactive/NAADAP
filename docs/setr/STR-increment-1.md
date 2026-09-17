# <a id="str1-title"></a>Software Test Report — NAADAP Increment 1, SVR-1 / FCA-1

| Field | Value |
| --- | --- |
| Data items | Contract Data Requirements List (CDRL) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md#cdrl-a013" target="_blank">A013</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-IPSC-81440/" target="_blank">DI-IPSC-81440A</a> Software Test Report; CDRL <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md#cdrl-a018" target="_blank">A018</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-NDTI-80809/" target="_blank">DI-NDTI-80809B</a> Test/Inspection Report |
| Software tested | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a> at commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/a2885d9" target="_blank"><code>a2885d9</code></a> (2026-09-17), knowledge base `2026-09-17.1` |
| Test procedures | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> Test Procedures (TP-nnn); <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a> |
| Prepared by | Systems Engineer from Test Engineer executions, 2026-09-17; finalized at System Verification Review 1 (SVR-1) exit |
| Evidence | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/evidence/" target="_blank"><code>docs/setr/evidence/</code></a> (machine-readable results), <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/reference-run/" target="_blank"><code>docs/reference-run/</code></a> (output bundle) |
| Classification | UNCLASSIFIED |

## <a id="str1-1"></a>1. Scope

### <a id="str1-1-1"></a>1.1 Identification

NAADAP, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a>, six assemblies, version 1.0, candidate for the
Increment 1 submission tag (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SVD-increment-1.md#svd1-1-1" target="_blank"><code>docs/setr/SVD-increment-1.md</code> §1.1</a>).

### <a id="str1-1-2"></a>1.2 System overview

See <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> §System overview.

### <a id="str1-1-3"></a>1.3 Document overview

This report records the results of every test procedure executed for
Increment 1 verification: the automated suite, the reproducibility,
runtime, resource-tier, and replication procedures, and the inspections.
It states the deviations from the procedures as written and their effect
on validity. It contains no controlled or personal information.

## <a id="str1-2"></a>2. Referenced documents

<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> (requirements and test procedures); <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a>;
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-13" target="_blank"><code>docs/setr/SEMP.md</code> §3.2.13</a> (SVR-1 entry and exit criteria); <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a>
(resource tiers); <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/KB_SCHEMA.md" target="_blank"><code>docs/KB_SCHEMA.md</code></a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-IPSC-81440/" target="_blank">DI-IPSC-81440A</a> (2000-01-11);
<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-NDTI-80809/" target="_blank">DI-NDTI-80809B</a> (1997-01-24).

## <a id="str1-3"></a>3. Overview of test results

### <a id="str1-3-1"></a>3.1 Overall assessment

a. The software meets every Increment 1 requirement whose procedure could
be executed in the verification environment. The automated suite passes
(90 of 90). The pipeline is deterministic to the byte across 20 runs
(<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a>). It completes the reference set in 7.4 seconds at 1 core / 2 GB
(<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, limit 30 minutes, design target 5 minutes). Four concurrent
1-core replicas reproduce the baseline output exactly and process four
document sets 3.9 times faster than one replica in sequence (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a>).
Every inspection passes (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-240" target="_blank">TP-240</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-500" target="_blank">TP-500</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-510" target="_blank">TP-510</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-530" target="_blank">TP-530</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-920" target="_blank">TP-920</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-950" target="_blank">TP-950</a>).

b. Remaining limitations detected by testing:

| Item | Impact | Design impact | Recommended approach |
| --- | --- | --- | --- |
| ~~<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a>'s 8-core / 16 GB tier could not be executed on the 4-CPU verification host~~ **Resolved 2026-09-17 (Request for Action (RFA) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md#trsr-svr1-rfa-svr1-1" target="_blank">RFA-SVR1-1</a>):** all three tiers executed by the Principal on a 32-CPU / 63 GiB Docker Desktop host against the official image; exit 0 and manifest SHA-256 `ad331596…` at every tier (<a href="#str1-4-4" target="_blank">§4.4</a>) | None remaining; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a> Verified | None | Closed |
| Peak memory was not sampled inside the containers | The 2 GB cap was enforced by the kernel and no run was killed, which is the requirement; the numeric peak is not recorded | None | Add `docker stats` sampling to <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/scripts/tp/run_resource_tests.py" target="_blank"><code>scripts/tp/run_resource_tests.py</code></a> |
| ~~The container image under test was built from a local `dotnet publish` onto the official runtime base~~ **Resolved 2026-09-17 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md#trsr-svr1-rfa-svr1-3" target="_blank">RFA-SVR1-3</a>):** the Principal built the official multi-stage image with `docker build -t naadap:svr1 .` at commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/4127c5a" target="_blank"><code>4127c5a</code></a> (restore and publish stages completed; image manifest `sha256:58f42958…`, config `sha256:eafe57f1…`) and every <a href="#str1-4-3" target="_blank">§4.3</a>–4.5 result below was reproduced on it | None remaining | None | Closed |
| ~~<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a> (Visual Studio / Windows) not run on the candidate~~ **Resolved 2026-09-17 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md#trsr-svr1-rfa-svr1-2" target="_blank">RFA-SVR1-2</a>):** `windows-verification` run <a href="https://github.com/HoloSim-Interactive/NAADAP/actions/runs/35178944747" target="_blank">35178944747</a> and `build-and-test` run <a href="https://github.com/HoloSim-Interactive/NAADAP/actions/runs/35178897168" target="_blank">35178897168</a> (windows-latest job) succeeded on <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a>: Release build, 90 tests, framework-dependent and self-contained publish on Windows | None remaining; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a> Verified | None | Closed |

### <a id="str1-3-2"></a>3.2 Impact of test environment

Tests ran on a 4 vCPU / 16 GB Ubuntu host inside the development
container, with a Docker daemon started for the tier and replication
procedures. Differences from the operational environment (a Government
Impact Level 4 (IL4) cloud): the host Central Processing Unit (CPU) is faster than the 1-core tier's likely hardware,
so absolute times are optimistic by an unknown factor; the 30-minute limit
leaves a margin of more than two hundred times, so the conclusion does not
depend on the factor. Network policy: containers ran with `--network none`,
which is stricter than the operational environment and is the condition
Non-Functional Requirement (NFR) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-510" target="_blank">NFR-510</a> requires.

### <a id="str1-3-3"></a>3.3 Recommended improvements

1. Sample peak memory in the resource-test runner.
2. Add an `hit@k` measure of vehicle recommendations against a Naval Air Systems Command (NAVAIR)
ground truth once Government Furnished Information (GFI) arrives; the present metric scores cluster
composition, not vehicle identity (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a>).
3. Reduce `manifest.json` size by moving eliminations to the ranking file
in a compact mode (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SVD-increment-1.md#svd1-3-7" target="_blank">Software Version Description §3.7</a>).

## <a id="str1-4"></a>4. Detailed test results

Test identifiers are the Requirements Traceability and Verification Matrix (RTVM)'s. "As expected" means every step produced
the expected result with no deviation.

### <a id="str1-4-1"></a>4.1 Automated suite (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-001" target="_blank">TP-001</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-100" target="_blank">TP-100</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-110" target="_blank">TP-110</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-120" target="_blank">TP-120</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-200" target="_blank">TP-200</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-250" target="_blank">TP-250</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-300" target="_blank">TP-300</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-400" target="_blank">TP-400</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-410" target="_blank">TP-410</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-420" target="_blank">TP-420</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-430" target="_blank">TP-430</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-440" target="_blank">TP-440</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-500" target="_blank">TP-500</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-510" target="_blank">TP-510</a> component tests, plus the knowledge-base and matcher tests)

| Assembly | Tests | Result |
| --- | --- | --- |
| Naadap.Ingestion.Tests | 16 | passed |
| Naadap.Core.Tests | 14 | passed |
| Naadap.Output.Tests | 26 | passed (10 added 2026-09-17: knowledge-base integrity and content, matcher rules) |
| Naadap.Cli.Tests | 12 | passed |
| Naadap.LlmStep.Tests | 14 | passed |
| Naadap.Alternative.Tests | 8 | passed |

Summary: all results as expected. Problems encountered: none. Deviations: none.

### <a id="str1-4-2"></a>4.2 <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a> Reproducibility

Summary: as expected. 20 runs of the reference set in process on the host;
1 distinct top-5 cluster list, 1 distinct set of per-cluster vehicle
candidate lists, 1 distinct `manifest.json` SHA-256
(`ad3315960a32657b…`), which also equals the committed
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/reference-run/manifest.json" target="_blank"><code>docs/reference-run/manifest.json</code></a> and every container run below.
Requirement: identical in 19 of 20; achieved 20 of 20. Evidence:
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/evidence/tp-210-results-2026-09-17.json" target="_blank"><code>evidence/tp-210-results-2026-09-17.json</code></a>.

### <a id="str1-4-3"></a>4.3 <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a> Runtime at 1 core / 2 GB

Summary: as expected. Container `naadap:svr1`, `--cpus=1 --memory=2g
--network none`, reference set: exit 0, wall-clock 7.43 s from `docker run`
to exit. Limit 1,800 s; design target 300 s. Evidence:
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/evidence/tp-220-230-520-results-2026-09-17.json" target="_blank"><code>evidence/tp-220-230-520-results-2026-09-17.json</code></a>.

### <a id="str1-4-4"></a>4.4 <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a> Resource tiers

Two executions. First, the sandbox host (4 vCPU / 16 GB, image built from local publish); second, the Principal's host (32 CPU / 63 GiB Docker Desktop, official `docker build`). Evidence: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/evidence/tp-220-230-520-results-2026-09-17.json" target="_blank"><code>evidence/tp-220-230-520-results-2026-09-17.json</code></a> and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/evidence/tp-220-230-520-results-2026-09-18-principal.json" target="_blank"><code>evidence/tp-220-230-520-results-2026-09-18-principal.json</code></a>.

| Tier | Sandbox: exit, wall-clock | Principal: exit, wall-clock | Manifest SHA-256 (both) | Status |
| --- | --- | --- | --- | --- |
| 1 core / 2 GB | 0, 7.43 s | 0, 4.38 s | `ad331596…` | as expected |
| 4 cores / 8 GB | 0, 5.82 s | 0, 3.94 s | `ad331596…` | as expected |
| 8 cores / 16 GB | not run (4-CPU host) | 0, 3.95 s | `ad331596…` | as expected on the Principal's host; see 4.4.1 |

4.4.1 Deviation, sandbox execution of tier 8c/16GB: the sandbox host has
4 CPUs and Docker rejects `--cpus=8`. Rationale: environment limitation.
Resolution: the tier was executed on the Principal's 32-CPU host on
2026-09-17 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md#trsr-svr1-rfa-svr1-1" target="_blank">RFA-SVR1-1</a>) with exit 0 and the same manifest hash as every
other run. <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a> Verified.

### <a id="str1-4-5"></a>4.5 <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> Replicability (amended 2026-09-17)

(a) Invariance: 4 concurrent containers at 1 core / 2 GB on the same
input; all exit 0; all four manifests equal the baseline SHA-256. As
expected.

(b) Throughput: 4 document sets (copies of the reference set). Sandbox
host: 29.68 s sequential, 7.53 s concurrent, speedup 3.94. Principal's
host: 18.93 s sequential, 4.74 s concurrent, speedup 3.99. Every concurrent
output equals its sequential counterpart on both hosts. As expected:
replication improves throughput without affecting results. (A first pass
on the Principal's host immediately after the image build showed 1.61×
under Docker Desktop's post-build housekeeping; the committed evidence is
the second pass.)

### <a id="str1-4-6"></a>4.6 Inspections

| Procedure | Result | Evidence |
| --- | --- | --- |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-240" target="_blank">TP-240</a> core path has no third-party or Large Language Model (LLM) dependency | pass | `dotnet list src/Naadap.Core/Naadap.Core.csproj package`: "No packages were found for this framework" |
| Reference graph equals Systems Engineering Management Plan (SEMP) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-table-3-2-3" target="_blank">Table 3.2-3</a> | pass | `dotnet list reference` per project: Ingestion, Output, LlmStep, Core reference Core only; Cli references Ingestion, Core, Output, LlmStep; Alternative references Core, Ingestion, Output and is referenced by nothing |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-500" target="_blank">TP-500</a> no run-time dependency fetch | pass | Runtime image is `mcr.microsoft.com/dotnet/runtime:9.0` with published output only; every container run used `--network none` and completed |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-510" target="_blank">TP-510</a> zero outbound connections by default | pass | Same runs, `--network none`, exit 0, full bundle written |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-530" target="_blank">TP-530</a> deployment configuration never above 8 cores / 16 GB | pass | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docker-compose.yml" target="_blank"><code>docker-compose.yml</code></a> limits: 1.0/2G, 4.0/8G, 8.0/16G, 4.0/8G |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-920" target="_blank">TP-920</a> every third-party package justified | pass | `dotnet list Naadap.sln package`: PdfPig 0.1.16, DocumentFormat.OpenXml 3.5.1 (ingestion), xunit 2.9.2, xunit.runner.visualstudio 2.8.2, Microsoft.NET.Test.Sdk 17.12.0, coverlet.collector 6.0.2 (tests); each with a justification comment in its `.csproj` and a row in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a> |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-950" target="_blank">TP-950</a> knowledge-base schema and Extract, Transform, Load (ETL) documentation | pass | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/KB_SCHEMA.md" target="_blank"><code>docs/KB_SCHEMA.md</code></a> has the file table, the field table with per-row-kind sources, the ETL diagram naming the scripts, and the coverage figure; `python3 scripts/kb/build_kb.py` on the committed inputs leaves `git status` clean |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-900" target="_blank">TP-900</a> / <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-930" target="_blank">TP-930</a> clean-clone build and run | see <a href="#str1-5" target="_blank">§5</a> | Clean clone of the candidate commit built, tested, and run on the smoke set |

### <a id="str1-4-7"></a>4.7 <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-910" target="_blank">TP-910</a> Visual Studio / Windows

As expected. Executed on the candidate commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a> on 2026-09-17:
`windows-verification` run <a href="https://github.com/HoloSim-Interactive/NAADAP/actions/runs/35178944747" target="_blank">35178944747</a> (dispatched manually on an
`issue-svr1` copy of the branch head because the push's head commit touched
only documentation and the workflow's path filter did not fire): Software Development Kit (SDK)
resolved from `global.json`, Release build, tests on Windows, publish of
every executable project with launch, self-contained publish; all
succeeded. `build-and-test` run <a href="https://github.com/HoloSim-Interactive/NAADAP/actions/runs/35178897168" target="_blank">35178897168</a> also built and tested on
windows-latest and ubuntu-latest. The Windows test job includes the
knowledge-base integrity tests, which confirms the `.gitattributes` pin
holds on a Windows checkout. Evidence:
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/evidence/tp-910-hosted-workflows-2026-09-17.json" target="_blank"><code>evidence/tp-910-hosted-workflows-2026-09-17.json</code></a>.

## <a id="str1-5"></a>5. Test log

| Date, time (Coordinated Universal Time, UTC) | Activity | Configuration | Performed by |
| --- | --- | --- | --- |
| 2026-09-17 01:09 | Automated suite, 90 tests | `dotnet test -c Release`, .NET SDK 9.0.317, Ubuntu, commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/7783c39" target="_blank"><code>7783c39</code></a> and again at <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/a2885d9" target="_blank"><code>a2885d9</code></a> | Test Engineer role |
| 2026-09-17 01:16–01:20 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a>, 20 runs | `Naadap.Cli.dll` in process, commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/a2885d9" target="_blank"><code>a2885d9</code></a>, Knowledge Base (KB) `2026-09-17.1` | Test Engineer role |
| 2026-09-17 01:24 | Image `naadap:svr1` built from `dotnet publish` output on `mcr.microsoft.com/dotnet/runtime:9.0`; smoke run on <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/tests/fixtures/smoke/" target="_blank"><code>tests/fixtures/smoke</code></a> exit 0 | Docker 29.3.1, daemon started in the verification container | Test Engineer role |
| 2026-09-17 01:24–01:26 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a> (two tiers), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> (a) and (b) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/scripts/tp/run_resource_tests.py" target="_blank"><code>scripts/tp/run_resource_tests.py</code></a>, docker mode, `--network none` | Test Engineer role |
| 2026-09-17 01:27 | Inspections <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-240" target="_blank">TP-240</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-500" target="_blank">TP-500</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-510" target="_blank">TP-510</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-530" target="_blank">TP-530</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-920" target="_blank">TP-920</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-950" target="_blank">TP-950</a> | as above | Systems Engineer role |
| 2026-09-17 01:30 | Clean-clone build, test, and smoke run (Physical Configuration Audit 1 (PCA-1) rehearsal) | `git clone` of the branch head into an empty directory; `dotnet build`, `dotnet test`, `dotnet run` | Continuous Integration / Continuous Delivery (CI/CD) role |
| 2026-09-17 03:38–03:41 Coordinated Universal Time (UTC) | Hosted workflows on `issue-svr1` at <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a>: build-and-test (ubuntu-latest, windows-latest), windows-verification (dotnet-verify) | GitHub-hosted runners | CI/CD role, dispatched by the Systems Engineer on the Principal's instruction |
| 2026-09-17 (Principal's local time; evidence timestamp 03:29 UTC 2026-09-17) | Official `docker build` at <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/4127c5a" target="_blank"><code>4127c5a</code></a>; smoke run exit 0; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a> all three tiers, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> (a) and (b) | Windows 11 Pro, 32 logical processors, 128 GB; Docker Desktop 29.8.0, Windows Subsystem for Linux (WSL) 2 (32 CPUs, 62.79 GiB); image manifest `sha256:58f42958…` | Principal |

Witnesses: none; every activity is reproducible from the repository and
the evidence files.

## <a id="str1-6"></a>6. Notes

Abbreviations used in this document, each spelled out at its first use in the body (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-11" target="_blank">SEMP §3.2.11</a>, documentation conventions). The program's global list is <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-appendix-a" target="_blank">SEMP Appendix A</a>.

| Abbreviation | Expansion |
| --- | --- |
| CDRL | Contract Data Requirements List |
| CI/CD | Continuous Integration / Continuous Delivery |
| CPU | Central Processing Unit |
| ETL | Extract, Transform, Load |
| FCA | Functional Configuration Audit |
| GFI | Government Furnished Information |
| IL4 | Impact Level 4 |
| KB | Knowledge Base |
| LLM | Large Language Model |
| NAVAIR | Naval Air Systems Command |
| NFR | Non-Functional Requirement |
| PCA | Physical Configuration Audit |
| RFA | Request for Action |
| RTVM | Requirements Traceability and Verification Matrix |
| SDK | Software Development Kit |
| SEMP | Systems Engineering Management Plan |
| SVR | System Verification Review |
| TP | Test Procedure |
| UTC | Coordinated Universal Time |
| WSL | Windows Subsystem for Linux |
