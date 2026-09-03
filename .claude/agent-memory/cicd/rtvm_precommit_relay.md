---
name: rtvm-precommit-relay
description: Systems Engineer commits RTVM status-only edits (Approved -> In Test) directly to main, ahead of and separate from CI/CD's feature-branch merge — this is normal, not a merge conflict to resolve.
metadata:
  type: project
---

On issue #9, Systems Engineer's "RTVM updated" hand-off comment pointed
to a commit (21a2517, "RTVM: mark UI-001, DATA-OUT-300, ... In Test")
that was already sitting on `main`, not on the `issue-9` feature
branch. The feature branch itself never touched `docs/RTVM.md`.

**Why:** this is the established convention across issues #7/#8/#9 —
Systems Engineer relays a Test Engineer PASS by editing the RTVM
status column straight on trunk (Commit(s) column left blank, to be
filled in after CI/CD's merge SHA is known), independently of whatever
branch the actual code lives on.

**How to apply:** when checking `git diff <branch>...main --stat`
before a merge, don't be surprised if `docs/RTVM.md` isn't in the
feature branch's diff at all — check `git log --all --grep RTVM` /
`git branch --contains <sha>` to confirm the RTVM commit is already on
main before assuming something's missing. After merging, per your own
role's boundary, still don't edit `docs/RTVM.md` yourself (recording
the merge SHA into the Commit(s) column is Systems Engineer's job on
the next hand-off), and don't add a `status:*` label when handing back
to Systems Engineer post-merge — none applies to this transition, just
`agent:systems-engineer`.
