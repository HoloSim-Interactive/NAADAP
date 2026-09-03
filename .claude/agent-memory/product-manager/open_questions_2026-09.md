---
name: open-questions-2026-09
description: Resolved — client's 2026-09-03 answers to the four open questions posted on issue #1, now reflected as [CONFIRMED] in docs/PROJECT_DEFINITION.md.
metadata:
  type: project
---

Asked on issue #1 on 2026-09-03; client answered same day in a
follow-up comment. All four are now [CONFIRMED] in
`docs/PROJECT_DEFINITION.md` — this file is the historical record of
the Q&A, not an open item anymore.

1. **Deadline conflict in the challenge text.** Resolved: plan against
   the worst case — Submissions Deadline 2026-09-22, Final Demo Day
   2026-11-09 — until the client gets an authoritative answer from
   Tech Grove. Do not plan against the later (Oct 2 / Nov 19) dates
   that also appear in the challenge text.
2. **Challenge phase status.** Resolved: as of 2026-09-03, HoloSim has
   NOT submitted Phase 1 Pre-Screening yet, and has no Phase 2 GFI
   access. Client wants to start building now anyway. Development
   proceeds against genuine, realistic SOW/PWS/CDRL-style documents
   pulled from public references (client gave an NSWC Dahlgren
   "Leading Edge" link as an example) — not synthetic/fabricated docs.
   Systems Engineer is tasked with researching a concrete public
   corpus to build/test against.
3. **LLM usage strategy.** Resolved, matches the proposed default: no
   LLM in the core clustering/recommendation algorithm — lean heavily
   into the challenge's "beyond RAG/retrieval, into cluster analysis"
   framing. LLM only optionally for the
   summarization/interpretation/visualization step the rules allow to
   be stochastic, and only once shown to be sound/lower-risk logic.
   Additional nuance beyond the original proposal: client also wants
   at least one alternative approach (e.g. LLM-assisted/retrieval)
   demonstrated and its shortcomings documented via rigorous testing,
   to substantiate the "why not LLM-first" rationale in the algorithm
   documentation — not built as a second production path.
4. **Documentation scope.** Resolved: yes, want a fresh-clone-to-
   running-result build/run doc, at SETR/CDRL rigor, including where a
   maintainer adds a new document type or clustering component (client
   confirmed via "develop this in line with our workflow ... just with
   more rigor"). Client separately tasked Systems Engineer with
   researching the SETR process itself and producing the specific
   article list, placed on the timeline, reconciled against this
   project's existing `docs/SDD.md` / `docs/RTVM.md` /
   `docs/IMPLEMENTATION_PLAN.md` pipeline artifacts. Not every SETR
   artifact is required — at minimum, the ones that already correspond
   to what this pipeline produces.

See [[navair-prize-challenge]] for the constraint list this refines.
