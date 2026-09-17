# Reference run: output bundle for the N=20 reference set

A committed copy of the OUT-440 output bundle produced by running the
pipeline on `tests/fixtures/reference-20/`, so an evaluator can read the
visualizations, the metric, and the vehicle recommendations (Phase 2
package items 7, 8, and 9) without building first. It is regenerated at
each SETR verification event and the commit that produced it is recorded
below; any difference between this directory and a fresh run of the same
commit is a defect.

| Item | Value |
| --- | --- |
| Command | `dotnet src/Naadap.Cli/bin/Release/net9.0/Naadap.Cli.dll --input tests/fixtures/reference-20 --output docs/reference-run` |
| Code commit | `a2885d9` (2026-09-17) |
| Knowledge base | `2026-09-17.1` (`knowledgeBaseVersion` and `knowledgeBaseManifestSha256` in `manifest.json`) |
| Input | 20 public documents plus `ground-truth.json` (skipped by ingestion as an unsupported type, as in every run against this directory) |
| Summary metric | precision@5 = 0.60 (3 of 5), `manifest.json` → `summaryMetric` |

| File | Package item | Content |
| --- | --- | --- |
| `manifest.json` | 9 (metric); index of 7 and 8 | Ranked cluster candidates with cohesion scores and contributing documents; per-cluster strategic-vehicle recommendations with per-channel evidence, below-floor candidates, and eliminations; the summary metric with raw counts; skipped files; knowledge-base version and manifest hash |
| `method-visualization.md` | 7 | The clustering method as run: documents in, skipped, clusters formed |
| `result-visualization.md` | 8 | The ranked cluster list and, per cluster, the vehicle candidates, the below-floor candidates, and the vehicles ruled out with the constraint named |
| `vehicle-ranking.tsv` | 8 (full account) | Every knowledge-base vehicle for every cluster: rank, score, channel values, tier, status |
| `validation-methodology.md` | 10 | Copy of `docs/VALIDATION_METHODOLOGY.md` embedded at build |

Reading note for the vehicle recommendations: the reference set is drawn
from NAVFAC, NAVSEA, and NAVSUP solicitations and Congressional testimony,
while the knowledge base is built from NAVAIR's FY2025 orders
(`docs/KB_SCHEMA.md`, "Coverage and limits"). The vehicle candidates for
these clusters are therefore NAVAIR vehicles whose scope text resembles the
documents, and the ground truth's NAVFAC vehicles cannot appear by name.
The GFI corpus is NAVAIR's own and is the population the knowledge base was
built for.
