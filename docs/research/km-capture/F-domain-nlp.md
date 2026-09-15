# Area F — Domain-Specific NLP for Procurement/Legal Text, and Efficient Use of Scarce Expert Labels

Research conducted 2026-09-15 for NAADAP (NAVAIR/NAWCAD prize challenge).
Every citation below was retrieved live via Crossref API, arXiv, ACL Anthology, institutional
repositories, or the actual source PDF/repo. Items I could **not** retrieve are marked
**UNVERIFIED** or **NOT RETRIEVED** explicitly.

Standing constraints assumed throughout: offline Docker, no runtime network, deterministic
(>=95% identical top-5), 1 CPU / 2 GB / 30 min for 20 docs, no LLM in core path, C#/.NET 9.

---

# PART 1 — PROCUREMENT / CONTRACT / GOVERNMENT-DOCUMENT NLP

## F1.1 The NPS / AFICC PSC classification work — VERIFIED, with one correction

**Correction to the prompt's claim:** the paper is by **two** authors, not three. There is no
"Westermeyer" on it, and I found no Westermeyer co-authored work in this area at all.

**Citation (verified via Crossref API, DOI resolved, full metadata + abstract retrieved):**

> Muir, William A. (U.S. Air Force Installation Contracting Center, Randolph Air Force Base, TX)
> and Reich, Daniel (Naval Postgraduate School, Monterey, CA).
> "Using Machine Learning to Improve Public Reporting on U.S. Government Contracts."
> *INFORMS Journal on Applied Analytics*, Vol. 51, No. 6 (Nov 2021), pp. 463–479.
> DOI: 10.1287/inte.2021.1098

**Abstract (retrieved verbatim from Crossref JATS, key claims):**
- ~$500B/yr of US government contracts classified against a **hierarchical product/service
  taxonomy** (PSC). Classification is done manually today, "time-consuming and error-prone."
- Trained on **almost 4 million historical records** of governmental purchases.
- Two headline findings:
  1. "superior performance when explicitly modeling the hierarchical structure of information
     domains through the use of **top-down strategies**"
  2. "the effectiveness of **character-level convolutional neural networks** when textual inputs
     are **terse and contain irregularities such as abnormal character combinations and
     misspellings**, which are common in government contracts."
- "Our machine learning models are embedded in multiple software applications, including a web
  application that we developed, used by federal government personnel."

**Architecture / hierarchical decoding / ONNX — VERIFIED via the deployed system, not the paper.**
The deployed tool is **https://www.fscpsc.com/** ("A prediction engine for Product Service Codes").
Its /about page (retrieved) states:
- Built by "researchers at the Naval Postgraduate School and Air Force Installation Contracting
  Center" — matches Muir (AFICC) + Reich (NPS).
- Uses **character-level convolutional neural networks** for **hierarchical** PSC classification.
- Served via **ONNX Runtime** for inference, with a **Go** backend (chi router, SQLite3).
- Released **CC0 1.0 Universal** (public domain). Explicitly "not a U.S. Government website."
- Points to code at **github.com/wamuir** (= William A. Muir).
- API docs at https://api.fscpsc.com ; links the official PSC Manual on acquisition.gov.

So: **character-level CNN + hierarchical top-down decoding + ONNX serving is CONFIRMED.**
The "around 2021" date is CONFIRMED (Nov 2021).

**Reported accuracy — NOT RETRIEVED.** The INFORMS full text is paywalled; pubsonline.informs.org
returns HTTP 403 to both WebFetch and curl with browser headers. I could not find an open
preprint, an NPS technical-report version, a DTIC copy, or an author copy on
faculty.nps.edu/dreich/research/. **Do not cite a specific accuracy number for this paper.**
What is safely citable is the *qualitative* finding above (hierarchy helps; char-CNN helps on
terse/irregular text) plus the 4M-record training scale.

**GitHub repos by the author (retrieved from github.com/wamuir?tab=repositories):**
| Repo | Language | License | Note |
|---|---|---|---|
| `wamuir/onnxruntime_go` | Go | MIT | Go wrapper for microsoft/onnxruntime |
| `wamuir/graft` | Go | Apache-2.0 | TensorFlow C API bindings for Go (~71 stars) |
| `wamuir/golang-tf` | Dockerfile | MIT | Go+TF docker (~23 stars) |
| `wamuir/procurement-data-standard` | Go | (unstated) | Federal procurement data standard |
| `wamuir/onnxruntime`, `wamuir/tensorflow` | C++ | MIT / Apache-2.0 | forks |

**The trained PSC model itself does not appear to be published.** I found no repo containing the
weights or the training code. Treat the model as unavailable; only the *method* is reusable.

### What this means for NAADAP (assessment)
- **Do not copy the architecture.** Char-CNN is the right answer *at 4M labeled records*. NAADAP
  has a few dozen. See F1.5 below: Zhang/Zhao/LeCun themselves found the crossover point where
  char-CNN beats n-gram TF-IDF is at the **millions-of-records** scale.
- **Do copy two things:** (a) the *top-down hierarchical decoding* idea — it is directly relevant
  if NAADAP ever predicts PSC/NAICS as an intermediate feature, and it is trivially deterministic
  and trivially implementable in C#; (b) the observation that procurement text is **terse and
  character-irregular** (abbreviations, run-together tokens, misspellings), which argues for
  character n-gram features in the C# tokenizer rather than pure word tokens.
- ONNX Runtime has first-party C# bindings (`Microsoft.ML.OnnxRuntime`), so *if* a small model
  were ever trained offline, this serving path is viable in .NET and is deterministic for fixed
  weights on a fixed EP. But this is an option, not a recommendation, given the label budget.

## F1.2 Contract / legal NLP datasets and benchmarks

### CUAD — VERIFIED
> Hendrycks, Dan; Burns, Collin; Chen, Anya; Ball, Spencer.
> "CUAD: An Expert-Annotated NLP Dataset for Legal Contract Review."
> arXiv:2103.06268 (submitted 10 Mar 2021; rev. 8 Nov 2021). **NeurIPS 2021** (Datasets & Benchmarks).

- Task: **extractive span highlighting** — given a contract and a clause category, find the
  salient spans a lawyer must review.
- Built with "dozens of legal experts from The Atticus Project"; **>13,000 annotations**.
- Baseline finding (from abstract): "Transformer models have nascent performance, but that this
  performance is strongly influenced by model design and **training dataset size**."
- Positioned as "one of the only large, specialized NLP benchmarks annotated by experts."

**Transfer to NAADAP: essentially none.** CUAD is commercial-contract clause extraction (NDAs,
M&A, licensing), an extractive QA task over *executed contracts*. NAADAP's inputs are
pre-award requirement documents (SOW/PWS/CDRL/sources-sought) and the output is a ranked
*vehicle* recommendation, not a span. The vocabulary overlap (FAR clause language) is low in the
requirement sections that matter. The one genuinely transferable lesson is the meta-point:
**expert legal annotation is so expensive that even a flagship dataset is only 13k spans over
a few hundred contracts** — this validates NAADAP's premise that labels will be scarce.

### LexGLUE — VERIFIED
> Chalkidis, Ilias; Jana, Abhik; Hartung, Dirk; Bommarito, Michael; Androutsopoulos, Ion;
> Katz, Daniel Martin; Aletras, Nikolaos.
> "LexGLUE: A Benchmark Dataset for Legal Language Understanding in English."
> arXiv:2110.00976 (3 Oct 2021; v4 8 Nov 2022). **ACL 2022** (long paper).
> Code: `coastalcph/lex-glue` (GitHub); data on Hugging Face.

- Collection of legal NLU datasets (ECtHR, EUR-LEX, SCOTUS, LEDGAR, UNFAIR-ToS, CaseHOLD).
- Finding: "legal-oriented models consistently offer performance improvements across multiple
  tasks" over generic models.
- **Notable for NAADAP:** the v4 revision explicitly notes updated **TFIDF-SVM scores** — i.e.
  the benchmark maintainers considered a classical TF-IDF + SVM baseline important enough to
  correct and republish. In LexGLUE, TF-IDF+SVM is competitive on several tasks.

**Transfer: none directly** (EU/US case law and ToS, not procurement). Value is as evidence that
(a) domain-specific pretraining beats generic, and (b) TF-IDF+SVM remains a respectable baseline
on legal text even in a transformer benchmark.

### ContractNLI — VERIFIED
> Koreeda, Yuta and Manning, Christopher D.
> "ContractNLI: A Dataset for Document-level Natural Language Inference for Contracts."
> *Findings of ACL: EMNLP 2021*, pp. 1907–1919. DOI: 10.18653/v1/2021.findings-emnlp.164

- Task: given a **fixed set of hypotheses** (e.g. "Some obligations of Agreement may survive
  termination") and a contract, classify each as entailed / contradicted / not-mentioned, and
  identify **evidence spans**.
- **607 annotated contracts** (NDAs) — "the largest corpus to date" for this task.
- Baseline: "existing models fail badly"; their Span-NLI BERT models evidence identification as
  **multi-label classification over spans** rather than start/end token prediction, plus better
  long-document context segmentation.
- Explicitly flags that "negations by exceptions" make contract language hard.

**Transfer to NAADAP: conceptually interesting, practically no.** The *shape* of the task is the
closest analogue in the legal-NLP literature to "does this requirement fit within this vehicle's
scope?" — a fixed hypothesis set (one per vehicle scope statement) scored against a document.
But it needs a fine-tuned BERT and 607 annotated documents. A deterministic C# approximation of
the same shape — score each vehicle's scope statement against the requirement text with a
lexical/semantic similarity function — is exactly what NAADAP already plans, and does not need
this dataset.

### Corpora of government solicitations — the honest answer
There is **no established, peer-reviewed NLP benchmark built from US federal solicitations.**
What exists:

- **`abigailhaddad/govdocs`** (GitHub) — archive of US federal documents mirrored to Hugging Face.
  Includes `abigailhaddad/sam-solicitation-documents`: attachments from SAM.gov notices, explicitly
  including **statements of work, performance work statements, justifications, amendments, wage
  determinations**. Layout is `documents/<source>/<doc_id>.pdf` plus `metadata.parquet`; PDFs are
  stored as files rather than parquet binary columns so they stay directly downloadable. Documents
  are US Government works, no copyright. Document counts and date range are **not stated** in the
  README; repo license not stated in the README excerpt I retrieved.
  → **Directly usable for NAADAP as an offline corpus** for IDF statistics, vocabulary building,
  tokenizer tuning, and unlabeled-pool construction. This is probably the single most actionable
  find in Part 1 after the Muir/Reich work. Vet it yourself before relying on it — it is one
  person's archive, not an institutional release.
- **FPDS-NG / USAspending** bulk award data — structured, includes PSC/NAICS labels and short
  award descriptions. This is the data Muir & Reich used (~4M records). Bulk downloadable,
  offline-friendly. Not an NLP *benchmark* but the only large labeled procurement text source.
- GSA's `srt-ml` ships **993 pre-labeled solicitation documents** (see F1.4) — small, but real,
  expert-labeled federal solicitation text.

## F1.3 Institutional work on NLP for acquisition documents

### RAND, PE-A926-1 — VERIFIED, full PDF retrieved and read. **The most useful single source in this whole report.**
> Schirmer, Peter; Jaycocks, Amber; Mann, Sean; Marcellino, William; Matthews, Luke J.;
> Parsons, John David; Schulker, David.
> "Natural Language Processing: Security- and Defense-Related Lessons Learned."
> RAND Corporation Perspective **PE-A926-1**, July 2021. 16 pp.
> Conducted within RAND's **Acquisition and Technology Policy Center**.
> (rand.org returns 403 to normal fetch; the PDF at
> rand.org/content/dam/rand/pubs/perspectives/PEA900/PEA926-1/RAND_PEA926-1.pdf downloads fine
> with a browser User-Agent.)

Verbatim-grounded findings, all directly on point for NAADAP:

1. **Framing (p.1):** lessons "may prove particularly salient for DoD, because its terminology is
   very domain-specific and full of jargon, much of its data are classified or sensitive, its
   computing environment is more restricted, and its information systems are generally not
   designed to support large-scale analysis."

2. **TF-IDF is not a fallback, it is often the right answer (p.6):**
   "Simple text processing, such as term frequency–inverse document frequency (TFIDF) or even just
   bags of words, often work quite well for text classification tasks." On officer performance
   bullets — text that "is actually nowhere close to 'natural language'", with abbreviations,
   acronyms and word fragments — "our exploration of open-source NLP tools generally did not
   identify any that would help in this application. Instead, we found that **a basic TFIDF model
   performed as well as or better than more-complex approaches**, and it had the advantage of
   being easier to explain to the audience and more understandable."

3. **Stemming and stopword removal can HURT on jargon (p.6):** in normal text they improved models,
   but "In some cases in which extensive jargon is used, we have not seen improvements from doing
   this—or have even **lost information**—because, in jargon language, different grammatical usages
   of the same word can indicate radically different meanings."

4. **"Word embeddings may disappoint" (pp.6–7):** pretrained GloVe/Word2Vec/FastText "may work
   quite well for a simple NLP task, such as text retrieval on the basis of search terms. But even
   for text classification and similar tasks, their usefulness has proven to be limited."
   Pretrained embeddings on Wikipedia / Brown corpus "have been too generic."

5. **The critical nuance on custom domain embeddings (p.7):** they trained custom embeddings on
   20 years of RAND DoD-supporting publications. These "**vastly outperformed pretrained
   general-corpus embeddings on analogy and word-similarity tasks but did not improve performance
   on a text classification task.**"
   → Intrinsic embedding-quality gains do **not** imply downstream task gains. This is the single
   most important caveat for anyone tempted by "train Word2Vec on procurement text."
   They also recommend **building bigrams/n-grams as single tokens** (`Air_Force`,
   `Department_of_Defense`, `collective_training`) — this "definitely improved" intrinsic
   performance — and, with a small corpus, setting the minimum word-occurrence threshold **low
   (3–5)**.

6. **Tokenizers, stemmers, lemmatizers, POS taggers, stop-word lists "may need to be customized to
   the particular task"** and "lemmas, like word embeddings, are based on general language usage
   and may have limited utility... for topics with specialized language."

7. **"Ground truth is not necessarily true" (pp.8–9):** on one text classification task they had a
   **U.S. Navy surface warfare SME manually label 100 observations**, compared against the model
   and the original source. With the model at F1 ≈ 80%, they heavily oversampled the model's
   errors — and "**In roughly half of the 'erroneous' model results, our subject-matter expert
   agreed with the model and not with the original source.**"
   → For NAADAP: your SME ground truth and your existing administrative labels will disagree, and
   the model will often be right. Budget SME time for adjudicating disagreements, and report
   inter-annotator agreement, not just accuracy against a single label source.

8. They also flag a small-dictionary trick (a few hundred terms) to ensure the model sees
   **highly discriminating words that occur frequently in only one category** — "most important
   when dealing with imbalanced data sets."

**Assessment: this report independently validates the NAADAP architecture** (deterministic
lexical/TF-IDF core, domain-specific tokenization, no dependence on general-purpose pretrained
embeddings) from an institution with no stake in the challenge. Cite it.

### AIRC / Stevens + Virginia Tech — NDAA→FAR/DFARS mapping — VERIFIED, full PDF retrieved
> Ramirez-Marquez, Jose E.; Amer, Akram; Gorman, Joshua; Buettner, Douglas (Stevens Institute of
> Technology); Mayer, Brian; Self, Nathan; Laxman, Harith; Ramakrishnan, Naren (Virginia Tech).
> "NLP Recognition and Categorization of Acquisition Text and Documents."
> Final Technical Report **AIRC-2026-TR-005 / WRT-2507.11B**, 06 April 2026. 30 pp.
> Contract HQ0034-24-D-0023, Task Order 048. Sponsor: OUSD(A&S). Cleared for Public Release.
> Retrieved from dmi-ida.org (DTIC accession referenced in the filename as AD1357794 — note the
> filename's accession number appears to refer to an **earlier** version; the PDF I extracted is
> dated 2026 and is TR-005. Treat the AD number as **UNVERIFIED**.)

Why it matters to NAADAP: **this is the closest published analogue to NAADAP's actual task** —
match each section of one document against a large corpus of candidate reference sections and
return a **ranked top-N**, then evaluate the ranking against a small analyst-built golden set.

Method (Step 2, "NDAA ↔ FAR/DFARS Matching & Ranking"): "each NDAA candidate section is compared
against all FAR and DFARS sections for that regulation year. **A scoring algorithm evaluates
relevance and potential impact. The top-ranked FAR/DFARS sections (e.g., top 10) are selected per
NDAA section.**" **The scoring algorithm itself is not specified in this report** — a real gap.
(Steps 3, 5 and 6 — draft text generation and public-comment summarization — are LLM-based
[GPT-4o via NIPRGPT, chain-of-thought prompting]. Steps 1–2, extraction and matching, are not
described as LLM-based.)

Evaluation design, worth copying wholesale:
- **Extraction quality index** Q(t) = E_ext(t) / E_exp(t), where "expected" is the
  **analyst-identified** count. Results: 90–100% across FY2010–2023 (Table 1), with the misses
  named individually (e.g. FY2011 missed §834, §893).
- **Matching quality index**, micro-averaged: QI = Σ|R(s)∩G(s)| / Σ|G(s)| × 100, over sections
  with at least one validated ground-truth mapping, plus a second reported statistic,
  "**sections with <50% mapping coverage**." Results (Table 2):

  | FY | Sections w/ published mappings | Sections <50% coverage | Quality Index |
  |---|---|---|---|
  | 2010 | 15 | 2 | 87% |
  | 2013 | 24 | 7 | 71% |
  | 2015 | 18 | 7 | **61%** |
  | 2017 | 50 | 16 | 68% |
  | 2020 | 31 | 5 | 84% |
  | 2023 | 19 | 5 | 74% |

  Across FY2010–2023 the top-10 retrieval quality index ranges **61%–87%**.

- Generated-text evaluation used BLEU, ROUGE, and LLM-as-a-judge against finalized DFARS sections.

**Assessment for NAADAP:**
1. **Calibrate expectations.** A well-funded UARC team, with LLMs available and a far richer
   ground truth (published NDAA→DFARS mappings), gets **61–87% top-10 coverage**. A deterministic
   C# top-5 on a harder, fuzzier task should not be held to 95% recall, and you should say so in
   the write-up.
2. **Steal the metric.** The micro-averaged coverage index plus "fraction of items below 50%
   coverage" is a better, more honest scorecard than mean reciprocal rank alone, and it reports
   *where the system fails*, which the judging rubric will reward.
3. **Steal the golden-set discipline.** Ground truth is "derived from manual analyst identification"
   and every miss is listed by name. Do the same with your SME-labeled set.

Related earlier AIRC report (verified separately):
> Ramirez-Marquez, Jose E.; Lopez, Bryce; Chan, Enoch.
> "Automated User Interface for NLP-Driven Keyword Analysis in Department of Defense Documents."
> AIRC report **WRT-1097.18.1-v1.1**, June 2025, Stevens Institute of Technology, sponsored by
> OUSD(A&S). PDF: acqirc.org/wp-content/uploads/2025/06/WRT-1097.18.1-v1.1.pdf
> Five-phase framework: PDF processing/text extraction → similarity analysis and visualization →
> keyword extraction → summarization and clustering → bigram selection and expansion.
> Uses **Louvain clustering** and bigram analysis, plus the OpenAI API for summarization.
> Evaluation is qualitative ("reduced manual effort"), no quantitative accuracy reported.
> **Louvain community detection is deterministic given a fixed node ordering and seed** and is
> straightforwardly implementable in C# — relevant if NAADAP wants graph-based clustering of
> requirements. The bigram-expansion idea corroborates RAND's n-gram-token recommendation.

### NPS Acquisition Research Program (DAIR) — partial
DAIR (dair.nps.edu) hosts the Acquisition Research Symposium proceedings back to 2003. Confirmed
relevant items exist (e.g. sentiment/text-mining of Selected Acquisition Report executive summaries
correlated with MDAP unit cost; Stanford CoreNLP + POS-tagging pipelines; latent-semantic comparison
of Navy Urgent Need Statements against Trident Warrior technologies), but DAIR's search endpoint
returned 404 to my fetches and the individual papers came back only as search snippets.
**I did not verify these individually — treat them as leads, not citations.** If you want them,
browse dair.nps.edu by hand; the bitstream URLs follow
`dair.nps.edu/bitstream/123456789/<id>/1/<report-no>.pdf`.

Adjacent but **not NLP** (verified via Crossref, listed so nobody miscites it as NLP work):
> Landale, Karen A.F.; Apte, Aruna; Rendon, Rene G.; Salmerón, Javier.
> "Using analytics to inform category management and strategic sourcing."
> *Journal of Defense Analytics and Logistics*, Vol. 1, No. 2 (2017), pp. 151–171.
> DOI: 10.1108/jdal-06-2017-0010.
> Method is **sequential regression, Wilcoxon rank-sum, ordered logistic regression** on
> integrated solid waste management contracts — price and contractor-performance drivers,
> including small-business set-aside effects. Relevant to category management *analytics*;
> contains no text processing.

### Other Part-1 leads I could not substantiate
- No MITRE technical report on NLP for acquisition/requirements documents surfaced. MITRE's
  published NLP work I could find is in cyber threat intelligence (ATT&CK technique extraction),
  not acquisition. **UNVERIFIED / likely does not exist publicly.**
- No IDA (Institute for Defense Analyses) report on this topic surfaced. Note: the
  "dmi-ida.org" host that serves the AIRC report above is **not** the Institute for Defense
  Analyses — do not confuse them.
- No DAU (Defense Acquisition University) technical publication on NLP methodology surfaced;
  DAU material found was glossary/Acquipedia content, not research.

### Related peer-reviewed work on hierarchical code classification with few labels — VERIFIED
> Kaur, Simerjot; Stefanucci, Andrea; Shah, Sameena.
> "InProC: Industry and Product/Service Code Classification." arXiv:2305.13532, 22 May 2023
> (q-fin.CP). *Venue/peer-review status beyond arXiv: not established.*
> Hierarchical multi-class industry classifier + targeted multi-label product/service classifier,
> built on **unsupervised representation learning**, explicitly **not LLM-based**.
> >20,000 companies; **>92%** industry-code accuracy; **>96%** product/service-code accuracy
> validated against **350 SME-labeled codes**. Explicitly framed as working with an "extremely
> limited labeled dataset."
> **Caveat: their "product/service code" is a finance-industry taxonomy, not the federal PSC.**
> Author affiliations were not shown on the abstract page I retrieved — **UNVERIFIED**.
> Value to NAADAP: an existence proof that hierarchical code assignment can be validated against
> ~350 SME labels and that unsupervised representation + small supervised head is a viable shape.

## F1.4 Government-built tools with published methodology

### GSA Solicitation Review Tool (SRT) — real repos, thin methodology
| Repo | Language | License | Status | Notes |
|---|---|---|---|---|
| `GSA/srt-ui` | JavaScript / Angular 15 | (GSA open source) | active | Client for viewing Section 508 compliance predictions |
| `GSA/srt-api` | JavaScript | (GSA open source) | active | Server-side API |
| `GSA/srt-fbo-scraper` | Python 3.6.6 | not stated in README | **not archived**, ~67 stars, ~1124 commits | Scrapes SAM.gov, extracts text with `textract`, runs the ML model, stores to PostgreSQL; runs as a cron daemon in Docker on cloud.gov |
| `GSA/srt-ml` | Python (sklearn + AWS SageMaker) | **CC0 1.0 Universal** | ~7 stars, ~100 commits | The actual model |

What SRT does: predicts whether a federal ICT solicitation contains appropriate **Section 508
accessibility language** — a binary/compliance classification over solicitation attachments.

**Published methodology is genuinely thin.** `srt-fbo-scraper`'s README says only "a supervised
machine learning model" and defers to `srt-ml`. `srt-ml` does not document the algorithm,
vectorizer, or any accuracy figure. **No accuracy numbers are published for SRT anywhere I could
find.** Do not cite a performance claim for it.

Two things are still worth having:
1. **`srt-ml` ships 993 pre-labeled solicitation documents** (README links a Google Drive
   download). That is a real, expert-labeled corpus of federal solicitation text under CC0.
   Small — but it is roughly the scale NAADAP is operating at, and it is *the* public precedent
   that a fielded federal solicitation classifier was built on ~1,000 labels, not 100,000.
2. `srt-fbo-scraper` documents an intended **human-in-the-loop retraining loop**: "the script will
   poll the database for the number of user-validated predictions... those newly validated
   documents will be sent to the machine learning component... to train a new and improved model."
   Described as a future feature, i.e. **aspirational, not demonstrated**. This is the same
   pattern as NAADAP's SME-feedback loop; note that a fielded government system planned it and
   (as documented) had not yet shipped it.

### 18F Discovery — **archived, and contains no NLP**
`18F/discovery` (GitHub). Python/Django (Python 2.7+). **CC0 1.0 Universal**.
**Archived 27 August 2019, read-only.** Was live at discovery.gsa.gov.
Market research tool over the OASIS family of vehicles; data from SAM and FPDS.
Its matching is **structured categorical filtering only** — vendors filtered by NAICS shortcode,
set-aside code, and vendor pool (e.g. `?setasides=A5,QF&naics=541330`). **No keyword matching,
no text similarity, no NLP of any kind.**

**This is an important negative result for NAADAP's positioning.** The government's own prior
market-research tool for exactly the OASIS-style multiple-award vehicle problem did structured
code filtering and nothing else, and it was retired in 2019. NAADAP's claim to novelty — reading
the *requirement text* rather than filtering on a pre-assigned NAICS — is well founded, and this
repo is the citation that establishes it.

### GSA Market Research As a Service (MRAS) — **a human service, no algorithm**
MRAS is delivered through buy.gsa.gov / gsa.gov (FAS Customer and Stakeholder Engagement).
Offerings: Request for Information (drafted by GSA staff, posted to GSA eBuy), Product Market
Research (pricing report over up to 20,000 GSA Advantage products), and Rapid Review
("a comprehensive list of potential GSA solutions and contract information").
Process: agency provides its requirement → **MRAS staff draft a custom RFI** → posted to eBuy →
responses returned as a market research report.
**There is no published algorithm, model, or technical documentation.** The Rapid Review service
is functionally the human version of what NAADAP automates. Cite it as the *status quo baseline*
(a human analyst, days-to-weeks turnaround), **not** as prior technical art.

## F1.5 The one architectural fact that settles char-CNN for NAADAP — VERIFIED from the source
> Zhang, Xiang; Zhao, Junbo; LeCun, Yann. "Character-level Convolutional Networks for Text
> Classification." arXiv:1509.01626. **NeurIPS/NIPS 2015**. (Full PDF retrieved and read.)

From their own Section 6 discussion, verbatim:
> "**Dataset size forms a dichotomy between traditional and ConvNets models.** ... Traditional
> methods like n-grams TFIDF remain strong candidates for dataset of size up to **several hundreds
> of thousands**, and only until the dataset goes to the scale of **several millions** do we
> observe that character-level ConvNets start to do better."

And:
> "ConvNets may work well for user-generated data ... character-level ConvNets work better for
> **less curated** user-generated texts."

**Implication:** Muir & Reich's char-CNN choice is correct *because they had ~4M records*.
At NAADAP's scale (dozens of labeled documents, a few thousand unlabeled), the same paper says
n-gram TF-IDF is the stronger model. This is a clean, citable, first-party justification for
NAADAP's no-neural-net core — stronger than a resource-constraint argument, because it is an
accuracy argument.

---

# PART 2 — MAXIMUM VALUE FROM VERY FEW EXPERT LABELS

## F2.1 Active learning: mechanism

Canonical references, all verified via Crossref / institutional repository:

| Method | Citation | Verified |
|---|---|---|
| Survey | Settles, Burr. "Active Learning Literature Survey." Computer Sciences Technical Report **1648**, University of Wisconsin–Madison, 2009. minds.wisc.edu handle 1793/60660, file TR1648.pdf | ✅ repository record retrieved |
| Book-length update | Settles, Burr. *Active Learning.* Synthesis Lectures on AI and ML, 2012. DOI 10.1007/978-3-031-01560-1 | ✅ Crossref |
| Uncertainty sampling | Lewis, David D. and Gale, William A. "A Sequential Algorithm for Training Text Classifiers." *SIGIR '94*, pp. 3–12. DOI 10.1007/978-1-4471-2099-5_1 | ✅ Crossref |
| Query-by-committee | Seung, H.S.; Opper, M.; Sompolinsky, H. "Query by committee." *COLT '92*, pp. 287–294. DOI 10.1145/130385.130417 | ✅ Crossref |
| Empirical comparison on NLP | Settles, Burr and Craven, Mark. "An Analysis of Active Learning Strategies for Sequence Labeling Tasks." *EMNLP 2008*, pp. 1070–1079. ACL ID D08-1112 | ✅ full PDF retrieved |

Mechanisms, stated plainly:
- **Uncertainty sampling** — label the instance the current model is least confident about.
  Variants: *least confident* (1 − P(ŷ|x)), *margin* (P(ŷ₁) − P(ŷ₂)), *entropy*. O(n) per round,
  fully deterministic given a deterministic model and a defined tie-break.
- **Query-by-committee** — train a committee (bagged models, different seeds/feature views),
  query the instance with maximum disagreement (vote entropy, or average KL to the mean).
  Deterministic only if the committee's randomness is seeded and fixed.
- **Expected model change / expected gradient length (EGL)** — query the instance that would
  produce the largest gradient-norm update if labeled. Requires a differentiable model;
  **not applicable to a non-gradient C# scoring pipeline.**
- **Information density** (Settles & Craven's contribution) — weight an informativeness measure by
  the instance's average similarity to the rest of the pool, so the learner does not chase
  unrepresentative outliers.

**Settles & Craven 2008, Table 2 (area under F1 learning curve, averaged over 8 corpora), read
directly from the PDF:**

| Strategy | Avg AULC |
|---|---|
| **Random baseline** | **83.1** |
| Longest-sequence baseline | 83.9 |
| Least confident (LC) | 91.6 |
| Sequence entropy (SE) | 91.4 |
| Token entropy (TE) | **78.0** ← *worse than random* |
| Vote entropy (VE) | **78.2** ← *worse than random* |
| Token KL (KL) | **80.7** ← *worse than random* |
| Sequence vote entropy (SVE, QBC) | 91.6 |
| Expected gradient length (EGL) | 90.0 |
| **Information density (ID)** | **92.2** ← best |
| Fisher information (FIR) | 90.0 |

Their own summary: "**there is no single clear winner.**" Information density "never performs
poorly" and has the highest average. Strategies that score the **whole sequence** beat those that
aggregate token-level scores. And note the per-corpus detail: on the *References* corpus,
random (90.0) beat or matched nearly every strategy, and on *Sig+Reply* the spread between random
(129.1) and the best (133.2) is trivial.

## F2.2 Active learning: the negative results — take these seriously

### Attenberg & Provost 2011 — VERIFIED
> Attenberg, Josh and Provost, Foster. "Inactive learning?: difficulties employing active learning
> in practice." *ACM SIGKDD Explorations Newsletter*, Vol. 12, No. 2 (31 Mar 2011), pp. 36–41.
> DOI 10.1145/1964897.1964906. Open copy: pages.stern.nyu.edu/~fprovost/Papers/Attenberg_inactive_Explorations.pdf

Core observation: "despite the tremendous level of adoption of machine learning techniques in
real-world settings, and the large volume of research on active learning, **active learning
techniques have been slow to gain substantial traction in practical applications**." The paper is
an essay on "several important and under-discussed challenges to using active learning well in
practice."

### Lowell, Lipton & Wallace 2019 — VERIFIED
> Lowell, David; Lipton, Zachary C.; Wallace, Byron C. "Practical Obstacles to Deploying Active
> Learning." *EMNLP-IJCNLP 2019*, pp. 21–30. ACL D19-1003. DOI 10.18653/v1/D19-1003.
> Also arXiv:1807.04801.

Two findings that matter enormously for NAADAP:
1. **AL does not reliably beat random sampling.** "the benefits of current approaches do not
   generalize reliably across models and tasks." You cannot know in advance whether AL will help
   on *your* task, and you cannot afford to run the experiment that would tell you.
2. **The successor-model problem.** A dataset collected by AL under model A, when used to train a
   *different* model B, "does not consistently outperform training on i.i.d. sampled data." AL
   couples your labeled set to the model that selected it. **For NAADAP this is close to
   disqualifying**: the whole point of spending an SME's scarce hours is to produce a labeled set
   that stays valuable as the scoring pipeline evolves over the challenge period. An AL-selected
   set is biased toward whatever the v1 scorer found confusing, and that bias is not recoverable.
   The authors close by questioning whether "the downsides inherent to AL are worth the modest and
   inconsistent performance gains."

### Ghose & Nguyen 2024 — VERIFIED (arXiv abstract page)
> Ghose, Abhishek and Nguyen, Emma Thuong. "On the Fragility of Active Learners for Text
> Classification." arXiv:2403.15744 (cs.LG, cs.CL).
> *A search result listed an aclanthology.org/2024.emnlp-main.1240.pdf URL implying EMNLP 2024 main
> conference; I did not fetch that page, so the **venue is UNVERIFIED** — cite the arXiv ID.*

Their framing question, verbatim from the abstract: AL techniques "lack 'prerequisite checks', i.e.,
there are no prescribed criteria to pick an AL algorithm best suited for a dataset... The important
questions then are: **how often on average, do we expect any AL technique to reliably beat the
computationally cheap and easy-to-implement strategy of random sampling?**" They evaluate across
datasets × text representations × classifiers, examining **warm-up time before AL shows gains** and
whether "Always ON" deployment is viable. Conclusion: AL's effectiveness depends heavily on the
representation and classifier, and practitioners cannot predict it without prior testing.

### The cold-start problem
With 0 labels there is no model, so there is nothing to be uncertain about. Every AL loop needs a
seed set drawn some other way (random, or diversity/k-center). At NAADAP's budget (a few dozen
labels total), the seed set *is* most of the budget — there are not enough rounds left for AL to
pay back its warm-up cost. This follows directly from Ghose & Nguyen's warm-up analysis.

### **Recommendation for NAADAP: do not build an active learning loop.**
Instead:
1. **Stratified/diversity seed selection, done once, deterministically.** Cluster the unlabeled
   document pool (deterministic k-means++ with a fixed seed, or deterministic agglomerative
   clustering), then hand the SME the **medoid of each cluster**, plus a few boundary cases. This
   is a one-shot, model-free, fully deterministic, reproducible selection. It gives coverage
   without the successor-model bias, and it survives any later change to the scorer.
2. **Spend a fixed extra slice of SME time on adjudicating disagreements**, per the RAND
   finding that the SME sided with the model against the recorded label ~50% of the time.
3. If you want the *narrative* of active learning for the rubric, present it as: "we evaluated
   uncertainty sampling and QBC against the published negative results (Lowell et al. 2019;
   Ghose & Nguyen 2024) and deliberately chose one-shot diversity sampling because AL's
   model-coupling would invalidate the label set as the scorer evolves." That is a stronger
   answer than implementing AL, and it is defensible.

## F2.3 Few-shot text classification without an LLM

### SetFit — VERIFIED, with full result tables from ar5iv HTML
> Tunstall, Lewis; Reimers, Nils; Jo, Unso Eun Seo; Bates, Luke; Korat, Daniel; Wasserblat, Moshe;
> Pereg, Oren. "Efficient Few-Shot Learning Without Prompts." arXiv:2209.11055, 22 Sep 2022.
> Repo: **`huggingface/setfit`**, Python, **Apache-2.0**, ~2.8k stars, actively maintained.

Mechanism (two stages, no prompts, no verbalizers, **no generative LLM**):
1. **Contrastive fine-tuning** of a pretrained Sentence Transformer in a Siamese/triplet setup on
   text pairs generated from the few-shot set.
2. Freeze, embed, and train a **plain classification head** (logistic regression) on the embeddings.

**Pair generation** (the part that makes it work): for each class, generate **R positive pairs**
from within the class and **R negative pairs** against other classes, giving a training set of
size **2R|C|**. The paper uses **R = 20** throughout. With 8 examples/class × 3 classes that turns
24 labels into 120 training pairs — this combinatorial blow-up is the whole trick.

**Results (from the paper's tables):**

| Method | N=8/class | N=64/class | Params |
|---|---|---|---|
| Standard fine-tuning | 43.0 | 69.7 | — |
| PERFECT | 48.7 | 72.7 | — |
| ADAPET | 58.3 | 73.8 | — |
| T-Few 3B | **63.4** | 70.3 | 3B |
| **SetFit (MPNet)** | 62.3 | **75.3** | **110M** |

- RAFT benchmark: SetFit-RoBERTa scored **71.3 (rank 6)**, beating GPT-3's 62.7, "more than
  30 times smaller than T-Few 11B."
- Model sizes: SetFit-RoBERTa 355M, SetFit-MPNet 110M, **SetFit-MiniLM 15M**.
- Cost: SetFit-MPNet ≈ **30 seconds** to train on a p3.2xlarge, **$0.025/split**, vs T-Few 3B
  ≈ 700 seconds, $0.7/split, requiring ≥40 GB GPU memory.

**Rubric point the prompt is right about:** SetFit uses a **sentence transformer** (an encoder),
not a generative language model. If NAADAP's rubric constraint is "no large language model in the
core path," SetFit is arguably compliant on a strict reading — but it is an 110M-parameter
transformer, it needs PyTorch/`sentence-transformers` at *training* time, and defending it as
"not an LLM" invites an argument you do not need to have.

**Assessment for NAADAP — honest verdict: use the *idea*, not the library.**
- ❌ **Training** requires Python + PyTorch + a downloaded pretrained checkpoint. Not offline-clean,
  not C#, not in-budget.
- ⚠️ **Inference** could run via ONNX Runtime in .NET (110M params fits in 2 GB, though a 20-doc
  batch on 1 CPU core in 30 min is tight but feasible; MiniLM at 15M would be comfortable).
  But you'd be shipping frozen weights, which is a supply-chain and determinism story you have to
  own, and float non-determinism across EPs/threads is real (mitigable: pin thread count to 1,
  fix the EP, quantize deterministically).
- ✅ **The transferable, C#-implementable idea is the pair-expansion.** NAADAP can take its
  few dozen SME labels and expand them into O(R·|C|) *pairwise* training signals — "these two
  requirements belong on the same vehicle" / "these two do not" — and fit a deterministic
  low-rank metric or a simple linear reweighting of TF-IDF dimensions on those pairs. That is
  ordinary linear algebra, exactly reproducible in C#, and captures SetFit's actual insight
  (pairs multiply scarce labels) without the transformer.

### The simpler baselines you should actually beat first
| Baseline | Citation | C#-implementable deterministically? |
|---|---|---|
| TF-IDF + linear SVM / logistic regression | standard; LexGLUE (Chalkidis et al. 2022) reports TFIDF-SVM as a maintained baseline; RAND PE-A926-1 found basic TF-IDF "as well as or better than more-complex approaches" on jargon text | ✅ Trivially. Deterministic given a fixed feature order and a deterministic solver (LBFGS with fixed init, or closed-form ridge) |
| BM25 ranking | Robertson, Stephen and Zaragoza, Hugo. "The Probabilistic Relevance Framework: BM25 and Beyond." *Foundations and Trends in Information Retrieval*, Vol. 4 (2009), pp. 1–174. DOI 10.1561/1500000019 | ✅ Exactly. Pure arithmetic over term statistics; the natural scorer for requirement→vehicle-scope matching |
| fastText linear bag-of-n-grams | Joulin, Armand; Grave, Edouard; Bojanowski, Piotr; Mikolov, Tomas. "Bag of Tricks for Efficient Text Classification." *EACL 2017*, Vol. 2 (short papers), pp. 427–431. DOI 10.18653/v1/e17-2068 | ✅ The linear model is simple; hashing + averaging + softmax. Deterministic with fixed hashing seed |
| SIF / smooth-inverse-frequency sentence embeddings + common-component removal | Arora, Sanjeev; Liang, Yingyu; Ma, Tengyu. "A Simple but Tough-to-Beat Baseline for Sentence Embeddings." **ICLR 2017**, Toulon. (Verified via dblp `conf/iclr/AroraLM17` and Princeton's research portal; OpenReview forum SyK00v5xx is behind a bot check.) Reported ~10–30% improvement on textual-similarity tasks, beating supervised RNN/LSTM methods | ✅ Weighted average of word vectors (weight a/(a+p(w))) minus the first principal component. Needs a deterministic PCA/power iteration — use a fixed number of iterations and a fixed sign convention |

**SIF is the highest-leverage item in this table for NAADAP** if you end up with any static word
vectors: it is 15 lines of code, it is the standard way to turn word vectors into a document
vector, it is fully deterministic if you fix the power-iteration count and pin the eigenvector
sign, and it has a real ICLR paper behind it.

## F2.4 Semi-supervised / self-training / co-training — real gains, but not at this scale

### Foundational
> Blum, Avrim and Mitchell, Tom. "Combining labeled and unlabeled data with co-training."
> *COLT '98*, pp. 92–100. DOI 10.1145/279943.279962. ✅ Crossref
> — requires two **conditionally independent, individually sufficient** feature views.

> Nigam, Kamal and Ghani, Rayid. "Analyzing the effectiveness and applicability of co-training."
> *CIKM 2000*, pp. 86–93. DOI 10.1145/354756.354805. ✅ Crossref
> — the empirical follow-up that examines when the view-independence assumption actually holds.

**For NAADAP, co-training is a non-starter:** a SOW has no natural pair of independent sufficient
views. You could construct pseudo-views (lexical features vs. structural/metadata features), but
they are neither independent nor individually sufficient, and Blum & Mitchell's guarantees
evaporate. Skip it.

### The central negative result — VERIFIED
> Oliver, Avital; Odena, Augustus; Raffel, Colin; Cubuk, Ekin Dogus; Goodfellow, Ian J.
> "Realistic Evaluation of Deep Semi-Supervised Learning Algorithms." *NeurIPS 2018*, pp. 3239–3250.
> proceedings.neurips.cc/paper/2018/hash/c1fea270c48e8079d8ddf7d06d26ab52-Abstract.html

Three findings, all of which apply to NAADAP:
1. "**simple baselines which do not use unlabeled data are often underreported**" — SSL papers
   compare against weak supervised baselines, inflating apparent gains.
2. SSL methods "differ in sensitivity to the amount of labeled and unlabeled data."
3. "**performance can degrade substantially when the unlabeled dataset contains out-of-distribution
   examples.**" ← This is the killer for NAADAP. A pool of scraped SAM.gov attachments will
   contain wage determinations, amendments, Q&A logs, and boilerplate — none of which are
   requirement content. Self-training on that pool will pull the model toward the boilerplate.

### Modern few-shot self-training for text — VERIFIED
> Chen, Yiming; Zhang, Yan; Zhang, Chen; Lee, Grandee; Cheng, Ran; Li, Haizhou.
> "Revisiting Self-Training for Few-Shot Learning of Language Model." *EMNLP 2021*, pp. 9125–9135.
> arXiv:2110.01256. ACL ID 2021.emnlp-main.718.
> (Note: this is an **EMNLP 2021** paper, not ICLR 2021, if it appears in project notes otherwise.)
> Method (SFLM): two views via weak and strong augmentation; pseudo-label the weakly-augmented
> view; require the strongly-augmented view to predict the same label. Outperforms supervised
> and semi-supervised baselines on sentence and sentence-pair classification.
> **Prompt-based and requires a fine-tunable language model — out of scope for NAADAP's core path.**

### **Recommendation for NAADAP: no self-training in the core path.**
Do use the unlabeled corpus for the things that are safe and deterministic:
- **IDF statistics** computed over a large, frozen, versioned procurement corpus. This is
  *unsupervised use of unlabeled data that carries no pseudo-label risk*, and it materially
  improves TF-IDF/BM25 quality on domain text.
- **Domain vocabulary / collocation mining** to build the bigram tokens RAND recommends
  (`performance_work_statement`, `contractor_furnished_equipment`, `SeaPort_NxG`).
- **Deterministic clustering of the unlabeled pool** to drive the one-shot seed selection in F2.2.
Ship the corpus and its derived statistics *inside the container* so the IDF table is frozen and
the output is bit-reproducible.

## F2.5 Small-model / static text representation

### Model2Vec — VERIFIED, with numbers
Repo: **`MinishLab/model2vec`**, Python, **MIT**, ~2.2k stars, ~370 commits, actively maintained,
on PyPI, Zenodo DOI 10.5281/zenodo.17270888. **No arXiv paper** — the README cites only the
software DOI. Base package's "only major dependency is numpy."

Method: forward a **vocabulary** (not a dataset) through a Sentence Transformer to get static
per-token embeddings, then post-process. Multiple secondary sources describe this as **PCA
dimensionality reduction + Zipf weighting**; the repo README I retrieved does **not** state the
PCA/Zipf steps and defers to external docs, so treat the exact post-processing as
**partially verified**. Distillation is dataset-free and takes ~30 s on CPU. Claims up to 50×
size reduction (smallest model ~8 MB) and up to 500× faster CPU inference than the source model.

**The numbers that actually answer the question "do small static embeddings match transformers?"**

MTEB English average (from `results/README.md`, retrieved):

| Model | MTEB avg |
|---|---|
| all-MiniLM-L6-v2 (transformer) | **55.93** |
| potion-base-32M (static) | 52.13 |
| potion-base-8M (static) | 51.08 |
| GloVe 300d | 45.82 |
| BPEmb 50k 300d | 41.74 |

potion-base-32M reaches **93.21%** of all-MiniLM-L6-v2's performance.

Few-shot-ish classification, **14 classification datasets, 1,000 training examples each**:

| Method | Avg accuracy |
|---|---|
| BGE-base + logistic regression | **82.8** |
| SetFit | 82.6 |
| Model2Vec full fine-tune | 79.2 |
| Model2Vec + logistic regression | 78.0 |

Model2Vec full fine-tune is reported as **35× faster than SetFit** on CPU during training.

**Honest reading: static embeddings do NOT match transformers — they trail by ~4–5 accuracy points
on classification and ~4 MTEB points — but they close most of the gap versus GloVe/BPEmb and they
are 2–3 orders of magnitude cheaper.** Anyone claiming parity is overselling.

**NAADAP applicability:** a distilled static embedding table is **the single most C#-friendly
neural artifact that exists**. It is a `Dictionary<string, float[]>` plus a tokenizer plus vector
averaging. Inference is a table lookup and a sum — **exactly deterministic**, trivially offline,
negligible memory (8–30 MB), and no ONNX Runtime, no BLAS non-determinism, no thread-count
sensitivity. If NAADAP wants any semantic (non-lexical) signal at all, **this is the shape to use**:
ship a frozen static embedding table in the container, combine with SIF weighting (F2.3), and
blend with BM25. Two caveats you must handle: (1) distillation itself requires Python + a
pretrained checkpoint **at build time** — document it as a build-time artifact, not a runtime
dependency; (2) the RAND finding in F1.3 warns that general-corpus embeddings are "too generic"
for DoD jargon, so a static table distilled from a general sentence transformer may add little
over good TF-IDF. **Test it; do not assume it helps.**

### Related option not covered by the prompt
`sentence-transformers` also ships purpose-trained **static embedding models** (the
`static-retrieval-mrl-en-v1` line) rather than distilled ones. Same runtime story (lookup +
average). I did not verify these in depth — flagging as a lead only. **UNVERIFIED.**

## F2.6 The "Word2Vec + GMM beat transformer embeddings on procurement text" claim
### → **UNVERIFIED. I could not find any source supporting this, and I searched hard.**

Searches run: Word2Vec + GMM + procurement/spend/UNSPSC vs BERT/transformer; spend classification
+ clustering + embeddings; procurement text mining + embeddings comparison; healthcare procurement
NLP. **No paper, report, thesis, or repo makes this claim about procurement text.**

The two nearest things I found, and why neither supports the claim:

1. **Saha, Rohan. "Influence of various text embeddings on clustering performance in NLP."
   arXiv:2305.03144, 4 May 2023.** Single author, **preprint, no peer review indicated**.
   Domain is **e-commerce product reviews** (star-rating/review-text mismatch), **not procurement**.
   Clustering algorithms are KMeans, single-linkage agglomerative, DBSCAN and HDBSCAN — **GMM is
   not among them**. Metrics: silhouette score, adjusted Rand index, cluster purity.
   A secondary source reports the finding that "when using Word2Vec–Average embeddings, the
   silhouette score is greater than those obtained... on BERT-CLS and BERT-Average embeddings when
   the number of clusters is greater than 3." **But the paper's own abstract states: "the type of
   embedding chosen drastically affects the performance of the algorithm" and "no embedding type is
   better than the other."** So the paper explicitly declines to make the claim someone may have
   extracted from it. Also note silhouette is an **internal** cohesion metric — a higher silhouette
   for Word2Vec means its space is more compactly clusterable, **not** that the clusters are more
   correct. ARI and purity are the metrics that would settle it, and the abstract does not claim a
   Word2Vec win on those.

2. **RAND PE-A926-1** (F1.3) is the *strongest real evidence* in this direction, and it says
   something importantly different and more defensible:
   - pretrained general-corpus embeddings are "too generic";
   - basic TF-IDF "performed as well as or better than more-complex approaches" on jargon-dense
     defense text;
   - custom domain-trained embeddings "**vastly outperformed pretrained general-corpus embeddings
     on analogy and word-similarity tasks but did not improve performance on a text classification
     task**."

**My recommendation: strike the Word2Vec+GMM claim from the project notes and replace it with the
RAND citation.** RAND says what you actually want to say (simple lexical methods hold up on
jargon-heavy defense text; general pretrained embeddings disappoint), it is institutional, it is
about defense text specifically, it is free to retrieve, and it is honest about the limits
(custom embeddings did not help downstream). The Word2Vec+GMM framing is stronger than the
evidence and will not survive a technically literate reviewer.

---

# CONSOLIDATED RECOMMENDATIONS FOR NAADAP

**Adopt:**
1. **BM25 / TF-IDF lexical core**, with IDF computed over a frozen in-container procurement corpus.
   Justified by RAND PE-A926-1 (TF-IDF as good or better on jargon text) and by Zhang/Zhao/LeCun
   (n-gram TF-IDF is the stronger model below ~10^5–10^6 records). Both C#-native and exactly
   deterministic.
2. **Character n-grams in the tokenizer.** Justified by Muir & Reich's finding that procurement
   text is "terse and contains irregularities such as abnormal character combinations and
   misspellings." Deterministic.
3. **Domain bigram/n-gram tokens** (`performance_work_statement`, `SeaPort_NxG`), mined from the
   unlabeled corpus. RAND explicitly recommends this and says it "definitely improved" performance.
4. **Do NOT stem or remove stopwords by default** on this text. RAND observed information *loss*
   from stemming jargon. Make it a configurable, A/B-tested switch, and report the result.
5. **One-shot deterministic diversity seed selection** instead of an active learning loop
   (Lowell et al. 2019's successor-model problem; Ghose & Nguyen 2024's warm-up cost).
6. **SetFit's pair-expansion idea without SetFit** — turn N labels into O(R·N) pairwise
   same-vehicle/different-vehicle constraints and fit a deterministic linear reweighting.
7. **Evaluate with AIRC's metric**: micro-averaged coverage QI = Σ|R∩G|/Σ|G|, plus the count of
   items with <50% coverage, plus a named list of misses. Calibrate against their **61–87%**
   top-10 result on an easier task with richer ground truth.
8. **Budget SME time for adjudicating model-vs-label disagreements**, not just for labeling
   (RAND: the SME sided with the model ~50% of the time on flagged errors).

**Consider, with eyes open:**
9. A **distilled static embedding table** (Model2Vec-style) as a *build-time* artifact shipped in
   the container: ~8–30 MB, pure lookup + average at runtime, exactly deterministic in C#,
   combined with **SIF weighting** (Arora et al., ICLR 2017). Expect it to trail a transformer by
   ~4–5 points (Model2Vec's own benchmarks) and possibly to add nothing over good TF-IDF on DoD
   jargon (RAND). **Gate it behind an ablation.**

**Reject:**
10. Character-level CNN (needs millions of records — the authors of the method say so).
11. Active learning loops (negative results; successor-model bias destroys label-set reuse).
12. Co-training (no independent sufficient views exist for a SOW).
13. Self-training / pseudo-labeling on a scraped SAM.gov pool (Oliver et al. 2018: out-of-
    distribution unlabeled data degrades performance substantially — and that pool is full of it).
14. SetFit as a shipped component (Python/PyTorch at train time; defending "a sentence transformer
    is not an LLM" is a fight you don't need).

**Positioning fact worth using in the write-up:**
18F/GSA's own market research tool for the OASIS multiple-award vehicles (`18F/discovery`,
CC0, archived Aug 2019) did **structured NAICS/set-aside code filtering with no text processing at
all**, and GSA's current Market Research As a Service is a **human analyst service** with no
published algorithm. NAADAP reading the requirement *text* to rank vehicles is genuinely
unoccupied ground.

---

# CITATION LEDGER — verification status

| # | Citation | How verified |
|---|---|---|
| 1 | Muir & Reich 2021, INFORMS J. Appl. Analytics 51(6):463–479, DOI 10.1287/inte.2021.1098 | ✅ Crossref API: authors, affiliations, vol/iss/pages, full abstract |
| 1b | fscpsc.com architecture: char-CNN, hierarchical, ONNX Runtime, Go, CC0, github.com/wamuir | ✅ /about page retrieved |
| 1c | Muir & Reich **accuracy numbers** | ❌ **NOT RETRIEVED** — paywalled, 403 to all fetch attempts, no preprint found |
| 1d | "Westermeyer" as third author | ❌ **DISCONFIRMED** — not an author; no such co-authored work found |
| 2 | Hendrycks, Burns, Chen, Ball — CUAD, arXiv:2103.06268, NeurIPS 2021 | ✅ arXiv abs page |
| 3 | Chalkidis et al. — LexGLUE, arXiv:2110.00976, ACL 2022 | ✅ arXiv abs page |
| 4 | Koreeda & Manning — ContractNLI, Findings EMNLP 2021, 1907–1919, DOI 10.18653/v1/2021.findings-emnlp.164 | ✅ ACL Anthology page |
| 5 | Schirmer et al. — RAND PE-A926-1, July 2021 | ✅ full PDF downloaded and text-extracted |
| 6 | Ramirez-Marquez et al. — AIRC-2026-TR-005 / WRT-2507.11B, 6 Apr 2026 | ✅ full PDF downloaded and text-extracted |
| 6b | That report's DTIC accession AD1357794 | ⚠️ **UNVERIFIED** — from filename only; PDF is dated 2026 |
| 7 | Ramirez-Marquez, Lopez, Chan — AIRC WRT-1097.18.1-v1.1, June 2025 | ✅ acqirc.org publication page |
| 8 | Landale, Apte, Rendon, Salmerón — JDAL 1(2):151–171, DOI 10.1108/jdal-06-2017-0010 | ✅ Crossref (note: regression, not NLP) |
| 9 | Zhang, Zhao, LeCun — arXiv:1509.01626, NIPS 2015 | ✅ arXiv abs + full PDF; dataset-size dichotomy quoted verbatim |
| 10 | Kaur, Stefanucci, Shah — InProC, arXiv:2305.13532 | ✅ arXiv abs page; affiliations & venue UNVERIFIED |
| 11 | GSA/srt-ui, GSA/srt-api, GSA/srt-fbo-scraper, GSA/srt-ml | ✅ repo pages + raw README |
| 12 | 18F/discovery — CC0, archived 2019-08-27, no NLP | ✅ repo page |
| 13 | GSA MRAS — human service, no published algorithm | ✅ gsa.gov / buy.gsa.gov pages |
| 14 | abigailhaddad/govdocs + HF sam-solicitation-documents | ✅ repo page; doc counts/date range/license NOT stated |
| 15 | Settles — AL Literature Survey, UW-Madison CS TR1648, 2009 | ✅ minds.wisc.edu repository record |
| 16 | Settles — *Active Learning*, Synthesis Lectures 2012, DOI 10.1007/978-3-031-01560-1 | ✅ Crossref |
| 17 | Lewis & Gale — SIGIR '94, 3–12, DOI 10.1007/978-1-4471-2099-5_1 | ✅ Crossref |
| 18 | Seung, Opper, Sompolinsky — COLT '92, 287–294, DOI 10.1145/130385.130417 | ✅ Crossref |
| 19 | Settles & Craven — EMNLP 2008, 1070–1079, D08-1112 | ✅ full PDF; Table 2 read directly |
| 20 | Attenberg & Provost — SIGKDD Explorations 12(2):36–41, DOI 10.1145/1964897.1964906 | ✅ ACM DL record |
| 21 | Lowell, Lipton, Wallace — EMNLP-IJCNLP 2019, 21–30, D19-1003, DOI 10.18653/v1/D19-1003 | ✅ ACL Anthology page |
| 22 | Ghose & Nguyen — arXiv:2403.15744 | ✅ arXiv abs page; **EMNLP 2024 venue UNVERIFIED** |
| 23 | Tunstall et al. — SetFit, arXiv:2209.11055 | ✅ arXiv abs + ar5iv HTML result tables |
| 24 | Blum & Mitchell — COLT '98, 92–100, DOI 10.1145/279943.279962 | ✅ Crossref |
| 25 | Nigam & Ghani — CIKM 2000, 86–93, DOI 10.1145/354756.354805 | ✅ Crossref |
| 26 | Oliver, Odena, Raffel, Cubuk, Goodfellow — NeurIPS 2018, 3239–3250 | ✅ NeurIPS proceedings + dblp |
| 27 | Chen et al. — EMNLP 2021, 9125–9135, arXiv:2110.01256 | ✅ ACL Anthology + arXiv |
| 28 | MinishLab/model2vec — MIT, Python, Zenodo 10.5281/zenodo.17270888 | ✅ repo page + results/README.md |
| 28b | Model2Vec internals = PCA + Zipf weighting | ⚠️ **PARTIALLY VERIFIED** — from secondary sources, not the repo README |
| 29 | Robertson & Zaragoza — FnTIR 4:1–174 (2009), DOI 10.1561/1500000019 | ✅ Crossref |
| 30 | Joulin, Grave, Bojanowski, Mikolov — EACL 2017, 427–431, DOI 10.18653/v1/e17-2068 | ✅ Crossref |
| 31 | Arora, Liang, Ma — ICLR 2017 (SIF) | ✅ dblp conf/iclr/AroraLM17 + Princeton portal; OpenReview bot-blocked |
| 32 | **Word2Vec+GMM beat transformers on procurement text** | ❌ **UNVERIFIED — no source found. Recommend striking the claim.** |
| 33 | MITRE / IDA / DAU technical reports on acquisition NLP | ❌ **None found publicly.** |
| 34 | NPS DAIR acquisition-NLP papers (SAR sentiment, Urgent Need Statements) | ⚠️ **UNVERIFIED** — search snippets only; DAIR search endpoint 404'd |
