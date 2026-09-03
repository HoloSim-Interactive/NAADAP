# Dependency Documentation (DELIV-920 / DELIV-970)

<!--
Owned by the Software Engineer. Produced during the DELIV-9xx
consolidation issue (#12) as the discrete, individually locatable
"dependency documentation" artifact `docs/PROJECT_DEFINITION.md` SN-5
and `docs/RTVM.md` DELIV-970 require, distinct from the algorithm
documentation (`docs/ALGORITHM_COMPARISON.md`) and deployment
instructions (`docs/DEPLOYMENT.md`).

This is a consolidated *view* of the justifications that already live
inline in each `.csproj` (DELIV-920's actual requirement: the
justification must be on-file, next to the reference itself, so it
can't drift from what's actually restored). Do not let this file and
the `.csproj` comments disagree — if a dependency changes, update the
`.csproj` comment first, then reflect it here.
-->

Per `docs/SDD.md`'s dependency policy: every `<PackageReference>`
beyond the .NET 9 BCL requires an inline `<!-- Justification: ... -->`
comment in the `.csproj` next to it. `TP-920` (Inspection) verifies
this table matches those comments exactly — nothing is added to a
`.csproj` without one.

## Third-party NuGet packages

| Package | Version | Project | License | Justification |
| --- | --- | --- | --- | --- |
| `PdfPig` | 0.1.16 | `src/Naadap.Ingestion/Naadap.Ingestion.csproj` | Apache 2.0 | Pure-managed PDF text-extraction library with no native/platform dependencies, satisfying DATA-IN-100's PDF format requirement and the single-Docker-image/no-network-at-runtime constraint (NFR-500). Ingestion-only — never referenced by `Naadap.Core` (CORE-240's zero-dependency rule). |
| `DocumentFormat.OpenXml` | 3.5.1 | `src/Naadap.Ingestion/Naadap.Ingestion.csproj` | MIT | Microsoft's official Open XML SDK; pure-managed DOCX text-extraction library satisfying DATA-IN-100's DOCX format requirement. Same ingestion-only rationale as `PdfPig` above. |

## BCL-covered needs that could have been NuGet packages, but aren't

Called out explicitly so a reviewer diffing "what does this pipeline
depend on" doesn't go looking for a package that was never added:

| Need | Project | Resolution |
| --- | --- | --- |
| HTTP client for the optional LLM step's USN-approved model allowlist (CORE-250) | `src/Naadap.LlmStep/Naadap.LlmStep.csproj` | `System.Net.Http.HttpClient`, part of the `net9.0` BCL — no `PackageReference` needed. |
| JSON serialization for `manifest.json` (OUT-440) and the LLM run log | `src/Naadap.Output`, `src/Naadap.LlmStep` | `System.Text.Json`, part of the `net9.0` BCL/SDK — no `PackageReference` needed. |

## Projects with zero dependencies (beyond other `Naadap.*` projects)

- `src/Naadap.Core` — CORE-240's zero-third-party-dependency rule for
  the production clustering/recommendation path. Only
  `ProjectReference`s to nothing (it is the innermost project).
- `src/Naadap.Cli`, `src/Naadap.Output`, `src/Naadap.Alternative` —
  `ProjectReference`s only, no `PackageReference`s.

## Test projects

Every `tests/Naadap.*.Tests` project references only
`Microsoft.NET.Test.Sdk`, `xunit`, and `xunit.runner.visualstudio` —
the standard .NET test toolchain, not a project-specific choice
requiring its own justification beyond "this is how `dotnet test`
works." These are dev/test-time only; none of them ship in the
published runtime image (see `docs/DEPLOYMENT.md`'s multi-stage build
description).

## How to keep this current (for maintainers)

1. Add the `<PackageReference>` to the relevant `.csproj`.
2. Add an inline `<!-- Justification: ... -->` comment immediately
   above or alongside it, in the same style as the existing entries —
   name the RTVM item it satisfies and why no BCL-only alternative
   works.
3. Add a row to the table above in the same commit. `TP-920` checks
   both files agree.
