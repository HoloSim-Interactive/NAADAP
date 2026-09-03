using Naadap.Core;

namespace Naadap.Alternative.Tests;

/// <summary>
/// TP-200-style fixture, TP-260 purpose: exercises CORE-260's alternative
/// (<see cref="RetrievalAugmentedClusteringComponent"/>) against the same
/// checked-in <c>tests/fixtures/synthetic-core200/</c> corpus
/// <c>Naadap.Core.Tests.TfIdfCosineClusteringComponentTests</c> uses for the
/// non-LLM core, so the two components' behavior on identical, known input
/// is directly comparable — including where they diverge.
/// </summary>
public class RetrievalAugmentedClusteringComponentTests
{
    private static string SyntheticCore200Directory =>
        Path.Combine(AppContext.BaseDirectory, "fixtures", "synthetic-core200");

    [Fact]
    public void Cluster_SyntheticCore200Set_GroupsTheTwoIntendedThemes()
    {
        var documents = LoadSyntheticCore200();

        var clusters = new RetrievalAugmentedClusteringComponent().Cluster(documents);

        var flightLineCluster = FindClusterContaining(clusters, "doc-a.txt");
        Assert.Equal(
            new[] { "doc-a.txt", "doc-c.txt", "doc-e.txt" },
            flightLineCluster.DocumentFilenames.OrderBy(f => f, StringComparer.Ordinal));

        var fireControlCluster = FindClusterContaining(clusters, "doc-b.txt");
        Assert.Equal(
            new[] { "doc-b.txt", "doc-d.txt" },
            fireControlCluster.DocumentFilenames.OrderBy(f => f, StringComparer.Ordinal));

        Assert.NotEqual(flightLineCluster.ClusterId, fireControlCluster.ClusterId);
    }

    [Fact]
    public void Cluster_SyntheticCore200Set_ForcedTopKRetrievalCanOvergroupUnrelatedDistractors()
    {
        // Documents this run's real, reproducible finding
        // (docs/ALGORITHM_COMPARISON.md): unlike Core's global threshold,
        // which lets a document join no cluster at all if nothing meets the
        // threshold, mutual top-K retrieval forces every document to
        // nominate K neighbors regardless of how weak the best available
        // match is -- so on this fixture, some of the five unrelated
        // distractors (doc-f/g/h/i/j) end up mutually retrieving each other
        // and merge into a cluster despite sharing no real theme. This is
        // the retrieval-based approach's documented shortcoming, not a bug
        // to "fix" here (fixing it is exactly the kind of extra tuning
        // docs/ALGORITHM_COMPARISON.md notes was out of scope for this
        // comparison).
        var documents = LoadSyntheticCore200();

        var clusters = new RetrievalAugmentedClusteringComponent().Cluster(documents);

        var distractorClusterIds = new[] { "doc-f.txt", "doc-g.txt", "doc-h.txt", "doc-i.txt", "doc-j.txt" }
            .Select(name => FindClusterContaining(clusters, name).ClusterId)
            .Distinct()
            .ToList();

        // At least two distractors were forced into the same cluster -- the
        // behavior this test exists to pin down and keep visible.
        Assert.True(
            distractorClusterIds.Count < 5,
            "Expected at least one pair of unrelated distractors to be over-grouped by forced top-K retrieval.");
    }

    [Fact]
    public void Cluster_SameInputAcross20Runs_ProducesIdenticalClustersEveryTime()
    {
        var documents = LoadSyntheticCore200();
        var component = new RetrievalAugmentedClusteringComponent();

        var baseline = Describe(component.Cluster(documents));

        for (var run = 0; run < 20; run++)
        {
            var result = Describe(component.Cluster(documents));
            Assert.Equal(baseline, result);
        }
    }

    [Fact]
    public void Cluster_EmptyDocumentList_ReturnsNoClusters()
    {
        Assert.Empty(new RetrievalAugmentedClusteringComponent().Cluster([]));
    }

    [Fact]
    public void Cluster_SingleDocument_ReturnsOneClusterContainingIt()
    {
        var documents = new[]
        {
            new DocumentRecord("only.txt", DocType.Sow, "Statement of work for widget maintenance.", null),
        };

        var clusters = new RetrievalAugmentedClusteringComponent().Cluster(documents);

        var cluster = Assert.Single(clusters);
        Assert.Equal(new[] { "only.txt" }, cluster.DocumentFilenames);
    }

    private static DocumentCluster FindClusterContaining(IReadOnlyList<DocumentCluster> clusters, string fileName) =>
        Assert.Single(clusters, c => c.DocumentFilenames.Contains(fileName));

    private static string Describe(IReadOnlyList<DocumentCluster> clusters) =>
        string.Join(
            "|",
            clusters
                .OrderBy(c => c.ClusterId, StringComparer.Ordinal)
                .Select(c => $"{c.ClusterId}:{string.Join(",", c.DocumentFilenames)}"));

    private static List<DocumentRecord> LoadSyntheticCore200()
    {
        return Directory.EnumerateFiles(SyntheticCore200Directory, "doc-*.txt")
            .OrderBy(f => f, StringComparer.Ordinal)
            .Select(path => new DocumentRecord(
                Path.GetFileName(path),
                DocType.Sow,
                File.ReadAllText(path),
                null))
            .ToList();
    }
}
