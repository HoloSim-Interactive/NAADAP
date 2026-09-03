using Naadap.Core;

namespace Naadap.Ingestion.Tests;

/// <summary>
/// TP-100 / TP-110: exercises <see cref="IngestionRunner"/> against the
/// checked-in <c>tests/fixtures/smoke/</c> corpus (see
/// <c>tests/fixtures/README.md</c>), copied alongside the test binaries by
/// this project's <c>.csproj</c>.
/// </summary>
public class IngestionRunnerSmokeTests
{
    private static string SmokeDirectory => Path.Combine(AppContext.BaseDirectory, "fixtures", "smoke");

    [Fact]
    public void IngestDirectory_SmokeSetWithoutCorruptedFile_Produces6NormalizedRecords()
    {
        // TP-100: 2 SOW, 1 PWS, 1 CDRL, 1 sources-sought, 1 open-source,
        // mixed PDF/DOCX -> 6 normalized records, no crash.
        var cleanBatch = CopyToTempDirectory(excludeCorrupted: true);

        var result = IngestionRunner.CreateDefault().IngestDirectory(cleanBatch);

        Assert.Equal(6, result.Records.Count);
        Assert.Empty(result.SkippedFiles);

        foreach (var record in result.Records)
        {
            Assert.False(string.IsNullOrWhiteSpace(record.ExtractedText));
            Assert.False(string.IsNullOrWhiteSpace(record.SourceFilename));
        }

        AssertDocTypePresent(result.Records, "sow-01-beq-m400-paintflooring.pdf", DocType.Sow);
        AssertDocTypePresent(result.Records, "sow-02-advanced-power.docx", DocType.Sow);
        AssertDocTypePresent(result.Records, "pws-01-nswccd-ta-instruments.pdf", DocType.Pws);
        AssertDocTypePresent(result.Records, "cdrl-01-nswccd-waters-inspection.pdf", DocType.Cdrl);
        AssertDocTypePresent(result.Records, "sources-sought-01-rot-frcs.pdf", DocType.SourcesSought);
        AssertDocTypePresent(result.Records, "open-source-text-01-v22-osprey-testimony.pdf", DocType.OpenSource);
    }

    [Fact]
    public void IngestDirectory_SmokeSetWithCorruptedFile_Skips1AndProcesses6WithoutThrowing()
    {
        // TP-110: TP-100's batch plus 1 corrupted (truncated) PDF -> 6
        // records processed, run report lists the 7th as skipped with a
        // human-readable reason. Ingestion must not throw / terminate the
        // batch.
        var fullBatch = CopyToTempDirectory(excludeCorrupted: false);

        var result = IngestionRunner.CreateDefault().IngestDirectory(fullBatch);

        Assert.Equal(6, result.Records.Count);
        Assert.Single(result.SkippedFiles);

        var skipped = result.SkippedFiles[0];
        Assert.Equal("corrupted-truncated.pdf", skipped.SourceFilename);
        Assert.False(string.IsNullOrWhiteSpace(skipped.Reason));
    }

    private static void AssertDocTypePresent(IReadOnlyList<DocumentRecord> records, string fileName, DocType expected)
    {
        var record = Assert.Single(records, r => r.SourceFilename == fileName);
        Assert.Equal(expected, record.DocType);
    }

    /// <summary>
    /// Copies the smoke fixtures into an isolated temp directory per test so
    /// tests can freely include/exclude the corrupted file without
    /// interfering with each other or mutating the checked-in fixtures.
    /// </summary>
    private static string CopyToTempDirectory(bool excludeCorrupted)
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "naadap-smoke-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        foreach (var file in Directory.EnumerateFiles(SmokeDirectory))
        {
            var fileName = Path.GetFileName(file);
            if (excludeCorrupted && fileName == "corrupted-truncated.pdf")
            {
                continue;
            }

            File.Copy(file, Path.Combine(tempDirectory, fileName));
        }

        return tempDirectory;
    }
}
