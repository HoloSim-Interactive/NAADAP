# Test Engineer — memory

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

## Test harness notes

## Platform-specific test considerations

## Recurring failure patterns

## Flaky tests

- [docker build transient 502](docker_build_transient_502.md) — a `docker build` failure at the image-resolve/auth stage (not inside the project's own steps) is often Docker Hub flakiness; retry once before reporting FAIL.
