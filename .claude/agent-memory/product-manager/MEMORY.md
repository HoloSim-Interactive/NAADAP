# Product Manager — memory

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

## Understanding of the product

- [NAVAIR Prize Challenge context](navair_prize_challenge.md) — this is a competition submission (recommender system for acquisition docs), plus client's fixed C#/VS/MBSE/SETR constraints.

## Client / stakeholder context

- Client (HoloSim) posted challenge-defined technical requirements directly in issue #1; treat challenge rules (issue #1 body) as a second, non-negotiable set of constraints alongside HoloSim's own asks.

## Open questions log

- [2026-09-03 questions, now resolved](open_questions_2026-09.md) — historical record of client's answers on deadline, phase/GFI status, LLM strategy, doc scope. All now [CONFIRMED] in PROJECT_DEFINITION.md.

## Decisions made

- 2026-09-03: Drafted docs/PROJECT_DEFINITION.md from issue #1 challenge text + client comment; several MVP/strategy items left [PROPOSED] pending client answers.
- 2026-09-03: Client answered all 4 open questions same day. Flipped every [PROPOSED] item in PROJECT_DEFINITION.md to [CONFIRMED]: worst-case deadlines (submit 9/22, demo 11/9), no Phase 1/GFI yet (build against real public-source SOW/PWS/CDRL docs, not synthetic), no-LLM-core clustering + optional LLM only for summarization/viz + demonstrate-and-document alternative approaches, and full fresh-clone build/run docs at SETR rigor. Scope now fully defined; closed issue #1 and opened "RTVM" issue for Systems Engineer.
- 2026-09-03: Issue #13 (anchor-convention retrofit) — added `pd-sn-1`..`pd-sn-6` anchors to PROJECT_DEFINITION.md's Stakeholder Need table; confirmed issue #1 predates the RTVM/SN scheme and needs no cross-ref edits. Left the many bare `SN-#` mentions inside RTVM.md's Stakeholder Need column and SDD.md prose as a flagged, non-blocking follow-up (anchors now exist to support it, but nobody's claimed the linking pass) — see AGENT_LABELS.md convention if this resurfaces.
