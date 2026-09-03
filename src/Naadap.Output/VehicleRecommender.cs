using Naadap.Core;

namespace Naadap.Output;

/// <summary>
/// DATA-OUT-300: turns Core's <see cref="DocumentCluster"/>s (CORE-200) into
/// a ranked list of <see cref="CandidateVehicle"/>s — the pipeline's
/// recommendation output. One cluster maps to exactly one candidate; there
/// is no merging or splitting here, since Core has already decided which
/// documents share requirement content. This stage only scores and orders
/// that decision, and derives a human-readable identifier for it.
/// </summary>
public static class VehicleRecommender
{
    /// <summary>
    /// Ranks <paramref name="clusters"/> into <see cref="CandidateVehicle"/>s,
    /// highest score first. <paramref name="documents"/> must be the same
    /// list (same order not required) that produced <paramref name="clusters"/>
    /// — it is re-vectorized here (same TF-IDF representation CORE-200's own
    /// clustering step used) purely to score cluster cohesion; Output never
    /// re-decides cluster membership.
    /// </summary>
    public static IReadOnlyList<CandidateVehicle> Recommend(
        IReadOnlyList<DocumentRecord> documents,
        IReadOnlyList<DocumentCluster> clusters)
    {
        if (clusters.Count == 0)
        {
            return [];
        }

        var vectorsByFilename = BuildVectorsByFilename(documents);

        var candidates = clusters
            .Select(cluster => BuildCandidate(cluster, vectorsByFilename))
            .ToList();

        // Deterministic ranking (CORE-210's determinism guarantee carried
        // forward into Output): highest cohesion first; ties broken by more
        // corroborating documents, then ordinally by VehicleId so the order
        // never depends on clustering/dictionary iteration artifacts.
        return candidates
            .OrderByDescending(c => c.Score)
            .ThenByDescending(c => c.ContributingDocuments.Count)
            .ThenBy(c => c.VehicleId, StringComparer.Ordinal)
            .ToList();
    }

    private static Dictionary<string, IReadOnlyDictionary<string, double>> BuildVectorsByFilename(
        IReadOnlyList<DocumentRecord> documents)
    {
        var vectors = TfIdfVectorizer.Vectorize(documents);
        var byFilename = new Dictionary<string, IReadOnlyDictionary<string, double>>(StringComparer.Ordinal);
        for (var i = 0; i < documents.Count; i++)
        {
            byFilename[documents[i].SourceFilename] = vectors[i];
        }

        return byFilename;
    }

    private static CandidateVehicle BuildCandidate(
        DocumentCluster cluster,
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, double>> vectorsByFilename)
    {
        var score = ComputeCohesion(cluster, vectorsByFilename);
        var vehicleId = BuildVehicleId(cluster);

        return new CandidateVehicle(vehicleId, score, cluster.DocumentFilenames);
    }

    /// <summary>
    /// Mean pairwise cosine similarity among the cluster's members — the
    /// same metric CORE-200's clustering step thresholds on
    /// (<see cref="TfIdfCosineClusteringComponent.SimilarityThreshold"/>), so
    /// a candidate's score is directly comparable to that threshold and
    /// auditable the same way. A single-document cluster has no pair to
    /// compare, so it is scored 1.0 by convention (its one document is, by
    /// definition, perfectly self-consistent).
    /// </summary>
    private static double ComputeCohesion(
        DocumentCluster cluster,
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, double>> vectorsByFilename)
    {
        if (cluster.DocumentFilenames.Count <= 1)
        {
            return 1.0;
        }

        var vectors = cluster.DocumentFilenames
            .Select(filename => vectorsByFilename[filename])
            .ToList();

        var total = 0.0;
        var pairs = 0;
        for (var i = 0; i < vectors.Count; i++)
        {
            for (var j = i + 1; j < vectors.Count; j++)
            {
                total += TfIdfVectorizer.CosineSimilarity(vectors[i], vectors[j]);
                pairs++;
            }
        }

        return pairs == 0 ? 1.0 : total / pairs;
    }

    /// <summary>
    /// Derives a deterministic, auditable identifier from the cluster's
    /// evidence rather than looking up (or inventing via an LLM call, which
    /// CORE-240's spirit forbids anywhere on this path) a real-world
    /// contract-vehicle name. Format: up to 3 of the cluster's top terms,
    /// joined by hyphens, uppercased, prefixed by the cluster's own stable
    /// ID for uniqueness even if two clusters happen to share top terms.
    /// </summary>
    private static string BuildVehicleId(DocumentCluster cluster)
    {
        var slugTerms = cluster.TopTerms.Take(3).Select(t => t.ToUpperInvariant());
        var slug = string.Join("-", slugTerms);
        return slug.Length == 0
            ? cluster.ClusterId.ToUpperInvariant()
            : $"{cluster.ClusterId.ToUpperInvariant()}-{slug}";
    }
}
