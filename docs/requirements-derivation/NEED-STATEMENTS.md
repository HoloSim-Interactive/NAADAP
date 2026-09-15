# Need statements feeding the atomic-requirements derivation

The durable record of the client's distilled need statements, each of
which is decomposed into atomic RTVM requirements by the derivation
workflow (`naadap-atomic-requirements`, run `wf_fe14ec90-697`) and then
vetted through two lenses. Needs 1–5 were derived on 2026-09-15 (50
requirements, 7 of 100 vet verdicts run — see
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/requirements-derivation/DERIVED-REQUIREMENTS.md" target="_blank">DERIVED-REQUIREMENTS.md</a>).
Needs 6–15 were added the same day from a paragraph-by-paragraph reading
of the challenge's Problem Statement and Benefits text and are queued,
not yet derived.

These are need statements, not requirements. They trace to the
[CONFIRMED] stakeholder needs SN-1..SN-6 in
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/PROJECT_DEFINITION.md" target="_blank">PROJECT_DEFINITION.md</a>
and are the client's framing of what those needs demand of the design.

## From the development conversation (needs 1–5)

1. **Compartmentalized SME agent.** A self-contained agent that leverages
   knowledge captured from subject-matter experts and provides more than
   retrieval: insight connecting the need in an acquisition request to the
   best available resource on the shortest path to fulfillment.
2. **Judgment in the grey areas.** When the probability distribution is
   flat, the recommender must still choose deterministically, based on
   understanding of the need and the available vehicles — and must surface
   ambiguity rather than resolve it silently.
3. **Distillation.** Data science, NLP, and ML must distill pedantic
   documentation down to the core need that must be filled.
4. **Multiple separate current contracts.** Finding that pattern — FAR
   2.101's definition of consolidation — is the whole point, not a
   similarity artifact.
5. **Small-business impact and the reserved-decision boundary.** No
   recommendation without the small-business impact note; the
   recommendation feeds a determination, never substitutes for one.

## From Problem Statement paragraph 1 (needs 6–8)

6. **Knowledge-capture workflow and build pipeline.** The methodology
   includes repeatable human workflows to gather and input SME knowledge
   and a reproducible build-time pipeline that fits and freezes model
   parameters; both documented, automated where possible, bit-identical
   from identical inputs.
7. **Cross-format alignment and open-source extraction.** Correlated
   content in different sections of different document types resolves to
   the same requirement content, with gaps handled explicitly; open-source
   text yields its own field set (platforms, programs, budget lines,
   capability-gap language).
8. **Future-capability new-vehicle recommendation.** Beyond the no-fit
   case, recommend new strategic vehicles proactively from forward-looking
   open-source signals, before the requirement documents exist.

## From Problem Statement paragraph 2 (needs 9–11)

9. **Vehicle breadth as a classification criterion.** Breadth of ordering
   access is recorded and tested; a vehicle failing it is a consolidation
   source, never a target, however large.
10. **Scope coherence as a consolidation constraint.** One
    performance-based scope statement with one integrated QASP must be
    able to govern all cluster members; textual similarity is not enough.
    ("Performance" is the FAR 37.6 term of art, not "performant.")
11. **The five benefits as quantified determination fields.** Buying
    power, reduced duplication, process efficiency, speed, and cost
    savings — distinct, separately quantified, with "not quantifiable from
    available data" stated where true. This absorbs the earlier
    four-benefit reading and adds ceiling utilization as both benefit and
    constraint.

## From Problem Statement paragraph 3 (needs 12–15)

12. **Graceful degradation on sparse input.** A usable, evidence-bearing
    recommendation when most structured fields are null; degradation
    explicit, never silent.
13. **Sub-SAT consolidation trap.** Clusters with below-threshold members
    carry an explicit flag: consolidation lifts them above SAT, removes
    the automatic small-business reservation, and can trigger FAR 7.107.
14. **Three-legged tacit knowledge.** Existing systems (where to look),
    requirements (what has been bought), and vehicles — captured as one
    need so no leg is dropped.
15. **Vehicle promotion as a third output mode.** Existing contracts not
    in the catalog but behaving like enterprise vehicles, identified as
    candidates for formalization and on-ramping. (Reading is ambiguous;
    see the Tech Grove question list.)

## Refinements folded in or pending

- **Benefit quantification (refinement a)** — folded into need 11.
- **DELIV-970 reform-instrument citations (refinement b)** — the algorithm
  documentation traces each claimed benefit to a named DoD
  acquisition-reform instrument by memo or instruction number. An RTVM
  amendment; listed with the others in
  <a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank">the design doc's G3 gate</a>.

## Standing wording correction

Wherever project text says the system "picks," "selects," or "decides"
a vehicle, it should say the system **identifies suitable** vehicles
with evidence and the contracting officer decides. The sponsor's own
sentence — "automatically identify contracts suitable to be used" — is
careful on this point and the project should match it.

## ID bands

The suggested bands in the workflow script are hints for
collision-avoidance, not assignments. The derivation's structure pass
resolves final IDs against the occupied numbers in
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/RTVM.md" target="_blank">RTVM.md</a>,
and it has already recommended a new `KB` block (600–699). Several of
the bands above are crowded; expect the structure pass to propose
opening further blocks rather than squeezing.
