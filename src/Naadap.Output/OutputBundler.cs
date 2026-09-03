using System.Text.Json;
using Naadap.Core;

namespace Naadap.Output;

/// <summary>
/// OUT-440: orchestrates every Output-stage step (DATA-OUT-300, OUT-400,
/// OUT-410, OUT-420, OUT-430) for one run and writes the single indexing
/// <c>manifest.json</c> (the <see cref="RunManifest"/> shape) that ties them
/// together — the "single reviewable output bundle" the requirement asks
/// for is simply the run's output directory itself, with this file as its
/// index.
/// </summary>
public static class OutputBundler
{
    public const string ManifestFileName = "manifest.json";

    private static readonly JsonSerializerOptions manifestJsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Runs the full Output stage: ranks candidates (DATA-OUT-300), writes
    /// both visualizations (OUT-400/410), computes the summary metric
    /// (OUT-420, using a <c>ground-truth.json</c> found directly inside
    /// <paramref name="inputDirectory"/> if any — see
    /// <see cref="GroundTruth.TryLoad"/>), copies in the validation-
    /// methodology document (OUT-430), and writes <c>manifest.json</c>
    /// indexing all of it (OUT-440) to <paramref name="outputDirectory"/>.
    /// </summary>
    public static RunManifest Bundle(
        string inputDirectory,
        string outputDirectory,
        IReadOnlyList<DocumentRecord> documents,
        IReadOnlyList<DocumentCluster> clusters,
        IReadOnlyList<SkippedFile> skippedFiles)
    {
        var candidates = VehicleRecommender.Recommend(documents, clusters);

        var methodVisualizationPath = MethodVisualizationWriter.Write(
            outputDirectory, documents.Count, skippedFiles.Count, clusters);
        var resultVisualizationPath = ResultVisualizationWriter.Write(outputDirectory, candidates);
        var validationMethodologyPath = ValidationMethodologyWriter.Write(outputDirectory);

        var groundTruth = GroundTruth.TryLoad(inputDirectory);
        var summaryMetric = MetricCalculator.ComputePrecisionAtFive(candidates, groundTruth);

        var manifest = new RunManifest(
            candidates,
            methodVisualizationPath,
            resultVisualizationPath,
            summaryMetric,
            validationMethodologyPath,
            skippedFiles);

        File.WriteAllText(
            Path.Combine(outputDirectory, ManifestFileName),
            JsonSerializer.Serialize(manifest, manifestJsonOptions));

        return manifest;
    }
}
