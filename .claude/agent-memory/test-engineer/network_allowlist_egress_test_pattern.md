---
name: network-allowlist-egress-test-pattern
description: how to test a container's "outbound calls limited to an allowlisted endpoint" requirement (e.g. NFR-510) without real iptables egress rules in this sandbox.
metadata:
  type: feedback
---

For a requirement like "with feature X enabled, outbound connections are
limited to a configured allowlist" tested via Docker, the sandbox here
doesn't give you a clean way to set up a real network namespace with
egress rules (no iptables control over the runner's Docker daemon
observed). A close-enough two-sided test that still exercises the real
code path:

1. **Negative case** (endpoint not on allowlist): run the container with
   `--network none` at all. If the app's allowlist check happens before
   any socket is opened (verify this in source — e.g. an
   `AllowlistEnforcingModelClient`-style decorator that checks-then-calls),
   the run will still complete successfully under `--network none`,
   proving no connection was even attempted. Assert on the app's own
   audit log (`networkCalls: []`, skip reason) rather than trying to
   packet-sniff.
2. **Positive case** (endpoint on allowlist): stand up a trivial stub
   HTTP server on the host (Python's `http.server`/`BaseHTTPRequestHandler`
   is enough), then run the container with
   `--add-host=host.docker.internal:host-gateway` and point both the
   configured endpoint and the allowlist at
   `http://host.docker.internal:<port>/...`. Confirm the run succeeds and
   the app's own audit log shows exactly that one endpoint, `allowed:true`.

This isn't a literal reproduction of "network namespace with egress
blocked except the allowlist" (TP-510's stated procedure) but is
sufficient evidence when combined with reading the enforcement code,
since the decorator pattern makes the allowlist check structural rather
than best-effort. Note this gap explicitly in the test report if the
Systems Engineer's test procedure demanded literal iptables-level
isolation — call it "verified via audit-log + code-path inspection",
not "verified via network namespace," so nobody assumes more rigor than
what actually ran.

**Why:** used on issue #11 (NAADAP) for NFR-510/TP-510. Both directions
passed and matched the Software Engineer's own claims.

**How to apply:** any future NFR/TP pair about outbound network
allowlisting in a containerized pipeline in this environment.
