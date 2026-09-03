using Naadap.Core;

namespace Naadap.Ingestion.Tests;

/// <summary>
/// TP-120: demonstrates that a new document-format handler can be added
/// entirely behind the <see cref="IDocumentParser"/> extension point,
/// without editing <see cref="IngestionRunner"/>'s dispatch logic
/// (<see cref="IngestionRunner.IngestDirectory"/>) at all. <see
/// cref="FakeMarkdownDocumentParser"/> below is deliberately defined in the
/// test project, not <c>Naadap.Ingestion</c>, to prove the point: it is
/// wired in purely via the public constructor.
/// </summary>
public class IngestionRunnerExtensibilityTests
{
    [Fact]
    public void IngestDirectory_WithExtraRegisteredParser_HandlesNewFormatWithoutCoreChanges()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "naadap-ext-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var markdownPath = Path.Combine(tempDirectory, "sow-99-new-format.md");
        File.WriteAllText(markdownPath, "# Statement of Work\nSample markdown content.");

        // The runner is composed purely via its public constructor with a
        // new, externally-defined parser -- IngestionRunner's own source
        // (including its dispatch loop) is untouched to support this.
        var runner = new IngestionRunner(new IDocumentParser[] { new FakeMarkdownDocumentParser() });

        var result = runner.IngestDirectory(tempDirectory);

        Assert.Empty(result.SkippedFiles);
        var record = Assert.Single(result.Records);
        Assert.Equal("sow-99-new-format.md", record.SourceFilename);
        Assert.Contains("Statement of Work", record.ExtractedText);
        Assert.Equal(DocType.Sow, record.DocType);
    }

    /// <summary>
    /// A stand-in "new document type" handler (e.g. Markdown) added purely
    /// via <see cref="IDocumentParser"/> -- exactly what DATA-IN-120 asks a
    /// reviewer to be able to do.
    /// </summary>
    private sealed class FakeMarkdownDocumentParser : IDocumentParser
    {
        public bool CanParse(string filePath) =>
            string.Equals(Path.GetExtension(filePath), ".md", StringComparison.OrdinalIgnoreCase);

        public string ExtractText(string filePath) => File.ReadAllText(filePath);
    }
}
