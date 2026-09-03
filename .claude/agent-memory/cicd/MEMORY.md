# CI/CD — memory

**This file is an index, not a store.** It is loaded on every run you
ever do, so anything verbose here is re-read on every future hand-off
for the rest of the project. Keep each entry to one line: a link and a
one-sentence summary. Put the actual detail in its own file in this
folder.

    - [Short title](descriptive_slug.md) — one sentence on what it is.

A genuinely one-line fact can stay a plain line with no file of its
own. Split a lesson out when it needs a reproduction, a command
sequence, or real reasoning to be useful later. See "Memory structure"
in `.github/AGENT_LABELS.md`.

## Branching conventions

- Branch names are `issue-<number>`; merge to `main` with `--no-ff` so
  the merge commit is a discrete, dated marker for that issue's work.

## Build & toolchain notes

- [Shallow clone gotcha](shallow_clone_gotcha.md) — `git fetch
  --unshallow` before trusting any merge-base/diff/ahead-behind
  calculation ahead of a trunk merge.
- [Local main staleness](local_main_staleness.md) — `git reset --hard
  origin/main` immediately before merging, every time; a fetch minutes
  earlier isn't proof local `main` is current (issue #11, v1.0.67).
- `dotnet build Naadap.sln` builds clean (0 warnings/errors) on this
  environment's `dotnet 10.0.400` SDK targeting the project's `net9.0`
  projects — safe to re-verify build (not full `dotnet test`) as a
  quick sanity check before merging C# work.

## Release & versioning

- NAADAP had no `VERSION` file before issue #5's merge (4b1b3b2) —
  created it as `1.0`, this project's first release cycle. Future
  merges: read it, don't write it, unless SE/PM deliberately bump it.
- No RTVM item in `docs/RTVM.md` reaches Verified from a scaffold-only
  issue (e.g. #5, "Generate Code Base") — expect tag-only merges, no
  `gh release create`, until real functional issues land.

## Known issues

- [Fixture-only merges](fixture_only_merges.md) — data/fixture-only issues (#6) skip the dotnet build check and stay tag-only (v1.0.24) since no RTVM item verifies.
- [RTVM pre-commit relay](rtvm_precommit_relay.md) — Systems Engineer commits RTVM status edits straight to main ahead of CI/CD's feature-branch merge (issue #9, v1.0.48); don't expect `docs/RTVM.md` in the feature branch's diff.
- [RTVM "In Test" before CI/CD](rtvm_in_test_before_cicd.md) — rows can be In Test with a blank commit cell at merge time; only count Verified rows toward the whole-table release check (issue #12, v1.0.82).
