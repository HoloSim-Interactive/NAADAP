---
name: editorconfig-naming-no-underscore-private-fields
description: This repo's .editorconfig forbids the common `_camelCase` private-field convention; `dotnet format --verify-no-changes` fails on it.
metadata:
  type: project
---

`.editorconfig`'s `dotnet_naming_rule.locals_camel_case` applies to both
`local` and `field` (private) symbol kinds, with **no** `required_prefix`
configured — so private instance fields, private static fields, and
`static readonly` fields must all be plain `camelCase` (`parsers`,
`longForm`), not the `_camelCase` convention common elsewhere in .NET
(`_parsers`). `dotnet format Naadap.sln --verify-no-changes` (the CI
check per docs/SDD.md's Coding Standards) fails with IDE1006 on any
underscore-prefixed field.

**Why:** discovered running `dotnet format --verify-no-changes` during
issue #7 (Ingestion) — a constructor parameter and a private field
needed the same name, worked around by naming the constructor param
`parserList` and keeping the field `parsers`, rather than reaching for
`_parsers`.

**How to apply:** before committing any new `src/`/`tests/` code with
private or static-readonly fields, run `dotnet format Naadap.sln
--verify-no-changes` and fix any IDE1006 hits — don't assume the
`_field` convention is fine just because it's idiomatic elsewhere.
