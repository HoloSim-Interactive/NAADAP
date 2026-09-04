# Mapping requirement clusters to strategic vehicles, deterministically and offline

This is the step NAADAP currently lacks. The core clusters documents by
TF-IDF cosine and labels each cluster with its top terms. The judges score
agreement with a human-curated list of correct vehicle candidates. Bridging
that gap means a vendored vehicle knowledge base plus an auditable matching
step, all inside the container with no egress.

## The shape of the problem

A procurement SME deciding "could this requirement ride an existing
vehicle" asks, in this order:

1. What kind of work is it? (PSC family, DoD portfolio group, SeaPort
   functional area, OASIS+ domain.)
2. Who is buying it? (Contracting office DoDAAC, PEO/PMA, command.)
3. Is it commercial, developmental, sustainment, construction, or
   environmental? (Those route to different vehicle families outright.)
4. Which vehicles can that office order from, whose scope covers the
   work, whose ordering period covers the intended performance period,
   and whose pool matches the set-aside intent?
5. Has this office, or this command, put similar work on that vehicle
   before? (Historical task orders under the vehicle.)
6. Would consolidating these requirements onto that vehicle clear the
   FAR 7.107 benefit test and survive the small-business review?

Encode those six questions as evidence channels, not as a single score.
The output must let a contracting officer see why, because a bare score
cannot be defended in a determination or a debrief.

## Building the vehicle knowledge base

Version it, hash it, and ship it in the image. Sources are public.

For each vehicle record: identity (name, family acronym as carried in
FPDS element 6P, IDV PIIDs); scope narrative in the vehicle's own words
(SeaPort-NxG's 23 subject areas, OASIS+ domain definitions, MAS SIN
descriptions, NAWCAD MAC scope statements); ordering-period end
(`last_date_to_order` on the USAspending IDV record); ceiling and
headroom; eligible ordering activities; fee; socioeconomic pools;
security or OCONUS pools; allowed NAICS per pool (official) and the
empirical PSC and NAICS distribution of historical orders; office
affinity (orders and obligations by awarding office code); holder list
with UEIs; provenance (URL, retrieval date, hash) on every field.

Where the data comes from, in offline-friendly form:

- **USAspending bulk archive**: monthly zip files named
  `FY{yyyy}_All_Contracts_Full_{yyyymmdd}.zip` with `parent_award_id_piid`,
  `awarding_office_code` and name, `product_or_service_code`,
  `naics_code`, `type_of_set_aside`, `idv_type`, `award_description`,
  place of performance. The Referenced IDV PIID on every order is the
  authoritative "which vehicle absorbed it" label, which makes historical
  orders the natural training and validation set.
- **USAspending API** `POST /api/v2/idvs/awards/` returns every child order
  of an IDV with PIID, description, dates, `last_date_to_order`,
  obligations, and offices. No key required.
- **SAM.gov Contract Awards API** (FPDS successor) filters on
  `referencedIdvPiid`, `contractingOfficeCode`, PSC, NAICS; DoD awards
  under 90 days old are masked for non-federal keys. The FPDS ATOM feed
  retires in FY2026.
- **SAM.gov Opportunities API v2**: notice types by `ptype`, attachments
  via `resourceLinks[]`, contracting office via `fullParentPathCode`, plus
  NAICS, PSC, set-aside, and award number. Special notices (`ptype=s`)
  carry field-activity LRAFs.
- **OASIS+ domain workbook** (GSA, Sep 2022) maps each domain to NAICS
  and PSC sets, and notes that orders under multiple-award IDIQs inherit
  the IDIQ's NAICS in FPDS, so order-level PSC is the reliable code.
- **GSA eLibrary** XLSX (CC0) for MAS and GWAC holders by SIN; **CALC+**
  ceiling-rate API (no key) for labor-rate benchmarks by SIN and labor
  category.
- **GWCM PSC taxonomy user guide** (Mar 2018): the exact rule pipeline
  that assigns each PSC to the 19 government-wide categories: PSC lookup,
  then PSC-by-NAICS overrides, then keyword regexes on the 250-character
  FPDS description. It is deterministic and can be vendored as-is.
- **SAM.gov PSC API** serves Level 1 and 2 category per PSC directly
  (10 calls per day for a non-federal key, so pull once).
- **NAICS 2022** and the **PSC Manual** (April 2025, XLSX with start and
  end dates; April 2024 restructured 68 IT service codes into 40 new ones).
- **Navy LRAFs**: rows carry title, description, NAICS, estimated value,
  anticipated vehicle, set-aside, award quarter, contracting office.
  Weak labels, but labels.
- **GovInfo** CHRG collection (Congressional hearings) and GAO reports
  for the open-source corpus; **Federal Register** API for rulemaking.

Contracting-office names are not in the public Federal Hierarchy API
below sub-tier level; derive the office table from distinct
`awarding_office_code` values in the USAspending archive.

## Preparing the requirement side

Cluster on requirement content, not on template. Before vectorizing:

1. Strip clause text and numbers with the regex family
   `\b(?:52|252|552|852|352|1852|5252|5352)\.2\d{2}-\d{1,4}\b`, the
   "Clauses Incorporated by Reference" tables, Sections I, K, L, and M,
   DID citations `DI-[A-Z]{4}-\d{5}`, repeated page headers, and any
   normalized paragraph appearing in more than a fixed share of the
   corpus (hash with a fixed seed so runs reproduce).
2. Detect sections with a numbered-heading regex plus a synonym table
   (Scope, Background, Applicable Documents, Requirements or Tasks,
   Deliverables, Period and Place of Performance, Security, GFP, Travel,
   QASP). Weight Requirements and Scope highest; zero-weight Security,
   Travel, GFP, and Period for topical similarity but keep them as
   structured attributes (clearance, place of performance) for vehicle
   constraints.
3. Chunk Requirements text into task statements: sentences containing
   "shall", "will provide", "is required to", or "must", tagged with
   their heading path. Keep CDRL rows as deliverable chunks with DID
   numbers.
4. Use a procurement stoplist on top of English stopwords (contractor,
   government, shall, provide, support, services, requirement, task,
   order, contract, performance, COR, CO, KO, offeror, IAW, para, section,
   attachment, exhibit, CLIN, CDRL, DID, FAR, DFARS), and learn
   corpus-specific boilerplate terms by document frequency above 0.8,
   cached to a versioned file.

Representation choices that stay deterministic in .NET: TF-IDF with
sublinear TF or BM25 term weights (ML.NET TextCatalog or hand-rolled);
static embeddings (ML.NET GloVe or fastText via ApplyWordEmbedding, or
Model2Vec potion tables at 8 to 100 MB with Microsoft.ML.Tokenizers);
small transformer encoders (all-MiniLM-L6-v2 int8 ONNX at 23 MB via
Microsoft.ML.Tokenizers BertTokenizer and Microsoft.ML.OnnxRuntime with
one intra-op thread, sequential execution, and rounded outputs, embedding
task-statement chunks under 256 word-pieces then pooling). The published
evidence favors modest models: a Word2Vec plus GMM pipeline on procurement
text beat transformer embeddings on cluster quality, and NPS/AFICC's PSC
predictor used a character CNN served through ONNX Runtime.

Clustering that stays deterministic: sort by document id before any
pairwise loop; compute similarities in double precision with a fixed
summation order; use average-linkage agglomerative with an explicit
tie-break on (i, j) or connected components on the threshold graph with a
second, stricter pass to split giant components; never depend on
multi-threaded reductions. Label clusters with c-TF-IDF top terms, the
representative "shall" sentences nearest the centroid, the modal PSC and
NAICS, and the most-cited standards.

Two lessons from NAADAP's own reference run: a threshold tuned on a
synthetic ten-document fixture (0.35) left 11 of 20 real documents as
singletons, and scoring singletons at 1.0 cohesion pushed every loose
document above every real cluster. Tune on real documents, and never let
a one-document cluster outrank a multi-document one by convention.

## Scoring a cluster against vehicles

Score is a weighted, explainable sum of independent channels, each
stored with the rows that produced it.

| Channel | Evidence | Notes |
| --- | --- | --- |
| Lexical scope overlap | BM25 or c-TF-IDF cosine between the cluster's task statements and each vehicle's scope text | Report matched terms and the exact scope sentences |
| Code intersection | Overlap between the cluster's declared or predicted PSC/NAICS set and the vehicle's allowed pools and empirical order distribution | Prefer order-level PSC |
| Office affinity | P(vehicle given awarding office) and P(vehicle given command) from historical orders | Penalize vehicles the office cannot order from |
| Historical orders | k nearest prior orders under the vehicle by description similarity | Show PIIDs, offices, obligations, dates |
| Hard constraints | Ordering period open; ceiling headroom; set-aside compatibility; place of performance or zone; clearance and OCONUS pools; no-T&M rules | A failed constraint removes the vehicle, it does not lower a score |
| Policy priors | SeaPort-NxG mandatory consideration for Annex 22 work; DON ESL for software; NAVFAC for construction; DFARS 217.770 penalty for non-DoD vehicles | Encode as explicit rules with citations |

Emit the FAR 7.107-2 inputs alongside: estimated aggregate value of the
cluster from award notices and forecasts, count of separate current
contracts covering its members, labor-rate benchmarks (CALC+), and a
small-business impact note (incumbent sizes, set-aside history). The
recommendation then feeds a determination instead of replacing one.

## Output record

```
cluster_id, label_terms[], representative_shall_statements[{doc_id, section_path, sentence}],
member_documents[{doc_id, type, office_code, naics, psc, est_value, date}],
current_contracts_covering_members[{piid, parent_idv, office, obligations, pop_end}],
candidate_vehicles[{
  vehicle_id, name, family, ordering_period_end, eligibility, score,
  channels: {lexical, codes, office_affinity, history, constraints},
  far_7_107_inputs: {estimated_total_value, n_separate_requirements,
                     benchmark_rates_source, small_business_impact_note}
}],
kb_version, model_hashes, run_timestamp
```

Everything in the record traces to a knowledge-base row or a document
span. That is what makes the recommendation reviewable by PGIL.

## Validation that matches the rubric

The judges count correct predictions against a predetermined list of 20.
Proxy that with labeled public data before GFI arrives:

- Sources-sought notices that later awarded: the award's Referenced IDV
  PIID is the true vehicle. Score precision@k, recall@k, and MRR of the
  vehicle ranking against it.
- LRAF rows with an anticipated vehicle: weak labels for the same test.
- For clustering quality with proxy labels (PSC, NAICS, vehicle): ARI,
  NMI, homogeneity and completeness. Pair purity with NMI, since purity
  rewards many small clusters, which is exactly the singleton failure
  mode.

## Data-quality rules to encode

FPDS descriptions are at most 250 characters, often uppercase and
abbreviated; use character n-grams and PSC priors. Orders under
multiple-award IDIQs inherit the IDIQ NAICS; use the order PSC. Forecasts
are non-binding and may omit the vehicle. OT consortium projects appear
inconsistently in FPDS. Category-management Level 2 for some PSCs depends
on NAICS and description keywords, so run the GWCM rule pipeline rather
than a bare lookup. The eLibrary data.gov extract is dated 2021; refresh
from eLibrary directly.

## What the Government's own tools do

GSA Market Research As a Service takes a requirements document, has GSA
SMEs "identify and recommend the best available GSA acquisition vehicles,"
posts a sources-sought RFQ to all holders of the chosen SIN, and returns a
market research report. GSA's OASIS+ scope review confirms the domain a
requirement belongs in. 18F's Discovery tool modeled Vehicle to Pool
(defined by NAICS sets) to Vendor with FPDS history. No public Navy
"vehicle finder" exists; the LRAF is the closest artifact. NAADAP's
recommender is, in effect, an automated MRAS for the Navy's own vehicles.
