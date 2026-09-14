---
name: next-line-is-machine-parsed
description: In the agent-relay pipeline the "**Next:**" line is parsed by reconcile-labels.sh, so its wording decides whether a parked issue gets retriggered hourly.
metadata:
  type: project
---

In the =TEMPLATE= agent pipeline (NAADAP, Drone_Tracks, DungeonMaster),
the last comment's `**Next:**` line is not prose — `stall-recovery.yml`
runs `scripts/reconcile-labels.sh` hourly and greps that line to decide
whether an issue is deliberately parked or stalled.

The template shipped that test matching exactly one string, `waiting on
human reply`. On NAADAP the Product Manager parked issue #1 with
`**Next:** waiting on client answers to the 4 questions above` — the same
state in different words — which did NOT match, so the sweep would have
retriggered the agent every hour, each run re-reading an unanswered thread
and re-posting the same questions.

Fixed on NAADAP 2026-09-03 (commit 2ad3fca) by matching the waiting
*shape* instead: `(waiting|blocked|pending) (on|for) (a|an|the)?
(human|client|customer|user|stakeholder)`. Note `waiting on the lock` must
still NOT match — it has its own `status:waiting-on-lock` pause label.

**Why:** an agent parked on a client question looks identical to a stalled
one from the labels alone. The Next line is the only signal that separates
them, which makes its wording load-bearing.

**How to apply:** when writing a `**Next:**` line as any role, use the
canonical phrasing. When diagnosing an agent that keeps re-running and
re-asking the same thing, check this grep before suspecting the relay. The
same narrow match is still present in Drone_Tracks and DungeonMaster —
port the fix if it bites there. See [[navair-github-hosted-runners]].
