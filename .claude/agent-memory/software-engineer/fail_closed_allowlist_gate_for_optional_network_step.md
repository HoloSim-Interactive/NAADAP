---
name: fail-closed-allowlist-gate-for-optional-network-step
description: How CORE-250's optional LLM step structurally guarantees zero network calls when disabled/misconfigured, for any future optional-and-restricted outbound-call feature.
metadata:
  type: project
---

When a requirement needs "no outbound calls except to a configured
allowlist, and zero calls at all when the feature is off" (NFR-510 /
CORE-250 in this project), the pattern used in `Naadap.LlmStep`:

- The orchestrator (`LlmSummarizationStep.RunAsync`) checks every
  precondition — feature enabled, endpoint configured, endpoint on the
  allowlist, prompt within token budget — **before** it ever
  constructs a real `IModelClient`/`HttpClient`. Each failure returns
  a `Skipped` result with a reason; none of them throw, and none of
  them reach the network. This makes "zero network calls when
  disabled" a structural property of the call graph (the caller in
  `Naadap.Cli.Program` doesn't even construct `HttpModelClient` when
  the flag is off), not just a runtime check that could be bypassed by
  a code path nobody re-checked.
- A thin decorator (`AllowlistEnforcingModelClient : IModelClient`)
  re-checks the allowlist one layer down and records every attempt
  (allowed or refused) to an audit log, as defense-in-depth — it
  should never actually be the thing that catches a violation in
  practice, since the orchestrator already refused earlier.
- The allowlist itself is **operator-supplied config** (environment
  variables here), never a hardcoded vendor list — this project has no
  way of knowing the real USN-approved endpoint list, and the
  test procedure's own wording ("matches the configured allowlist")
  confirmed that's intentional, not a gap to fill in with a guess.
- The audit trail (token usage + every call attempt) is written to its
  own file in the output directory, independent of the
  already-verified `RunManifest`/`manifest.json` schema — an optional,
  independently-testable feature doesn't need to touch a sibling
  feature's verified contract to prove its own behavior.

Reusable whenever a future optional feature needs "off by default,
allowlist-restricted when on, auditable either way."

**Related:** [[naadap_shared_dtos_live_in_core]] (Core/Output schema
stability), [[derive_thresholds_empirically_against_fixtures]] (same
"measure, don't guess" instinct applied to a budget/threshold).
