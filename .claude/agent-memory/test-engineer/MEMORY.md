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

- [Fixture ground-truth spot-check](fixture_ground_truth_spotcheck.md) — when spot-checking a corpus + hand-derived ground truth, re-verify the rationale claiming the *strongest* evidence against actual extracted text, not just the README paraphrase (caught an overstated "named directly in the title" claim on issue #6 that wasn't in the actual PDF text).
- [Threshold derivation recompute](threshold_derivation_recompute.md) — don't trust a claimed numeric derivation for a threshold constant (e.g. similarity cutoffs); scaffold a throwaway console app referencing the real project and recompute it yourself (found stale figures on issue #8, conclusion still held).

## Flaky tests

- [docker build transient 502](docker_build_transient_502.md) — a `docker build` failure at the image-resolve/auth stage (not inside the project's own steps) is often Docker Hub flakiness; retry once before reporting FAIL.
