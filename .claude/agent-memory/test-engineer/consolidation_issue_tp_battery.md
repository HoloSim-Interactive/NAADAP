---
name: consolidation-issue-tp-battery
description: How to run the multi-TP battery on a DELIV-9xx-style consolidation issue (deliverable docs + Windows/VS check) — which TPs are executable here and which are legitimately NOT-RUN.
metadata:
  type: project
---

Issue #12 ("[RTVM-900] Deliverable documentation and Windows/Visual
Studio consolidation check") bundled seven TPs into one hand-off
instead of the usual one-TP-per-feature-issue shape. Pattern for any
future consolidation issue that looks like this:

- **TP-900** (buildable from clean clone) — `dotnet build`/`dotnet
  test`/`dotnet format --verify-no-changes`, plus confirm no
  `.cs`/`.csproj` is gitignored (`git ls-files | grep '\.cs$'` should
  be nonzero and match what you see on disk).
- **TP-920** (dependency justification) — diff the doc's table
  against the actual `<!-- Justification: ... -->` comments in each
  `.csproj`, not just against the doc's own prose — the doc could
  drift from the source of truth without anyone noticing.
- **TP-930** (fresh-clone build/run doc) — actually execute the
  README's worked example verbatim, don't just read it. Run from repo
  root using the default `input`/`output` dirs exactly as documented
  (not a `/tmp` scratch dir the README never mentions — that's not
  testing the doc as written). Diff the real output bundle's file list
  and manifest content against the doc's claims line by line.
- **TP-940** (maintainer extension walkthrough) — grep-confirm every
  interface/file/test the doc names actually exists with that name;
  don't just skim for plausibility.
- **TP-960/TP-970** (SETR mapping table / three discrete docs) — quick
  inspection, low risk of drift since these are mostly structural.
- **TP-910** (Windows/VS demonstration) — genuinely cannot be executed
  in this sandbox (Linux only, and even on Linux the workflow file
  needed to run it on a real Windows GH runner requires a human to
  copy `docs/ci/windows-verification.yml` into
  `.github/workflows/` — no agent identity has `workflows` write
  permission). Confirm what's checkable without a Windows runner (all
  `.csproj` target plain `net9.0`, no WPF/WinForms) and report the
  demonstration half as NOT-RUN, not PASS — per the evidence-vs-verdict
  discipline, don't let a partial check read as a full pass in the
  comment.

**Why:** this shape (one issue closing out several DELIV items at
once) is likely to recur near a submission deadline; worth having the
per-TP checklist ready rather than re-deriving it.

**How to apply:** when a consolidation-style issue lands, read
`docs/RTVM.md`'s TP list for every DELIV item named in the issue title
and treat each as its own mini test procedure, but write one combined
PASS comment covering all of them (per this role's comment-brevity
rule) rather than one comment per TP.
