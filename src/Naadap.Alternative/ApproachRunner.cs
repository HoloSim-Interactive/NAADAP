using System.Diagnostics;
using Naadap.Core;
using Naadap.Output;

namespace Naadap.Alternative;

/// <summary>
/// Runs one <see cref="IClusteringComponent"/> end to end (cluster -&gt;
/// rank -&gt; score) against a fixed document set and ground truth, timing
/// each stage — the shared harness both approaches in a CORE-260 comparison
/// go through, so the comparison is apples-to-apples by construction rather
/// than by two independently-written measurement loops.
/// </summary>
public static class ApproachRunner
{
    public static ApproachRunResult Run(
        string approachName,
        IClusteringComponent component,
        IReadOnlyList<DocumentRecord> documents,
        GroundTruth? groundTruth)
    {
        var stopwatch = Stopwatch.StartNew();
        var clusters = component.Cluster(documents);
        var clusteringElapsed = stopwatch.Elapsed;

        var candidates = VehicleRecommender.Recommend(documents, clusters);
        var metric = MetricCalculator.ComputePrecisionAtFive(candidates, groundTruth);
        var totalElapsed = stopwatch.Elapsed;

        return new ApproachRunResult(
            approachName,
            clusters.Count,
            candidates.Count,
            clusteringElapsed,
            totalElapsed,
            metric,
            LlmTokensUsed: 0); // neither approach compared here calls an LLM -- see docs/ALGORITHM_COMPARISON.md
    }
}
