namespace Naadap.Core;

/// <summary>
/// DATA-IN-100's normalized internal record: what every ingested source
/// document (SOW, PWS, CDRL, sources-sought notice, open-source text, in
/// PDF/DOCX/plain text) is reduced to before it reaches the Core clustering
/// engine. Lives in <c>Naadap.Core</c> (rather than <c>Naadap.Ingestion</c>)
/// so it is a dependency-free shared contract both Ingestion (producer) and
/// Core (consumer) can reference without Core ever needing a project
/// reference to Ingestion — see docs/SDD.md's CORE-240 zero-third-party-
/// dependency note.
/// </summary>
/// <param name="SourceFilename">File name (not full path) the record was extracted from.</param>
/// <param name="DocType">Document-category classification.</param>
/// <param name="ExtractedText">Full extracted plain text of the document. Never null or empty for a successfully-ingested record.</param>
/// <param name="Date">Date found in the source document, if any (e.g. solicitation issue date).</param>
public sealed record DocumentRecord(
    string SourceFilename,
    DocType DocType,
    string ExtractedText,
    DateOnly? Date);
