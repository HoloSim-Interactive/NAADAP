using System.Text.RegularExpressions;

namespace Naadap.Core;

/// <summary>
/// Splits a <see cref="DocumentRecord.ExtractedText"/> into the content-bearing
/// terms <see cref="TfIdfVectorizer"/> weights (CORE-200). Deliberately BCL-only
/// (<see cref="Regex"/> is part of the .NET runtime, not a NuGet package) so it
/// never risks pulling a dependency into the CORE-240 zero-package code path.
/// </summary>
public static partial class Tokenizer
{
    /// <summary>
    /// A standard English function-word stoplist plus generic acquisition-
    /// document boilerplate ("shall", "contractor", "solicitation", ...).
    /// Deliberately domain-neutral: it is not tuned to any one requirement
    /// theme, so it does not bias which acquisition topic a document is
    /// clustered toward. Differential term weighting across genuinely
    /// distinct themes is left to <see cref="TfIdfVectorizer"/>'s IDF
    /// term, not to this list.
    /// </summary>
    private static readonly HashSet<string> stopWords = new(StringComparer.Ordinal)
    {
        "a", "an", "and", "are", "as", "at", "be", "by", "for", "from", "has",
        "have", "had", "in", "is", "it", "its", "of", "on", "or", "that",
        "the", "to", "was", "were", "will", "with", "this", "these", "those",
        "shall", "contractor", "provide", "provides", "provided", "providing",
        "support", "supports", "supported", "supporting", "service",
        "services", "during", "per", "all", "each", "any", "also",
        "including", "include", "includes", "not", "no", "if", "then",
        "than", "into", "within", "under", "over", "such", "other", "which",
        "who", "whom", "their", "they", "them", "he", "she", "his", "her",
        "we", "you", "your", "our", "us", "may", "must", "should", "could",
        "would", "can", "requirement", "requirements", "scope", "place",
        "performance", "solicitation", "title", "notional", "installation",
        "government", "furnished", "document", "documented", "data", "number",
        "section", "applicable", "accordance", "including", "task", "tasks",
        "objective", "objectives",
    };

    [GeneratedRegex("[A-Za-z][A-Za-z\\-']*")]
    private static partial Regex WordPattern();

    /// <summary>
    /// Lowercases <paramref name="text"/>, extracts letter-runs (hyphenated
    /// compounds like "flight-line" survive as one token, since a hyphen
    /// inside a real acquisition term is itself a meaningful signal), and
    /// drops stopwords and tokens shorter than 3 characters (mostly
    /// abbreviations/numbering noise, e.g. section labels).
    /// </summary>
    public static IReadOnlyList<string> Tokenize(string text)
    {
        var matches = WordPattern().Matches(text.ToLowerInvariant());
        var tokens = new List<string>(matches.Count);

        foreach (Match match in matches)
        {
            var token = match.Value;
            if (token.Length >= 3 && !stopWords.Contains(token))
            {
                tokens.Add(token);
            }
        }

        return tokens;
    }
}
