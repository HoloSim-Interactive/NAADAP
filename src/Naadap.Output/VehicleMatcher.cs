using System.Text.RegularExpressions;
using Naadap.Core;

namespace Naadap.Output;

/// <summary>
/// Increment 1 vehicle matching (design: docs/design/vehicle-recommendation-pipeline.md,
/// "Run" steps 3 to 6). For each cluster: apply hard constraints, which
/// remove a vehicle and never lower its score; score the survivors on two
/// explainable channels (lexical scope overlap, office affinity); rank with
/// the CORE-273 tier rule inside the near-tie band; and assemble the
/// evidence record including the reasons the others were ruled out.
/// Everything is deterministic: fixed weights, fixed tie-break ladder,
/// ordinal sorts, and an as-of date taken from the knowledge-base version
/// rather than the clock.
/// </summary>
public static class VehicleMatcher
{
    /// <summary>Weight of the lexical channel in the composite score.</summary>
    public const double LexicalWeight = 0.6;

    /// <summary>Weight of the office-affinity channel in the composite score.</summary>
    public const double AffinityWeight = 0.4;

    /// <summary>Candidates scoring below this composite value are recorded under "below evidence floor", never suppressed.</summary>
    public const double EvidenceFloor = 0.05;

    /// <summary>Two candidates whose composite scores differ by at most this much are a near-tie; the lower acquisition-path tier ranks first (CORE-273).</summary>
    public const double NearTieEpsilon = 0.05;

    /// <summary>How many candidates at or above the floor, and how many below it, the per-cluster record carries. The full ranking is written separately.</summary>
    public const int MaxCandidates = 5;

    private const int maxMatchedTerms = 8;

    public const string OrderingPeriodClosed = "ORDERING-PERIOD-CLOSED";
    public const string OfficeNotEligible = "OFFICE-NOT-AMONG-OBSERVED-ORDERING-ACTIVITIES";
    public const string SetAsideCompatibility = "SET-ASIDE-COMPATIBILITY";

    private static readonly Regex dodaacPattern = new(@"\bN\d{5}\b", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>Contracting-office names that documents use where they do not print the DoDAAC.</summary>
    private static readonly (string Pattern, string Code)[] officeNames =
    [
        (@"\bNAWCTSD\b|Naval Air Warfare Center Training Systems", "N61340"),
        (@"\bNAWCAD\b|Naval Air Warfare Center Aircraft Division", "N00421"),
        (@"\bNAWCWD\b|Naval Air Warfare Center Weapons Division", "N68936"),
        (@"\bLakehurst\b", "N68335"),
        (@"\bFleet Readiness Center|\bCOMFRC\b|\bFRC[A-Z]{1,2}\b", "N68520"),
        (@"\bNAVAIR\b|Naval Air Systems Command", "N00019"),
    ];

    /// <summary>
    /// Matches every cluster against the knowledge base. Returns one record
    /// per cluster in cluster order, plus the full per-cluster ranking rows
    /// for the vehicle-ranking file.
    /// </summary>
    public static (IReadOnlyList<VehicleRecommendation> Recommendations, IReadOnlyList<VehicleRankingRow> FullRanking) Match(
        IReadOnlyList<DocumentRecord> documents,
        IReadOnlyList<DocumentCluster> clusters,
        VehicleKnowledgeBase kb)
    {
        if (clusters.Count == 0)
        {
            return ([], []);
        }

        var textByFilename = documents.ToDictionary(d => d.SourceFilename, d => d.ExtractedText, StringComparer.Ordinal);

        // One TF-IDF space over vehicle scope texts and cluster texts so the
        // cosine is comparable across vehicles for a given cluster.
        var corpus = new List<DocumentRecord>(kb.Vehicles.Count + clusters.Count);
        foreach (var v in kb.Vehicles)
        {
            corpus.Add(new DocumentRecord("kb:" + v.VehicleId, DocType.Unknown, VehicleText(v), null));
        }

        foreach (var c in clusters)
        {
            var text = string.Join("\n", c.DocumentFilenames.Select(f => textByFilename.GetValueOrDefault(f, string.Empty)));
            corpus.Add(new DocumentRecord("cluster:" + c.ClusterId, DocType.Unknown, text, null));
        }

        var vectors = TfIdfVectorizer.Vectorize(corpus);
        var vehicleVectors = vectors.Take(kb.Vehicles.Count).ToList();

        var recommendations = new List<VehicleRecommendation>(clusters.Count);
        var ranking = new List<VehicleRankingRow>();

        for (var ci = 0; ci < clusters.Count; ci++)
        {
            var cluster = clusters[ci];
            var clusterVector = vectors[kb.Vehicles.Count + ci];
            var clusterText = corpus[kb.Vehicles.Count + ci].ExtractedText;
            var offices = FindRequestingOffices(clusterText);

            var scored = new List<VehicleCandidate>();
            var eliminated = new List<VehicleElimination>();

            foreach (var (vehicle, vv) in kb.Vehicles.Zip(vehicleVectors))
            {
                var passed = new List<string>();
                var notEvaluated = new List<string> { SetAsideCompatibility };

                // Hard constraint 1: ordering period. Curated dates only;
                // parent-IDV rows carry a proxy date and are not eliminated on it.
                if (vehicle.RowKind == "family" && DateOnly.TryParseExact(vehicle.OrderingPeriodEnd, "yyyy-MM-dd", out var end))
                {
                    if (end < kb.AsOfDate)
                    {
                        eliminated.Add(new VehicleElimination(vehicle.VehicleId, OrderingPeriodClosed,
                            $"ordering period ended {vehicle.OrderingPeriodEnd}, before knowledge-base date {kb.AsOfDate:yyyy-MM-dd}"));
                        continue;
                    }

                    passed.Add(OrderingPeriodClosed);
                }
                else
                {
                    notEvaluated.Add(OrderingPeriodClosed);
                }

                // Hard constraint 2: requesting office among the vehicle's
                // ordering activities. Only checkable when the cluster names an
                // office and the row lists office codes (parent-IDV rows).
                if (offices.Count > 0 && vehicle.RowKind == "parent-idv")
                {
                    if (!vehicle.EligibleOrderingActivities.Any(offices.Contains))
                    {
                        eliminated.Add(new VehicleElimination(vehicle.VehicleId, OfficeNotEligible,
                            $"requesting office(s) {string.Join(",", offices)} not among FY2025 ordering activities {string.Join(",", vehicle.EligibleOrderingActivities)}"));
                        continue;
                    }

                    passed.Add(OfficeNotEligible);
                }
                else
                {
                    notEvaluated.Add(OfficeNotEligible);
                }

                var cosine = TfIdfVectorizer.CosineSimilarity(clusterVector, vv);
                var matched = MatchedTerms(clusterVector, vv);
                var affinity = BestAffinity(kb, vehicle.VehicleId, offices);
                var score = LexicalWeight * cosine + AffinityWeight * affinity.Share;

                scored.Add(new VehicleCandidate(
                    vehicle.VehicleId, vehicle.Name, vehicle.Family, vehicle.AcquisitionPathTier,
                    Math.Round(score, 6), new LexicalEvidence(Math.Round(cosine, 6), matched), affinity,
                    passed, notEvaluated, MarginToNext: 0, DecidingKey: string.Empty));
            }

            var ordered = Rank(scored);
            var above = ordered.Where(c => c.Score >= EvidenceFloor).ToList();
            var below = ordered.Where(c => c.Score < EvidenceFloor).ToList();

            recommendations.Add(new VehicleRecommendation(
                cluster.ClusterId,
                offices,
                above.Take(MaxCandidates).ToList(),
                below.Take(MaxCandidates).ToList(),
                below.Count,
                eliminated.OrderBy(e => e.VehicleId, StringComparer.Ordinal).ToList(),
                NewVehicleIndicated: above.Count == 0 && cluster.DocumentFilenames.Count >= 2,
                EvidenceFloor));

            for (var r = 0; r < ordered.Count; r++)
            {
                var c = ordered[r];
                ranking.Add(new VehicleRankingRow(cluster.ClusterId, r + 1, c.VehicleId, c.Score, c.Lexical.Cosine,
                    c.OfficeAffinity.Share, c.AcquisitionPathTier, c.Score >= EvidenceFloor ? "candidate" : "below-floor"));
            }

            foreach (var e in eliminated.OrderBy(e => e.VehicleId, StringComparer.Ordinal))
            {
                ranking.Add(new VehicleRankingRow(cluster.ClusterId, 0, e.VehicleId, 0, 0, 0, 0, "eliminated:" + e.Constraint));
            }
        }

        return (recommendations, ranking);
    }

    /// <summary>
    /// Deterministic ranking with the CORE-273 near-tie rule: sort by score
    /// descending; walk the list forming groups whose scores lie within
    /// <see cref="NearTieEpsilon"/> of the group leader; inside a group order
    /// by tier ascending, then score descending, then vehicle id ordinal.
    /// Each candidate records the key that placed it above the next one and
    /// its margin to the next.
    /// </summary>
    private static List<VehicleCandidate> Rank(List<VehicleCandidate> scored)
    {
        var byScore = scored
            .OrderByDescending(c => c.Score)
            .ThenBy(c => c.AcquisitionPathTier)
            .ThenBy(c => c.VehicleId, StringComparer.Ordinal)
            .ToList();

        var result = new List<VehicleCandidate>(byScore.Count);
        var i = 0;
        while (i < byScore.Count)
        {
            var leader = byScore[i].Score;
            var j = i;
            while (j < byScore.Count && leader - byScore[j].Score <= NearTieEpsilon)
            {
                j++;
            }

            result.AddRange(byScore
                .Skip(i).Take(j - i)
                .OrderBy(c => c.AcquisitionPathTier)
                .ThenByDescending(c => c.Score)
                .ThenBy(c => c.VehicleId, StringComparer.Ordinal));
            i = j;
        }

        for (var k = 0; k < result.Count; k++)
        {
            var next = k + 1 < result.Count ? result[k + 1] : null;
            var margin = next is null ? 0 : Math.Round(result[k].Score - next.Score, 6);
            var key = next is null ? "last"
                : Math.Abs(result[k].Score - next.Score) > NearTieEpsilon ? "score"
                : result[k].AcquisitionPathTier != next.AcquisitionPathTier ? "tier"
                : result[k].Score != next.Score ? "score"
                : "vehicle_id";
            result[k] = result[k] with { MarginToNext = margin, DecidingKey = key };
        }

        return result;
    }

    private static string VehicleText(VehicleRecord v) =>
        string.Join("\n", new[] { v.Name, v.ScopeText }.Concat(v.ObservedDescriptions).Concat(v.DodPrograms));

    private static IReadOnlyList<string> FindRequestingOffices(string text)
    {
        var found = new SortedSet<string>(StringComparer.Ordinal);
        foreach (Match m in dodaacPattern.Matches(text))
        {
            found.Add(m.Value);
        }

        foreach (var (pattern, code) in officeNames)
        {
            if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                found.Add(code);
            }
        }

        return found.ToList();
    }

    private static OfficeAffinityEvidence BestAffinity(VehicleKnowledgeBase kb, string vehicleId, IReadOnlyList<string> offices)
    {
        OfficeAffinityEvidence best = new(string.Empty, 0, 0);
        foreach (var office in offices)
        {
            if (kb.Affinity.TryGetValue((vehicleId, office), out var row) && row.ShareOfOfficeOrders > best.Share)
            {
                best = new OfficeAffinityEvidence(office, row.Orders, Math.Round(row.ShareOfOfficeOrders, 6));
            }
        }

        return best;
    }

    private static IReadOnlyList<string> MatchedTerms(IReadOnlyDictionary<string, double> a, IReadOnlyDictionary<string, double> b)
    {
        var (smaller, larger) = a.Count <= b.Count ? (a, b) : (b, a);
        return smaller
            .Where(kv => larger.ContainsKey(kv.Key))
            .Select(kv => (Term: kv.Key, Weight: kv.Value * larger[kv.Key]))
            .OrderByDescending(t => t.Weight)
            .ThenBy(t => t.Term, StringComparer.Ordinal)
            .Take(maxMatchedTerms)
            .Select(t => t.Term)
            .ToList();
    }
}

/// <summary>One row of the run's <c>vehicle-ranking.tsv</c>: every scored or eliminated vehicle for every cluster.</summary>
public sealed record VehicleRankingRow(string ClusterId, int Rank, string VehicleId, double Score, double Lexical, double Affinity, int Tier, string Status);
