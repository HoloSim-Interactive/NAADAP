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
