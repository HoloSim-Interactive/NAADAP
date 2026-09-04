# NAADAP session handoff

Prepared 2026-09-04 to move work on `HoloSim-Interactive/NAADAP` from
one computer to another (local or cloud). **Paste this entire document
as the first message in the new session**, then ask Claude to save it
to memory — that seeds it with everything this project's history
depends on, regardless of which machine or session type you're on.

The project itself needs no migration: working tree was clean and
`HEAD` matched `origin/main` at `c2b5d80` when this was written. Only
this reasoning/context is local to the old machine — everything else
is already on GitHub.

---

## 1. What NAADAP is (type: project)

`HoloSim-Interactive/NAADAP` is HoloSim's entry to the NAVAIR/NAWCAD
prize challenge (Central Florida Tech Grove) — build a recommender
that clusters procurement documents (SOWs, PWSs, CDRLs, Congressional
testimony) and recommends strategic contract vehicles for
consolidation. **$150K prize pool ($100K/$50K), hard external
deadline.** The client is competing to win, not doing open-ended R&D.

"NAADAP" is deliberately internal shorthand — the repo is **public**,
so the challenge's full title was scrubbed from repo content to avoid
handing competitors a search string. References to NAVAIR/NAWCAD
themselves were kept, on the client's explicit instruction — only the
challenge title is obscured.

Constraints that shape every architecture decision:

- **Dockerized and offline.** Runs in a US-Government IL4-accredited
  cloud; no external services, no external industry-hosted models. Any
  LLM/embedding step ships inside the container.
- **Determinism gate:** the identification methodology must produce
  the same top-5 results ≥95% of the time. Only the final
  summarization step may be stochastic.
- **C# is a client mandate**, source code is a deliverable, and a
  Visual Studio solution is wanted at the end (see §3 below — this is
  a packaging step, not a port).
- Must ship a visualization of method + results, plus a summary
  performance metric.

**How to apply:** check any proposed dependency against "does this run
inside a sealed container with no egress." Prefer ONNX Runtime / ML.NET
with model weights vendored into the image. Treat the determinism gate
as a testable requirement with fixed seeds, not an aspiration.

---

## 2. Runners: GitHub-hosted, permanently (type: project)

The agent relay runs on `ubuntu-latest` — GitHub-hosted VMs — not the
org's self-hosted Windows runners, and this is **permanent**, not a
stopgap.

The repo is public. GitHub refuses to schedule self-hosted runners on
public repos unless the runner group opts in, and
`HoloSim-Interactive`'s Default group has
`allows_public_repositories: false`. A `runs-on` that matches no
runner does not error — **it queues silently forever.** That's what
stalled the kickoff's first two relay runs for ~24 minutes before this
was diagnosed.

Public repos also get unlimited free minutes on GitHub-hosted standard
runners, and the Free org plan allows 20 concurrent jobs. The relay's
concurrency group is per-issue, so that's up to 20 hand-offs running
in parallel — far more than the shared Windows runners could give.

**How to apply:** if a relay run ever sits "queued"/"pending" with no
runner picking it up, suspect label/runner-group mismatch, not
capacity. Moving the repo out of the org is not needed and would gain
nothing.

---

## 3. C# on Ubuntu is not a port (type: reference)

Developing on Ubuntu and delivering a Visual Studio solution are the
**same artifact**, not two phases. `dotnet new sln` produces `.sln`/
`.csproj` files Visual Studio opens directly — no conversion step. The
.NET SDK is preinstalled on `ubuntu-latest`.

- **C# does not use CMake.** Build system is MSBuild via `dotnet
  build`/`dotnet test`. The client's original kickoff note said "C# on
  Ubuntu with CMAKE" — that was corrected at the source (see §6); if
  "CMake" ever resurfaces in a new document, it's wrong.
- **The only real portability trap is a Windows-only target
  framework** — `net9.0-windows`, WPF, WinForms. Keep the core on
  plain `net9.0` and the VS hand-off costs nothing — and it has to,
  since the deliverable also ships in a Linux Docker container.

**Why it matters:** budgeting a late "convert to Visual Studio" phase
would spend schedule that doesn't need to exist, on a challenge with a
hard external deadline.

---

## 4. The `**Next:**` line is machine-parsed (type: project)

In the `=TEMPLATE=` agent pipeline (NAADAP, and its sibling projects
Drone_Tracks, DungeonMaster), the last comment's `**Next:**` line
isn't prose — `stall-recovery.yml` runs `scripts/reconcile-labels.sh`
hourly and greps that exact line to decide whether an issue is
deliberately parked on a human or genuinely stalled.

The template shipped that test matching only the literal string
`waiting on human reply`. On NAADAP the Product Manager parked an
issue with `**Next:** waiting on client answers to the 4 questions
above` — same state, different words — which didn't match, so the
sweep would have retriggered the agent every hour, each run re-posting
the same unanswered questions.

**Fixed on NAADAP** (commit `2ad3fca`) by matching the waiting
*shape* instead: `(waiting|blocked|pending) (on|for) (a|an|the)?
(human|client|customer|user|stakeholder)`. Note `waiting on the lock`
deliberately still does NOT match — it has its own
`status:waiting-on-lock` pause label and must not be swallowed here.

**How to apply:** when writing a `**Next:**` line as any role, use the
canonical phrasing regardless. If an agent keeps re-running and
re-asking the same thing, check this regex before suspecting the relay
itself. The narrow, unfixed version is still present in Drone_Tracks
and DungeonMaster — port the fix there if it ever bites.

---

## 5. Template fixes don't propagate automatically (type: project)

`HoloSim-Interactive/NAADAP` was duplicated from `Holosim/-TEMPLATE-`
at kickoff, then NAADAP's own `.github/workflows/agent-relay.yml`
diverged significantly: GitHub-hosted `ubuntu-latest` (§2),
concurrency-group split + `GITHUB_TOKEN` override (a no-op status-label
run could cancel a real agent pickup before any job was created — now
fixed, commit `96631fd`), and a self-driving end-of-run dispatch to
`dependency-check`/`lock-retry` so the pipeline doesn't depend on
GitHub's cron delivery alone (commit `b1dcf8d` — cron demoted to
fail-safe after a real 2h37m scheduled-event outage on 2026-09-03).

**Consequence:** a fix committed to `=TEMPLATE=` does **not** reach
NAADAP on its own — it's a one-time fork, not an ongoing sync.
Confirmed concretely 2026-09-03: `=TEMPLATE=` commit `7b43ef8` fixed
the failure classifier's `overloaded_error` pattern (the API's real
error field is bare `"overloaded"`, so every genuine Anthropic-side
service overload fell through to an immediate `needs-human` park
instead of the retry-1/2/3 ladder). NAADAP had the identical unfixed
line until ported by hand as commit `c2b5d80` — found only because
the client flagged a live status.claude.com incident and named the
template commit.

That incident is also documented in `docs/reference/console-advisor-brief.md`
(ported alongside): the diagnostic shape of a genuine Anthropic-side
overload — several issues parking `needs-human` within minutes of each
other, each with a tiny near-identical `total_cost_usd`, and
`"error_status": 529` / `"error": "overloaded"` in the run log — versus
a real classifier bug, which can look identical until the log is
actually read.

**How to apply:** when told a fix landed in `=TEMPLATE=` (or when
diagnosing an unexplained NAADAP relay failure), diff
`=TEMPLATE=`'s `.github/workflows/agent-relay.yml` and
`docs/reference/console-advisor-brief.md` against NAADAP's own copies —
don't assume either is current. Local clone (old machine):
`/c/_Dev/GIT/=TEMPLATE=`; remote: `https://github.com/Holosim/-TEMPLATE-.git`.
Port by hand, adapting to NAADAP's already-diverged file — never
overwrite NAADAP's own ubuntu/concurrency/self-driving changes with
the template's stock content.

---

## 6. Cross-reference and anchor convention (type: feedback — standing rule)

Client directive, 2026-09-03, phrased as a standing practice ("should
persist through the rest of all documentation efforts"), not a
one-off:

1. Any document defining indexed, individually-referenceable points —
   Stakeholder Needs (`SN-#`), requirement categories (`UI-#`,
   `CORE-#`, `DATA-IN-#`, `DELIV-#`, etc.), Test Procedures (`TP-#`),
   or any other specific location worth pointing at directly — gets an
   anchor at that point.
2. **Anchor ID format is strict:** all lowercase, must start with a
   letter (never a digit), nothing but lowercase letters, digits, and
   hyphens — no underscores, no spaces, no other punctuation. Prefix
   every ID with the owning document's short code so IDs stay unique
   project-wide: `rtvm-core-200`, `pd-sn-3`.
3. Every *reference* to one of these (or to another document
   generally) is an inline **HTML** hyperlink —
   `<a href="..." target="_blank">` — never markdown's `[text](url)`
   shorthand. Client's stated reason: markdown links don't reliably
   focus/scroll the target page to the anchor; the HTML form does.
4. Applies everywhere: `.md` files, comments in scripts, issue
   descriptions, issue comments.

**One judgment call, not explicitly stated by the client:** git commit
messages are exempt — GitHub doesn't render HTML there, so an `<a>` tag
would be inert noise, not a link. Flagged explicitly when applying;
revisit if the client pushes back.

**Status as of this handoff:** full spec lives in
`.github/AGENT_LABELS.md`'s "Cross-reference and anchor convention"
section, with pointers added to all 8 `.claude/agents/*.md` files
(commit `ca7070f`). Retroactive application to the project's own
content documents — `docs/PROJECT_DEFINITION.md`, `docs/RTVM.md`,
`docs/SDD.md`, `docs/IMPLEMENTATION_PLAN.md` — was completed via issue
#13 (Systems Engineer did RTVM/SDD/Plan + its own issues, Product
Manager did PROJECT_DEFINITION.md + issue #1, both closed out
2026-09-03). One item Product Manager explicitly deferred rather than
guessed on: the bare `SN-#` mentions inside RTVM.md's own
Stakeholder-Need column (30 rows) and scattered prose in SDD.md are
now linkable (the `pd-sn-#` anchors exist) but still plain text —
Systems Engineer's call whether that's worth a dedicated follow-up
issue.

**How to apply:** this reads as a general documentation preference,
not NAADAP-only — apply it on any project with indexed requirements
docs, not just this one.

---

## How to use this on arrival

1. Confirm the project state is current: `git -C <path-to-NAADAP-clone> log -1` should be at or ahead of `c2b5d80`.
2. Save each numbered section above as its own memory file (type noted in each heading), and add one-line index entries — same structure as before, so future sessions don't have to re-read all six in full every time.
3. If this is a **cloud session**, there's no local `.github/copilot-instructions.md` file to worry about — that untracked file only exists on the old machine's local checkout and was never committed; it can be ignored or the user can bring it separately if it matters to them.
