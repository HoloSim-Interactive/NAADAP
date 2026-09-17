# Demonstration runbook — NAADAP Increment 1

| Field | Value |
| --- | --- |
| Purpose | A script for demonstrating the Increment 1 product baseline live, on short notice or inside the Phase 3 presentation (Systems Engineering Management Plan (SEMP) risk <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-r-6" target="_blank">R-6</a>) |
| Product demonstrated | Commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a> (Increment 1 product baseline, accepted 2026-09-17), knowledge base `2026-09-17.1` |
| Rehearsed | 2026-09-17 on the verification host: 5.96 s wall-clock on the reference set, output byte-identical to <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/reference-run/" target="_blank"><code>docs/reference-run/</code></a> (manifest SHA-256 `ad331596…`) |
| Prepared by | Systems Engineer |

## 1. What the tool does, in one paragraph

NAADAP reads a directory of acquisition documents (PDF and DOCX: statements of work, performance work statements, Contract Data Requirements Lists (CDRLs), solicitations, sources-sought notices, requests for information), groups documents that share requirement content, ranks the groups by how strongly they cohere, and for each group lists the strategic contract vehicles in a shipped knowledge base that could absorb the group, with the evidence for each candidate, the candidates that fell below the evidence floor, and the vehicles ruled out by a hard constraint. It runs as one non-interactive command, in a container with no network, in about six seconds for twenty documents at the lowest scored resource tier (1 core, 2 GB). It identifies suitable vehicles; it does not pick one.

## 2. What it can and cannot do today

| Capability | Status | Where the evidence is |
| --- | --- | --- |
| Ingest PDF and DOCX; skip unreadable or unsupported files without aborting | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-100" target="_blank">DATA-IN-100</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-110" target="_blank">DATA-IN-110</a>) | `manifest.json` → `skippedFiles` names the file and the reason |
| Group documents by shared requirement content (Term Frequency–Inverse Document Frequency (TF-IDF) cosine clustering, threshold 0.35) | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-200" target="_blank">CORE-200</a>) | `method-visualization.md` |
| Rank groups by cohesion; single-document groups score 0.0 | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-out-300" target="_blank">DATA-OUT-300</a>; gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a>) | `result-visualization.md` first table |
| Recommend strategic vehicles per group from the knowledge base (484 vehicles, 11 families, NAVAIR FY2025 orders), with lexical and office-affinity evidence, tier tie-break, evidence floor, eliminations by hard constraint, and a "new vehicle indicated" flag | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a>; gates G5, G6) | `result-visualization.md` per-cluster sections; `vehicle-ranking.tsv` full ranking |
| Summary metric with raw counts (precision@5 against the fixture's ground truth) | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a>) | `manifest.json` → `summaryMetric` |
| Deterministic to the byte across runs, hosts, and operating systems | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a>, 20 of 20 runs; Ubuntu and Windows) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/STR-increment-1.md#str1-4-2" target="_blank">STR §4.2</a> |
| Runs inside every scored resource tier with identical output; four replicas give a 3.9× throughput gain | Verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/STR-increment-1.md#str1-4-4" target="_blank">STR §4.4</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/STR-increment-1.md#str1-4-5" target="_blank">§4.5</a> |
| Knowledge base integrity check at load; exit code 2 on tamper | Verified (KB-630) | `manifest.json` → `knowledgeBaseManifestSha256` |
| Optional Large Language Model (LLM) summarization step, off by default, allowlisted endpoint only | Implemented (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a>); not demonstrated | Do not enable in a demonstration without a reachable, allowlisted endpoint |
| Vehicle-level accuracy against a NAVAIR ground truth | **Not possible yet.** The reference set is NAVFAC, NAVSEA, and NAVSUP material; the knowledge base is NAVAIR. The ground-truth vehicles cannot appear by name | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/KB_SCHEMA.md" target="_blank"><code>docs/KB_SCHEMA.md</code></a>, "Coverage and limits" |
| Learned scoring, outcome linkage, promotion of an existing contract to a vehicle | Increment 2 (gates G2, G3) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank">design document</a> |
| Any user interface beyond the command line | None; UI-001 specifies a single non-interactive invocation | — |

## 3. Pre-flight (the morning of)

1. Clone or pull the baseline: `git clone https://github.com/HoloSim-Interactive/NAADAP.git && cd NAADAP && git checkout v1.0.156` (commit `0f5b2b3`; until the tag is pushed, `git checkout 0f5b2b3`).
2. Build the image: `docker build -t naadap:demo .` (about two minutes on the Principal's host). Confirm `docker image inspect naadap:demo --format '{{.RootFS.Layers}}'` prints layers; the reproducible manifest digest is recorded in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SVD-increment-1.md#svd1-3-1" target="_blank">SVD §3.1</a>.
3. Stage inputs: copy `tests/fixtures/reference-20/` to a directory you can point at (say `C:\demo\in`), and create an empty `C:\demo\out`.
4. Dry run once, exactly as in §4 step 2, and diff `manifest.json` against `docs/reference-run/manifest.json`. They must be identical. If they are not, stop and use the committed bundle (§6).
5. Open, in browser tabs: the repository README, `docs/reference-run/result-visualization.md` (rendered), and `docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md`.

## 4. The demonstration (12 minutes live, fits the 30-minute presentation with the timed run inside it)

**Minute 0–2. The problem and the shape of the answer.** One slide or one sentence: a procurement group receives requirement documents from many programs; the question is which of them describe the same need and which existing strategic vehicles could carry that need. Show the input directory: twenty public Navy documents and one JSON file the tool will refuse.

**Minute 2–4. The run.** In a terminal:

```powershell
docker run --rm --network none --cpus=1 --memory=2g `
  -v C:\demo\in:/data/in:ro -v C:\demo\out:/data/out `
  naadap:demo --input /data/in --output /data/out
echo $LASTEXITCODE
```

Say while it runs (about six seconds; the constrained tier is the point): no network, one core, two gigabytes, nothing fetched at run time, exit code 0. Then `dir C:\demo\out`: five files.

**Minute 4–6. The method.** Open `method-visualization.md`. Twenty ingested, one skipped and why, fourteen clusters, the top terms per cluster. Point at cluster-0002 (three NSWC Carderock CDRL and inspection documents) and cluster-0004 (four documents that share contracting boilerplate): the second is the honest example of a lexical method grouping on shared language rather than shared need.

**Minute 6–9. The result.** Open `result-visualization.md`. First table: fourteen candidates ranked by cohesion, the three multi-document groups first, eleven singletons at 0.0 (say why: a group of one has no agreement to measure, and the tool says so rather than scoring it perfect). Then the per-cluster vehicle sections. For cluster-0004 show the full account: the requesting office N62742 was found in the documents; 473 vehicles were ruled out because that office has never ordered under them in FY2025; four candidates survived with their lexical and tier evidence; seven fell below the evidence floor and are still listed. The wording to use: the tool identifies vehicles whose observed scope and ordering history are compatible with the group; the contracting officer decides.

**Minute 9–10. The metric and the audit trail.** Open `manifest.json` in the browser. `summaryMetric`: precision@5 = 0.60, 3 of 5, with the definition text. `knowledgeBaseVersion` and `knowledgeBaseManifestSha256`: the knowledge base is versioned and hash-checked at load. Show `vehicle-ranking.tsv` scrolling: every vehicle, every cluster, rank, score, channels, tier, status.

**Minute 10–12. Determinism and the evidence.** Run the same command a second time into `C:\demo\out2` and compare:

```powershell
Get-FileHash C:\demo\out\manifest.json, C:\demo\out2\manifest.json | Format-Table Hash
```

Identical hashes. Then one slide or the SVR-1 record in a tab: verified twenty of twenty byte-identical runs, all three resource tiers, Ubuntu and Windows hosted runners, 3.9× throughput with four replicas.

## 5. Questions to expect, and the answers on record

| Question | Answer |
| --- | --- |
| Why are the recommended vehicles NAVAIR vehicles for NAVFAC documents? | The knowledge base is built from NAVAIR's FY2025 orders because the challenge sponsor is NAVAIR and the Government furnished corpus is NAVAIR's. The public reference set is what could be obtained without that corpus. Increment 2 widens the extract (SVD §3.7). |
| Is 0.60 good? | It is the cluster-level precision at five against a twenty-document public set, and it is reported with raw counts so nobody has to trust the decimal. The alternative approach scores 0.80 on the same set and the choice between them is an Increment 2 decision on the Increment 2 metric (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank">algorithm comparison</a>). |
| Where is the machine learning? | Increment 1 is deliberately deterministic retrieval with an explicit evidence record; a fitted scoring model is designed for Increment 2 and will be trained on FPDS outcomes, not on the documents. |
| Can it run in our cloud? | It is a standard Open Container Initiative (OCI) image, no ports, no credentials, no network, self-contained; the resource limits are the platform's own (`docs/DEPLOYMENT.md`). |
| What if a document is classified or CUI? | Nothing leaves the container; there is no telemetry. Handling rules for Government furnished material are in SEMP §3.2.11. |
| Can it tell us we need a new vehicle? | Yes: `newVehicleIndicated` is set when no candidate clears the evidence floor, and the below-floor list still shows the nearest vehicles. |

## 6. If the live run cannot be done

The committed bundle in `docs/reference-run/` is the output of the same commit on the same input, verified byte-identical on two hosts and two operating systems. Present it in the same order as §4 from the rendered files on GitHub, and show the SVR-1 record's verification table in place of the timed run. Say plainly that the run is being shown from the committed bundle and why.

## 7. Abbreviations

| Abbreviation | Expansion |
| --- | --- |
| CDRL | Contract Data Requirements List |
| CUI | Controlled Unclassified Information |
| FPDS | Federal Procurement Data System |
| FY | Fiscal Year |
| LLM | Large Language Model |
| NAVAIR | Naval Air Systems Command |
| NAVFAC | Naval Facilities Engineering Systems Command |
| NAVSEA | Naval Sea Systems Command |
| NAVSUP | Naval Supply Systems Command |
| NSWC | Naval Surface Warfare Center |
| OCI | Open Container Initiative |
| PCA | Physical Configuration Audit |
| SEMP | Systems Engineering Management Plan |
| STR | Software Test Report |
| SVD | Software Version Description |
| SVR | System Verification Review |
| TF-IDF | Term Frequency–Inverse Document Frequency |
| TP | Test Procedure |
| UI | User Interface |
