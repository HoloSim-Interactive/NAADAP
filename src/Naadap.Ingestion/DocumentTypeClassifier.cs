using System.Text.RegularExpressions;
using Naadap.Core;

namespace Naadap.Ingestion;

/// <summary>
/// Classifies a <see cref="DocType"/> for an ingested file. This is a
/// content-category tag (DATA-IN-100), orthogonal to file format — it is
/// not the DATA-IN-120 extension point (that is <see
/// cref="IDocumentParser"/>, which is format-based); adding recognition
/// for a new naming convention here does not require touching parser
/// dispatch logic at all, since classification runs after a parser has
/// already produced text.
/// </summary>
public static class DocumentTypeClassifier
{
    /// <summary>
    /// Classifies from the source file name using keyword tokens (cheap,
    /// deterministic, no LLM call — consistent with keeping the ingestion
    /// path non-LLM). Falls back to scanning the extracted text's opening
    /// section if the filename is inconclusive, then to <see
    /// cref="DocType.Unknown"/>.
    /// </summary>
    public static DocType Classify(string fileName, string extractedText)
    {
        var fromName = ClassifyTokens(Tokenize(Path.GetFileNameWithoutExtension(fileName)));
        if (fromName != DocType.Unknown)
        {
            return fromName;
        }

        // Fall back to the first ~2000 characters of extracted text, which is
        // typically enough to catch a document's own title/heading.
        var textSample = extractedText.Length > 2000 ? extractedText[..2000] : extractedText;
        return ClassifyTokens(Tokenize(textSample));
    }

    private static List<string> Tokenize(string value) =>
        Regex.Split(value.ToLowerInvariant(), @"[^a-z0-9]+")
            .Where(token => token.Length > 0)
            .ToList();

    private static DocType ClassifyTokens(List<string> tokens)
    {
        if (ContainsSequence(tokens, "sources", "sought"))
        {
            return DocType.SourcesSought;
        }

        if (ContainsSequence(tokens, "open", "source"))
        {
            return DocType.OpenSource;
        }

        if (tokens.Contains("cdrl"))
        {
            return DocType.Cdrl;
        }

        if (tokens.Contains("pws"))
        {
            return DocType.Pws;
        }

        if (tokens.Contains("sow"))
        {
            return DocType.Sow;
        }

        return DocType.Unknown;
    }

    private static bool ContainsSequence(List<string> tokens, string first, string second)
    {
        for (var i = 0; i < tokens.Count - 1; i++)
        {
            if (tokens[i] == first && tokens[i + 1] == second)
            {
                return true;
            }
        }

        return false;
    }
}
