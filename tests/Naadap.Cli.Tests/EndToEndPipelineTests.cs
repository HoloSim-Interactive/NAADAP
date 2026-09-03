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

    [Fact]
    public void Main_LlmStepNotEnabled_NeverWritesLlmRunLog()
    {
        // NFR-510/CORE-250 default path: the LLM step's own audit artifact
        // must not even appear on disk when the feature was never asked
        // for -- not merely "empty", genuinely absent.
        var exitCode = Program.Main(["--input", SmokeInputDirectory, "--output", outputDirectory]);

        Assert.Equal(0, exitCode);
        Assert.False(File.Exists(Path.Combine(outputDirectory, "llm-run-log.json")));
    }

    [Fact]
    public void Main_LlmStepEnabledWithNoEndpointConfigured_WritesRunLogRecordingSkipAndStillExitsZero()
    {
        // CORE-250 enabled via --enable-llm-step but with no
        // NAADAP_LLM_ENDPOINT configured in this test environment: the run
        // must still complete (never abort the pipeline for an optional,
        // misconfigured step) and the run log must explain why nothing ran.
        var exitCode = Program.Main(["--input", SmokeInputDirectory, "--output", outputDirectory, "--enable-llm-step"]);

        Assert.Equal(0, exitCode);

        var runLogPath = Path.Combine(outputDirectory, "llm-run-log.json");
        Assert.True(File.Exists(runLogPath));

        using var runLog = JsonDocument.Parse(File.ReadAllText(runLogPath));
        Assert.True(runLog.RootElement.GetProperty("enabled").GetBoolean());
        Assert.True(runLog.RootElement.GetProperty("skipped").GetBoolean());
        Assert.Equal(0, runLog.RootElement.GetProperty("totalTokensUsed").GetInt32());
        Assert.Empty(runLog.RootElement.GetProperty("networkCalls").EnumerateArray());

        // manifest.json (OUT-440) must still be produced, unaffected by the
        // optional step's outcome.
        Assert.True(File.Exists(Path.Combine(outputDirectory, "manifest.json")));
    }
}
