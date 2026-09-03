---
name: anchor_convention_retrofit
description: How to apply/audit the cross-reference & anchor convention across RTVM/SDD/Plan and issue bodies, and what to leave alone
metadata:
  type: feedback
---

Applied 2026-09-03 (NAADAP issue #13, client directive to retrofit the
convention documented in AGENT_LABELS.md#cross-reference-and-anchor-convention
onto every existing content doc and issue description).

**Anchor ID scheme actually used (extend this, don't reinvent per project):**
- RTVM row/TP: `rtvm-<lowercased-id>` (`rtvm-core-200`, `rtvm-tp-210`).
  Heading anchors for non-ID sections: `rtvm-reference-scale`,
  `rtvm-research-notes`, `rtvm-open-items`.
- SDD section anchors are hand-picked (headings aren't ID-shaped), named
  for what the section decides, not its literal heading text:
  `sdd-decision-no-database`, `sdd-nfr-520-replicability`,
  `sdd-target-platform-verification`, `sdd-setr-documentation-mapping`,
  `sdd-build-toolchain-conventions`, `sdd-why-no-use-case-diagram-icd`,
  `sdd-data-architecture` (parent heading, kept separate from the
  no-database decision subsection since other refs point at just the
  decision, not the whole Data Architecture section).
- Plan: `plan-step-<n>` for build-sequence items (not `plan-phase-<n>` —
  this project explicitly rejected the 3-axis phase model, so "phase"
  terminology would misleadingly imply that structure exists).

**What NOT to touch, and why:** the `## Dependencies` bullet lines
(`Finish-Start:`/`Start-Start:`/`Owner:`) and the `**Next:**` line in
every issue body. `dependency-check.yml` greps those lines literally
for `#[0-9]+` and `agent:[a-z-]+` patterns; `stall-recovery.yml` parses
`**Next:**` by exact format. Wrapping an issue number in an `<a>` tag on
those lines doesn't break the regex (the digits are still there as
visible text) but AGENT_LABELS.md explicitly says "keep prose off
dependency lines" — treat that as a hard boundary, not just a style
preference, and leave those lines byte-for-byte alone even when doing a
sweep like this.

**Don't preemptively link to anchors that don't exist yet.** RTVM's
`SN-#` column and every `SN-#` prose mention across all docs/issues
were left as plain text because `docs/PROJECT_DEFINITION.md` had no
`pd-sn-<n>` anchors at the time (that's Product Manager's part of the
same retrofit issue, done after Systems Engineer's). Flagged as
deferred in the handoff comment rather than guessed at.

**Gotcha found mid-task:** two of the six feature issues (#8, #10)
already had a partial, malformed edit applied — an `<a href>` using
`/tree/main/...` instead of `/blob/main/...`, only the first ID in a
list linked, and a stray space before the following comma. Evidence of
an earlier attempt at this same retrofit getting interrupted mid-run.
Fix: don't try to patch around partial state — regex-unwrap any
existing `<a href="...">TEXT</a>` back to plain `TEXT` first, then
reapply the full conversion cleanly in one pass. Trying to detect and
only-fix the malformed parts is much more error-prone than a clean
redo.

**Concurrent-edit collision:** pushing docs/RTVM.md changes here raced
with CI/CD's own RTVM status-column update (NFR-500/510/520/530 →
Verified, commit `d9c65ef`) landing on `main` first. `git pull --rebase`
produced a real content conflict inside the Requirements table (not a
clean auto-merge) because both sides touched the same table rows.
Resolved by keeping the anchor/link-bearing version of every row's text
but taking the Status/Commit columns from whichever side had the
fresher data — don't just pick one side wholesale when a table row was
edited by both.
