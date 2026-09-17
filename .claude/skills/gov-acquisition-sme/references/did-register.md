# Data Item Description register

Every DID on file under `sources/DI-*/`, read from the documents themselves
on 2026-09-16 (text extraction where the PDF carries text; OCR of page 1
where ASSIST serves a scan, marked *scan*). "Current" is the highest
revision on file; "Validated" is the latest ASSIST Notice of Validation on
file, which re-affirms a DID without changing it. Approval dates are as
printed on the DID. Files are named by ASSIST's download convention
(`<DID>_<Revision>_<GUID>.pdf`); the SEMP folder was renamed by hand.

Use this register, not memory, when citing a DID number. Two numbers this
skill had recalled wrongly before the documents were in hand are corrected
below and flagged.

## MIL-STD-498 software data items (preparing activity EC; DI-IPSC)

| DID | Current | Title | Approved | Validated | Supersedes | Note |
| --- | --- | --- | --- | --- | --- | --- |
| DI-IPSC-81427 | B | Software Development Plan (SDP) | 2017-03-13 (AMSC N9775) | A validated 2000-01-10 | A | B adds Agile, cybersecurity, and safety-assurance content; 11 pages, text |
| DI-IPSC-81430 | A | Operational Concept Description (OCD) | 2000-01-10 (N7375) | 2021-09-03 (notice 3) | base | *scan* |
| DI-IPSC-81431 | A | System/Subsystem Specification (SSS) | 2000-01-10 (N7376) | 2013-07-08 (notice 2) | base | *scan* |
| DI-IPSC-81432 | A | **System/Subsystem Design Description (SSDD)** | 1999-08-10 (N7351) | 2021-09-03 (notice 3) | base | *scan*. **Correction:** this skill's 2026-09-15 list called 81432 the IRS. Wrong; the IRS is 81434. |
| DI-IPSC-81433 | A | Software Requirements Specification (SRS) | 1999-12-15 (N7358) | 2025-04-08 (notice 3) | base | *scan* |
| DI-IPSC-81434 | A | **Interface Requirements Specification (IRS)** | 1999-12-15 (N7359) | 1999-12-15 (notice 1) | DI-MCCR-80026A, DI-MCCR-80303 | *scan*. **Correction:** the 2026-09-15 list called 81434 the SSDD. Wrong; see 81432. |
| DI-IPSC-81435 | B | Software Design Description (SDD) | 2021-11-22 (N10191) | B validated 2021-11-22 | A (1999-12-15) | 8 pages, text. Describes CSCI-wide design decisions, architectural design, detailed design; may be supplemented by IDD (81436) and DBDD (81437) |
| DI-IPSC-81436 | A | Interface Design Description (IDD) | 1999-12-15 (N7361) | 2021-09-03 (notice 3) | base | *scan* |
| DI-IPSC-81437 | A | Database Design Description (DBDD) | 1999-12-15 (N7362) | 2021-09-03 (notice 3) | base | *scan* |
| DI-IPSC-81438 | A | Software Test Plan (STP) | 1999-12-15 (N7363) | 2021-09-03 (notice 3) | base | *scan*; folder and files are named `81438A_…` by the downloader |
| DI-IPSC-81439 | A | Software Test Description (STD) | 1999-12-15 (N7364) | 2021-09-03 (notice 3) | base | *scan* |
| DI-IPSC-81440 | A | Software Test Report (STR) | 1999-12-15 (N7365) | 2021-09-03 (notice 3) | base | *scan* |
| DI-IPSC-81441 | A | Software Product Specification (SPS) | 1999-12-15 (N7366) | 2025-04-08 (notice 3) | base | *scan* |
| DI-IPSC-81442 | A | Software Version Description (SVD) | 2000-01-11 (N7377) | 2013-07-08 (notice 2) | base | *scan* |

Cross-references printed inside the DIDs confirm the numbering: the SSS
(81431A) points to "Interface Requirements Specifications (IRSs)
(DI-IPSC-81434A)"; the SSDD (81432A) says it "may be supplemented by
Interface Design Descriptions (IDDs) (DI-IPSC-81436A) and Database Design
Descriptions (DBDDs) (DI-IPSC-81437A)"; the IRS (81434A) names the SSS
(81431A) and SRS (81433A) as the specifications it accompanies.

## Meetings, presentations, schedule (DI-ADMN, DI-MGMT)

| DID | Current | Title | Approved | Validated | Supersedes | Note |
| --- | --- | --- | --- | --- | --- | --- |
| DI-ADMN-81249 | C | Meeting Agenda | 2021-07-30 (F10263; AFLCMC/EZSC) | — | B | Text. "Conference Agenda" in earlier revisions; a conference is a type of meeting. Relates to 81250. |
| DI-ADMN-81250 | C | Meeting Minutes | 2021-07-30 (F10264; AFLCMC/EZSC) | — | B | Text. Content list includes title page, attendees, decisions and agreements, action items. |
| DI-ADMN-81373 | base | Presentation Material | 1993-10-01 | 2017-06-28 (notice 2) | DI-A-3024A | *scan*, one page |
| DI-MGMT-81650 | base | Integrated Master Schedule (IMS) | 2005-03-30 (D7544; OUSD(AT&L)) | — | — | Text, 6 pages. Applies to contracts requiring EVM "and other contracts based on the contract risk assessment." |

## Test (DI-NDTI)

| DID | Current | Title | Approved | Validated | Supersedes | Note |
| --- | --- | --- | --- | --- | --- | --- |
| DI-NDTI-80566 | A | Test Plan | 2006-11-14 (AMSC 7639; OPR NS/DA02) | — | base | Text, 3 pages. Contractor format. |
| DI-NDTI-80603 | A | Test Procedure | 2006-11-14 (AMSC 7637; OPR NS/DA02) | — | base (*scan*) | Text, 4 pages. Re-downloaded 2026-09-17 after the first download delivered copies of 80566; verified by title and number. Content starts with a cover and title page (3.1.1). |
| DI-NDTI-80809 | B | Test/Inspection Report | 1997-01-24 | 2019-12-11 (notice 3) | — | *scan* (poor OCR); DD Form 1664 layout |

## Systems engineering (DI-SESS)

| DID | Current | Title | Approved | Validated | Supersedes | Note |
| --- | --- | --- | --- | --- | --- | --- |
| DI-SESS-81000 | F | Product Engineering Design Data and Associated Lists | 2019-04-17 (AMSC 10017; AR) | F validated 2019-04-17 | E | Text. Work task 5.4.1.3 of MIL-STD-31000; related to 81001F/81002F/81003F. Hardware TDP; for software the SPS and SVD carry the product baseline. Revisions A through F on file. |
| DI-SESS-81785 | B | Systems Engineering Management Plan (SEMP) | 2025-01-08 (AMSC 10515; SE) | — | A (2015-09-29), base (2009-10-14) | Text. Content 3.1–3.8 identical A to B; B cites IEEE 24748-7/-8:2019 in place of IEEE 15288.1/.2. See `docs/setr/SEMP.md` §1.3. |

## Not on file, still wanted

Numbers unverified; search ASSIST by title.

- Configuration Audit Plan; Configuration Audit Summary Report (FCA and PCA). The DI-CMAN to DI-SESS renumbering is where recall is least reliable.
- Requirements Traceability Matrix or Verification Cross-Reference Matrix (SRR, SVR).
- Risk Management Plan (SRR).
- Trade Study Report (PDR).
