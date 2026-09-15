# D — Knowledge Graph / Ontology Construction, Serialization, and Provenance

Research area D for the NAADAP vehicle knowledge base.
Prepared 2026-09-15. Every citation in this document was retrieved during
this research session; retrieval status is recorded per item in §9.
Items I could not retrieve in full text are marked **PARTIALLY VERIFIED**
or **UNVERIFIED** at the point of use.

---

## 0. Bottom line up front

**Do not build an RDF/OWL knowledge graph for the NAADAP vehicle knowledge
base.** Build a versioned, hashed, columnar knowledge base (CSV/TSV or JSONL
with a frozen column order) with explicit provenance columns, validated at
build time by a hand-written schema checker, and shipped as an immutable
resource in the container. Borrow four specific ideas from the semantic-web
stack — SHACL's *validation-report* discipline, PROV-O's *entity / activity /
agent* triad and its `wasDerivedFrom` / `wasRevisionOf` / `hadPrimarySource`
vocabulary as **column names**, SKOS's `broader` / `narrower` /
`prefLabel` / `altLabel` discipline for the PSC/NAICS taxonomy, and RDFC-1.0's
insistence on a canonical byte form before hashing — without importing the
machinery. Full argument in §8.

**Do not use knowledge-graph embeddings (TransE/DistMult/ComplEx/RotatE).**
They are architecturally incompatible with the ≥95 % determinism requirement
(random initialization + random negative sampling + SGD), they need orders of
magnitude more triples than NAADAP will have, and the link-prediction
literature they come from has documented evaluation pathologies that make
their reported numbers unusable as evidence. Full argument in §6.

**Do not use an off-the-shelf Open IE system to populate the knowledge base.**
Beyond the accuracy problem (§1), ReVerb and Ollie are non-commercial-only and
patent-encumbered, and Stanford CoreNLP and OpenIE6 are GPL-3.0. §1.7.

---

## 1. Knowledge graph construction from text

### 1.1 The four generations of Open IE, and what each actually does

**Generation 1 — TextRunner (2007).**
Banko, M., Cafarella, M. J., Soderland, S., Broadhead, M., Etzioni, O.
"Open Information Extraction from the Web." *IJCAI 2007*, pp. 2670–2676.
Retrieved: https://www.ijcai.org/Proceedings/07/Papers/429.pdf

Mechanism, verbatim from the paper's §2:

1. **Self-Supervised Learner.** Parses a few thousand sentences with a
   dependency parser (Klein & Manning 2003). For each sentence it finds all
   *base noun phrase* constituents `e_i`; for every ordered pair
   `(e_i, e_j), i < j` it walks the parse structure between them to get a
   candidate relation string `r_ij`, producing a tuple `t = (e_i, r_ij, e_j)`.
   `t` is labelled **positive** if heuristic constraints hold — the paper
   lists: the dependency chain between `e_i` and `e_j` is no longer than a
   fixed length; the path does not cross "a sentence-like boundary (e.g.
   relative clauses)"; neither argument is solely a pronoun — and **negative**
   otherwise. It then trains a **Naive Bayes** classifier on domain-independent,
   parser-free features: POS-tag sequences inside `r_ij`, token count of
   `r_ij`, stopword count in `r_ij`, whether an argument is a proper noun, the
   POS tag left of `e_i`, the POS tag right of `e_j`.
2. **Single-Pass Extractor.** POS-tags and NP-chunks the whole corpus (no
   parser), generates candidate tuples, keeps those the classifier calls
   trustworthy.
3. **Redundancy-Based Assessor.** Assigns a probability from a redundancy
   model (Downey et al. 2005) — a tuple seen in many distinct sentences is
   more likely true.

Reported results on a 9 M-page Web corpus (133 M sentences): after filtering,
11.3 M tuples over 278,085 distinct relation strings. 7.8 M tuples had a
well-formed relation *and* well-formed arguments at probability ≥ 0.8; of
those, **80.4 % were judged correct**, split into ~1 M *concrete* tuples at
88.1 % correct and ~6.8 M *abstract* tuples at 79.2 % correct. Against
KNOWITALL over the same corpus, TextRunner had a **33 % lower average error
rate** at comparable recall.

The three properties that matter for NAADAP: the correctness figure is *per
tuple in aggregate*, the assessor's confidence is **frequency-derived** (it
needs redundancy across a Web-scale corpus — NAADAP has a 20-document set),
and the arguments are raw strings, not linked entities.

**Generation 2 — ReVerb (2011).**
Fader, A., Soderland, S., Etzioni, O. "Identifying Relations for Open
Information Extraction." *EMNLP 2011*, Edinburgh, pp. 1535–1545.
ACL Anthology ID **D11-1142**.
Retrieved (full text): https://reverb.cs.washington.edu/emnlp11.pdf
and https://aclanthology.org/D11-1142/

ReVerb's contribution is two constraints, both mechanical:

*Syntactic constraint* — the relation phrase must match this POS regex
(Figure 1 of the paper, verbatim):

```
V | V P | V W* P
V = verb particle? adv?
W = (noun | adj | adv | pron | det)
P = (prep | particle | inf. marker)
```

Longest match wins; adjacent matching spans are merged (so "wants to extend"
is one relation phrase); the relation phrase must be a **contiguous span** and
must appear **between its two arguments**.

*Lexical constraint* — a relation phrase is kept only if it takes at least
*k* distinct argument pairs in a large corpus (the paper uses a dictionary of
relation phrases built offline), which filters "overspecified" relations.

The failure modes ReVerb was built to kill, quantified by the paper:

| Failure mode | TextRunner | WOE^pos | WOE^parse | ReVerb |
|---|---|---|---|---|
| Incoherent extractions | 13 % | 15 % | 30 % | — |
| Uninformative extractions | 7 % | 5–6 % | 4 % | 1 % |

*Incoherent* = the relation phrase has no meaningful interpretation, e.g.
from "The Mark 14 was central to the torpedo scandal of the fleet",
TextRunner returns the relation `was central torpedo`. *Uninformative* = a
light-verb construction is truncated to the bare verb, so "Faust made a deal
with the Devil" yields `made` instead of `made a deal with`.

Headline results: ReVerb's **AUC is 30 % higher than WOE^parse and more than
double** that of WOE^pos or TextRunner; the lexical constraint alone is worth
**23 % AUC** over ReVerb¬lex and cuts overspecified extractions from 20 % to
1 % of output. **More than 30 % of ReVerb's extractions are at precision
≥ 0.8, "compared to virtually none for the other systems."** WOE^parse reaches
slightly higher recall (0.64 vs 0.62) at lower precision. Speed on 100,000
sentences on a Pentium 4 / 4 GB: ReVerb 16 min, TextRunner 21 min, WOE^pos
21 min, WOE^parse **11 hours** (the parser dominates).

**ReVerb's own error analysis — the single most important table for NAADAP**
(paper Tables 5 and 6):

Incorrect extractions returned: **65 % correct relation phrase, incorrect
arguments**; 16 % n-ary relation forced into a binary tuple (from "I gave him
15 photographs" it extracts `(I, gave, him)`); 8 % non-contiguous relation
phrase; 2 % imperative verb; 2 % overspecified; 7 % other (POS/chunking).

Correct extractions missed: **52 % could not identify correct arguments**;
23 % relation filtered out by the lexical constraint; 17 % identified a more
specific relation; 8 % POS/chunking error.

So the dominant error in both directions is **argument boundary detection**,
not relation identification. For NAADAP that is fatal, because the arguments
are exactly the thing you need: "ordering period ends 30 September 2029",
"ceiling of $5,300,000,000", "NAICS 541330". Getting the relation right and
the argument wrong produces a knowledge-base row that is *confidently and
specifically* wrong — the exact failure mode the team was already burned by.

At scale, ReVerb's frequency-1 extractions had precision **0.75**; TextRunner
could not match that even at frequency 10 (precision 0.34). Again: the
precision story depends on redundancy NAADAP does not have.

**Generation 2.5 — Ollie (2012).**
Mausam, Schmitz, M., Soderland, S., Bart, R., Etzioni, O. "Open Language
Learning for Information Extraction." *EMNLP-CoNLL 2012*, Jeju Island, Korea,
pp. 523–534. ACL Anthology ID **D12-1048**.
Retrieved (metadata): https://aclanthology.org/D12-1048/

Mechanism: Ollie takes ReVerb's high-confidence tuples as **seeds**,
finds every sentence in a large corpus containing the seed's argument pair and
relation content words, builds a bootstrap training set from them, and learns
**open pattern templates over dependency paths** (rather than over surface POS
patterns). Those templates are applied at extraction time. This buys two
things ReVerb structurally cannot have: relations not mediated by a verb
(noun-mediated, e.g. "Obama, the president of the US"), and **context
attribution / clausal modifiers** — Ollie can emit `AttributedTo` and
`ClausalModifier` fields, so "Early reports say Romney won" is not asserted as
"Romney won".

That second feature is the one thing in the Open IE line that is genuinely
relevant to procurement prose, which is saturated with conditionals and
attributions ("the Contracting Officer may, at his discretion, …"). But see
§1.7 — Ollie's licence makes it unusable here.

**Generation 3 — Stanford OpenIE (2015).**
Angeli, G., Johnson Premkumar, M. J., Manning, C. D. "Leveraging Linguistic
Structure For Open Domain Information Extraction." *ACL-IJCNLP 2015*, Beijing,
pp. 344–354. ACL Anthology ID **P15-1034**.
Retrieved (full text): https://aclanthology.org/P15-1034.pdf

Mechanism, in three stages:

1. **Clause splitting.** A **multinomial logistic regression** classifier walks
   the dependency tree and, at each edge, chooses an action: yield a new clause
   rooted here; yield a clause here but have it *inherit* the governing clause's
   subject or object (for controlled subordinate clauses); or do not yield.
   Training data is collected distantly from a subset of KBP source documents.
   Output: a set of short, self-contained clauses.
2. **Natural-logic shortening.** Each clause is shortened by deleting
   modifiers, but only along edges where **natural logic** guarantees the
   shortened form is entailed by the longer one (monotonicity-licensed
   deletion). This is what makes the system emit `(cats, play, grass)` from
   "*all young cats play with yarn in the grass*" *soundly* rather than
   heuristically.
3. **Segmentation into triples.** Six dependency patterns segment an atomic
   sentence into (subject, relation, object) — Table 2 of the paper. With named
   entity information available, 5 more dependency patterns and 3 TokensRegex
   surface patterns extract *nominal* relations (Table 3).

**End-to-end results on TAC-KBP 2013 Slot Filling** (Table 5 of the paper) —
this is the number to quote, because it measures exactly the NAADAP task:
run open IE on real documents, map the open relations into a **fixed schema**,
and score the resulting KB entries.

| System | P | R | F1 |
|---|---|---|---|
| UW official 2013 submission | 69.8 | 11.4 | 19.6 |
| Ollie (in their framework) | 57.4 | 4.8 | 8.9 |
| Ollie + nominal rels | 57.7 | 11.8 | 19.6 |
| Their system, − nominal rels | 64.3 | 8.6 | 15.2 |
| Their system, + nominal rels | 61.9 | 13.9 | **22.7** |
| + alt. name detector | 57.8 | 17.8 | 27.1 |
| + alt. name + website | 58.6 | 18.6 | 28.3 |

**Recall is 9–19 %.** Precision is 58–64 %. That is the state of the art for
"open IE, then map to a schema" on curated newswire with a fixed, well-studied
relation inventory and a team that had been doing KBP for years. A NAADAP
knowledge-base field populated at 60 % precision / 15 % recall is not
defensible in a legal determination; it is a liability.

Note also the mapping step: they built the open-IE→KBP relation map by
computing **PMI²** between open relations and KBP relations over a distantly
labelled corpus, conditioned on type signatures matching. Even with that, and
with a human annotator spending ~1 day on a manual mapping, the ceiling was
22.7 F1.

**Generation 4 — OpenIE6 (2020).**
Kolluru, K., Adlakha, V., Aggarwal, S., Mausam, Chakrabarti, S. "OpenIE6:
Iterative Grid Labeling and Coordination Analysis for Open Information
Extraction." *EMNLP 2020*, pp. 3748–3761.
Retrieved (full text): https://aclanthology.org/2020.emnlp-main.306.pdf
arXiv:2010.03147

Mechanism: casts Open IE as a **2-D grid labeling** problem. A BERT encoder
(BERT-base-cased for the extractor, BERT-large-cased for coordination
analysis) produces token embeddings; an iterative labeling head emits one row
of BIO-style labels per extraction, conditioning each row on the previous
ones, which gets IMoJIE-style iterative behaviour without IMoJIE's autoregressive
decoding cost. A separate **coordination analyzer** (IGL-CA), same
architecture, splits conjunctions. Soft constraints (POS coverage, head-verb
coverage, extraction count, argument-entailment) are added to the loss.

Results on CaRB (Table 2 of the paper), the relevant columns:

| System | CaRB F1 | CaRB AUC | Sentences/sec |
|---|---|---|---|
| MinIE | 41.9 | — | 8.9 |
| ClausIE | 45.0 | 22.0 | 4.0 |
| OpenIE4 | 51.6 | 29.5 | 20.1 |
| OpenIE5 | 48.0 | 25.0 | 3.1 |
| RnnOIE | 49.0 | 26.0 | 149.2 |
| IMoJIE | 53.5 | 33.3 | 2.6 |
| CIGL-OIE | 54.0 | 35.7 | 142.0 |
| **OpenIE6 (CIGL-OIE + IGL-CA)** | 52.7 | 33.7 | 31.7 |

**After thirteen years of work, the best Open IE system scores ~53 F1** on a
benchmark built from ordinary English. It also requires a transformer
encoder — out of scope for NAADAP's 1 CPU / 2 GB / no-LLM constraints — and
is GPL-3.0 (§1.7).

### 1.2 Distant supervision and its noise problem

**Mintz, M., Bills, S., Snow, R., Jurafsky, D. "Distant supervision for
relation extraction without labeled data." *ACL-IJCNLP 2009*, Suntec,
Singapore, pp. 1003–1011.** ACL Anthology ID **P09-1113**.
Retrieved (full text): https://aclanthology.org/P09-1113.pdf

Mechanism:

- Take a KB (Freebase). After filtering, 1.8 M instances of 102 relations over
  940 K entities.
- **The distant supervision assumption**, stated verbatim in the paper: "if two
  entities participate in a relation, any sentence that contain those two
  entities might express that relation."
- Run an NER tagger over an unlabelled corpus (Wikipedia: ~1.8 M articles,
  601,600,703 tokens; 800 K articles for training, 400 K for testing). Whenever
  a sentence contains both members of a known KB pair, extract features from
  that sentence and add them to the **feature vector for the relation**, merging
  features across all sentences that mention the same `(relation, e1, e2)` tuple.
- Train a **multiclass logistic regression** classifier over those merged
  feature vectors. Features are lexical (word sequence between the entities,
  windows, POS) and syntactic (dependency path between the entities).
- At test time, every co-occurring entity pair in a sentence is a candidate;
  features from all sentences mentioning that pair are merged and classified.

Headline: "10,000 instances of 102 relations at a precision of 67.6 %."

**The noise problem, quantified.** The Mechanical Turk human evaluation
(paper §7.2) over the 10 most frequent test relations, best-100 and best-1000
instances, gives per-relation precision from **0.42 to 0.89**:

| Relation | Syn@100 | Lex@100 | Both@100 | Both@1000 |
|---|---|---|---|---|
| /film/director/film | 0.49 | 0.43 | 0.44 | 0.46 |
| /film/writer/film | 0.70 | 0.60 | 0.65 | 0.69 |
| /geography/river/basin_countries | 0.65 | 0.64 | 0.67 | 0.64 |
| /location/country/administrative_divisions | 0.68 | 0.59 | 0.70 | 0.72 |
| /location/location/contains | 0.81 | 0.89 | 0.84 | 0.84 |
| /location/us_county/county_seat | 0.51 | 0.51 | 0.53 | 0.42 |
| /music/artist/origin | 0.64 | 0.66 | 0.71 | 0.60 |
| /people/deceased_person/place_of_death | 0.80 | 0.79 | 0.81 | 0.78 |
| /people/person/nationality | 0.61 | 0.70 | 0.72 | 0.63 |
| /people/person/place_of_birth | 0.78 | 0.77 | 0.78 | — |

Read that as: even for the easiest relations on the cleanest corpus with the
largest available KB, **one in five to one in two extracted facts is wrong.**

The assumption is the cause. A sentence containing both "Steven Spielberg" and
"Saving Private Ryan" need not express `director`; it may express
`produced`, `was criticised for`, or nothing at all. The label is applied to
*every* such sentence, so the training signal is polluted at exactly the rate
at which the KB pair co-occurs for non-KB reasons.

**Riedel, S., Yao, L., McCallum, A. "Modeling Relations and Their Mentions
without Labeled Text." *ECML PKDD 2010*, Part III, LNCS 6323, pp. 148–163.**
DOI 10.1007/978-3-642-15939-8_10.
Retrieved (metadata only — **PARTIALLY VERIFIED**, full text not retrieved):
https://dl.acm.org/doi/10.1007/978-3-642-15939-8_10 and
https://link.springer.com/chapter/10.1007/978-3-642-15939-8_10

The fix: replace "*any* sentence expresses the relation" with
"*at least one* sentence expresses the relation". Modelled as a factor graph
with a latent variable per mention deciding whether *that* sentence expresses
the relation, plus a variable deciding whether the pair is related at all,
trained with constraint-driven semi-supervision. Reported **31 % error
reduction** over the Mintz-style baseline on NYT + Freebase.

**Why this whole line is wrong for NAADAP.** Distant supervision requires a
pre-existing KB of true `(vehicle, relation, value)` facts to bootstrap from.
If you had that KB you would not need the extractor. NAADAP's vehicle
knowledge base *is* the KB; it has to be authored, not bootstrapped. The
honest framing is: NAADAP has a **cold-start KB construction problem with no
seed KB and no redundant corpus**, which is the single worst configuration for
every technique in §1.1 and §1.2.

### 1.3 Relation extraction as classification

The supervised alternative: fix a relation inventory, hand-label mention pairs,
train a classifier. The NIST ACE RDC 2003/2004 corpora — over 1,000 documents,
5–7 major relation types with 23–24 subrelations, **16,771 labelled relation
instances** (figure quoted from Mintz et al. §1) — are the canonical training
resource, and they exist because a funded multi-year government programme paid
for them.

For NAADAP, this reduces to a cost question: how many labelled instances per
field, produced by someone who can read a SOW and a J&A correctly? The CUAD
data point (§1.6) is the right anchor — and CUAD's answer is that expert
annotation of 41 clause types over 510 contracts produced 13,000+ annotations
after **70–100 hours of training per annotator**, and the best model still
reaches only 44 % precision at 80 % recall.

The correct conclusion is not "train a classifier"; it is "the field values are
few enough and stable enough that you should **transcribe them by hand from the
authoritative source document once, record where each came from, and re-check
on a schedule**." That is §7.

### 1.4 Entity linking

Canonical references (retrieved as metadata; **PARTIALLY VERIFIED** — I did not
retrieve full text for either):

- Ratinov, L., Roth, D., Downey, D., Anderson, M. "Local and Global Algorithms
  for Disambiguation to Wikipedia." *ACL-HLT 2011*, Portland, pp. 1375–1384.
  ACL Anthology ID **P11-1138**. https://aclanthology.org/P11-1138/
- Hoffart, J., et al. "Robust Disambiguation of Named Entities in Text."
  *EMNLP 2011*, Edinburgh, pp. 782–792 (the AIDA system).

Mechanism, common to both: generate candidate KB entities per surface mention
from an alias dictionary (Wikipedia anchor text / redirects); score each
candidate by a **local** compatibility between the mention's context and the
candidate's KB description, plus a **global** coherence term that rewards
candidate sets that are mutually related in the KB link graph; solve the
resulting joint assignment (AIDA: dense-subgraph on a mention–entity graph;
Ratinov et al.: local ranker + global re-ranker).

**Bast, H., Hertel, M., Prange, N. "A Fair and In-Depth Evaluation of Existing
End-to-End Entity Linking Systems." arXiv:2305.14937.**
Retrieved (full text): https://arxiv.org/pdf/2305.14937
Its finding is directly load-bearing here: the widely used benchmarks
(AIDA-CoNLL among them) "have strong biases and artifacts" — a strong bias
toward a small set of prominent entities, and inconsistent handling of
non-named / non-KB mentions — which linkers can exploit, so aggregate P/R/F1
on those benchmarks "say little about how the system is going to perform for a
particular application."

**Why entity linking is inapplicable to NAADAP as normally understood.** Every
system in this line disambiguates *to Wikipedia / DBpedia / Freebase / YAGO /
Wikidata*. SeaPort-NxG, the NAWCAD MACs, specific PMA offices, specific PSC
and NAICS codes, and DoDAACs are either absent from those KBs or present as
stubs with no usable link structure, so the global coherence term — the thing
that makes these systems work — has nothing to operate on.

What NAADAP actually needs is **gazetteer resolution against its own registry**:
a controlled list of vehicle identifiers with `prefLabel` / `altLabel` /
`hiddenLabel` (§3.3), matched deterministically. That is a dictionary lookup
with normalization rules, not entity linking, and it is *better* than entity
linking for this purpose because it is exactly reproducible and every match
traces to a registry row.

### 1.5 Coreference

**Zhu, Y., Pradhan, S., Zeldes, A. "OntoGUM: Evaluating Contextualized SOTA
Coreference Resolution on 12 More Genres." *ACL-IJCNLP 2021* (Short Papers),
pp. 461–467.** ACL Anthology ID **2021.acl-short.59**. arXiv:2106.00933.
Retrieved (metadata + abstract): https://aclanthology.org/2021.acl-short.59/

The result to carry: SOTA neural coreference systems trained on OntoNotes
degrade by roughly **15–20 points** when evaluated out of domain across 12
genres, and the degradation affects both deterministic and neural systems,
"indicating a lack of generalizability or covert overfitting."

Procurement documents are as far out of OntoNotes' domain as text gets:
heavy nominal anaphora to defined terms ("the Contractor", "the Government",
"the Ordering Activity"), document-internal cross-reference ("as defined in
Section C.3.2"), and pronouns that refer to clauses rather than entities.
Assume out-of-the-box coreference is unusable and do not put it in the core
path. If you need "the Contractor" resolved, you resolve it by **reading the
document's definitions section** — a deterministic, auditable rule.

### 1.6 Where all of this breaks on formal / legal prose — the hard number

**Hendrycks, D., Burns, C., Chen, A., Ball, S. "CUAD: An Expert-Annotated NLP
Dataset for Legal Contract Review." *NeurIPS 2021 Datasets and Benchmarks
Track*.** arXiv:2103.06268.
Retrieved (full text): https://arxiv.org/pdf/2103.06268

The dataset: 510 contracts, 41 clause categories, **13,000+ expert
annotations**, produced by law-student annotators who first went through
**70–100 hours of training** designed by experienced lawyers, with video
instructions, live workshops and quizzes. The paper notes large-firm billing
rates of roughly **$500–$900/hour** to contextualise the cost of doing this
work manually.

The task is precisely NAADAP-shaped: highlight the span(s) of a long formal
document that answer a specific structured question.

Results (Table 2 of the paper):

| Model | AUPR | Precision @ 80 % Recall | Precision @ 90 % Recall |
|---|---|---|---|
| BERT-base | 32.4 | 8.2 | 0.0 |
| BERT-large | 32.3 | 7.6 | 0.0 |
| ALBERT-base | 35.3 | 11.1 | 0.0 |
| ALBERT-xxlarge | 38.4 | 31.0 | 0.0 |
| RoBERTa-base | 42.6 | 31.1 | 0.0 |
| RoBERTa-base + contracts pretraining | 45.2 | 34.1 | 0.0 |
| RoBERTa-large | 48.2 | 38.1 | 0.0 |
| **DeBERTa-xlarge** | **47.8** | **44.0** | **17.8** |

Two further findings that matter:

- Pretraining RoBERTa-base on ~8 GB of EDGAR contracts raised AUPR by only
  **~3 points**. Domain-adaptive pretraining on gigabytes of in-domain text is
  nearly worthless compared to a few thousand expert annotations. Read that as
  a direct instruction: **spend the effort on curation, not on modelling.**
- Per-category AUPR for the best model ranges from near 100 % down to **~20 %**.
  Aggregate numbers hide categories where the model is useless.

For NAADAP this settles the question. If transformer models with 100M–1B
parameters achieve 44 % precision at 80 % recall on span extraction from legal
contracts, then a 1-CPU / 2-GB / no-LLM pipeline is not going to extract
"remaining ceiling headroom" or "ordering period end date" from a procurement
document at a quality that survives a legal challenge. Those values must come
from an **authored knowledge-base row with a citation**, and document text may
at most be used to *match against* that row.

### 1.7 Licence and patent status of the Open IE toolchain — a hard blocker

Verified by fetching each repository's licence file on 2026-09-15:

| Repo | Language | Licence | Status |
|---|---|---|---|
| `knowitall/reverb` | Java, 545★ | **"ReVerb Software License Agreement"** — non-commercial only, UW patent-encumbered | Effectively unmaintained (2011-era) |
| `knowitall/ollie` | Scala, 253★ | **"Ollie Software License Agreement"** — *"Ollie is not used for any commercial purposes, or as part of a system which has commercial purposes"*; explicitly claims **US patent 7,877,343 (issued) and 12/970,155 (pending)**; commercial use requires negotiating a licence and fee with UW CoMotion | Effectively unmaintained (2012-era) |
| `stanfordnlp/CoreNLP` | Java, 10,115★ | **GPL-3.0** | Active |
| `dair-iitd/openie6` | Python, 128★ | **GPL-3.0** | Low activity |
| `RDFLib/pySHACL` | Python, 348★ | Apache-2.0 | Active |
| `TopQuadrant/shacl` | Java, 246★ | Apache-2.0 | Active |

ReVerb and Ollie are **not open source** and are patent-encumbered; a prize
challenge deliverable is a commercial purpose under any reasonable reading.
CoreNLP and OpenIE6 are GPL-3.0, which for a container-shipped deliverable
means the whole distributed work is subject to GPL-3.0 copyleft obligations.
None of them is a .NET library anyway. **Do not put any of them in NAADAP.**

---

## 2. Ontology learning from text

### 2.1 The layer cake

**Buitelaar, P., Cimiano, P., Magnini, B. (eds.) *Ontology Learning from Text:
Methods, Evaluation and Applications*. Frontiers in Artificial Intelligence and
Applications, vol. 123. IOS Press, Amsterdam, 2005. v+171 pp. ISBN
1-58603-523-1.** The framing chapter is Buitelaar, Cimiano & Magnini,
"Ontology Learning from Text: An Overview," pp. 3–12.
**PARTIALLY VERIFIED** — I verified the book's bibliographic record via the
*Computational Linguistics* 32(4):569 book review page
(https://direct.mit.edu/coli/article/32/4/569/) but could not retrieve the
chapter's own full text; the layer enumeration below is corroborated from two
independent peer-reviewed sources that cite it (below).

The layer cake, bottom to top:

```
                general axioms          ← least automatable
                axiom schemata
                relation hierarchy
                relations  (non-taxonomic)
                concept hierarchy  (taxonomy / is-a)
                concepts
                synonyms
                terms                   ← most automatable
```

Each layer is a prerequisite for the one above it. Terms are the base because
they are "the smallest units in text and are the linguistic symbols to
represent concepts"; terms must be grouped into synonym sets before concepts
can be formed; concepts must exist before a hierarchy can be induced; and so on.

Corroborating sources I *did* retrieve in full:

- **Asim, M. N., Wasim, M., Khan, M. U. G., Mahmood, W., Abbasi, H. M.
  "A survey of ontology learning techniques and applications." *Database
  (Oxford)*, vol. 2018, 2018. DOI **10.1093/database/bay101**.
  Retrieved: https://pmc.ncbi.nlm.nih.gov/articles/PMC6173224/
- **Ontology Learning from Text: an Analysis on LLM Performance.** CEUR-WS
  Vol-3874, paper 5. Retrieved: https://ceur-ws.org/Vol-3874/paper5.pdf
  It states directly that Buitelaar et al. "divide the task into complexity
  levels, based on the ontology learning layer cake: Starting in complexity
  with terms, … up to rules and axioms," and that in its own experiment "the
  top steps of the ontology learning layer cake — rules and axioms … and the
  relation hierarchy, axioms schemata, and general axioms" were **not
  annotated**, i.e. left out of scope even for a 2024 LLM study.

### 2.2 What is realistically automatable

From the *Database* survey (full text retrieved), reported performance of
automated methods at the lower layers:

| Technique | Domain | Reported figure |
|---|---|---|
| Agglomerative clustering | medical corpus | 71 % precision |
| Formal Concept Analysis | medical corpus | 47 % precision |
| Hierarchical clustering | cooking recipes | 92.1 % precision |
| Lexico-syntactic (Hearst) patterns | NYT news | 75.55 % accuracy |
| Dependency analysis | bioinformatics | 83.3 % accuracy |
| Association rule mining | medical | 72.5 % accuracy |

And its conclusions, quoted from the retrieved text: "a hybrid approach
comprising of both linguistic and statistical techniques produces better
ontologies," but "a reasonable amount of post processing is required to boost
the quality of ontology, which is another massive drawback of fully automated
ontology acquisition." Human-based evaluation is the only method applicable at
all five levels, and it carries "high manual cost in terms of time and effort."

**Verdict for NAADAP.** Layers 1–2 (terms, synonyms) are automatable and worth
automating: harvesting the vocabulary of a procurement corpus and building an
alias table is a tractable, deterministic, dictionary-building exercise, and it
is exactly what you need for §1.4's gazetteer. Layer 3–4 (concepts, taxonomy)
should **not** be induced — the taxonomies NAADAP needs already exist, are
authoritative, and are published: PSC (Product and Service Codes), NAICS, the
FSC, the DoD organizational hierarchy. Inducing a taxonomy when an
authoritative one is published is strictly worse: it is non-deterministic,
unverifiable, and indefensible ("why is 541330 under this node?" — "because
agglomerative clustering put it there" is not an answer a contracting officer
will accept). Layers 5–8 (relations, relation hierarchy, axiom schemata,
general axioms) are not automatable at usable quality by anyone, and the
NAADAP relation inventory is small enough (a few dozen predicates) to be
authored in an afternoon.

There is a published critique of the layer cake itself —
**"Ontology Learning from Text: Why the Ontology Learning Layer Cake is not
Viable," *International Journal of Signs and Semiotic Systems* 4(2), 2015,
DOI 10.4018/ijsss.2015070101** — but the ACM DL page returned HTTP 403 and I
could not retrieve author names or abstract. **UNVERIFIED; do not cite without
retrieving.** The related conference paper "Departing the Ontology Layer Cake"
also appears in the literature but I did not verify it.

---

## 3. Serialization standards

### 3.1 RDF and RDF datasets

**RDF 1.1 Concepts and Abstract Syntax**, W3C Recommendation 25 February 2014,
https://www.w3.org/TR/rdf11-concepts/ — the stable base.

**RDF 1.2 Concepts and Abstract Data Model**, W3C **Candidate Recommendation
Snapshot, 07 April 2026**, https://www.w3.org/TR/rdf12-concepts/
Retrieved and verified 2026-09-15. W3C issued a call for implementations in
2026; it is **not yet a Recommendation**.

What RDF 1.2 adds that matters here:

- **Triple terms.** "An RDF triple used as the object of another triple."
  Formally: "If s is an IRI or a blank node, p is an IRI, and o is an RDF
  triple, then (s, p, o) is an RDF triple." Triple terms may appear **only in
  object position**.
- **Reification via `rdf:reifies`.** "A reifying triple is a triple where the
  predicate is `rdf:reifies` and the object is a triple term." The subject is
  called a **reifier** and denotes something *about* the proposition — a
  statement, a belief, a claim. Crucially, "RDF terms that appear in a triple
  term have the same denotation as when they appear in an asserted triple," and
  a triple term may be **asserted or unasserted**. This is the clean replacement
  for RDF 1.1's `rdf:Statement` / `rdf:subject` / `rdf:predicate` / `rdf:object`
  reification, which required four triples per reified statement and had no
  denotational guarantee.
- **Directional language-tagged strings** (base text direction).

**RDF datasets / named graphs**, verbatim from the CR: "An RDF dataset is a
collection of RDF graphs, and comprises: Exactly one default graph, being an
RDF graph. The default graph does not have a name and MAY be empty. Zero or
more named graphs." Each named graph pairs a graph name (IRI or blank node)
with a graph. The spec warns: "Despite the use of the word 'name' in 'named
graph,' the graph name is not required to denote the graph."

The originating paper: **Carroll, J. J., Bizer, C., Hayes, P., Stickler, P.
"Named graphs, provenance and trust." *WWW 2005*, Chiba, Japan, pp. 613–622.
DOI 10.1145/1060745.1060835.** Extended as "Named graphs," *Journal of Web
Semantics* 3(4):247–267, 2005, DOI 10.1016/j.websem.2005.09.001.
**PARTIALLY VERIFIED** — bibliographic record confirmed via ACM DL and
ScienceDirect listings; I did not retrieve the full text. The paper's stated
contribution is to extend RDF's syntax and semantics to cover RDF graphs
nameable by URIs, giving abstract syntax, formal semantics, an XML syntax and
an N3-based syntax, with **provenance as the motivating application** and the
whole framework proposed as a foundation for a Semantic Web trust layer.

### 3.2 OWL 2 profiles — and why the profile choice is the whole ballgame

**OWL 2 Web Ontology Language Profiles (Second Edition)**, W3C Recommendation
**11 December 2012**. Editors: Boris Motik, Bernardo Cuenca Grau, Ian Horrocks,
Zhe Wu, Achille Fokoue, Carsten Lutz. https://www.w3.org/TR/owl2-profiles/
Retrieved and verified 2026-09-15.

| Profile | Designed for | Key allowances | Key prohibitions | Consistency / subsumption / instance checking | Conjunctive query (data complexity) |
|---|---|---|---|---|---|
| **OWL 2 EL** | ontologies with very large numbers of classes/properties | existential quantification, self-restrictions, singleton enumerations, intersection, property chains, reflexive & transitive properties, functional data properties, keys | universal quantification, cardinality restrictions, disjunction, class negation, multi-element enumerations, inverse properties, symmetric/asymmetric properties | **PTime-complete** | **PTime-complete** |
| **OWL 2 QL** | very large instance data, query answering is the main task | limited class expressions positionally, property inverses, simple property inclusion, domain/range, reflexive/irreflexive/symmetric/asymmetric | existential quantification in subclass position, self-restrictions, property chains, functional properties, transitive properties, keys, individual equality assertions, cardinality restrictions | **NLogSpace-complete** | **AC⁰** |
| **OWL 2 RL** | scalable reasoning, rule-engine implementable | unions, intersections, enumerations, ∃ and ∀, cardinality 0/1, property chains, functional & transitive properties, keys | disjoint unions, reflexive properties | consistency **PTime-complete**; class satisfiability / subsumption / instance checking **co-NP-complete** (PTime for atomic) | **NP-complete** |
| **OWL 2 DL** (direct semantics) | full expressivity | — | — | **N2ExpTime-complete** | — |

**Why this matters for NAADAP specifically.** N2ExpTime is a double-exponential
worst case. That is not a "big constant"; it is a class where a small,
innocuous-looking ontology change can turn a 200 ms reasoner call into one that
does not terminate inside the 30-minute budget. Under a hard wall-clock SLA
(30 min for 20 documents) on **1 CPU core and 2 GB RAM**, admitting an OWL 2 DL
reasoner into the core path is admitting an unbounded-runtime component into a
system with a bounded-runtime requirement. If RDF/OWL were used at all, the
*only* defensible choice would be **OWL 2 RL implemented as forward-chaining
materialization at build time** (the profile explicitly designed for rule-engine
implementation), so that all inference happens offline in CI, is materialized
into the shipped artifact, and the runtime does zero reasoning. But at that
point the shipped artifact is a finite set of ground facts — i.e. a table — and
the ontology is a build-time convenience, not a runtime component. That is the
seed of the §8 verdict.

### 3.3 SKOS

**SKOS Simple Knowledge Organization System Reference**, W3C Recommendation
**18 August 2009**. Editors: Alistair Miles, Sean Bechhofer.
Namespace `http://www.w3.org/2004/02/skos/core#`.
https://www.w3.org/TR/skos-reference/ Retrieved and verified 2026-09-15.

Core model:

- **Classes**: `skos:Concept`, `skos:ConceptScheme`, `skos:Collection`,
  `skos:OrderedCollection`.
- **Scheme membership**: `skos:inScheme`, `skos:hasTopConcept` /
  `skos:topConceptOf`.
- **Lexical labels**: `skos:prefLabel`, `skos:altLabel`, `skos:hiddenLabel`.
  Integrity conditions: **a resource has no more than one `skos:prefLabel` per
  language tag**, and the three label properties are **pairwise disjoint**.
- **Documentation**: `skos:note` is the parent of `skos:definition`,
  `skos:scopeNote`, `skos:historyNote`, `skos:editorialNote`,
  `skos:changeNote`, `skos:example`.
- **Semantic relations**: `skos:broader` / `skos:narrower` (hierarchical),
  `skos:related` (associative), all sub-properties of `skos:semanticRelation`.
- **Mapping**: `skos:exactMatch` (transitive, symmetric), `skos:closeMatch`
  (symmetric, **not** transitive), `skos:broadMatch`, `skos:narrowMatch`,
  `skos:relatedMatch`, all under `skos:mappingRelation`.

**The design decision worth stealing.** `skos:broader` is declared a
sub-property of `skos:broaderTransitive`, but **`skos:broader` itself is NOT
declared transitive**. The reason, per the Reference: preserving the *direct*
(immediate) hierarchical link is essential for visual and systematic displays,
while transitive closure is provided separately by `skos:broaderTransitive`
for query expansion. The parent link and the ancestor relation are different
relations and must not be conflated.

For NAADAP: PSC and NAICS are exactly this shape. A PSC like `R425` has one
immediate parent (`R4`) and a chain of ancestors. Query expansion over vehicle
eligibility ("this vehicle covers all of R4") needs the closure; audit display
("this vehicle's scope names R425 specifically") needs the direct link. Model
both, keep them distinct, and materialize the closure at build time. Also steal
the `prefLabel` / `altLabel` / `hiddenLabel` triad for the vehicle gazetteer
(`hiddenLabel` for misspellings and OCR variants you want to match but never
display) and `skos:changeNote` / `skos:historyNote` for recording *why* a
knowledge-base row changed.

You can adopt this *discipline* in a tabular KB with four columns
(`pref_label`, `alt_labels`, `hidden_labels`, `parent_code`) plus a
materialized `ancestor_codes` column. You do not need an RDF store to enforce
"one prefLabel per language"; you need a unit test.

### 3.4 JSON-LD

**JSON-LD 1.1: A JSON-based Serialization for Linked Data**, W3C Recommendation
**16 July 2020**. Editors: Gregg Kellogg, Pierre-Antoine Champin, Dave Longley.
https://www.w3.org/TR/json-ld11/ Retrieved and verified 2026-09-15.

Keywords: `@context` maps short names to IRIs; `@id` identifies node objects
(IRI or blank node identifier); `@type` sets node type or literal datatype;
`@graph` expresses a graph (named-graph support); `@value` carries the literal
data; `@language` sets a language tag or document default; `@container` sets
default container type for a term (language maps, index maps, etc.).

Algorithms (expansion, compaction, flattening, framing) live in the companion
**JSON-LD 1.1 Processing Algorithms and API** Recommendation, not in the
serialization spec.

**JSON-LD's real value for NAADAP is negative and important**: it demonstrates
that "RDF" and "a specific file format" are separable. You can hold a
JSON-shaped knowledge base and, *if it ever becomes necessary*, add an
`@context` file that gives every column an IRI and turns the same bytes into
RDF. That is the cheapest possible option on RDF: write the JSON now, keep the
column names stable and IRI-mappable, and defer the entire RDF decision to a
future where someone actually needs SPARQL. **That is my recommended hedge.**

### 3.5 RDF Dataset Canonicalization — the piece that actually matters for a hashed artifact

**RDF Dataset Canonicalization: A Standard RDF Dataset Canonicalization
Algorithm**, W3C Recommendation **21 May 2024**. Editors: Dave Longley,
Gregg Kellogg, Dan Yamamoto. https://www.w3.org/TR/rdf-canon/
Retrieved and verified 2026-09-15.

Defines **RDFC-1.0**. Mechanism:

1. **First-degree hashing.** For each blank node, hash the set of quads that
   mention it, with blank node identifiers replaced by placeholders — `_:a` for
   the node being hashed, `_:z` for every other blank node.
2. Blank nodes whose first-degree hash is **unique** get a canonical identifier
   immediately, assigned in **Unicode code point order** of the hashes.
3. For blank nodes that share a first-degree hash, **N-degree hashing**
   recursively explores transitive connections ("gossip paths") to distinguish
   them, again ordering deterministically.

Determinism: **yes** — the algorithm sorts in Unicode code point order at
every stage, so all conforming implementations produce identical output.
Default hash: **SHA-256** (SHA-256 and SHA-384 required; others optional).

**The warning you must read.** The worst case is tied to graph isomorphism.
The spec states that "some datasets may be constructed to prevent this
algorithm from terminating in a reasonable amount of time," and recommends
implementations "defend against potential denial-of-service attacks by raising
suitable exceptions and terminating early." This is the "poison dataset"
problem. It only arises when the dataset has many mutually-indistinguishable
blank nodes.

**Design rule that follows directly**: if you use RDF at all, **use no blank
nodes.** Give every node an IRI. Then canonicalization is trivial (sort the
quads lexicographically) and the poison-dataset risk is zero. Note that the
canonicalization hedge is precisely what a plain tabular KB gets for free: a
CSV with a frozen column order and a frozen row sort is *already* canonical,
and `SHA256(bytes)` is the artifact hash. RDF needs a whole W3C Recommendation
to recover a property that a sorted table has by construction.

### 3.6 SHACL — how validation actually works

**Shapes Constraint Language (SHACL)**, W3C Recommendation **20 July 2017**.
Editors: Holger Knublauch (TopQuadrant), Dimitris Kontokostas (Univ. Leipzig).
https://www.w3.org/TR/shacl/ (dated URL
https://www.w3.org/TR/2017/REC-shacl-20170720/) Retrieved and verified
2026-09-15.

#### 3.6.1 The mechanism, step by step

SHACL validation takes two RDF graphs — the **data graph** and the **shapes
graph** — and produces a third RDF graph, the **validation report**. The
process is:

**Step 1: Find the shapes.** A *shape* is an IRI or blank node satisfying at
least one of: it is a SHACL instance of `sh:NodeShape` or `sh:PropertyShape`;
it has a target declaration; it has a constraint parameter as a predicate; or
it is referenced by a shape-expecting parameter.

- A **node shape** constrains a focus node itself. It does not have `sh:path`.
- A **property shape** is the subject of a triple with predicate `sh:path`. It
  constrains the *values reachable from the focus node along that path*.
  (`sh:path` accepts full SPARQL-style property paths: sequences, alternatives,
  inverse, zero-or-more, one-or-more, zero-or-one.)

**Step 2: Compute the focus nodes.** Each shape's **targets** select which
nodes of the data graph it applies to:

- `sh:targetNode` — this specific IRI or literal.
- `sh:targetClass` — every SHACL instance of the named class.
- `sh:targetSubjectsOf` — every node that is the *subject* of a triple with the
  named predicate.
- `sh:targetObjectsOf` — every node that is the *object* of a triple with the
  named predicate.

This is the goal-directed part and it is the design's best idea: validation is
scoped to declared targets, so a defect elsewhere in the graph cannot fail your
shape.

**Step 3: For each (focus node, shape), evaluate every constraint component.**
Each constraint parameter present on the shape (e.g. `sh:minCount 1`) triggers
its **constraint component** (e.g. `sh:MinCountConstraintComponent`). For a
property shape, the component is evaluated against the **value nodes** — the
set of nodes reached from the focus node by `sh:path`. For a node shape, the
value node set is just `{focus node}`.

**Step 4: Emit results.** Each failed evaluation produces one
`sh:ValidationResult`.

**Step 5: Conformance.** Verbatim from the spec: *"A focus node conforms to a
shape if and only if the set of result of the validation of the focus node
against the shape is empty and no failure has been reported by it."* Note the
two distinct negative outcomes: a **validation result** (a constraint was
violated) and a **failure** (the processor could not complete, e.g. a malformed
SPARQL constraint). Conformance requires the absence of both.

#### 3.6.2 The validation report — this is the part to steal

The report is itself an RDF graph containing one `sh:ValidationReport` with:

- `sh:conforms` — boolean.
- `sh:result` — zero or more `sh:ValidationResult`, each carrying:
  - `sh:focusNode` — which node failed
  - `sh:resultPath` — which property path (property shapes only)
  - `sh:value` — the offending value node (optional)
  - `sh:sourceShape` — which shape produced this result
  - `sh:sourceConstraintComponent` — which constraint *kind* fired, as an IRI
    (e.g. `sh:MinCountConstraintComponent`)
  - `sh:resultSeverity` — `sh:Violation` (default), `sh:Warning`, or `sh:Info`
  - `sh:resultMessage` — human-readable

That five-tuple — **(what node, what path, what value, what shape, what
constraint kind, what severity)** — is the correct shape of a knowledge-base
validation error report, independent of RDF. A NAADAP build-time validator
should emit exactly this, as JSON:
`{row_id, column, value, rule_id, rule_kind, severity, message}`. Machine-
readable, diffable across builds, and directly citable in a defect report.
Severity levels matter operationally: `sh:Warning` lets you ship a KB with
known soft defects (e.g. a scope narrative shorter than N characters) while
`sh:Violation` fails the build.

#### 3.6.3 The core constraint components

- **Cardinality**: `sh:minCount`, `sh:maxCount`
- **Value type**: `sh:class`, `sh:datatype`, `sh:nodeKind`
- **Value range**: `sh:minInclusive`, `sh:minExclusive`, `sh:maxInclusive`,
  `sh:maxExclusive`
- **String**: `sh:minLength`, `sh:maxLength`, `sh:pattern` (+ `sh:flags`),
  `sh:languageIn`, `sh:uniqueLang`
- **Value enumeration**: `sh:in`, `sh:hasValue`
- **Property pair**: `sh:equals`, `sh:disjoint`, `sh:lessThan`,
  `sh:lessThanOrEquals`
- **Logical**: `sh:and`, `sh:or`, `sh:not`, `sh:xone`
- **Shape-based**: `sh:node`, `sh:property`, `sh:qualifiedValueShape`
  (+ `sh:qualifiedMinCount` / `sh:qualifiedMaxCount`)
- **Other**: `sh:closed` (+ `sh:ignoredProperties`)

**SHACL Core** (spec sections 2–4) is mandatory for all conforming processors.
**SHACL-SPARQL** (sections 5–6) adds `sh:sparql` constraints and user-defined
constraint components via SPARQL; it is an extension, not required for Core
conformance.

#### 3.6.4 The recursion trap

This is the mechanical detail most SHACL summaries omit, and it decides whether
SHACL is a bounded-cost component.

**The SHACL Recommendation deliberately leaves the semantics of recursive
constraints undefined.** Verified from a peer-reviewed source I retrieved in
full — Okulmus et al., "SHACL Validation under the Well-founded Semantics,"
*KR 2024*, Proceedings of the 21st International Conference on Principles of
Knowledge Representation and Reasoning, retrieved from
https://proceedings.kr.org/2024/52/kr2024-0052-okulmus-et-al.pdf — which states:
"the SHACL standard only defines the semantics of non-recursive SHACL
constraints. The case of cyclic dependencies, or recursion, was intentionally
left unspecified: it is currently up to the developers of SHACL validators to
come up with ad-hoc solutions to handle recursion."

The academic fix and its cost, from the same source: Corman, Reutter &
Savković (**"Semantics and Validation of Recursive SHACL," ISWC 2018,
pp. 318–336, DOI 10.1007/978-3-030-00671-6_19**) proposed a *supported model*
semantics; Andreșel et al. (WWW 2020) proposed a *stable model* semantics.
Both are **NP-hard in data complexity**, and "NP-hardness already applies when
the size of input constraints is assumed to be bounded by a constant."
For the supported model semantics, "NP-hardness holds already for constraints
with stratified negation." The cause is "the simultaneous presence of recursion
and negation in constraints."
(The Corman et al. ISWC paper itself is **PARTIALLY VERIFIED** — bibliographic
record confirmed via dblp and Springer listings, complexity claim taken from the
KR 2024 paper's full text, not from the ISWC paper directly.)

**Design rule**: keep your shapes graph **non-recursive** — no shape may, via
`sh:node` / `sh:property` / `sh:qualifiedValueShape`, reach itself. Then
validation is polynomial, terminating, implementation-independent, and
standard-defined. For NAADAP's vehicle KB (a flat set of rows with typed
columns and a foreign key into a code taxonomy), non-recursive shapes are
sufficient and recursion is never needed. Write that down as a constraint on
the schema, and add a CI check that the shape graph is acyclic.

**SHACL 1.2** is in progress: the W3C Data Shapes Working Group published
**SHACL 1.2 Core** as a Working Draft (31 March 2026,
https://www.w3.org/TR/shacl12-core/), with SHACL 1.2 Rules, SHACL 1.2 SPARQL
Extensions (WD 28 Aug 2026), SHACL 1.2 Profiling (FPWD 2026) and SHACL 1.2
User Interfaces (FPWD 26 May 2026). **None of these is a Recommendation.**
Target SHACL 1.0 (2017) if you target SHACL at all.

---

## 4. PROV-O / W3C PROV — mechanism, and a concrete NAADAP design

### 4.1 The specification family

**PROV-O: The PROV Ontology**, W3C **Recommendation, 30 April 2013**.
Editors: Timothy Lebo (RPI), Satya Sahoo (Case Western Reserve), Deborah
McGuinness (RPI). Namespace `http://www.w3.org/ns/prov#`, conventional prefix
`prov:`. https://www.w3.org/TR/prov-o/ Retrieved and verified 2026-09-15.

The full family, verified from https://www.w3.org/TR/prov-overview/ (all dated
30 April 2013):

| Document | Status |
|---|---|
| PROV-DM — conceptual data model | **Recommendation** |
| PROV-O — OWL 2 ontology for the data model | **Recommendation** |
| PROV-N — human-readable notation | **Recommendation** |
| PROV-CONSTRAINTS — constraints on the data model | **Recommendation** |
| PROV-PRIMER | Note |
| PROV-XML — XML schema | Note |
| PROV-AQ — locating and retrieving provenance on the Web | Note |
| PROV-DICTIONARY | Note |
| PROV-LINKS — linking provenance across bundles | Note |
| PROV-SEM — first-order-logic semantics | Note |

Four Recommendations and six Notes. PROV-O is "a light-weight OWL2 ontology for
the provenance data model" — which in practice means it is deliberately shallow,
mostly a vocabulary with domains, ranges and a few subproperty axioms; it does
not require heavy reasoning to use.

### 4.2 The core: three classes, one shape

- **`prov:Entity`** — "a physical, digital, conceptual, or other kind of thing
  with some fixed aspects."
- **`prov:Activity`** — "something that occurs over a period of time and acts
  upon or with entities."
- **`prov:Agent`** — "something that bears some form of responsibility for an
  activity taking place."

Starting-point properties:

| Property | Domain → Range | Meaning |
|---|---|---|
| `prov:wasGeneratedBy` | Entity → Activity | this entity was produced by that activity |
| `prov:used` | Activity → Entity | that activity consumed this entity |
| `prov:wasDerivedFrom` | Entity → Entity | this entity came from that one |
| `prov:wasAttributedTo` | Entity → Agent | responsibility for the entity |
| `prov:wasAssociatedWith` | Activity → Agent | responsibility for the activity |
| `prov:actedOnBehalfOf` | Agent → Agent | delegation |
| `prov:wasInformedBy` | Activity → Activity | activity depended on another |
| `prov:startedAtTime` / `prov:endedAtTime` | Activity → xsd:dateTime | temporal bounds |

Expanded terms for versioning and sourcing:

| Term | Meaning |
|---|---|
| `prov:wasRevisionOf` | entity contains substantial content from a predecessor |
| `prov:specializationOf` | a more specific entity of a more general one |
| `prov:alternateOf` | two entities present different aspects of the same thing |
| `prov:hadPrimarySource` | derived from an original source with direct knowledge |
| `prov:generatedAtTime` / `prov:invalidatedAtTime` | entity lifecycle timestamps |
| `prov:wasInvalidatedBy` | the activity that ended the entity's usability |
| `prov:SoftwareAgent` | a running program, as an Agent |
| `prov:Plan` / `prov:hadPlan` | the intended steps an agent followed |
| `prov:Collection` / `prov:hadMember` | an entity that structures constituents |

### 4.3 The qualified-influence pattern — the mechanism that makes PROV useful

The binary properties above carry no attributes. `A prov:wasDerivedFrom B`
cannot say *when*, *how*, *by which activity*, or *with what confidence*.
PROV's answer is the **qualified pattern**: for each binary influence property
there is a corresponding **influence class** and a **qualification property**
that reifies the relationship into a first-class node which can then carry
arbitrary attributes.

| Unqualified | Qualification property | Influence class |
|---|---|---|
| `prov:wasGeneratedBy` | `prov:qualifiedGeneration` | `prov:Generation` |
| `prov:used` | `prov:qualifiedUsage` | `prov:Usage` |
| `prov:wasDerivedFrom` | `prov:qualifiedDerivation` | `prov:Derivation` |
| `prov:wasRevisionOf` | `prov:qualifiedRevision` | `prov:Revision` |
| `prov:wasAttributedTo` | `prov:qualifiedAttribution` | `prov:Attribution` |
| `prov:wasAssociatedWith` | `prov:qualifiedAssociation` | `prov:Association` |

Example from the Recommendation:

```turtle
:activity prov:used :entity .                       # unqualified
:activity prov:qualifiedUsage [                      # qualified
    a prov:Usage ;
    prov:entity :entity ;
    prov:atTime "2011-07-14T03:03:03Z"^^xsd:dateTime
] .
```

`prov:atTime` marks **instantaneous events** — Start, Generation, Usage,
Invalidation, End. The spec's guidance, quoted: "It is correct and acceptable
for an implementer to use either qualified or unqualified forms… Consuming
applications _should_ recognize both qualified and unqualified forms, and treat
the qualified form as implying the unqualified form."

**This is the mechanism NAADAP needs and it is the reason PROV is worth reading
even if you never write a triple.** It says: *a provenance link is itself a
thing with attributes*. The attribute you care about is the retrieval time.

### 4.4 Concrete: how to record "this KB field came from this document retrieved on this date" so staleness is detectable

Here is a worked PROV-O model for one NAADAP knowledge-base field. I will then
show its tabular equivalent, which is what I actually recommend shipping.

```turtle
@prefix prov: <http://www.w3.org/ns/prov#> .
@prefix nd:   <https://naadap.example/ns#> .
@prefix xsd:  <http://www.w3.org/2001/XMLSchema#> .

### The source document, as retrieved. This is an Entity.
nd:doc/seaport-nxg-conformed-p00042
    a prov:Entity ;
    nd:sourceUri        "https://…/SeaPort-NxG_conformed_P00042.pdf" ;
    nd:sha256           "e3b0c44298fc1c149afbf4c8996fb924…" ;
    prov:generatedAtTime "2029-03-11T00:00:00Z"^^xsd:dateTime ;   # doc's own effective date
    nd:retrievedAtTime  "2029-08-02T14:22:09Z"^^xsd:dateTime ;    # when WE got it
    nd:issuerRevision   "P00042" ;                                # the revision identifier
    prov:wasAttributedTo nd:agent/NAVSEA-SeaPort-PMO .            # who issued it

### The specific span within that document that supports the value.
nd:span/seaport-nxg/orderingPeriodEnd
    a prov:Entity ;
    prov:specializationOf nd:doc/seaport-nxg-conformed-p00042 ;
    nd:page 14 ; nd:charStart 8123 ; nd:charEnd 8197 ;
    nd:quotedText "The ordering period ends 5 January 2034." .

### The knowledge-base field value. Also an Entity.
nd:kb/seaport-nxg/orderingPeriodEnd@v3
    a prov:Entity ;
    nd:vehicleId    "SEAPORT-NXG" ;
    nd:field        "ordering_period_end" ;
    nd:value        "2034-01-05"^^xsd:date ;

    # WHERE IT CAME FROM
    prov:wasDerivedFrom  nd:span/seaport-nxg/orderingPeriodEnd ;
    prov:hadPrimarySource nd:doc/seaport-nxg-conformed-p00042 ;

    # QUALIFIED: carries the retrieval instant, so staleness is computable
    prov:qualifiedDerivation [
        a prov:Derivation ;
        prov:entity   nd:span/seaport-nxg/orderingPeriodEnd ;
        prov:atTime   "2029-08-02T14:22:09Z"^^xsd:dateTime ;
        nd:method     nd:plan/manual-transcription-v2 ;
        nd:sourceRevisionSeen "P00042"
    ] ;

    # WHO / WHAT PRODUCED IT
    prov:wasGeneratedBy  nd:act/kb-build-2029-08-02 ;
    prov:wasAttributedTo nd:agent/analyst-jdoe ;

    # VERSION CHAIN
    prov:wasRevisionOf nd:kb/seaport-nxg/orderingPeriodEnd@v2 ;
    prov:qualifiedRevision [
        a prov:Revision ;
        prov:entity nd:kb/seaport-nxg/orderingPeriodEnd@v2 ;
        prov:atTime "2029-08-02T14:22:09Z"^^xsd:dateTime ;
        nd:changeNote "Mod P00042 extended ordering period from 2032-01-05."
    ] ;

    # STALENESS POLICY, attached to the fact itself
    nd:reviewIntervalDays 180 ;
    nd:nextReviewDue "2030-01-29"^^xsd:date .

### The build activity and the agents.
nd:act/kb-build-2029-08-02
    a prov:Activity ;
    prov:startedAtTime "2029-08-02T14:00:00Z"^^xsd:dateTime ;
    prov:endedAtTime   "2029-08-02T15:10:00Z"^^xsd:dateTime ;
    prov:used          nd:doc/seaport-nxg-conformed-p00042 ;
    prov:wasAssociatedWith nd:agent/naadap-kbbuild-1.4.2 ;
    prov:qualifiedAssociation [
        a prov:Association ;
        prov:agent   nd:agent/analyst-jdoe ;
        prov:hadPlan nd:plan/manual-transcription-v2
    ] .

nd:agent/naadap-kbbuild-1.4.2 a prov:SoftwareAgent .
nd:agent/analyst-jdoe         a prov:Person .

### Superseding a fact found to be wrong — the "14 vs 18 events" case.
nd:kb/setr/technical-review-event-count@v1
    prov:wasInvalidatedBy nd:act/kb-audit-2029-08-02 ;
    prov:invalidatedAtTime "2029-08-02T15:05:00Z"^^xsd:dateTime ;
    nd:invalidationReason "Captured from NAVAIRINST rev C (14 events); current rev E has 18." .
```

**How staleness becomes mechanically detectable from this.** Four independent
checks, each a simple comparison, none requiring inference:

1. **Interval staleness.** `now() > nd:nextReviewDue` → the field is overdue
   for re-verification regardless of whether anything changed. This is the
   *only* check that works fully offline, and it is the one that would have
   caught all three of the team's past failures. It requires nothing but a
   date column.
2. **Source-revision drift.** The field records
   `nd:sourceRevisionSeen = "P00042"`. At the next refresh, whoever fetches the
   source compares the new revision identifier against the recorded one. A
   mismatch flags **every field derived from that document** — because
   `prov:hadPrimarySource` makes the field→document edge queryable in reverse.
   This is precisely the "instruction revision C vs revision E" failure.
3. **Source-hash drift.** `nd:sha256` on the document entity. Cheaper and
   stricter than revision drift: any byte change flags the derived fields. Use
   it when the issuer does not version cleanly.
4. **Validity-window expiry.** For fields that are themselves dates (ordering
   period end, ceiling), `now() > value` means the *world* has moved past the
   fact, not just the capture. `prov:invalidatedAtTime` /
   `prov:wasInvalidatedBy` record the moment a fact was retired and by what
   activity — the "regulation deleted in 2018" case is exactly a
   `wasInvalidatedBy` edge pointing at the rescinding action.

**Note what is doing the work.** It is not OWL reasoning; it is *four
timestamps and two identifiers, recorded next to the value, with a reverse
index from document to derived fields*. PROV-O's contribution is telling you
**which** timestamps and identifiers, and giving them stable, externally
meaningful names.

### 4.5 The tabular equivalent — what I recommend actually shipping

Every semantic commitment above survives as columns. This is the design I
recommend:

`vehicles.tsv` (one row per vehicle, the facts):

| column | notes |
|---|---|
| `vehicle_id` | primary key |
| `field` | e.g. `ordering_period_end` — or use a wide table, one column per field, with a parallel provenance table |
| `value` | the value, typed by a schema file |
| `value_datatype` | `xsd:date`, `xsd:decimal`, `string`, … |

`provenance.tsv` (one row per *(vehicle_id, field, kb_version)*):

| column | PROV-O analogue |
|---|---|
| `kb_version` | entity version identifier |
| `source_doc_id` | `prov:hadPrimarySource` |
| `source_uri` | — |
| `source_sha256` | — |
| `source_revision_seen` | — |
| `source_effective_date` | `prov:generatedAtTime` on the doc |
| `retrieved_at` | `prov:atTime` on the qualified Derivation |
| `span_page`, `span_char_start`, `span_char_end`, `quoted_text` | `prov:specializationOf` + locator |
| `generated_by_activity` | `prov:wasGeneratedBy` |
| `generated_by_tool_version` | `prov:SoftwareAgent` |
| `attributed_to` | `prov:wasAttributedTo` |
| `plan_id` | `prov:hadPlan` |
| `revision_of_kb_version` | `prov:wasRevisionOf` |
| `change_note` | `skos:changeNote` / `prov:Revision` attribute |
| `review_interval_days`, `next_review_due` | staleness policy |
| `invalidated_at`, `invalidated_by`, `invalidation_reason` | `prov:wasInvalidatedBy` / `prov:invalidatedAtTime` |

Two joins, no triplestore, no reasoner, no blank nodes, `git diff`-able,
`sha256sum`-able, and every one of the four staleness checks is a `WHERE`
clause or a single pass in C#.

**Use the PROV term names as the column names** (`had_primary_source`,
`was_derived_from`, `was_revision_of`, `generated_at_time`, `retrieved_at`,
`invalidated_at_time`, `was_attributed_to`). This costs nothing, makes the
design self-documenting to anyone who knows PROV, makes the JSON-LD escape
hatch in §3.4 a one-file change, and — the real benefit — **forces the team to
answer PROV's questions**, which are the right questions: who is responsible,
what activity produced this, from what, at what time, and which earlier version
does it replace.

### 4.6 Artifact-level provenance: the other standard you should know about

For "the whole knowledge base artifact, built by this pipeline, from these
inputs," the practical standard is **in-toto Attestation Framework** (a signed
`Statement` with `_type: "https://in-toto.io/Statement/v1"`, a `subject` array
of `{name, digest}` pairs, a `predicateType` IRI and a `predicate` object) with
**SLSA Provenance** as the predicate. Retrieved:
https://github.com/in-toto/attestation/blob/v1.0/spec/v1.0/statement.md and
https://slsa.dev/spec/v1.1/verification_summary
**PARTIALLY VERIFIED** — I retrieved the specification pages but did not
exhaustively verify the v1.2 predicate schema.

The important structural point, and it applies to the NAADAP KB verbatim: the
`subject` binds the attestation to **specific artifact digests**, and an
attestation "without a subject, or with a subject that uses a weak digest
algorithm (MD5, SHA-1), cannot be reliably matched to an artifact." So:
SHA-256 the canonical bytes of `vehicles.tsv` and `provenance.tsv`, put both
digests in the attestation subject, emit the build inputs (source document
URIs + hashes + retrieval dates) as the predicate, and ship the attestation in
the container next to the KB. PROV-O covers field-level provenance;
in-toto/SLSA covers artifact-level provenance. They are complementary and
neither replaces the other.

---

## 5. (reserved — see §6)

---

## 6. Knowledge graph embeddings — verdict: NO

### 6.1 The models and their scoring functions

All four scoring functions verified from Table 1 of the RotatE paper
(retrieved full text: https://arxiv.org/pdf/1902.10197), cross-checked against
the originals.

| Model | Score `f_r(h,t)` | Space |
|---|---|---|
| **TransE** (Bordes et al., NIPS 2013) | `−‖h + r − t‖` (L1 or L2) | `h, r, t ∈ ℝ^k` |
| **DistMult** (Yang et al., ICLR 2015) | `⟨r, h, t⟩` (generalized 3-way dot product; equivalently `hᵀ diag(r) t`) | `h, r, t ∈ ℝ^k` |
| **ComplEx** (Trouillon et al., ICML 2016) | `Re(⟨r, h, t̄⟩)` — Hermitian/sesquilinear dot product, `·̄` = complex conjugate | `h, r, t ∈ ℂ^k` |
| **RotatE** (Sun et al., ICLR 2019) | `−‖h ∘ r − t‖²`, with constraint `|r_i| = 1` (∘ = Hadamard product) | `h, r, t ∈ ℂ^k` |

**TransE.** Bordes, A., Usunier, N., Garcia-Duran, A., Weston, J., Yakhnenko, O.
"Translating Embeddings for Modeling Multi-relational Data." *NIPS 2013*,
Lake Tahoe. Retrieved full text from NeurIPS proceedings. The relation is a
**translation** in embedding space: `h + ℓ ≈ t` when `(h,ℓ,t)` holds. Energy is
`d(h + ℓ, t)` with `d` = L1 or L2 norm. Objective, verbatim from the paper:

```
L = Σ_{(h,ℓ,t)∈S} Σ_{(h',ℓ,t')∈S'_(h,ℓ,t)} [ γ + d(h+ℓ, t) − d(h'+ℓ, t') ]_+
```

a **margin-based ranking criterion** with margin `γ > 0`. `S'` is the corrupted
set: for each positive triplet, replace **either** the head **or** the tail with
a **uniformly randomly sampled** entity. Training (Algorithm 1 of the paper):
initialize relation and entity embeddings `~ uniform(−6/√k, +6/√k)`, normalize;
each iteration, normalize entity embeddings (which the paper says is essential —
otherwise the loss is trivially minimized by inflating norms), sample a
minibatch, sample **one corrupted triplet per positive**, take a constant-
learning-rate gradient step. Stop on validation performance.

**DistMult.** Yang, B., Yih, W., He, X., Gao, J., Deng, L. "Embedding Entities
and Relations for Learning and Inference in Knowledge Bases." *ICLR 2015*.
arXiv:1412.6575. Retrieved full text. It is the Bilinear model with the relation
matrix `M_r` restricted to be **diagonal** ("Bilinear-diag"), which drops the
relation parameter count from `O(k²)` to `O(k)`. Reported: top-10 accuracy
73.2 % vs TransE's 54.7 % on Freebase. **Structural limitation**: the diagonal
bilinear form is **symmetric** in `h` and `t`, so DistMult cannot represent any
antisymmetric relation.

**ComplEx.** Trouillon, T., Welbl, J., Riedel, S., Gaussier, É., Bouchard, G.
"Complex Embeddings for Simple Link Prediction." *ICML 2016*, PMLR 48.
arXiv:1606.06357. Retrieved full text. Moves embeddings into ℂ^k and uses the
**Hermitian** dot product, which "involves the conjugate-transpose of one of the
two vectors. As a consequence, the dot product is not symmetric any more, and
facts about antisymmetric relations can receive different scores depending on
the ordering of the entities." Fixes DistMult's symmetry defect while staying
linear in space and time.

**RotatE.** Sun, Z., Deng, Z.-H., Nie, J.-Y., Tang, J. "RotatE: Knowledge Graph
Embedding by Relational Rotation in Complex Space." *ICLR 2019*.
arXiv:1902.10197. Retrieved full text. Motivated by Euler's identity: a unitary
complex number is a rotation. Requires `t = h ∘ r` with `|r_i| = 1`, i.e. each
dimension is `t_i = h_i r_i`. This models **symmetry/antisymmetry** (`r_i = ±1`),
**inversion**, and **composition** simultaneously — the first model to do all
three. Loss:

```
L = −log σ(γ − d_r(h,t)) − Σ_{i=1..n} (1/k) log σ(d_r(h'_i, t'_i) − γ)
```

plus the paper's contribution of **self-adversarial negative sampling**:
negatives are drawn from `p(h'_j, r, t'_j) ∝ exp(α f_r(h'_j, t'_j))` — i.e.
weighted by the *current* model's score — rather than uniformly.

### 6.2 Why the answer is no

**(a) Determinism. This is disqualifying on its own.** NAADAP requires the same
top-5 output in ≥95 % of runs. Every model above has, at minimum: random uniform
initialization of all embeddings (TransE Algorithm 1, lines 1 and 3); random
minibatch sampling; and random negative sampling — uniform for TransE/DistMult/
ComplEx, *model-dependent and therefore path-dependent* for RotatE's
self-adversarial scheme. Determinism could in principle be recovered by fixing a
PRNG seed, but that only makes the *artifact* reproducible; it does not make the
*function* stable. Change the training data by one triple, or the library
version, or the float accumulation order on a different CPU, and every embedding
moves. A scoring function whose parameters are the output of seeded SGD is not
something you can defend in a determination: "why is this vehicle ranked third?"
has no answer other than "that is where the gradient descent landed."

**(b) Data size.** All four papers evaluate on FB15k / WN18 / FB15k-237 /
WN18RR — hundreds of thousands of triples over ~15,000 entities. NAADAP's vehicle
KB will have on the order of **tens of vehicles and hundreds to low thousands of
facts**. TransE alone has `(n_e + n_r) × k` parameters; at `k = 50` with 50
vehicles and 30 relations, that is 4,000 parameters fit to perhaps 1,000
observations, with no held-out set large enough to tune `γ`, `k`, the learning
rate, or the negative-sample count. There is nothing to learn: every fact is
already known and asserted. **Embeddings are a technology for completing a KB
that is large and incomplete.** NAADAP's KB is small and, by requirement,
complete-and-cited. The tool does not match the problem.

**(c) Traceability.** The requirement is that "every output must trace to a
knowledge-base row or a document span." An embedding score is a dot product over
learned latent dimensions. It traces to nothing. Even if the accuracy were
perfect, it would fail the defensibility requirement by construction.

**(d) The literature's own evaluation is unsound — so their reported numbers
are not evidence you can lean on.** Two peer-reviewed re-evaluations:

- **Sun, Z., Vashishth, S., Sanyal, S., Talukdar, P., Yang, Y. "A Re-evaluation
  of Knowledge Graph Completion Methods." *ACL 2020*.** arXiv:1911.03903.
  Retrieved full text. Finding: several recent neural KGC models report high
  performance that "can be attributed to the inappropriate evaluation protocol
  used by them." The mechanism is a **tie-breaking** artifact: those models
  assign the *exact same score* to the valid triplet and to large numbers of
  negatives, so where the valid triplet lands in the ranking depends on how ties
  are broken. In one FB15k-237 example, **8,520 of 14,541 negatively sampled
  triplets (58.5 %) had exactly the same score as the valid triplet.** Across the
  whole evaluation set, ConvKB and CapsE averaged 125 and 197 entities tied with
  the valid triplet. A benchmark number that moves by tens of MRR points
  depending on `sort()` stability is not a measurement.
- **Akrami, F., Saeef, M. S., Zhang, Q., Hu, W., Li, C. "Realistic Re-evaluation
  of Knowledge Graph Completion Methods: An Experimental Study." *SIGMOD 2020*.
  DOI 10.1145/3318464.3380599.** arXiv:2003.08001.
  **PARTIALLY VERIFIED** — bibliographic record and abstract confirmed via ACM
  DL and arXiv listing; I did not retrieve full text. Finding: most triples in
  the standard FB15k and WN18 benchmarks belong to **reverse and duplicate
  relations**, producing "excessive data leakage where a model is trained using
  features that otherwise would not be available when applied for real
  prediction," and **Cartesian product relations** — relations for which every
  triple in the Cartesian product of applicable subjects and objects is true —
  which are trivially predictable and inflate scores.
- **Ruffinelli, D., Broscheit, S., Gemulla, R. "You CAN Teach an Old Dog New
  Tricks! On Training Knowledge Graph Embeddings." *ICLR 2020*.**
  **PARTIALLY VERIFIED** — record confirmed via dblp and the University of
  Mannheim announcement page; full text not retrieved. Finding: when older models
  are trained with a modern, properly-tuned training pipeline, "the relative
  performance differences between various model architectures often shrinks and
  sometimes even reverses when compared to prior results." In other words most of
  the reported progress in this literature is hyperparameter tuning, not
  architecture.

**Verdict: reject KG embeddings for NAADAP entirely.** Not "defer," not "maybe
as a tiebreaker" — reject. They violate the determinism requirement structurally,
they violate the traceability requirement structurally, the data regime is wrong
by two to three orders of magnitude, and the field's own published numbers are
contested by three independent re-evaluations. If a reviewer asks "did you
consider embeddings," the answer is "yes, and here is the four-part reason they
are the wrong tool," which is a much stronger position than having shipped them.

---

## 7. C#/.NET tooling

### 7.1 dotNetRDF — the only credible option

| Attribute | Value |
|---|---|
| Repository | **`dotnetrdf/dotnetrdf`** |
| Language | C# |
| Stars | 331 (forks 99, open issues 79) |
| Last push | **2026-08-16** — actively maintained |
| Created | 2016-04-24 (the project itself dates to ~2009 on CodePlex) |
| Licence | **MIT** (per NuGet package metadata for `dotNetRdf.Core` and `dotNetRdf.Shacl`; GitHub's licence detector reports `NOASSERTION` because the licence file is not at a path it recognizes — **treat the NuGet metadata as authoritative and confirm the licence text in the source tree before shipping**) |
| Target | **.NET Standard 2.0** → works on .NET Framework 4.7+, .NET Core 2.0+, .NET 5.0+, and therefore .NET 9 |
| Latest version | **3.5.2, published 2026-06-27** |
| Recent cadence | 3.3.2 (2025-01-26), 3.4.1 (2025-10-06), 3.5.0 (2026-02-02), 3.5.1 (2026-02-09), 3.5.2 (2026-06-27) — roughly quarterly |
| Downloads | `dotNetRdf.Core` 971.1 K total; `dotNetRdf.Shacl` 850.7 K total |

Verified 2026-09-15 via the GitHub API and nuget.org package pages.

**Capabilities**: RDF parsing/serialization across the standard formats; SPARQL
1.1 Query and Update; in-memory triple store; RDF-star and SPARQL-star;
RDFS / SKOS / limited OWL inference; **SHACL validation**; full-text query;
adapters for AllegroGraph, Jena/Fuseki, Stardog, Virtuoso.

**SHACL support specifics**: the `dotNetRdf.Shacl` package is described as a
fully compliant **SHACL Core and SHACL-SPARQL** processor. Advanced SHACL
features — Custom Targets, Annotation Properties, SHACL Functions, Node
Expressions, Expression Constraints, and SHACL Rules — are **explicitly not
planned** (per `dotnetrdf/dotnetrdf` Discussion #559). For NAADAP that is fine:
you need Core, and Core is the mandatory conformance level.

**Reasoning**: implemented as `IInferenceEngine` implementations in
`VDS.RDF.Query.Inference` — `StaticRdfsReasoner` and `RdfsReasoner`
(subclass/subproperty/domain/range materialization), `StaticSkosReasoner` and
`SkosReasoner` (SKOS hierarchy expansion), `SimpleN3RulesReasoner`. These
**materialize** inferred triples into the graph or into a separate output graph.
There is **no OWL 2 DL reasoner**; nothing in dotNetRDF gives you OWL 2 EL/QL/RL
entailment. If you needed real OWL reasoning in .NET you would be shelling out
to a Java reasoner (HermiT, ELK, Openllet) — an unacceptable addition to a 2 GB
container with a no-network constraint.

**Offline operation**: yes. It is a pure managed library with no runtime network
requirement (the SPARQL endpoint adapters are opt-in). It works fully offline.

**The dependency cost, and why it matters here.** `dotNetRdf.Core` 3.5.2 pulls
in **AngleSharp (≥1.4.0), HtmlAgilityPack (≥1.12.4), Newtonsoft.Json (≥13.0.4)**
plus System.* compatibility shims. That is a full HTML parser, a second HTML
parser, and a second JSON serializer, added to a project whose current entire
third-party surface is PdfPig + DocumentFormat.OpenXml.

I read `/home/user/NAADAP/docs/DEPENDENCIES.md`. The project has an explicit
policy — every `PackageReference` beyond the BCL requires an inline
`<!-- Justification: ... -->` in the `.csproj`, verified by inspection test
TP-920 — and **CORE-240 is a zero-third-party-dependency rule for
`src/Naadap.Core`**, the production clustering/recommendation path. Adding
dotNetRDF to the core path would violate CORE-240 outright. Adding it anywhere
would add three transitive dependencies with their own CVE surface to a
deliverable that currently has two.

**Recommendation on dotNetRDF**: do not add it to the runtime image. If the
team wants SHACL validation of the knowledge base, run it **in CI, at build
time, from a separate tools project that never ships** — and even then, prefer
pySHACL (§7.3) or a hand-written validator (§8.3).

### 7.2 The alternatives, briefly

| Project | Language | Licence | Last push | Stars | Assessment |
|---|---|---|---|---|---|
| **`semiodesk/trinity-rdf`** | C# | **MIT** (verified from the repo's `LICENSE` file: "The Semiodesk.Trinity library and tools found in this repository are licensed under the the MIT License (MIT). Copyright (c) 2015 Semiodesk GmbH") | **2026-09-10** — active | 29 | An **object mapper** layered on top of dotNetRDF (LINQ over mapped objects, MVC/MVVM patterns). Inherits all of dotNetRDF's dependencies plus its own. Solves a problem NAADAP does not have. |
| **`BrightstarDB/BrightstarDB`** | C# | **MIT** | **2023-05-31** — **stale, ~3.3 years without a commit** | 462 | An embedded NoSQL triple store for .NET with code-first entity generation. The staleness alone disqualifies it for a deliverable that must be maintained. |
| `Canyala.Mercury` | C# | not verified | not verified | low | **UNVERIFIED.** Surfaced in search only. Do not consider. |

There is no credible .NET OWL 2 reasoner. There is no .NET implementation of
RDFC-1.0 that I was able to verify.

### 7.3 Non-.NET tools worth knowing for the CI side

| Project | Language | Licence (verified) | Stars | Use |
|---|---|---|---|---|
| **`RDFLib/pySHACL`** | Python | **Apache-2.0** | 348 | Reference-quality SHACL validator, built on RDFLib with OWL-RL for OWL 2 RL expansion. Runs offline. Good choice for a build-time gate if the KB is emitted as RDF. |
| **`TopQuadrant/shacl`** | Java | **Apache-2.0** | 246 | The de-facto SHACL reference implementation, on Apache Jena. Supports validation *and* SHACL Rules. |

Both are Apache-2.0, both run offline, neither ships in the runtime image.
Either is an acceptable CI-only validator. Neither is acceptable in the
container.

---

## 8. Versioning, temporal knowledge, and staleness — plus the verdict

### 8.1 How the standards handle "this fact was true as of date X"

There are four mechanisms, in increasing order of standardization maturity and
decreasing order of convenience.

**(1) Named graphs (RDF 1.1 / RDF 1.2 datasets).** Put each snapshot of the KB
into its own named graph and attach the temporal and provenance metadata to the
*graph name* in the default graph. This is exactly the design Carroll, Bizer,
Hayes & Stickler proposed in WWW 2005, with provenance as the motivating use
case. It is fully standard, SPARQL queries it natively (`GRAPH ?g { … }`), and
`dotNetRDF` supports it. Its cost is granularity: the unit of annotation is a
whole graph, so per-fact temporal scoping means either one graph per fact
(absurd) or accepting snapshot-level granularity.

**(2) RDF 1.1 reification** — `rdf:Statement` / `rdf:subject` / `rdf:predicate`
/ `rdf:object`. Four extra triples per annotated fact, with no denotational
guarantee that the reified statement means the same thing as the asserted one.
Widely deprecated in practice. **Do not use.**

**(3) RDF-star / RDF 1.2 triple terms + `rdf:reifies`.** The modern answer:

```turtle
<< :seaportNxg :orderingPeriodEnd "2034-01-05"^^xsd:date >>
```
is a triple term; a reifier points at it with `rdf:reifies` and carries the
annotations:
```turtle
:r1 rdf:reifies << :seaportNxg :orderingPeriodEnd "2034-01-05"^^xsd:date >> ;
    prov:hadPrimarySource :doc/seaport-conformed-P00042 ;
    nd:retrievedAt "2029-08-02T14:22:09Z"^^xsd:dateTime ;
    nd:validFrom  "2029-03-11"^^xsd:date .
```
This is clean and it is what you would want. **But RDF 1.2 Concepts is a
Candidate Recommendation dated 07 April 2026 and is not yet a Recommendation.**
Building a deliverable that must be defensible in a legal determination on a
spec that has not reached Recommendation is a poor trade. dotNetRDF does
support "RDF-Star" per its feature list, but which draft it implements would
need verification.

**(4) Temporal annotation patterns in plain RDF.** Two canonical academic
approaches:

- **Gutierrez, C., Hurtado, C. A., Vaisman, A. A. "Introducing Time into RDF."
  *IEEE Transactions on Knowledge and Data Engineering* 19(2):207–218, 2007.**
  **PARTIALLY VERIFIED** — bibliographic record confirmed via dblp
  (https://dblp.org/rec/journals/tkde/GutierrezHV07.html) and Semantic Scholar;
  full text not retrieved. Contribution: *temporal RDF graphs* — RDF triples
  labelled with time, a notion of **temporal entailment**, and a syntax that
  encodes the framework inside standard RDF using the RDF vocabulary plus
  temporal labels. The load-bearing result for NAADAP: temporal entailment is
  characterized in terms of ordinary RDF entailment and **"does not yield extra
  asymptotic complexity with respect to nontemporal RDF graphs"**; query
  evaluation stays tractable. That is a genuinely useful licence — adding time
  does not make the problem harder, so you should not be afraid to add it.
- **The 4D-fluents pattern** (Welty & Fikes, FOIS 2006, "A Reusable Ontology for
  Fluents in OWL") — **UNVERIFIED**, I did not retrieve this; do not cite it
  without checking. It reifies each time-varying property into a *time slice*
  entity.

**(5) OWL-Time**, for the temporal values themselves.
**Time Ontology in OWL**, W3C Candidate Recommendation Draft **15 November
2022**. Editors: Simon Cox (CSIRO), Chris Little (Met Office). Namespace
`http://www.w3.org/2006/time#`. https://www.w3.org/TR/owl-time/
Retrieved and verified 2026-09-15.
`time:TemporalEntity` splits into `time:Instant` (zero extent) and
`time:Interval` (has extent), with `time:ProperInterval` for intervals whose
beginning and end differ. `time:hasBeginning` / `time:hasEnd` link to Instants;
`time:inXSDDateTimeStamp` gives an Instant an xsd-typed position;
`time:hasTime` attaches a temporal entity to *any* resource;
`time:hasTemporalDuration` gives extent. Thirteen Allen interval relations
(`time:intervalBefore`, `intervalMeets`, `intervalOverlaps`, `intervalStarts`,
`intervalDuring`, `intervalFinishes`, `intervalEquals` and inverses) plus
`intervalDisjoint` and `intervalIn`.

For NAADAP the relevant primitive is exactly `time:ProperInterval` with
`hasBeginning` / `hasEnd` — a contract vehicle's ordering period *is* a proper
interval, and "is this vehicle available on date D" is `time:intervalContains`
or a simple `begin ≤ D ≤ end` comparison. **You do not need OWL-Time to do that
comparison.** You need two `xsd:date` columns. But OWL-Time is the right thing
to read to make sure your column semantics are unambiguous (inclusive or
exclusive endpoints? is the "ordering period end" the last day an order may be
placed, or the first day it may not be? Allen's algebra forces you to answer).

### 8.2 Staleness detection and truth maintenance — the literature

**The classical line.**

- **Doyle, J. "A Truth Maintenance System." *Artificial Intelligence*
  12(3):231–272, 1979.** DOI 10.1016/0004-3702(79)90008-0.
  **PARTIALLY VERIFIED** — bibliographic record confirmed via ScienceDirect and
  a retrievable copy at
  https://cse.buffalo.edu/~rapaport/Papers/Papers.by.Others/NONMONOTONIC/doyle79.pdf;
  I confirmed volume/pages but did not read the full text in this session.
  The mechanism: a TMS records, for each belief, the **reasons (justifications)**
  that support it. The current belief set is computed from the current reason
  set; when reasons change, the TMS propagates incrementally, and it "traces the
  reasons for beliefs to find the consequences of changes in the set of
  assumptions."

  **This is precisely the architecture NAADAP needs, and it predates all of the
  semantic-web stack by three decades.** Every KB field records its
  justification (the source document + span). When a source document is
  superseded, you follow the justification links *backwards* to find every
  belief that depended on it, and mark them for re-derivation. That is the "14
  vs 18 events" failure, mechanized.
- **de Kleer, J. "An Assumption-based TMS." *Artificial Intelligence*
  28(2):127–162, 1986.** **UNVERIFIED** — I did not retrieve this. The ATMS
  extension labels each belief with the *sets of assumptions* under which it
  holds, so multiple contexts coexist. Mentioned for completeness; do not cite
  without verifying.
- **Alchourrón, C. E., Gärdenfors, P., Makinson, D. "On the Logic of Theory
  Change: Partial Meet Contraction and Revision Functions." *Journal of Symbolic
  Logic* 50(2):510–530, 1985.** **PARTIALLY VERIFIED** — record confirmed via
  Project Euclid (https://projecteuclid.org/euclid.jsl/1183741857) and Semantic
  Scholar; full text not retrieved. The AGM paradigm: the formal theory of how a
  belief set should change under *expansion*, *contraction* (remove a belief and
  its support) and *revision* (add a belief that contradicts the set). The
  operational lesson for NAADAP: when you retract a fact, you must also decide
  what to do with everything that depended on it — retraction is not deletion,
  and the AGM postulates (especially **minimal change**) are the right
  discipline. Practically: a retracted KB row becomes a row with
  `invalidated_at` set, not a deleted row.

**Temporal scoping of facts.**

- **Talukdar, P. P., Wijaya, D. T., Mitchell, T. "Coupled Temporal Scoping of
  Relational Facts." *WSDM 2012*, Seattle, pp. 73–82.
  DOI 10.1145/2124295.2124307.** **PARTIALLY VERIFIED** — record confirmed via
  ACM DL and a CMU-hosted PDF (https://www.cs.cmu.edu/~tom/pubs/talukdar-wsdm12.pdf);
  full text not retrieved. Their CoTS system infers the *validity interval* of
  a relational fact by macro-reading temporal signals — Google Books Ngram
  frequency curves and newswire document creation times — and **coupling**
  constraints across mutually exclusive facts (a person holds one office at a
  time), so evidence about one fact constrains the scope of another. Offline and
  corpus-hungry; not usable in NAADAP's regime, but the *coupling* idea is
  directly applicable: "vehicle X's ordering period ended" and "vehicle X is
  eligible for new orders" are coupled, and one constrains the other.

**Direct staleness detection.**

- **Hao, S., Chai, C., Li, G., Tang, N. "Outdated Fact Detection in Knowledge
  Bases." *ICDE 2020* (IEEE 36th International Conference on Data Engineering).**
  Retrieved (full text): https://dbgroup.cs.tsinghua.edu.cn/ligl/papers/icde2020-kb.pdf
  This is the most directly relevant paper I found. Framing: "there exist
  outdated facts in most KBs" because KBs are built once and the world moves;
  the standard fix — mining up-to-date facts from news — has "limited coverage"
  and is expensive.

  Their mechanism is a **human-in-the-loop** classifier:
  1. Train a binary classifier to predict `P(fact is outdated)` from features
     extracted from the **KB revision history** — notably the fact's
     **historical update frequency** and its **existence time** (how long the
     current value has been in place without change).
  2. Iteratively select the highest-likelihood candidates and issue **Human
     Intelligence Tasks** asking a person to verify whether the fact is indeed
     outdated.
  3. Use **logical rules** to propagate human feedback: if `(Curry, playsFor,
     WarriorsX)` is confirmed outdated, rules deduce that correlated facts are
     also outdated. Rule-derived outdated facts are fed back as training data,
     amplifying each human answer.

  Evaluated on YAGO and DBpedia.

  **The transferable insight for NAADAP is feature (1), and it is cheap.**
  *Historical update frequency* and *existence time* are computable from the KB's
  own version history with zero external data and zero network. A vehicle
  ceiling that has been modified four times in three years and has not been
  touched in 400 days is far more likely stale than a scope narrative that has
  never changed in eight years. That is a **deterministic, offline, explainable
  risk score**, and it is strictly better than a fixed review interval because
  it allocates the analyst's limited attention to the fields most likely to have
  moved. Implement it as two columns (`update_count`, `days_since_last_change`)
  and a documented, hand-written scoring rule — not a learned classifier, so it
  stays deterministic and inspectable.

- Related but not retrieved in full, listed for the team's follow-up:
  "Deep Outdated Fact Detection in Knowledge Graphs" (arXiv:2402.03732);
  "HOFD: An Outdated Fact Detector for Knowledge Bases"; "When Facts Expire:
  Benchmarking Temporal Validity in Knowledge Graphs" (CIKM 2025,
  DOI 10.1145/3746252.3761648). All **UNVERIFIED**.

### 8.3 THE VERDICT: a versioned tabular knowledge base, not an RDF/OWL stack

**Recommendation: build the NAADAP vehicle knowledge base as versioned,
hashed, schema-validated tabular files with explicit PROV-named provenance
columns. Do not adopt RDF, OWL, SPARQL, or a triple store.**

The argument, in the order a sceptical reviewer would attack it:

**1. The data is tabular. It is not a graph.**
A vehicle knowledge base is: one row per contract vehicle, with typed scalar
fields (ordering-period end date, ceiling, remaining headroom), a few
set-valued fields (eligible ordering activities, allowed PSC/NAICS codes,
socioeconomic pools), one long-text field (scope narrative), and one histogram
(empirical distribution of historical orders). There is **one** genuinely
hierarchical structure — the PSC/NAICS code taxonomy — and it is a *tree with
an authoritative published definition*, which is a `parent_code` column plus a
materialized `ancestor_codes` column. There are no deep join paths, no
multi-hop queries, no schema heterogeneity across sources, and no need to merge
independently-authored graphs. RDF's comparative advantage is precisely
schema-free integration of heterogeneous, independently-authored, link-dense
data. **None of those conditions hold here.** Adopting RDF for a few dozen rows
of well-typed data is paying the graph tax for graph benefits you will never
collect.

**2. The determinism requirement is cheaper to meet with bytes than with
triples.** The requirement is the same top-5 output in ≥95 % of runs, plus a
hashed artifact. A TSV with a frozen column order and a frozen row sort is
canonical *by construction*: `sha256sum vehicles.tsv` is the artifact hash, full
stop. An RDF graph is an unordered set with blank nodes, so getting a stable
hash requires the **RDF Dataset Canonicalization** Recommendation (§3.5), whose
own specification warns that adversarial or unlucky datasets "may be constructed
to prevent this algorithm from terminating in a reasonable amount of time" and
tells implementers to defend against denial of service. You would be introducing
a graph-isomorphism-adjacent algorithm into a system with a 30-minute wall-clock
budget, in order to recover a property `sort` gives you free. (And I could not
verify any .NET implementation of RDFC-1.0 exists.)

**3. Reasoning is either unnecessary or unbounded.** §3.2 lays out the profile
complexities. The only inference NAADAP needs is PSC/NAICS ancestor closure and
maybe eligibility propagation — both of which are transitive closure over a
small tree, computable at build time in a few lines of C#. If you take the
general OWL path you take on a worst case that ranges from PTime-complete
(EL/RL) to **N2ExpTime-complete** (OWL 2 DL), on 1 CPU core, under a hard SLA.
And in .NET there is **no OWL 2 reasoner at all** — dotNetRDF gives you
RDFS/SKOS materialization and nothing more — so "we might need OWL later" is not
even an available option without adding a JVM to the container.

**4. The dependency cost is real and the project has already priced it.**
`docs/DEPENDENCIES.md` records CORE-240: `src/Naadap.Core` takes **zero**
third-party dependencies, and every other `PackageReference` needs an inline
justification verified by inspection test TP-920. dotNetRDF brings AngleSharp,
HtmlAgilityPack and Newtonsoft.Json into a deliverable whose entire current
third-party surface is PdfPig and DocumentFormat.OpenXml. Writing a TSV reader,
a schema validator and a JSON emitter against the .NET 9 BCL is perhaps 600–900
lines of code the team owns, tests, and can explain line by line in a defence
package. That is a *better* position for a legal determination than "we depend
on a 5,000-commit third-party graph library and its three transitive
dependencies."

**5. Auditability favours text files decisively.** The requirement is that every
output trace to a KB row or document span, defensible in a legal determination.
A reviewer can open `vehicles.tsv` and `provenance.tsv` in Excel. `git diff`
shows exactly which field changed between KB versions, and `provenance.tsv`
shows who changed it, from what source, at what retrieval time, and why
(`change_note`). Doing the same with a serialized RDF graph requires a diff tool
that understands graph isomorphism, and doing it with a triple store requires
the store. **The single most valuable property of the knowledge base is that a
contracting officer can read it.** Do not trade that away.

**6. Everything genuinely valuable in the semantic-web stack is a discipline,
not a runtime.** Adopt all four disciplines and none of the runtimes:

- **From SHACL**: the validation-report structure. Write a build-time validator
  in C# that emits
  `{row_id, column, value, rule_id, rule_kind, severity, message}` per defect,
  with `severity ∈ {violation, warning, info}`; `violation` fails the build.
  Rule kinds mirror the Core components: `min_count`, `max_count`, `datatype`,
  `pattern`, `in`, `min_inclusive`, `max_inclusive`, `less_than`, `closed`. Keep
  the rule set **non-recursive** (§3.6.4) so validation is linear and always
  terminates. Encode the rules declaratively in a JSON file so they are diffable
  and reviewable separately from the code.
- **From PROV-O**: the column names and, more importantly, the questions.
  `had_primary_source`, `was_derived_from`, `was_revision_of`, `was_attributed_to`,
  `generated_by_activity`, `generated_at_time`, `retrieved_at`,
  `invalidated_at_time`, `invalidation_reason`. §4.5 has the full schema.
- **From SKOS**: `pref_label` / `alt_labels` / `hidden_labels` for the vehicle
  gazetteer, one preferred label per vehicle, labels pairwise disjoint; and the
  `broader` vs `broaderTransitive` distinction for PSC/NAICS — store the direct
  parent, materialize the ancestor set, never conflate them. `change_note` and
  `history_note` on every KB row.
- **From RDFC-1.0 / in-toto**: canonicalize before you hash; hash with SHA-256;
  put the artifact digest in a signed build attestation alongside the source
  document URIs, hashes and retrieval dates.

**7. The escape hatch is free, so take it.** Emit the KB as JSON Lines in
addition to (or instead of) TSV, keep every column name IRI-mappable, and ship
a `context.jsonld` file that maps each column to a PROV-O / SKOS / custom IRI.
The same bytes then *are* JSON-LD, and therefore *are* RDF, the day someone
actually needs SPARQL — at which point they load them into dotNetRDF in a tool
project and run whatever they like. You pay nothing now and preserve the option
completely. **This is the only part of the RDF stack I recommend adopting up
front, and it costs one file.**

**What would change my mind.** I would revisit this if any of these became true:
(a) the KB grew past a few thousand vehicles *and* needed to be merged with
externally-authored graphs the team does not control; (b) a downstream consumer
required a SPARQL endpoint as a contractual interface; (c) the reasoning
requirement grew to include genuine subsumption over a class hierarchy the team
authors itself, rather than closure over a published code tree. None of these is
on the horizon for a 20-document, offline, 30-minute, 2 GB prize-challenge
deliverable.

### 8.4 The staleness design, concretely

Combining §4.4 and §8.2, here is the recommended staleness mechanism. All four
checks are deterministic, offline, and explainable.

| # | Check | Inputs (columns) | Fires when | Catches |
|---|---|---|---|---|
| 1 | **Review interval overdue** | `next_review_due` | `build_date > next_review_due` | Everything. This is the backstop that would have caught all three past failures. |
| 2 | **Source revision drift** | `source_revision_seen`, plus the current revision recorded at refresh time | recorded ≠ current | "instruction rev C vs rev E" |
| 3 | **Source hash drift** | `source_sha256` | recomputed ≠ recorded | any silent edit to a source document |
| 4 | **Validity window expiry** | `value` where `value_datatype = xsd:date` | `build_date > value` | "regulation deleted in 2018", expired ordering periods |
| 5 | **Volatility risk score** *(Hao et al. feature, adapted)* | `update_count`, `days_since_last_change` | hand-written monotone score exceeds threshold | prioritizes analyst attention on historically volatile fields |

Emit checks 1–4 as **build-time SHACL-style validation results** — check 4 and
an overdue check 1 as `violation` (fail the build, refuse to ship a knowingly
stale KB); check 5 as `info` (a prioritized worklist). Check 2 and 3 run at
*refresh* time, when the analyst is online and can re-fetch; their output is a
worklist of fields to re-verify, and any field on that list gets its
`next_review_due` pulled forward.

The key architectural commitment, and it is the Doyle-1979 one: **every KB field
records its justification, and the justification index is reversible.** Given a
source document, you must be able to enumerate every field derived from it in
one pass. That is a single index over `provenance.source_doc_id`. Without it,
checks 2 and 3 cannot propagate, and "this document was superseded" does not
translate into "these eleven fields are suspect."

---

## 9. Citation ledger

**Retrieved in full text and read during this session (highest confidence):**

| Work | Where I got it |
|---|---|
| Banko et al., "Open Information Extraction from the Web," IJCAI 2007, pp. 2670–2676 | https://www.ijcai.org/Proceedings/07/Papers/429.pdf |
| Fader, Soderland, Etzioni, "Identifying Relations for Open Information Extraction," EMNLP 2011, pp. 1535–1545, ACL ID D11-1142 | https://reverb.cs.washington.edu/emnlp11.pdf + https://aclanthology.org/D11-1142/ |
| Angeli, Johnson Premkumar, Manning, "Leveraging Linguistic Structure For Open Domain Information Extraction," ACL-IJCNLP 2015, pp. 344–354, ACL ID P15-1034 | https://aclanthology.org/P15-1034.pdf |
| Mintz, Bills, Snow, Jurafsky, "Distant supervision for relation extraction without labeled data," ACL-IJCNLP 2009, pp. 1003–1011, ACL ID P09-1113 | https://aclanthology.org/P09-1113.pdf |
| Kolluru et al., "OpenIE6," EMNLP 2020, pp. 3748–3761, arXiv:2010.03147 | https://aclanthology.org/2020.emnlp-main.306.pdf |
| Hendrycks, Burns, Chen, Ball, "CUAD," NeurIPS 2021 Datasets & Benchmarks, arXiv:2103.06268 | https://arxiv.org/pdf/2103.06268 |
| Bast, Hertel, Prange, "A Fair and In-Depth Evaluation of Existing End-to-End Entity Linking Systems," arXiv:2305.14937 | https://arxiv.org/pdf/2305.14937 |
| Asim et al., "A survey of ontology learning techniques and applications," *Database* 2018, DOI 10.1093/database/bay101 | https://pmc.ncbi.nlm.nih.gov/articles/PMC6173224/ |
| "Ontology Learning from Text: an Analysis on LLM Performance," CEUR-WS Vol-3874 paper 5 | https://ceur-ws.org/Vol-3874/paper5.pdf |
| **W3C** SHACL, Recommendation 2017-07-20 | https://www.w3.org/TR/shacl/ |
| **W3C** PROV-O, Recommendation 2013-04-30 | https://www.w3.org/TR/prov-o/ |
| **W3C** PROV-OVERVIEW (family listing) | https://www.w3.org/TR/prov-overview/ |
| **W3C** OWL 2 Profiles (2nd ed.), Recommendation 2012-12-11 | https://www.w3.org/TR/owl2-profiles/ |
| **W3C** SKOS Reference, Recommendation 2009-08-18 | https://www.w3.org/TR/skos-reference/ |
| **W3C** JSON-LD 1.1, Recommendation 2020-07-16 | https://www.w3.org/TR/json-ld11/ |
| **W3C** RDF Dataset Canonicalization (RDFC-1.0), Recommendation 2024-05-21 | https://www.w3.org/TR/rdf-canon/ |
| **W3C** RDF 1.2 Concepts, Candidate Recommendation Snapshot 2026-04-07 | https://www.w3.org/TR/rdf12-concepts/ |
| **W3C** Time Ontology in OWL, CR Draft 2022-11-15 | https://www.w3.org/TR/owl-time/ |
| Bordes et al., "Translating Embeddings for Modeling Multi-relational Data," NIPS 2013 | NeurIPS proceedings PDF (paper 5071) |
| Yang, Yih, He, Gao, Deng, "Embedding Entities and Relations…," ICLR 2015, arXiv:1412.6575 | https://arxiv.org/pdf/1412.6575 |
| Trouillon et al., "Complex Embeddings for Simple Link Prediction," ICML 2016, arXiv:1606.06357 | https://arxiv.org/pdf/1606.06357 |
| Sun, Deng, Nie, Tang, "RotatE," ICLR 2019, arXiv:1902.10197 | https://arxiv.org/pdf/1902.10197 |
| Sun, Vashishth, Sanyal, Talukdar, Yang, "A Re-evaluation of Knowledge Graph Completion Methods," ACL 2020, arXiv:1911.03903 | https://arxiv.org/pdf/1911.03903 |
| Okulmus et al., "SHACL Validation under the Well-founded Semantics," KR 2024 | https://proceedings.kr.org/2024/52/kr2024-0052-okulmus-et-al.pdf |
| Hao, Chai, Li, Tang, "Outdated Fact Detection in Knowledge Bases," ICDE 2020 | https://dbgroup.cs.tsinghua.edu.cn/ligl/papers/icde2020-kb.pdf |
| in-toto Attestation v1 Statement spec | https://github.com/in-toto/attestation/blob/v1.0/spec/v1.0/statement.md |
| Licence files for reverb / ollie / CoreNLP / openie6 / pySHACL / TopQuadrant-shacl / trinity-rdf | raw.githubusercontent.com, fetched 2026-09-15 |
| dotNetRDF repo metadata + NuGet package pages (`dotNetRdf.Core`, `dotNetRdf.Shacl`) | GitHub API + nuget.org, 2026-09-15 |

**PARTIALLY VERIFIED — bibliographic record confirmed from at least two
independent indexes, full text NOT retrieved. Do not quote these; cite them
only for the claims attributed above.**

- Mausam, Schmitz, Soderland, Bart, Etzioni, "Open Language Learning for
  Information Extraction," EMNLP-CoNLL 2012, pp. 523–534, ACL ID D12-1048.
  (Metadata page retrieved; full text not read.)
- Riedel, Yao, McCallum, "Modeling Relations and Their Mentions without Labeled
  Text," ECML PKDD 2010 Part III, LNCS 6323, pp. 148–163,
  DOI 10.1007/978-3-642-15939-8_10.
- Carroll, Bizer, Hayes, Stickler, "Named graphs, provenance and trust,"
  WWW 2005, pp. 613–622, DOI 10.1145/1060745.1060835; extended as "Named
  graphs," *J. Web Semantics* 3(4):247–267, DOI 10.1016/j.websem.2005.09.001.
- Gutierrez, Hurtado, Vaisman, "Introducing Time into RDF," *IEEE TKDE*
  19(2):207–218, 2007.
- Corman, Reutter, Savković, "Semantics and Validation of Recursive SHACL,"
  ISWC 2018, pp. 318–336, DOI 10.1007/978-3-030-00671-6_19. *(NP-hardness claim
  above is sourced from the KR 2024 paper's full text, not this one.)*
- Ruffinelli, Broscheit, Gemulla, "You CAN Teach an Old Dog New Tricks! On
  Training Knowledge Graph Embeddings," ICLR 2020.
- Akrami, Saeef, Zhang, Hu, Li, "Realistic Re-evaluation of Knowledge Graph
  Completion Methods: An Experimental Study," SIGMOD 2020,
  DOI 10.1145/3318464.3380599, arXiv:2003.08001.
- Ratinov, Roth, Downey, Anderson, "Local and Global Algorithms for
  Disambiguation to Wikipedia," ACL-HLT 2011, pp. 1375–1384, ACL ID P11-1138.
- Hoffart et al., "Robust Disambiguation of Named Entities in Text,"
  EMNLP 2011, pp. 782–792.
- Zhu, Pradhan, Zeldes, "OntoGUM," ACL-IJCNLP 2021 Short Papers, pp. 461–467,
  ACL ID 2021.acl-short.59, arXiv:2106.00933.
- Doyle, J., "A Truth Maintenance System," *Artificial Intelligence*
  12(3):231–272, 1979, DOI 10.1016/0004-3702(79)90008-0.
- Alchourrón, Gärdenfors, Makinson, "On the Logic of Theory Change,"
  *J. Symbolic Logic* 50(2):510–530, 1985.
- Talukdar, Wijaya, Mitchell, "Coupled Temporal Scoping of Relational Facts,"
  WSDM 2012, pp. 73–82, DOI 10.1145/2124295.2124307.
- Buitelaar, Cimiano, Magnini (eds.), *Ontology Learning from Text: Methods,
  Evaluation and Applications*, IOS Press, FAIA vol. 123, 2005,
  ISBN 1-58603-523-1. (Book record verified via the *Computational Linguistics*
  32(4):569 review page; the "An Overview" chapter's own text not retrieved —
  the layer enumeration is corroborated from the two full-text sources above.)
- in-toto / SLSA Provenance v1.x predicate schema details.

**UNVERIFIED — surfaced in search only, could not retrieve. DO NOT CITE without
retrieving first.**

- "Ontology Learning from Text: Why the Ontology Learning Layer Cake is not
  Viable," *Int. J. Signs and Semiotic Systems* 4(2), 2015,
  DOI 10.4018/ijsss.2015070101. (ACM DL returned HTTP 403; authors unknown to me.)
- Welty & Fikes, "A Reusable Ontology for Fluents in OWL," FOIS 2006 (4D fluents).
- de Kleer, "An Assumption-based TMS," *Artificial Intelligence* 28(2), 1986.
- "Deep Outdated Fact Detection in Knowledge Graphs," arXiv:2402.03732.
- "HOFD: An Outdated Fact Detector for Knowledge Bases."
- "When Facts Expire: Benchmarking Temporal Validity in Knowledge Graphs,"
  CIKM 2025, DOI 10.1145/3746252.3761648.
- `Canyala.Mercury` (.NET semantic web libraries).
- dotNetRDF's exact licence *text* in-repo (NuGet metadata says MIT for both
  `dotNetRdf.Core` and `dotNetRdf.Shacl`; GitHub's detector reports
  `NOASSERTION`. **Confirm before shipping.**)
- The claim that dotNetRDF's SHACL processor is "fully compliant SHACL Core and
  SHACL-SPARQL" and that advanced features are "not planned" — this came from a
  search summary of the dotNetRDF docs and Discussion #559, which I did not
  fetch directly.

