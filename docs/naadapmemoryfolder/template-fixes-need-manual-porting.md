---
name: template-fixes-need-manual-porting
description: "NAADAP's agent-relay.yml diverged from =TEMPLATE= (ubuntu runner, concurrency split, self-driving dispatch) — fixes committed to =TEMPLATE= don't reach NAADAP automatically and must be diffed/ported by hand."
metadata: 
  node_type: memory
  type: project
  originSessionId: 0e63a960-1002-453e-963a-6e2ce6b904e0
  modified: 2026-09-03T16:13:27.284Z
---

`HoloSim-Interactive/NAADAP` was duplicated from `Holosim/-TEMPLATE-`
at kickoff, then its `.github/workflows/agent-relay.yml` diverged
significantly from the template in this session: GitHub-hosted
`ubuntu-latest` instead of self-hosted (public-repo constraint),
concurrency-group split + `GITHUB_TOKEN` override for the no-op-run
fix, and the self-driving end-of-run dispatch to `dependency-check`/
`lock-retry`. See [[navair-github-hosted-runners]].

**Consequence:** a fix committed to `=TEMPLATE=` does NOT propagate to
NAADAP on its own — there's no ongoing sync relationship, just a
one-time fork. Confirmed 2026-09-03: `=TEMPLATE=` commit `7b43ef8`
fixed the failure classifier's `overloaded_error` pattern (the API's
real field is bare `"overloaded"`, so every genuine Anthropic-side
service overload was falling through to a bare `needs-human` park
instead of the retry-1/2/3 ladder). NAADAP had the identical pre-fix
line at `.github/workflows/agent-relay.yml:412` until ported by hand as
commit `c2b5d80`, discovered only because the client explicitly flagged
a live status.claude.com incident (Opus 5/4.8 elevated errors,
2026-09-03 13:26-15:25+ UTC) and referenced the template commit.

**Why this matters:** NAADAP's SE and PM runs on issue #13 executed
squarely inside that outage window and happened not to hit the bug —
but the exposure was real, and nothing would have surfaced it
automatically. `=TEMPLATE=`'s own `docs/reference/console-advisor-brief.md`
(also ported, commit `c2b5d80`) has the diagnostic shape: several
issues parking `needs-human` within minutes of each other, each with a
tiny near-identical `total_cost_usd`, and `"error_status": 529` /
`"error": "overloaded"` in the run log — that pattern means check
`status.claude.com` before assuming a real bug.

**How to apply:** when the client mentions a fix landed in `=TEMPLATE=`
(or when diagnosing an unexplained NAADAP relay failure), diff
`=TEMPLATE=`'s `.github/workflows/agent-relay.yml` and
`docs/reference/console-advisor-brief.md` against NAADAP's own copies
rather than assuming either is current — `/c/_Dev/GIT/=TEMPLATE=` is
the local clone, remote `https://github.com/Holosim/-TEMPLATE-.git`.
Port by hand, adapting line numbers/context to NAADAP's already-diverged
file; do not overwrite NAADAP's own ubuntu/concurrency/self-driving
changes with the template's stock content.
