# NAADAP

NAADAP ("NAVAIR Acquisition Analysis & Decision Aid Pipeline" — the
project's internal shorthand for the NAVAIR/NAWCAD prize challenge it
implements) is a batch analysis pipeline that reads a directory of
procurement documents (SOWs, PWSs, CDRLs, sources-sought notices, and
open-source text such as Congressional testimony), clusters them by
shared acquisition-requirement content, and recommends candidate
strategic contract vehicles for consolidation. See
[`docs/PROJECT_DEFINITION.md`](docs/PROJECT_DEFINITION.md) for the
full mission and stakeholder needs this satisfies.

This document (DELIV-930) takes a reader with no prior exposure to
this project from a fresh clone to a completed run, using only what's
written here.

## Prerequisites

One of:

- **Docker** (recommended — this is how the deliverable is actually
  packaged and run; also satisfies SN-3's "all dependencies bundled,
  nothing fetched at runtime" requirement), or
- **.NET 9 SDK**, to build/run directly on the host without a
  container.

No other tooling is required — no database, no external service, no
network access in the default (LLM-disabled) configuration.

## Build

From a fresh clone, at the repo root:

```bash
docker compose build
```

(equivalently: `docker build -t naadap:latest .`)

or, without Docker:

```bash
dotnet build Naadap.sln
```

Both build every project in the solution, including `Naadap.Cli`, the
entrypoint. The generated `Naadap.sln`/`.csproj` files also open
directly in Visual Studio on Windows with no conversion step
(DELIV-910) — every project targets plain `net9.0`.

## Input format & location

Point the pipeline at a directory containing your documents — no
subdirectory structure, no manifest file, no per-document metadata
needed. Each file is read directly from that directory (non-recursive).

- **Supported formats:** PDF, DOCX, and plain text.
- **Naming:** no required naming convention to *run* the pipeline —
  file names are used only as a best-effort hint for content
  classification (e.g. a file whose name contains "sow" is tagged as
  a Statement of Work), with a fallback to scanning the extracted
  text if the name is inconclusive.
- **Corrupt/unsupported files:** never abort the run. Each is recorded
  in the output bundle's `manifest.json` under `skippedFiles`, with a
  human-readable reason, and the run still completes and produces
  results for everything else (DATA-IN-110).
- **Optional ground truth:** if the input directory also contains a
  `ground-truth.json` (see `tests/fixtures/reference-20/` for the
  shape), the run computes and reports a precision@5 metric against
  it. This is optional and validation-only — a real, unlabeled input
  directory simply omits it, and the metric section of the output
  explains that it wasn't computed.

### Worked example

This repo ships a small, real worked example at
[`tests/fixtures/smoke/`](tests/fixtures/smoke/) — 6 real documents
(2 SOW, 1 PWS, 1 CDRL, 1 sources-sought notice, 1 open-source text
sample, in a mix of PDF and DOCX) plus 1 deliberately corrupted PDF,
sourced as documented in
[`tests/fixtures/README.md`](tests/fixtures/README.md).

```bash
mkdir -p input output
cp tests/fixtures/smoke/*.pdf tests/fixtures/smoke/*.docx input/
docker compose run --rm naadap
```

## Launch command

```bash
docker compose run --rm naadap
```

This runs the lowest-footprint resource tier (1 core / 2GB RAM, no
network — see [`docs/DEPLOYMENT.md`](docs/DEPLOYMENT.md) for the
other resource tiers and the optional LLM-step configuration), reading
from `./input` and writing to `./output` by default (override with
`NAADAP_INPUT_DIR` / `NAADAP_OUTPUT_DIR`).

Equivalently, without Compose:

```bash
docker run --rm -v "$(pwd)/input:/data/in:ro" -v "$(pwd)/output:/data/out" \
  naadap:latest --input /data/in --output /data/out
```

or, without Docker at all:

```bash
dotnet run --project src/Naadap.Cli -- --input ./input --output ./output
```

The container/executable reads no interactive input and always exits
`0` for a well-formed invocation (safe to run with `</dev/null` or in
any non-interactive scheduler).

## Output — what a correct run produces

A completed run writes a single reviewable output bundle (OUT-440) to
the output directory:

| File | Contents |
| --- | --- |
| `manifest.json` | The bundle's index: the ranked candidate-vehicle list, paths to both visualizations below, the summary metric, a pointer to the validation-methodology document, and the list of any skipped input files with reasons. |
| `method-visualization.md` | A visualization of the clustering method/pipeline itself — how many documents went in, how many were skipped, and the clusters formed. |
| `result-visualization.md` | A visualization of the ranked candidate-vehicle list / cluster-to-vehicle mapping. |
| `validation-methodology.md` | A copy of [`docs/VALIDATION_METHODOLOGY.md`] describing the test corpus, ground-truth derivation, and metric definition — included in every run's bundle so it's readable without the source repo present. |
| `llm-run-log.json`, `llm-summary.txt` | Only present if `--enable-llm-step`/`NAADAP_LLM_ENABLED=true` was set — the optional CORE-250 step's audit log and summary text. |

Running the worked example above produces one candidate-vehicle entry
per cluster the 6 sample documents fall into, each with a score and
its contributing source document(s), plus one skipped-file entry for
the deliberately corrupted PDF (`corrupted-truncated.pdf`) with a
human-readable parse-failure reason. Because that input directory has
no `ground-truth.json`, the `summaryMetric` section explains that
precision@5 was not computed for this run rather than reporting a
number — this is expected, not an error.

To see a precision@5 score, run against the full N=20 reference set
instead, which does carry ground truth:

```bash
mkdir -p input-ref20 output-ref20
cp tests/fixtures/reference-20/*.pdf tests/fixtures/reference-20/*.docx \
   tests/fixtures/reference-20/ground-truth.json input-ref20/
NAADAP_INPUT_DIR=./input-ref20 NAADAP_OUTPUT_DIR=./output-ref20 \
  docker compose run --rm naadap
```

## Documentation index

| Document | Covers |
| --- | --- |
| [`docs/PROJECT_DEFINITION.md`](docs/PROJECT_DEFINITION.md) | Mission, stakeholders, scope. |
| [`docs/RTVM.md`](docs/RTVM.md) | Every requirement, traced to a test procedure and a stakeholder need. |
| [`docs/SDD.md`](docs/SDD.md) | System architecture, coding standards, build/toolchain conventions, SETR documentation mapping. |
| [`docs/IMPLEMENTATION_PLAN.md`](docs/IMPLEMENTATION_PLAN.md) | Build sequencing and the dependency graph between features. |
| [`docs/ALGORITHM_COMPARISON.md`](docs/ALGORITHM_COMPARISON.md) | Algorithm documentation (DELIV-970): the non-LLM core vs. the alternative (RAG-style) approach, with accuracy/runtime/cost comparison substantiating the core's selection. |
| [`docs/DEPENDENCIES.md`](docs/DEPENDENCIES.md) | Dependency documentation (DELIV-970): every third-party package and why it's there. |
| [`docs/DEPLOYMENT.md`](docs/DEPLOYMENT.md) | Deployment instructions (DELIV-970): image build, resource tiers, network policy, IL4 considerations, optional-LLM-step configuration. |
| [`docs/VALIDATION_METHODOLOGY.md`](docs/VALIDATION_METHODOLOGY.md) | Test corpus, ground-truth derivation, and metric definition (OUT-430). |
| [`docs/MAINTAINER_GUIDE.md`](docs/MAINTAINER_GUIDE.md) | How to add a new input document type or a new clustering component (DELIV-940). |
| [`tests/fixtures/README.md`](tests/fixtures/README.md) | Full provenance of every document used in testing/validation. |

## How this repo was built

NAADAP was built by a semi-automated multi-agent team (Product
Manager, Solutions Architect, Systems Engineer, Software Engineer,
Test Engineer, CI/CD) coordinated through GitHub issues. If you're
maintaining or extending this process rather than the application
itself, start at
[`KICKOFF_RUNBOOK.md`](KICKOFF_RUNBOOK.md) and
[`.github/AGENT_LABELS.md`](.github/AGENT_LABELS.md).
