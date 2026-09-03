# Systems Engineer — memory

**This file is an index, not a store.** It is loaded on every run you
ever do, so anything verbose here is re-read on every future hand-off
for the rest of the project. Keep each entry to one line: a link and a
one-sentence summary. Put the actual detail in its own file in this
folder.

    - [Short title](descriptive_slug.md) — one sentence on what it is.

A genuinely one-line fact can stay a plain line with no file of its
own. Split a lesson out when it needs a reproduction, a command
sequence, or real reasoning to be useful later. See "Memory structure"
in `.github/AGENT_LABELS.md`.

## RTVM conventions

- [Deliverable reqs as RTVM items](deliverable_reqs_as_rtvm_items.md) — NAADAP: PM wants DELIV-9xx as real line items, not narrative-only.
- NAADAP RTVM test-data convention: use SN-1's PGIL group-of-20 as the "representative document set" size unless a procedure calls for smaller.
- [NAADAP SDD decisions](naadap_sdd_decisions.md) — no DB, NFR-520 semantics, VS-verification-once-at-consolidation, no UI track, Core/Alt assembly separation. Read before Implementation Plan issue.
- [NAADAP Implementation Plan](naadap_implementation_plan.md) — build sequence + issues #5-#12 created, one per SDD pipeline-stage component, single linear order.
- [Scaffold issue RTVM fast-path](scaffold_issue_rtvm_fastpath.md) — Generate Code Base-style issues touch RTVM IDs structurally but don't Verify them; their real verification issue is elsewhere in the plan.

## Cross-product interface standards

## Requirements patterns and traps

- [Network access from pipeline](network_access_from_pipeline.md) — `.mil` domains 403 from this sandbox; sam.gov/acqnotes.com/dau.edu work.
- [NAADAP SETR reference](naadap_setr_reference.md) — verified SETR review sequence + source; "PEDDAL" unverified, flagged to PM.

## Documentation index
