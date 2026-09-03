namespace Naadap.Ingestion.Tests;

/// <summary>
/// DATA-IN-110's "unsupported" case (as distinct from "malformed/corrupt",
/// covered by <see cref="IngestionRunnerSmokeTests"/>): a file whose format
/// no registered <see cref="IDocumentParser"/> claims at all.
/// </summary>
public class IngestionRunnerUnsupportedFileTests
{
    [Fact]
    public void IngestDirectory_UnsupportedExtension_IsSkippedWithReasonAndDoesNotThrow()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "naadap-unsupported-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        File.WriteAllText(Path.Combine(tempDirectory, "notes.rtf"), "unsupported format content");

        var result = IngestionRunner.CreateDefault().IngestDirectory(tempDirectory);

        Assert.Empty(result.Records);
        var skipped = Assert.Single(result.SkippedFiles);
        Assert.Equal("notes.rtf", skipped.SourceFilename);
        Assert.Contains("unsupported", skipped.Reason, StringComparison.OrdinalIgnoreCase);
    }
}
