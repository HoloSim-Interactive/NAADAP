---
name: rtvm-status-pretest-to-commit
description: Which RTVM status value to use when relaying a Test Engineer PASS to CI/CD, before the commit-confirmation loop closes
metadata:
  type: project
---

The RTVM status vocabulary (`docs/RTVM.md`) is Draft → Approved → In
Implementation → In Test → Verified. On the systems-engineer.md "fast
path" (`status:ready-for-rtvm-update`, Test Engineer's test passed),
set the item's status to **In Test** — not Verified. "Verified" is
reserved specifically for the later step ("Receiving a commit
confirmation from CI/CD"), which also fills in the Commit(s) column
with the SHA. Leaving the Commit(s) column blank at the "In Test"
stage is correct and expected — it gets filled in on the next
round-trip.

**Why:** the workflow explicitly splits these into two separate
hand-offs (RTVM update → CI/CD, then CI/CD → RTVM update again with
SHA). Jumping straight to Verified before the commit actually lands
would make the RTVM claim something is verified-in-trunk when it's
only verified-in-branch.

**How to apply:** first time an `[RTVM-0xx]`-style issue reaches you
with `status:ready-for-rtvm-update`, this is the pattern (confirmed
2026-09-03 on NAADAP issue #7, DATA-IN-100/110/120: Approved → In
Test, handed to `agent:cicd` with `status:ready-for-commit`). If a
later run of this same lifecycle reveals a different intended
convention (e.g. the project wants Verified set earlier), update this
memory rather than re-deriving it from scratch.

**Don't blanket-advance every RTVM ID an issue's title lists** — check
per-ID whether a dedicated test procedure actually ran. Confirmed
2026-09-03 on issue #8 (CORE-200/210/220/230/240): SE and TE both
explicitly flagged that TP-220/230 (runtime and resource-tier checks)
have no test yet because they need the containerized CLI, which
doesn't exist. Only CORE-200/210/240 (which had component-level tests
actually execute and pass) moved to In Test; CORE-220/230 stayed
Approved. Read the SE/TE comments for per-requirement caveats like
this rather than assuming a uniform PASS covers every ID in the
issue title.

**Post-Verified regression close-out needs no RTVM edit if nothing
changed.** After CI/CD's commit confirmation sets an item to Verified
with a SHA, that item's chain still routes through one more
regression-testing round-trip (CI/CD flagged trunk merge → TE
regression → back to you). Confirmed 2026-09-03 on issue #8: TE's
regression PASS reported code unchanged since the merge SHA already
recorded in RTVM, so the correct action was to re-confirm the existing
Verified/SHA entries (no edit), comment confirming, and close the
issue — not to re-run the RTVM update step. Only re-touch RTVM here if
TE's regression comment reports something that actually changed
(different SHA, a regression found, etc).
