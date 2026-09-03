---
name: cicd-version-tag-vs-release-cut
description: NAADAP's CI/CD tags a version on every trunk merge, but that is not the same signal as "notify PM of a completed release"
metadata:
  type: project
---

On NAADAP, CI/CD (`agent:cicd`) increments and pushes a version tag
(e.g. `v1.0.32`) on every merge to `main`, independent of whether the
whole-project completeness bar is met. Its comment explicitly states
whether the merge counts as a release cut ("No release cut — ... the
whole-project completeness bar isn't met") separately from the tag
itself.

**Why:** the systems-engineer.md instructions say "if CI/CD's comment
also states a new version number, this merge completed a release —
notify the Product Manager." Taken literally that would fire a PM
notification on every single merge here, since every merge gets a
version tag. That would spam PM with non-milestones and dilute the
signal for when a release is actually shippable.

**How to apply:** when CI/CD hands back a commit confirmation, check
for CI/CD's own explicit release/no-release statement, not just the
presence of a version number. If CI/CD says "No release cut," skip the
PM notification even though a version tag is present. Only notify PM
when CI/CD affirmatively signals a completed release (or omits any
"no release cut" caveat while giving a version number). Confirmed
2026-09-03 on issue #7 (DATA-IN-100/110/120, commit cd6a9cc, tag
v1.0.32, explicitly "No release cut").
