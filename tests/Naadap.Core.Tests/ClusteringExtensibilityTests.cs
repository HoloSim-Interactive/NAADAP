namespace Naadap.Core.Tests;

/// <summary>
/// TP-120's "new clustering component" half (DATA-IN-120/DELIV-940) —
/// deferred from the Ingestion issue (#7) since <see cref="IClusteringComponent"/>
/// had no shape until CORE-200 defined it (see that issue's hand-off note).
/// Demonstrates that a caller can plug in a brand-new clustering strategy
/// purely by implementing <see cref="IClusteringComponent"/>, with zero
/// changes to <see cref="TfIdfCosineClusteringComponent"/> or any other
/// existing Core source — <see cref="SingleClusterComponent"/> below is
/// deliberately defined in the test project, not <c>Naadap.Core</c>, to
/// prove the point.
/// </summary>
public class ClusteringExtensibilityTests
{
    [Fact]
    public void CustomClusteringComponent_PlugsInViaInterfaceAlone_WithoutCoreChanges()
    {
        IClusteringComponent component = new SingleClusterComponent();
        var documents = new[]
        {
            new DocumentRecord("one.txt", DocType.Sow, "First document.", null),
            new DocumentRecord("two.txt", DocType.Pws, "Second, unrelated document.", null),
        };

        var clusters = component.Cluster(documents);

        var cluster = Assert.Single(clusters);
        Assert.Equal(new[] { "one.txt", "two.txt" }, cluster.DocumentFilenames);
    }

    /// <summary>
    /// A stand-in "new clustering strategy" (deliberately trivial: everything
    /// lands in one cluster) — exactly what DATA-IN-120 asks a reviewer to
    /// be able to add via the documented extension point only.
    /// </summary>
    private sealed class SingleClusterComponent : IClusteringComponent
    {
        public IReadOnlyList<DocumentCluster> Cluster(IReadOnlyList<DocumentRecord> documents)
        {
            if (documents.Count == 0)
            {
                return [];
            }

            var filenames = documents.Select(d => d.SourceFilename).ToList();
            return [new DocumentCluster("cluster-0001", filenames, [])];
        }
    }
}
