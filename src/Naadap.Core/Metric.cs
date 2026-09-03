namespace Naadap.Core;

/// <summary>
/// OUT-420's single summary performance metric plus the raw counts it was
/// computed from. Lives in <c>Naadap.Core</c> alongside the other
/// <see cref="RunManifest"/> field types — see this role's memory note
/// "NAADAP shared DTOs live in Core".
/// </summary>
/// <param name="Name">
/// The metric's name, e.g. <c>"precision@5"</c> — see
/// <c>Naadap.Output.MetricCalculator</c> for the exact definition.
/// </param>
/// <param name="Value">
/// The metric's value, or <see langword="null"/> when no ground-truth
/// mapping was available for this run's input set (a real, un-labeled
/// production input has no ground truth — this is expected and not an
/// error; see <c>Naadap.Output.MetricCalculator</c> remarks and
/// docs/VALIDATION_METHODOLOGY.md). A <see langword="null"/> value still
/// carries <paramref name="Definition"/> explaining why.
/// </param>
/// <param name="CorrectCount">
/// Raw count of evaluated candidates judged correct against ground truth.
/// Zero when <paramref name="Value"/> is <see langword="null"/>.
/// </param>
/// <param name="TotalCount">
/// Raw count of candidates evaluated (the metric's denominator). Zero when
/// <paramref name="Value"/> is <see langword="null"/>.
/// </param>
/// <param name="Definition">
/// Human-readable description of exactly how <paramref name="Value"/> was
/// computed (or why it was not), so OUT-420's output is self-explanatory
/// without cross-referencing code.
/// </param>
public sealed record Metric(
    string Name,
    double? Value,
    int CorrectCount,
    int TotalCount,
    string Definition);
