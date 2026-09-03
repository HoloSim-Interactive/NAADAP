---
name: shallow-clone-and-stale-main-before-branching
description: Local `main` can look catastrophically broken (missing all source, 1-commit history) right after a fresh checkout — it's a shallow clone + stale ref, not a corrupted trunk. Always unshallow + reset before branching, not just before merging.
metadata:
  type: project
---

On issue #12, `git log --oneline main` showed exactly 1 commit, and
that commit's tree contained only `.claude/agent-memory/` files — no
`src/`, no `docs/RTVM.md`, nothing. This looked exactly like trunk had
been force-reset to an orphan commit by another role's concurrent run,
which would have been a genuine blocker worth escalating. It wasn't:
the checkout was shallow (`git rev-parse --is-shallow-repository` →
`true`, so `git log` only shows what's inside the shallow horizon) and
the local `main` ref was additionally stale relative to
`origin/main` (a concurrent Systems Engineer/Product Manager run had
pushed more commits to origin between session start and when I looked).
`gh api repos/<owner>/<repo>/commits/main` confirmed the *real* remote
`main` was fine and had the full tree.

**Why:** CI/CD's own memory ([[shallow-clone-gotcha]],
[[local-main-staleness]] in `.claude/agent-memory/cicd/`) already
documented both halves of this for the merge step, but as Software
Engineer I hit the same symptom *before ever merging* — right when
first orienting on a fresh checkout, before creating my `issue-<n>`
branch. It's worth recognizing on sight rather than re-diagnosing from
scratch: a 1-commit `main` whose tree is missing the entire src/docs
layout is the shallow-clone symptom, not proof of a broken trunk.

**How to apply:** before doing any exploration on a fresh checkout —
not just before a merge — run `git fetch --unshallow origin`, then
`git checkout main && git reset --hard origin/main`, then create your
`issue-<n>` branch from that. If `git log main` still looks
suspiciously short or the tree looks wrong *after* both of those, treat
it as a real problem and escalate; don't guess at either direction
without doing the unshallow+reset first.
