# Software Test Report — NAADAP Increment 1, SVR-1 / FCA-1

| Field | Value |
| --- | --- |
| Data items | CDRL A013, DI-IPSC-81440A Software Test Report; CDRL A018, DI-NDTI-80809B Test/Inspection Report |
| Software tested | `Naadap.sln` at commit `a2885d9` (2026-09-17), knowledge base `2026-09-17.1` |
| Test procedures | `docs/RTVM.md` Test Procedures (TP-nnn); `docs/VALIDATION_METHODOLOGY.md` |
| Prepared by | Systems Engineer from Test Engineer executions, 2026-09-17; finalized at SVR-1 exit |
| Evidence | `docs/setr/evidence/` (machine-readable results), `docs/reference-run/` (output bundle) |
| Classification | UNCLASSIFIED |

## 1. Scope

### 1.1 Identification

NAADAP, `Naadap.sln`, six assemblies, version 1.0, candidate for the
Increment 1 submission tag (`docs/setr/SVD-increment-1.md` §1.1).

### 1.2 System overview

See `docs/SDD.md` §System overview.

### 1.3 Document overview

This report records the results of every test procedure executed for
Increment 1 verification: the automated suite, the reproducibility,
runtime, resource-tier, and replication procedures, and the inspections.
It states the deviations from the procedures as written and their effect
on validity. It contains no controlled or personal information.

## 2. Referenced documents

`docs/RTVM.md` (requirements and test procedures); `docs/VALIDATION_METHODOLOGY.md`;
`docs/setr/SEMP.md` §3.2.13 (SVR-1 entry and exit criteria); `docs/DEPLOYMENT.md`
(resource tiers); `docs/KB_SCHEMA.md`; DI-IPSC-81440A (2000-01-11);
DI-NDTI-80809B (1997-01-24).

## 3. Overview of test results

### 3.1 Overall assessment

a. The software meets every Increment 1 requirement whose procedure could
be executed in the verification environment. The automated suite passes
(90 of 90). The pipeline is deterministic to the byte across 20 runs
(TP-210). It completes the reference set in 7.4 seconds at 1 core / 2 GB
(TP-220, limit 30 minutes, design target 5 minutes). Four concurrent
1-core replicas reproduce the baseline output exactly and process four
document sets 3.9 times faster than one replica in sequence (TP-520).
Every inspection passes (TP-240, TP-500, TP-510, TP-530, TP-920, TP-950).

b. Remaining limitations detected by testing:

| Item | Impact | Design impact | Recommended approach |
| --- | --- | --- | --- |
| ~~TP-230's 8-core / 16 GB tier could not be executed on the 4-CPU verification host~~ **Resolved 2026-09-17 (RFA-SVR1-1):** all three tiers executed by the Principal on a 32-CPU / 63 GiB Docker Desktop host against the official image; exit 0 and manifest SHA-256 `ad331596…` at every tier (§4.4) | None remaining; CORE-230 Verified | None | Closed |
| Peak memory was not sampled inside the containers | The 2 GB cap was enforced by the kernel and no run was killed, which is the requirement; the numeric peak is not recorded | None | Add `docker stats` sampling to `scripts/tp/run_resource_tests.py` |
| ~~The container image under test was built from a local `dotnet publish` onto the official runtime base~~ **Resolved 2026-09-17 (RFA-SVR1-3):** the Principal built the official multi-stage image with `docker build -t naadap:svr1 .` at commit `4127c5a` (restore and publish stages completed; image manifest `sha256:58f42958…`, config `sha256:eafe57f1…`) and every §4.3–4.5 result below was reproduced on it | None remaining | None | Closed |
| ~~DELIV-910 (Visual Studio / Windows) not run on the candidate~~ **Resolved 2026-09-17 (RFA-SVR1-2):** `windows-verification` run 35178944747 and `build-and-test` run 35178897168 (windows-latest job) succeeded on `261f7e3`: Release build, 90 tests, framework-dependent and self-contained publish on Windows | None remaining; DELIV-910 Verified | None | Closed |

### 3.2 Impact of test environment

Tests ran on a 4 vCPU / 16 GB Ubuntu host inside the development
container, with a Docker daemon started for the tier and replication
procedures. Differences from the operational environment (a Government
IL4 cloud): the host CPU is faster than the 1-core tier's likely hardware,
so absolute times are optimistic by an unknown factor; the 30-minute limit
leaves a margin of more than two hundred times, so the conclusion does not
depend on the factor. Network policy: containers ran with `--network none`,
which is stricter than the operational environment and is the condition
NFR-510 requires.

### 3.3 Recommended improvements

1. Sample peak memory in the resource-test runner.
2. Add an `hit@k` measure of vehicle recommendations against a NAVAIR
ground truth once GFI arrives; the present metric scores cluster
composition, not vehicle identity (`docs/VALIDATION_METHODOLOGY.md`).
3. Reduce `manifest.json` size by moving eliminations to the ranking file
in a compact mode (Software Version Description §3.7).

## 4. Detailed test results

Test identifiers are the RTVM's. "As expected" means every step produced
the expected result with no deviation.

### 4.1 Automated suite (TP-001, TP-100, TP-110, TP-120, TP-200, TP-250, TP-300, TP-400, TP-410, TP-420, TP-430, TP-440, TP-500, TP-510 component tests, plus the knowledge-base and matcher tests)

| Assembly | Tests | Result |
| --- | --- | --- |
| Naadap.Ingestion.Tests | 16 | passed |
| Naadap.Core.Tests | 14 | passed |
| Naadap.Output.Tests | 26 | passed (10 added 2026-09-17: knowledge-base integrity and content, matcher rules) |
| Naadap.Cli.Tests | 12 | passed |
| Naadap.LlmStep.Tests | 14 | passed |
| Naadap.Alternative.Tests | 8 | passed |

Summary: all results as expected. Problems encountered: none. Deviations: none.

### 4.2 TP-210 Reproducibility

Summary: as expected. 20 runs of the reference set in process on the host;
1 distinct top-5 cluster list, 1 distinct set of per-cluster vehicle
candidate lists, 1 distinct `manifest.json` SHA-256
(`ad3315960a32657b…`), which also equals the committed
`docs/reference-run/manifest.json` and every container run below.
Requirement: identical in 19 of 20; achieved 20 of 20. Evidence:
`evidence/tp-210-results-2026-09-17.json`.

### 4.3 TP-220 Runtime at 1 core / 2 GB

Summary: as expected. Container `naadap:svr1`, `--cpus=1 --memory=2g
--network none`, reference set: exit 0, wall-clock 7.43 s from `docker run`
to exit. Limit 1,800 s; design target 300 s. Evidence:
`evidence/tp-220-230-520-results-2026-09-17.json`.

### 4.4 TP-230 Resource tiers

Two executions. First, the sandbox host (4 vCPU / 16 GB, image built from local publish); second, the Principal's host (32 CPU / 63 GiB Docker Desktop, official `docker build`). Evidence: `evidence/tp-220-230-520-results-2026-09-17.json` and `evidence/tp-220-230-520-results-2026-09-18-principal.json`.

| Tier | Sandbox: exit, wall-clock | Principal: exit, wall-clock | Manifest SHA-256 (both) | Status |
| --- | --- | --- | --- | --- |
| 1 core / 2 GB | 0, 7.43 s | 0, 4.38 s | `ad331596…` | as expected |
| 4 cores / 8 GB | 0, 5.82 s | 0, 3.94 s | `ad331596…` | as expected |
| 8 cores / 16 GB | not run (4-CPU host) | 0, 3.95 s | `ad331596…` | as expected on the Principal's host; see 4.4.1 |

4.4.1 Deviation, sandbox execution of tier 8c/16GB: the sandbox host has
4 CPUs and Docker rejects `--cpus=8`. Rationale: environment limitation.
Resolution: the tier was executed on the Principal's 32-CPU host on
2026-09-17 (RFA-SVR1-1) with exit 0 and the same manifest hash as every
other run. CORE-230 Verified.

### 4.5 TP-520 Replicability (amended 2026-09-17)

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

### 4.6 Inspections

| Procedure | Result | Evidence |
| --- | --- | --- |
| TP-240 core path has no third-party or LLM dependency | pass | `dotnet list src/Naadap.Core/Naadap.Core.csproj package`: "No packages were found for this framework" |
| Reference graph equals SEMP Table 3.2-3 | pass | `dotnet list reference` per project: Ingestion, Output, LlmStep, Core reference Core only; Cli references Ingestion, Core, Output, LlmStep; Alternative references Core, Ingestion, Output and is referenced by nothing |
| TP-500 no run-time dependency fetch | pass | Runtime image is `mcr.microsoft.com/dotnet/runtime:9.0` with published output only; every container run used `--network none` and completed |
| TP-510 zero outbound connections by default | pass | Same runs, `--network none`, exit 0, full bundle written |
| TP-530 deployment configuration never above 8 cores / 16 GB | pass | `docker-compose.yml` limits: 1.0/2G, 4.0/8G, 8.0/16G, 4.0/8G |
| TP-920 every third-party package justified | pass | `dotnet list Naadap.sln package`: PdfPig 0.1.16, DocumentFormat.OpenXml 3.5.1 (ingestion), xunit 2.9.2, xunit.runner.visualstudio 2.8.2, Microsoft.NET.Test.Sdk 17.12.0, coverlet.collector 6.0.2 (tests); each with a justification comment in its `.csproj` and a row in `docs/DEPENDENCIES.md` |
| TP-950 knowledge-base schema and ETL documentation | pass | `docs/KB_SCHEMA.md` has the file table, the field table with per-row-kind sources, the ETL diagram naming the scripts, and the coverage figure; `python3 scripts/kb/build_kb.py` on the committed inputs leaves `git status` clean |
| TP-900 / TP-930 clean-clone build and run | see §5 | Clean clone of the candidate commit built, tested, and run on the smoke set |

### 4.7 TP-910 Visual Studio / Windows

As expected. Executed on the candidate commit `261f7e3` on 2026-09-17:
`windows-verification` run 35178944747 (dispatched manually on an
`issue-svr1` copy of the branch head because the push's head commit touched
only documentation and the workflow's path filter did not fire): SDK
resolved from `global.json`, Release build, tests on Windows, publish of
every executable project with launch, self-contained publish; all
succeeded. `build-and-test` run 35178897168 also built and tested on
windows-latest and ubuntu-latest. The Windows test job includes the
knowledge-base integrity tests, which confirms the `.gitattributes` pin
holds on a Windows checkout. Evidence:
`evidence/tp-910-hosted-workflows-2026-09-17.json`.

## 5. Test log

| Date, time (UTC) | Activity | Configuration | Performed by |
| --- | --- | --- | --- |
| 2026-09-17 01:09 | Automated suite, 90 tests | `dotnet test -c Release`, .NET SDK 9.0.317, Ubuntu, commit `7783c39` and again at `a2885d9` | Test Engineer role |
| 2026-09-17 01:16–01:20 | TP-210, 20 runs | `Naadap.Cli.dll` in process, commit `a2885d9`, KB `2026-09-17.1` | Test Engineer role |
| 2026-09-17 01:24 | Image `naadap:svr1` built from `dotnet publish` output on `mcr.microsoft.com/dotnet/runtime:9.0`; smoke run on `tests/fixtures/smoke` exit 0 | Docker 29.3.1, daemon started in the verification container | Test Engineer role |
| 2026-09-17 01:24–01:26 | TP-220, TP-230 (two tiers), TP-520 (a) and (b) | `scripts/tp/run_resource_tests.py`, docker mode, `--network none` | Test Engineer role |
| 2026-09-17 01:27 | Inspections TP-240, TP-500, TP-510, TP-530, TP-920, TP-950 | as above | Systems Engineer role |
| 2026-09-17 01:30 | Clean-clone build, test, and smoke run (PCA-1 rehearsal) | `git clone` of the branch head into an empty directory; `dotnet build`, `dotnet test`, `dotnet run` | CI/CD role |
| 2026-09-17 03:38–03:41 UTC | Hosted workflows on `issue-svr1` at `261f7e3`: build-and-test (ubuntu-latest, windows-latest), windows-verification (dotnet-verify) | GitHub-hosted runners | CI/CD role, dispatched by the Systems Engineer on the Principal's instruction |
| 2026-09-17 (Principal's local time; evidence timestamp 03:29 UTC 2026-09-17) | Official `docker build` at `4127c5a`; smoke run exit 0; TP-220, TP-230 all three tiers, TP-520 (a) and (b) | Windows 11 Pro, 32 logical processors, 128 GB; Docker Desktop 29.8.0, WSL 2 (32 CPUs, 62.79 GiB); image manifest `sha256:58f42958…` | Principal |

Witnesses: none; every activity is reproducible from the repository and
the evidence files.

## 6. Notes

CPU, central processing unit; FCA, functional configuration audit; GFI,
Government furnished information; IL4, Impact Level 4; KB, knowledge base;
LLM, large language model; PCA, physical configuration audit; SDK,
software development kit; SVR, system verification review; TP, test
procedure.
