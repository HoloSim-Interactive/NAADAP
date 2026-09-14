---
name: navair-github-hosted-runners
description: NAADAP is public, so self-hosted runners can never schedule on it; it runs on GitHub-hosted ubuntu-latest with free unlimited minutes.
metadata:
  type: project
---

`HoloSim-Interactive/NAADAP` (created 2026-09-03) runs its
agent relay on `ubuntu-latest`, NOT on the org's self-hosted Windows
runners, and this is permanent rather than a stopgap.

The repo is **public**. GitHub refuses to schedule self-hosted runners for
public repositories unless the runner group opts in, and the org's Default
group has `allows_public_repositories: false`. A `runs-on` that matches no
runner does not error — it queues silently forever. That is what stalled
the kickoff's first two relay runs for ~24 minutes on 2026-09-03.

Public repos also get unlimited free minutes on GitHub-hosted standard
runners, and the Free org plan allows 20 concurrent jobs. Since the relay's
concurrency group is per-issue, that is up to 20 hand-offs in parallel —
far more than the 2 shared Windows runners could ever give.

**Why:** the =TEMPLATE= ships `runs-on: [self-hosted, agents]` and a
Windows-only `path_to_claude_code_executable`, both fossils of projects
whose GitHub minutes were exhausted. Copying the template into a public
repo inherits a configuration that cannot run.

**How to apply:** for any new public repo from this template, switch
`agent-relay.yml` and `usage-reset-resume.yml` to `ubuntu-latest` and
delete `path_to_claude_code_executable` before the first hand-off. If a
relay run sits "queued"/"pending" with no runner, suspect label matching,
not capacity. Moving the repo out of the org is NOT needed and would gain
nothing. See [[navair-prize-challenge]].
