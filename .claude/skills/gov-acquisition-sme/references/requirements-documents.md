# Requirements documents: what they are and what to extract from them

The pipeline's input population is SOWs, PWSs, CDRLs, sources-sought
notices, and open-source text. Each has a canonical shape. Knowing the
shape is what separates "acquisition requirement content" from boilerplate,
and it is the difference between clustering on what a squadron actually
needs and clustering on which contracting office wrote the template.

## Document types

| Document | Authority | Nature | Sections to expect | Part of the contract? |
| --- | --- | --- | --- | --- |
| SOW (Statement of Work) | FAR 11.101, 11.002; MIL-HDBK-245D (guidance) | Prescriptive "work words"; technical requirements belong in specifications, data in CDRLs | 1 Scope (background only, no tasks); 2 Applicable Documents (every document invoked in section 3, exact version); 3 Requirements (numbered tasks) | Yes, Section C or attachment |
| PWS (Performance Work Statement) | FAR 2.101, 37.601 to 37.603 | Results-oriented, measurable standards, incentives; should not state hours (37.602(b)) | Introduction; General Information; Background; GFP/GFI; Performance Objectives and Standards (the PRS: objective, standard, AQL, surveillance method); Applicable Documents; Special Requirements (security); Deliverables | Yes |
| SOO (Statement of Objectives) | FAR 37.602(c) | Government objectives; offerors write the PWS | Purpose; scope or mission; period and place of performance; background; performance objectives; operating constraints | No, the offeror's PWS replaces it |
| QASP | FAR 37.604, 46.401; DFARS 237.172 | Surveillance plan | Work requiring surveillance; method (100%, random sampling, periodic, customer feedback); COR roles; AQL; remedies | Attachment or Government-internal |
| CDRL (DD Form 1423) | DFARS 215.470; DoD 5010.12-M | One data item per DD 1423-1 line | See block list below | Exhibit A |
| Sources Sought / RFI | FAR 10.002(b)(2); 15.201(e); provision 52.215-3 | Market research; "not a solicitation" | Notice metadata, draft scope, capability questions | No |
| J&A | FAR 6.303-2 | Justification for other than full and open | Twelve elements including authority cited, unique-source rationale, market research, future-competition actions | Contract file |
| Acquisition plan | FAR 7.105 | Strategy | Background and objectives; plan of action including sources, competition, contract type, consolidation analysis | Contract file |

Section C of a solicitation carries the SOW/PWS; Section B the CLINs and
contract type per CLIN; Section F period and place of performance; Section
H special requirements (key personnel, OCI, security, data rights,
transition); Section J attachments (PWS, CDRL, DD 254, QASP, wage
determinations); Section L proposal instructions; Section M evaluation
factors (FAR 15.204-1, Table 15-1).

## A useful acronym trap

A SOW is not a specification and a PWS is not a SOW. If a "PWS" contains
FTE counts, labor hours, or labor categories in the body, it is a
level-of-effort SOW wearing a PWS label, and it signals T&M or LH pricing.
Treat that as a fact about the incumbent contract, not an error to fix.

## Fields to extract from a SOW, PWS, or SOO

**Identity.** Title; document type from headings; version and date;
requiring activity; PEO, PMA, or program name; contracting office DoDAAC
(N00019, N00421, N68335, N61340, N68936 for NAVAIR; see
navair-navy-context.md).

**Scope and lineage.** Mission and platform names; incumbent contract
numbers ("currently performed under N00178-xx-D-xxxx"); predecessor
vehicles; "this is a follow-on to" phrases.

**Applicable documents.** MIL-STDs, DoDIs, NIST SP 800-171 and CMMC level,
DFARS clauses, security classification guide, DD 254.

**Tasks.** Numbered task areas (3.1, 3.2 ...); "shall" statements;
deliverable references "(CDRL A001)"; labor categories and key personnel;
FTE or hour counts (presence signals LOE pricing); surge and optional
tasks; place of performance (on-site installation, contractor site,
OCONUS); period of performance (base plus option years; ordering period
versus performance period); travel; GFP/GFI/GFE; security clearance level
(Secret, TS, SCI; facility clearance; CAC or IT-II); OPSEC; data rights
(unlimited, GPR, limited, restricted, SBIR; 252.227-7013/7014/7015/7017);
Section 508; SCA or DBA wage determinations; personal-services red flags;
inherently governmental functions; contract-type hints ("firm-fixed-price
CLIN", "T&M CLIN", "cost-plus"); CLIN structure; NAICS; PSC; set-aside;
small-business goals; transition-in and transition-out; performance
standards and AQLs; QASP references; PRS table; surveillance method;
incentives; cybersecurity clauses (7012, 7019, 7020, 7021); DPAS rating;
OCI mitigation; SCIF requirements.

## Fields to extract from a CDRL (DD Form 1423)

Blocks: 1 Data Item Number (A001...); 2 Title; 3 Subtitle; 4 Authority
(DID number); 5 Contract Reference (the SOW/PWS paragraph); 6 Requiring
Office; 7 DD 250 requirement; 8 Approval code; 9 Distribution statement;
10 Frequency; 11 As-of date; 12 Date of first submission; 13 Subsequent
submissions; 14 Distribution addressees and copies; 15 Total; 16 Remarks
(tailoring, format, media); 17 Price group; 18 Estimated total price.
Header: category (TDP, TM, Other), system/item, contract or PR number,
contractor.

DID numbers read "DI-" plus a four-letter standardization area plus a
five-digit serial plus revision: DI-MGMT (management, e.g., 81861 IPMR),
DI-IPSC (software, e.g., 81435A SDD, 81433A SRS), DI-SESS (systems
engineering, e.g., 81785 SEP), DI-MISC (e.g., 80508 technical report),
DI-ADMN, DI-FNCL, DI-ILSS, DI-SAFT, DI-TMSS. DIDs live in ASSIST
(quicksearch.dla.mil). Frequency codes: MTHLY, QRTLY, ASREQ, ONE/R.
Distribution statements A through F.

Extract the Block 5 cross-references: they tie every deliverable back to
a task paragraph, and the set of DID areas on a CDRL is a compact
fingerprint of what kind of work the contract is (a CDRL heavy in
DI-IPSC is software development; heavy in DI-ILSS is logistics).

## Fields to extract from a sources-sought notice or RFI

Notice ID and type (SAM.gov ptype r = Sources Sought, p =
Presolicitation, o = Solicitation, k = Combined, s = Special Notice, a =
Award, u = J&A, i = Intent to Bundle); agency, office, contracting
activity code; NAICS and size standard; PSC; set-aside intent; response
date; place and anticipated period of performance; anticipated contract
type or vehicle ("anticipated single-award IDIQ", "task order under
SeaPort-NxG"); estimated value or ceiling; draft PWS attachment; requested
capability-statement content (CAGE, UEI, size, socioeconomic status,
relevant contracts, clearance, teaming); questions to industry (bundling
or consolidation impact questions signal a 7.107 analysis in progress);
incumbent identification; security requirements; the "this is not a
solicitation" disclaimer.

An "Intent to Bundle" notice or a sources-sought asking "would
consolidation of these requirements adversely affect your ability to
compete" is direct evidence the Government is already contemplating the
consolidation the pipeline would recommend.

## Fields from an acquisition plan or J&A

Acquisition plan (7.105): statement of need; applicable conditions; cost;
capability; delivery period; trade-offs; risks; sources including small
business and required sources; competition; contract-type selection;
source-selection procedures; acquisition considerations (this is where the
7.107 consolidation analysis lives in practice); budgeting; product
descriptions; DPAS; inherently governmental functions; data and GFP/GFI;
security; contract administration; milestones; participants.

J&A (6.303-2): authority cited (6.302-1 through -7); estimated value;
description; unique-source rationale; efforts to solicit others; fair and
reasonable determination; market research; sources expressing interest;
actions to remove barriers to future competition; CO certification;
approval level per 6.304.

## Open-source text (Congressional testimony, news)

These are not requirements documents. Their value is forward-looking:
program names, platforms, budget lines, and capability gaps that predict
future requirements. Extract named platforms and programs (V-22, F/A-18,
E-6B, CH-53K, MQ-25), fiscal-year budget references, named vehicles or
consortia, and capability language ("sustainment shortfall", "readiness",
"digital engineering"). The challenge text explicitly includes
"identifying new strategic contracting vehicles based on future
capabilities," and open-source text is the only input that speaks to the
future.

## Boilerplate to strip before clustering

Government documents share enormous common vocabulary that says nothing
about the requirement. Remove or down-weight before similarity scoring:
FAR and DFARS clause text and numbers (regex 52\.2\d{2}-\d+ and
252\.2\d{2}-7\d{3}); Section I clause lists; standard security, OPSEC,
and CUI paragraphs; travel regulations (JTR); invoicing and WAWF
instructions; contractor identification (11.106); Section 508 language;
signature blocks and distribution statements. What remains, mainly the
section 3 task statements and the PRS table, is the requirement content
that a human SME reads when deciding whether two efforts belong on the
same vehicle.
