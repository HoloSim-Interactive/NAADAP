namespace Naadap.Core.Tests;

public class TfIdfVectorizerTests
{
    [Fact]
    public void Vectorize_TermUniqueToOneDocument_WeightsHigherThanTermInEveryDocument()
    {
        var documents = new[]
        {
            new DocumentRecord("d1.txt", DocType.Sow, "flight line engineering avionics turnaround", null),
            new DocumentRecord("d2.txt", DocType.Sow, "fire control radar maintenance turnaround", null),
            new DocumentRecord("d3.txt", DocType.Sow, "custodial grounds keeping turnaround", null),
        };

        var vectors = TfIdfVectorizer.Vectorize(documents);

        // "turnaround" appears in all three documents (low IDF); "avionics"
        // appears only in d1 (high IDF). Both occur once in d1, so avionics
        // must outweigh turnaround in d1's own vector.
        Assert.True(vectors[0]["avionics"] > vectors[0]["turnaround"]);
    }

    [Fact]
    public void Vectorize_EachDocumentVector_IsL2Normalized()
    {
        var documents = new[]
        {
            new DocumentRecord("d1.txt", DocType.Sow, "flight line engineering avionics turnaround discrepancy", null),
            new DocumentRecord("d2.txt", DocType.Pws, "fire control radar maintenance calibration", null),
        };

        var vectors = TfIdfVectorizer.Vectorize(documents);

        foreach (var vector in vectors)
        {
            var norm = Math.Sqrt(vector.Values.Sum(w => w * w));
            Assert.Equal(1.0, norm, precision: 9);
        }
    }

    [Fact]
    public void Vectorize_DocumentWithNoContentWords_YieldsEmptyVector()
    {
        var documents = new[]
        {
            new DocumentRecord("empty.txt", DocType.Unknown, "1.1 2.2 -- --", null),
        };

        var vectors = TfIdfVectorizer.Vectorize(documents);

        Assert.Empty(vectors[0]);
    }

    [Fact]
    public void CosineSimilarity_IdenticalVectors_IsOne()
    {
        var documents = new[]
        {
            new DocumentRecord("d1.txt", DocType.Sow, "flight line engineering avionics turnaround", null),
        };
        var vectors = TfIdfVectorizer.Vectorize(documents);

        var similarity = TfIdfVectorizer.CosineSimilarity(vectors[0], vectors[0]);

        Assert.Equal(1.0, similarity, precision: 9);
    }

    [Fact]
    public void CosineSimilarity_DisjointVocabularies_IsZero()
    {
        var documents = new[]
        {
            new DocumentRecord("d1.txt", DocType.Sow, "flight line avionics turnaround", null),
            new DocumentRecord("d2.txt", DocType.Sow, "custodial grounds landscaping mowing", null),
        };
        var vectors = TfIdfVectorizer.Vectorize(documents);

        var similarity = TfIdfVectorizer.CosineSimilarity(vectors[0], vectors[1]);

        Assert.Equal(0.0, similarity, precision: 9);
    }
}
