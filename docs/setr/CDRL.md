# SETR package data item list (contractor CDRL equivalent)

There is no contract and therefore no DD Form 1423. This list is the
program's own statement of which Data Item Description governs each artifact
in the SETR package, so that a Government reader can check an artifact
against the DID it claims. It is maintained by the Systems Engineer under
`docs/setr/SEMP.md` §3.2.13 and §4, and it is the input to the conformance
work listed at the end.

DID numbers, titles, and revisions are taken from
`.claude/skills/gov-acquisition-sme/references/did-register.md`, which was
built from the documents on file, not from memory.

## Data items

| Item | DID (current revision) | Title | NAADAP artifact | First delivered at | Conformance to the DID |
| --- | --- | --- | --- | --- | --- |
| A001 | DI-SESS-81785B | Systems Engineering Management Plan | `docs/setr/SEMP.md` | SEMP Draft A.1, 2026-09-15 | Checked: §4 crosswalk against DID 3.1–3.8 |
| A002 | DI-IPSC-81430A | Operational Concept Description | SEMP Appendix D; `docs/PROJECT_DEFINITION.md` Mission Statement | SRR-I | Not yet checked |
| A003 | DI-IPSC-81431A | System/Subsystem Specification | `docs/RTVM.md` Requirements table | SRR-I | Checked 2026-09-17: conformance map in `docs/RTVM.md` §DI-IPSC-81431A / 81433A conformance; six paragraphs tailored out with reasons |
| A004 | DI-IPSC-81433A | Software Requirements Specification | `docs/RTVM.md` (the system is one CSCI set; SSS and SRS coincide) | SRR-I | Checked 2026-09-17 with A003; coincidence stated in the same map |
| A005 | DI-IPSC-81434A | Interface Requirements Specification | SEMP Table 2.2-2; `docs/SDD.md` Build & Toolchain Conventions and Data Architecture | PDR | Not yet checked |
| A006 | DI-IPSC-81432A | System/Subsystem Design Description | `docs/SDD.md` Architecture (BDD, activity diagram) | PDR | Not yet checked |
| A007 | DI-IPSC-81435B | Software Design Description | `docs/SDD.md` | CDR | Checked 2026-09-17: `docs/SDD.md` §DI-IPSC-81435B conformance adds identification, overview, referenced documents, CSCI-wide decisions, software-unit table with identifiers SU-01 to SU-13, interfaces IF-1 to IF-7, traceability, and notes; three items tailored out with reasons |
| A008 | DI-IPSC-81436A | Interface Design Description | `docs/SDD.md` §Interfaces (IF-1 to IF-7) | CDR | Folded into A007 as 81435B §2.2 permits; checked 2026-09-17 |
| A009 | DI-IPSC-81437A | Database Design Description | `docs/KB_SCHEMA.md` (no database; the knowledge base is a frozen file set with a documented schema and ETL pipeline, DELIV-950 reopened narrowly 2026-09-17) | G6, 2026-09-17 | Not yet checked against 81437A's content list; expected to satisfy the design-description and data-element sections and to state the others as not applicable |
| A010 | DI-IPSC-81427B | Software Development Plan | SEMP §3.2.8 | SEMP Draft A.1 | Checked 2026-09-17: map at the end of SEMP §3.2.8.3; subcontractor, associate-developer, and hardware-integration items tailored out |
| A011 | DI-IPSC-81438A | Software Test Plan | `docs/VALIDATION_METHODOLOGY.md` with the RTVM Test Procedures | PDR | Checked 2026-09-17: map in `docs/VALIDATION_METHODOLOGY.md` §DID conformance |
| A012 | DI-IPSC-81439A | Software Test Description | `docs/RTVM.md` TP-nnn | CDR | Checked 2026-09-17: same map |
| A013 | DI-IPSC-81440A | Software Test Report | `docs/setr/STR-increment-1.md` | SVR-1 / FCA-1 | Produced 2026-09-17 to the DID's section structure (scope, referenced documents, overview, detailed results with deviations, test log, notes) |
| A014 | DI-IPSC-81441A | Software Product Specification | Submission tag; built image digest; `docs/DEPLOYMENT.md`; `docs/DEPENDENCIES.md` | PCA-1 | Not yet checked |
| A015 | DI-IPSC-81442A | Software Version Description | `docs/setr/SVD-increment-1.md` | PCA-1 | Produced 2026-09-17 to the DID's section structure; tag and image digest blanks filled at PCA-1 |
| A016 | DI-NDTI-80566A | Test Plan | `docs/VALIDATION_METHODOLOGY.md` with the RTVM Test Procedures | SVR-1 | Checked 2026-09-17: same map |
| A017 | DI-NDTI-80603A | Test Procedure | `docs/RTVM.md` TP-nnn | SVR-1 | Checked 2026-09-17: same map |
| A018 | DI-NDTI-80809B | Test/Inspection Report | `docs/setr/STR-increment-1.md` (one report serves both DIDs) | SVR-1 / FCA-1 | Produced 2026-09-17 |
| A019 | DI-ADMN-81249C | Meeting Agenda | Each review's agenda, in the review record | Every SETR event | Retrospective records for SRR-I, PDR, CDR carry no agenda; planned reviews will |
| A020 | DI-ADMN-81250C | Meeting Minutes | `docs/setr/reviews/<EVENT>-<date>.md` from `docs/setr/reviews/TEMPLATE.md` | Every SETR event | Template aligned to 81250C items a–h and 3.1–3.2 on 2026-09-17; the three retrospective records predate it and are not restructured |
| A021 | DI-ADMN-81373 | Presentation Material | Review briefing; Demo Day deck (Phase 3) | Each review; Demo Day | Not yet produced |
| A022 | DI-MGMT-81650 | Integrated Master Schedule | SEMP Table 3.1-1; GitHub issues | SEMP | Not applicable in full: no EVM. The event schedule serves; noted in SEMP §3.1.1 |
| A023 | DI-SESS-81000F | Product Engineering Design Data and Associated Lists | Not applicable: no hardware. The product baseline is A014 and A015 | — | Not applicable |

## Conformance work

Items 1 through 5 and 7 of the original order were completed on
2026-09-17 (maps recorded in the artifacts named above). Remaining:

1. **A013/A015/A018** — produced 2026-09-17; SVD blanks filled at PCA-1.
2. **A002 (OCD, 81430A)**, **A005 (IRS, 81434A)**, **A006 (SSDD, 81432A)**, **A009 (DBDD, 81437A)**, **A014 (SPS, 81441A)** — maps not yet written; A005/A006 are expected to fold into A007 as the DIDs permit, A009 into `docs/KB_SCHEMA.md`.
