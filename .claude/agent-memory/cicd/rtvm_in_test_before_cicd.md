---
name: rtvm-in-test-before-cicd
description: RTVM rows can sit at "In Test" (not yet "Verified") with a blank Commit(s) column when CI/CD gets the hand-off — that's expected, not a sign something's missing.
metadata:
  type: project
---

On issue #12, Systems Engineer advanced DELIV-900/920/930/940/960/970
from Approved straight to **In Test** (not Verified) after Test
Engineer's PASS, explicitly leaving the Commit(s) column blank "to be
filled in by CI/CD's confirmation." Per [[rtvm_precommit_relay]],
CI/CD doesn't edit `docs/RTVM.md` itself — it reports the merge SHA in
its hand-off comment, and Systems Engineer is the one who later writes
it into the table (and presumably advances status to Verified then).

**Why:** keeps `docs/RTVM.md` the single source of truth, written by
one role only (Systems Engineer), rather than split between SE and
CI/CD — this project's stated design goal.

**How to apply:** when deciding whether a trunk merge completes a
release (whole-table check per cicd.md), only rows already marked
**Verified** count. Rows at **In Test** with a blank commit column at
the moment you're merging are normal and not yet part of the release
math — don't wait on them or treat the blank cell as a problem to fix
yourself.
