using Naadap.Core;

namespace Naadap.Alternative;

/// <summary>
/// CORE-260's alternative clustering strategy: a retrieval-based (RAG-style)
/// approach, evaluated only offline against the same reference set as
/// <c>Naadap.Core.TfIdfCosineClusteringComponent</c> to substantiate that
/// component's selection (DELIV-970) — never referenced from
/// <c>Naadap.Core</c> or <c>Naadap.Cli</c> (docs/SDD.md).
/// </summary>
/// <remarks>
/// <para>
/// <b>Why this counts as "retrieval-based":</b> for every document, this
/// algorithm's first step is exactly RAG's retrieval step — rank every
/// other document by similarity and keep only the top-<see cref="K"/>
/// ("retrieved neighbors"). Two documents are then linked into the same
/// cluster only if they retrieve each other (a <em>mutual</em> k-NN edge),
/// and clusters are the connected components of that retrieval graph. This
/// is a genuinely different connectivity rule from Core's approach — a
/// single global similarity threshold applied to every pair — not a
/// relabeling of the same algorithm: mutual k-NN is locally adaptive (a
/// document in a dense topic area needs to beat more competitors to retain
/// an edge than one in a sparse area), where Core's fixed threshold is not.
/// </para>
/// <para>
/// <b>Why no live LLM/generation call:</b> the requirement's own phrasing
/// (docs/RTVM.md CORE-260, docs/PROJECT_DEFINITION.md Scope) lists
/// "LLM-assisted <em>or</em> retrieval/RAG-based" as alternatives — this
/// implements the retrieval half directly, using the same TF-IDF vector
/// space Core already computes (<c>Naadap.Core.TfIdfVectorizer</c>), rather
/// than fabricating a stand-in for a live USN-approved model call this
/// development environment has no accredited endpoint to reach. This keeps
/// the comparison run's LLM token cost honestly reported as zero for this
/// particular alternative (see the comparison report), rather than an
/// invented number — itself a documented finding, not a limitation of the
/// analysis.
/// </para>
/// <para>
/// <b>Determinism:</b> same construction discipline as Core's component —
/// fixed index order, union-find over integer indices only, final ordering
/// an explicit ordinal sort, never dictionary/hash-set iteration order.
/// </para>
/// </remarks>
public sealed class RetrievalAugmentedClusteringComponent : IClusteringComponent
{
    /// <summary>
    /// Retrieval depth: how many nearest neighbors each document retrieves.
    /// Chosen small (per typical RAG top-k retrieval practice) so a
    /// document must be a strong match for one of only a few slots, not
    /// merely "more similar than a fixed cutoff" — the property this
    /// algorithm exists to compare against Core's approach.
    /// </summary>
    public const int K = 2;

    private const int topTermCount = 5;

    public IReadOnlyList<DocumentCluster> Cluster(IReadOnlyList<DocumentRecord> documents)
    {
        if (documents.Count == 0)
        {
            return [];
        }

        var vectors = TfIdfVectorizer.Vectorize(documents);
        var neighbors = RetrieveTopKNeighbors(vectors);

        var unionFind = new UnionFind(documents.Count);
        for (var i = 0; i < documents.Count; i++)
        {
            foreach (var j in neighbors[i])
            {
                // Mutual retrieval only: i must be in j's top-K exactly as j
                // is in i's. Guard i < j purely to avoid unioning the same
                // pair twice; it does not change which pairs qualify.
                if (j > i && neighbors[j].Contains(i))
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

        var orderedGroups = groupsByRoot.Values
            .OrderBy(members => members.Min(i => documents[i].SourceFilename), StringComparer.Ordinal)
            .ToList();

        var clusters = new List<DocumentCluster>(orderedGroups.Count);
        for (var clusterIndex = 0; clusterIndex < orderedGroups.Count; clusterIndex++)
        {
            var members = orderedGroups[clusterIndex];
            // "alt-" prefix keeps these IDs visually distinct from Core's
            // "cluster-####" IDs if ever surfaced side by side (they never
            // are in production output — comparison-report-only).
            var clusterId = $"alt-cluster-{clusterIndex + 1:D4}";
            var filenames = members
                .Select(i => documents[i].SourceFilename)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();
            var topTerms = ComputeTopTerms(members, vectors);

            clusters.Add(new DocumentCluster(clusterId, filenames, topTerms));
        }

        return clusters;
    }

    /// <summary>
    /// The retrieval step: for each document, the indices of its top-
    /// <see cref="K"/> most-similar other documents (ties broken ordinally
    /// by index, never by dictionary iteration, for determinism). A
    /// document with zero similarity to everything retrieves nothing.
    /// </summary>
    private static IReadOnlyList<HashSet<int>> RetrieveTopKNeighbors(
        IReadOnlyList<IReadOnlyDictionary<string, double>> vectors)
    {
        var n = vectors.Count;
        var neighbors = new List<HashSet<int>>(n);

        for (var i = 0; i < n; i++)
        {
            var ranked = Enumerable.Range(0, n)
                .Where(j => j != i)
                .Select(j => (Index: j, Similarity: TfIdfVectorizer.CosineSimilarity(vectors[i], vectors[j])))
                .Where(candidate => candidate.Similarity > 0)
                .OrderByDescending(candidate => candidate.Similarity)
                .ThenBy(candidate => candidate.Index)
                .Take(K)
                .Select(candidate => candidate.Index)
                .ToHashSet();

            neighbors.Add(ranked);
        }

        return neighbors;
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

    /// <summary>Disjoint-set-union over document indices only — see <c>Naadap.Core.TfIdfCosineClusteringComponent</c>'s identical rationale.</summary>
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
