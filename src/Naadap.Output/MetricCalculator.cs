using Naadap.Core;

namespace Naadap.Output;

/// <summary>
/// OUT-420: computes the single headline summary metric (precision@5) plus
/// the raw counts it is built from. Full derivation, including why a run
/// with no ground truth reports the metric as "not computed" rather than
/// 0.0, is written up in docs/VALIDATION_METHODOLOGY.md — that document and
/// this implementation must stay in sync.
/// </summary>
public static class MetricCalculator
{
    public const string MetricName = "precision@5";

    private const string noGroundTruthDefinition =
        "No ground-truth mapping (" + GroundTruth.FileName + ") was found in the input " +
        "directory, so precision@5 was not computed for this run. This is expected for a " +
        "real, unlabeled production input — ground truth applies only to validation runs " +
        "against tests/fixtures/reference-20 (see docs/VALIDATION_METHODOLOGY.md).";

    /// <summary>
    /// Computes precision@5 against <paramref name="groundTruth"/> if
    /// present, else returns a Metric with a <see langword="null"/> value
    /// explaining why. See docs/VALIDATION_METHODOLOGY.md "Metric
    /// definition" for the full, worked-through algorithm this implements.
    /// </summary>
    public static Metric ComputePrecisionAtFive(
        IReadOnlyList<CandidateVehicle> rankedCandidates,
        GroundTruth? groundTruth)
    {
        if (groundTruth is null)
        {
            return new Metric(MetricName, Value: null, CorrectCount: 0, TotalCount: 0, noGroundTruthDefinition);
        }

        var evaluatedCount = Math.Min(5, rankedCandidates.Count);
        var topCandidates = rankedCandidates.Take(evaluatedCount).ToList();

        var claimedRealVehicles = new HashSet<string>(StringComparer.Ordinal);
        var correctCount = 0;

        foreach (var candidate in topCandidates)
        {
            var majorityRealVehicle = MajorityGroundTruthVehicle(candidate, groundTruth);
            if (majorityRealVehicle is not null && claimedRealVehicles.Add(majorityRealVehicle))
            {
                correctCount++;
            }
        }

        var value = evaluatedCount == 0 ? 0.0 : (double)correctCount / evaluatedCount;
        var definition =
            $"{MetricName}: of the top {evaluatedCount} ranked candidate(s), {correctCount} matched a " +
            "distinct ground-truth vehicle by majority vote of their contributing documents " +
            "(each real vehicle counted at most once, for the highest-ranked matching candidate). " +
            "See docs/VALIDATION_METHODOLOGY.md for the full definition.";

        return new Metric(MetricName, value, correctCount, evaluatedCount, definition);
    }

    private static string? MajorityGroundTruthVehicle(CandidateVehicle candidate, GroundTruth groundTruth)
    {
        var votes = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var filename in candidate.ContributingDocuments)
        {
            if (groundTruth.VehicleIdByFilename.TryGetValue(filename, out var realVehicleId))
            {
                votes[realVehicleId] = votes.GetValueOrDefault(realVehicleId) + 1;
            }
        }

        if (votes.Count == 0)
        {
            return null;
        }

        // Deterministic tie-break: highest vote count, then ordinal on the
        // vehicle ID itself, never dictionary iteration order.
        return votes
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => pair.Key, StringComparer.Ordinal)
            .First()
            .Key;
    }
}
