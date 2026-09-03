using Naadap.Core;
using Naadap.Ingestion;
using Naadap.Output;

namespace Naadap.Cli;

/// <summary>
/// UI-001 entrypoint: a single invocation, pointed at an input document
/// directory and an output directory, runs the full pipeline end to end
/// with no interactive prompts (reads no interactive input; safe to run
/// with <c>&lt;/dev/null&gt;</c>) — Ingestion (DATA-IN-1xx) -&gt; Core
/// clustering (CORE-200) -&gt; Output ranking/visualization/metrics/bundling
/// (DATA-OUT-300, OUT-4xx), matching the activity diagram in docs/SDD.md.
/// The optional LLM step (CORE-250) is not wired in here yet — it stays
/// off the default path until it exists and is gated behind an explicit
/// config flag, per docs/SDD.md's activity diagram. Neither Ingestion
/// (DATA-IN-110) nor any Output step aborts the run early — this entrypoint
/// always reaches exit 0 for a well-formed invocation, regardless of how
/// many individual files were skipped or how few clusters/candidates were
/// found.
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

        var clusters = new TfIdfCosineClusteringComponent().Cluster(ingestionResult.Records);

        OutputBundler.Bundle(
            arguments.InputDirectory,
            arguments.OutputDirectory,
            ingestionResult.Records,
            clusters,
            ingestionResult.SkippedFiles);

        return 0;
    }
}
