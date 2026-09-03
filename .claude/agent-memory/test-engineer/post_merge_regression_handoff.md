---
name: post-merge-regression-handoff
description: Label/hand-off pattern when CI/CD flags a trunk merge as needing regression testing
metadata:
  type: project
---

When CI/CD merges to trunk and flags "regression testing needed," Systems
Engineer hands the issue back to `agent:test-engineer` (after first
recording the merge SHA in `docs/RTVM.md` as Verified). The regression
pass itself:

- Check out/pull `main` (trunk), not the issue branch — the merge is
  already on trunk. If the local clone is shallow (`git rev-parse
  --is-shallow-repository`), run `git fetch --unshallow` first so
  `git log`/`merge-base` checks against the merge SHA actually work.
- Re-run full build + full test suite + format check, and manually
  re-exercise the fixture-based test procedures named in the RTVM for
  the requirements this merge touched (not just the new ones — confirm
  older regressions like DATA-IN-110's corrupted-file skip still hold).
- On PASS: hand off to `agent:systems-engineer` with
  `status:ready-for-rtvm-update` — same label as a normal first-pass.
  Systems Engineer then re-confirms the existing Verified/SHA RTVM
  entries (no edit needed if nothing changed) and closes out. Confirmed
  on issue #9 (2026-09-03), consistent with the pattern systems-engineer
  recorded from issue #8. Reconfirmed on issue #11 (2026-09-03,
  NFR-500/510/520/530, Docker packaging pass) — same shape: re-ran full
  Docker build + TP-500/510/520/530 fixture procedures against `main`
  post-merge, all passed identically to the pre-merge run, RTVM already
  correct so no edit needed, straight to systems-engineer.
