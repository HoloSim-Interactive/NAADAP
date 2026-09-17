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
| A003 | DI-IPSC-81431A | System/Subsystem Specification | `docs/RTVM.md` Requirements table | SRR-I | Not yet checked. The RTVM is the requirements source; the SSS section structure (states and modes, capability requirements, interface, qualification provisions, traceability) is to be mapped onto it, not imposed on it |
| A004 | DI-IPSC-81433A | Software Requirements Specification | `docs/RTVM.md` (the system is one CSCI set; SSS and SRS coincide) | SRR-I | Not yet checked; expected to be satisfied by A003 with a statement of coincidence |
| A005 | DI-IPSC-81434A | Interface Requirements Specification | SEMP Table 2.2-2; `docs/SDD.md` Build & Toolchain Conventions and Data Architecture | PDR | Not yet checked |
| A006 | DI-IPSC-81432A | System/Subsystem Design Description | `docs/SDD.md` Architecture (BDD, activity diagram) | PDR | Not yet checked |
| A007 | DI-IPSC-81435B | Software Design Description | `docs/SDD.md` Coding Standards and Data Architecture; per-assembly design | CDR | Not yet checked. 81435B is a text DID (8 pages) and the most likely to require restructuring of `docs/SDD.md` |
| A008 | DI-IPSC-81436A | Interface Design Description | `docs/SDD.md` in-memory record contracts; `manifest.json` layout | CDR | Not yet checked; may be folded into A007 as 81435B permits |
| A009 | DI-IPSC-81437A | Database Design Description | `docs/KB_SCHEMA.md` (no database; the knowledge base is a frozen file set with a documented schema and ETL pipeline, DELIV-950 reopened narrowly 2026-09-17) | G6, 2026-09-17 | Not yet checked against 81437A's content list; expected to satisfy the design-description and data-element sections and to state the others as not applicable |
| A010 | DI-IPSC-81427B | Software Development Plan | SEMP §3.2.8 | SEMP Draft A.1 | Not yet checked. 81427B's Agile, cybersecurity, and safety content is to be mapped to §3.2.8 and Appendix C |
| A011 | DI-IPSC-81438A | Software Test Plan | `docs/RTVM.md` Test Procedures preamble; `docs/VALIDATION_METHODOLOGY.md` | PDR | Not yet checked |
| A012 | DI-IPSC-81439A | Software Test Description | `docs/RTVM.md` TP-nnn | CDR | Not yet checked |
| A013 | DI-IPSC-81440A | Software Test Report | Test Engineer verdicts on issues #5–#12; SVR-1 summary report | SVR-1 / FCA-1 | Not yet produced as one document |
| A014 | DI-IPSC-81441A | Software Product Specification | Submission tag; built image digest; `docs/DEPLOYMENT.md`; `docs/DEPENDENCIES.md` | PCA-1 | Not yet checked |
| A015 | DI-IPSC-81442A | Software Version Description | `VERSION`; tag; `docs/DEPENDENCIES.md`; release notes to be written at PCA-1 | PCA-1 | Not yet produced as one document |
| A016 | DI-NDTI-80566A | Test Plan | `docs/VALIDATION_METHODOLOGY.md` with the RTVM Test Procedures | SVR-1 | Not yet checked; overlaps A011, one document will cite both |
| A017 | DI-NDTI-80603A | Test Procedure | `docs/RTVM.md` TP-nnn | SVR-1 | Not yet checked (DID on file 2026-09-17) |
| A018 | DI-NDTI-80809B | Test/Inspection Report | SVR-1 summary report with TP results | SVR-1 / FCA-1 | Not yet produced |
| A019 | DI-ADMN-81249C | Meeting Agenda | Each review's agenda, in the review record | Every SETR event | Retrospective records for SRR-I, PDR, CDR carry no agenda; planned reviews will |
| A020 | DI-ADMN-81250C | Meeting Minutes | `docs/setr/reviews/<EVENT>-<date>.md` | Every SETR event | Not yet checked against 81250C's content list |
| A021 | DI-ADMN-81373 | Presentation Material | Review briefing; Demo Day deck (Phase 3) | Each review; Demo Day | Not yet produced |
| A022 | DI-MGMT-81650 | Integrated Master Schedule | SEMP Table 3.1-1; GitHub issues | SEMP | Not applicable in full: no EVM. The event schedule serves; noted in SEMP §3.1.1 |
| A023 | DI-SESS-81000F | Product Engineering Design Data and Associated Lists | Not applicable: no hardware. The product baseline is A014 and A015 | — | Not applicable |

## Conformance work, in order

1. **A007 (SDD, 81435B)** — read the DID's content requirements and map `docs/SDD.md` section by section; add missing sections; record the map in the SDD's front matter. This is the largest item and the one a Government reviewer will open first.
2. **A003/A004 (SSS/SRS, 81431A/81433A)** — map the RTVM onto the SSS section structure in a front-matter table; do not reorder the RTVM.
3. **A010 (SDP, 81427B)** — map SEMP §3.2.8 and Appendix C to 81427B's content list; add what is missing.
4. **A011/A012/A016 (test plan and description)** — one front-matter map in `docs/VALIDATION_METHODOLOGY.md` and the RTVM Test Procedures preamble.
5. **A020 (minutes, 81250C)** — align the review-record template with the DID's content list before SVR-1 so the first live record conforms.
6. **A013/A015/A018 (test report, version description, test/inspection report)** — produce at SVR-1 and PCA-1 from the DID content lists.
7. **A017 (test procedure, 80603A)** — map TP-nnn onto the DID's content list with A012.
