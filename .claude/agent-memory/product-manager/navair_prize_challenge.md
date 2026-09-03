---
name: navair-prize-challenge
description: What this project actually is — a NAVAIR/NAWCAD Prize Challenge submission, not an internal product — and the client's fixed technical constraints.
metadata:
  type: project
---

This project is HoloSim's entry to the NAVAIR/NAWCAD "Advanced
Acquisition Documentation Analysis" Prize Challenge (via Central
Florida Tech Grove). The "product" is a recommender system that
clusters procurement documents (SOWs, PWSs, CDRLs, sources sought
notices, open-source text like Congressional testimony) and
recommends candidate strategic contract vehicles for consolidation.
The deliverable is a Phase 2 competition submission, not a standalone
internal tool — read requirements through that lens (challenge rules
in issue #1's body are effectively a second client whose constraints
are non-negotiable, alongside HoloSim itself).

Client-mandated technical constraints (2026-09-03, issue #1 comment,
all [CONFIRMED] in `docs/PROJECT_DEFINITION.md`):
- Language: C#. Source code is a must-have deliverable.
- Final IDE target is Microsoft Visual Studio. This is a PACKAGING
  step, not a port: a `.sln`/`.csproj` built on Ubuntu opens directly
  in Visual Studio. Build on Ubuntu with the .NET 9.0 SDK.
- NOT CMake. The kickoff comment said "CMake"; the client corrected
  this on 2026-09-03. C# builds with MSBuild via the `dotnet` CLI.
  Keep the core on plain `net9.0` (no `net9.0-windows`, WPF, WinForms)
  — it must also run in a Linux IL4 Docker container.
- Avoid 3rd-party plugins/software unless explicitly necessary.
- SDLC must follow MBSE (Model-Based Systems Engineering).
- Team process must follow Agile Systems Engineering best practices.
- Documentation must follow the US Navy SETR (Systems Engineering
  Technical Review) process and produce the SETR article set (SDD,
  PDR, CDR, PEDDAL, etc.).

**Why this matters going forward:** any future feature or decision
should be checked against these constraints by default — they came
from the client directly, not inferred. In particular, don't let
scope creep introduce a 3rd-party dependency or non-C# component
without flagging it back to the client first.

**Open reconciliation issue (flagged to Systems Engineer,
2026-09-03):** this repo's pipeline already produces its own
`docs/SDD.md` / `docs/RTVM.md` / `docs/IMPLEMENTATION_PLAN.md`
artifacts through the agent pipeline. How those map onto the client's
required SETR article set (SDD, PDR, CDR, PEDDAL) — which are
equivalent, which are additional, whether PDR/CDR are documents or
review *events* — is unresolved. See [[open-questions-2026-09]].
