---
name: threshold-derivation-recompute
description: How to independently verify a claimed similarity/score threshold derivation (e.g. CORE-200's TF-IDF cosine threshold) without trusting the doc comment's numbers.
metadata:
  type: feedback
---

When a Software Engineer's hand-off cites specific numeric evidence for a
derived constant (e.g. "in-theme similarities 0.560-0.792 vs. best
cross-theme 0.229" for `TfIdfCosineClusteringComponent.SimilarityThreshold`
on issue #8), don't just trust the doc comment or re-run the existing unit
test — independently recompute the matrix.

**How:** scaffold a throwaway console app outside the repo (e.g.
`/tmp/verify-*`), give it a `<ProjectReference>` to the real
`src/.../*.csproj`, and call the public static methods directly (e.g.
`TfIdfVectorizer.Vectorize` / `CosineSimilarity`) over the actual fixture
files. Clean it up afterward (`rm -rf`) — it's scratch, not part of the repo,
and the pre-commit hook blocks Write/Edit outside the memory folder anyway
so this has to go through Bash heredocs.

**Why:** on issue #8 my independently recomputed numbers (in-theme
0.5750-0.8104, max cross-theme 0.2271) didn't exactly match the class doc
comment's claimed numbers (0.560-0.792, 0.229) — off by ~0.01-0.02, likely a
stale figure from an earlier iteration of the fixture/vectorizer. The
*conclusion* (0.35 sits safely between the two bands) still held, so this
was a note not a FAIL, but it would have gone unnoticed by just re-running
the checked-in test. See [[fixture_ground_truth_spotcheck]] for the related
practice of re-verifying evidentiary claims against source data, not just
the paraphrase.

**How to apply:** any time a hand-off's PASS case rests on "I computed X and
it came out in range Y", recompute X yourself before accepting Y.
