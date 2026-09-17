# <a id="semp-title"></a>Systems Engineering Management Plan (SEMP)

## <a id="semp-title-b"></a>NAADAP — Acquisition-Documentation Analysis and Contract-Vehicle Recommender

| Field | Value |
| --- | --- |
| Document identifier | NAADAP-SEMP-001 |
| Revision | Draft A.3 |
| Date | 2026-09-17 |
| Data Item Description | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785B</a>, Systems Engineering Management Plan (SEMP), approved 2025-01-08, Acquisition Management Systems Control (AMSC) 10515, project SESS-2024-043; supersedes <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785A</a>. All three revisions (2009, A, B) are on file in <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/</code></a>, downloaded from Acquisition Streamlining and Standardization Information System (ASSIST) 2026-09-15. The A-to-B delta is reconciled in <a href="#semp-1-3" target="_blank">§1.3</a>. |
| Topic source | Office of the Secretary of Defense (OSD) Systems Engineering Plan (SEP) Outline, Version 4.1, May 2023 (Defense Office of Prepublication and Security Review (DOPSR) 23-S-1904), applied per <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785</a> §2: "In the absence of a government SEP, the SEMP shall address the topics in the OSD SEP Outline active at the time of the Request for Proposal (RFP)." |
| Program | NAADAP prize-challenge entry (Phase 2 initial technical package; Phase 3 Demo Day) |
| Preparing organization | HoloSim Interactive |
| Sponsor | Naval Air Systems Command (NAVAIR) / Naval Air Warfare Center Aircraft Division (NAWCAD), Procurement Group Innovation Lab (PGIL), via Tech Grove |
| Classification | UNCLASSIFIED |
| Distribution | Public repository content. No Government Furnished Information (GFI) is reproduced in this document. |
| Supersedes | The "Systems Engineering Technical Review (SETR) Documentation Mapping (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-960" target="_blank">DELIV-960</a>)" table in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>, for the review sequence only (see <a href="#semp-3-2-13" target="_blank">§3.2.13</a>). |

### <a id="semp-approval"></a>Approval

The SEP Outline's approval page names Government positions. This is a contractor SEMP; the positions below are the contractor equivalents. A signature on the "Approved" line constitutes acceptance of this plan as the technical-management baseline for the program.

| Action | Position | Name | Signature | Date |
| --- | --- | --- | --- | --- |
| Prepared by | Systems Engineer | | | |
| Reviewed by | Solutions Architect | | | |
| Reviewed by | Product Manager | | | |
| Approved by | Principal, HoloSim Interactive (Program Manager) | | | |
| Concurrence (when applicable) | Government Lead Systems Engineer, PGIL / NAWCAD | | | |

### <a id="semp-revision-history"></a>Revision history

| Revision | Date | Description | Author |
| --- | --- | --- | --- |
| Draft A | 2026-09-15 | Initial issue. Addresses SEP Outline 4.1 topics for Increments 1 and 2. | Systems Engineer |
| Draft A.1 | 2026-09-15 | Minor update per <a href="#semp-1-6" target="_blank">§1.6</a> item 5: <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785B</a> reconciled (<a href="#semp-1-3" target="_blank">§1.3</a>, <a href="#semp-4" target="_blank">§4</a>, References). Issue <a href="#semp-i-2" target="_blank">I-2</a> closed. Engineering Site Survey (ESS) recorded as not applicable (<a href="#semp-table-3-2-4" target="_blank">Table 3.2-4</a>). | Systems Engineer |
| Draft A.2 | 2026-09-17 | Minor update per <a href="#semp-1-6" target="_blank">§1.6</a> item 5: documentation conventions recorded (<a href="#semp-3-2-11" target="_blank">§3.2.11</a>: abbreviations, cross-reference hyperlinks); review-package definition made explicit (<a href="#semp-3-2-13" target="_blank">§3.2.13</a>); the Integrated Product Team (IPT) table renumbered from 3.1-1 to 3.1-2 to remove a duplicate; System Verification Review 1 (SVR-1) / Functional Configuration Audit 1 (FCA-1) exit and Increment 1 product-baseline acceptance recorded (<a href="#semp-table-3-1-1" target="_blank">Table 3.1-1</a>); <a href="#semp-appendix-a" target="_blank">Appendix A</a> retitled and extended to every abbreviation in the SETR package. | Systems Engineer |
| Draft A.3 | 2026-09-17 | Minor update per §1.6 item 5: operator-interface direction recorded (queued amendment UI-010 in §2.1.4; Table 3.1-1 Increment 2 row). Branch `issue-svr1` deletion requested by the Principal (Issue I-5 note). | Systems Engineer |

### <a id="semp-contents"></a>Contents

1. <a href="#semp-1" target="_blank">Introduction</a>
2. <a href="#semp-2" target="_blank">Program Technical Definition</a>
3. <a href="#semp-3" target="_blank">Program Technical Management</a>
4. <a href="#semp-4" target="_blank">DID Conformance Crosswalk</a>
- <a href="#semp-appendix-a" target="_blank">Appendix A — Abbreviations and Acronyms</a>
- <a href="#semp-appendix-b" target="_blank">Appendix B — Item Unique Identification Implementation Plan</a>
- <a href="#semp-appendix-c" target="_blank">Appendix C — Agile and DevSecOps Software Development Metrics</a>
- <a href="#semp-appendix-d" target="_blank">Appendix D — Concept of Operations Description</a>
- <a href="#semp-appendix-e" target="_blank">Appendix E — Digital Engineering Implementation Plan</a>
- <a href="#semp-references" target="_blank">References</a>

---

## <a id="semp-1"></a>1 Introduction

### <a id="semp-1-1"></a>1.1 Program summary

NAADAP is HoloSim Interactive's entry to a NAVAIR/NAWCAD prize challenge. The product is a batch software tool that reads a set of acquisition documents (statements of work, performance work statements, Contract Data Requirements Lists (CDRLs), sources-sought notices, and open-source text) and returns, for each group of documents sharing a common requirement, a ranked list of candidate strategic contract vehicles capable of absorbing that requirement, with the evidence for each candidate and the reasons the other vehicles were ruled out. The tool identifies suitable vehicles; it does not select one. Selection is reserved to the contracting officer and the approval authorities named in Federal Acquisition Regulation (FAR) 7.107 and Navy Marine Corps Acquisition Regulation Supplement (NMCARS) 5237.102.

The program has two increments, decided by the Principal on 2026-09-15.

| Increment | Deliverable | Due | Data |
| --- | --- | --- | --- |
| Increment 1 (v1) | Phase 2 initial technical package: the twelve items listed in <a href="#semp-3-1-1" target="_blank">§3.1.1</a> | Submit 2026-09-22; Government-directed revisions resubmitted by 2026-10-02 (client direction 2026-09-17, Issue <a href="#semp-i-7" target="_blank">I-7</a>) | Public only: SAM.gov documents, the USAspending/FPDS bulk archive, a hand-curated then data-derived vehicle knowledge base |
| Increment 2 (v2) | Phase 3 Demo Day materials and the basis for a follow-on Other Transaction agreement | Materials 2026-11-12; Demo Day 2026-11-09 or 2026-11-19 | GFI for the document side; Federal Procurement Data System (FPDS) for the label side |

Increment 1 is implemented, verified against its Requirements Traceability and Verification Matrix (RTVM) except as noted in <a href="#semp-3-1-2" target="_blank">§3.1.2</a>, and tagged (`v1.0.82` at this revision). Increment 2 is designed (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a>) and gated (<a href="#semp-3-1-1" target="_blank">§3.1.1</a>, gates <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g2" target="_blank">G2</a> through <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a>).

### <a id="semp-1-2"></a>1.2 Purpose and applicability

This SEMP is the contractor's plan for the engineering of NAADAP. It is the first document of the SETR package the program is producing under Naval Air Systems Command Instruction (NAVAIRINST) 4355.19E and the SETR Process Handbook v1.0. Every later SETR artifact (review packages, technical review summary reports, requests for action) is planned here and traces back here.

It applies to all engineering work on the NAADAP repository from 2026-09-03 (program start; first commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/869f205" target="_blank"><code>869f205</code></a>) through the close of Phase 3, and to the follow-on Other Transaction agreement if one is awarded, until superseded by a SEMP realigned to that agreement's Government SEP.

### <a id="semp-1-3"></a>1.3 Relationship to the DID and the SEP Outline

<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785</a> prescribes the SEMP's minimum content (<a href="#semp-3-1" target="_blank">§3.1</a> through §3.8 of the Data Item Description (DID)) and leaves format to the contractor. Because no Government SEP exists for a prize challenge, the DID's own rule applies and this SEMP addresses the topics of the OSD SEP Outline 4.1. To make that traceable, <a href="#semp-1" target="_blank">§1</a> through <a href="#semp-3" target="_blank">§3</a> of this document follow the Outline's section numbering, so a Government reviewer finds each topic where the Outline puts it. <a href="#semp-4" target="_blank">§4</a> maps the DID's numbered content items to the sections here that satisfy them.

Two conformance limits are recorded rather than hidden.

- The active DID revision is B (2025-01-08), reconciled against Revision A (2015-09-29) on 2026-09-15 from the ASSIST copies of both. The content requirements 3.1 through 3.8 are word-for-word identical between the two revisions. Revision B changes only the standards it cites: the originating work task moves from Institute of Electrical and Electronics Engineers (IEEE) 15288.1 paragraph 6.3.1.4 to IEEE 24748-7:2019 paragraph 6.3.1.4; reference (b) becomes IEEE 24748-7:2019 (application of systems engineering on defense programs) and reference (c) becomes IEEE 24748-8:2019 (technical reviews and audits on defense programs), which replace IEEE 15288.1 and 15288.2 respectively; and content item 3.5.b now names IEEE 24748-8 as the definition of formal technical reviews and audits. No section of this SEMP changes in substance; <a href="#semp-4" target="_blank">§4</a> and the References cite the Revision B standards.
- The Outline is written for a Government Program Management Office. Where an Outline topic has no contractor-side content for this program (for example, the Acquisition Program Baseline, the Program Executive Officer's approval, or Milestone decisions), the section says so and gives the reason. A section marked "Not applicable" is a considered answer, not an omission.

### <a id="semp-1-4"></a>1.4 Tailoring

NAADAP is a non-ACAT software effort with no hardware, no manufacturing, no fielded hardware to sustain, and no Government contract yet. Tailoring follows SETR Process Handbook §6.4 for rapid-acquisition and non-ACAT programs: the minimum review set is System Requirements Review (SRR) (I and II), Preliminary Design Review (PDR), Critical Design Review (CDR), and Test Readiness Review (TRR); Alternative Systems Review (ASR) and Software Specification Review (SSR) fold into PDR; System Functional Review (SFR) folds into SRR-II or PDR; Integration Readiness Review (IRR) folds into CDR for high off-the-shelf content. The program adds SVR/FCA and Physical Configuration Audit (PCA) for each increment because the deliverable is a frozen package whose product baseline is what the evaluators build and run, and adds Release Backlog Review (RBR) for Increment 2 because it is developed in Agile sprints against a release backlog. The full tailored sequence and each review's criteria are in <a href="#semp-3-2-13" target="_blank">§3.2.13</a>.

### <a id="semp-1-5"></a>1.5 Alignment with a Government SEP

None exists today. If an Other Transaction agreement follows Phase 3 and the Government issues a SEP, this SEMP is realigned to it within the window the Outline sets for the corresponding post-award SEP update: 120 days after award or 30 days before the next technical review, whichever is earlier.

### <a id="semp-1-6"></a>1.6 Update criteria and authority

This SEMP is updated when any of the following occurs, and at no other time:

1. A tailored review in <a href="#semp-3-2-13" target="_blank">§3.2.13</a> is added, removed, or has its entry or exit criteria changed.
2. A technical baseline (<a href="#semp-3-2-10" target="_blank">§3.2.10</a>) is established or re-established.
3. Tech Grove answers a question in <a href="#semp-3-2-1" target="_blank">§3.2.1</a> that changes a date, a scoring interpretation, or a deliverable.
4. The Principal accepts an RTVM amendment that changes a requirement's verification method or status class.
5. A new revision of <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785</a> is issued.

Updates are proposed by the Systems Engineer, reviewed by the Solutions Architect and Product Manager, and approved by the Principal. A change to <a href="#semp-3-2-13" target="_blank">§3.2.13</a> (review criteria) or <a href="#semp-3-2-10" target="_blank">§3.2.10</a> (baselines) is a major update and receives a new revision letter. Any other change is a minor update and receives a numbered sub-revision (Draft A.1).

### <a id="semp-1-7"></a>1.7 Program phase, entry and exit criteria

| Phase | Entry criteria | Exit criteria | Status 2026-09-15 |
| --- | --- | --- | --- |
| Challenge Phase 1 — Pre-Screening | Challenge launched 2026-07-30 | Questionnaire submitted; approval received; GFI and portal access granted | Questionnaire in preparation; submission targeted within five days |
| Challenge Phase 2 — Initial technical package (Increment 1) | Phase 1 approval | Twelve-item package submitted through the portal by the deadline; completeness gate satisfied | Engineering complete to RTVM; packaging and SVR/FCA pending |
| Challenge Phase 3 — Demo Day (Increment 2) | Semifinalist notification, 2026-10-26 | Materials submitted 2026-11-12; live demonstration completed within the 30-minute window | Design complete; gates <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g2" target="_blank">G2</a>–<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> open |

---

## <a id="semp-2"></a>2 Program Technical Definition

### <a id="semp-2-1"></a>2.1 Requirements Development

#### <a id="semp-2-1-1"></a>2.1.1 Requirements sources and decomposition

The challenge has no Joint Capabilities Integration and Development System (JCIDS) document. The authoritative requirements source is the challenge announcement (Overview, Problem Statement, Benefits, Critical Technical Criteria, Phase 2 delivery definition, and Evaluation Criteria), reproduced and analyzed in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.claude/skills/gov-acquisition-sme/references/challenge-brief.md" target="_blank"><code>.claude/skills/gov-acquisition-sme/references/challenge-brief.md</code></a>. Decomposition proceeds in four levels. Every requirement at each level carries a parent at the level above; the RTVM's stakeholder-need column enforces this and there are no orphan requirements.

| Level | Artifact | Owner | Content | Status |
| --- | --- | --- | --- | --- |
| L0 Capability source | Challenge announcement | Sponsor | Problem statement, criteria, rubric | Fixed; two open interpretation questions (<a href="#semp-3-2-1" target="_blank">§3.2.1</a>, <a href="#semp-r-2" target="_blank">R-2</a> and <a href="#semp-r-7" target="_blank">R-7</a>) |
| L1 Stakeholder needs | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank"><code>docs/PROJECT_DEFINITION.md</code></a>, Stakeholder Need (SN) <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a> through <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-6" target="_blank">SN-6</a> | Product Manager | Six confirmed needs, each tagged [CONFIRMED] by the Principal | Baselined 2026-09-03 |
| L2 System requirements | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> | Systems Engineer | 30 line items in seven families (User Interface (UI), DATA-IN, CORE, DATA-OUT, OUT, Non-Functional Requirement (NFR), DELIV); 2 Withdrawn | Baselined 2026-09-03 (SRR-I); amendments queued (<a href="#semp-2-1-4" target="_blank">§2.1.4</a>) |
| L3 Verification | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> Test Procedures TP-nnn | Systems Engineer; executed by Test Engineer | One procedure per verifiable requirement with concrete inputs and expected outputs | 25 requirements Verified |
| L1′ Increment 2 needs | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/requirements-derivation/NEED-STATEMENTS.md" target="_blank"><code>docs/requirements-derivation/NEED-STATEMENTS.md</code></a> | Product Manager | 15 need statements from the Problem Statement and the design conversation | Recorded; awaiting vetting |
| L2′ Increment 2 derived requirements | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/requirements-derivation/DERIVED-REQUIREMENTS.md" target="_blank"><code>docs/requirements-derivation/DERIVED-REQUIREMENTS.md</code></a> | Systems Engineer | 50 atomic requirements in bands CORE-270+, Knowledge Base (KB)-, BUILD-, OUT-450+ | Draft; 7 of 100 vetting verdicts complete (gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a>) |

<a href="#semp-figure-2-1-1" target="_blank">Figure 2.1-1</a> shows the decomposition and the baseline each level establishes.

```mermaid
flowchart TD
    L0["L0  Challenge announcement<br/>(capability source)"]
    L1["L1  Stakeholder needs SN-1..SN-6<br/>docs/PROJECT_DEFINITION.md"]
    L1p["L1′  Need statements 1..15<br/>docs/requirements-derivation/NEED-STATEMENTS.md"]
    L2["L2  System requirements<br/>docs/RTVM.md (30 items)"]
    L2p["L2′  Derived requirements (50)<br/>DERIVED-REQUIREMENTS.md — gate G3"]
    L3["L3  Test procedures TP-nnn<br/>docs/RTVM.md"]
    SDD["Allocated baseline<br/>docs/SDD.md"]
    PB["Product baseline<br/>tagged release + frozen artifacts"]
    L0 --> L1 --> L2 --> L3
    L0 --> L1p --> L2p --> L3
    L2 -. "PDR" .-> SDD
    L2p -. "PDR-II" .-> SDD
    SDD -. "CDR, SVR/FCA, PCA" .-> PB
```

<a id="semp-figure-2-1-1"></a>*<a href="#semp-figure-2-1-1" target="_blank">Figure 2.1-1</a> Requirements decomposition and technical baselines.*

#### <a id="semp-2-1-2"></a>2.1.2 Requirements Traceability Matrix

The RTVM is maintained as a version-controlled Markdown table in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>, which is the Outline's "tool reference location." <a href="#semp-table-2-1-1" target="_blank">Table 2.1-1</a> is a summary extract at this revision; the RTVM is authoritative. "Change expected" marks requirements whose text is expected to change because of an open sponsor question or a queued amendment, which is the Outline's criterion for flagging requirements for modular-design attention.

<a id="semp-table-2-1-1"></a>*<a href="#semp-table-2-1-1" target="_blank">Table 2.1-1</a> Requirements Traceability Matrix (summary extract, 2026-09-15)*

| Req ID | Requirement (abbreviated) | Source | Verification | Status | Change expected |
| --- | --- | --- | --- | --- | --- |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-ui-001" target="_blank">UI-001</a> | Single-invocation container entrypoint, no prompts | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-5" target="_blank">SN-5</a> | Test | Verified | No |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-100" target="_blank">DATA-IN-100</a> | Ingest SOW/PWS/CDRL/sources-sought/open text; PDF, DOCX, TXT | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-6" target="_blank">SN-6</a> | Test | Verified | Yes — GFI vocabulary (Increment 2) |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-110" target="_blank">DATA-IN-110</a> | Malformed file skipped and reported, batch continues | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-5" target="_blank">SN-5</a> | Test | Verified | No |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-120" target="_blank">DATA-IN-120</a> | Extension points for new document types and components | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-4" target="_blank">SN-4</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-5" target="_blank">SN-5</a> | Demonstration | Verified | No |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-200" target="_blank">CORE-200</a> | Non-LLM clustering by shared requirement content | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-6" target="_blank">SN-6</a> | Test | Verified | Yes — singleton fix (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a>) |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-210" target="_blank">CORE-210</a> | Same top-5 in ≥95% of runs | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-2" target="_blank">SN-2</a> | Test | Verified | No |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> | N=20 set completes ≤30 min at 1 core / 2 GB | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-1" target="_blank">SN-1</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-2" target="_blank">SN-2</a> | Test | Approved | Yes — design-target note (<a href="#semp-2-1-4" target="_blank">§2.1.4</a>) |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a> | Completes at 1c/2GB, 4c/8GB, 8c/16GB | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-2" target="_blank">SN-2</a> | Test | Approved | No |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a> | Zero Large Language Model (LLM) calls or libraries on the core path | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-2" target="_blank">SN-2</a> | Inspection | Verified | No |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a> | Optional LLM step <50k tokens, allowlisted targets only | SN-2, SN-3 | Test | Verified | No |
| CORE-260 | Alternative approach compared and documented | SN-1, SN-2 | Analysis | Verified | No |
| DATA-OUT-300 | Ranked candidate list with score and contributing documents | SN-1, SN-6 | Test | Verified | Yes — evidence record (Increment 2) |
| DATA-OUT-310 | Persisted result set, if a database is used | SN-6 | Test | Withdrawn | — |
| OUT-400 | Visualization of the method | SN-6 | Test | Verified | No |
| OUT-410 | Visualization of the results | SN-6 | Test | Verified | No |
| OUT-420 | Summary performance metric with raw counts | SN-1, SN-6 | Test | Verified | Yes — Mean Reciprocal Rank (MRR) / Recall@5 (§2.1.4) |
| OUT-430 | Written validation methodology | SN-5, SN-6 | Inspection | Verified | No |
| OUT-440 | One output bundle with manifest | SN-1, SN-5, SN-6 | Test | Verified | No |
| NFR-500 | All dependencies bundled; nothing fetched at run time | SN-3 | Test | Verified | No |
| NFR-510 | Zero outbound connections by default; allowlist only when LLM enabled | SN-3 | Test | Verified | No |
| NFR-520 | Independent stateless replicas reproduce the same top 5 | SN-2 | Test | Verified | Yes — throughput demonstration (§2.1.4) |
| NFR-530 | Never provisions above 8 cores / 16 GB | SN-2 | Inspection | Verified | No |
| DELIV-900 | Full C# source, buildable from a clean clone | SN-4 | Inspection | Verified | No |
| DELIV-910 | `.sln`/`.csproj` open in Visual Studio; plain `net9.0` | SN-4 | Demonstration | Approved | No |
| DELIV-920 | Every third-party dependency justified | SN-4 | Inspection | Verified | Yes — license column (§2.1.4) |
| DELIV-930 | Build/run documentation sufficient for a first-time reader | SN-5 | Demonstration | Verified | No |
| DELIV-940 | Maintainer extension walkthrough | SN-4, SN-5 | Inspection | Verified | No |
| DELIV-950 | Database schema and Extract, Transform, Load (ETL) documentation, if a database is used | SN-6 | Inspection | Withdrawn | Yes — narrow reopen for the knowledge base (§2.1.4) |
| DELIV-960 | SETR artifact list reconciled to pipeline artifacts | SN-4 | Inspection | Verified | Yes — cites 4355.19D; superseded by 19E (§3.2.13) |
| DELIV-970 | Algorithm, dependency, and deployment documents are discrete | SN-5 | Inspection | Verified | Yes — reform-instrument citations (§2.1.4) |

#### <a id="semp-2-1-3"></a>2.1.3 Cybersecurity, survivability, and resilience requirements

The requirements source contains no Risk Management Framework control set, no cyber-survivability endorsement, and no System Survivability Key Performance Parameter (KPP); it contains the Impact Level 4 (IL4) deployment constraint and the prohibition on external services. The program derived the following cyber requirements from that constraint, and they trace to <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-3" target="_blank">SN-3</a> in the RTVM: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-500" target="_blank">NFR-500</a> (no run-time fetch), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-510" target="_blank">NFR-510</a> (no egress by default; allowlist only), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a> (no LLM library on the core path), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a> (allowlisted targets only). Operational resilience is addressed by <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-110" target="_blank">DATA-IN-110</a> (a bad input does not abort the run) and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-210" target="_blank">CORE-210</a> (determinism). No Mission-Based Cyber Risk Assessment has been performed; the system holds no mission data, exposes no network service, and runs as a batch process under the Government's own accreditation boundary. An authorization to operate is a Government action at deployment and is outside the challenge's scope (<a href="#semp-2-6" target="_blank">§2.6</a>).

#### <a id="semp-2-1-4"></a>2.1.4 Requirements change control and queued amendments

A change to RTVM requirement text, verification method, or status class is a Class I change (<a href="#semp-3-2-10" target="_blank">§3.2.10</a>) and requires Systems Engineer proposal and Principal approval. Six amendments were agreed with the Principal on 2026-09-15 and are queued, not applied, so that they enter the RTVM with the Increment 2 vetted requirements at SRR-II rather than by hand-edit of Verified rows. They are listed in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> ("RTVM amendments riding with <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a>") and summarized here.

| Item | Amendment | Review at which it enters |
| --- | --- | --- |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a> | Reopen narrowly: document the vehicle knowledge-base schema and the build-time FPDS-to-KB pipeline as schema-and-ETL documentation | SRR-II |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> | Metric becomes MRR and Recall@5, labeled "agreement with historical practice" | SRR-II |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-920" target="_blank">DELIV-920</a> | Add a license column; license compatibility with permanent Government access is part of the justification | SRR-II |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-970" target="_blank">DELIV-970</a> | Each claimed benefit cites a named acquisition-reform instrument | SRR-II |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a> / <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> | Demonstrate throughput improvement under replication, not only result invariance | SRR-II |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> | Add a design-target note: the practical Demo Day budget is minutes, because the timed run may sit inside the 30-minute presentation | SRR-II |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-960" target="_blank">DELIV-960</a> | Replace the 4355.19D citation with 4355.19E and point the requirement at <a href="#semp-3-2-13" target="_blank">§3.2.13</a> of this SEMP | SRR-II |
| UI-010 (new) | Add an Increment 2, non-scored requirement: an operator application shall render an output bundle produced by the baseline command-line interface (CLI) without recomputing any result, so the two can never disagree; the application is a separate configuration item that consumes the bundle read-only and never enters the product baseline of the scored deliverable. Principal's direction 2026-09-17: option 1 of the interface options (a manager application wrapping the container), built so the core assemblies remain hostable by a service mode later | SRR-II |

#### <a id="semp-2-1-5"></a>2.1.5 System safety in requirements

System safety engineering principles were examined and are not part of any requirement. Justification: the system is a decision-support tool that reads documents and writes files. It controls no equipment, operates no hardware, and its output is advisory to a human approval chain. No hazard to personnel, equipment, or the environment can result from its operation or failure.

### <a id="semp-2-2"></a>2.2 Architectures and Interface Control

#### <a id="semp-2-2-1"></a>2.2.1 Architecture products

The architecture is documented in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> (Increment 1, allocated baseline) and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> (Increment 2, proposed). <a href="#semp-table-2-2-1" target="_blank">Table 2.2-1</a> lists the planned suite and the status of each product. The program produces no JCIDS architecture viewpoints because there is no JCIDS document and no external system to integrate with.

<a id="semp-table-2-2-1"></a>*<a href="#semp-table-2-2-1" target="_blank">Table 2.2-1</a> Architecture products*

| Product | Kind | Location | Status | Relationship to requirements |
| --- | --- | --- | --- | --- |
| Block definition diagram, pipeline components | Systems Modeling Language (SysML) BDD (Mermaid) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> §Architecture | Complete, baselined at PDR | Allocates each RTVM family to an assembly; makes <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a> a dependency-graph inspection |
| Activity diagram, pipeline run | SysML activity (Mermaid) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> §Architecture | Complete | Records <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-110" target="_blank">DATA-IN-110</a> and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a> branch points |
| Sequence diagram, build order | Unified Modeling Language (UML) sequence (Mermaid) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a> | Complete | Orders the eight build steps by dependency |
| Use case diagram | — | — | Not produced, by decision | One actor, one interaction; recorded in the Software Design Description (SDD) |
| Interface Control Document | — | — | Not produced, by decision | Interfaces specified in <a href="#semp-2-2-2" target="_blank">§2.2.2</a> and the SDD; no second system builds against them |
| Build/Ship/Run pipeline description | Structured prose with decision table | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> | Proposed; enters the SDD at gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> | Increment 2 architecture: build-time fit, frozen artifacts, run-time evaluation |
| Vehicle knowledge-base schema | Tabular schema with provenance by reference | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/KB_SCHEMA.md" target="_blank"><code>docs/KB_SCHEMA.md</code></a>; artifact under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Output/Resources/kb/" target="_blank"><code>src/Naadap.Output/Resources/kb/</code></a> | Complete (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a>, 2026-09-17); 484 rows, 87.3% FY2025 order coverage | Reopened <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a>; need statements 9, 11, 15 |

#### <a id="semp-2-2-2"></a>2.2.2 Interfaces and dependencies

<a id="semp-table-2-2-2"></a>*<a href="#semp-table-2-2-2" target="_blank">Table 2.2-2</a> Interface register*

| Interface | Direction | Specification | Controlled by |
| --- | --- | --- | --- |
| Input document directory | Inbound to container | Files in PDF, DOCX, or plain text; any name; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> §Volumes | SDD Build & Toolchain Conventions |
| Output bundle | Outbound from container | `manifest.json` plus candidate list, two visualizations, metric, methodology pointer, skipped-file list (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-440" target="_blank">OUT-440</a>); `RunManifest` record in SDD Coding Standards | SDD Data Architecture |
| Command line | Inbound | `--input <dir> --output <dir>`; optional LLM configuration (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-ui-001" target="_blank">UI-001</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Cli/" target="_blank"><code>src/Naadap.Cli</code></a> |
| Optional LLM endpoint | Outbound, disabled by default | Hypertext Transfer Protocol Secure (HTTPS) to an address on the configured USN-approved allowlist only (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-510" target="_blank">NFR-510</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.LlmStep/" target="_blank"><code>src/Naadap.LlmStep</code></a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> |
| Container resource envelope | Environmental | 1 core / 2 GB baseline; never above 8 cores / 16 GB (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-530" target="_blank">NFR-530</a>) | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docker-compose.yml" target="_blank"><code>docker-compose.yml</code></a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> |
| Frozen model artifacts (Increment 2) | Build-time output, run-time input | Coefficient vector, feature vocabulary, knowledge base, labeling-function registry, `context.jsonld`, SHA-256 manifest; checked at startup | Build pipeline; <a href="#semp-3-2-10" target="_blank">§3.2.10</a> |
| USAspending bulk archive (Increment 2) | Inbound to build pipeline only, never to the container | `FY{yyyy}_097_Contracts_Full_{yyyymmdd}.zip`; filter `awarding_sub_agency_code == 1700`; six NAVAIR office codes; `parent_award_id_piid` as label | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/G1-FINDINGS.md" target="_blank"><code>docs/research/g1-fpds/G1-FINDINGS.md</code></a> |
| GFI (Increment 2) | Inbound to development only | Handled per <a href="#semp-3-2-11" target="_blank">§3.2.11</a>; never committed to the repository | Principal |

There are no temporary or mission-phase interfaces. There are no mechanical, electrical, or thermal interfaces.

### <a id="semp-2-3"></a>2.3 Specialty Engineering

Each specialty area in the Systems Engineering (SE) Guidebook (2022) was examined. <a href="#semp-table-2-3-1" target="_blank">Table 2.3-1</a> records the disposition. Areas marked applicable are planned in the section cited.

<a id="semp-table-2-3-1"></a>*<a href="#semp-table-2-3-1" target="_blank">Table 2.3-1</a> Specialty engineering disposition*

| Specialty area | Disposition | Rationale or planning section |
| --- | --- | --- |
| Software engineering | Applicable | <a href="#semp-3-2-8" target="_blank">§3.2.8</a> |
| Cybersecurity / system security engineering | Applicable | <a href="#semp-2-1-3" target="_blank">§2.1.3</a>, <a href="#semp-3-2-12" target="_blank">§3.2.12</a> |
| Technical data and data rights | Applicable | <a href="#semp-3-2-11" target="_blank">§3.2.11</a> |
| Configuration management | Applicable | <a href="#semp-3-2-10" target="_blank">§3.2.10</a> |
| Human systems integration | Applicable, limited | <a href="#semp-3-2-5" target="_blank">§3.2.5</a> |
| Reliability and maintainability | Not applicable as an engineering program | <a href="#semp-3-2-3" target="_blank">§3.2.3</a> |
| Manufacturing and quality | Not applicable (manufacturing); quality handled as Software Quality Assurance (SQA) | <a href="#semp-3-2-4" target="_blank">§3.2.4</a>, <a href="#semp-3-2-8-3" target="_blank">§3.2.8.3</a> |
| System safety | Not applicable | <a href="#semp-2-1-5" target="_blank">§2.1.5</a>, <a href="#semp-3-2-6" target="_blank">§3.2.6</a> |
| Corrosion prevention and control | Not applicable | <a href="#semp-3-2-7" target="_blank">§3.2.7</a> |
| Environment, safety, and occupational health | Not applicable | No physical product, emissions, or field operations |
| Electromagnetic environmental effects | Not applicable | No hardware |
| Survivability (kinetic, Chemical, Biological, Radiological, and Nuclear (CBRN)) | Not applicable | <a href="#semp-2-5" target="_blank">§2.5</a>, <a href="#semp-table-2-5-1" target="_blank">Table 2.5-1</a> |
| Producibility, Diminishing Manufacturing Sources and Material Shortages (DMSMS), parts management | Not applicable (hardware sense); software obsolescence handled in <a href="#semp-3-2-8-4" target="_blank">§3.2.8.4</a> | <a href="#semp-2-5" target="_blank">§2.5</a> |
| Intelligence / Life-cycle Mission Data Plan | Not applicable | <a href="#semp-2-5" target="_blank">§2.5</a> |
| Item Unique Identification | Not applicable | <a href="#semp-appendix-b" target="_blank">Appendix B</a> |

### <a id="semp-2-4"></a>2.4 Modeling Strategy

The Outline's three tiers are model-supported, model-integrated, and model-centric. NAADAP is **model-supported**. The reasons, stated plainly so the claim is not read as more than it is:

- The requirements model is the RTVM, a text table under version control. It is not held in a SysML tool and is not executable.
- The architecture models are SysML-notation diagrams (BDD, activity) authored in Mermaid and rendered from the repository. They are the source of the design decisions they show, not illustrations drawn after the fact, and they are changed by pull request like code. They are not linked by tooling to the requirements table; the link is by identifier in the diagram text.
- The authoritative source of truth (ASoT) is the `main` branch of the GitHub repository. There is no separate model repository.
- The product's own analytical models (the Term Frequency–Inverse Document Frequency (TF-IDF) clustering model, and in Increment 2 the conditional logit coefficient vector and the vehicle knowledge base) are engineering artifacts of the system, not systems-engineering models of it. They are configuration items (<a href="#semp-3-2-10" target="_blank">§3.2.10</a>) and are listed in <a href="#semp-appendix-e" target="_blank">Appendix E</a>, <a href="#semp-table-e-4" target="_blank">Table E-4</a>, so that "model" is not used ambiguously.

The program does not plan to move to a model-integrated tier within Phases 2 and 3. The cost of a SysML tool chain is not justified for a single-process, single-container batch tool with one actor. If an Other Transaction agreement follows and the Government's SEP requires model-integrated delivery, this section is revised at the realignment in <a href="#semp-1-5" target="_blank">§1.5</a>. <a href="#semp-appendix-e" target="_blank">Appendix E</a> holds the Digital Engineering Implementation Plan.

### <a id="semp-2-5"></a>2.5 Design Considerations

Every design consideration in Department of Defense Instruction (DoDI) 5000.88 and the Outline's <a href="#semp-table-2-5-1" target="_blank">Table 2.5-1</a> was examined. <a href="#semp-table-2-5-1" target="_blank">Table 2.5-1</a> uses the Outline's columns. "Contractual requirements" is given as the RTVM identifier that carries the consideration into the product, since there is no CDRL yet.

<a id="semp-table-2-5-1"></a>*<a href="#semp-table-2-5-1" target="_blank">Table 2.5-1</a> Design Considerations*

| Name (reference) | Cognizant organization | Certification documentation | Contractual requirement (RTVM) | How the program addresses it |
| --- | --- | --- | --- | --- |
| Modular Open Systems Approach (DoDI 5000.88 §3.4.a.(3).(h)) | Solutions Architect | None required | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-120" target="_blank">DATA-IN-120</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-940" target="_blank">DELIV-940</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a> | Six assemblies with one-way references; the core assembly has zero third-party dependencies; new document types and clustering components attach through two documented interfaces (`IDocumentParser`, `IClusteringComponent`) without editing dispatch; the optional LLM step and the alternative-approach harness are separate assemblies never referenced by the core. Key interfaces: <a href="#semp-2-2-2" target="_blank">§2.2.2</a>. Reference standards: .NET assembly boundaries; Open Container Initiative (OCI) container image; JSON manifest. Requirements expected to change (<a href="#semp-table-2-1-1" target="_blank">Table 2.1-1</a>) are isolated in Ingestion and Output, not the core. |
| Digital ecosystem (DoDI 5000.88 §3.4.a.(3).(m)) | Systems Engineer | None | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-900" target="_blank">DELIV-900</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-930" target="_blank">DELIV-930</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-440" target="_blank">OUT-440</a> | Repository as ASoT; every run emits a manifest-indexed bundle that is itself a digital artifact of the run; Increment 2 adds hashed, frozen model artifacts so a run is reproducible from the repository and the manifest alone. <a href="#semp-appendix-e" target="_blank">Appendix E</a>. |
| System security engineering / cybersecurity (DoDI 5000.83, 5000.88) | Software Engineer; reviewed by Systems Engineer | None at Phase 2; Government Authorization to Operate (ATO) at deployment | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-500" target="_blank">NFR-500</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-510" target="_blank">NFR-510</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-530" target="_blank">NFR-530</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a> | No network service; no egress by default; dependencies pinned and justified; <a href="#semp-3-2-12" target="_blank">§3.2.12</a>. |
| Software (DoDI 5000.87 as reference, not the pathway) | Software Engineer | None | All CORE, DATA-IN, OUT | <a href="#semp-3-2-8" target="_blank">§3.2.8</a>. |
| Diminishing manufacturing sources and material shortages | — | — | — | Not applicable: no hardware. Software obsolescence is in <a href="#semp-3-2-8-4" target="_blank">§3.2.8.4</a>. |
| Parts management | — | — | — | Not applicable: no parts. |
| Intelligence / Life-cycle Mission Data Plan | — | — | — | Not applicable: the system consumes acquisition documents, not intelligence mission data. |
| Chemical, biological, radiological, and nuclear survivability | — | — | — | Not applicable: no fielded materiel. |
| Reliability and maintainability | — | — | — | Not applicable as R&M engineering; <a href="#semp-3-2-3" target="_blank">§3.2.3</a>. |
| Human systems integration | Product Manager | None | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-400" target="_blank">OUT-400</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-410" target="_blank">OUT-410</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-out-300" target="_blank">DATA-OUT-300</a> | <a href="#semp-3-2-5" target="_blank">§3.2.5</a>. |
| Anti-counterfeiting and supply chain | Software Engineer | None | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-920" target="_blank">DELIV-920</a> | Two NuGet packages, pinned by version, sourced from nuget.org, licenses recorded; `dependency-check.yml` workflow. |
| Accessibility (Section 508) | — | — | — | Not applicable: no user interface; output is files an evaluator opens in tools of their choosing. |
| Environment, safety, and occupational health | — | — | — | Not applicable. |

### <a id="semp-2-6"></a>2.6 Technical Certifications

<a id="semp-table-2-6-1"></a>*<a href="#semp-table-2-6-1" target="_blank">Table 2.6-1</a> Certifications*

| Certification | Required for Phase 2 | Required for deployment | Authority | Plan |
| --- | --- | --- | --- | --- |
| Authorization to Operate in an IL4 environment | No | Yes, Government action | Government authorizing official for the hosting enclave | The program delivers a container with no egress, all dependencies bundled, and a dependency inventory, which is the evidence an assessor would ask a vendor for. The program does not and cannot obtain an ATO for the challenge. |
| Airworthiness, flight clearance, weapon safety, EMI/EMC, spectrum | No | No | — | Not applicable. |
| Software build attestation | Internal | Internal | Systems Engineer | Increment 2: SHA-256 manifest of frozen artifacts checked at container start (<a href="#semp-3-2-10" target="_blank">§3.2.10</a>). |

---

## <a id="semp-3"></a>3 Program Technical Management

### <a id="semp-3-1"></a>3.1 Technical Planning

#### <a id="semp-3-1-1"></a>3.1.1 Technical schedule

The program has no Integrated Master Plan or Integrated Master Schedule in the Department of Defense (DoD) sense and no earned-value system; the challenge is unfunded until a prize is awarded. The schedule instrument is the GitHub issue list, one issue per RTVM feature group (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a>, issues #5 through #12), plus the gate list below for Increment 2. <a href="#semp-table-3-1-1" target="_blank">Table 3.1-1</a> gives the event schedule. Two sponsor dates conflict (Risk <a href="#semp-r-2" target="_blank">R-2</a>); the program plans against the earlier set until Tech Grove answers.

<a id="semp-table-3-1-1"></a>*<a href="#semp-table-3-1-1" target="_blank">Table 3.1-1</a> Technical event schedule*

| Event | Increment | Date | Basis | Status |
| --- | --- | --- | --- | --- |
| Program start; Project Definition confirmed (Initial Technical Review (ITR) equivalent) | 1 | 2026-09-03 | First commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/869f205" target="_blank"><code>869f205</code></a> | Held |
| SRR-I: RTVM approved | 1 | 2026-09-03 | RTVM issue closed | Held |
| PDR: SDD approved (SFR, ASR folded in) | 1 | 2026-09-03 | SDD issue closed | Held |
| CDR: Implementation Plan and code structure approved (IRR folded in) | 1 | 2026-09-03 | Implementation Plan issue; issue #5 merged | Held |
| TRR, per feature issue | 1 | 2026-09-03 to 2026-09-14 | `status:ready-for-test` transitions | Held, eight times |
| Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a>: FPDS premise check | 2 | 2026-09-15 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/G1-FINDINGS.md" target="_blank"><code>docs/research/g1-fpds/G1-FINDINGS.md</code></a> | Complete: pass on volume, fail on catalog coverage |
| Phase 1 questionnaire submitted | — | ≤ 2026-09-20 (target) | Principal | In preparation |
| Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a>: singleton-cohesion fix | 1 | 2026-09-17 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> | Complete: singletons score 0.0; 80 tests pass; comparison re-run |
| Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a>: vehicle knowledge base, data-derived and family-grouped | 1 | 2026-09-17 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> design consequence 1 | Complete: 484 rows, 87.3% coverage; matcher and evidence record implemented (commit 7783c39) |
| Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a>: design accepted into the SDD | 1 | 2026-09-17 | Solutions Architect | Complete: SDD block diagram, data architecture, and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a> reopen recorded; core-vs-alternative choice left as-is for Increment 1 (see <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank"><code>docs/ALGORITHM_COMPARISON.md</code></a>) |
| SVR-1 / FCA-1: full regression against the RTVM; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-910" target="_blank">TP-910</a> Windows check | 1 | 2026-09-17 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md" target="_blank"><code>docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md</code></a> | **Exited 2026-09-17.** Principal signed the record and accepted commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a> as the Increment 1 product baseline; the submission tag is authorized (PCA-1 entry criterion met) |
| PCA-1: clean-clone build of the tagged package | 1 | 2026-09-21 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| Phase 2 submission (Increment 1 delivered) | 1 | **2026-09-22** | Challenge announcement; client direction 2026-09-17 | Planned |
| Phase 2 revision window: Government findings, corrections, omissions executed and resubmitted | 1 | by 2026-10-02 | Client direction 2026-09-17 (the announcement's second date set is the resubmission deadline, the Navy's usual SETR practice) | Planned |
| Semifinalist notification | — | 2026-10-26 | Challenge announcement | Sponsor action |
| Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a>: vetting of the 50 derived requirements | 2 | after Phase 2 submission | Workflow `wf_fe14ec90-697`, parked | Open |
| Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g2" target="_blank">G2</a>: outcome-linkage research (the "was it right" channel) | 2 | after Phase 2 submission | Research pass 7 | Open |
| SRR-II: vetted requirements and RTVM amendments approved | 2 | week of 2026-09-28 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| Operator application (UI-010, non-scored): design as a read-only consumer of the output bundle; core assemblies stay a library so a service host can wrap them later | 2 | after SRR-II; not before Phase 2 submission | Principal's direction 2026-09-17; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-operator-interface" target="_blank">design note</a> | Planned |
| PDR-II: knowledge-base schema, build pipeline, model design (SFR folded in) | 2 | week of 2026-10-05 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| RBR, per sprint | 2 | weekly from 2026-10-05 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| CDR-II | 2 | week of 2026-10-19 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| TRR-II | 2 | week of 2026-10-26 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| SVR-2 / FCA-2 | 2 | week of 2026-11-02 | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> | Planned |
| PCA-2; Demo Day materials submitted | 2 | ≤ 2026-11-12 (or earlier if Demo Day is 2026-11-09) | Challenge announcement | Planned |
| Demo Day | 2 | 2026-11-09; 2026-11-19 read as the revision deadline for Demo Day materials, same pattern | Challenge announcement; client direction 2026-09-17 | Sponsor action |

The Increment 1 critical path is <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a> → <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a> → <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> → SVR-1/FCA-1 → PCA-1 → submission. The Increment 2 critical path is <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a> → SRR-II → PDR-II → build pipeline → CDR-II → TRR-II → SVR-2. The ten days between submission and the revision deadline are for Government-directed corrections only, not for new scope.

**Phase 2 package.** The twelve items the sponsor requires, and the artifact that satisfies each. Any omission fails the completeness gate and yields a score of zero.

| # | Item | Artifact |
| --- | --- | --- |
| 1 | Algorithm documentation | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank"><code>docs/ALGORITHM_COMPARISON.md</code></a>; Increment 2 roadmap section per <a href="#semp-1-1" target="_blank">§1.1</a> |
| 2 | Complete codebase | Repository at the submission tag |
| 3 | Docker container | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Dockerfile" target="_blank"><code>Dockerfile</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docker-compose.yml" target="_blank"><code>docker-compose.yml</code></a>; built image |
| 4 | Code packages and deployment instructions | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> |
| 5 | Database schema, if a database is used | Knowledge-base schema (reopened <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a>), or the SDD's no-database decision if <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a> does not land |
| 6 | ETL process documentation, if applicable | Build-time FPDS-to-KB pipeline description, same condition |
| 7 | Visual representation of the analysis method | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-400" target="_blank">OUT-400</a> output in a committed reference-run bundle |
| 8 | Visual representation of the results | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-410" target="_blank">OUT-410</a> output, same bundle |
| 9 | Algorithm performance summary metrics | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> output, same bundle |
| 10 | Description of validation methodology | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a> |
| 11 | Documentation of external dependencies | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a> |
| 12 | Required technical and supporting documentation | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/MAINTAINER_GUIDE.md" target="_blank"><code>docs/MAINTAINER_GUIDE.md</code></a>, this SEMP, the SETR review records under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a>, and the data item list <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md" target="_blank"><code>docs/setr/CDRL.md</code></a> naming the DID each artifact follows |

#### <a id="semp-3-1-2"></a>3.1.2 Technical maturity assessment

No Technology Readiness Assessment is required for a non-ACAT effort and none is performed. Maturity is assessed on two axes.

*Requirements maturity (RTVM status counts, 2026-09-15).* 25 Verified; 3 Approved (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a>); 2 Withdrawn. <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a> await a timed run at the resource tiers; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a> awaits the Windows/Visual Studio run, whose workflow was added to the repository on 2026-09-14 and whose result is not yet recorded in the RTVM.

*Technology maturity.* Every technique on the Increment 1 path (TF-IDF, cosine similarity, single-link clustering at a global threshold) is mature, has been implemented, and has been measured on the reference set (precision@5 of 0.60 against a four-vehicle ground truth; see <a href="#semp-table-3-2-1" target="_blank">Table 3.2-1</a>). Every technique on the Increment 2 path is mature in the literature (McFadden's conditional logit, 1974; MRR and Recall@k) but not implemented in this codebase and not available in Machine Learning (ML).NET, so the implementation is new code (approximately 250 lines by estimate) and is tracked as Risk <a href="#semp-r-8" target="_blank">R-8</a>.

*Critical technologies.* One: deterministic, bit-reproducible ranking under container replication. It is verified for Increment 1 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-210" target="_blank">CORE-210</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a>) and is preserved in Increment 2 by design (globally concave log-likelihood, fixed summation order, fitting done outside the container, only frozen artifacts shipped).

#### <a id="semp-3-1-3"></a>3.1.3 Technical structure and organization

*Work breakdown structure.* The WBS follows the solution's assembly structure so that each element has one owner and one test project.

| WBS | Element | Assembly or location | RTVM allocation |
| --- | --- | --- | --- |
| 1.0 | Ingestion and normalization | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Ingestion/" target="_blank"><code>src/Naadap.Ingestion</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-100" target="_blank">DATA-IN-100</a>/110/120 |
| 2.0 | Core clustering engine | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Core/" target="_blank"><code>src/Naadap.Core</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-200" target="_blank">CORE-200</a>/210/220/230/240 |
| 3.0 | Alternative-approach harness | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Alternative/" target="_blank"><code>src/Naadap.Alternative</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a> |
| 4.0 | Optional LLM step | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.LlmStep/" target="_blank"><code>src/Naadap.LlmStep</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a> |
| 5.0 | Ranking, visualization, metrics, bundle | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Output/" target="_blank"><code>src/Naadap.Output</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-out-300" target="_blank">DATA-OUT-300</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-400" target="_blank">OUT-400</a>/410/420/430/440 |
| 6.0 | Command line and container | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Cli/" target="_blank"><code>src/Naadap.Cli</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Dockerfile" target="_blank"><code>Dockerfile</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-ui-001" target="_blank">UI-001</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-500" target="_blank">NFR-500</a>/510/520/530 |
| 7.0 | Test | `tests/Naadap.*.Tests`, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/tests/fixtures/" target="_blank"><code>tests/fixtures</code></a> | All TP-nnn |
| 8.0 | Documentation and SETR | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/" target="_blank"><code>docs/</code></a> | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-900" target="_blank">DELIV-900</a> through 970 |
| 9.0 | Increment 2: knowledge base, build pipeline, fitted model, evidence record | To be located at PDR-II | Derived requirements, gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a> |

*Organization.* The program is staffed by one human, the Principal, who holds the Program Manager and approval authority, and by Artificial Intelligence (AI) agent personas that execute the engineering roles under the Principal's direction. This is stated so a reviewer does not infer a larger staff. Every artifact is reviewed by the Principal before it is baselined; agents cannot approve their own work, and the Test Engineer role is prevented by a repository script from writing to source or documentation. The roles, their authority, and their interfaces are defined in `.claude/agents/*.md`.

| Role | Holder | Authority | Products |
| --- | --- | --- | --- |
| Program Manager / approval authority | Principal, HoloSim Interactive | Approves baselines, RTVM changes, SEMP updates, submission | Signatures in this document; Phase 1 questionnaire |
| Product Manager | Agent persona | Owns stakeholder needs and priority; sole channel for client questions | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank"><code>docs/PROJECT_DEFINITION.md</code></a>, need statements |
| Systems Engineer (Lead Systems Engineer equivalent) | Agent persona | Owns requirements, RTVM, SDD contributions, test procedures, this SEMP, SETR records | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/" target="_blank"><code>docs/setr/</code></a> |
| Solutions Architect | Agent persona | Owns macro-architecture; resolves architecture escalations | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> architecture decisions |
| Software Engineer | Agent persona | Implements to the SDD | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/" target="_blank"><code>src/</code></a> |
| Test Engineer | Agent persona | Executes TP-nnn; reports pass/fail; independent of implementation | Test results in issues; <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/tests/" target="_blank"><code>tests/</code></a> |
| Continuous Integration / Continuous Delivery (CI/CD) | Agent persona | Merges only compilable, tested code; assigns version tags | Tags `v1.0.n` |
| Government acquisition subject-matter expertise | Skill (<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/" target="_blank"><code>.claude/skills/gov-acquisition-sme</code></a>) with primary sources | Advisory; no approval authority | Reference files; source library |

<a id="semp-table-3-1-2"></a>*<a href="#semp-table-3-1-2" target="_blank">Table 3.1-2</a> Integrated Product Team Details.* The program has one IPT.

| Team name | Chair | Team membership | Role, responsibility, and authority | Products and metrics |
| --- | --- | --- | --- | --- |
| NAADAP Systems Engineering IPT | Systems Engineer | Principal; Product Manager; Solutions Architect; Software Engineer; Test Engineer; CI/CD; acquisition Subject Matter Expert (SME) skill | Plans and conducts the tailored SETR events; maintains the RTVM and the risk register; recommends baselines to the Principal, who approves | RTVM, SDD, this SEMP, review records, Technical Performance Measures (TPMs) in <a href="#semp-table-3-2-1" target="_blank">Table 3.2-1</a>, metrics in <a href="#semp-appendix-c" target="_blank">Appendix C</a> |

*Staffing.* One human at partial availability; agent capacity is bounded by the Principal's weekly usage allowance, which was at 93% on 2026-09-14. This is Risk <a href="#semp-r-8" target="_blank">R-8</a>'s second cause.

### <a id="semp-3-2"></a>3.2 Technical Tracking

#### <a id="semp-3-2-1"></a>3.2.1 Risks, issues, and opportunities

*Process.* Risks, issues, and opportunities are identified top-down at each review (<a href="#semp-3-2-13" target="_blank">§3.2.13</a> entry criteria require an updated register) and bottom-up from any team member at any time by opening a GitHub issue labeled `risk`, `issue`, or `opportunity`. The register of record is this section until the SETR review records are established under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a>, after which each review's record carries the register as of that review. The Systems Engineer owns the register; the Principal approves mitigation plans that change scope or schedule. There is one board, the IPT, which reviews the register at every SETR event and at every RBR.

*Scales.* Likelihood and consequence use the Outline's 5-by-5 matrix with consequence criteria rewritten for a prize challenge, since the Outline's cost criteria refer to an Acquisition Program Baseline this program does not have.

| Level | Likelihood | Consequence: schedule | Consequence: score | Consequence: technical |
| --- | --- | --- | --- | --- |
| 5 | Near certain (>80%) | Misses the submission or Demo Day deadline | Completeness gate failed (score zero) or ≥25 rubric points lost | A stated capability is not delivered |
| 4 | Highly likely (61–80%) | Consumes all margin before a deadline | 15–24 points lost | A verified requirement regresses to Approved |
| 3 | Likely (41–60%) | Slips a SETR event by more than one week | 5–14 points lost | A design target is missed with a workaround |
| 2 | Low likelihood (21–40%) | Slips a SETR event by up to one week | 1–4 points lost | Margin reduced within trade space |
| 1 | Not likely (≤20%) | Absorbed within the sprint | No score effect | Minimal |

<a id="semp-figure-3-2-1"></a>*<a href="#semp-figure-3-2-1" target="_blank">Figure 3.2-1</a> Risk reporting matrix (2026-09-15).* Rows are likelihood 5 (top) to 1; columns are consequence 1 (left) to 5. Cells hold risk identifiers.

| L \ C | 1 | 2 | 3 | 4 | 5 |
| --- | --- | --- | --- | --- | --- |
| 5 | | | | | |
| 4 | | | | <a href="#semp-r-8" target="_blank">R-8</a> | <a href="#semp-r-1" target="_blank">R-1</a> |
| 3 | | <a href="#semp-r-10" target="_blank">R-10</a> | | | <a href="#semp-r-6" target="_blank">R-6</a> |
| 2 | | | <a href="#semp-r-11" target="_blank">R-11</a> | | |
| 1 | | | | | |

High: <a href="#semp-r-1" target="_blank">R-1</a>, <a href="#semp-r-6" target="_blank">R-6</a>, <a href="#semp-r-8" target="_blank">R-8</a>. Moderate: <a href="#semp-r-11" target="_blank">R-11</a>. Low: <a href="#semp-r-10" target="_blank">R-10</a>. Closed 2026-09-17: <a href="#semp-r-2" target="_blank">R-2</a>, <a href="#semp-r-7" target="_blank">R-7</a> (client direction), <a href="#semp-r-3" target="_blank">R-3</a> (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a> delivered), <a href="#semp-r-4" target="_blank">R-4</a> (fixed), <a href="#semp-r-5" target="_blank">R-5</a> (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> passed), <a href="#semp-r-9" target="_blank">R-9</a> (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-910" target="_blank">TP-910</a> passed).

<a id="semp-table-3-2-2"></a>*<a href="#semp-table-3-2-2" target="_blank">Table 3.2-2</a> Risk register*

| ID | Risk (if–then) | L | C | Mitigation | Owner | Trace |
| --- | --- | --- | --- | --- | --- | --- |
| <a id="semp-r-1"></a>R-1 | If Phase 1 approval arrives with no usable days before the Phase 2 deadline, then GFI cannot influence Increment 1 | 4 | 5 | Accepted by design: Increment 1 scores on public documents; GFI tuning is Increment 2 work. Residual: none for Increment 1. | Principal | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-100" target="_blank">DATA-IN-100</a> |
| <a id="semp-r-2"></a>R-2 | If the sponsor's summary-box dates (22 Sep / 9 Nov) rather than its timeline-section dates (2 Oct / 19 Nov) govern, then ten fewer days exist for <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a>–<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> and SVR-1 | — | — | **Closed 2026-09-17.** Client direction: submit 22 Sep; the second date set is the deadline to execute Government-directed revisions and resubmit. Schedule in <a href="#semp-table-3-1-1" target="_blank">Table 3.1-1</a> updated. | Product Manager | <a href="#semp-3-1-1" target="_blank">§3.1.1</a> |
| <a id="semp-r-3"></a>R-3 | If the vehicle knowledge base is built from the curated catalog alone, then two-thirds of NAVAIR's historical orders have no candidate row (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> finding F4: 33% coverage) | — | — | **Closed 2026-09-17.** Knowledge base built from the FPDS parent-PIID list (484 rows: 11 catalog families plus every parent Indefinite-Delivery Vehicle (IDV) with three or more FY2025 orders); coverage 87.3% of FY2025 NAVAIR orders under vehicles against the 80% target. Residual: FY2025 only; last-date-to-order for parent IDVs is a proxy (Increment 2). | Systems Engineer | Need 9; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a> |
| <a id="semp-r-4"></a>R-4 | If singleton clusters keep a cohesion score of 1.0, then eleven of twenty reference documents are scored as perfectly cohesive and the grey-area requirements cannot be verified | — | — | **Closed 2026-09-17.** Fixed: `VehicleRecommender.SingletonScore` = 0.0; unit test pins the ordering; all 80 tests pass. Core precision@5 unchanged (0.60); the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a> alternative rose from 0.40 to 0.80, recorded in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank"><code>docs/ALGORITHM_COMPARISON.md</code></a> and referred to <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> as a Class I decision. | Software Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-200" target="_blank">CORE-200</a> |
| <a id="semp-r-5"></a>R-5 | If the evaluator reads "replication must demonstrably improve performance" literally, then the SDD's independent-replica interpretation earns zero of ten Replicability points | — | — | **Closed 2026-09-17 at SVR-1.** <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> (b): four replicas process four sets in 7.53 s against 29.68 s sequential (3.94×) with byte-identical outputs. | Solutions Architect | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a> |
| <a id="semp-r-6"></a>R-6 | If the Demo Day timed run occurs inside the 30-minute presentation, then a run near the 30-minute ceiling fails the demonstration | 3 | 5 | Confirmed by client direction 2026-09-17: the run is inside the presentation. Design target of minutes (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> note); current reference-20 run completes in under one second of compute; risk retires when <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a> is measured at 1 core / 2 GB and a Demo Day rehearsal is timed | Systems Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> |
| <a id="semp-r-7"></a>R-7 | If "a correct prediction" means a document-to-vehicle pairing rather than a vehicle name, then the output shape scores differently than designed | — | — | **Closed 2026-09-17.** Client direction: points can be awarded for any of the three forms, and a "new strategic vehicle indicated" entry counts when it is the expected answer. Output keeps all three views (vehicle, document-to-vehicle, cluster-to-vehicle) and the new-vehicle mode. | Product Manager | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-out-300" target="_blank">DATA-OUT-300</a> |
| <a id="semp-r-8"></a>R-8 | If Increment 2 (new subsystem, new fitted model, new build pipeline, new extraction layer) is attempted at full SETR rigor within the Phase 3 window with one part-time human and a capped agent allowance, then SVR-2 slips past the materials deadline | 4 | 4 | Two-increment plan; lookup-table fallback for office affinity already computable from `navair_orders_fy2025.csv`; RBR cadence exposes slip weekly. Burn-down: <a href="#semp-figure-3-2-2" target="_blank">Figure 3.2-2</a>. | Principal | <a href="#semp-1-1" target="_blank">§1.1</a> |
| <a id="semp-r-9"></a>R-9 | If a Windows-only Application Programming Interface (API) entered the code base, then <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a> fails at the one-time Visual Studio check | — | — | **Closed 2026-09-17.** <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-910" target="_blank">TP-910</a> passed on the candidate commit on hosted Windows runners (build, test, publish). | CI/CD | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-910" target="_blank">DELIV-910</a> |
| <a id="semp-r-10"></a>R-10 | If a dependency's license does not permit permanent Government use, then containerization does not cure it | 3 | 2 | Current packages: PdfPig (Apache 2.0), DocumentFormat.OpenXml (Massachusetts Institute of Technology, MIT, license), test packages (MIT/Apache); license column queued for <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-920" target="_blank">DELIV-920</a> | Software Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-920" target="_blank">DELIV-920</a> |
| <a id="semp-r-11"></a>R-11 | If the 50 derived requirements enter implementation unvetted, then the project's own rule ("nothing built against unvetted items") is broken | 2 | 3 | Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a> before SRR-II. The abstention question is decided (client direction 2026-09-17): candidates below the evidence floor are recorded in a separate "below evidence floor" section, never suppressed, so the record shows they were considered. CORE-286 is re-derived on that basis at <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a>. | Systems Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a> |

*Issues (realized).*

| ID | Issue | Resolution | Owner |
| --- | --- | --- | --- |
| <a id="semp-i-1"></a>I-1 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> names precision@5 or F1 "against validation ground truth"; a model that learns office habit scores better on that metric while recommending worse | Amend to MRR and Recall@5, "agreement with historical practice," at SRR-II | Systems Engineer |
| <a id="semp-i-2"></a>I-2 | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785B</a> not in hand; ASSIST's document link returns a script redirect | Closed 2026-09-15: the Principal supplied the ASSIST copies of the 2009, A, and B revisions; reconciled at Draft A.1 (<a href="#semp-1-3" target="_blank">§1.3</a>) | Principal |
| <a id="semp-i-3"></a>I-3 | The SDD's SETR mapping cites 4355.19D and 14 events; the current instruction is 19E with 18 events | Superseded by <a href="#semp-3-2-13" target="_blank">§3.2.13</a>; pointer added to the SDD | Systems Engineer |
| <a id="semp-i-4"></a>I-4 | "PEDDAL" in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-4" target="_blank">SN-4</a> has no source | Closed 2026-09-15 as client-specific terminology from a prior project; not pursued | Product Manager |
| <a id="semp-i-5"></a>I-5 | The build-and-test workflow is a template under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/ci/" target="_blank"><code>docs/ci/</code></a> and is not installed in <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.github/workflows/" target="_blank"><code>.github/workflows/</code></a>; no agent role can install it. Increment 1 merges were gated by local build and test results recorded on issues, not by GitHub-hosted Continuous Integration (CI) | Installed 2026-09-16. Reopened 2026-09-17 at SVR-1: the workflow triggers on `main`, `issue-*`, and pull requests, not on the working branch. Closed for Increment 1 the same day by pushing the candidate to `issue-svr1` (runs 35178897168, 35178944747, both green). Filter left as set. | Principal |
| <a id="semp-i-6"></a>I-6 | The <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> extraction scripts (passes 1–4) are not committed; only their outputs are | Closed 2026-09-15: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/g1_extract.py" target="_blank"><code>docs/research/g1-fpds/g1_extract.py</code></a> committed; re-run against the same archive reproduced every committed output byte-for-byte | Systems Engineer |
| <a id="semp-i-7"></a>I-7 | The four sponsor questions in `challenge-brief.md` were open | Closed 2026-09-17 by client direction, recorded there and in <a href="#semp-r-2" target="_blank">R-2</a>, <a href="#semp-r-5" target="_blank">R-5</a>, <a href="#semp-r-6" target="_blank">R-6</a>, <a href="#semp-r-7" target="_blank">R-7</a>: (1) submit 22 Sep, revisions by 2 Oct; (2) any prediction form scores, new-vehicle entries count when expected; (3) "recommending common requirements" means recommending that a cluster be treated as common, and promoting an existing contract to strategic-vehicle status is a third output mode (need 15); (4) the timed run is inside the presentation, and replication means throughput across document sets. If Tech Grove later answers differently, the sponsor's answer governs and these rows reopen. | Product Manager |

*Opportunities.*

| ID | Opportunity | Action | Owner |
| --- | --- | --- | --- |
| <a id="semp-o-1"></a>O-1 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> confirmed FPDS volume is sufficient (6,778 NAVAIR orders under vehicles in FY2025), so the fitted coefficient vector may be feasible within Increment 1 rather than deferred | Decide at <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> on schedule grounds; lookup table remains the fallback | Solutions Architect |
| <a id="semp-o-2"></a>O-2 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> produced a concrete vehicle-promotion candidate list (134 single-office, high-volume Procurement Instrument Identifiers (PIIDs)) | Feeds need statement 15 at SRR-II | Systems Engineer |
| <a id="semp-o-3"></a>O-3 | The "reasons the others were ruled out" record serves experienced contracting officers as a written defense of a determination they already intend to make | Carry into the algorithm documentation and the Demo Day narrative | Product Manager |

<a id="semp-figure-3-2-2"></a>*<a href="#semp-figure-3-2-2" target="_blank">Figure 3.2-2</a> Risk burn-down plans for high risks.* Planned likelihood by event; actual recorded at each review.

| Risk | Now | SVR-1 (2026-09-21) | SRR-II | PDR-II | CDR-II | SVR-2 |
| --- | --- | --- | --- | --- | --- | --- |
| <a href="#semp-r-3" target="_blank">R-3</a> catalog coverage | closed 2026-09-17 at 87.3% | — | — | — | — | — |
| <a href="#semp-r-8" target="_blank">R-8</a> Increment 2 schedule | 4 | 4 | 3 (scope fixed at SRR-II) | 3 | 2 (fit and freeze complete) | 1 |
| <a href="#semp-r-1" target="_blank">R-1</a> | Sponsor-dependent; retires on Phase 1 approval timing | | | | | |
| <a href="#semp-r-6" target="_blank">R-6</a> | 3 | 2 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a> measured at 1 core / 2 GB; rehearsal timed) | 2 | 2 | 1 | 1 |

#### <a id="semp-3-2-2"></a>3.2.2 Technical performance measures

*Selection.* TPMs are the rubric's scored quantities plus the engineering quantities that predict them. Each traces to an RTVM requirement. The Systems Engineer selects and retires TPMs; the Test Engineer measures them; the values are reported at every SETR event and every RBR. A TPM is added when a rubric interpretation is settled by the sponsor and retired when its requirement is Verified and no later change can regress it. No contractual provision attaches to any TPM. Models and artifacts: every TPM value is computed from a committed run bundle or a committed test result, so the reporting artifact is the ASoT, not a slide.

Key performance parameters, key system attributes, and critical technical parameters do not exist for a prize challenge. The rubric's scored quantities are treated as their equivalent and each is covered by a TPM below. Software measures are in <a href="#semp-appendix-c" target="_blank">Appendix C</a>. No Validated Online Lifecycle Threat exists or applies. No critical intelligence parameters exist.

<a id="semp-table-3-2-1"></a>*<a href="#semp-table-3-2-1" target="_blank">Table 3.2-1</a> Technical Performance Measures (2026-09-15)*

Actuals for Increment 1 are recorded under TRR-1, the last feature-level TRR (2026-09-14), because the reviews before it preceded implementation.

| TPM | Category | Responsible | Requirement trace | Rubric equivalent | Goal | Plan / Actual | SRR-I | PDR / CDR | TRR-1 | SVR-1 | SRR-II | CDR-II | SVR-2 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Wall-clock run time, N=20, 1 core / 2 GB (minutes) | Performance | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-220" target="_blank">CORE-220</a> | Runtime, 15 pts | ≤ 30 (score); ≤ 5 (design target) | Plan | — | ≤30 | ≤30 | ≤5 | ≤5 | ≤5 | ≤5 |
| | | | | | | Actual | — | — | not run | 0.12 (7.43 s sandbox); 0.07 (4.38 s Principal's host) | | | |
| Top-5 reproducibility over 20 runs (%) | Determinism | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-210" target="_blank">CORE-210</a> | Replicability, 10 pts | ≥ 95 | Plan | — | 95 | 95 | 100 | 100 | 100 | 100 |
| | | | | | | Actual | — | — | ≥95 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a> pass) | 100 (20/20 byte-identical) | | | |
| Peak resident memory, N=20 (GB) | Performance | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-530" target="_blank">NFR-530</a> | Compute cost, 15 pts | ≤ 2 | Plan | — | ≤2 | ≤2 | ≤2 | ≤2 | ≤2 | ≤2 |
| | | | | | | Actual | — | — | not run | ≤2 GB cap held (peak not sampled) | | | |
| LLM tokens per run, default configuration | Cost | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-250" target="_blank">CORE-250</a> | LLM cost, 10 pts | 0 | Plan | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| | | | | | | Actual | — | — | 0 | 0 | | | |
| Outbound network connections, default configuration | Security | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-510" target="_blank">NFR-510</a> | IL4 criterion | 0 | Plan | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| | | | | | | Actual | — | — | 0 | 0 (`--network none`) | | | |
| Third-party packages referenced by the core assembly | Architecture | Software Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-920" target="_blank">DELIV-920</a> | LLM cost; maintainability | 0 | Plan | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| | | | | | | Actual | — | 0 | 0 | 0 | | | |
| Agreement with ground truth on reference-20 (precision@5 through SVR-1; MRR and Recall@5 thereafter) | Accuracy | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> | Initial technical evaluation, 40 pts | Increment 1: ≥ 0.60; Increment 2: Recall@5 ≥ 0.80 on FPDS holdout | Plan | — | — | 0.60 | 0.60 | — | 0.80 | 0.80 |
| | | | | | | Actual | — | — | 0.60 | 0.60 | | | |
| Knowledge-base coverage of NAVAIR FY2025 orders under vehicles (% by order count) | Data | Systems Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-950" target="_blank">DELIV-950</a> (reopened), need 9 | Initial technical evaluation | ≥ 80 | Plan | — | — | — | 80 | 80 | 90 | 90 |
| | | | | | | Actual | — | — | 33 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a>); 87.3 (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a>, 2026-09-17) | 87.3 | | | |
| Throughput scaling, N replicas on N document sets | Performance | Test Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a> (amended) | Replicability, 10 pts | Wall-clock for N sets ≤ 1.2 × single-set time at N=4 | Plan | — | — | — | meet | meet | meet | meet |
| | | | | | | Actual | — | — | not run | 3.94× (4 sets: 29.68 s → 7.53 s) | | | |
| Automated test methods / active requirements Verified | Verification | Test Engineer | All | Completeness gate | 100% of active requirements | Plan | — | — | — / 25 | 74 / 28 | | | |
| | | | | | | Actual | — | — | 72 / 25 | 90 / 28 (all active) | | | |

"—" means the measure was not yet defined or measurable at that event. Blank cells are future events.

#### <a id="semp-3-2-3"></a>3.2.3 Reliability and maintainability

Not applicable as an R&M engineering program: no hardware, no field failures, no repair concept, no mean-time-between-failure requirement in the source. The Outline's expectation that a program "identify design features and manufacturing processes" for R&M has no object here. Software reliability in the ordinary sense (the tool completes its run on valid input and reports, rather than aborts on, invalid input) is a functional requirement (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-110" target="_blank">DATA-IN-110</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-ui-001" target="_blank">UI-001</a>) and is verified by test. Software maintainability is a deliverable requirement (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-940" target="_blank">DELIV-940</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-120" target="_blank">DATA-IN-120</a>) and is verified by demonstration.

#### <a id="semp-3-2-4"></a>3.2.4 Manufacturing and quality

Manufacturing is not applicable. Quality is software quality assurance, planned in <a href="#semp-3-2-8-3" target="_blank">§3.2.8.3</a>.

#### <a id="semp-3-2-5"></a>3.2.5 Human systems integration

The system has one human role: the contracting or acquisition professional who reads the output bundle. There is no interactive interface (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-5" target="_blank">SN-5</a> excludes one), so the Human Systems Integration (HSI) concerns are the legibility and sufficiency of the output. The requirements are <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-400" target="_blank">OUT-400</a> and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-410" target="_blank">OUT-410</a> (visualizations), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-out-300" target="_blank">DATA-OUT-300</a> (each candidate carries its contributing documents), and in Increment 2 the evidence record (top feature contributions, the labeling functions that fired and where, nearest historical orders, constraint results, FAR 7.107 inputs, small-business note, and the reasons each eliminated vehicle was ruled out). The design principle is that the record must let an experienced contracting officer confirm what they already believe and let a new one learn why; the tool identifies, the human decides. Manpower, personnel, training, and habitability domains are not applicable. <a href="#semp-appendix-d" target="_blank">Appendix D</a> describes the concept of operations.

#### <a id="semp-3-2-6"></a>3.2.6 System safety

Not applicable. See <a href="#semp-2-1-5" target="_blank">§2.1.5</a>. No hazard analysis, safety assessment report, or safety release is produced.

#### <a id="semp-3-2-7"></a>3.2.7 Corrosion prevention and control

Not applicable. No materiel.

#### <a id="semp-3-2-8"></a>3.2.8 Software engineering

##### <a id="semp-3-2-8-1"></a>3.2.8.1 Overview

The product is entirely software. <a href="#semp-table-3-2-10" target="_blank">Table 3.2-10</a> gives the scope in the Outline's terms.

<a id="semp-table-3-2-10"></a>*<a href="#semp-table-3-2-10" target="_blank">Table 3.2-10</a> Software Development Scope (2026-09-15)*

| Attribute | Value |
| --- | --- |
| Scope | NAADAP batch pipeline, Increments 1 and 2 |
| Size | 2,919 lines of C# in 66 source files under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/" target="_blank"><code>src/</code></a>; 1,807 lines of test code; Increment 2 estimated at 1,500 to 2,500 additional lines (knowledge base, build pipeline, conditional logit, evidence record) |
| Peak staff | One human (part time); agent personas as listed in <a href="#semp-3-1-3" target="_blank">§3.1.3</a> |
| Number of software suppliers | Zero subcontractors. Two third-party open-source packages (PdfPig, DocumentFormat.OpenXml), ingestion only. |
| Methodology | Agile, one issue per feature group; continuous integration on pull request; sprints of one week in Increment 2 |
| Duration | Increment 1: 2026-09-03 to 2026-09-22; Increment 2: 2026-09-28 to 2026-11-12 |
| Number of computer software configuration items | 6 assemblies under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/" target="_blank"><code>src/</code></a> (<a href="#semp-table-3-2-3" target="_blank">Table 3.2-3</a>); 6 test assemblies |
| Software development cost | Not tracked in dollars; unfunded challenge entry. Effort is bounded by the Principal's time and agent allowance. |
| Number of builds | Increment 1: 8 tagged releases to date (`v1.0.17` through `v1.0.82`); one submission tag planned. Increment 2: one tag per RBR. |

<a id="semp-table-3-2-3"></a>*<a href="#semp-table-3-2-3" target="_blank">Table 3.2-3</a> Computer software configuration items*

| CSCI | Assembly | Third-party references | May reference |
| --- | --- | --- | --- |
| Ingestion | `Naadap.Ingestion` | PdfPig, DocumentFormat.OpenXml | — |
| Core | `Naadap.Core` | None | — |
| Alternative | `Naadap.Alternative` | None at present; may reference a retrieval or LLM client | Core (read-only comparison) |
| LlmStep | `Naadap.LlmStep` | None (Base Class Library (BCL) `HttpClient`) | — |
| Output | `Naadap.Output` | None (BCL `System.Text.Json`) | Core, Ingestion |
| Cli | `Naadap.Cli` | None | Ingestion, Core, Output, LlmStep. Never Alternative. |

##### <a id="semp-3-2-8-2"></a>3.2.8.2 Software planning

*Methodology, tools, environments.* Agile. Each RTVM feature group is a GitHub issue; each issue is a branch, a pull request, a build-and-test run, a Test Engineer verdict, and a merge by the CI/CD role, which tags the result. Development environment: Ubuntu, .NET Software Development Kit (SDK) 9.0.3xx, `dotnet build` and `dotnet test` over <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a>; no other build system. Test framework: xunit 2.9 with coverlet for coverage. Target environment: Linux container from `mcr.microsoft.com/dotnet/runtime:9.0` built by a two-stage <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Dockerfile" target="_blank"><code>Dockerfile</code></a>; nothing restored at run time. Packaging for the maintainer: the same `.sln`/`.csproj` opened in Visual Studio on Windows without conversion. Pipeline tools: GitHub Actions for the two workflows installed in <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.github/workflows/" target="_blank"><code>.github/workflows/</code></a> (`windows-verification.yml`, added 2026-09-14; `dependency-check.yml`). The ordinary build-and-test gate exists as a project-agnostic template at <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ci/build-and-test.yml" target="_blank"><code>docs/ci/build-and-test.yml</code></a> and is **not installed**: no agent role can write to <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.github/workflows/" target="_blank"><code>.github/workflows/</code></a>, and the Principal has not yet copied it. Through Increment 1, build and test ran on the Test Engineer's and CI/CD role's local `dotnet build` and `dotnet test` invocations, recorded on each issue (Issue <a href="#semp-i-5" target="_blank">I-5</a>). Degree of automation: dependency check and Windows verification are automated in GitHub; build and unit test are automated locally and will be automated in GitHub when the template is installed; the timed resource-tier runs (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a>) are scripted but launched by hand because they require a constrained container.

*Estimation.* Effort is estimated per RTVM requirement, in requirements rather than story points, at issue creation, and measured as requirements Verified per week (<a href="#semp-appendix-c" target="_blank">Appendix C</a>, velocity). Increment 1 delivered 25 Verified requirements in twelve days. Increment 2 is estimated at 50 derived requirements less those Withdrawn at <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a>; at Increment 1's rate that is 24 working days, which fits the window only if <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a> removes a substantial number. That arithmetic is Risk <a href="#semp-r-8" target="_blank">R-8</a>'s basis.

*Capability roadmap.* Increment 1 is the strategy illustrated: curated knowledge base, hard constraints as rules, scope matching by TF-IDF cosine, office affinity as a counted lookup table, the singleton fix, the full evidence record including eliminations. Increment 2 is the strategy realized: coefficient vector fitted on FPDS replacing the lookup table, labeling functions as validated features, extraction re-tuned on GFI, the outcome-proxy channel, MRR and Recall@5 on FPDS holdouts. Build time is not yet measured and is recorded at SVR-1; the reference-20 run completes in under one second of compute on the development machine. Build cycle: on every pull request; tag on every merge.

*Sustainment strategy.* The Government receives full source, a maintainer guide with two worked extension examples, a dependency inventory with licenses, and a deterministic build. Sustainment beyond Phase 3 is the subject of the follow-on agreement; the design decisions that make it cheap (no database, no service, no network, two dependencies, one process) are recorded in the SDD. Management review interval: each SETR event.

*Metrics.* <a href="#semp-appendix-c" target="_blank">Appendix C</a>.

*Integration, test, and release.* One integration point, the `main` branch. Release is a git tag plus a built image. There is no continuous authorization to operate; the container is delivered to the Government, not operated by the contractor.

*Software risks.* <a href="#semp-r-8" target="_blank">R-8</a>, <a href="#semp-r-9" target="_blank">R-9</a>, <a href="#semp-r-10" target="_blank">R-10</a> in <a href="#semp-table-3-2-2" target="_blank">Table 3.2-2</a> (<a href="#semp-r-4" target="_blank">R-4</a> closed 2026-09-17).

*Critical software requirements.* <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-210" target="_blank">CORE-210</a> (determinism), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a> (no LLM on the core path), <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-510" target="_blank">NFR-510</a> (no egress). Each is verified by a test that a regression would fail, and each is a TPM.

*Commercial Off-the-Shelf (COTS), Government Off-the-Shelf (GOTS), and reuse.* Two commercial open-source libraries, both pure managed code, both confined to Ingestion. No GOTS. No reuse of prior HoloSim code. ML.NET was evaluated and rejected for Increment 2: it does not implement conditional logit, and its LightGBM binding lacks the determinism control the underlying library requires. ML.NET FastTree is permitted as an offline benchmark only.

*Deliverables and intellectual property.* The repository is public under the MIT license (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/LICENSE" target="_blank"><code>LICENSE</code></a>). The Government receives unrestricted access to everything in it. GFI-derived tuning parameters are handled per <a href="#semp-3-2-11" target="_blank">§3.2.11</a>. Rights in a follow-on agreement are to be negotiated under Defense Federal Acquisition Regulation Supplement (DFARS) 252.227-7014 or the Other Transaction (OT) agreement's own terms.

##### <a id="semp-3-2-8-3"></a>3.2.8.3 Software execution

*Development environment.* As above; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> is the build-from-clean-clone procedure and is itself verified (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-930" target="_blank">DELIV-930</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-930" target="_blank">TP-930</a>).

*Requirements analysis.* <a href="#semp-2-1" target="_blank">§2.1</a>.

*Design.* <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> Coding Standards fix naming, solution layout, the three in-memory record contracts, the two extension interfaces, and the dependency-justification rule. Design changes are pull requests to the SDD, reviewed by the Solutions Architect.

*Integration and test.* Unit and component tests per assembly (72 test methods); end-to-end tests through the Command-Line Interface (CLI) against the smoke, synthetic, and reference-20 fixture sets; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a>'s comparison harness as an analysis, not a test. Test data provenance is in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/tests/fixtures/README.md" target="_blank"><code>tests/fixtures/README.md</code></a>; every reference document is a U.S. Government work (17 U.S.C. §105).

*Deployment.* Container build and run per <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a>; resource tiers per <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-530" target="_blank">NFR-530</a>; network policy per <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-500" target="_blank">NFR-500</a>/510.

*Software configuration management.* <a href="#semp-3-2-10" target="_blank">§3.2.10</a>.

*Software quality assurance.* Every merge requires a passing `dotnet build` and `dotnet test` (local until Issue <a href="#semp-i-5" target="_blank">I-5</a> closes; GitHub-hosted after) and a Test Engineer pass/fail verdict recorded on the issue; the Test Engineer cannot modify source (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/scripts/guard-test-engineer-writes.sh" target="_blank"><code>scripts/guard-test-engineer-writes.sh</code></a>); `dotnet format` is the style check, enforced in the build-and-test template; every third-party reference carries an inline justification in the `.csproj` and a row in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a>.

*Technical debt.* Recorded as issues labeled `debt`. Open at this revision: the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> metric (<a href="#semp-i-1" target="_blank">I-1</a>). Closed: the singleton-cohesion inversion (<a href="#semp-r-4" target="_blank">R-4</a>, 2026-09-17); the build-and-test workflow (<a href="#semp-i-5" target="_blank">I-5</a>, 2026-09-16).

*Defects.* GitHub issues labeled `bug`, with the failing TP identifier, the commit, and the fixture. A defect against a Verified requirement returns that requirement to In Test until the fix passes.

*<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-IPSC-81427/" target="_blank">DI-IPSC-81427B</a> map (CDRL item <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md#cdrl-a010" target="_blank">A010</a>).* The Software Development Plan DID's
content items and where this SEMP satisfies them, checked against the DID
text on file on 2026-09-17. Items the DID lists for a multi-supplier or
hardware program are tailored out with the reason.

| DID paragraph | Where satisfied |
| --- | --- |
| 3.1 Identification; 3.2 System overview; 3.3 Document overview | <a href="#semp-1-1" target="_blank">§1.1</a>, <a href="#semp-1-2" target="_blank">§1.2</a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> §Identification |
| 3.4 Relationship to other plans | <a href="#semp-1-3" target="_blank">§1.3</a>, <a href="#semp-4" target="_blank">§4</a> (DID 3.7 row) |
| 3.5 Overview of required work | <a href="#semp-1-1" target="_blank">§1.1</a> increments; <a href="#semp-3-1-1" target="_blank">§3.1.1</a> schedule and Phase 2 package; <a href="#semp-3-1-3" target="_blank">§3.1.3</a> WBS |
| 3.6 Plans for performing general activities (Agile items 1–25: sprints, backlog, feedback, Configuration Management (CM), artifact delivery, regression, automation) | <a href="#semp-3-2-8-2" target="_blank">§3.2.8.2</a> methodology; <a href="#semp-3-2-13" target="_blank">§3.2.13</a> RBR; <a href="#semp-3-2-10" target="_blank">§3.2.10</a>; <a href="#semp-appendix-c" target="_blank">Appendix C</a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a> |
| 3.7 General plans: software development methods, standards (format, comments, naming, restrictions), reusable software, assurance (safety, cyber, other critical) | <a href="#semp-3-2-8-2" target="_blank">§3.2.8.2</a>, <a href="#semp-3-2-8-3" target="_blank">§3.2.8.3</a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> Coding Standards and `.editorconfig`; safety tailored out (<a href="#semp-2-1-5" target="_blank">§2.1.5</a>); cyber <a href="#semp-3-2-12" target="_blank">§3.2.12</a> |
| 3.8 Detailed activities: project planning, environments, requirements analysis, design, implementation and unit test, integration, qualification testing, installation, transition, CM, quality assurance, corrective action, reviews, risk, metrics, security, subcontractor management, interface with Independent Verification and Validation (IV&V), coordination with associate developers, improvement | <a href="#semp-3-2-8-3" target="_blank">§3.2.8.3</a> (environment, requirements, design, integration and test, deployment, CM, SQA, defects); <a href="#semp-3-2-13" target="_blank">§3.2.13</a> (reviews); <a href="#semp-3-2-1" target="_blank">§3.2.1</a> (risk); <a href="#semp-appendix-c" target="_blank">Appendix C</a> (metrics); <a href="#semp-3-2-12" target="_blank">§3.2.12</a> (security); subcontractor management and associate developers tailored out (none); IV&V is the Test Engineer role (<a href="#semp-4" target="_blank">§4</a>, DID 3.5.d); CSCI/HWCI integration tailored out (no hardware) |
| 4 Schedules and activity network | <a href="#semp-3-1-1" target="_blank">§3.1.1</a> <a href="#semp-table-3-1-1" target="_blank">Table 3.1-1</a> |
| 5 Project organization and resources | <a href="#semp-3-1-3" target="_blank">§3.1.3</a> |
| 6 Notes | <a href="#semp-appendix-a" target="_blank">Appendix A</a> |

##### <a id="semp-3-2-8-4"></a>3.2.8.4 Software obsolescence

.NET 9 is a standard-term-support release; Microsoft's published end of support is November 2026, inside Phase 3. .NET 10 is the long-term-support successor. The program stays on .NET 9 for Increments 1 and 2 because <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md#pd-sn-4" target="_blank">SN-4</a> names it and the runtime is bundled in the container, so end of support does not affect a delivered image; migration to .NET 10 is a one-line target-framework change per project and is recommended as the first sustainment action under any follow-on agreement. The two NuGet packages are pinned; `dependency-check.yml` reports known vulnerabilities on a schedule.

#### <a id="semp-3-2-9"></a>3.2.9 Technology insertion

Two insertion points are designed in. First, the office-affinity lookup table in Increment 1 is replaced by the fitted coefficient vector in Increment 2 behind the same scoring interface, so the run-time path does not change shape. Second, the extension interfaces (`IDocumentParser`, `IClusteringComponent`) admit new document types and new clustering components without touching the core, which is how GFI-specific parsers enter in Increment 2. No other insertion is planned.

#### <a id="semp-3-2-10"></a>3.2.10 Configuration management

*Baselines.*

| Baseline | Established at | Content | Change authority |
| --- | --- | --- | --- |
| Functional (requirements) | SRR-I (2026-09-03); SRR-II for Increment 2 | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank"><code>docs/PROJECT_DEFINITION.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> | Principal, on Systems Engineer proposal |
| Allocated (design) | PDR (2026-09-03); PDR-II | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a> | Principal, on Solutions Architect proposal |
| Product (as built) | CDR through PCA; each tag `v1.0.n`; the submission tag for Increment 1; the Demo Day tag for Increment 2 | Repository at the tag; built image digest; Increment 2: the frozen artifact set and its SHA-256 manifest | CI/CD tags; Principal approves the submission tag |

*Configuration items.* The six assemblies (<a href="#semp-table-3-2-3" target="_blank">Table 3.2-3</a>), the six test assemblies, the fixture sets and their ground truth, the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Dockerfile" target="_blank"><code>Dockerfile</code></a> and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docker-compose.yml" target="_blank"><code>docker-compose.yml</code></a>, every document under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/" target="_blank"><code>docs/</code></a>, the CI workflows, and in Increment 2 the frozen artifacts (coefficient vector, vocabulary, knowledge base, labeling-function registry, `context.jsonld`, manifest) and the build-pipeline scripts that produce them.

*Change classification.* Class I: any change to RTVM requirement text, verification method, or status class; any change to an SDD architecture decision; any change to a frozen artifact's content or hash; any change to <a href="#semp-3-2-13" target="_blank">§3.2.13</a> of this SEMP. Class I requires a proposal by the owning role and approval by the Principal, recorded on the pull request. Class II: everything else, approved by the owning role's reviewer through the ordinary pull request. Frozen artifacts are never edited; they are regenerated by the build pipeline and the new hash is a Class I change.

<a id="semp-figure-3-2-6"></a>*<a href="#semp-figure-3-2-6" target="_blank">Figure 3.2-6</a> Configuration management process.*

```mermaid
flowchart LR
    A[Need or defect<br/>GitHub issue] --> B[Branch from main]
    B --> C[Change: code, doc, or artifact]
    C --> D{Class I?}
    D -- yes --> E[Owning role proposes;<br/>Principal approves on PR]
    D -- no --> F[Reviewer approves on PR]
    E --> G[Build and test; dependency check<br/>(local until I-5 closes)]
    F --> G
    G --> H[Test Engineer verdict<br/>on the issue]
    H -- pass --> I[CI/CD merges to main<br/>and tags v1.0.n]
    H -- fail --> C
    I --> J[Baseline updated;<br/>RTVM status advanced]
```

*Audits.* FCA at each SVR verifies that every active requirement has a passing test procedure against the tagged commit. PCA verifies that the tagged commit builds from a clean clone by the documented procedure and that the built image's dependency inventory matches <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a>.

#### <a id="semp-3-2-11"></a>3.2.11 Technical data management

*Repository and access.* All technical data is in the public GitHub repository, which is the ASoT. There is no separate data repository. The Government has read access now and receives the tagged package through the challenge portal.

*Data rights.* No contract exists, so DFARS 252.227-7013, -7014, -7015, and -7017 do not yet apply. The repository is MIT-licensed; the Government's rights in the submission are those the challenge announcement grants and are at least sufficient for evaluation and Demo Day. For a follow-on agreement the program will assert no restrictions on software developed under it and will deliver a 252.227-7017 assertion list that names only the two third-party packages, each with its open-source license.

*Public source data.* Reference documents are U.S. Government works from SAM.gov and GovInfo; USAspending bulk data is public domain. Provenance is recorded per file in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/tests/fixtures/README.md" target="_blank"><code>tests/fixtures/README.md</code></a> and per finding in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/G1-FINDINGS.md" target="_blank"><code>docs/research/g1-fpds/G1-FINDINGS.md</code></a>.

*Government Furnished Information.* GFI arrives with Phase 1 approval and may be marked Controlled Unclassified Information. Handling rules: GFI is stored outside the repository on the Principal's controlled system; no GFI document, excerpt, Program Manager, Air (PMA) number, Department of Defense Activity Address Code (DoDAAC), or vehicle name learned only from GFI is committed; tuning parameters derived from GFI (vocabulary, thresholds) are reviewed by the Principal for whether they reveal GFI content before they are committed, and if they do they are delivered to the Government by the portal rather than the repository. Marking follows the GFI's own marking. This rule is a Class I item under <a href="#semp-3-2-10" target="_blank">§3.2.10</a>.

*Markings.* All repository content is UNCLASSIFIED and unmarked. Nothing in this SEMP is derived from GFI.

*Documentation conventions (Principal's direction, 2026-09-17; Class II under <a href="#semp-3-2-10" target="_blank">§3.2.10</a>).* Every document in the SETR package and the Phase 2 package observes two conventions.

1. *Abbreviations.* The first time an abbreviation or acronym appears in a document it is spelled out, with the abbreviation in parentheses after it; every later reference uses the abbreviation, unless the sense of a passage calls for the full term again, in which case the full term is used without re-qualifying the abbreviation. Headings, the document-control table, and the table of contents do not count as the first use; the first use is in the body. Every document ends with an abbreviations section listing the abbreviations it uses, or points to <a href="#semp-appendix-a" target="_blank">Appendix A</a> of this SEMP, which is the program's global list and is kept a superset of every document's list. Widely recognized units and file-format names (GB, MB, s, `.md`) and proper names (NAADAP, GitHub, Docker, .NET) are not abbreviations for this purpose. Identifiers built on an abbreviation (SVR-1, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-230" target="_blank">CORE-230</a>) inherit the expansion of their stem at the stem's first use.

2. *Cross-references.* Every reference from one document to a table, figure, section, requirement, test procedure, gate, risk, request for action, commit, or workflow run in another document is a hyperlink to that item's anchor, per the cross-reference and anchor convention in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.github/AGENT_LABELS.md" target="_blank"><code>.github/AGENT_LABELS.md</code></a> (HTML `<a href>` with the full repository Uniform Resource Locator (URL) and `target="_blank"`; anchor identifiers prefixed with the document's short code). Naming an item without linking it is a convention violation. A document that defines referenceable items exposes an anchor on each of them the moment the item is written. Short codes for the SETR documents: `semp` (this SEMP), `str1` (Software Test Report, Increment 1), `svd1` (Software Version Description, Increment 1), `cdrl` (data item list), `trsr-<event>` (each review record, e.g. `trsr-svr1`).

Both conventions applied retroactively to the SETR package on 2026-09-17. Their application to the remaining Phase 2 package documents is audited at PCA-1.

#### <a id="semp-3-2-12"></a>3.2.12 System security engineering and program protection

There is no Program Protection Plan because there is no critical program information: the algorithms are published, the code is public, and the data is public. Security engineering is therefore confined to the delivered container's behavior and its supply chain.

| Concern | Control | Verification |
| --- | --- | --- |
| Egress from the container | None by default; allowlist only when the optional LLM step is enabled | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-510" target="_blank">TP-510</a> |
| Run-time dependency fetch | None; two-stage build bundles everything | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-500" target="_blank">TP-500</a> |
| Supply chain | Two packages, pinned versions, nuget.org source, licenses recorded; scheduled vulnerability check | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a>; `dependency-check.yml` |
| Software bill of materials | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a> plus `dotnet list package --include-transitive` output committed at each submission tag | PCA |
| Artifact integrity (Increment 2) | SHA-256 manifest of frozen artifacts, verified at start; mismatch aborts the run | TP to be written at SRR-II |
| Input handling | Malformed files skipped and reported; parsers are pure managed code with no native decoders | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-110" target="_blank">TP-110</a> |
| Secrets | None in the repository; the optional LLM endpoint configuration is supplied at run time | Inspection |
| Anti-tamper | Not applicable: no Critical Program Information (CPI) |

#### <a id="semp-3-2-13"></a>3.2.13 Technical reviews and audits

*Tailored sequence.* <a href="#semp-table-3-2-4" target="_blank">Table 3.2-4</a> gives the tailored NAVAIRINST 4355.19E sequence for both increments and the disposition of every one of the instruction's eighteen events. This table supersedes the "SETR Documentation Mapping (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-deliv-960" target="_blank">DELIV-960</a>)" table in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>, which was built on 4355.19D and treated PDR and CDR as documents. PDR and CDR are events; the SDD and the Implementation Plan are the artifacts those events baseline.

<a id="semp-table-3-2-4"></a>*<a href="#semp-table-3-2-4" target="_blank">Table 3.2-4</a> SETR event disposition*

| 4355.19E event | Increment 1 | Increment 2 | Basis for disposition |
| --- | --- | --- | --- |
| ITR | Held as Project Definition confirmation, 2026-09-03 | Not repeated | Handbook §6.4 |
| ASR | Folded into PDR; the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a> comparison is the alternative-systems analysis | Folded into PDR-II; the research verdict table in the design doc is the analysis | Handbook §6.4 |
| SRR-I | Held 2026-09-03 | — | Minimum set |
| SRR-II | Not held; Increment 1 requirements were complete at SRR-I | Planned | Minimum set |
| SFR | Folded into PDR | Folded into PDR-II | Handbook §6.4 |
| SSR | Folded into PDR | Folded into PDR-II | Handbook §6.4 |
| PDR | Held 2026-09-03 | Planned (PDR-II) | Minimum set |
| RBR | Not held; no release backlog in a twelve-day increment | Planned, weekly | Added for Agile per 4355.19E |
| CDR | Held 2026-09-03 | Planned (CDR-II) | Minimum set |
| IRR | Folded into CDR | Folded into CDR-II | Handbook §6.4, high off-the-shelf content |
| TRR | Held per feature issue | Planned once for the increment | Minimum set |
| Flight Readiness Review (FRR) | Not applicable | Not applicable | No flight |
| FCA | Planned with SVR-1 | Planned with SVR-2 | Added: the deliverable is a frozen package |
| SVR | Planned | Planned | Added, same reason |
| Production Readiness Review (PRR) | Not applicable | Not applicable | No production; PCA covers the build |
| PCA | Planned | Planned | Added, same reason |
| In-Service Review (ISR) | Not applicable | Not applicable | Not fielded; revisit under a follow-on agreement |
| Integrated Baseline Review (IBR), Technology Readiness Assessment (TRA), Operational Test Readiness Review (OTRR), Manufacturing Readiness Assessment (MRA) | Not applicable | Not applicable | No Earned Value Management System (EVMS), non-ACAT, no operational test, no manufacturing |
| Engineering Site Survey (ESS), as part of a Before Action Review | Not applicable | Not applicable | Examined at the Principal's direction 2026-09-15: the program takes ownership of no physical site. Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> is a feasibility analysis feeding SRR-II entry, not a site survey. |

*Review package.* No separate review document (a "PDR document", "CDR document", "SVR document") is produced for any event. The review package for an event is (1) the baselined artifacts that the event examines, at the commits named in the record's "Documents reviewed" table, (2) the review's GitHub issue thread, when the review was held on one, and (3) the Technical Review Summary Report (TRSR) at <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a>. So the SRR package is the RTVM (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>) with the Project Definition; the PDR and CDR packages are the SDD (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>) with the RTVM allocation; the SVR / FCA package is the RTVM, the Software Test Report, the Software Version Description, and the evidence directory; the PCA package is the tagged repository and the built image. <a href="#semp-table-3-2-11" target="_blank">Table 3.2-11</a>'s "Products" row names the artifact each event baselines. This is the <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-ADMN-81250/" target="_blank">DI-ADMN-81250C</a> model: the minutes record what was reviewed and decided; the documents reviewed are the program's data items (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md" target="_blank"><code>docs/setr/CDRL.md</code></a>), not documents written for the meeting. Briefing charts (<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-ADMN-81373/" target="_blank">DI-ADMN-81373</a>) are produced only when a review is briefed rather than conducted on the artifacts themselves, and each TRSR says which.

*Conduct.* Reviews are event-driven: a review is held when its entry criteria are met, not on a date. The chair is the Solutions Architect for design reviews and the Systems Engineer for requirements and verification reviews, so that no role chairs the review of its own product; the Principal attends every review and holds the decision. Each review produces a Technical Review Summary Report at `docs/setr/reviews/<event>-<date>.md` containing attendees, the artifacts reviewed at their commit, the register of risks, issues, and opportunities as of the review, requests for action, and the chair's recommendation. Requests for action are GitHub issues opened from <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.github/ISSUE_TEMPLATE/request-for-action.yml" target="_blank"><code>.github/ISSUE_TEMPLATE/request-for-action.yml</code></a> and labeled `rfa`, categorized per the Handbook as Category I (in scope, proceed), II (out of scope, needs Principal direction), or III (rejected), at urgency Level 1, 2, or 3, with Level 1 reserved for a completeness-gate or deadline threat since the program has no flight safety. The reviews already held (SRR-I, PDR, CDR) have retrospective summary reports under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a>, assembled from the closed issues and the git history and marked as such; their Requests for Action (RFAs) are tracked in the records until opened as issues.

<a id="semp-table-3-2-11"></a>*<a href="#semp-table-3-2-11" target="_blank">Table 3.2-11</a> Technical Review and Audit Details.* One block per planned review. Criteria are objective; "current" means at the commit named in the review's summary report.

<a id="semp-table-3-2-11-srr-i"></a>**SRR-I** (held 2026-09-03; recorded retrospectively)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | Principal; Product Manager; Solutions Architect |
| Purpose | Establish the Increment 1 functional baseline |
| Entry criteria | Every SN in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank"><code>docs/PROJECT_DEFINITION.md</code></a> tagged [CONFIRMED]; every RTVM item traces to at least one SN; every Test-verified item has a TP with concrete inputs and expected outputs |
| Exit criteria | RTVM approved by the Principal; no item in Draft; open items listed with owners |
| Products | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> at the approved commit; open-item list |

<a id="semp-table-3-2-11-pdr"></a>**PDR** (held 2026-09-03; ASR, SFR, SSR folded in; recorded retrospectively)

| Detail | Content |
| --- | --- |
| Chair | Solutions Architect |
| Participants | Principal; Systems Engineer; Product Manager |
| Purpose | Establish the allocated baseline; accept the assembly decomposition and the no-database, no-ICD, no-use-case decisions |
| Entry criteria | Functional baseline established; SDD contains BDD and activity diagram; every RTVM item allocated to an assembly; alternative approach identified for <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a> |
| Exit criteria | SDD approved; every architecture decision has a recorded rationale; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-nfr-520" target="_blank">NFR-520</a> interpretation recorded |
| Products | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> at the approved commit; decision list |

<a id="semp-table-3-2-11-cdr"></a>**CDR** (held 2026-09-03; IRR folded in; recorded retrospectively)

| Detail | Content |
| --- | --- |
| Chair | Solutions Architect |
| Participants | Principal; Systems Engineer; Software Engineer; Test Engineer; CI/CD |
| Purpose | Establish the initial product baseline: solution layout, coding standards, build order, dependency policy |
| Entry criteria | Allocated baseline established; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a> orders every RTVM item; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a> scaffolded with six assemblies and the reference rules of <a href="#semp-table-3-2-3" target="_blank">Table 3.2-3</a> |
| Exit criteria | Implementation Plan approved; scaffold builds and its tests pass; issues #6 through #12 created |
| Products | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a>; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a> at commit <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/869f205" target="_blank"><code>869f205</code></a>; feature issues |

<a id="semp-table-3-2-11-trr"></a>**TRR** (Increment 1: per feature issue, held eight times; Increment 2: once)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | Software Engineer; Test Engineer |
| Purpose | Confirm that a feature is ready for independent verification |
| Entry criteria | `dotnet build` and `dotnet test` pass on the feature branch; every TP for the issue's requirements has its fixture committed; Software Engineer has run the TPs locally and recorded the result |
| Exit criteria | Test Engineer accepts the hand-off (`status:ready-for-test`) |
| Products | Test Engineer's pass/fail verdict on the issue; RTVM status advanced to In Test then Verified |

<a id="semp-table-3-2-11-svr-1-fca-1"></a>**SVR-1 / FCA-1** (planned, 2026-09-19 to 2026-09-21)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | Principal; Solutions Architect; Test Engineer; CI/CD |
| Purpose | Verify that the product baseline satisfies every active Increment 1 requirement; audit the functional configuration |
| Entry criteria | Gates <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g4" target="_blank">G4</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a> closed; every active RTVM item at In Test or Verified; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a> executed at the constrained tiers with results committed; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-910" target="_blank">TP-910</a> workflow run with result recorded; build-and-test workflow installed in <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.github/workflows/" target="_blank"><code>.github/workflows/</code></a> and green on the candidate commit, or waived by the Principal with the local results recorded on the issue (Issue <a href="#semp-i-5" target="_blank">I-5</a>); risk register updated; reference-run bundle committed |
| Exit criteria | Every active requirement Verified against the candidate tag; no Category I RFA open; <a href="#semp-table-3-2-1" target="_blank">Table 3.2-1</a> actuals recorded; Principal accepts the candidate tag as the product baseline |
| Products | Summary report; RTVM with every active item Verified and commit-stamped; TPM table; candidate tag |

<a id="semp-table-3-2-11-pca-1"></a>**PCA-1** (planned, 2026-09-21)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | Principal; CI/CD |
| Purpose | Verify that the package the evaluators receive is the product baseline |
| Entry criteria | SVR-1 exited; submission tag applied |
| Exit criteria | Clean clone of the tag builds by <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a> alone (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-930" target="_blank">TP-930</a>) and produces the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-440" target="_blank">OUT-440</a> bundle (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-001" target="_blank">TP-001</a>); image dependency inventory matches <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a>; all twelve package items present and located; Principal approves submission |
| Products | Summary report; package inventory with locations; submission |

<a id="semp-table-3-2-11-srr-ii"></a>**SRR-II** (planned, week of 2026-09-28)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | Principal; Product Manager; Solutions Architect |
| Purpose | Establish the Increment 2 functional baseline |
| Entry criteria | Gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g3" target="_blank">G3</a> complete: every derived requirement Approved or Withdrawn with a verdict; gate <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g2" target="_blank">G2</a> findings recorded; the seven queued amendments (<a href="#semp-2-1-4" target="_blank">§2.1.4</a>) drafted as RTVM edits; abstention question (<a href="#semp-r-11" target="_blank">R-11</a>) decided by the Principal; Tech Grove answers received or explicitly still open |
| Exit criteria | RTVM amended and approved; every Increment 2 requirement traces to a need statement; every Test-verified item has a TP |
| Products | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> at the approved commit; DERIVED-REQUIREMENTS.md closed into the RTVM |

<a id="semp-table-3-2-11-pdr-ii"></a>**PDR-II** (planned, week of 2026-10-05; ASR, SFR, SSR folded in)

| Detail | Content |
| --- | --- |
| Chair | Solutions Architect |
| Participants | Principal; Systems Engineer; Software Engineer |
| Purpose | Establish the Increment 2 allocated baseline: knowledge-base schema, build pipeline, model specification, evidence-record schema, artifact-manifest interface |
| Entry criteria | SRR-II exited; design doc accepted into the SDD (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g6" target="_blank">G6</a>) with the Increment 2 sections added; KB schema with provenance columns drafted; conditional logit specification with covariates, ridge term, and fitting procedure written; choice-set construction rule written |
| Exit criteria | SDD approved; every Increment 2 requirement allocated to an assembly or the build pipeline; frozen-artifact interface specified; release backlog created and ordered |
| Products | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>; KB schema; model specification; release backlog |

<a id="semp-table-3-2-11-rbr"></a>**RBR** (planned, weekly from 2026-10-05)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | Principal; Software Engineer; Test Engineer |
| Purpose | Review the release backlog against the allocated baseline; re-order; accept the sprint's increment |
| Entry criteria | Sprint's build and tests pass; <a href="#semp-appendix-c" target="_blank">Appendix C</a> metrics for the sprint recorded; risk register updated |
| Exit criteria | Backlog re-ordered and approved; TPM actuals recorded; slip against <a href="#semp-table-3-1-1" target="_blank">Table 3.1-1</a> stated in days |
| Products | Sprint record under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a>; updated backlog; tag |

<a id="semp-table-3-2-11-cdr-ii"></a>**CDR-II** (planned, week of 2026-10-19; IRR folded in)

| Detail | Content |
| --- | --- |
| Chair | Solutions Architect |
| Participants | Principal; Systems Engineer; Software Engineer; Test Engineer |
| Purpose | Establish the Increment 2 initial product baseline |
| Entry criteria | Build pipeline runs end to end on FY2022–FY2025 archives and emits the frozen artifact set with manifest; conditional logit fit converges from β = 0 and reproduces bit-for-bit on a second run; KB coverage TPM at plan; GFI-tuned parsers pass <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-data-in-100" target="_blank">DATA-IN-100</a> on a GFI sample (result reported, sample not committed) |
| Exit criteria | Artifact set frozen and hashed; every Increment 2 TP has a fixture; Principal accepts the design as built |
| Products | Frozen artifacts and manifest; CDR summary report |

<a id="semp-table-3-2-11-trr-ii-svr-2-fca-2-pca-2"></a>**TRR-II, SVR-2 / FCA-2, PCA-2** (planned, weeks of 2026-10-26, 2026-11-02, and by 2026-11-12)

| Detail | Content |
| --- | --- |
| Chair | Systems Engineer |
| Participants | As for the Increment 1 counterparts |
| Purpose | As for the Increment 1 counterparts, against the Increment 2 RTVM and the Demo Day tag |
| Entry criteria | As for the Increment 1 counterparts, plus: MRR and Recall@5 on the FPDS holdout recorded; artifact-integrity TP passed; a rehearsal of the Demo Day run completed within the design-target time on a fresh container |
| Exit criteria | As for the Increment 1 counterparts; Demo Day materials inventory complete |
| Products | Summary reports; Demo Day tag; materials submission |

---

## <a id="semp-4"></a>4 DID Conformance Crosswalk

<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785B</a>, block 10, prescribes the SEMP's minimum content (identical to Revision A; see <a href="#semp-1-3" target="_blank">§1.3</a>). Each item and the section satisfying it:

| DID item | Requirement (paraphrased) | Satisfied by |
| --- | --- | --- |
| 3.1 | The contractor's planned engineering approach | <a href="#semp-1-1" target="_blank">§1.1</a>, <a href="#semp-1-4" target="_blank">§1.4</a>, <a href="#semp-2-4" target="_blank">§2.4</a>, <a href="#semp-3-2-8-2" target="_blank">§3.2.8.2</a> |
| 3.2 | Detailed operational plan, including integration of specialty engineering | <a href="#semp-2-3" target="_blank">§2.3</a>, <a href="#semp-3-1" target="_blank">§3.1</a>, <a href="#semp-3-2" target="_blank">§3.2</a> |
| 3.3 | Annotated mapping of the contractor's SE processes to the Government's, with rationale for any not mapped | <a href="#semp-table-4-1" target="_blank">Table 4-1</a> below |
| 3.4 | Alignment with subcontractor SE plans | No subcontractors (<a href="#semp-3-2-8-1" target="_blank">§3.2.8.1</a>). Not applicable. |
| 3.5.a | Technical solution: architecture and interfaces | <a href="#semp-2-2" target="_blank">§2.2</a> |
| 3.5.b | Formal reviews as defined in IEEE 24748-8:2019, with entry and exit criteria | <a href="#semp-3-2-13" target="_blank">§3.2.13</a> |
| 3.5.c | Trade studies | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a> comparison (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank"><code>docs/ALGORITHM_COMPARISON.md</code></a>); research verdicts in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> "Decisions and their basis"; ML.NET evaluation (<a href="#semp-3-2-8-2" target="_blank">§3.2.8.2</a>) |
| 3.5.d | Independent verification and validation | Test Engineer role, independent of implementation and write-guarded (<a href="#semp-3-2-8-3" target="_blank">§3.2.8.3</a>); ground truth derived by inspection and never read by the pipeline (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a>) |
| 3.6 | Tailored process planning, including suppliers and COTS | <a href="#semp-1-4" target="_blank">§1.4</a>, <a href="#semp-3-2-8-2" target="_blank">§3.2.8.2</a> |
| 3.7 | Referenced lower-level plans: risk, requirements, data, configuration (see also <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/CDRL.md" target="_blank"><code>docs/setr/CDRL.md</code></a>, the data item list naming the DID each plan and product follows) | <a href="#semp-3-2-1" target="_blank">§3.2.1</a>; <a href="#semp-2-1" target="_blank">§2.1</a> and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>; <a href="#semp-3-2-11" target="_blank">§3.2.11</a>; <a href="#semp-3-2-10" target="_blank">§3.2.10</a>. Test: RTVM Test Procedures and <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a>. |
| 3.8 | Other areas as necessary | <a href="#semp-appendix-c" target="_blank">Appendix C</a> (metrics), <a href="#semp-appendix-e" target="_blank">Appendix E</a> (digital engineering) |

<a id="semp-table-4-1"></a>*<a href="#semp-table-4-1" target="_blank">Table 4-1</a> Process mapping: HoloSim SE workflow to Government processes.* The contractor's process is the ten-step workflow in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/reference/Human_SE_Workflow.txt" target="_blank"><code>docs/reference/Human_SE_Workflow.txt</code></a>. The Government references are the ISO/IEC/IEEE 15288 technical processes named in the SE Guidebook (2022) and the NAVAIRINST 4355.19E event at which the step's product is reviewed.

| HoloSim step | ISO/IEC/IEEE 15288 process | SETR event | NAADAP product | Note |
| --- | --- | --- | --- | --- |
| 1 Stakeholder needs (mission, value, narrative, Statement of Work (SOW), needs matrix) | Business or mission analysis; Stakeholder needs and requirements definition | ITR | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank"><code>docs/PROJECT_DEFINITION.md</code></a> | Maps directly |
| 2 Scope of work (internal narrative, initial requirements breakdown, questions and Requests for Information (RFIs), five W's) | Stakeholder needs and requirements definition | ITR, SRR-I entry | Scope sections of the Project Definition; Tech Grove questions in `challenge-brief.md` | RFIs are the Tech Grove questions |
| 3 Requirements (agents, action diagrams) | System requirements definition | SRR-I, SRR-II | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>; activity diagram in the SDD | Maps directly |
| 4 Data structures (data points, collections, types, transfer, storage) | Architecture definition (data view) | SFR (folded into PDR) | SDD Data Architecture; in-memory record contracts; Increment 2 KB schema | Transfer protocol items (Message Queuing Telemetry Transport (MQTT), Transmission Control Protocol (TCP), User Datagram Protocol (UDP)) not applicable: no data transfer |
| 5 Architecture (structure, wireframes, events, error handling, interfaces, external communication) | Architecture definition | PDR | SDD BDD; interface register <a href="#semp-2-2-2" target="_blank">§2.2.2</a> | Wireframes not applicable: no UI. External communication limited to the optional allowlisted endpoint |
| 6 Logic planning (functions per wireframe, classes and methods, SysML, UML) | Design definition | PDR, CDR | SDD Coding Standards; sequence diagram | Maps directly |
| 7 Code file structure (project types, classes, folders, stubs, configuration files) | Implementation | CDR | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/Naadap.sln" target="_blank"><code>Naadap.sln</code></a> scaffold, issue #5 | Maps directly |
| 8 Server database (tables, stored procedures, IO classes behind interfaces) | Implementation | — | Not performed: no database by SDD decision. Increment 2 KB is a versioned table with a build pipeline, reviewed at PDR-II | Rationale recorded in the SDD |
| 9 Refine functional code (Agile iteration, buildable at session end, component validation, daily push) | Implementation; Integration; Verification | TRR; RBR (Increment 2) | Feature issues #6–#12; CI on every Pull Request (PR); tags | Maps directly |
| 10 Test and adjust | Verification; Validation; Transition | TRR, SVR/FCA, PCA | TP-nnn execution; reference-20 metric; PCA clean-clone build | Maps directly |
| (no contractor step) | Configuration management; Risk management; Technical planning; Decision management | All | <a href="#semp-3-2-10" target="_blank">§3.2.10</a>; <a href="#semp-3-2-1" target="_blank">§3.2.1</a>; <a href="#semp-3-1" target="_blank">§3.1</a>; SDD decision records | Government processes with no named step in the contractor workflow are performed and are recorded here so the mapping has no gap |

---

## <a id="semp-appendix-a"></a><a href="#semp-appendix-a" target="_blank">Appendix A</a> — Abbreviations and Acronyms

The program's global list (<a href="#semp-3-2-11" target="_blank">§3.2.11</a>, documentation conventions). Every abbreviation used in any document of the SETR package appears here; each document's own abbreviations section is a subset of this table.

| Abbreviation | Expansion |
| --- | --- |
| ACAT | Acquisition Category |
| AI | Artificial Intelligence |
| AMSC | Acquisition Management Systems Control |
| API | Application Programming Interface |
| ASoT | Authoritative Source of Truth |
| ASR | Alternative Systems Review |
| ASSIST | Acquisition Streamlining and Standardization Information System |
| ATO | Authorization to Operate |
| BCL | Base Class Library |
| BDD | Block Definition Diagram |
| CBRN | Chemical, Biological, Radiological, and Nuclear |
| CDR | Critical Design Review |
| CDRL | Contract Data Requirements List |
| CI | Continuous Integration |
| CI/CD | Continuous Integration / Continuous Delivery |
| CLI | Command-Line Interface |
| CM | Configuration Management |
| CONOPS | Concept of Operations |
| COTS | Commercial Off-the-Shelf |
| CPI | Critical Program Information |
| CPU | Central Processing Unit |
| CSCI | Computer Software Configuration Item |
| CUI | Controlled Unclassified Information |
| DBDD | Database Design Description |
| DevSecOps | Development, Security, and Operations |
| DFARS | Defense Federal Acquisition Regulation Supplement |
| DID | Data Item Description |
| DMSMS | Diminishing Manufacturing Sources and Material Shortages |
| DoD | Department of Defense |
| DoDAAC | Department of Defense Activity Address Code |
| DoDI | Department of Defense Instruction |
| DOPSR | Defense Office of Prepublication and Security Review |
| DORA | DevOps Research and Assessment |
| ESS | Engineering Site Survey |
| ETL | Extract, Transform, Load |
| EVM | Earned Value Management |
| EVMS | Earned Value Management System |
| FAR | Federal Acquisition Regulation |
| FCA | Functional Configuration Audit |
| FPDS | Federal Procurement Data System |
| FRR | Flight Readiness Review |
| GFI | Government Furnished Information |
| GOTS | Government Off-the-Shelf |
| HSI | Human Systems Integration |
| HTTP | Hypertext Transfer Protocol |
| HTTPS | Hypertext Transfer Protocol Secure |
| IBR | Integrated Baseline Review |
| ICD | Interface Control Document |
| ID | Identifier |
| IDV | Indefinite-Delivery Vehicle |
| IEEE | Institute of Electrical and Electronics Engineers |
| IL4 | Impact Level 4 |
| IPT | Integrated Product Team |
| IRR | Integration Readiness Review |
| IRS | Interface Requirements Specification |
| ISR | In-Service Review |
| ITR | Initial Technical Review |
| IV&V | Independent Verification and Validation |
| JCIDS | Joint Capabilities Integration and Development System |
| KB | Knowledge Base |
| KPP | Key Performance Parameter |
| KSA | Key System Attribute |
| LLM | Large Language Model |
| LTS | Long-Term Support |
| MIT | Massachusetts Institute of Technology |
| ML | Machine Learning |
| MOSA | Modular Open Systems Approach |
| MQTT | Message Queuing Telemetry Transport |
| MRA | Manufacturing Readiness Assessment |
| MRR | Mean Reciprocal Rank |
| MVP | Minimum Viable Product |
| NAICS | North American Industry Classification System |
| NAVAIR | Naval Air Systems Command |
| NAVAIRINST | Naval Air Systems Command Instruction |
| NAVFAC | Naval Facilities Engineering Systems Command |
| NAVSEA | Naval Sea Systems Command |
| NAVWAR | Naval Information Warfare Systems Command |
| NAWCAD | Naval Air Warfare Center Aircraft Division |
| NFR | Non-Functional Requirement |
| NMCARS | Navy Marine Corps Acquisition Regulation Supplement |
| NSWC | Naval Surface Warfare Center |
| OCD | Operational Concept Description |
| OCI | Open Container Initiative |
| OSD | Office of the Secretary of Defense |
| OT | Other Transaction |
| OTRR | Operational Test Readiness Review |
| PCA | Physical Configuration Audit |
| PDR | Preliminary Design Review |
| PGIL | Procurement Group Innovation Lab |
| PIID | Procurement Instrument Identifier |
| PMA | Program Manager, Air |
| PR | Pull Request |
| PRR | Production Readiness Review |
| PSC | Product Service Code |
| PWS | Performance Work Statement |
| RBR | Release Backlog Review |
| RFA | Request for Action |
| RFI | Request for Information |
| RFP | Request for Proposal |
| RTM | Requirements Traceability Matrix |
| RTVM | Requirements Traceability and Verification Matrix |
| SDD | Software Design Description |
| SDK | Software Development Kit |
| SE | Systems Engineering |
| SEMP | Systems Engineering Management Plan |
| SEP | Systems Engineering Plan |
| SETR | Systems Engineering Technical Review |
| SFR | System Functional Review |
| SME | Subject Matter Expert |
| SN | Stakeholder Need |
| SOW | Statement of Work |
| SPS | Software Product Specification |
| SQA | Software Quality Assurance |
| SRR | System Requirements Review |
| SRS | Software Requirements Specification |
| SSDD | System/Subsystem Design Description |
| SSR | Software Specification Review |
| SSS | System/Subsystem Specification |
| STR | Software Test Report |
| SVD | Software Version Description |
| SVR | System Verification Review |
| SysML | Systems Modeling Language |
| TCP | Transmission Control Protocol |
| TF-IDF | Term Frequency–Inverse Document Frequency |
| TP | Test Procedure |
| TPM | Technical Performance Measure |
| TRA | Technology Readiness Assessment |
| TRR | Test Readiness Review |
| TRSR | Technical Review Summary Report |
| UDP | User Datagram Protocol |
| UI | User Interface |
| UML | Unified Modeling Language |
| URL | Uniform Resource Locator |
| UTC | Coordinated Universal Time |
| WBS | Work Breakdown Structure |
| WSL | Windows Subsystem for Linux |

## <a id="semp-appendix-b"></a><a href="#semp-appendix-b" target="_blank">Appendix B</a> — Item Unique Identification Implementation Plan

Not applicable. The deliverable is software and documentation; no tangible item meeting the DFARS 252.211-7003 criteria is delivered.

## <a id="semp-appendix-c"></a><a href="#semp-appendix-c" target="_blank">Appendix C</a> — Agile and DevSecOps Software Development Metrics

*Use.* Metrics are recorded at each RBR (Increment 2) and each SETR event, in the review's summary report, from committed artifacts: the git log, CI run history, test output, and the RTVM. They are reviewed by the IPT and reported to the Principal. There is no dashboard; the summary report is the report. The Outline's minimum set is adopted with the tailoring stated per metric.

<a id="semp-table-c-1"></a>*<a href="#semp-table-c-1" target="_blank">Table C-1</a> Agile metrics*

| Metric (Outline) | Definition here | Source | Baseline (Increment 1, 2026-09-03 to 2026-09-15) |
| --- | --- | --- | --- |
| Sprint velocity | RTVM requirements advanced to Verified per week (requirements replace story points; see <a href="#semp-3-2-8-2" target="_blank">§3.2.8.2</a>) | RTVM commit column | 25 in 12 days |
| Average cycle time | Days from feature-issue open to Verified | Issue timestamps | To be computed at SVR-1 from issues #6–#12 |
| Burn-down | Active requirements not yet Verified, by week | RTVM status counts | 30 → 5 → (3 Approved, 2 Withdrawn) |
| Sprint retrospective | What went well and what to improve, one paragraph each | Summary report | Recorded from Increment 2's first RBR |
| Build automation | Percentage of build, test, and package steps automated in GitHub | Installed workflows | Dependency check and Windows verification automated; build and test local pending Issue <a href="#semp-i-5" target="_blank">I-5</a>; constrained-tier timing runs manual |
| Builds per day or week, pass and fail | Count from workflow history once installed; until then from issue records | GitHub Actions; issues | To be computed at SVR-1 |
| Average build duration | Minimum, average, maximum minutes | GitHub Actions once installed | Not yet measured; recorded at SVR-1 |
| Unit test coverage | Percent automated (100%); percent line coverage from coverlet | `dotnet test --collect` | To be recorded at SVR-1 |
| Static analysis coverage | `dotnet format` verification; percent of projects covered (100% once Issue <a href="#semp-i-5" target="_blank">I-5</a> closes); weakness findings burn-down | Build-and-test template | Not yet run in GitHub |
| Functional thread test coverage | Percent of RTVM Test-verified items with a passing TP | RTVM | 100% of Verified items |
| System test coverage | End-to-end TPs (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-001" target="_blank">TP-001</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-440" target="_blank">TP-440</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-210" target="_blank">TP-210</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-520" target="_blank">TP-520</a>) passing | RTVM | 4 of 6 executed; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-220" target="_blank">TP-220</a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-tp-230" target="_blank">TP-230</a> pending |

<a id="semp-table-c-2"></a>*<a href="#semp-table-c-2" target="_blank">Table C-2</a> Development, Security, and Operations (DevSecOps) metrics.* The Outline's supplemental set applies to programs operating a deployment pipeline. NAADAP delivers a container to the Government and operates no environment beyond development and CI, so the environment metrics reduce as shown; the DevOps Research and Assessment (DORA) four are recorded because they are cheap and comparable.

| Metric | Definition here | Baseline |
| --- | --- | --- |
| Number of active environments | Development (Ubuntu); Windows verification runner; build-and-test runner once installed | 2 (3 when Issue <a href="#semp-i-5" target="_blank">I-5</a> closes) |
| Environment availability | Not measured; environments are ephemeral and re-created per run | — |
| Time to create environment | Container build from clean clone, minutes | Not yet measured; recorded at PCA-1 |
| Automated environment controls audited | Not applicable; no operated environment | — |
| Deployment frequency (DORA) | Tags per week | 8 tags in 12 days |
| Lead time for changes (DORA) | Commit to tag, hours | To be computed at SVR-1 |
| Mean time to recover (DORA) | Not applicable; no production | — |
| Change failure rate (DORA) | Tags followed by a defect issue against the same requirement | 0 recorded |

## <a id="semp-appendix-d"></a><a href="#semp-appendix-d" target="_blank">Appendix D</a> — Concept of Operations Description

There is no Government Concept of Operations (CONOPS). The operational concept, from the challenge announcement and the Principal's framing:

An acquisition professional at NAVAIR or NAWCAD has a set of documents describing requirements that today are, or would be, contracted separately. The professional places the documents in a directory and runs the container once, pointing it at that directory and an output directory. The container reads every document it can, reports the ones it cannot, groups the documents by shared requirement content, and for each group lists the strategic contract vehicles that could absorb the group, ranked, with the evidence for each and the reasons the others were ruled out. It writes one bundle: the list, a picture of how it reached the list, a picture of the list, a summary metric, and a pointer to how the metric was validated. The professional reads the bundle and takes the determination to the approval authority that FAR 7.107 and NMCARS 5237.102 require. The tool does not choose; it shows the map, the recommended route, and why the other routes were rejected, so that an experienced officer can confirm a judgment and a new one can learn it.

The mission scenario for evaluation is the sponsor's own: twenty documents, thirty minutes, one core and two gigabytes, scored against PGIL's predetermined answers; then, on Demo Day, a fresh validation set and five manually identified candidates, live, inside a thirty-minute presentation.

## <a id="semp-appendix-e"></a><a href="#semp-appendix-e" target="_blank">Appendix E</a> — Digital Engineering Implementation Plan

<a id="semp-appendix-e-1"></a>*E.1 Goals, objectives, and approach.* The goal is that every engineering claim in the package can be regenerated from the repository by the Government without HoloSim's help. The objectives: one source of truth; diagrams and tables that are the design rather than pictures of it; every run and every model artifact reproducible from committed inputs. The approach is model-supported (<a href="#semp-2-4" target="_blank">§2.4</a>): version-controlled text and diagram sources, a deterministic build, and hashed artifacts, without a SysML tool chain.

<a id="semp-appendix-e-2"></a>*E.2 Roles and responsibilities.*

| Activity | Responsible | Resources |
| --- | --- | --- |
| Maintain the ASoT (repository, branch protection, tags) | CI/CD | GitHub |
| Requirements model (RTVM) | Systems Engineer | Markdown, git |
| Architecture models (BDD, activity, sequence) | Solutions Architect | Mermaid in Markdown, rendered by GitHub |
| Analytical models of the product (clustering, conditional logit, KB) | Software Engineer; specification by Systems Engineer | C#/.NET; build pipeline scripts |
| Model verification | Test Engineer | xunit; fixture sets |
| Training | Not required; all tools are the team's ordinary tools | — |

<a id="semp-appendix-e-3"></a>*E.3 Modeling methodologies and standards.* SysML notation for structure and behavior diagrams, authored in Mermaid so that the diagram source is text under version control and renders in the repository without a tool license. Naming: diagram nodes carry the RTVM identifiers they allocate. Templates: the SDD's existing diagrams are the template for any new diagram. Statistical model specification follows the notation in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a> (utility, softmax over the eligible set, ridge-penalized log-likelihood, Newton–Raphson from zero with fixed summation order).

<a id="semp-appendix-e-4"></a>*E.4 Configuration control baseline for models.* Models are configuration items under <a href="#semp-3-2-10" target="_blank">§3.2.10</a>. Diagram and table sources change by pull request. Frozen analytical artifacts are regenerated, never edited, and their hashes are Class I changes. Discoverability: every model is listed in <a href="#semp-table-e-4" target="_blank">Table E-4</a> with its path.

<a id="semp-appendix-e-5"></a>*E.5 Authoritative data.* The repository is the authoritative source for every model and every datum the program controls. Public source data (SAM.gov documents, USAspending archives) is authoritative at its origin and is recorded by provenance, not mirrored, except for the reference-20 fixture set and the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> extract, which are committed so the results are reproducible without a network. GFI is never in the repository (<a href="#semp-3-2-11" target="_blank">§3.2.11</a>).

<a id="semp-appendix-e-6"></a>*E.6 Collaboration.* Reviews are conducted on GitHub issues and pull requests, which hold the artifact at its commit, the discussion, and the decision. The Government reads the repository; no separate collaboration environment is provided. Summary reports under <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/docs/setr/reviews/" target="_blank"><code>docs/setr/reviews/</code></a> give an external stakeholder the review's outcome without reading the issue thread.

<a id="semp-appendix-e-7"></a>*E.7 Model use.*

<a id="semp-table-e-4"></a>*<a href="#semp-table-e-4" target="_blank">Table E-4</a> Model register*

| Model | Kind | Used for | Owner | Location | Metadata |
| --- | --- | --- | --- | --- | --- |
| Requirements model | Table with identifiers, trace, verification, status | Traceability; review entry and exit criteria; TPM trace | Systems Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a> | Commit column per row |
| Block definition diagram | SysML structure (Mermaid) | Assembly allocation; <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-240" target="_blank">CORE-240</a> inspection basis | Solutions Architect | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> | Baselined at PDR |
| Activity diagram | SysML behavior (Mermaid) | Run-time branch points | Solutions Architect | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a> | Baselined at PDR |
| Sequence diagram | UML (Mermaid) | Build order | Systems Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a> | Baselined at CDR |
| Requirements decomposition tree | Flowchart (Mermaid) | Baseline structure | Systems Engineer | This SEMP, <a href="#semp-figure-2-1-1" target="_blank">Figure 2.1-1</a> | — |
| CM process | Flowchart (Mermaid) | Change control | Systems Engineer | This SEMP, <a href="#semp-figure-3-2-6" target="_blank">Figure 3.2-6</a> | — |
| Clustering model | TF-IDF vectors; cosine; single-link at a global threshold | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-200" target="_blank">CORE-200</a> | Software Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Core/" target="_blank"><code>src/Naadap.Core</code></a> | Threshold derivation in source remarks |
| Alternative model | Mutual k-NN over TF-IDF | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-core-260" target="_blank">CORE-260</a> comparison only | Software Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/src/Naadap.Alternative/" target="_blank"><code>src/Naadap.Alternative</code></a> | Never on the run path |
| Ground truth | Document-to-vehicle mapping by inspection | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md#rtvm-out-420" target="_blank">OUT-420</a> scoring only | Systems Engineer | <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/tests/fixtures/reference-20/ground-truth.json" target="_blank"><code>tests/fixtures/reference-20/ground-truth.json</code></a> | Per-document rationale |
| Office-affinity lookup table (Increment 1) | Counts of vehicle family by office from FPDS FY2025 | Scoring | Systems Engineer | To be produced at <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a> from <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/navair_orders_fy2025.csv" target="_blank"><code>docs/research/g1-fpds/navair_orders_fy2025.csv</code></a> | Source archive date |
| Vehicle knowledge base (Increment 1, revised Increment 2) | Tabular, family-grouped, PROV-named provenance columns | Hard constraints; scoring; evidence record | Systems Engineer | To be produced at <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g5" target="_blank">G5</a> | Schema version; source per row |
| Conditional logit coefficient vector (Increment 2) | Fitted β; vocabulary; ridge term | Scoring | Software Engineer | Frozen artifact, hashed | Fit date; archive years; holdout metrics |
| Labeling-function registry (Increment 2) | Deterministic SME rules as features | Scoring; evidence record | Systems Engineer with the acquisition SME skill | Frozen artifact, hashed | Author; source citation per rule |

<a id="semp-appendix-e-8"></a>*E.8 Tool chain.*

| Tool | Purpose | License |
| --- | --- | --- |
| Git, GitHub | ASoT; review; CI | Service |
| .NET SDK 9.0 | Build, test | MIT |
| xunit, coverlet | Test, coverage | Apache 2.0, MIT |
| Docker | Packaging; constrained-tier runs | Apache 2.0 (Engine) |
| Mermaid (GitHub rendering) | Diagrams | MIT |
| Increment 2 build pipeline (outside the container) | Archive pull, filter, fit, freeze | Language decided at PDR-II; the <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md#vrp-g1" target="_blank">G1</a> extraction is <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/g1_extract.py" target="_blank"><code>docs/research/g1-fpds/g1_extract.py</code></a> (Python 3, standard library) |

---

## <a id="semp-references"></a>References

1. <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/" target="_blank">DI-SESS-81785B</a>, *Systems Engineering Management Plan (SEMP)*, Data Item Description, approved 2025-01-08, AMSC 10515. On file: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/DI-SESS-81785B.pdf" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/DI-SESS-81785/DI-SESS-81785B.pdf</code></a>, with the superseded Revision A (2015-09-29) and base (2009-10-14) revisions alongside it.
2. Office of the Under Secretary of Defense for Research and Engineering, *Systems Engineering Plan (SEP) Outline*, Version 4.1, May 2023, DOPSR 23-S-1904. On file: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.claude/skills/gov-acquisition-sme/sources/SEP-Outline-4.1.pdf" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/SEP-Outline-4.1.pdf</code></a>.
3. Office of the Under Secretary of Defense for Research and Engineering, *Systems Engineering Guidebook*, February 2022. On file: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.claude/skills/gov-acquisition-sme/sources/SE-Guidebook-Feb2022.pdf" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/SE-Guidebook-Feb2022.pdf</code></a>.
4. Office of the Under Secretary of Defense for Research and Engineering, *Engineering of Defense Systems Guidebook*, Change 1, July 2024. On file: <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.claude/skills/gov-acquisition-sme/sources/Eng-Def-Sys-Change1-July2024.pdf" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/Eng-Def-Sys-Change1-July2024.pdf</code></a>.
5. DoD Instruction 5000.88, *Engineering of Defense Systems*, 18 November 2020.
6. DoD Instruction 5000.87, *Operation of the Software Acquisition Pathway*, 2 October 2020 (reference only; the program is not on the pathway).
7. NAVAIR Instruction 4355.19E, *Systems Engineering Technical Review Process*, 6 February 2015. On file in <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/</code></a>.
8. NAVAIR, *SETR Process Handbook*, Version 1.0. On file in <a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/" target="_blank"><code>.claude/skills/gov-acquisition-sme/sources/</code></a>.
9. ISO/IEC/IEEE 15288:2015, *Systems and software engineering — System life cycle processes*; IEEE 24748-7:2019, *Application of systems engineering on defense programs* (formerly IEEE 15288.1); IEEE 24748-8:2019, *Technical reviews and audits on defense programs* (formerly IEEE 15288.2).
10. FAR 7.107, *Additional requirements for acquisitions involving consolidation, bundling, or substantial bundling*; FAR 2.101; NMCARS 5237.102.
11. DFARS 252.227-7013, -7014, -7015, -7017; DFARS 252.211-7003.
12. HoloSim Interactive, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank"><code>docs/PROJECT_DEFINITION.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank"><code>docs/RTVM.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/SDD.md" target="_blank"><code>docs/SDD.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/IMPLEMENTATION_PLAN.md" target="_blank"><code>docs/IMPLEMENTATION_PLAN.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPENDENCIES.md" target="_blank"><code>docs/DEPENDENCIES.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/DEPLOYMENT.md" target="_blank"><code>docs/DEPLOYMENT.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/VALIDATION_METHODOLOGY.md" target="_blank"><code>docs/VALIDATION_METHODOLOGY.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/ALGORITHM_COMPARISON.md" target="_blank"><code>docs/ALGORITHM_COMPARISON.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/MAINTAINER_GUIDE.md" target="_blank"><code>docs/MAINTAINER_GUIDE.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank"><code>docs/design/vehicle-recommendation-pipeline.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/requirements-derivation/NEED-STATEMENTS.md" target="_blank"><code>docs/requirements-derivation/NEED-STATEMENTS.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/requirements-derivation/DERIVED-REQUIREMENTS.md" target="_blank"><code>docs/requirements-derivation/DERIVED-REQUIREMENTS.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/research/g1-fpds/G1-FINDINGS.md" target="_blank"><code>docs/research/g1-fpds/G1-FINDINGS.md</code></a>, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/reference/Human_SE_Workflow.txt" target="_blank"><code>docs/reference/Human_SE_Workflow.txt</code></a>, this repository, 2026.
13. HoloSim Interactive, <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.claude/skills/gov-acquisition-sme/references/challenge-brief.md" target="_blank"><code>.claude/skills/gov-acquisition-sme/references/challenge-brief.md</code></a> (the challenge announcement as analyzed), `navair-navy-context.md`, `requirements-documents.md`, `glossary.md`, this repository, 2026.
14. McFadden, D., "Conditional logit analysis of qualitative choice behavior," in *Frontiers in Econometrics*, P. Zarembka (ed.), Academic Press, 1974.
15. Tomlinson, K., Ugander, J., and Benson, A. R., "Choice Set Confounding in Discrete Choice," *Proceedings of the 27th ACM SIGKDD Conference*, 2021.

All illustrations in this document are original to it.
