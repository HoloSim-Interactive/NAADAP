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

    public static string Write(string outputDirectory, IReadOnlyList<CandidateVehicle> rankedCandidates) =>
        Write(outputDirectory, rankedCandidates, []);

    public static string Write(
        string outputDirectory,
        IReadOnlyList<CandidateVehicle> rankedCandidates,
        IReadOnlyList<VehicleRecommendation> vehicleRecommendations)
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

        if (vehicleRecommendations.Count > 0)
        {
            AppendVehicleSection(lines, vehicleRecommendations);
        }

        File.WriteAllLines(Path.Combine(outputDirectory, FileName), lines);
        return FileName;
    }

    /// <summary>Mermaid node IDs must be alphanumeric/underscore; filenames carry dots/hyphens.</summary>
    /// <summary>
    /// Per-cluster strategic-vehicle recommendations from the knowledge base:
    /// the candidates with their evidence, the candidates below the evidence
    /// floor (recorded, not suppressed), and the vehicles ruled out by a hard
    /// constraint with the constraint named. The tool identifies; it does not
    /// select. Wording stays as competing evidence, never a determination.
    /// </summary>
    private static void AppendVehicleSection(List<string> lines, IReadOnlyList<VehicleRecommendation> recs)
    {
        lines.Add(string.Empty);
        lines.Add("## Strategic-vehicle candidates per cluster");
        lines.Add(string.Empty);
        lines.Add("Candidates are knowledge-base vehicles that survived every evaluable hard constraint, " +
                  $"scored as {VehicleMatcher.LexicalWeight:0.0} × lexical scope overlap + {VehicleMatcher.AffinityWeight:0.0} × office affinity, " +
                  $"ranked with the acquisition-path tier deciding near-ties (within {VehicleMatcher.NearTieEpsilon:0.00}). " +
                  $"Evidence floor {VehicleMatcher.EvidenceFloor:0.00}. The complete ranking, including every eliminated vehicle, is in `vehicle-ranking.tsv`. " +
                  "Selection is reserved to the contracting officer and the FAR 7.107 / NMCARS 5237.102 approval authorities.");

        foreach (var r in recs)
        {
            lines.Add(string.Empty);
            lines.Add($"### {r.ClusterId}");
            lines.Add(string.Empty);
            lines.Add("Requesting office(s) found in the documents: " + (r.RequestingOffices.Count == 0 ? "none" : string.Join(", ", r.RequestingOffices)));
            if (r.NewVehicleIndicated)
            {
                lines.Add(string.Empty);
                lines.Add("**NEW-VEHICLE-INDICATED**: no knowledge-base vehicle reached the evidence floor for a cluster of two or more requirements.");
            }

            lines.Add(string.Empty);
            lines.Add("| Rank | Vehicle | Family | Tier | Score | Lexical | Affinity (office, share) | Matched terms | Deciding key |");
            lines.Add("| --- | --- | --- | --- | --- | --- | --- | --- | --- |");
            var rank = 1;
            foreach (var c in r.Candidates)
            {
                lines.Add($"| {rank++} | {c.VehicleId} | {c.Family} | {c.AcquisitionPathTier} | {c.Score:0.000} | {c.Lexical.Cosine:0.000} | " +
                          $"{(c.OfficeAffinity.Office.Length == 0 ? "—" : $"{c.OfficeAffinity.Office}, {c.OfficeAffinity.Share:0.000}")} | " +
                          $"{string.Join(" ", c.Lexical.MatchedTerms)} | {c.DecidingKey} |");
            }

            if (r.BelowEvidenceFloor.Count > 0)
            {
                lines.Add(string.Empty);
                lines.Add($"Below the evidence floor ({r.BelowEvidenceFloorTotal} in total; first {r.BelowEvidenceFloor.Count} shown): " +
                          string.Join("; ", r.BelowEvidenceFloor.Select(c => $"{c.VehicleId} ({c.Score:0.000})")));
            }

            if (r.Eliminations.Count > 0)
            {
                var byConstraint = r.Eliminations.GroupBy(e => e.Constraint).OrderBy(g => g.Key, StringComparer.Ordinal);
                lines.Add(string.Empty);
                lines.Add("Ruled out by hard constraint: " + string.Join("; ", byConstraint.Select(g => $"{g.Key} × {g.Count()} (e.g. {g.First().VehicleId}: {g.First().Detail})")));
            }
        }
    }

    private static string SanitizeMermaidId(string filename)
    {
        var chars = filename.Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray();
        return new string(chars);
    }
}
