---
name: navair-prize-challenge
description: NAADAP is a $150K NAVAIR/NAWCAD prize challenge with a hard external deadline and IL4/offline constraints — not open-ended work.
metadata:
  type: project
---

`HoloSim-Interactive/NAADAP` is a competition entry, not a
normal build. Central Florida Tech Grove / NAVAIR/NAWCAD "Advanced
Acquisition Documentation Analysis Prize Challenge": build a recommender
that clusters procurement documents (SOWs, PWSs, CDRLs, Congressional
testimony) and recommends strategic contract vehicles for consolidation.
Prizes $100K / $50K. The client is competing to win.

Constraints that shape architecture, from the challenge text (issue #1):

- **Deliverable is Dockerized and offline.** Must run in a US-Government
  IL4-accredited cloud; no external services, "no external industry-hosted
  custom models." Any LLM/embedding step must ship inside the container.
- **Determinism gate:** the identification methodology must produce the
  same top-5 results 95% of the time. Only the final summarization may be
  stochastic.
- **C# is a client mandate**, with source code as a deliverable, and a
  Visual Studio solution wanted at the end (see
  [[csharp-on-ubuntu-is-not-a-port]]).
- Must ship a visualization of method and results, plus a summary metric
  for algorithm performance.

**Why:** an offline-only, deterministic, C# ML pipeline is a much narrower
design space than "build a recommender." Decisions that assume a hosted
model API or a Python ML stack are dead on arrival at judging.

**How to apply:** check any proposed dependency against "does this run
inside a sealed container with no egress." Prefer ONNX Runtime / ML.NET
with model weights vendored into the image. Treat the determinism gate as
a testable requirement with fixed seeds, not an aspiration.
