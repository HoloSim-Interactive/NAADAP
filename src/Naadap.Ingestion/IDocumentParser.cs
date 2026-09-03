namespace Naadap.Ingestion;

/// <summary>
/// DATA-IN-120's extension point: the only thing a new input format needs
/// to implement to be picked up by <see cref="IngestionRunner"/>. Adding
/// support for a new file format means writing a new <see
/// cref="IDocumentParser"/> implementation and registering it (see
/// <see cref="IngestionRunner.CreateDefault"/>) — the runner's dispatch
/// loop (<see cref="IngestionRunner.IngestDirectory"/>) never special-cases
/// any format by name or extension itself; it only asks each registered
/// parser whether it can handle a given file.
/// </summary>
public interface IDocumentParser
{
    /// <summary>
    /// Returns true if this parser can handle the given file, based on
    /// file extension (cheap, no I/O). <see cref="IngestionRunner"/> tries
    /// registered parsers in order and uses the first match.
    /// </summary>
    bool CanParse(string filePath);

    /// <summary>
    /// Extracts the raw text content of the file. Throws <see
    /// cref="DocumentParseException"/> with a human-readable reason
    /// (DATA-IN-110) if the file is malformed/corrupt and cannot be read,
    /// even though <see cref="CanParse"/> matched it by extension.
    /// </summary>
    string ExtractText(string filePath);
}
