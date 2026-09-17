using System.Globalization;

namespace Naadap.Output;

/// <summary>
/// Writes <c>vehicle-ranking.tsv</c>: the complete per-cluster ranking of
/// every knowledge-base vehicle, including those below the evidence floor and
/// those eliminated by a hard constraint. The manifest's per-cluster record
/// carries only the top few of each; this file is the full account, so no
/// candidate is ever suppressed from the run's output (client direction
/// 2026-09-17).
/// </summary>
public static class VehicleRankingWriter
{
    public const string FileName = "vehicle-ranking.tsv";

    public static string Write(string outputDirectory, IReadOnlyList<VehicleRankingRow> rows)
    {
        var lines = new List<string>(rows.Count + 1)
        {
            "cluster_id\trank\tvehicle_id\tscore\tlexical\taffinity\ttier\tstatus",
        };
        foreach (var r in rows)
        {
            lines.Add(string.Join('\t',
                r.ClusterId, r.Rank.ToString(CultureInfo.InvariantCulture), r.VehicleId,
                r.Score.ToString("0.000000", CultureInfo.InvariantCulture),
                r.Lexical.ToString("0.000000", CultureInfo.InvariantCulture),
                r.Affinity.ToString("0.000000", CultureInfo.InvariantCulture),
                r.Tier.ToString(CultureInfo.InvariantCulture), r.Status));
        }

        File.WriteAllLines(Path.Combine(outputDirectory, FileName), lines);
        return FileName;
    }
}
