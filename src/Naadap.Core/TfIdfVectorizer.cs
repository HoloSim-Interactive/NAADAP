namespace Naadap.Core;

/// <summary>
/// Turns a corpus of <see cref="DocumentRecord"/>s into TF-IDF term-weight
/// vectors (CORE-200's non-LLM requirement-content representation) and
/// scores similarity between them by cosine distance. Pure BCL arithmetic —
/// no external library, satisfying CORE-240 for the module that feeds
/// clustering.
/// </summary>
/// <remarks>
/// Standard "smooth IDF" weighting (the same smoothing scikit-learn's
/// <c>TfidfVectorizer</c> uses by default): tf(t, d) is the term's relative
/// frequency within d (count / total tokens in d, so document length does
/// not itself bias the weight), and idf(t) = ln((1 + N) / (1 + df(t))) + 1,
/// where N is the corpus size and df(t) is the number of documents
/// containing t at least once. The "+1" keeps every term's weight strictly
/// positive (even a term present in every document still contributes,
/// rather than dropping to zero) and avoids a division by zero when a term
/// is absent from the corpus. Each document's resulting vector is then
/// L2-normalized so a plain dot product between two vectors already equals
/// their cosine similarity.
/// </remarks>
public static class TfIdfVectorizer
{
    /// <summary>
    /// Computes one L2-normalized TF-IDF vector per document, in the same
    /// order as <paramref name="documents"/>. A document whose text
    /// tokenizes to nothing (e.g. empty extracted text) gets an empty
    /// vector, which has cosine similarity 0 with every other document —
    /// it will not join any cluster, which is the correct behavior for
    /// content-free input.
    /// </summary>
    public static IReadOnlyList<IReadOnlyDictionary<string, double>> Vectorize(
        IReadOnlyList<DocumentRecord> documents)
    {
        var tokenized = documents
            .Select(d => Tokenizer.Tokenize(d.ExtractedText))
            .ToList();

        var documentFrequency = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var tokens in tokenized)
        {
            foreach (var term in new HashSet<string>(tokens, StringComparer.Ordinal))
            {
                documentFrequency[term] = documentFrequency.GetValueOrDefault(term) + 1;
            }
        }

        var corpusSize = documents.Count;
        var vectors = new List<IReadOnlyDictionary<string, double>>(corpusSize);

        foreach (var tokens in tokenized)
        {
            vectors.Add(BuildVector(tokens, documentFrequency, corpusSize));
        }

        return vectors;
    }

    private static IReadOnlyDictionary<string, double> BuildVector(
        IReadOnlyList<string> tokens,
        IReadOnlyDictionary<string, int> documentFrequency,
        int corpusSize)
    {
        if (tokens.Count == 0)
        {
            return new Dictionary<string, double>(StringComparer.Ordinal);
        }

        var termCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var term in tokens)
        {
            termCounts[term] = termCounts.GetValueOrDefault(term) + 1;
        }

        var weights = new Dictionary<string, double>(StringComparer.Ordinal);
        foreach (var (term, count) in termCounts)
        {
            var termFrequency = (double)count / tokens.Count;
            var df = documentFrequency.GetValueOrDefault(term);
            var inverseDocumentFrequency = Math.Log((1.0 + corpusSize) / (1.0 + df)) + 1.0;
            weights[term] = termFrequency * inverseDocumentFrequency;
        }

        var norm = Math.Sqrt(weights.Values.Sum(w => w * w));
        if (norm > 0)
        {
            foreach (var term in weights.Keys.ToList())
            {
                weights[term] /= norm;
            }
        }

        return weights;
    }

    /// <summary>
    /// Cosine similarity between two already-L2-normalized vectors (a plain
    /// dot product, since normalization already divided out both vectors'
    /// magnitudes). Iterates the smaller vector for efficiency; result is
    /// symmetric regardless of argument order.
    /// </summary>
    public static double CosineSimilarity(
        IReadOnlyDictionary<string, double> a,
        IReadOnlyDictionary<string, double> b)
    {
        var (smaller, larger) = a.Count <= b.Count ? (a, b) : (b, a);

        var dot = 0.0;
        foreach (var (term, weight) in smaller)
        {
            if (larger.TryGetValue(term, out var otherWeight))
            {
                dot += weight * otherWeight;
            }
        }

        return dot;
    }
}
