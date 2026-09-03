using Naadap.Core;

namespace Naadap.Alternative.Tests;

public class ApproachRunnerTests
{
    [Fact]
    public void Run_NoGroundTruth_ProducesClustersCandidatesAndUncomputedMetric()
    {
        var documents = new List<DocumentRecord>
        {
            new("a.txt", DocType.Sow, "flight-line engineering disposition support statement of work", null),
            new("b.txt", DocType.Sow, "flight-line engineering disposition support statement of work", null),
            new("c.txt", DocType.Sow, "shipboard fire-control system maintenance requirement", null),
        };

        var result = ApproachRunner.Run(
            "test approach",
            new RetrievalAugmentedClusteringComponent(),
            documents,
            groundTruth: null);

        Assert.Equal("test approach", result.ApproachName);
        Assert.True(result.ClusterCount >= 1);
        Assert.Equal(result.ClusterCount, result.CandidateCount);
        Assert.Null(result.Metric.Value);
        Assert.Equal(0, result.LlmTokensUsed);
        Assert.True(result.TotalElapsed >= result.ClusteringElapsed);
    }
}
