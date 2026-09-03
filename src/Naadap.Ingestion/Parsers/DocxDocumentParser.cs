using DocumentFormat.OpenXml.Packaging;

namespace Naadap.Ingestion.Parsers;

/// <summary>
/// <see cref="IDocumentParser"/> implementation for <c>.docx</c> files,
/// backed by the Open XML SDK. Registered by default in <see
/// cref="IngestionRunner.CreateDefault"/>.
/// </summary>
public sealed class DocxDocumentParser : IDocumentParser
{
    public bool CanParse(string filePath) =>
        string.Equals(Path.GetExtension(filePath), ".docx", StringComparison.OrdinalIgnoreCase);

    public string ExtractText(string filePath)
    {
        try
        {
            using var document = WordprocessingDocument.Open(filePath, false);
            var body = document.MainDocumentPart?.Document?.Body;
            var text = body?.InnerText ?? string.Empty;

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new DocumentParseException("unable to parse: DOCX contained no extractable text");
            }

            return text;
        }
        catch (DocumentParseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Covers malformed OOXML package structure, missing parts, etc.
            throw new DocumentParseException($"unable to parse: malformed or corrupt DOCX package ({ex.Message})", ex);
        }
    }
}
