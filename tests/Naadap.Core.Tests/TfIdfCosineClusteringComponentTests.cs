namespace Naadap.Core.Tests;

/// <summary>
/// TP-200 / CORE-210 (component-level): exercises
/// <see cref="TfIdfCosineClusteringComponent"/> — CORE-200's clustering
/// algorithm — against the checked-in
/// <c>tests/fixtures/synthetic-core200/</c> corpus (see
/// <c>tests/fixtures/README.md</c> and that fixture's own
/// <c>ground-truth.json</c>), copied alongside the test binaries by this
/// project's <c>.csproj</c>.
/// </summary>
public class TfIdfCosineClusteringComponentTests
{
    private static string SyntheticCore200Directory =>
        Path.Combine(AppContext.BaseDirectory, "fixtures", "synthetic-core200");

    [Fact]
    public void Cluster_SyntheticCore200Set_GroupsSharedThemesAndSeparatesDistractors()
    {
        // TP-200: {A, C, E} share one requirement theme, {B, D} share a
        // distinct theme, {F..J} are unrelated distractors. Expected: {A, C,
        // E} land in one cluster, {B, D} in a distinct cluster, and no
        // distractor joins either.
        var documents = LoadSyntheticCore200();

        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);

        var flightLineCluster = FindClusterContaining(clusters, "doc-a.txt");
        Assert.Equal(
            new[] { "doc-a.txt", "doc-c.txt", "doc-e.txt" },
            flightLineCluster.DocumentFilenames.OrderBy(f => f, StringComparer.Ordinal));

        var fireControlCluster = FindClusterContaining(clusters, "doc-b.txt");
        Assert.Equal(
            new[] { "doc-b.txt", "doc-d.txt" },
            fireControlCluster.DocumentFilenames.OrderBy(f => f, StringComparer.Ordinal));

        Assert.NotEqual(flightLineCluster.ClusterId, fireControlCluster.ClusterId);

        foreach (var distractor in new[] { "doc-f.txt", "doc-g.txt", "doc-h.txt", "doc-i.txt", "doc-j.txt" })
        {
            var distractorCluster = FindClusterContaining(clusters, distractor);
            Assert.NotEqual(flightLineCluster.ClusterId, distractorCluster.ClusterId);
            Assert.NotEqual(fireControlCluster.ClusterId, distractorCluster.ClusterId);
        }

        // Every cluster documents the terms that drove it (CORE-200's
        // "with the assignment documented alongside the similarity/score
        // threshold used" requirement).
        Assert.All(clusters, c => Assert.NotEmpty(c.TopTerms));
    }

    [Fact]
    public void Cluster_SameInputAcross20Runs_ProducesIdenticalClustersEveryTime()
    {
        // CORE-210 (component level): fixed input -> identical output in
        // >=95% of runs. The algorithm is deterministic arithmetic with no
        // randomness, so this asserts the stronger 100%/20 bar the design
        // is meant to guarantee; full end-to-end TP-210 (the N=20
        // reference-20 corpus, run through the assembled CLI) lands once
        // Output/Cli wiring exists in a later issue.
        var documents = LoadSyntheticCore200();
        var component = new TfIdfCosineClusteringComponent();

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
        Assert.Empty(new TfIdfCosineClusteringComponent().Cluster([]));
    }

    [Fact]
    public void Cluster_SingleDocument_ReturnsOneClusterContainingIt()
    {
        var documents = new[]
        {
            new DocumentRecord("only.txt", DocType.Sow, "Statement of work for widget maintenance.", null),
        };

        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);

        var cluster = Assert.Single(clusters);
        Assert.Equal(new[] { "only.txt" }, cluster.DocumentFilenames);
    }

    private static DocumentCluster FindClusterContaining(IReadOnlyList<DocumentCluster> clusters, string fileName) =>
        Assert.Single(clusters, c => c.DocumentFilenames.Contains(fileName));

    /// <summary>
    /// Reduces a clustering result to a comparable, order-independent shape
    /// (cluster ID -> sorted member set) for the reproducibility check
    /// above.
    /// </summary>
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
