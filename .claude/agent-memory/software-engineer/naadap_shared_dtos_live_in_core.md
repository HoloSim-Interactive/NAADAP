---
name: naadap-shared-dtos-live-in-core
description: In NAADAP, cross-stage DTOs (DocumentRecord, DocType, SkippedFile, ...) belong in Naadap.Core, not in the producing stage's own project — keeps CORE-240's zero-dependency inspection trivial.
metadata:
  type: project
---

`docs/SDD.md`'s block diagram requires `Naadap.Core` (the CORE-2xx
clustering engine) to reference nothing beyond the BCL — CORE-240 is
graded by a one-command dependency-graph check (`dotnet list
<Core.csproj> reference`/package diff), not a manual read. If a shared
contract like `DocumentRecord` lived in `Naadap.Ingestion` instead (the
stage that produces it), `Naadap.Core` would need a project reference
to `Naadap.Ingestion` to consume it downstream — and `Naadap.Ingestion`
carries real NuGet dependencies (PdfPig, DocumentFormat.OpenXml), which
would transitively break CORE-240's zero-dependency guarantee.

**Why:** decided while implementing DATA-IN-100 (issue #7): `DocType`,
`DocumentRecord`, and `SkippedFile` were added to `src/Naadap.Core/` as
plain dependency-free records/enums, and `Naadap.Ingestion` takes a
`ProjectReference` on `Naadap.Core` (one-directional) to produce them.

**How to apply:** any future cross-stage contract (e.g. `CandidateVehicle`,
`RunManifest` for DATA-OUT-300/OUT-440) should default to living in
`Naadap.Core` too, unless it specifically needs a dependency `Naadap.Core`
can't carry — in which case it doesn't belong in the Core-consumed data
flow at all. Verify with `dotnet list src/Naadap.Core/Naadap.Core.csproj
reference` / `package` before committing — both should stay empty.
