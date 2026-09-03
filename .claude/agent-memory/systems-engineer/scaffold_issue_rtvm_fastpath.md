---
name: scaffold-issue-rtvm-fastpath
description: How to handle status:ready-for-rtvm-update on an infra/scaffold issue (e.g. Generate Code Base) that isn't tied to a single RTVM ID.
metadata:
  type: feedback
---

Generate Code Base (and any similar pure-scaffolding issue) doesn't
verify a specific RTVM item, even when Test Engineer's PASS comment
lists RTVM IDs whose *structural* constraints happen to check out
(e.g. NAADAP issue #5 touched CORE-240 zero-deps, DELIV-910 sln/csproj
shape, DELIV-920 dependency-justification convention, NFR-500
no-restore-at-runtime Dockerfile design).

**Why:** each of those IDs has its own dedicated downstream issue in
`docs/IMPLEMENTATION_PLAN.md` that will run the real functional
Test/Demonstration and move it Approved → Verified (see
[[naadap-implementation-plan]]). Flipping the Status column to
Verified off a scaffold-only build preempts that later issue's actual
test and leaves nothing for it to verify.

**How to apply:** on the `status:ready-for-rtvm-update` fast path, if
the issue's own scope explicitly disclaims verifying any single RTVM
item, don't force a Status-column edit — say plainly in the comment
that no RTVM change applies and why (name the future issue that
actually owns each touched ID), then still hand off to `agent:cicd`
with `status:ready-for-commit` as normal, since the code still needs
to land. Don't skip the CI/CD hand-off just because RTVM didn't
change — those are separate concerns.
