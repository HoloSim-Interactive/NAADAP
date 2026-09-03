namespace Naadap.Ingestion.Parsers;

/// <summary>
/// <see cref="IDocumentParser"/> implementation for <c>.txt</c> files.
/// Registered by default in <see cref="IngestionRunner.CreateDefault"/>.
/// </summary>
public sealed class PlainTextDocumentParser : IDocumentParser
{
    public bool CanParse(string filePath) =>
        string.Equals(Path.GetExtension(filePath), ".txt", StringComparison.OrdinalIgnoreCase);

    public string ExtractText(string filePath)
    {
        string text;
        try
        {
            text = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            throw new DocumentParseException($"unable to parse: could not read text file ({ex.Message})", ex);
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new DocumentParseException("unable to parse: text file is empty");
        }

        return text;
    }
}
