namespace Naadap.Core;

/// <summary>
/// CORE-200's chosen non-LLM clustering algorithm: TF-IDF term weighting
/// (<see cref="TfIdfVectorizer"/>) plus cosine-similarity connected
/// components. Two documents join the same cluster if their similarity
/// meets <see cref="SimilarityThreshold"/> directly, or transitively
/// through a chain of documents that each meet it pairwise (single-link
/// clustering) — the simplest connectivity rule that still lets a cluster
/// span more than a single strong pair, which the reference themes need
/// (e.g. CORE-200's {A, C, E} triple).
/// </summary>
/// <remarks>
/// <para>
/// <b>Why this algorithm:</b> requirement-statement text is short,
/// vocabulary-driven, and the acquisition domain already gives strong,
/// literal term overlap within a real theme (shared nouns like
/// "flight-line", "fire-control", "calibration") — a lexical
/// bag-of-words method captures that directly, with no training data, no
/// model weights, and a result a reviewer can audit term-by-term. This
/// keeps the module dependency-free (CORE-240) and is why CORE-260's
/// LLM/retrieval-based alternative is evaluated separately rather than
/// used here.
/// </para>
/// <para>
/// <b>Threshold derivation (documented per CORE-200/TP-200):</b>
/// <see cref="SimilarityThreshold"/> = 0.35 was chosen by computing the
/// full pairwise cosine-similarity matrix for the TP-200 synthetic
/// 10-document fixture (<c>tests/fixtures/synthetic-core200/</c>). The
/// two intended themes' internal similarities ranged 0.560-0.792; the
/// highest similarity between any two documents that do *not* share a
/// theme (including between the five distractors) was 0.229. 0.35 sits
/// in the middle of that gap, with roughly 0.12 of margin on each side,
/// so the exact value is not fragile to small vocabulary changes.
/// </para>
/// <para>
/// <b>Determinism (CORE-210):</b> every step is pure arithmetic over the
/// input list's own order — pairs are compared in a fixed (i, j) index
/// order, union-find operates on integer indices (never a hash-based
/// key), and final cluster/member ordering is an explicit ordinal sort
/// on filename, never on dictionary/hash-set iteration order. The same
/// input list therefore always produces byte-identical output.
/// </para>
/// </remarks>
public sealed class TfIdfCosineClusteringComponent : IClusteringComponent
{
    /// <summary>
    /// Minimum cosine similarity for two documents to be linked into the
    /// same cluster. See the type-level remarks for how this value was
    /// derived from the TP-200 reference fixture.
    /// </summary>
    public const double SimilarityThreshold = 0.35;

    /// <summary>Number of top-weighted terms recorded per cluster's <see cref="DocumentCluster.TopTerms"/>.</summary>
    private const int topTermCount = 5;

    public IReadOnlyList<DocumentCluster> Cluster(IReadOnlyList<DocumentRecord> documents)
    {
        if (documents.Count == 0)
        {
            return [];
        }

        var vectors = TfIdfVectorizer.Vectorize(documents);
        var unionFind = new UnionFind(documents.Count);

        for (var i = 0; i < documents.Count; i++)
        {
            for (var j = i + 1; j < documents.Count; j++)
            {
                var similarity = TfIdfVectorizer.CosineSimilarity(vectors[i], vectors[j]);
                if (similarity >= SimilarityThreshold)
                {
                    unionFind.Union(i, j);
                }
            }
        }

        var groupsByRoot = new Dictionary<int, List<int>>();
        for (var i = 0; i < documents.Count; i++)
        {
            var root = unionFind.Find(i);
            if (!groupsByRoot.TryGetValue(root, out var members))
            {
                members = [];
                groupsByRoot[root] = members;
            }

            members.Add(i);
        }

        // Order clusters deterministically by the ordinal-smallest source
        // filename among their members, not by root index (an artifact of
        // union-find's internal tree shape) or dictionary iteration order.
        var orderedGroups = groupsByRoot.Values
            .OrderBy(members => members.Min(i => documents[i].SourceFilename), StringComparer.Ordinal)
            .ToList();

        var clusters = new List<DocumentCluster>(orderedGroups.Count);
        for (var clusterIndex = 0; clusterIndex < orderedGroups.Count; clusterIndex++)
        {
            var members = orderedGroups[clusterIndex];
            var clusterId = $"cluster-{clusterIndex + 1:D4}";
            var filenames = members
                .Select(i => documents[i].SourceFilename)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();
            var topTerms = ComputeTopTerms(members, vectors);

            clusters.Add(new DocumentCluster(clusterId, filenames, topTerms));
        }

        return clusters;
    }

    private static IReadOnlyList<string> ComputeTopTerms(
        IReadOnlyList<int> memberIndices,
        IReadOnlyList<IReadOnlyDictionary<string, double>> vectors)
    {
        var averageWeight = new Dictionary<string, double>(StringComparer.Ordinal);
        foreach (var index in memberIndices)
        {
            foreach (var (term, weight) in vectors[index])
            {
                averageWeight[term] = averageWeight.GetValueOrDefault(term) + (weight / memberIndices.Count);
            }
        }

        return averageWeight
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => pair.Key, StringComparer.Ordinal)
            .Take(topTermCount)
            .Select(pair => pair.Key)
            .ToList();
    }

    /// <summary>
    /// Disjoint-set-union over document indices only (never over hashable
    /// keys), so cluster formation depends solely on the fixed index order
    /// of the input list — union-by-rank with path compression, the
    /// standard near-constant-time construction (no third-party library
    /// needed, keeping this on the CORE-240 dependency-free path).
    /// </summary>
    private sealed class UnionFind(int size)
    {
        private readonly int[] parent = Enumerable.Range(0, size).ToArray();
        private readonly int[] rank = new int[size];

        public int Find(int i)
        {
            if (parent[i] != i)
            {
                parent[i] = Find(parent[i]);
            }

            return parent[i];
        }

        public void Union(int a, int b)
        {
            var rootA = Find(a);
            var rootB = Find(b);
            if (rootA == rootB)
            {
                return;
            }

            if (rank[rootA] < rank[rootB])
            {
                (rootA, rootB) = (rootB, rootA);
            }

            parent[rootB] = rootA;
            if (rank[rootA] == rank[rootB])
            {
                rank[rootA]++;
            }
        }
    }
}
