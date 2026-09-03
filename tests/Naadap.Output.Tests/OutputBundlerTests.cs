using System.Text.Json;
using Naadap.Core;

namespace Naadap.Output.Tests;

/// <summary>
/// TP-400/TP-410/TP-430/TP-440: exercises the full Output stage orchestrated
/// by <see cref="OutputBundler"/> — both visualizations, the methodology
/// document, and the indexing manifest, all in one run.
/// </summary>
public class OutputBundlerTests : IDisposable
{
    private readonly string inputDirectory = Path.Combine(Path.GetTempPath(), "naadap-in-" + Guid.NewGuid());
    private readonly string outputDirectory = Path.Combine(Path.GetTempPath(), "naadap-out-" + Guid.NewGuid());

    public OutputBundlerTests()
    {
        Directory.CreateDirectory(inputDirectory);
        Directory.CreateDirectory(outputDirectory);
    }

    public void Dispose()
    {
        Directory.Delete(inputDirectory, recursive: true);
        Directory.Delete(outputDirectory, recursive: true);
    }

    [Fact]
    public void Bundle_WritesTwoDistinctVisualizationArtifacts()
    {
        var manifest = RunBundle();

        Assert.NotEqual(manifest.MethodVisualizationPath, manifest.ResultVisualizationPath);
        Assert.True(File.Exists(Path.Combine(outputDirectory, manifest.MethodVisualizationPath)));
        Assert.True(File.Exists(Path.Combine(outputDirectory, manifest.ResultVisualizationPath)));
    }

    [Fact]
    public void Bundle_WritesValidationMethodologyNamingCorpusGroundTruthAndMetric()
    {
        var manifest = RunBundle();

        var path = Path.Combine(outputDirectory, manifest.ValidationMethodologyPath);
        Assert.True(File.Exists(path));

        // TP-430 (Inspection): names the corpus, the ground-truth
        // derivation, and the metric definition.
        var content = File.ReadAllText(path);
        Assert.Contains("reference-20", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ground truth", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("precision@5", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Bundle_WritesManifestIndexingEveryArtifact()
    {
        RunBundle();

        var manifestPath = Path.Combine(outputDirectory, OutputBundler.ManifestFileName);
        Assert.True(File.Exists(manifestPath));

        using var document = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("candidates", out _));
        Assert.True(root.TryGetProperty("methodVisualizationPath", out _));
        Assert.True(root.TryGetProperty("resultVisualizationPath", out _));
        Assert.True(root.TryGetProperty("summaryMetric", out _));
        Assert.True(root.TryGetProperty("validationMethodologyPath", out _));
        Assert.True(root.TryGetProperty("skippedFiles", out _));
    }

    [Fact]
    public void Bundle_NoGroundTruthInInputDirectory_ReportsMetricNotComputed()
    {
        var manifest = RunBundle();

        Assert.Null(manifest.SummaryMetric.Value);
    }

    [Fact]
    public void Bundle_CarriesSkippedFilesIntoManifest()
    {
        var documents = new List<DocumentRecord>
        {
            new("a.txt", DocType.Unknown, "flight line engineering support disposition", null),
        };
        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);
        var skipped = new List<SkippedFile> { new("bad.pdf", "unable to parse: truncated PDF stream") };

        var manifest = OutputBundler.Bundle(inputDirectory, outputDirectory, documents, clusters, skipped);

        Assert.Single(manifest.SkippedFiles);
        Assert.Equal("bad.pdf", manifest.SkippedFiles[0].SourceFilename);
    }

    private RunManifest RunBundle()
    {
        var documents = new List<DocumentRecord>
        {
            new("a.txt", DocType.Unknown, "flight line engineering disposition support avionics", null),
            new("b.txt", DocType.Unknown, "shipboard fire control radar maintenance", null),
        };
        var clusters = new TfIdfCosineClusteringComponent().Cluster(documents);

        return OutputBundler.Bundle(inputDirectory, outputDirectory, documents, clusters, []);
    }
}
