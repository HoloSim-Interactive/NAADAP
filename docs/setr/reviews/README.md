# SETR review records

One Technical Review Summary Report per SETR event, in the form the SEMP
prescribes (`docs/setr/SEMP.md` §3.2.13): attendees, the artifacts reviewed
at their commit, the register of risks, issues, and opportunities as of the
review, requests for action, and the chair's recommendation.

Records for reviews held before the SEMP existed are marked *retrospective*.
They were assembled on 2026-09-15 from the closed GitHub issues that carried
the review and from the git history; nothing in them was generated at the
time of the event, and each says so.

| Record | Event | Date held | Increment | Status |
| --- | --- | --- | --- | --- |
| [`SRR-I-2026-09-03.md`](SRR-I-2026-09-03.md) | System Requirements Review I | 2026-09-03 | 1 | Retrospective |
| [`PDR-2026-09-03.md`](PDR-2026-09-03.md) | Preliminary Design Review (ASR, SFR, SSR folded in) | 2026-09-03 | 1 | Retrospective |
| [`CDR-2026-09-03.md`](CDR-2026-09-03.md) | Critical Design Review (IRR folded in) | 2026-09-03 | 1 | Retrospective |
| [`SVR-1-FCA-1-2026-09-17.md`](SVR-1-FCA-1-2026-09-17.md) | System Verification Review and Functional Configuration Audit | Entered 2026-09-17; criteria met 2026-09-17 | 1 | Exit recommended; Principal's acceptance pending |
| PCA-1 | Physical Configuration Audit | Planned 2026-09-21 | 1 | Not yet held |

Requests for action are GitHub issues opened from the
`.github/ISSUE_TEMPLATE/request-for-action.yml` template and labeled `rfa`.
Each record lists its RFAs by issue number.

Naming: `<EVENT>-<yyyy-mm-dd>.md`, the date being the date the review exited.
