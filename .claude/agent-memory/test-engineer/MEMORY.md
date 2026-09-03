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

- [Consolidation-issue TP battery](consolidation_issue_tp_battery.md) — checklist for a DELIV-9xx-style multi-TP consolidation issue; TP-910 (Windows/VS) is legitimately NOT-RUN here, not a fail.
- [Post-merge regression hand-off](post_merge_regression_handoff.md) — pattern for CI/CD-flagged trunk regression passes: test on `main` (unshallow first if needed), PASS routes back to systems-engineer via `status:ready-for-rtvm-update`.
- [Network allowlist egress test pattern](network_allowlist_egress_test_pattern.md) — no real iptables control in this sandbox; test outbound-allowlist requirements via `--network none` (negative case) + host stub server via `--add-host=host.docker.internal:host-gateway` (positive case), reading the app's own audit log.

## Platform-specific test considerations

## Test harness gotchas (mine, not the product's)

- [.NET HttpClient chunked-encoding mock gotcha](dotnet_httpclient_chunked_mock_gotcha.md) — a hand-rolled Content-Length-only mock server will falsely look like a client bug against .NET's chunked-encoded POSTs; also, use Bash's `run_in_background` not `nohup ... &` for helper servers in this sandbox.

## Recurring failure patterns

- [Fixture ground-truth spot-check](fixture_ground_truth_spotcheck.md) — when spot-checking a corpus + hand-derived ground truth, re-verify the rationale claiming the *strongest* evidence against actual extracted text, not just the README paraphrase (caught an overstated "named directly in the title" claim on issue #6 that wasn't in the actual PDF text).
- [Threshold derivation recompute](threshold_derivation_recompute.md) — don't trust a claimed numeric derivation for a threshold constant (e.g. similarity cutoffs); scaffold a throwaway console app referencing the real project and recompute it yourself (found stale figures on issue #8, conclusion still held).

## Flaky tests

- [docker build transient 502](docker_build_transient_502.md) — a `docker build` failure at the image-resolve/auth stage (not inside the project's own steps) is often Docker Hub flakiness; retry once before reporting FAIL.
