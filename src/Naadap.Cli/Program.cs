using Naadap.Ingestion;

namespace Naadap.Cli;

/// <summary>
/// UI-001 entrypoint. Reads no interactive input (safe to run with
/// <c>&lt;/dev/null</c>) and parses arguments, ensures the output directory
/// exists, then runs the Ingestion stage (DATA-IN-1xx). Real
/// Core -&gt; Output (+ optional LlmStep) wiring lands as each stage is
/// implemented in later RTVM issues; see the activity diagram in
/// docs/SDD.md. Ingestion never terminates the run early (DATA-IN-110) —
/// this entrypoint always reaches exit 0 for a well-formed invocation,
/// regardless of how many individual files were skipped.
/// </summary>
public static class Program
{
    public static int Main(string[] args)
    {
        if (!CliArgumentParser.TryParse(args, out var arguments, out var error))
        {
            Console.Error.WriteLine(error);
            Console.Error.WriteLine("Usage: naadap --input <dir> --output <dir>");
            return 1;
        }

        Directory.CreateDirectory(arguments!.OutputDirectory);

        var ingestionResult = IngestionRunner.CreateDefault().IngestDirectory(arguments.InputDirectory);
        WriteIngestionReport(arguments.OutputDirectory, ingestionResult);

        // Pipeline stages are wired in one by one as their RTVM items land:
        // Core (CORE-2xx) -> Output.Recommend (DATA-OUT-300) -> Output
        // visualization/metrics/bundler (OUT-4xx), with the optional LlmStep
        // (CORE-250) gated behind config between Recommend and Viz. Nothing
        // downstream of Ingestion to run yet.
        return 0;
    }

    /// <summary>
    /// DATA-IN-110: writes a human-readable run report listing every
    /// skipped file and its reason (plus the successful-ingestion count) to
    /// the output directory. This is superseded/absorbed by OUT-440's full
    /// manifest bundle once the Output stage exists; until then it is the
    /// reviewable record DATA-IN-110 requires.
    /// </summary>
    private static void WriteIngestionReport(string outputDirectory, IngestionResult result)
    {
        var lines = new List<string>
        {
            $"Ingested {result.Records.Count} document(s); skipped {result.SkippedFiles.Count} file(s).",
        };

        lines.AddRange(result.SkippedFiles.Select(skipped => $"SKIPPED: {skipped.SourceFilename} - {skipped.Reason}"));

        File.WriteAllLines(Path.Combine(outputDirectory, "ingestion-report.txt"), lines);
    }
}
