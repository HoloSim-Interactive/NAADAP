using Naadap.Core;

namespace Naadap.Output;

/// <summary>
/// OUT-400: writes a visualization of the analysis method for one run —
/// the pipeline stages actually executed, instantiated with this run's real
/// counts (documents ingested/skipped, clusters formed, the similarity
/// threshold used), not just static prose. Written as a Markdown file
/// containing a Mermaid flowchart, the same diagramming convention
/// docs/SDD.md's own architecture diagrams use — this needs no image-
/// generation library (DELIV-920's minimal-dependency policy) and renders
/// directly in GitHub, VS Code, and any Mermaid-aware viewer.
/// </summary>
public static class MethodVisualizationWriter
{
    public const string FileName = "method-visualization.md";

    public static string Write(
        string outputDirectory,
        int documentCount,
        int skippedCount,
        IReadOnlyList<DocumentCluster> clusters)
    {
        var lines = new List<string>
        {
            "# Analysis Method (OUT-400)",
            "",
            $"This run ingested **{documentCount}** document(s) and skipped **{skippedCount}**, " +
            $"then grouped the ingested documents into **{clusters.Count}** cluster(s) using " +
            $"TF-IDF cosine-similarity clustering (CORE-200), with a similarity threshold of " +
            $"**{TfIdfCosineClusteringComponent.SimilarityThreshold:0.00}** (see " +
            "`Naadap.Core.TfIdfCosineClusteringComponent` for the threshold's derivation).",
            "",
            "```mermaid",
            "flowchart TD",
            "    Input[\"Input directory\"] --> Ingest[\"Ingestion & Normalization\\n(DATA-IN-100/110)\"]",
            $"    Ingest --> Core[\"TF-IDF cosine clustering\\n(CORE-200)\\n{documentCount} document(s) -> {clusters.Count} cluster(s)\"]",
            "    Core --> Recommend[\"Vehicle recommendation\\n(DATA-OUT-300)\"]",
            "    Recommend --> Bundle[\"Output bundle\\n(OUT-440)\"]",
            "```",
            "",
            "## Clusters formed this run",
            "",
            "| Cluster | Documents | Top terms |",
            "| --- | --- | --- |",
        };

        foreach (var cluster in clusters)
        {
            lines.Add(
                $"| {cluster.ClusterId} | {cluster.DocumentFilenames.Count} | " +
                $"{string.Join(", ", cluster.TopTerms)} |");
        }

        File.WriteAllLines(Path.Combine(outputDirectory, FileName), lines);
        return FileName;
    }
}
