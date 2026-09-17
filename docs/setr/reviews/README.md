# <a id="trsr-index-title"></a>SETR review records

One Technical Review Summary Report per Systems Engineering Technical Review (SETR) event, in the form the Systems Engineering Management Plan (SEMP)
prescribes (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-13" target="_blank"><code>docs/setr/SEMP.md</code> §3.2.13</a>): attendees, the artifacts reviewed
at their commit, the register of risks, issues, and opportunities as of the
review, requests for action, and the chair's recommendation.

Records for reviews held before the SEMP existed are marked *retrospective*.
They were assembled on 2026-09-15 from the closed GitHub issues that carried
the review and from the git history; nothing in them was generated at the
time of the event, and each says so.

| Record | Event | Date held | Increment | Status |
| --- | --- | --- | --- | --- |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SRR-I-2026-09-03.md" target="_blank">`SRR-I-2026-09-03.md`</a> | System Requirements Review I | 2026-09-03 | 1 | Retrospective |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/PDR-2026-09-03.md" target="_blank">`PDR-2026-09-03.md`</a> | Preliminary Design Review (Alternative Systems Review (ASR), System Functional Review (SFR), Software Specification Review (SSR) folded in) | 2026-09-03 | 1 | Retrospective |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/CDR-2026-09-03.md" target="_blank">`CDR-2026-09-03.md`</a> | Critical Design Review (Integration Readiness Review (IRR) folded in) | 2026-09-03 | 1 | Retrospective |
| <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/reviews/SVR-1-FCA-1-2026-09-17.md" target="_blank">`SVR-1-FCA-1-2026-09-17.md`</a> | System Verification Review and Functional Configuration Audit | 2026-09-17 | 1 | Exited; Principal signed 2026-09-17 and accepted <a href="https://github.com/HoloSim-Interactive/NAADAP/commit/261f7e3" target="_blank"><code>261f7e3</code></a> as the product baseline |
| Physical Configuration Audit 1 (PCA-1) | Physical Configuration Audit | Planned 2026-09-21 | 1 | Entry open (product baseline accepted); not yet held |

## <a id="trsr-index-where-the-review-documents-are"></a>Where the review documents are

There is no separate Preliminary Design Review (PDR), Critical Design Review (CDR), System Requirements Review (SRR), or System Verification Review (SVR) document. A SETR event reviews
the program's data items themselves at a named commit, and the record above
is the minutes of that review (<a href="https://github.com/HoloSim-Interactive/NAADAP/tree/main/.claude/skills/gov-acquisition-sme/sources/DI-ADMN-81250/" target="_blank">DI-ADMN-81250C</a>). Each record's "Documents
reviewed" table is the review package: the Requirements Traceability and Verification Matrix (RTVM) for SRR, the Software Design Description (SDD) for PDR
and CDR, the RTVM with the Software Test Report and Software Version
Description for SVR / Functional Configuration Audit (FCA), and the tagged repository and built image for
PCA. The SEMP defines this in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-13" target="_blank">§3.2.13</a> ("Review package") and names the
artifact each event baselines in <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-table-3-2-11" target="_blank">Table 3.2-11</a> ("Products").

Requests for action are GitHub issues opened from the
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/.github/ISSUE_TEMPLATE/request-for-action.yml" target="_blank"><code>.github/ISSUE_TEMPLATE/request-for-action.yml</code></a> template and labeled `rfa`.
Each record lists its Requests for Action (RFAs) by issue number.

Naming: `<EVENT>-<yyyy-mm-dd>.md`, the date being the date the review exited.

## <a id="trsr-index-abbreviations"></a>Abbreviations

Abbreviations used in this document, each spelled out at its first use in the body (<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-3-2-11" target="_blank">SEMP §3.2.11</a>, documentation conventions). The program's global list is <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/setr/SEMP.md#semp-appendix-a" target="_blank">SEMP Appendix A</a>.

| Abbreviation | Expansion |
| --- | --- |
| ASR | Alternative Systems Review |
| CDR | Critical Design Review |
| FCA | Functional Configuration Audit |
| IRR | Integration Readiness Review |
| PCA | Physical Configuration Audit |
| PDR | Preliminary Design Review |
| RFA | Request for Action |
| RTVM | Requirements Traceability and Verification Matrix |
| SDD | Software Design Description |
| SEMP | Systems Engineering Management Plan |
| SETR | Systems Engineering Technical Review |
| SFR | System Functional Review |
| SRR | System Requirements Review |
| SSR | Software Specification Review |
| SVR | System Verification Review |
