---
name: derive-thresholds-empirically-against-fixtures
description: How CORE-200's clustering similarity threshold (0.35) was chosen — compute the real metric against the checked-in fixture first, don't guess a round number.
metadata:
  type: project
---

When a requirement asks for a numeric threshold to be "documented"
(e.g. CORE-200/TP-200: "the assignment documented alongside the
similarity/score threshold used"), don't pick a plausible-sounding
round number and hope it works — compute the actual metric against the
checked-in fixture first, in a throwaway script, then pick a value
with real margin on both sides and record the derivation in the
production code's doc comment.

**How it was done for `TfIdfCosineClusteringComponent`** (issue #8):
wrote a ~40-line Python scratch script (`python3` was available in the
sandbox) replicating the planned TF-IDF + cosine formula exactly,
ran it against `tests/fixtures/synthetic-core200/`'s 10 documents, and
printed the full 10x10 similarity matrix. That showed the two intended
theme clusters at 0.560-0.792 internal similarity, and the highest
similarity between any two unrelated documents at 0.229 — a clean gap.
0.35 (roughly centered in that gap) was then hardcoded as a `public
const` in the C# implementation, with the derivation numbers written
directly into the XML doc comment so a reviewer doesn't have to
re-derive them.

**Why:** a threshold picked without measurement either fails the test
fixture outright or passes with zero margin (fragile to any small
wording/vocabulary change in real documents later). Measuring first
makes the constant defensible under CORE-240-style inspection and
gives Test Engineer/Systems Engineer something concrete to check
against, not just "it happened to pass."

**How to apply:** any time a future issue needs a new empirically-tuned
constant (e.g. [[naadap_shared_dtos_live_in_core]]'s downstream
DATA-OUT-300 ranking cutoffs, or CORE-260's alternative-approach
comparison scoring), reach for the same throwaway-script-against-the-
real-fixture technique before hardcoding a number, and delete the
scratch script once the derivation is written into the real source's
doc comments (don't leave it in the repo).
