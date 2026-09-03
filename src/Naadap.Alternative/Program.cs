using Naadap.Core;
using Naadap.Ingestion;
using Naadap.Output;

namespace Naadap.Alternative;

/// <summary>
/// CORE-260's offline analysis entry point — deliberately <em>not</em> part
/// of <c>Naadap.Cli</c>'s production run (docs/SDD.md: "not a second
/// production path wired into the CLI/OUT-440 bundle"). Invoked manually
/// (<c>dotnet run --project src/Naadap.Alternative -- [input-dir]
/// [report-output-path]</c>) during development/validation against the same
/// N=20 reference set the non-LLM core is validated against
/// (<c>tests/fixtures/reference-20/</c>), to produce the comparison table
/// TP-260 requires.
/// </summary>
public static class Program
{
    public static int Main(string[] args)
    {
        var inputDirectory = args.Length > 0 ? args[0] : DefaultReferenceDirectory();
        var reportOutputPath = args.Length > 1 ? args[1] : null;

        if (!Directory.Exists(inputDirectory))
        {
            Console.Error.WriteLine($"Input directory not found: {inputDirectory}");
            Console.Error.WriteLine(
                "Usage: dotnet run --project src/Naadap.Alternative -- [input-dir] [report-output-path]");
            return 1;
        }

        var ingestionResult = IngestionRunner.CreateDefault().IngestDirectory(inputDirectory);
        var groundTruth = GroundTruth.TryLoad(inputDirectory);

        var coreResult = ApproachRunner.Run(
            "Non-LLM core (TF-IDF cosine, global-threshold single-link)",
            new TfIdfCosineClusteringComponent(),
            ingestionResult.Records,
            groundTruth);

        var alternativeResult = ApproachRunner.Run(
            "Alternative (retrieval-based, mutual k-NN, TF-IDF)",
            new RetrievalAugmentedClusteringComponent(),
            ingestionResult.Records,
            groundTruth);

        var report = ComparisonReportWriter.BuildMarkdown(
            inputDirectory,
            ingestionResult.Records.Count,
            ingestionResult.SkippedFiles.Count,
            coreResult,
            alternativeResult);

        Console.WriteLine(report);

        if (reportOutputPath is not null)
        {
            File.WriteAllText(reportOutputPath, report);
        }

        return 0;
    }

    /// <summary>
    /// Walks up from the executing assembly's location to the repo root and
    /// into <c>tests/fixtures/reference-20</c> — the N=20 set TP-260 names —
    /// so a bare <c>dotnet run --project src/Naadap.Alternative</c> works
    /// with no arguments from a checked-out clone.
    /// </summary>
    private static string DefaultReferenceDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Naadap.sln")))
        {
            directory = directory.Parent;
        }

        var repoRoot = directory?.FullName ?? Directory.GetCurrentDirectory();
        return Path.Combine(repoRoot, "tests", "fixtures", "reference-20");
    }
}
