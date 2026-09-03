using Naadap.Core;

namespace Naadap.Output.Tests;

/// <summary>
/// TP-300 (DATA-OUT-300): exercises <see cref="VehicleRecommender"/> against
/// CORE-200's own synthetic-core200 test set (see
/// <c>tests/fixtures/README.md</c>), copied alongside the test binaries.
/// </summary>
public class VehicleRecommenderTests
{
    private static string SyntheticCore200Directory =>
        Path.Combine(AppContext.BaseDirectory, "fixtures", "synthetic-core200");

    [Fact]
    public void Recommend_SyntheticCore200Set_ProducesRankedCandidatesWithScoreAndContributors()
    {
        var documents = LoadSyntheticCore200();
        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);

        var candidates = VehicleRecommender.Recommend(documents, clusters);

        // TP-300: >= 1 candidate, each with a numeric score and a non-empty
        // list of contributing source documents.
        Assert.NotEmpty(candidates);
        foreach (var candidate in candidates)
        {
            Assert.False(double.IsNaN(candidate.Score));
            Assert.InRange(candidate.Score, 0.0, 1.0);
            Assert.NotEmpty(candidate.ContributingDocuments);
            Assert.NotEmpty(candidate.VehicleId);
        }
    }

    [Fact]
    public void Recommend_IsRankedDescendingByScore()
    {
        var documents = LoadSyntheticCore200();
        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);

        var candidates = VehicleRecommender.Recommend(documents, clusters);

        var scores = candidates.Select(c => c.Score).ToList();
        Assert.Equal(scores.OrderByDescending(s => s), scores);
    }

    [Fact]
    public void Recommend_EmptyClusterList_ReturnsEmpty()
    {
        var documents = LoadSyntheticCore200();

        var candidates = VehicleRecommender.Recommend(documents, []);

        Assert.Empty(candidates);
    }

    [Fact]
    public void Recommend_IsDeterministic_AcrossRepeatedCalls()
    {
        var documents = LoadSyntheticCore200();
        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);

        var first = VehicleRecommender.Recommend(documents, clusters);
        var second = VehicleRecommender.Recommend(documents, clusters);

        Assert.Equal(
            first.Select(c => (c.VehicleId, c.Score)),
            second.Select(c => (c.VehicleId, c.Score)));
    }

    private static List<DocumentRecord> LoadSyntheticCore200()
    {
        return Directory.EnumerateFiles(SyntheticCore200Directory, "*.txt")
            .OrderBy(f => f, StringComparer.Ordinal)
            .Select(f => new DocumentRecord(Path.GetFileName(f), DocType.Unknown, File.ReadAllText(f), null))
            .ToList();
    }
}
