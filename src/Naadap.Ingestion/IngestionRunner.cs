using Naadap.Core;
using Naadap.Ingestion.Parsers;

namespace Naadap.Ingestion;

/// <summary>
/// DATA-IN-1xx orchestrator: enumerates an input directory and turns every
/// file it can into a <see cref="DocumentRecord"/>, skipping and recording
/// (never crashing on) anything malformed/unsupported/corrupt (DATA-IN-110).
/// </summary>
/// <remarks>
/// This is the "ingestion core" DATA-IN-120 requires to stay untouched when
/// a new format is added: <see cref="IngestDirectory"/> only ever dispatches
/// by asking each registered <see cref="IDocumentParser"/> "can you handle
/// this file?" — it never switches on file extension or document type
/// itself. Adding a new format means writing a new <see
/// cref="IDocumentParser"/> and adding it to the list passed into the
/// constructor (or to <see cref="CreateDefault"/>'s list); this loop does
/// not change.
/// </remarks>
public sealed class IngestionRunner
{
    private readonly IReadOnlyList<IDocumentParser> parsers;

    public IngestionRunner(IEnumerable<IDocumentParser> parserList)
    {
        parsers = parserList.ToList();
    }

    /// <summary>
    /// The built-in parser set: PDF, DOCX, plain text (DATA-IN-100's three
    /// required formats). A caller wiring in an additional format (DATA-IN-120)
    /// passes an extended list into the constructor directly instead of using
    /// this factory — see <c>Naadap.Ingestion.Tests</c> for an example that adds
    /// a fake parser without touching this class.
    /// </summary>
    public static IngestionRunner CreateDefault() => new(new IDocumentParser[]
    {
        new PdfDocumentParser(),
        new DocxDocumentParser(),
        new PlainTextDocumentParser(),
    });

    /// <summary>
    /// Ingests every file directly inside <paramref name="inputDirectory"/>
    /// (non-recursive). Never throws for an individual file's parse failure
    /// — see class remarks and DATA-IN-110.
    /// </summary>
    public IngestionResult IngestDirectory(string inputDirectory)
    {
        var records = new List<DocumentRecord>();
        var skipped = new List<SkippedFile>();

        foreach (var filePath in Directory.EnumerateFiles(inputDirectory).OrderBy(f => f, StringComparer.Ordinal))
        {
            var fileName = Path.GetFileName(filePath);
            var parser = parsers.FirstOrDefault(p => p.CanParse(filePath));

            if (parser is null)
            {
                skipped.Add(new SkippedFile(fileName, $"unsupported file type: '{Path.GetExtension(filePath)}'"));
                continue;
            }

            try
            {
                var text = parser.ExtractText(filePath);
                var docType = DocumentTypeClassifier.Classify(fileName, text);
                var date = DocumentDateExtractor.Extract(text);
                records.Add(new DocumentRecord(fileName, docType, text, date));
            }
            catch (DocumentParseException ex)
            {
                skipped.Add(new SkippedFile(fileName, ex.Message));
            }
            catch (Exception ex)
            {
                // Defense-in-depth: even a parser bug that throws something
                // other than DocumentParseException must not terminate the
                // batch run (DATA-IN-110) — record it as a skip instead of
                // letting it propagate out of IngestDirectory.
                skipped.Add(new SkippedFile(fileName, $"unexpected error: {ex.Message}"));
            }
        }

        return new IngestionResult(records, skipped);
    }
}
