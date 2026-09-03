using Naadap.Core;

namespace Naadap.Alternative;

/// <summary>
/// One approach's outcome from a single CORE-260 comparison run —
/// everything <see cref="ComparisonReportWriter"/> needs to render TP-260's
/// accuracy/runtime/cost table row.
/// </summary>
/// <param name="ApproachName">Human-readable label for the report (e.g. "Non-LLM core").</param>
/// <param name="ClusterCount">Number of clusters produced.</param>
/// <param name="CandidateCount">Number of ranked candidates produced (DATA-OUT-300).</param>
/// <param name="ClusteringElapsed">Wall-clock time spent clustering only.</param>
/// <param name="TotalElapsed">Wall-clock time spent clustering + ranking + scoring.</param>
/// <param name="Metric">OUT-420's precision@5, computed identically for both approaches via <c>Naadap.Output.MetricCalculator</c>.</param>
/// <param name="LlmTokensUsed">
/// Total LLM tokens this approach spent producing its result. Zero for any
/// approach — including CORE-260's alternative here — that does not call an
/// LLM/microservice at all; that is a real, reportable number, not a
/// placeholder.
/// </param>
public sealed record ApproachRunResult(
    string ApproachName,
    int ClusterCount,
    int CandidateCount,
    TimeSpan ClusteringElapsed,
    TimeSpan TotalElapsed,
    Metric Metric,
    int LlmTokensUsed);
