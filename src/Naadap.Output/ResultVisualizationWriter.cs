using Naadap.Core;

namespace Naadap.Output;

/// <summary>
/// OUT-410: writes a visualization of this run's results — the ranked
/// candidate-vehicle list and the cluster-to-vehicle mapping backing it —
/// distinct from OUT-400's method visualization (see docs/SDD.md, both are
/// required as separate artifacts per TP-400/TP-410). Same Markdown +
/// Mermaid convention as <see cref="MethodVisualizationWriter"/>, for the
/// same no-new-dependency reason.
/// </summary>
public static class ResultVisualizationWriter
{
    public const string FileName = "result-visualization.md";

    public static string Write(string outputDirectory, IReadOnlyList<CandidateVehicle> rankedCandidates)
    {
        var lines = new List<string>
        {
            "# Results (OUT-410)",
            "",
            $"**{rankedCandidates.Count}** candidate vehicle(s) recommended this run, ranked by " +
            "score (cluster cohesion — see DATA-OUT-300 in docs/SDD.md).",
            "",
            "| Rank | Candidate Vehicle | Score | Contributing Documents |",
            "| --- | --- | --- | --- |",
        };

        for (var i = 0; i < rankedCandidates.Count; i++)
        {
            var candidate = rankedCandidates[i];
            lines.Add(
                $"| {i + 1} | {candidate.VehicleId} | {candidate.Score:0.000} | " +
                $"{string.Join(", ", candidate.ContributingDocuments)} |");
        }

        lines.Add(string.Empty);
        lines.Add("```mermaid");
        lines.Add("flowchart LR");

        for (var i = 0; i < rankedCandidates.Count; i++)
        {
            var candidate = rankedCandidates[i];
            var nodeId = $"V{i + 1}";
            lines.Add($"    {nodeId}[\"#{i + 1} {candidate.VehicleId}\\nscore {candidate.Score:0.000}\"]");

            foreach (var document in candidate.ContributingDocuments)
            {
                var docNodeId = $"{nodeId}_{SanitizeMermaidId(document)}";
                lines.Add($"    {docNodeId}[\"{document}\"] --> {nodeId}");
            }
        }

        lines.Add("```");

        File.WriteAllLines(Path.Combine(outputDirectory, FileName), lines);
        return FileName;
    }

    /// <summary>Mermaid node IDs must be alphanumeric/underscore; filenames carry dots/hyphens.</summary>
    private static string SanitizeMermaidId(string filename)
    {
        var chars = filename.Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray();
        return new string(chars);
    }
}
