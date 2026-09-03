# NAADAP Validation Methodology

<!--
Owned by the Software Engineer, embedded verbatim into the
Naadap.Output assembly (see src/Naadap.Output/Naadap.Output.csproj) and
copied into every run's output bundle as `validation-methodology.md`,
satisfying OUT-430's "accompanies every run's output" requirement while
also existing here as a standalone, directly readable static submission
document (OUT-430's other satisfying form). This is the single source
of that text -- do not maintain a second copy.
-->

This document describes how NAADAP's output is validated: the test
corpus used, how ground truth was derived for it, and the exact
definition of the summary metric (OUT-420) reported with every run.

## Test corpus

Validation uses the fixed **N=20 reference set**
(`tests/fixtures/reference-20/`), the size `docs/RTVM.md`'s "Reference
scale for test data" specifies. Every document in it is a real, public
Navy/NAVFAC/NAVSEA/NAVSUP solicitation attachment (SOW, PWS, CDRL,
sources-sought notice) sourced from SAM.gov's public opportunities
API, plus one open-source-text sample (Congressional testimony on the
V-22 program) from GovInfo's Congressional Hearings collection. As
U.S. Government works, none of these carry a copyright restriction on
redistribution (17 U.S.C. § 105). Full per-document provenance --
which SAM.gov opportunity each file came from and why it was picked --
is recorded in `tests/fixtures/README.md`.

A smaller `tests/fixtures/smoke/` set (6 documents + 1 deliberately
corrupted file) exercises DATA-IN-100/110's ingestion behavior, and a
synthetic, hand-authored `tests/fixtures/synthetic-core200/` set (not
sourced from SAM.gov, by design) exercises CORE-200's clustering
algorithm against an unambiguous, constructed answer. Neither of those
two sets carries ground truth for the recommendation-accuracy metric
below -- only `reference-20/` does.

## Ground-truth derivation

`tests/fixtures/reference-20/ground-truth.json` maps each of the 20
documents to one of four candidate contract vehicles
(`SEAPORT-NXG`, `NAVFAC-MACC-JOC`, `SBRAC`, `GSA-MAS`). This mapping
was derived by inspection, not fabricated or reverse-engineered from
the pipeline's own output:

1. Read each document's title, scope section, and issuing office.
2. Grouped documents whose stated scope matches a real, named
   acquisition vehicle's actual purpose (e.g., an environmental
   remediation SOW groups with SBRAC, the vehicle NAVFAC Pacific
   actually uses for that work).
3. Where no single named vehicle obviously fit (the title/escrow
   documents), the closest general-purpose vehicle (GSA MAS) was
   chosen over inventing a vehicle name -- recorded as an inference,
   not a certainty, in that document's `rationale` field.
4. Four vehicles emerged from this process, not a predetermined count
   -- fewer than the "top-5" language in the metric below might
   suggest; see `precisionAt5Note` in the ground-truth file for how
   that is scored.

Full per-document rationale is recorded in
`tests/fixtures/reference-20/ground-truth.json` and
`tests/fixtures/README.md`'s "Ground truth derivation methodology".
Ground truth is used **only** to score the pipeline's output after the
fact -- it is never read by the clustering or ranking code
(`Naadap.Core`/`Naadap.Output.VehicleRecommender`), which stays
non-supervised per CORE-200/CORE-240.

## Metric definition (OUT-420): precision@5

Computed by `Naadap.Output.MetricCalculator`, against a ground-truth
file (the `reference-20/ground-truth.json` shape above) only when one
is found in the run's input directory. A real, unlabeled production
input set has no such file -- that is expected, not an error -- and
the metric is reported as **not computed** for that run (`value: null`
in `metric.json`), with `Definition` stating why. Precision@5 has no
meaningful denominator without a ground-truth mapping to check
against.

When ground truth **is** available, precision@5 is computed as
follows:

1. Take the top `min(5, candidates produced)` ranked
   `CandidateVehicle` entries from DATA-OUT-300's output.
2. For each, find the ground-truth vehicle held by a majority of its
   `ContributingDocuments` (documents absent from the ground-truth
   file are ignored for this vote).
3. A candidate counts as **correct** if it has a majority ground-truth
   vehicle *and* that real vehicle has not already been credited to a
   higher-ranked candidate in this run (each real vehicle can only be
   matched once, so duplicate/split clusters covering the same real
   vehicle cannot inflate the score).
4. `precision@5 = correct / min(5, candidates produced)`. Per
   `reference-20/ground-truth.json`'s `precisionAt5Note`: with only
   four real vehicles in this reference set, a fifth predicted
   candidate (if one exists) cannot itself be correct and therefore
   still counts against the denominator, exactly like standard
   precision@k against a shorter true-positive set.

`CorrectCount` and `TotalCount` (the raw counts OUT-420 requires
alongside the headline number) are exactly the numerator and
denominator above.
