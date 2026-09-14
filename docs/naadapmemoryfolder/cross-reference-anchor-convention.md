---
name: cross-reference-anchor-convention
description: "User's standing documentation practice — every indexed decision point (SN-#, CORE-#, TP-#, etc.) gets a prefixed lowercase anchor; every reference to one is an HTML <a target=\"_blank\"> link, never markdown shorthand."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 0e63a960-1002-453e-963a-6e2ce6b904e0
  modified: 2026-09-03T14:16:55.459Z
---

Client directive, given 2026-09-03 on the NAADAP project, phrased as a
standing practice ("should persist through the rest of all
documentation efforts") rather than a one-off request.

**The rule:**

1. Any document that defines indexed, individually-referenceable
   points — Stakeholder Needs (`SN-#`), requirement categories
   (`UI-#`, `CORE-#`, `DATA-IN-#`, `DELIV-#`, etc.), Test Procedures
   (`TP-#`), or any other specific location worth pointing at directly
   — gets an anchor at that point. Either markdown link syntax or an
   explicit `<a id="...">` works for defining the anchor itself.
2. Anchor ID format is strict: **all lowercase, must start with a
   letter (never a digit), and contains nothing but lowercase letters,
   digits, and hyphens** — no underscores, no spaces, no other
   punctuation. Prefix every ID with the owning document's short code
   so IDs stay unique project-wide: `rtvm-core-200`, `pd-sn-3`.
3. Every *reference* to one of these (or to another document generally)
   is an inline **HTML** hyperlink — `<a href="..." target="_blank">` —
   never markdown's `[text](url)` shorthand. The user's stated reason:
   markdown links don't reliably focus/scroll the target page to the
   anchor; the HTML form does.
4. Applies everywhere: `.md` files, comments in scripts, issue
   descriptions, issue comments — not just formal documentation.

**Why:** the user's own words — so other documents and future
readers can jump directly to the referenced location, not just learn
that a reference exists. This matters especially because HoloSim's
multi-agent pipeline generates heavy cross-referencing between roles
(RTVM items cited in issue comments, ICD sections cited by UI/Scene
Developer, etc.) and bare IDs like "CORE-200" are not actionable
without a working link.

**One judgment call I made, not explicitly stated by the user:** git
commit messages are exempt, since GitHub does not render HTML in commit
messages or `git log` output — an `<a>` tag there would be inert noise.
Flagged this explicitly when applying the rule; revisit if the user
pushes back.

**How to apply:** whenever writing or editing a document with indexed
items (an RTVM, a requirements doc, a stakeholder-needs list, a test
plan, an issue with numbered decision points) in *any* project, not
just NAADAP — this reads as a general documentation preference, not a
NAADAP-only rule. Applied to NAADAP so far in
`.github/AGENT_LABELS.md` (full spec, new "Cross-reference and anchor
convention" section) and all 8 `.claude/agents/*.md` files (pointers at
their existing comment-structure instructions), commit `ca7070f`.
**Not yet applied retroactively** to NAADAP's existing content
documents (`docs/PROJECT_DEFINITION.md`, `docs/RTVM.md`, `docs/SDD.md`,
`docs/IMPLEMENTATION_PLAN.md`) or `docs/VALIDATION_METHODOLOGY.md` — the
user explicitly wants to discuss that separately before touching
existing indexed content. See [[navair-prize-challenge]].
