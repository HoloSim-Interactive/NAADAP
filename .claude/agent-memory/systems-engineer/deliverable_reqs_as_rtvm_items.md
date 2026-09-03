---
name: deliverable-reqs-as-rtvm-items
description: NAADAP-specific override -- Product Manager wants Deliverable Requirements as real DELIV-9xx RTVM line items, not just narrative in SDD build/toolchain conventions.
metadata:
  type: feedback
---

systems-engineer.md's default is: PROJECT_DEFINITION.md's "Deliverable
requirements" section doesn't get RTVM line items, it becomes SDD
build/toolchain conventions instead. On NAADAP, Product Manager
explicitly overrode this in the RTVM kickoff handoff (issue #2,
2026-09-03): "please give these real RTVM line items, not just
narrative coverage."

**Why:** for this project the deliverable requirements (full C#
source, minimal NuGet deps, VS-openable solution on plain net9.0,
fresh-clone-to-running build/run docs, SETR/CDRL-rigor documentation)
are graded/verified almost like functional requirements — the client
scores against them directly — so PM wants them traceable and
verifiable the same way, not folded into prose.

**How to apply:** DELIV-9xx items exist in `docs/RTVM.md` for this
project with real Verification Methods and Test Procedures (mostly
Inspection/Demonstration, per the ID-scheme note that DELIV items are
typically inspection-verified). Keep doing this for any future
DELIV-9xx items added to NAADAP's RTVM — don't fall back to the
generic default of narrative-only SDD coverage for this project. This
is a per-project PM directive, not a new global default — check for
an equivalent explicit instruction before applying it to a different
project.
