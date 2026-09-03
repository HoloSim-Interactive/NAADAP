using System.Text.Json;

namespace Naadap.Cli.Tests;

/// <summary>
/// TP-001 (UI-001) / TP-440 (OUT-440): runs the full CLI entrypoint against
/// the checked-in TP-100/TP-110 smoke fixture (6 valid documents + 1
/// corrupted PDF) and confirms one invocation produces the complete OUT-440
/// output bundle with no interactive prompt (this entrypoint never reads
/// <see cref="Console.In"/> anywhere in its call graph — see
/// <see cref="Program.Main"/> and everything it calls).
/// </summary>
public class EndToEndPipelineTests : IDisposable
{
    private readonly string outputDirectory = Path.Combine(Path.GetTempPath(), "naadap-cli-out-" + Guid.NewGuid());

    private static string SmokeInputDirectory => Path.Combine(AppContext.BaseDirectory, "fixtures", "smoke");

    public void Dispose()
    {
        if (Directory.Exists(outputDirectory))
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Fact]
    public void Main_FullSmokeSet_ExitsZeroAndWritesCompleteOutputBundle()
    {
        var exitCode = Program.Main(["--input", SmokeInputDirectory, "--output", outputDirectory]);

        // TP-001: exit 0, output directory contains the OUT-440 bundle.
        Assert.Equal(0, exitCode);

        Assert.True(File.Exists(Path.Combine(outputDirectory, "manifest.json")));
        Assert.True(File.Exists(Path.Combine(outputDirectory, "method-visualization.md")));
        Assert.True(File.Exists(Path.Combine(outputDirectory, "result-visualization.md")));
        Assert.True(File.Exists(Path.Combine(outputDirectory, "validation-methodology.md")));
    }

    [Fact]
    public void Main_FullSmokeSet_ManifestRecordsTheCorruptedFileAsSkipped()
    {
        Program.Main(["--input", SmokeInputDirectory, "--output", outputDirectory]);

        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(outputDirectory, "manifest.json")));
        var skippedFiles = document.RootElement.GetProperty("skippedFiles");

        Assert.True(skippedFiles.GetArrayLength() >= 1);
        var skippedNames = skippedFiles.EnumerateArray()
            .Select(e => e.GetProperty("sourceFilename").GetString())
            .ToList();
        Assert.Contains("corrupted-truncated.pdf", skippedNames);
    }

    [Fact]
    public void Main_FullSmokeSet_ManifestCandidatesHaveScoresAndContributors()
    {
        Program.Main(["--input", SmokeInputDirectory, "--output", outputDirectory]);

        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(outputDirectory, "manifest.json")));
        var candidates = document.RootElement.GetProperty("candidates");

        Assert.True(candidates.GetArrayLength() >= 1);
        foreach (var candidate in candidates.EnumerateArray())
        {
            Assert.True(candidate.GetProperty("score").GetDouble() >= 0.0);
            Assert.True(candidate.GetProperty("contributingDocuments").GetArrayLength() > 0);
        }
    }
}
