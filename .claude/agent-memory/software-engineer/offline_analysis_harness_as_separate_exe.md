---
name: offline-analysis-harness-as-separate-exe
description: How CORE-260's "documented, rejected comparison, not a second production path" requirement was built — a standalone Exe project, never referenced by the CLI, run manually to produce a checked-in report.
metadata:
  type: project
---

When a requirement asks for an alternative approach to be implemented
and compared, but explicitly *not* wired into production (CORE-260:
"a documented, rejected comparison... not a second production path
wired into the CLI/OUT-440 bundle"), the pattern used:

- Give the alternative its own `OutputType=Exe` project
  (`Naadap.Alternative`) with its own `Program.cs`, invoked manually
  (`dotnet run --project src/Naadap.Alternative -- <input-dir>
  [report-path]`) — never added as a `ProjectReference` from the CLI
  project. This makes "never wired into production" a structural,
  inspectable fact (`dotnet list Naadap.Cli.csproj reference` never
  shows it), not a convention someone has to remember not to violate.
- The harness still reuses the *scoring* machinery the production
  pipeline uses (`VehicleRecommender`, `MetricCalculator`,
  `GroundTruth` from `Naadap.Output`) so the comparison's accuracy
  numbers can't drift from the real metric definition — only the
  clustering algorithm itself differs between the two runs being
  compared.
- Run it once, capture real output, and commit the results as prose +
  a generated table in a docs file (`docs/ALGORITHM_COMPARISON.md`
  here) — this is what actually satisfies an RTVM item whose
  Verification Method is "Analysis" rather than "Test": the artifact
  *is* the run's output, checked in, not a repeatable assertion in a
  CI test suite (though a normal xUnit test project alongside it is
  still worth having, to pin down the alternative algorithm's own
  behavior on a smaller fixture).
- Don't fabricate an LLM call to make the alternative look like a
  "real" LLM-assisted approach when no accredited endpoint is reachable
  in the dev environment — the requirement's own phrasing offered
  "LLM-assisted *or* retrieval/RAG-based" as options; implementing the
  retrieval half honestly (and reporting its real LLM cost as zero) is
  more defensible than simulating a model response.

**Related:** [[naadap_shared_dtos_live_in_core]],
[[fail_closed_allowlist_gate_for_optional_network_step]] (the sibling
CORE-250 feature from the same issue).
