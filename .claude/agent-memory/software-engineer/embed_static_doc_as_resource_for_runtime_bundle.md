---
name: embed-static-doc-as-resource-for-runtime-bundle
description: How OUT-430's validation-methodology doc is both a static repo doc and a per-run bundled artifact without duplicating content.
metadata:
  type: project
---

When a requirement needs a document to exist both as a standalone,
human-readable static submission doc *and* copied verbatim into every
run's output bundle at runtime (OUT-430's "accompanies every run's
output... or ships as a static submission document" — issue #9
treated this as "and", not "or"), the pattern used: author the content
once at `docs/VALIDATION_METHODOLOGY.md`, then `<EmbeddedResource
Include="..\..\docs\VALIDATION_METHODOLOGY.md" LogicalName="..." />`
in the producing project's `.csproj` so it's baked into the assembly
at build time. At runtime, `Assembly.GetExecutingAssembly()
.GetManifestResourceStream(logicalName)` copies it into the output
directory — no dependency on the repo's `docs/` folder existing next
to a published/containerized binary, and no risk of the two copies
drifting apart since there's only one copy.

**Gotcha:** MSBuild project-file XML comments cannot contain `--`
(rejects the file with `MSB4025`, an unhelpful "could not be loaded"
error with no further detail) — write "System.Text.Json etc., not
'...Json -- and...'" style comments in `.csproj` files, not em-dash
`--` sequences, unlike `.cs` file comments where `—` (em dash
character, not two hyphens) is used throughout this repo without
issue.

**Related:** [[naadap-shared-dtos-live-in-core]] — the same issue (#9)
extended that pattern to `CandidateVehicle`/`Metric`/`RunManifest`.
