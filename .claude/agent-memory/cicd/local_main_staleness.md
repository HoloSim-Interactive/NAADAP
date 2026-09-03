---
name: local-main-staleness
description: Local `main` branch ref can be stale even right after `git fetch --unshallow` — always reset to origin/main immediately before merging, not just fetch.
metadata:
  type: project
---

On issue #11's merge, `git fetch --unshallow origin` (done to fix the
[[shallow-clone-gotcha]] issue) updated remote-tracking refs, but the
local `main` branch pointer itself was not moved by that fetch.
Meanwhile issue #13 (Systems Engineer's concurrent doc-lock/formatting
work) had landed a new commit directly on `origin/main` after that
fetch. The first `git merge --no-ff issue-11` on `git checkout main`
silently merged into the *stale* local `main` (missing that new
commit) — `git status` even printed "Your branch is behind
'origin/main' by 1 commit" right after the merge, which is the tell.

**Why:** other agents (Systems Engineer especially) can push directly
to trunk between when you fetch and when you actually run the merge —
there's no lock preventing it. A merge built on a stale local branch
ref either loses that concurrent commit or (worse) creates a
divergent history that needs re-doing.

**How to apply:** immediately before `git merge --no-ff <branch>` on
`main`, run `git fetch origin main` then `git reset --hard
origin/main` (local main should never carry unpushed work, so this is
always safe) — don't rely on an earlier fetch in the same session
being current. Re-verify with `git log origin/main -1` matching `git
log -1` right before merging. If you already merged into a stale
base, `git reset --hard origin/main` and redo the merge before
pushing (safe as long as you haven't pushed yet).
