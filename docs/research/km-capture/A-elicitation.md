# A — Knowledge Elicitation and Its Serialization
### Literature review for NAADAP (NAVAIR/NAWCAD prize challenge)
Research date: 2026-09-15. All citations carry a verification status. Working files (downloaded PDFs, extracted text, OCR output) are in `./pdfs/` alongside this document.

---

## 0. How this review was verified, and what could not be

**Verification protocol used.** Every citation below was checked against at least one machine-readable authority, in this order of preference:

1. **Crossref REST API** (`api.crossref.org/works`) — returns publisher-deposited metadata: authors, year, exact title, container title, volume/issue/pages, DOI, type. This is the strongest available check short of holding the paper.
2. **Direct retrieval of the PDF** — downloaded with `curl`, text extracted with PyMuPDF, and where the PDF was a scan, OCR'd with Tesseract. Where I did this, the review quotes the paper's own words and I mark the source **FULL TEXT RETRIEVED**.
3. **Semantic Scholar Graph API** for open-access PDF locations and abstracts.

**Status labels used throughout:**

- **[VERIFIED+FT]** — bibliographic metadata confirmed AND full text retrieved and read. Claims about mechanism are quoted from the paper itself.
- **[VERIFIED]** — bibliographic metadata confirmed via Crossref/publisher; full text not obtained. Mechanism claims come from a *different* verified full-text source that cites it, and that chain is stated explicitly.
- **[PARTIAL]** — the work exists and metadata is confirmed, but a specific numeric claim attributed to it came from a search-engine summary of a paywalled page and I could not retrieve the page itself. Treat the number as unconfirmed.
- **[UNVERIFIED]** — could not confirm. Explicitly flagged; not relied upon.

**What I could not retrieve, and why it matters:**

- **DTIC (`apps.dtic.mil`) is blocked from this environment** — every request returns an HTML JavaScript challenge or HTTP 403, including with a browser User-Agent. This means the primary ACTA technical report (Militello, Hutton, Pliske, Knight & Klein, 1997, AD-A335225) and the CDM report AD-A466054 **could not be read**. The ACTA mechanism below is reconstructed from (a) the peer-reviewed *Ergonomics* abstract, retrieved verbatim; (b) an open-access paper that applied ACTA end-to-end; and (c) MIT course slides that reproduce the probe wording and table columns. Those slides are a *teaching secondary source*, and I label every claim that rests only on them.
- **Klein, Calderwood & MacGregor (1989)** — IEEE Xplore returned no body. DOI and full metadata verified via Crossref. The 5-step CDM procedure quoted below comes from Shadbolt & Smart (2015), which I retrieved in full and which quotes Klein et al. with page numbers.
- **Gaines (2013), "Knowledge acquisition: Past, present and future"** — DOI verified; the author's own archived PDF at `pages.cpsc.ucalgary.ca` and the UVic mirror both return 404. **I make no substantive claim from this paper.** It is listed for completeness only.
- **Hoffman, Coffey, Ford & Novak (2006)**, *Weather and Forecasting* — DOI verified; AMS journal site returns 403. The quantitative details of that study (hours, map counts) are therefore **UNVERIFIED** and I do not state them.
- **Burton, Shadbolt, Rugg & Hedgecock (1990)** — DOI verified; both the Southampton eprints copy and ScienceDirect return 403. The abstract's findings are marked **[PARTIAL]**.

I have not dropped or silently smoothed over any of these. Where the evidence is thin, the recommendation below is correspondingly hedged.

---

## 1. Cognitive Task Analysis (CTA)

CTA is not one method. It is a family: a **knowledge elicitation** step (get it out of the human), a **data analysis** step (structure it), and a **knowledge representation** step (put it in a form something else can consume). Crandall, Klein & Hoffman's book is the standard practitioner reference for the family.

> Crandall, B., Klein, G. A., & Hoffman, R. R. (2006). *Working Minds: A Practitioner's Guide to Cognitive Task Analysis*. Cambridge, MA: The MIT Press. DOI: `10.7551/mitpress/7304.001.0001` — **[VERIFIED]** (Crossref: MIT Press, 2006, authors confirmed in that order).

The broader placement of elicitation inside knowledge engineering is set out in a chapter I retrieved in full and lean on heavily:

> Shadbolt, N., & Smart, P. R. (2015). Knowledge Elicitation: Methods, Tools and Techniques. In J. R. Wilson & S. Sharples (Eds.), *Evaluation of Human Work* (4th ed., pp. 196–233). Boca Raton, FL: CRC Press. DOI: `10.1201/b18362-18` — **[VERIFIED+FT]** (Crossref confirms chapter/pages/publisher; full text retrieved from the author's own copy at `paulsmart.cognosys.co.uk/pubs/2015/Knowledge%20Elicitation.pdf`, 43 pp.)

Shadbolt & Smart frame the economics exactly as NAADAP does, which is worth quoting because it is the problem statement:

> "…the effort spent in gathering, transcribing and analysing an expert's knowledge. We would also like to minimise the time spent with expensive and scarce experts. And, of course, we would like to maximise the yield of usable knowledge."

And, directly on NAADAP's use case:

> "[the goal] may be to document the work-related knowledge and expertise … requirement to capture the knowledge of individuals who are about to leave an organization or who have recently retired."

### 1.1 The Critical Decision Method (CDM)

> Klein, G. A., Calderwood, R., & MacGregor, D. (1989). Critical decision method for eliciting knowledge. *IEEE Transactions on Systems, Man, and Cybernetics*, 19(3), 462–472. DOI: `10.1109/21.31053` — **[VERIFIED]** (Crossref: authors, volume 19, issue 3, pages 462–472, IEEE, 1989.)

> Hoffman, R. R., Crandall, B., & Shadbolt, N. (1998). Use of the Critical Decision Method to Elicit Expert Knowledge: A Case Study in the Methodology of Cognitive Task Analysis. *Human Factors*, 40(2), 254–276. DOI: `10.1518/001872098779480442` — **[VERIFIED]** (Crossref: SAGE, June 1998, vol 40 iss 2 pp 254–276.)

**What it is.** A retrospective interview built on Flanagan's critical incident technique, extended with cognitive probes. Shadbolt & Smart quote Klein et al. directly: CDM is "a retrospective interview strategy that applies a set of cognitive probes to actual nonroutine incidents that required expert judgement or decision making" (Klein et al., 1989, p. 464).

**The actual procedure — five steps** (Shadbolt & Smart 2015, summarizing Klein et al. 1989, with page citations to the original):

1. **Select incident.** The expert recalls a specific non-routine incident. Selection criterion, quoted from Klein et al. (1989, p. 466): "select an incident that was challenging and that, in his or her own decisionmaking, might have differed from someone with less experience."
2. **Obtain unstructured incident account.** The expert narrates it in their own words, uninterrupted. This does two jobs: bootstraps the analyst's understanding, and re-activates the expert's episodic memory as scaffolding for the probing.
3. **Construct incident timeline.** The analyst converts the narrative into an ordered sequence of events with durations. *This is the first structured artifact and the first thing a program could consume.*
4. **Decision point identification.** Points on the timeline are marked where an alternative course of action existed, or where a different expert might have chosen differently. Non-decisions are dropped.
5. **Decision point probing.** Each marked point is worked with cognitive probes.

**The probe set** (Shadbolt & Smart 2015, Table 1 — their working set, in the CDM tradition; not verbatim Klein et al. 1989):

| Probe type | Probe question |
|---|---|
| Cues | What were you seeing, hearing, smelling? |
| Knowledge | What information did you use in making this decision? How was it obtained? |
| Analogues | Were you reminded of any previous incidents? |
| Scenarios | Does this case fit a standard or typical scenario? Does it fit a scenario you were trained to deal with? |
| Goals | What were your specific goals and objectives at the time? |
| Options | What other courses of action were considered or available? |
| Choice | How was this option selected / other options rejected? What rule was being followed? |
| Anticipation | Did you imagine the possible consequences of this action? Did you imagine the events that would unfold? |
| Experience | What specific training or experience was necessary or helpful in this decision? What more would have helped? |
| Decision making | How much time pressure was involved? How long did it take to make the decision? |
| Aiding | What training, knowledge or information could have helped? |
| Situation assessment | If you were asked to describe the situation to a colleague at this point, how would you summarise it? |
| Errors | What mistakes are likely at this point? How might a novice have behaved differently? |
| Hypotheticals | If a key feature of the situation had been different, what differences would it have made in your decision? |

**The output artifacts.** This is the part that matters for NAADAP, because these are *tables*, not prose:

- **Critical Cue Inventory (CCI)** — "a collection of all the perceptual cues that are used to guide the consideration and selection of particular decisions." A flat list of (cue, what it indicates, which decision it feeds).
- **Situation Assessment Record (SAR)** — "records the changes in goals and cue usage associated with situation assessment processes. It typically combines information about the cues being sought or identified, the expectancies generated by these cues, the goals activated by the current situation, and the selected course of action."

So the SAR is natively a 4-column relation: **cue → expectancy → activated goal → selected action**. That is a directly serializable decision-rule table.

**Cost.** Shadbolt & Smart: "A typical CDM session can last around 2 hours." **[VERIFIED+FT]**

**Stated limitations** (same source, and honest ones):
- In *distributed* problem solving, no individual handles more than one element of the task, so "individuals … would never know whether their judgements or assessments were correct within the context of the larger socially-distributed process." **This is a direct hit on NAADAP's domain**: vehicle selection is distributed across a PCO, a requirements owner, a legal reviewer, and a small-business specialist. No single SME owns the whole decision.
- In high-workload domains, "incidents and events can become merged" — experts recount an incident and then cannot produce a coherent timeline.

**Applicability to NAADAP.** High, with the distributed-cognition caveat. The consolidation/vehicle-selection decision *does* have non-routine incidents ("the time we nearly bundled X and legal stopped it"), and the SAR structure maps cleanly onto NAADAP's need for citable evidence: each recommendation can point at a rule whose provenance is a specific elicited incident.

### 1.2 Applied Cognitive Task Analysis (ACTA)

> Militello, L. G., & Hutton, R. J. B. (1998). Applied cognitive task analysis (ACTA): a practitioner's toolkit for understanding cognitive task demands. *Ergonomics*, 41(11), 1618–1641. DOI: `10.1080/001401398186108` — **[VERIFIED]** (Crossref: Informa/Taylor & Francis, Nov 1998, vol 41 iss 11 pp 1618–1641, both authors confirmed.) Abstract retrieved verbatim via Semantic Scholar API.

Abstract, verbatim (retrieved):

> "Cognitive task analysis (CTA) is a set of methods for identifying cognitive skills, or mental demands, needed to perform a task proficiently. The product of the task analysis can be used to inform the design of interfaces and training systems. However, **CTA is resource intensive and has previously been of limited use to design practitioners.** A streamlined method of CTA, Applied Cognitive Task Analysis (ACTA), is presented in this paper. **ACTA consists of three interview methods** that help the practitioner to extract information about the cognitive demands and skills required for a task. ACTA also allows the practitioner to represent this information in a format that will translate more directly into applied products…"

Note the precision: **three interview methods plus one representation format**. The common "four stages of ACTA" phrasing counts the representation (the cognitive demands table) as a stage. Both framings describe the same thing.

> Related primary report: Militello, L. G., Hutton, R. J. B., Pliske, R. M., Knight, B. J., & Klein, G. (1997). *Applied Cognitive Task Analysis (ACTA) Methodology*. Final technical report, Klein Associates Inc., prepared for the Navy Personnel Research and Development Center, Contract N66001-94-C-7034. DTIC AD-A335225. — **[UNVERIFIED — could not retrieve]**. DTIC blocks this environment (403 / JS challenge on both `/sti/tr/pdf/` and `/sti/pdfs/` paths). The report demonstrably exists (it is indexed and cited consistently), but I did not read it, so nothing below is sourced to it.

**The procedure.**

**Stage 1 — Task Diagram Interview.** Purpose: a broad map of the task that flags which parts are cognitively hard, so the expensive probing is aimed. Mechanics (per the MIT teaching slides, which attribute Militello & Hutton — *secondary source*):
- Ask the SME to decompose the task into steps or subtasks — "3–6 subtasks, ideally."
- Ask which of those subtasks require *cognitive skill* — judgments, assessments, problem solving — as opposed to procedure execution.
- Draw the subtasks as a sequence diagram with the cognitively demanding ones marked.

Corroborated independently by an open-access application of ACTA, which produced "a detailed hierarchical task diagram of an extensive emergency scenario" as Stage 1 output:

> Stanik, C., Puhlfürß, T., Mahler, A., Sasu, P. B., Reip, W., & Maalej, W. (2021). Lessons Learned from Customizing and Applying ACTA to Design a Novel Device for Emergency Medical Care. arXiv:2108.05622. — **[VERIFIED+FT]** (PDF retrieved, 10 pp.; a peer-reviewed-venue preprint reporting an industry project. Confirms: four consecutive stages; Stage 1 task diagram; Stage 2 knowledge audit with 11 SMEs via semi-structured video interviews; Stage 3 simulation interview, adapted to a design workshop; Stage 4 output consolidation.)

**Stage 2 — Knowledge Audit.** Purpose, per the MIT slides: "surveys the expertise required for a task through probing concrete examples in the job context." The probes target named categories of expert knowledge: "diagnosing & predicting, situation awareness, perceptual skills, developing and knowing when to apply tricks of the trade, improvising, metacognition, recognizing anomalies, and compensating for equipment limitations."

The eight probe headings, with the two exemplar wordings I could confirm from the slides:

1. **Past & Future** — "Is there a time when you walked into the middle of a situation and knew exactly how things got there and where they were headed?"
2. **Big Picture** — "Can you give me an example of what is important about the Big Picture for this task?"
3. **Noticing** — (perceptual "popping out"; exact wording not confirmed)
4. **Job Smarts** — tricks of the trade / workarounds
5. **Opportunities / Improvising**
6. **Self-Monitoring** — metacognition
7. **Anomalies / Off-nominal situations**
8. **Equipment difficulties** — compensating for tool limitations

**Status of this list: [PARTIAL].** The eight headings are consistent across three independent secondary sources I saw; the exact interrogative wording of six of the eight is not confirmed from a primary source, because DTIC is blocked. **If NAADAP writes these probes into a protocol, get AD-A335225 or the *Ergonomics* article from a library first and confirm the wording.**

Critically, the audit is not just "tell me a story." Each probe yields a structured triple per the slides: "Probes result in an inventory of task-specific expertise" — and in practice each answer is logged with (a) the concrete example, (b) the cues/strategies used, and (c) why a novice would get it wrong.

**Stage 3 — Simulation Interview.** Purpose: "allows interviewer to probe the cognitive processes of the expert within the context of a specific scenario." Mechanics:
- Identify a challenging scenario — reuse a training scenario if one exists, or construct one from a prior CTA.
- Walk the expert through it; have them identify major events.
- At each event, apply the compound probe (verbatim from the slides): *"As the [job] in this scenario, what actions, if any, would you take at this point in time? What do you think is going on here? What is your assessment of the situation at this point in time? What pieces of information led you to this situation assessment and these actions? What errors would an inexperienced person be likely to make in this situation?"*

**Output is a table with five columns:** `Events | Actions | Assessment | Critical Cues | Potential Errors`.

**Stage 4 — Cognitive Demands Table.** Purpose (slides): "means to consolidate & synthesize data … arrange the data in terms of the type of information that the designers will need."

**Output is a table with four columns:** `Difficult cognitive element | Why difficult? | Common errors | Cues and strategies used`.

Worked row from the slides (firefighting): *Difficult element:* "Knowing where to search after an explosion." *Why difficult:* "Novices may not be trained in explosions. Other training suggests to start at the source. Not everyone knows about MSDS." *Common errors:* "Novice would be likely to start at source of explosion. Starting at source is a rule of thumb." *Cues and strategies:* "Start where likely to find victims. Refer to MSDS. Consider type of structure."

**This is the single most important structural finding in this review for NAADAP.** Both ACTA output artifacts are *already relations with fixed, named columns*. They are CSV or JSON on day one. No interpretation layer is required to serialize them, and each row is independently citable.

**Validity / reliability evidence.** From the MIT slides' account of Militello & Hutton's own evaluation study (**[PARTIAL]** — secondary source reporting the primary study):
- Participants: graduate students with no prior CTA experience — 12 in a firefighting domain, 11 in electronic warfare.
- Design: random assignment to a 2-hour general-CTA workshop (control) or a **6-hour ACTA workshop** (treatment). Each then ran one expert interview, observed another, attended a 4-hour analysis session, and produced 10 learning objectives plus revised training material.
- Reliability indices reported: information elicited across Rasmussen's decision-making categories **74%**; domain-specific content categorized into task categories **81%**; expert rating of materials' importance **87.8%**; accuracy **71.4%**.
- Validity criterion for the cognitive demands table: "Items must address a cognitive skill or a cognitive challenge … NOT declarative knowledge."

That last criterion is a usable acceptance test. It also flags a real risk for NAADAP: much of what a contracting SME knows *is* declarative (which vehicle has which NAICS, what the ceiling is). ACTA will deliberately filter that out. NAADAP needs both, from different instruments.

The open-access ACTA application also records a criticism worth carrying forward: Militello & Hutton themselves "criticize the lack of well-established metrics for testing the [method]" (Stanik et al. 2021, paraphrasing the 1998 evaluation).

**Cost.** The verified data point is the training cost, not the elicitation cost: a practitioner can be brought to competence in **6 hours**. Stanik et al. ran **11 SME interviews** plus two design workshops over one year for a single device's UI requirements. Treat "~10 SMEs, ~1–2 h each, plus roughly equal analyst time for consolidation" as the planning figure.

### 1.3 Does CTA-derived material actually work?

> Tofel-Grehl, C., & Feldon, D. F. (2013). Cognitive Task Analysis–Based Training: A Meta-Analysis of Studies. *Journal of Cognitive Engineering and Decision Making*, 7(3), 293–304. DOI: `10.1177/1555343412474821` — **[VERIFIED]** (Crossref: SAGE, 2013, vol 7 iss 3 pp 293–304, both authors.)

Reported effect size **Hedges's *g* = 0.871** for CTA-based instruction, with the authors themselves noting the meta-analysis is limited by a small number of studies. **[PARTIAL]** — this number came from a search-engine summary of the publisher page; the USU DigitalCommons copy returned 403 and I could not read the paper. **Do not put this number in a deliverable without pulling the PDF.**

---

## 2. Concept Mapping as a Formal Elicitation Technique

This is the method with the best serialization story, by a wide margin, because the format is a published XML schema that I fetched and validated against a real sample file.

### 2.1 The Novak & Cañas procedure

> Novak, J. D., & Cañas, A. J. (2008). *The Theory Underlying Concept Maps and How to Construct and Use Them*. Technical Report IHMC CmapTools 2006-01 Rev 01-2008. Pensacola, FL: Institute for Human and Machine Cognition. — **[VERIFIED+FT]** (36-page PDF retrieved from `https://cmap.ihmc.us/docs/pdf/TheoryUnderlyingConceptMaps.pdf`; the report's own self-citation on page 1 matches this string exactly.)

**Data structure, in the authors' own words:**

> "Concept maps are graphical tools for organizing and representing knowledge. They include **concepts**, usually enclosed in circles or boxes … and relationships between concepts indicated by a connecting line linking two concepts. Words on the line, referred to as **linking words or linking phrases**, specify the relationship between the two concepts. We define concept as **a perceived regularity in events or objects, or records of events or objects, designated by a label.** … **Propositions** are statements about some object or event … Propositions contain two or more concepts connected using linking words or phrases to form a meaningful statement. Sometimes these are called **semantic units, or units of meaning**."

So the atomic unit is a **proposition = (concept, linking phrase, concept)** — a labeled directed edge, with the label itself being a first-class object rather than a URI drawn from a fixed vocabulary. That distinction matters downstream (see §2.3).

**Two further structural commitments:**

- **Hierarchy.** "the concepts are represented in a hierarchical fashion with the most inclusive, most general concepts at the top of the map and the more specific, less general concepts arranged hierarchically below."
- **Cross-links.** "relationships or links between concepts in different segments or domains of the concept map … In the creation of new knowledge, cross-links often represent creative leaps on the part of the knowledge producer."

**The construction procedure, step by step** (all from the retrieved report):

1. **Focus question.** "A good way to define the context for a concept map is to construct a Focus Question, that … Every concept map responds to a focus question." The report warns that mapmakers "deviate from the focus question and build a concept map that may be related to the [topic but not the question]," and reports that "not only the focus question, but also the root [concept]" changes the resulting map. **CmapTools enforces this: "Whenever a concept map is made with CmapTools and then saved, the maker is asked to provide a focus question, as well as key concepts."** That is a machine-enforced provenance field, for free.
2. **Parking lot.** "We refer to a list of concepts waiting to be added to a concept map as the parking lot." Concepts are enumerated first, ranked roughly general-to-specific, and then placed. "Some concepts may remain in the parking lot as the map is completed if the mapmaker sees no good connection."
3. **Build and iterate.** Place concepts, add linking phrases, "restructure the map" repeatedly. The report explicitly notes that using a projector during group sessions lets participants "[see] their work as they progress."
4. **Cross-link.** Add the inter-cluster links — the step the report treats as where new knowledge actually shows up.
5. **Revise.** Iterative; maps are never one-pass.

**Expert Skeleton Maps.** A directly relevant variant: "An 'expert skeleton' concept map has [a small number of concepts already placed by an expert] to build … on a solid foundation." The report is candid that this is "a research topic we are pursuing, and for which we don't have as much experience as with the focus question and parking lot starting [points]," and insists "the 'expert skeleton' concept maps should be built by an expert." For NAADAP this is the right pattern: a NAVAIR contracting expert lays down the skeleton (vehicle types, scope tests, consolidation triggers), and subsequent SMEs extend it rather than each starting from a blank canvas — which is what makes multiple SMEs' output mergeable.

**Knowledge Models.** The report notes CmapTools supports "Knowledge Models" — a concept map with resources (documents, images, URLs) attached to individual concept nodes. This is the mechanism by which an elicited proposition can carry its evidence: a node for "Consolidation" can hang off the actual FAR 7.107 text.

> Cañas, A. J., Carff, R., Hill, G., Carvalho, M., Arguedas, M., Eskridge, T. C., Lott, J., & Carvajal, R. (2005). Concept Maps: Integrating Knowledge and Information Visualization. In *Knowledge and Information Visualization* (LNCS), pp. 205–219. Springer. DOI: `10.1007/11510154_11` — **[VERIFIED]** (Crossref: all eight authors, pages, Springer LNCS, 2005.)

### 2.2 CXL — the serialization format (this is the load-bearing finding)

CXL (Concept Map Extensible Language) is an **open, published XML schema**. I fetched both the schema and a sample instance:

- Schema: `https://cmap.ihmc.us/xml/cmap.xsd` — **HTTP 200, 53,589 bytes retrieved.** Target namespace `http://cmap.ihmc.us/xml/cmap/`. Imports Dublin Core (`purl.org/dc/elements/1.1/`) and DC Terms for metadata. Root element `<cmap>` containing optional `<res-meta>` (resource metadata) and `<map>`.
- Sample instance: `https://cmap.ihmc.us/xml/PlantsSimple.cxl` — **HTTP 200 retrieved.** Reproduced in full:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<cmap xmlns="http://cmap.ihmc.us/xml/cmap/"
      xmlns:dc="http://purl.org/dc/elements/1.1/">
    <res-meta>
        <dc:title>Plants</dc:title>
        <dc:description>How do plants grow?</dc:description>
    </res-meta>
    <map>
        <concept-list>
            <concept id="1" label="Plants"/>
            <concept id="2" label="Leaves"/>
        </concept-list>
        <linking-phrase-list>
            <linking-phrase id="3" label="have"/>
        </linking-phrase-list>
        <connection-list>
            <connection from-id="1" to-id="3"/>
            <connection from-id="3" to-id="2"/>
        </connection-list>
        <concept-appearance-list>
            <concept-appearance id="1" x="73" y="56"/>
            <concept-appearance id="2" x="136" y="158"/>
        </concept-appearance-list>
        <linking-phrase-appearance-list>
            <linking-phrase-appearance id="3" x="104" y="107"/>
        </linking-phrase-appearance-list>
    </map>
</cmap>
```

**Read the structure carefully — this is the design decision NAADAP inherits.** The linking phrase is **reified as its own node with its own `id`**, and a proposition is *two* `<connection>` elements (`1→3`, `3→2`). It is not an RDF triple; it is a bipartite graph. Consequences:

- Converting CXL → triples is mechanical but requires a join: walk `connection-list`, find each `linking-phrase` node, pair its inbound and outbound connections. An n-ary link (one phrase, several targets) legitimately produces several triples.
- Geometry (`*-appearance-list`) is cleanly separated from semantics (`concept-list`, `linking-phrase-list`, `connection-list`). **A hash computed over only the semantic elements is stable across cosmetic re-layout.** That is exactly what a versioned, hashed artifact needs.
- The element inventory per IHMC's CXL documentation (`cmap.ihmc.us/xml/cxl.html`, retrieved): `concept-list`, `linking-phrase-list`, `connection-list`, `resource-group-list`, `proposition-list`, the four `*-appearance-list` elements, and `style-sheet-list`. Attributes include `id`, `label`, `parent-id`, `from-id`, `to-id`, plus geometry and styling. Dublin Core and vCard namespaces carry authorship.
- The `proposition-list` element means CXL can carry the *derived* triples alongside the graph — you do not have to recompute the join at read time.

**License status: not stated.** The IHMC documentation calls CXL "publicly available" and says "the open data format makes it possible for any client or server application to read and write the full textual, graphical, and relational content of Cmaps," but I found no explicit license grant on the schema or the docs page. **For NAADAP this is a low risk** — you would be writing your own C# reader/writer against a published XML schema, which is a format, not code — but it should be noted in the dependency review. CmapTools itself is proprietary IHMC freeware, not open source; NAADAP should not ship it in the container.

### 2.3 Concept map → OWL: the COE work

> Hayes, P., Eskridge, T. C., Saavedra, R., Reichherzer, T., Mehrotra, M., & Bobrovnikoff, D. (2005). Collaborative knowledge capture in ontologies. In *Proceedings of the 3rd International Conference on Knowledge Capture (K-CAP '05)*, pp. 99–106. ACM. DOI: `10.1145/1088622.1088641` — **[VERIFIED+FT]** (Crossref confirms all six authors, pages 99–106, ACM, Oct 2005; 8-page PDF retrieved and read.)

This paper is the honest account of the gap between a concept map and a machine-reasonable artifact. From the paper itself:

> "the concept maps built using these methodologies and tools are **'informal' representations that are meant to communicate knowledge between people, and are not sufficiently formally specified to be used by automated reasoners.**"

COE (Collaborative Ontology Environment) closes that gap:

> "COE can display any OWL or RDF ontology as a readable concept map, supports intuitive editing and construction of these 'ontology maps', allows users to rapidly locate related concepts in published SW ontologies, and **outputs legal OWL/RDF/XML.** The goal is to enable rapid and intuitive capture of machine interpretable knowledge…"

**The lesson for NAADAP, stated plainly:** a raw concept map from an SME is *not* a knowledge base. The linking phrases are free text ("have", "usually means", "can sometimes absorb"). To get something a deterministic scorer can use, the linking phrases must be constrained to a controlled vocabulary at elicitation time — i.e., you give the SME a fixed palette of relation labels. COE's approach is to do that by anchoring to published ontologies; NAADAP's equivalent is to define ~10–20 relation types up front (`is-scope-of`, `excludes`, `requires-clause`, `triggers-consolidation-review`, `has-NAICS`, …) and have the elicitation instrument refuse anything outside the palette.

### 2.4 Concept mapping as expert-knowledge *preservation* — the flagship case

> Hoffman, R. R., Coffey, J. W., Ford, K. M., & Novak, J. D. (2006). A Method for Eliciting, Preserving, and Sharing the Knowledge of Forecasters. *Weather and Forecasting*, 21(3), 416–428. DOI: `10.1175/waf927.1` — **[VERIFIED]** (Crossref: American Meteorological Society, 1 June 2006, vol 21 iss 3 pp 416–428, all four authors.)

This is the closest published analogue to NAADAP's problem: Navy forecasters and aerographers at NAS Whiting Field, Pensacola, whose *local* forecasting expertise was undocumented and walking out the door. The output was **STORM-LK** (System To Organize Representations in Meteorology — Local Knowledge), a navigable concept-map knowledge model.

**I could not retrieve the full text** (AMS returns 403), so I state no figures from it — not hours, not map counts, not proposition counts. What is verified is the title, venue, and that the method is "eliciting, preserving, and sharing."

Supporting, verified:
> Hoffman, R. R., Coffey, J. W., Ford, K. M., & Carnot, M. J. (2001). Storm-LK: A Human-Centered Knowledge Model for Weather Forecasting. *Proceedings of the Human Factors and Ergonomics Society Annual Meeting*, 45(8), 752. DOI: `10.1177/154193120104500807` — **[VERIFIED]**

> Coffey, J. W., Hoffman, R., & Cañas, A. (2006). Concept Map-Based Knowledge Modeling: Perspectives from Information and Knowledge Visualization. *Information Visualization*, 5(3), 192–201. DOI: `10.1057/palgrave.ivs.9500129` — **[VERIFIED]**

> Moon, B., Hoffman, R. R., Novak, J. D., & Cañas, A. J. (Eds.) (2011). *Applied Concept Mapping: Capturing, Analyzing, and Organizing Knowledge*. Boca Raton, FL: CRC Press. DOI: `10.1201/b10716` — **[VERIFIED]** (Crossref: CRC Press, 7 Feb 2011.)

### 2.5 Efficiency

Shadbolt & Smart (2015), **[VERIFIED+FT]**:

> "Concept mapping has been reported to be a very efficient knowledge elicitation technique with the technique **yielding an average of two useful propositions per session minute** (Hoffman et al., 2001)."

Take that at face value and a **90-minute session yields roughly 180 propositions**. Ten SMEs at 90 minutes each is ~1,800 raw propositions before deduplication — comfortably enough to build a first-pass domain model, and small enough that the merged artifact fits in a few hundred kilobytes of XML.

Note the chain: the *2 propositions/minute* figure is quoted by a verified full-text source citing Hoffman et al. 2001 (whose citation I independently verified). I did not read Hoffman et al. 2001 itself. **[VERIFIED via citing source]**

---

## 3. Ontology Engineering Methodologies

### 3.1 Competency questions — Grüninger & Fox (the mechanism, from the primary source)

> Grüninger, M., & Fox, M. S. (1995). Methodology for the Design and Evaluation of Ontologies. *Workshop on Basic Ontological Issues in Knowledge Sharing, IJCAI-95*, Montreal. — **[VERIFIED+FT]** Retrieved from the authors' own lab: `http://www.eil.utoronto.ca/wp-content/uploads/enterprise-modelling/papers/gruninger-ijcai95.pdf` (HTTP 200, 10 pages). The PDF is a **scan**; page 1 carries a machine-readable header giving the citation string verbatim, and I OCR'd all 10 pages with Tesseract to read the content. Everything quoted below is from that OCR.

> Companion paper: Grüninger, M., & Fox, M. S. (1995). The Role of Competency Questions in Enterprise Engineering. In *Benchmarking — Theory and Practice* (IFIP AICT), pp. 22–31. Springer US. DOI: `10.1007/978-0-387-34847-6_3` — **[VERIFIED+FT]** (Crossref confirms; the pre-print PDF at `eil.utoronto.ca/.../benchIFIP94.pdf` retrieved, 17 pp., and its own header confirms it was submitted to the IFIP WG5.7 Workshop on Benchmarking, Trondheim, 1994.)

**The six-step procedure, quoted from the paper's introduction:**

> "Our approach to engineering ontologies begins with using these problems to **define an ontology's requirements in the form of questions that an ontology must be able to answer. We call this the competency of the ontology.** The second step is to **define the terminology of the ontology** — its objects, attributes, and relations… The third step is to **specify the definitions and constraints on the terminology**… The specifications are represented in First Order Logic and implemented in Prolog. Lastly, we **test the competency of the ontology by proving completeness theorems** with respect to the competency questions."

Expanded, the paper's own section structure gives six stages:

**Stage 1 — Motivating Scenario.**
> "The motivating scenarios often have the form of story problems or examples which are not adequately addressed by existing ontologies. A motivating scenario also provides a set of intuitively possible solutions to the scenario problems. These solutions provide a first idea of the informal intended semantics for the objects and relations."

Requirement: "**Any proposal for a new ontology or extension to an ontology must describe the motivating scenario, and the set of intended solutions.**"

**Stage 2 — Informal Competency Questions.** Natural-language questions the ontology must answer. Three rules stated in the paper, all mechanically checkable:

1. *Justification rule:* "for every object, attribute, relation, and axiom in the proposed ontology… **there must first be an informal competency question** … which intuitively requires the objects or constraints defined with the object." — i.e., **every term must trace to a question. Untraceable terms are deleted.**
2. *Stratification rule:* "the competency questions should be defined in a stratified manner, with higher level questions requiring the solution of lower level questions."
3. *Non-triviality rule:* "**It is not a well-designed ontology if all competency questions have the form of simple lookup queries**; there should be questions that use the solutions to such simple queries."

And, crucially for what CQs are *for*: "These competency questions **do not generate ontological commitments; rather, they are used to evaluate** the ontological commitments that have been made."

Their worked examples (Activity ontology) show the genre — temporal projection, planning and scheduling, execution monitoring, time-based competition. The Organisation ontology examples are startlingly close to NAADAP's domain:

> "What activities must a particular agent/position/role perform?"
> "**In order to perform a particular activity, whose permission is needed?**"
> "Is an agent allowed to perform an activity in some situation?"
> "**What authority constraints are necessary among a set of agents in order to achieve a goal?**"
> "What goals are solitarily unachievable for a given agent?"

Those are, almost word for word, the questions a contracting officer asks about warrant authority and delegation.

**Stage 3 — Terminology in first-order logic.** "The first step in specifying the terminology … is to identify the objects in the domain of discourse. These will be represented by constants and variables… Attributes of objects are then defined by unary predicates; relations among objects are defined using n-ary predicates." Requirement: "This language must provide the necessary terminology to **restate the informal competency questions.**"

**Stage 4 — Formal Competency Questions.** This is the mechanically important step. The paper:

> "the competency questions are defined formally as **an entailment or consistency problem** with respect to the axioms in the ontology. Thus, they will have one of the following forms, where T_ontology is the set of axioms in the proposed ontology, T_ground is a set of ground literals (instances), and Q is a first-order sentence using only predicates in the language of T_ontology:
> - Determine T_ontology ∪ T_ground ⊨ Q
> - Determine whether T_ontology ∪ T_ground ⊭ ¬Q; that is, determine if Q is consistent with T_ontology ∪ T_ground"

And the closure condition: "**all terms in the statement of the formal competency questions must be included in the terminology of the ontology.**"

**Stage 5 — Axioms.** "the axioms in the ontology **must be necessary and sufficient** to express the competency questions and to characterize their solutions; without the axioms we cannot express the question or its solution, and with the axioms we can express the question and its solutions." If insufficient, "additional objects or axioms must be added … until it is sufficient. This development of axioms … is therefore **an iterative process**." And the warning: "Simply proposing a set of objects alone, or proposing a set of ground terms in first-order logic, does not constitute an ontology."

**Stage 6 — Completeness Theorems.** Four admissible forms, quoted:
- `T_ontology ∪ T_ground ⊨ Θ` **iff** `T_ontology ∪ T_ground ⊨ Q`
- `T_ontology ∪ T_ground ⊨ Θ` **iff** `T_ontology ∪ T_ground ∪ Q` is consistent
- `T_ontology ∪ T_ground ∪ Θ ⊨ Q` **or** `T_ontology ∪ T_ground ∪ Θ ⊨ ¬Q`
- All models of `T_ontology ∪ T_ground` agree on the extension of some predicate `P`

Where `Θ` is "a set of first-order sentences defining the set of conditions under which the solutions to the problem are complete."

And the regression-test property, which is what NAADAP actually wants:
> "Completeness theorems can also provide a means of determining the **extendability** of an ontology, by making explicit the role that each axiom plays in proving the theorem. **Any extension to the ontology must be able to preserve the completeness theorems.**"

**Translation to NAADAP's engineering reality.** You should take Stages 1, 2, 3 and a weakened Stage 4, and *not* Stages 5–6.

- Stages 1–3 give you: motivating scenarios, a numbered CQ register, and a controlled terminology in which every term traces to a CQ. All cheap, all deterministic, all human-auditable.
- Stage 4 gives you the key move: **each CQ becomes an executable query.** You do not need first-order entailment to get this benefit. A CQ like *"Which vehicles have a NAICS code matching this requirement and a ceiling above the estimated value?"* becomes a deterministic C# query over the knowledge artifact, with an expected-result fixture. That is a unit test.
- Stages 5–6 require a theorem prover. **Reject them** for NAADAP: a description-logic or FOL reasoner inside 1 CPU / 2 GB / 30 minutes with a hard determinism requirement is a bad trade, and it buys you nothing a contracting officer would find more defensible than a table lookup with a citation.

### 3.2 METHONTOLOGY

> Fernández[-López], M., Gómez-Pérez, A., & Juristo, N. (1997). METHONTOLOGY: From Ontological Art Towards Ontological Engineering. In *Proceedings of the AAAI-97 Spring Symposium Series on Ontological Engineering* (SS-97-06), Stanford University, 24–26 March 1997, pp. 33–40. — **[VERIFIED+FT]** (Crossref has no record — AAAI symposium papers of that era are frequently not deposited. Verified instead by retrieving the full PDF from the authors' institutional repository, Archivo Digital UPM: `https://oa.upm.es/5484/1/METHONTOLOGY_.pdf`, 8 pages, and by the AAAI proceedings listing at `aaai.org/papers/0005-ss97-06-005-...`. The retrieved PDF's title page confirms authors and the Universidad Politécnica de Madrid affiliation.)

**The life cycle** (quoted from the retrieved paper): the ontology development process passes through "**specification, conceptualization, formalization, integration, implementation and maintenance.**" Three activities run across the *whole* life: "Knowledge Acquisition, evaluation of ontologies and documentation are tasks that are carried out during the whole life of the [ontology]." Evaluation effort "[is greatest during the] specification phase and decreases as the ontology development process moves forward. **In order to prevent error propagation, most of evaluation should be done during the earliest stages.**"

**The specification document.** METHONTOLOGY requires at minimum:
> "(a) The purpose of the ontology, including its intended uses, scenarios of use, end-users… (b) Level of formality of the implemented ontology…"
and notes the two specification styles explicitly, contrasting Uschold's Enterprise ontology (natural language) with "the TOVE ontology using a set of competence questions (Uschold & Gruninger 1996)."

**Knowledge acquisition techniques it prescribes** (verbatim list from the paper):
- "**Informal text analysis**, to study the main concepts given in books and handbooks."
- "**Formal text analysis.** The first thing to do is to identify the structures to be detected (definition, affirmation, etc.) and the kind of knowledge contributed by each one (concepts, attributes, values, and relationships)."
- "**Structured interviews with experts** to get specific and detailed knowledge about concepts, their properties and their relationships, to evaluate the conceptual model once the conceptualization activity has been finished, and to evaluate implementation."

**Conceptualization — the intermediate representations.** This is METHONTOLOGY's real contribution and it is directly usable by NAADAP. Quoted:

- "**Glossary of Terms (GT)**… Terms include concepts, instances, verbs and properties. So, the GT identifies and gathers all the useful and potentially usable domain knowledge and its meanings."
- "for each set of related concepts and related verbs, a **concepts classification tree** and a **verbs diagram** is built."
- Concepts are described using: "**Data Dictionary**, which describes and gathers all the useful and potentially usable domain concepts, their meanings, attributes, instances, etc.; **tables of instance attributes**…; **tables of class attributes**, to describe the concept itself, not its instances; **tables of constants**, used to specify information related to the domain of knowledge that always take the same value; **tables of instances**…; and **attributes classification trees**, to graphically display attributes and constants related in the inference sequence of the root attributes, as well as the sequence of formulas or rules to be executed to infer such attributes."
- Verbs get a "**Verbs Dictionary**."

Every one of these is a **table**. METHONTOLOGY's insight, which predates and outlives its own formalization layer, is that the intermediate representations are tabular and therefore tool-independent — you can fill them in a spreadsheet and they remain machine-readable. That is exactly what NAADAP needs to ship inside a container.

**Applicability: high, for the conceptualization tables specifically.** Ignore the formalization/implementation phases (they target frame systems and description logics of the late 1990s).

### 3.3 On-To-Knowledge

> Sure, Y., Staab, S., & Studer, R. (2004). On-To-Knowledge Methodology (OTKM). In S. Staab & R. Studer (Eds.), *Handbook on Ontologies*, pp. 117–132. Springer. DOI: `10.1007/978-3-540-24750-0_6` — **[VERIFIED]**
> Sure, Y., Staab, S., & Studer, R. (2009). Ontology Engineering Methodology. In *Handbook on Ontologies* (2nd ed.), pp. 135–152. Springer. DOI: `10.1007/978-3-540-92673-3_6` — **[VERIFIED]**
> Sure, Y., Staab, S., & Studer, R. (2002). Methodology for development and employment of ontology based knowledge management applications. *ACM SIGMOD Record*, 31(4), 18–23. DOI: `10.1145/637411.637414` — **[VERIFIED]**

**[Mechanism: PARTIAL]** — I verified all three citations but retrieved none of the full texts (all Springer/ACM paywalled). OTKM is universally described in the secondary literature as a five-phase process (feasibility study → kickoff → refinement → evaluation → application & evolution) oriented specifically at *knowledge-management applications* rather than ontologies as standalone artifacts, with a "kickoff" phase producing an Ontology Requirements Specification Document. NeOn's own state-of-the-art section (which I did retrieve, §3.4) lists OTKM alongside METHONTOLOGY and DILIGENT as the three methodologies that "have gone a step forward by having transformed the art of constructing single ontologies into an engineering activity." **I have not independently confirmed the five phase names from a primary source and NAADAP should not cite them as if I had.**

### 3.4 NeOn

> Suárez-Figueroa, M. C., Gómez-Pérez, A., & Fernández-López, M. (2011). The NeOn Methodology for Ontology Engineering. In *Ontology Engineering in a Networked World*, pp. 9–34. Springer Berlin Heidelberg. DOI: `10.1007/978-3-642-24794-1_2` — **[VERIFIED]**
> Suárez-Figueroa, M. C., & Gómez-Pérez, A. (2011). Ontology Requirements Specification. In *Ontology Engineering in a Networked World*, pp. 93–106. Springer. DOI: `10.1007/978-3-642-24794-1_5` — **[VERIFIED]**
> Suárez-Figueroa, M. C. *NeOn Methodology for Building Ontology Networks: Specification, Scheduling and Reuse.* PhD dissertation, Universidad Politécnica de Madrid. DOI: `10.20868/upm.thesis.3879` — **[VERIFIED]**
> Gómez-Pérez, A., & Suárez-Figueroa, M. C. (2009). NeOn Methodology for Building Ontology Networks: a Scenario-based Methodology. — **[VERIFIED+FT]** (8-page PDF retrieved from `https://oa.upm.es/5475/1/INVE_MEM_2009_64399.pdf`.)

From the retrieved paper, NeOn's key departure: it does not assume you build from scratch. It defines **nine scenarios**, of which Scenario 1 is "From specification to implementation" and Scenarios 2–3 are reuse/re-engineering of non-ontological and ontological resources respectively.

**Scenario 1, quoted in full because it is the template NAADAP should follow:**

> "Ontology developers develop the ontology network from scratch… The ontology development should start with the knowledge acquisition activity, which should be carried out during the whole development. Simultaneously with the knowledge acquisition, ontology developers should specify the requirements that the ontology should fulfill, by means of the ontology requirements specification activity. The objective of this activity is to output the **ontology requirements specification document (ORSD) that includes the purpose, the scope and the implementation language of the ontology network, target group and intended uses of the ontology network, and the set of requirements the ontology network should fulfill, mainly in the form of competency questions (CQs).** After such an activity, it is advisory to carry out a quick search for knowledge resources **using as input the terms appearing in the ORSD.**"

Note what that last sentence does: the ORSD's term list becomes the *search key* for finding reusable resources. For NAADAP, the analogous move is that the CQ-derived term list becomes the seed vocabulary for the document-clustering feature extractor. The elicitation output and the algorithm's lexicon are the same object.

Scenario 2 is also directly on point: "Ontology developers should carry out the non-ontological resource reuse process for deciding, **according to the requirements in the ORSD**, which existing NORs can be reused" — NORs being "ontologies, thesauri, lexicons, and classification schemas." NAADAP's domain is full of these: NAICS, PSC, the FPDS-NG taxonomy, the FAR/DFARS part structure, the PGIL. The ORSD is the instrument that decides which of them you import rather than re-elicit.

The ORSD's three stated uses, from NeOn: (1) find and reuse knowledge-aware resources for re-engineering; (2) find and reuse ontological resources; (3) **verification of the ontology along the ontology development**. **[PARTIAL for the exact ORSD template fields beyond those listed in the retrieved paper]** — the full field-by-field template lives in the paywalled book chapter (`10.1007/978-3-642-24794-1_5`), which I did not read.

Related, verified but not read:
> Suárez-Figueroa, M. C., Gómez-Pérez, A., & Villazón-Terrazas, B. (2009). How to Write and Use the Ontology Requirements Specification Document. Springer. DOI: `10.1007/978-3-642-05151-7_16` — **[VERIFIED]**

### 3.5 Making competency questions executable — modern tooling

The line from Grüninger & Fox to running code is CQ → SPARQL-OWL query → pass/fail.

> Potoniec, J., Wiśniewski, D., Ławrynowicz, A., & Keet, C. M. (2020). Dataset of ontology competency questions to SPARQL-OWL queries translations. *Data in Brief*, 29, 105098. DOI: `10.1016/j.dib.2019.105098` — **[VERIFIED]**
> Wiśniewski, D., Potoniec, J., Ławrynowicz, A., & Keet, C. M. (2019). Analysis of Ontology Competency Questions and their Formalisations in SPARQL-OWL. DOI: `10.2139/ssrn.3490821` (SSRN preprint of the *Journal of Web Semantics* paper) — **[VERIFIED]**
> Wiśniewski, D., Potoniec, J., & Ławrynowicz, A. (2022). BigCQ: Generating a Synthetic Set of Competency Questions Formalized into SPARQL-OWL (Student Abstract). *Proceedings of the AAAI Conference on Artificial Intelligence*, 36(11), 13079–13080. DOI: `10.1609/aaai.v36i11.21676` — **[VERIFIED]**
> Ren, Y., Parvizi, A., Mellish, C., Pan, J. Z., van Deemter, K., & Stevens, R. (2014). Towards Competency Question-Driven Ontology Authoring. In *The Semantic Web: Trends and Challenges (ESWC 2014)*, LNCS, pp. 752–767. Springer. DOI: `10.1007/978-3-319-07443-6_50` — **[VERIFIED]**

**Implementations (repo, license, language — all confirmed by fetching the repository page):**

| Repo | Language | License | What it does |
|---|---|---|---|
| `dwisniewski/SeeQuery` | Python | **MIT** | Translates competency questions into SPARQL-OWL queries to track ontology quality during authoring. Template-based, using mappings from the CQ2SPARQLOWL and BigCQ datasets. Reported 78.5% accuracy on the Pizza ontology, 65% on TRHOnto. 12 commits — a research prototype, not production code. |
| `oeg-upm/Themis` | Java | **Apache-2.0** | Web app + REST API + CLI JAR that executes test suites against one or more ontologies and emits verification results. Test definitions in RDF, RDFa, or a custom text syntax, aligned to the VTC ontology. Results downloadable as **JSON, RDFa, or JUnit**. 67 commits. |
| `CQ2SPARQLOWL/Dataset` | data | (see repo) | The hand-built CQ→SPARQL-OWL translation corpus behind the *Data in Brief* paper. |
| `dwisniewski/BigCQ` | data | (see repo) | Large synthetic CQ-pattern → SPARQL-OWL template dataset. |

**The Themis JUnit output format is the practical takeaway.** It means "does the knowledge artifact still answer CQ #17?" becomes a CI test result in a format any build system already understands. NAADAP does not need Themis itself (Java, and it wants a web service); it needs the *pattern* — CQs as a test suite emitting JUnit XML — reimplemented in about 200 lines of C#.

**Related standard for structural validation:**
> W3C (2017). *Shapes Constraint Language (SHACL)*. W3C Recommendation, 20 July 2017. Editors: Holger Knublauch (TopQuadrant) and Dimitris Kontokostas (University of Leipzig). URL: `https://www.w3.org/TR/shacl/` — **[VERIFIED+FT]** (fetched; status and date confirmed on the document itself.)

SHACL validates an RDF graph against shapes and emits a **validation report**. If NAADAP serializes its elicited knowledge as RDF/Turtle, SHACL gives structural validation (every vehicle must have a ceiling; every consolidation rule must cite a FAR reference) as a declarative, deterministic, offline check. **Caution:** the mature SHACL engines are Java and Python; the .NET story (dotNetRDF has partial SHACL support) is weaker. Evaluate before committing.

### 3.6 Supporting ontology-engineering references (verified, not central)

> Schreiber, G., Akkermans, H., Anjewierden, A., de Hoog, R., Shadbolt, N. R., Van de Velde, W., & Wielinga, B. J. (1999). *Knowledge Engineering and Management: The CommonKADS Methodology*. Cambridge, MA: The MIT Press. DOI: `10.7551/mitpress/4073.001.0001` — **[VERIFIED]** (Crossref confirms all seven authors and MIT Press, Dec 1999.) CommonKADS is the standard model-based knowledge-engineering methodology; its "model suite" (organization, task, agent, knowledge, communication, design models) is the canonical way to separate *what the expert knows* from *how the system will use it*. **[Mechanism: not independently verified — full text not retrieved.]**

> Guarino, N., & Welty, C. A. (2009). An Overview of OntoClean. In *Handbook on Ontologies* (2nd ed.), pp. 201–220. Springer. DOI: `10.1007/978-3-540-92673-3_9` — **[VERIFIED]** (also 2004 1st ed., pp. 151–171, DOI `10.1007/978-3-540-24750-0_8`). A formal method for finding *taxonomic* errors (misuse of subsumption) using rigidity, identity, and unity meta-properties. Relevant if NAADAP builds a vehicle/scope taxonomy — "SeaPort-NxG is-a contract vehicle" and "this task order is-a SeaPort-NxG" are different relations and OntoClean is the discipline that catches conflating them.

> Noy, N. F., & McGuinness, D. L. (2001). *Ontology Development 101: A Guide to Creating Your First Ontology*. Stanford Knowledge Systems Laboratory Technical Report KSL-01-05 / Stanford Medical Informatics Technical Report SMI-2001-0880. — **[PARTIAL]** — no Crossref DOI (it is a tech report); the PDF is served from `protege.stanford.edu/publications/ontology_development/ontology101.pdf` and appeared in search results, but I did not fetch and read it. Widely used as a practical primer; **cite only after retrieving it.**

---

## 4. The Knowledge Acquisition Bottleneck — and Which Critiques Still Bite

### 4.1 Feigenbaum's original statement (primary source, retrieved)

> Feigenbaum, E. A. (1977). The Art of Artificial Intelligence: 1. Themes and Case Studies of Knowledge Engineering. In *Proceedings of the 5th International Joint Conference on Artificial Intelligence (IJCAI-77)*, Cambridge, MA, pp. 1014–1029. — **[VERIFIED+FT]** (16-page PDF retrieved from the IJCAI open proceedings archive: `https://www.ijcai.org/Proceedings/77-2/Papers/092.pdf`. Also indexed as Stanford CS report via DTIC, DOI `10.21236/ada046289` — **[VERIFIED]** via Crossref.)

The canonical sentence, quoted from the retrieved text (p. 1020, §3.2.1, on the origins of META-DENDRAL):

> "…by a recognition that **the acquisition of domain knowledge was the bottleneck problem in the building of applications-oriented intelligent agents.**"

Note precisely what Feigenbaum said: the bottleneck is **acquisition of domain knowledge**, and his proposed remedy in that same paragraph was *automated* acquisition (META-DENDRAL inferring mass-spectrometry fragmentation rules from data). The bottleneck framing and the machine-learning response were born together, in the same sentence.

A second passage from the same paper is more useful to NAADAP than the famous one:

> "the **explanation capability is needed as part of the concerted attack on the knowledge acquisition problem.** Explanation of the reasoning process is central to the interactive transfer of expertise to the knowledge base, and **it is our most powerful tool for the debugging of the knowledge base.**"

And on why explanation is not optional:

> "The issue is not academic or philosophical. It is **an engineering issue that has arisen in medical and military applications of intelligent agents**, and will govern future acceptance of AI work in applications areas."

**This is the single most transferable finding in this section.** Feigenbaum in 1977 identified, for military applications specifically, that explanation is (a) the acceptance criterion and (b) the debugging instrument for the knowledge base itself. NAADAP's "every recommendation must carry citable evidence a contracting officer could defend" requirement is not a compliance burden bolted on top of the system — it is, per the primary literature, *the mechanism by which the knowledge base gets fixed.* Build the evidence trail first and the elicitation gets cheaper, because SMEs can review and correct the artifact by reading its explanations rather than by being re-interviewed.

### 4.2 The term "bottleneck" in the expert-systems canon

> Hayes-Roth, F., Waterman, D. A., & Lenat, D. B. (Eds.) (1983). *Building Expert Systems*. Reading, MA: Addison-Wesley. — **[VERIFIED indirectly]** No Crossref record for the book itself (1983 predates routine DOI assignment for monographs); verified via two independent contemporaneous reviews with DOIs: Self, J. (1984), *Robotica* 2(2), 119, DOI `10.1017/s0263574700002022`; and Owen, T. (1988), *Robotica* 6(2), 165, DOI `10.1017/s0263574700004069`. Both confirm editors, publisher, and 1983. Shadbolt & Smart (2015) **[VERIFIED+FT]** attribute the phrase "**knowledge acquisition bottleneck**" to this volume by name.

Shadbolt & Smart's account of what actually went wrong, quoted:

> "Knowledge engineers soon discovered, however, that **acquiring sufficient high-quality knowledge from individuals to build a robust and useful system was a very time-consuming and expensive activity. It seemed to take longer to elicit knowledge from** [experts than to build the system]…"

> Buchanan, B. G., & Shortliffe, E. H. (Eds.) (1984). *Rule-Based Expert Systems: The MYCIN Experiments of the Stanford Heuristic Programming Project*. Reading, MA: Addison-Wesley. — **[VERIFIED indirectly]** via contemporaneous review: Munro, J. (1984), *Civil Engineering Systems* 1(6), 342–343, DOI `10.1080/02630258408970370`, which confirms editors, publisher, 1984, and 748 pages.

### 4.3 Clancey's critique — the one that still applies

> Clancey, W. J. (1983). The epistemology of a rule-based expert system — a framework for explanation. *Artificial Intelligence*, 20(3), 215–251. DOI: `10.1016/0004-3702(83)90008-5` — **[VERIFIED]** (Crossref: Elsevier, May 1983, vol 20 iss 3 pp 215–251.) Follow-up: Clancey, W. J. (1994). Notes on "Epistemology of a rule-based expert system." In *Artificial Intelligence in Perspective*, pp. 197–204, MIT Press, DOI `10.7551/mitpress/1413.003.0029` — **[VERIFIED]**.

**[Mechanism: PARTIAL]** — I verified the citations but did not retrieve either full text. Clancey's argument, as it is universally reported and as the title states, is that MYCIN's rules **conflated several distinct kinds of knowledge into a single uniform representation**: domain facts, causal/structural models, diagnostic strategy, and heuristic shortcuts were all flattened into `IF…THEN` with certainty factors. The consequence was that the system could not explain *why* a rule was appropriate — only *that* it fired — and the knowledge base could not be maintained because the implicit strategy was invisible.

**Why it still bites, and why NAADAP should care.** The modern equivalent of "flattening everything into rules with certainty factors" is **flattening everything into a single similarity score**. If NAADAP's vehicle recommendation emerges from one blended number, it has made precisely Clancey's error: statutory constraints (a hard NAICS/size-standard bar), policy preferences (category management tiers), empirical similarity (shared requirement language), and heuristic shortcuts ("this shop always uses SeaPort") will be indistinguishable in the output, and no contracting officer will be able to defend it. **The design implication is to keep the knowledge layers separate and typed** — hard eliminations, soft preferences, and evidence-based similarity as three distinct scored channels with separate provenance — so that the explanation can say *which kind* of reason produced the recommendation.

### 4.4 The validity problem — and the most uncomfortable finding in this review

> Burton, A. M., Shadbolt, N. R., Rugg, G., & Hedgecock, A. P. (1990). The efficacy of knowledge elicitation techniques: a comparison across domains and levels of expertise. *Knowledge Acquisition*, 2(2), 167–178. DOI: `10.1016/S1042-8143(05)80010-X` — **[VERIFIED]** (Crossref confirms all four authors, Elsevier, June 1990, vol 2 iss 2 pp 167–178.)

**[PARTIAL — findings from the publisher abstract via search summary; both the Southampton eprints copy and ScienceDirect returned HTTP 403 to direct retrieval.]** The reported design and findings:

- Compared **four** techniques: structured interview, protocol analysis, card sort, laddered grid.
- Across **two classification domains**, using **eight experts in each**.
- "Despite its common usage, **protocol analysis is shown to be the least efficient technique.**"
- And the finding that should worry anyone building a knowledge-capture system: a study in which "**non-experts are subjected to 'knowledge elicitation', and subjects entirely ignorant of a domain are able to construct plausible knowledge bases from common sense alone.**"

**Read that last point carefully.** It means *plausibility of the elicited artifact is not evidence that the elicitation worked.* A concept map full of reasonable-looking propositions about contract vehicles can be produced by someone who knows nothing. **The consequence for NAADAP is a hard requirement: the knowledge artifact must be validated against outcomes, not against reviewer approval.** Concretely — hold out a set of historical consolidations with known vehicle assignments, and test whether the elicited knowledge reproduces them. If the artifact cannot beat a domain-naive baseline on held-out ground truth, the elicitation has produced plausible fiction.

I flag this as **[PARTIAL]** and recommend obtaining the paper before quoting it in a deliverable, but the finding is consistent enough across secondary reports that I would design around it regardless.

Shadbolt & Smart **[VERIFIED+FT]** corroborate the efficiency direction and add a related warning: "**an expert's own opinion of the worth of a technique is no guide as to its actual value** (Schweikert et al., 1987)," and "contrived techniques can sometimes prove more efficient than their non-contrived counterparts (Burton et al., 1990)."

### 4.5 The differential access hypothesis

> Hoffman, R. R., Shadbolt, N. R., Burton, A. M., & Klein, G. (1995). Eliciting Knowledge from Experts: A Methodological Analysis. *Organizational Behavior and Human Decision Processes*, 62(2), 129–158. DOI: `10.1006/obhd.1995.1039` — **[VERIFIED]** (Crossref: all four authors, Elsevier, May 1995, vol 62 iss 2 pp 129–158.) Full text not retrieved (Southampton eprints 403).

The hypothesis, as stated by Shadbolt & Smart **[VERIFIED+FT]**:

> "different knowledge elicitation techniques **differentially support the elicitation of particular kinds of information**. This is commonly known as the differential access hypothesis (Hoffman et al., 1995)."

with the worked example that protocol analysis yields "the 'when' and 'how' of using specific knowledge… problem solving and reasoning strategies, evaluation procedures and evaluation criteria… and procedural knowledge about how tasks and sub-tasks are decomposed," but "is intrinsically a narrow method since it can only be used to analyze a relatively small number of problems within the domain."

**Implication for NAADAP: you cannot use one instrument.** A single-method capture will systematically miss whole categories of what the retiring expert knows. The minimum viable programme is two complementary instruments — one conceptual/structural (concept mapping or laddering, for the taxonomy of vehicles and scopes) and one episodic/procedural (CDM or ACTA, for the judgment calls) — plus the documentary sources METHONTOLOGY calls formal text analysis.

### 4.6 What the elicitation literature says about the framing itself

Shadbolt & Smart make an epistemological correction that NAADAP's framing should absorb (footnote 1 of the retrieved chapter):

> "although early conceptualizations of knowledge elicitation cast the process as one of **extracting or mining knowledge from the heads of experts**, more recent conceptualizations view the process as **a modelling exercise**. The idea is that the knowledge elicitor and domain expert work together in order to create **a model of an expert's knowledge. This model may reflect reality to a greater or lesser extent.**"

This is not hedging; it is a design constraint. The artifact NAADAP ships is a *model*, with a fidelity that is an empirical question. It must therefore be versioned, dated, attributed to named SMEs, and testable — which is exactly the competency-question apparatus from §3.1.

### 4.7 Critiques that transfer to modern ML-based capture

Three of the above transfer directly, and NAADAP should treat them as design rules rather than history:

1. **Feigenbaum's explanation requirement.** An LLM-extracted knowledge base that cannot point at the source sentence for each assertion fails the 1977 criterion, let alone a 2026 legal determination. NAADAP's no-LLM-on-the-core-path constraint is, from this angle, a feature: a deterministic extractor's output is inspectable by construction.
2. **Clancey's conflation critique.** Modern embedding-based similarity is the purest possible form of the flattening Clancey diagnosed: every kind of relatedness collapses into one cosine. This is precisely why NAADAP's evidence requirement and its no-LLM core are coherent with each other rather than in tension.
3. **Burton et al.'s plausibility finding.** An LLM will generate a fluent, plausible, well-formatted knowledge base about contract vehicles with no expert involvement whatsoever. That is the *failure mode of the 1990 non-expert study, industrialized.* The only defence is held-out outcome validation.

Listed for completeness, **not relied upon**:
> Gaines, B. R. (2013). Knowledge acquisition: Past, present and future. *International Journal of Human-Computer Studies*, 71(2), 135–156. DOI: `10.1016/j.ijhcs.2012.10.010` — **[VERIFIED citation; FULL TEXT UNAVAILABLE]**. Both the author's archived copy at `pages.cpsc.ucalgary.ca/~gaines/reports/MFIT/IJHCSKA/IJHCSKA.pdf` and the UVic mirror return HTTP 404. This is the obvious retrospective source on the bottleneck and I would expect it to be the best single citation for §4 — **NAADAP should obtain it through a library.** I make no claim about its contents.

---

## 5. Repertory Grid Technique / Personal Construct Psychology

**Yes, it has a real mechanical procedure.** It is one of the few elicitation techniques with a fully specified algorithm on both the elicitation and analysis sides. Whether NAADAP should use it is a separate question (§5.5).

### 5.1 Foundations and key papers

> Kelly, G. A. (1955). *The Psychology of Personal Constructs.* New York: Norton. — **[UNVERIFIED]** — no DOI record retrieved; cited consistently in every verified source below (Shadbolt & Smart 2015 cite it as "Kelly, 1955"). Standard and uncontroversial, but I did not verify it directly.

> Gaines, B. R., & Shaw, M. L. G. (1993). Knowledge acquisition tools based on personal construct psychology. *The Knowledge Engineering Review*, 8(1), 49–85. DOI: `10.1017/S0269888900000060` — **[VERIFIED]** with abstract retrieved from Crossref:
> "Personal construct psychology is a theory of individual and group psychological and social processes that **has been used extensively in knowledge acquisition research to model the cognitive processes of human experts.** The psychology takes a constructivist position appropriate to the modelling of human knowledge processes…"

> Shaw, M. L. G., & Gaines, B. R. (1989). Comparing conceptual structures: consensus, conflict, correspondence and contrast. *Knowledge Acquisition*, 1(4), 341–363. DOI: `10.1016/S1042-8143(89)80010-X` — **[VERIFIED]** (Crossref: both authors, Elsevier, Dec 1989, vol 1 iss 4 pp 341–363.)

> Shaw, M. L. G., & Gaines, B. R. (1993). Personal construct psychology foundations for knowledge acquisition and representation. In *Knowledge Acquisition for Knowledge-Based Systems* (LNCS), pp. 256–276. Springer. DOI: `10.1007/3-540-57253-8_58` — **[VERIFIED]**

> Boose, J. H., & Bradshaw, J. M. (1987). Expertise transfer and complex problems: using AQUINAS as a knowledge-acquisition workbench for knowledge-based systems. *International Journal of Man-Machine Studies*, 26(1), 3–28. DOI: `10.1016/S0020-7373(87)80032-9` — **[VERIFIED]** (reprinted 1999 in *IJHCS* 51(2), 453–478, DOI `10.1006/ijhc.1987.0319` — also verified). AQUINAS is the canonical repertory-grid-based knowledge acquisition workbench; ETS was its predecessor.

> Boose, J. H. (1989). A survey of knowledge acquisition techniques and tools. *Knowledge Acquisition*, 1(1), 3–37. DOI: `10.1016/S1042-8143(89)80003-2` — **[VERIFIED]**

> Tan, F. B., & Hunter, M. G. (2002). The Repertory Grid Technique: A Method for the Study of Cognition in Information Systems. *MIS Quarterly*, 26(1), 39–57. DOI: `10.2307/4132340` — **[VERIFIED]**

### 5.2 The elicitation mechanism — triadic elicitation

From Shadbolt & Smart (2015) **[VERIFIED+FT]**, with their worked planets example:

1. Fix a set of **elements** — the things in the domain being distinguished.
2. Present the expert with **three** elements and ask them to choose "three, such that **two are similar, and different from the third**. This is known as the method of **triadic elicitation**."
3. Ask *why*: "The expert is then asked for her reason for differentiating these elements, and **this dimension is known as a construct.**" A construct is bipolar — e.g. `small — large`.
4. **Rate every remaining element** on that construct (their example uses a nine-point scale; Mercury = 1, Jupiter = 9 on `small/large`).
5. Repeat with new triads "until the expert can think of no further discriminating constructs."
6. Output: "**a matrix of similarity ratings, relating elements and constructs.**"

**The data structure is therefore a dense numeric matrix: rows = constructs (each with two named poles), columns = elements, cells = integer ratings on a fixed scale.** It is a CSV. There is no ambiguity in serializing it.

### 5.3 The analysis mechanism — and it is deterministic

The standard analysis is hierarchical cluster analysis over both rows and columns. **Shadbolt & Smart give the actual similarity formula** (their footnote 17, attributing the FOCUS algorithm to Jankowicz & Thomas 1982):

> "The percentage similarity between adjacent elements in the grid is computed as **((−100 × d) / (c × (n − 1))) + 100**, where *d* is the sum of the absolute differences between the ratings of adjacent elements, *c* is the number of constructs in the grid, and *n* is the largest rating possible."

This is a closed-form, integer-arithmetic city-block distance normalized to a percentage. **It is bit-for-bit deterministic and would run in microseconds** — it comfortably satisfies NAADAP's determinism and compute constraints. That is worth stating clearly, because it means the objection to repertory grids for NAADAP is *not* a computational one (§5.5).

In their worked example the method surfaced a concept the expert had not articulated: "Jupiter and Saturn are clustered together at around 84% similarity, Neptune and Uranus at around 88%… An astronomer might well observe that this group of four planets constitutes the gas giants. **A new concept — gas giant — has thus been uncovered.**" Constructs cluster too, and "such associations can reveal causal or other law-like relations in the domain."

### 5.4 Shaw & Gaines's four-way comparison — the genuinely valuable part

This is the piece of the repertory-grid tradition I would actually import into NAADAP. When you elicit grids from *several* experts, their terminologies only partly overlap. Shaw & Gaines classify every pairwise term relationship into a 2×2 over {same/different terminology} × {same/different distinction}:

| | Same distinction | Different distinction |
|---|---|---|
| **Same terminology** | **Consensus** — genuine agreement | **Conflict** — the dangerous case: two experts use the same word for different things and never discover it |
| **Different terminology** | **Correspondence** — synonyms; safe to merge | **Contrast** — genuinely distinct areas of expertise; keep both |

**[PARTIAL]** — the four category names and their definitions are confirmed by the paper's title (verified via Crossref) and by a consistent search-result summary of the publisher page, which I could not fetch directly. The framework is reported identically across the secondary literature.

**Why this matters enormously to NAADAP.** You will interview more than one contracting SME. They will use "**vehicle**," "**strategic sourcing**," "**consolidation**," "**bundling**," and "**requirement**" with materially different extensions — and *bundling* vs *consolidation* is a case where the terms have distinct statutory definitions that practitioners routinely blur. **Conflict** (same word, different distinction) is exactly the failure that produces an indefensible recommendation, and it is invisible unless you deliberately look for it. Shaw & Gaines's contribution is that this comparison can be *computed* from grids or from any structured elicitation output, rather than hoped for. **NAADAP should run a conflict check across SMEs even if it never elicits a single repertory grid** — the check applies equally to concept-map propositions and to glossary entries.

### 5.5 Tools, and the recommendation

**WebGrid** — the long-running web implementation by Gaines & Shaw, referenced by Shadbolt & Smart as "WebGrid 5" at `gigi.cpsc.ucalgary.ca` / `webgrid.uvic.ca`. **I checked `http://webgrid.uvic.ca` and it returned HTTP 503.** Treat it as unreliable infrastructure; do not depend on it.

> Gaines, B. R., & Shaw, M. L. G. (1997). Knowledge acquisition, modelling and inference through the World Wide Web. *International Journal of Human-Computer Studies*, 46(6), 729–759. DOI: `10.1006/ijhc.1996.0122` — **[VERIFIED]** (the WebGrid paper.)

**OpenRepGrid** — `markheckmann/OpenRepGrid`, **R**, **GPL (≥ 2)**, on CRAN, maintained by Mark Heckmann. Companion Shiny app `markheckmann/OpenRepGrid.ic` for interpretive clustering. **[VERIFIED]** via CRAN and GitHub listings. This is a live, maintained implementation — but it is R, which NAADAP cannot ship.

**Recommendation: do not build repertory grids into NAADAP's elicitation.** Reasons, in order of weight:

1. **Fixed element set.** Triadic elicitation requires the elements — the things being compared — to be enumerated *in advance*. NAADAP's inputs are arbitrary incoming procurement documents. You could grid the *vehicles* (SeaPort-NxG, OASIS+, GSA MAS, the NAWCAD MACs — a stable, small, enumerable set), but you cannot grid the requirements. The technique fits only half the problem.
2. **The output is not citable evidence.** A contracting officer defending a consolidation decision cannot rest on "these two vehicles clustered at 84% similarity on the expert's construct space." The number is real and reproducible, but it has no standing in a legal determination. It is not a FAR citation, not a scope statement, not a precedent. Compare the CDM's Situation Assessment Record or a concept-map proposition with an attached FAR reference — those *are* defensible. This is the decisive objection and it is about the character of the evidence, not the math.
3. **Construct-relative scales don't compose.** One SME's 1–9 `narrow/broad scope` scale is not commensurable with another's. Merging requires either forcing a shared construct set (which destroys the technique's main virtue) or a normalization step that introduces exactly the kind of unexplainable transformation NAADAP is trying to avoid.
4. **It is a contrived technique.** Shadbolt & Smart note contrived techniques must be "explain[ed] carefully to the expert before starting" and "experts may feel they are performing badly." With scarce, senior, soon-to-retire government SMEs, spending goodwill on an unfamiliar rating exercise is poor economics when concept mapping yields ~2 propositions/minute in a format the SME finds natural.

**What to take from this tradition instead:** (a) Shaw & Gaines's **consensus / conflict / correspondence / contrast** analysis, applied to whatever elicitation output you actually collect — this is cheap and catches a real, dangerous failure; and (b) the discipline of **bipolar constructs**, which is a better way to write a scoring feature than a unipolar score: `in-scope ←→ out-of-scope` with an explicit opposite pole forces the SME to say what the *other* end means, which is exactly the information a threshold needs.

---

## 6. Synthesis: What NAADAP Should Actually Build

### 6.1 The recommended instrument stack

Three complementary instruments, justified by the differential access hypothesis (§4.5):

| Instrument | Elicits | Output artifact | Est. SME cost |
|---|---|---|---|
| **Expert-skeleton concept mapping** (§2.1) | Conceptual structure: what a vehicle is, what scope means, how requirement types relate | CXL / triples with focus question + author metadata | ~90 min/SME, ~180 propositions each |
| **ACTA knowledge audit + simulation interview** (§1.2) | Judgment: what makes a vehicle decision hard, what cues drive it, where novices err | Cognitive demands table (4 cols) + simulation table (5 cols) | ~2 h/SME + ~2 h analyst |
| **CDM on 3–5 named past consolidations** (§1.1) | Episodic: the decisions that actually got made and why | Timeline + Critical Cue Inventory + Situation Assessment Record | ~2 h/incident |

Plus **formal text analysis** (METHONTOLOGY, §3.2) over FAR/DFARS/NMCARS, vehicle ordering guides, and past J&As — free of SME time and, for NAADAP, the source of most of the *citable* evidence.

Planning figure: **4–6 SMEs × ~4 hours each ≈ 20–24 SME-hours**, plus roughly 2–3× that in analyst time. That is a realistic scope for a prize-challenge effort.

### 6.2 The serialization architecture

The elicitation output should land in **one versioned, hashed, container-shipped artifact** with these layers:

1. **`competency-questions.json`** — numbered CQ register (Grüninger & Fox Stage 2), each CQ carrying: id, natural-language text, motivating scenario reference, the terms it justifies, and an expected-result fixture. **This file is the test suite.** Every term in the vocabulary must be justified by at least one CQ (the justification rule, §3.1), and that invariant is itself a check you can run in CI.
2. **`glossary.csv` / `terms.csv`** — METHONTOLOGY's Glossary of Terms and concept/attribute tables (§3.2). Columns: term, type (concept|verb|attribute|instance), definition, source (SME id or document citation), CQ ids justifying it, synonyms, and — from Shaw & Gaines — a `conflict-flag` where SMEs disagreed on extension.
3. **`propositions.ttl`** (or a CXL file plus a derived triples file) — the concept-map content, with linking phrases **constrained to a controlled relation palette** defined in the glossary (per the COE lesson, §2.3). Each proposition carries: subject, relation, object, contributing SME, session date, focus question, and zero or more `evidence` links.
4. **`rules.csv`** — the CDM Situation Assessment Records and ACTA cognitive demands table rows, as typed rows: `cue | expectancy | goal | action | difficulty | common-error | source-incident | authority-citation`.
5. **`evidence/`** — the actual citable sources: FAR/DFARS clause text, vehicle ordering guide excerpts, past J&A language. Each referenced by stable id from layers 3 and 4.
6. **`MANIFEST.json`** — for each file: SHA-256, byte length, schema version, elicitation session ids, SME identifiers (or pseudonyms), and capture dates. A single root hash over the manifest is the artifact version.

**On hashing and determinism.** Three rules make the hash stable and the pipeline reproducible:
- Serialize with a **canonical ordering** (sort propositions by `(subject, relation, object)`; sort CSV rows by primary key) before hashing. Never hash tool-emitted output directly.
- **Exclude presentation from the hash.** CXL's separation of `concept-list` from `concept-appearance-list` (§2.2) makes this trivial: hash only the semantic elements, so an SME dragging a box in CmapTools does not produce a new artifact version.
- **Pin the artifact hash into the recommendation output.** Every NAADAP top-5 result should carry the knowledge-artifact root hash alongside its evidence citations. Two runs that produce different recommendations can then be diagnosed immediately as either a code change or a knowledge change.

### 6.3 Validation — the part most projects skip

Three independent gates, in increasing order of what they prove:

1. **Structural.** Every term traces to a CQ; every proposition uses a relation from the palette; every rule row cites an authority. Trivially checkable; catches sloppiness. (Optionally SHACL, §3.5, if you go RDF — but verify .NET SHACL support before committing.)
2. **Competency.** Each CQ is an executable query with an expected result; the suite emits JUnit XML (the Themis pattern, §3.5). **This is the Grüninger & Fox completeness idea reduced to something that runs in 1 CPU / 2 GB.** Extending the artifact must not break existing CQs — the regression property from §3.1.
3. **Outcome.** Held-out historical consolidations with known vehicle assignments. **This is the non-negotiable one**, because of Burton et al. (§4.4): plausibility is not validity, and people who know nothing can produce plausible knowledge bases. If the elicited artifact does not beat a domain-naive baseline on held-out ground truth, it has not captured anything.

### 6.4 Consistency with NAADAP's hard constraints

| Constraint | Status |
|---|---|
| Fully offline in Docker | ✔ All artifacts are static files. No runtime service, no CmapTools, no Themis, no WebGrid. Elicitation tooling is used *offline during authoring* and does not ship. |
| Deterministic, ≥95% identical top-5 | ✔ Canonical ordering + integer/rational arithmetic + no reasoner. The one risk is floating-point score ties — resolve ties by a deterministic key (e.g. vehicle id), never by dictionary/hash order. |
| 1 CPU / 2 GB / 30 min for 20 docs | ✔ Expect the knowledge artifact at 10²–10³ propositions — a few hundred KB, loaded once. Even the FOCUS similarity formula (§5.3) is microseconds. The compute budget goes to document processing, not to knowledge lookup. |
| No LLM on the core path | ✔ Nothing here needs one. Elicitation is human; serialization is XML/CSV/Turtle; scoring is table lookup plus deterministic text features. |
| C#/.NET 9, minimal dependencies | ✔ CXL and CSV need only `System.Xml` and a small CSV reader. **Avoid** pulling in an RDF/OWL stack unless SHACL earns its place; a typed C# record + a dictionary index is enough. |
| Citable evidence for every recommendation | ✔ This is the architecture's organizing principle, and per Feigenbaum (§4.1) it is also what makes the knowledge base debuggable. |

---

## 7. Consolidated citation table

| # | Citation | Identifier | Status |
|---|---|---|---|
| 1 | Militello & Hutton (1998), *Ergonomics* 41(11):1618–1641 | `10.1080/001401398186108` | VERIFIED (abstract verbatim) |
| 2 | Militello, Hutton, Pliske, Knight & Klein (1997), ACTA Methodology, NPRDC, AD-A335225 | DTIC AD-A335225 | **UNVERIFIED — DTIC blocked** |
| 3 | Klein, Calderwood & MacGregor (1989), *IEEE T-SMC* 19(3):462–472 | `10.1109/21.31053` | VERIFIED |
| 4 | Hoffman, Crandall & Shadbolt (1998), *Human Factors* 40(2):254–276 | `10.1518/001872098779480442` | VERIFIED |
| 5 | Crandall, Klein & Hoffman (2006), *Working Minds*, MIT Press | `10.7551/mitpress/7304.001.0001` | VERIFIED |
| 6 | Shadbolt & Smart (2015), in *Evaluation of Human Work* 4e, pp. 196–233 | `10.1201/b18362-18` | **VERIFIED+FT** |
| 7 | Stanik et al. (2021), Lessons Learned … ACTA | arXiv:2108.05622 | **VERIFIED+FT** |
| 8 | Tofel-Grehl & Feldon (2013), *JCEDM* 7(3):293–304 | `10.1177/1555343412474821` | VERIFIED (effect size PARTIAL) |
| 9 | Novak & Cañas (2008), IHMC CmapTools 2006-01 Rev 01-2008 | cmap.ihmc.us | **VERIFIED+FT** |
| 10 | CXL schema + `PlantsSimple.cxl` | cmap.ihmc.us/xml/cmap.xsd | **VERIFIED+FT** (both fetched, HTTP 200) |
| 11 | Hayes, Eskridge, Saavedra, Reichherzer, Mehrotra & Bobrovnikoff (2005), K-CAP '05, pp. 99–106 | `10.1145/1088622.1088641` | **VERIFIED+FT** |
| 12 | Cañas et al. (2005), LNCS, pp. 205–219 | `10.1007/11510154_11` | VERIFIED |
| 13 | Hoffman, Coffey, Ford & Novak (2006), *Weather and Forecasting* 21(3):416–428 | `10.1175/waf927.1` | VERIFIED (full text 403) |
| 14 | Hoffman, Coffey, Ford & Carnot (2001), *HFES Proc.* 45(8):752 | `10.1177/154193120104500807` | VERIFIED |
| 15 | Coffey, Hoffman & Cañas (2006), *Information Visualization* 5(3):192–201 | `10.1057/palgrave.ivs.9500129` | VERIFIED |
| 16 | Moon, Hoffman, Novak & Cañas (2011), *Applied Concept Mapping*, CRC Press | `10.1201/b10716` | VERIFIED |
| 17 | Grüninger & Fox (1995), IJCAI-95 Workshop on Basic Ontological Issues in Knowledge Sharing | eil.utoronto.ca (OCR'd) | **VERIFIED+FT** |
| 18 | Grüninger & Fox (1995), *Benchmarking — Theory and Practice*, pp. 22–31 | `10.1007/978-0-387-34847-6_3` | **VERIFIED+FT** |
| 19 | Fernández, Gómez-Pérez & Juristo (1997), AAAI-97 Spring Symp. SS-97-06 | oa.upm.es/5484 | **VERIFIED+FT** (no DOI exists) |
| 20 | Sure, Staab & Studer (2004), OTKM, *Handbook on Ontologies* pp. 117–132 | `10.1007/978-3-540-24750-0_6` | VERIFIED (mechanism PARTIAL) |
| 21 | Sure, Staab & Studer (2009), *Handbook on Ontologies* 2e pp. 135–152 | `10.1007/978-3-540-92673-3_6` | VERIFIED |
| 22 | Sure, Staab & Studer (2002), *ACM SIGMOD Record* 31(4):18–23 | `10.1145/637411.637414` | VERIFIED |
| 23 | Suárez-Figueroa, Gómez-Pérez & Fernández-López (2011), NeOn Methodology, pp. 9–34 | `10.1007/978-3-642-24794-1_2` | VERIFIED |
| 24 | Suárez-Figueroa & Gómez-Pérez (2011), Ontology Requirements Specification, pp. 93–106 | `10.1007/978-3-642-24794-1_5` | VERIFIED |
| 25 | Gómez-Pérez & Suárez-Figueroa (2009), NeOn scenario-based methodology | oa.upm.es/5475 | **VERIFIED+FT** |
| 26 | Suárez-Figueroa et al. (2009), How to Write and Use the ORSD | `10.1007/978-3-642-05151-7_16` | VERIFIED |
| 27 | Suárez-Figueroa, PhD dissertation, UPM | `10.20868/upm.thesis.3879` | VERIFIED |
| 28 | Potoniec, Wiśniewski, Ławrynowicz & Keet (2020), *Data in Brief* 29:105098 | `10.1016/j.dib.2019.105098` | VERIFIED |
| 29 | Wiśniewski, Potoniec, Ławrynowicz & Keet (2019), CQ/SPARQL-OWL analysis | `10.2139/ssrn.3490821` | VERIFIED |
| 30 | Wiśniewski, Potoniec & Ławrynowicz (2022), BigCQ, *AAAI* 36(11):13079–13080 | `10.1609/aaai.v36i11.21676` | VERIFIED |
| 31 | Ren, Parvizi, Mellish, Pan, van Deemter & Stevens (2014), ESWC, pp. 752–767 | `10.1007/978-3-319-07443-6_50` | VERIFIED |
| 32 | W3C (2017), SHACL, W3C Recommendation 20 July 2017 | w3.org/TR/shacl/ | **VERIFIED+FT** |
| 33 | Schreiber et al. (1999), *Knowledge Engineering and Management* (CommonKADS), MIT Press | `10.7551/mitpress/4073.001.0001` | VERIFIED (mechanism not verified) |
| 34 | Guarino & Welty (2009), OntoClean, *Handbook on Ontologies* 2e pp. 201–220 | `10.1007/978-3-540-92673-3_9` | VERIFIED |
| 35 | Noy & McGuinness (2001), Ontology Development 101, KSL-01-05 | protege.stanford.edu | **PARTIAL — not retrieved** |
| 36 | Feigenbaum (1977), IJCAI-77, pp. 1014–1029 | ijcai.org/Proceedings/77-2/092 ; `10.21236/ada046289` | **VERIFIED+FT** |
| 37 | Hayes-Roth, Waterman & Lenat (eds.) (1983), *Building Expert Systems* | via reviews `10.1017/s0263574700002022`, `10.1017/s0263574700004069` | VERIFIED indirectly |
| 38 | Buchanan & Shortliffe (eds.) (1984), *Rule-Based Expert Systems* | via review `10.1080/02630258408970370` | VERIFIED indirectly |
| 39 | Clancey (1983), *Artificial Intelligence* 20(3):215–251 | `10.1016/0004-3702(83)90008-5` | VERIFIED (mechanism PARTIAL) |
| 40 | Clancey (1994), Notes on Epistemology…, MIT Press pp. 197–204 | `10.7551/mitpress/1413.003.0029` | VERIFIED |
| 41 | Burton, Shadbolt, Rugg & Hedgecock (1990), *Knowledge Acquisition* 2(2):167–178 | `10.1016/S1042-8143(05)80010-X` | VERIFIED (findings PARTIAL) |
| 42 | Hoffman, Shadbolt, Burton & Klein (1995), *OBHDP* 62(2):129–158 | `10.1006/obhd.1995.1039` | VERIFIED |
| 43 | Shadbolt & Burton (1989), *ACM SIGART Bulletin* 108:15–18 | `10.1145/63266.63268` | VERIFIED |
| 44 | Gaines (2013), *IJHCS* 71(2):135–156 | `10.1016/j.ijhcs.2012.10.010` | **VERIFIED citation; FULL TEXT UNAVAILABLE — no claims made** |
| 45 | Gaines & Shaw (1993), *Knowledge Engineering Review* 8(1):49–85 | `10.1017/S0269888900000060` | VERIFIED (abstract retrieved) |
| 46 | Shaw & Gaines (1989), *Knowledge Acquisition* 1(4):341–363 | `10.1016/S1042-8143(89)80010-X` | VERIFIED (4-way framework PARTIAL) |
| 47 | Shaw & Gaines (1993), LNCS, pp. 256–276 | `10.1007/3-540-57253-8_58` | VERIFIED |
| 48 | Gaines & Shaw (1997), *IJHCS* 46(6):729–759 (WebGrid) | `10.1006/ijhc.1996.0122` | VERIFIED |
| 49 | Boose & Bradshaw (1987), *IJMMS* 26(1):3–28 (AQUINAS) | `10.1016/S0020-7373(87)80032-9` | VERIFIED |
| 50 | Boose (1989), *Knowledge Acquisition* 1(1):3–37 | `10.1016/S1042-8143(89)80003-2` | VERIFIED |
| 51 | Tan & Hunter (2002), *MIS Quarterly* 26(1):39–57 | `10.2307/4132340` | VERIFIED |
| 52 | Kelly (1955), *The Psychology of Personal Constructs* | — | **UNVERIFIED (uncontroversial)** |
| 53 | Leu & Abbass (2018), Multi-Disciplinary Review of KA Methods | arXiv:1802.09669 | **VERIFIED+FT** |
| 54 | Gavrilova & Andreeva (2012), *J. Knowledge Management* 16(4):523–537 | `10.1108/13673271211246112` | **VERIFIED+FT** |
| 55 | Davison, H., *Cognitive Task Analysis: Current Research*, MIT 16.459 course slides | web.mit.edu/16.459/www/CTA2.pdf | **VERIFIED+FT — but a TEACHING SECONDARY SOURCE.** Source of ACTA probe wording and table columns; confirm against a primary before publishing. |
| 56 | `dwisniewski/SeeQuery` — Python, MIT | github.com | VERIFIED (repo page fetched) |
| 57 | `oeg-upm/Themis` — Java, Apache-2.0 | github.com | VERIFIED (repo page fetched) |
| 58 | `markheckmann/OpenRepGrid` — R, GPL (≥2), on CRAN | CRAN/github.com | VERIFIED |

### Explicitly NOT verified — do not cite without retrieval
- **AD-A335225** (primary ACTA methodology report) — DTIC blocked. The ACTA probe wording and cognitive-demands-table columns in §1.2 rest on secondary sources.
- **Gaines (2013)** — full text unobtainable at both known URLs. No claims made from it.
- **Hoffman et al. (2006)** *Weather and Forecasting* — quantitative details of the STORM-LK study.
- **Tofel-Grehl & Feldon (2013)** — the Hedges's *g* = 0.871 figure.
- **Burton et al. (1990)** — the protocol-analysis-is-least-efficient and non-experts-produce-plausible-KBs findings.
- **Shaw & Gaines (1989)** — the precise definitions of consensus/conflict/correspondence/contrast.
- **On-To-Knowledge** five phase names.
- **NeOn ORSD** full template field list.
- **Noy & McGuinness (2001)** — not retrieved.
- **Kelly (1955)** — not independently verified.
