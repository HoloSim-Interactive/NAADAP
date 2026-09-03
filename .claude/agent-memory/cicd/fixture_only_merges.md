---
name: fixture-only-merges
description: Data/fixture-only issues (e.g. #6, test corpus) merge without a dotnet build check and don't move any RTVM item.
metadata:
  type: project
---

Issue #6 ("Validation Corpus & Test Fixtures") added only binary/text
fixtures under `tests/fixtures/` — no `.cs`/`.csproj` changes. No
`dotnet build` sanity check applies to a merge like this; it's not
skipped, it's just not relevant (there's no code to build). Verify
instead by re-reading whether the Test Engineer's PASS actually
covered the deliverable (extraction round-trip, corrupted-file
behavior, ground-truth JSON validity) before merging.

**Why:** avoids wasting time hunting for a build step that doesn't
exist on non-code issues, and confirms the right kind of verification
was actually done before commit.

**How to apply:** before merging, check `git diff --stat` against
main — if it's entirely non-code (docs, fixtures, data), skip the
build-check step from [[MEMORY]] build notes and rely on the Test
Engineer's stated verification instead.

Also confirmed again on this issue: a merge where every RTVM item is
still Approved (none reaches Verified) gets a `git tag` only, no
`gh release create` — same pattern as issue #5. Issue #6 merged as
commit `5e4d176`, tagged `v1.0.24`.
