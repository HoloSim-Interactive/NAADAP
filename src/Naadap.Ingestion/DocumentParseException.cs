namespace Naadap.Ingestion;

/// <summary>
/// Thrown by an <see cref="IDocumentParser"/> when a file it claimed to
/// handle (<see cref="IDocumentParser.CanParse"/> returned true) turns out
/// to be malformed, unsupported in practice, or corrupt (DATA-IN-110).
/// <see cref="IngestionRunner"/> catches this — and any other exception a
/// parser throws — and records it as a <c>SkippedFile</c> rather than
/// letting it terminate the batch run.
/// </summary>
public sealed class DocumentParseException : Exception
{
    public DocumentParseException(string reason)
        : base(reason)
    {
    }

    public DocumentParseException(string reason, Exception innerException)
        : base(reason, innerException)
    {
    }
}
