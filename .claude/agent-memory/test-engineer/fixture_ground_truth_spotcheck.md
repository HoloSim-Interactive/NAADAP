---
name: fixture-ground-truth-spotcheck
description: How to spot-check checked-in document fixtures + hand-derived ground truth (e.g. tests/fixtures/), and a real defect class found doing so on issue #6
metadata:
  type: project
---

When a Software/Systems Engineer hands off a fixture corpus with a
hand-derived ground-truth mapping (candidate vehicles, clusters, doc
types) and asks for a spot-check, don't just confirm the files parse —
also re-derive a couple of the strongest claimed rationale points
against the actual extracted document text, not just the README's
summary of it.

**Why:** On issue #6 (`tests/fixtures/`), the README/`ground-truth.json`
for `reference-20/v2-01-sbrac-vi-solicitation.pdf` claimed the
candidate vehicle name ("SBRAC") appeared *directly, not inferred* in
the solicitation's own title. Full-text extraction (pypdf) of all 177
pages found zero occurrences of "SBRAC" anywhere in the document — the
actual title text is a spelled-out description ("Small Business IDIQ
... Environmental Remedial Action ... NAVFAC Pacific") that supports
the classification by content but never names the vehicle acronym.
The claim was likely carried over from the SAM.gov opportunity
*listing* title (outside the PDF, not directly checkable — SAM.gov is
a client-rendered SPA, so `curl` on the opportunity URL doesn't surface
it). This is a plausible mistake, not fabrication, but it's exactly
the kind of overstated-confidence claim worth catching before it feeds
a later validation-methodology write-up (OUT-430 in this project) —
the "least-inferred" example turned out to be inferred like the rest.

**How to apply:** For any fixture corpus with per-document rationale
text, pick the 1-2 documents whose rationale claims the *strongest*
evidence (e.g. "named directly," "unambiguous," "ground truth by
construction") and actually grep/extract the full document text for
the literal claimed string, not just skim the README's paraphrase. A
claim of directness is the cheapest one to verify and the most
consequential if wrong, since downstream metrics (precision@5-style
scoring here) get graded against it.
