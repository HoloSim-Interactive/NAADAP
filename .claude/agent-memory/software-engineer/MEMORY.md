# Software Engineer — memory

**This file is an index, not a store.** It is loaded on every run you
ever do, so anything verbose here is re-read on every future hand-off
for the rest of the project. Keep each entry to one line: a link and a
one-sentence summary. Put the actual detail in its own file in this
folder.

    - [Short title](descriptive_slug.md) — one sentence on what it is.

A genuinely one-line fact can stay a plain line with no file of its
own. Split a lesson out when it needs a reproduction, a command
sequence, or real reasoning to be useful later. See "Memory structure"
in `.github/AGENT_LABELS.md`.

## Architecture patterns

- [NAADAP shared DTOs live in Core](naadap_shared_dtos_live_in_core.md) — cross-stage records (DocumentRecord etc.) go in Naadap.Core, never in the producing stage, to keep CORE-240's zero-dependency check trivial.

- [Embed a static doc as a resource for a runtime bundle](embed_static_doc_as_resource_for_runtime_bundle.md) — pattern for a doc needed both standalone and copied into every run's output (OUT-430); also notes a `.csproj` XML-comment `--` gotcha.

## Platform-specific notes

- [dotnet new sln defaults to .slnx](dotnet_new_sln_defaults_to_slnx.md) — force `-f sln` when a classic `.sln` filename is required.

## Reusable solutions

- [SAM.gov public API for document sourcing](samgov_public_api_for_document_sourcing.md) — how to search/download real SOW/PWS/CDRL attachments from SAM.gov for test fixtures; NAVAIR-specific gotcha re: PIEE redirects.

## Coding standards

- [.editorconfig forbids `_camelCase` private fields](editorconfig_naming_no_underscore_private_fields.md) — run `dotnet format --verify-no-changes` before committing; IDE1006 fails on underscore-prefixed fields.

## Project notes

- [Repo .gitignore missing .NET patterns](repo_gitignore_missing_dotnet_patterns.md) — template skeleton's `.gitignore` is C/C++-flavored; add `bin/`/`obj/`/etc. yourself on first .NET scaffold.
- [Derive thresholds empirically against fixtures](derive_thresholds_empirically_against_fixtures.md) — how CORE-200's 0.35 similarity threshold was measured (Python scratch script vs. checked-in fixture), reuse this technique for future tuned constants.
