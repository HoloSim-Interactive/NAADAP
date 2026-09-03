---
name: shallow-clone-gotcha
description: This environment's git checkout is shallow by default, which breaks merge-base/diff calculations before a trunk merge.
metadata:
  type: project
---

The working copy starts as a shallow clone (`git rev-parse
--is-shallow-repository` → `true`). In a shallow clone, `git
merge-base <trunk> <branch>` can fail outright (exit 128, "unknown
revision") or `git log <trunk>..<branch>` can print misleading/stale
results, because history beyond the shallow horizon isn't present
locally even though `git log --all --graph` renders it as if it were
a normal straight line.

**Why:** Discovered on issue #5's merge — `git merge-base main
issue-5` errored, and a first `main..issue-5` listing looked like the
whole branch history was unmerged when in fact most of those commits
were already common ancestors. Merging or tagging off a
misdiagnosed diff risks the wrong commit being merged or a
double-count on BUILD.

**How to apply:** Before computing any diff, merge-base, or ahead/behind
count as part of a trunk merge, run `git fetch --unshallow origin`
first (cheap, one-time per checkout) and re-verify with `git
rev-list --left-right --count trunk...branch`. Do this before trusting
any `git log branch..trunk` / `trunk..branch` output.
