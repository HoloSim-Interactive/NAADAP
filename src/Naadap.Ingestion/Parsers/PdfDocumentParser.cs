using System.Text;
using UglyToad.PdfPig;

namespace Naadap.Ingestion.Parsers;

/// <summary>
/// <see cref="IDocumentParser"/> implementation for <c>.pdf</c> files, backed
/// by PdfPig. Registered by default in <see cref="IngestionRunner.CreateDefault"/>.
/// </summary>
public sealed class PdfDocumentParser : IDocumentParser
{
    public bool CanParse(string filePath) =>
        string.Equals(Path.GetExtension(filePath), ".pdf", StringComparison.OrdinalIgnoreCase);

    public string ExtractText(string filePath)
    {
        try
        {
            using var document = PdfDocument.Open(filePath);
            var builder = new StringBuilder();
            foreach (var page in document.GetPages())
            {
                builder.AppendLine(page.Text);
            }

            var text = builder.ToString();
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new DocumentParseException("unable to parse: PDF contained no extractable text");
            }

            return text;
        }
        catch (DocumentParseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // PdfPig throws a variety of exception types (its own parsing
            // exceptions, IOException, etc.) for truncated/corrupt PDFs.
            // All of them become one human-readable, batch-run-preserving
            // skip reason (DATA-IN-110).
            throw new DocumentParseException($"unable to parse: truncated or corrupt PDF stream ({ex.Message})", ex);
        }
    }
}
