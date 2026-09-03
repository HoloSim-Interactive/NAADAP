using Naadap.Core;

namespace Naadap.Alternative.Tests;

public class ComparisonReportWriterTests
{
    [Fact]
    public void BuildMarkdown_TwoApproachResults_RendersTableWithBothRowsAndDefinitions()
    {
        var coreMetric = new Metric("precision@5", 0.6, CorrectCount: 3, TotalCount: 5, "core definition text");
        var altMetric = new Metric("precision@5", 0.4, CorrectCount: 2, TotalCount: 5, "alt definition text");

        var core = new ApproachRunResult(
            "Non-LLM core",
            ClusterCount: 14,
            CandidateCount: 14,
            ClusteringElapsed: TimeSpan.FromMilliseconds(148.3),
            TotalElapsed: TimeSpan.FromMilliseconds(320.2),
            coreMetric,
            LlmTokensUsed: 0);

        var alternative = new ApproachRunResult(
            "Alternative",
            ClusterCount: 10,
            CandidateCount: 10,
            ClusteringElapsed: TimeSpan.FromMilliseconds(131.2),
            TotalElapsed: TimeSpan.FromMilliseconds(298.8),
            altMetric,
            LlmTokensUsed: 0);

        var markdown = ComparisonReportWriter.BuildMarkdown("tests/fixtures/reference-20", 20, 1, core, alternative);

        Assert.Contains("Non-LLM core", markdown);
        Assert.Contains("Alternative", markdown);
        Assert.Contains("0.60", markdown);
        Assert.Contains("0.40", markdown);
        Assert.Contains("3/5", markdown);
        Assert.Contains("2/5", markdown);
        Assert.Contains("core definition text", markdown);
        Assert.Contains("alt definition text", markdown);
        Assert.Contains("20 document(s) ingested, 1 skipped", markdown);
    }

    [Fact]
    public void BuildMarkdown_NoGroundTruth_RendersNotApplicableForPrecision()
    {
        var metricWithoutGroundTruth = new Metric("precision@5", null, CorrectCount: 0, TotalCount: 0, "no ground truth");
        var result = new ApproachRunResult(
            "Approach",
            ClusterCount: 1,
            CandidateCount: 1,
            ClusteringElapsed: TimeSpan.Zero,
            TotalElapsed: TimeSpan.Zero,
            metricWithoutGroundTruth,
            LlmTokensUsed: 0);

        var markdown = ComparisonReportWriter.BuildMarkdown("dir", 1, 0, result, result);

        Assert.Contains("n/a", markdown);
    }
}
