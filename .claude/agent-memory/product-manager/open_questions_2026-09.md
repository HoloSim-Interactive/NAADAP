---
name: open-questions-2026-09
description: Open questions posed to the client on issue #1 (2026-09-03) that PROJECT_DEFINITION.md still marks [PROPOSED] pending their answer.
metadata:
  type: project
---

Asked on issue #1, 2026-09-03, still awaiting client reply as of that
date:

1. **Deadline conflict in the challenge text itself.** The issue body
   states Submissions Deadline = Sept 22, 2026 and Final Demo Day =
   Nov 9, 2026 in its summary box, but its own "TIMELINE" section
   later says submission deadline = Oct 2, 2026 and Final Demo Day =
   Nov 19, 2026. Today is 2026-09-03 — if Sept 22 is correct there are
   only ~19 days to build an IL4-ready Docker-packaged ML recommender
   system, which drastically changes MVP scope vs. the Oct 2 date.
   **Blocking for realistic scope-setting; needs the client's
   authoritative date.**
2. **Challenge phase status.** Has HoloSim already submitted/passed
   the Phase 1 Pre-Screening Questionnaire? Do we have Phase 2 GFI
   (real procurement document corpus) access yet, or is development
   proceeding against synthetic/representative documents until GFI
   arrives?
3. **LLM usage strategy.** Proposed default in PROJECT_DEFINITION.md:
   no LLM in the core clustering/recommendation algorithm (maximizes
   the challenge's LLM-cost score category and matches its own stated
   goal of moving beyond RAG/retrieval into cluster analysis); LLM
   used only optionally for the summarization/visualization step the
   rules call out as allowed to be stochastic. Needs client
   confirmation before Systems Engineer designs around it.
4. **End-user/build documentation scope.** Proposed a
   "fresh-clone-to-running-result" doc standard (see
   `docs/PROJECT_DEFINITION.md` Deliverable Requirements). Client
   hasn't explicitly confirmed they want this beyond what the
   challenge already mandates (algorithm docs, deployment
   instructions, dependency docs). Confirm scope, especially whether
   they want a "how to extend this in Visual Studio" section for
   their own engineers per [[navair-prize-challenge]].

Once client answers arrive, update `docs/PROJECT_DEFINITION.md`
[PROPOSED] tags to [CONFIRMED] (or revise) before handing off to
Systems Engineer for the RTVM.
