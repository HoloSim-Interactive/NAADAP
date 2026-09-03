using Naadap.Core;
using Naadap.Ingestion;
using Naadap.LlmStep;
using Naadap.Output;

namespace Naadap.Cli;

/// <summary>
/// UI-001 entrypoint: a single invocation, pointed at an input document
/// directory and an output directory, runs the full pipeline end to end
/// with no interactive prompts (reads no interactive input; safe to run
/// with <c>&lt;/dev/null&gt;</c>) — Ingestion (DATA-IN-1xx) -&gt; Core
/// clustering (CORE-200) -&gt; optional LLM summarization (CORE-250) -&gt;
/// Output ranking/visualization/metrics/bundling (DATA-OUT-300, OUT-4xx),
/// matching the activity diagram in docs/SDD.md. The LLM step is off by
/// default (NFR-510): unless <see cref="CliArguments.EnableLlmStep"/> or
/// <c>NAADAP_LLM_ENABLED=true</c> is set, <see cref="RunOptionalLlmStep"/>
/// returns before constructing any model client, so no network call is even
/// attempted. Neither Ingestion (DATA-IN-110) nor any Output step aborts the
/// run early — this entrypoint always reaches exit 0 for a well-formed
/// invocation, regardless of how many individual files were skipped or how
/// few clusters/candidates were found, or whether the optional LLM step ran.
/// </summary>
public static class Program
{
    public static int Main(string[] args)
    {
        if (!CliArgumentParser.TryParse(args, out var arguments, out var error))
        {
            Console.Error.WriteLine(error);
            Console.Error.WriteLine("Usage: naadap --input <dir> --output <dir> [--enable-llm-step]");
            return 1;
        }

        Directory.CreateDirectory(arguments!.OutputDirectory);

        var ingestionResult = IngestionRunner.CreateDefault().IngestDirectory(arguments.InputDirectory);

        var clusters = new TfIdfCosineClusteringComponent().Cluster(ingestionResult.Records);

        RunOptionalLlmStep(arguments, ingestionResult.Records, clusters);

        OutputBundler.Bundle(
            arguments.InputDirectory,
            arguments.OutputDirectory,
            ingestionResult.Records,
            clusters,
            ingestionResult.SkippedFiles);

        return 0;
    }

    /// <summary>
    /// CORE-250: runs the optional summarization step between Recommend and
    /// Output's visualization/bundling stages, per docs/SDD.md's activity
    /// diagram. Writes its own audit artifacts (see
    /// <see cref="LlmRunLogWriter"/>) directly into the output directory,
    /// independent of OUT-440's manifest — this never touches
    /// <c>Naadap.Core.RunManifest</c>'s already-verified schema. Ranking is
    /// recomputed here (cheap, deterministic, identical to what
    /// <c>OutputBundler.Bundle</c> below computes again) purely to build the
    /// LLM prompt; this keeps the already-verified Output stage untouched
    /// rather than threading an optional feature through it.
    /// </summary>
    private static void RunOptionalLlmStep(
        CliArguments arguments,
        IReadOnlyList<DocumentRecord> documents,
        IReadOnlyList<DocumentCluster> clusters)
    {
        var config = LlmStepConfig.FromEnvironment(arguments.EnableLlmStep);
        if (!config.Enabled)
        {
            // NFR-510 default path: no client constructed, no network
            // attempted, not even a skip artifact written for the common
            // case where the feature was never asked for.
            return;
        }

        var candidates = VehicleRecommender.Recommend(documents, clusters);
        var result = LlmSummarizationStep
            .RunAsync(config, candidates, new HttpModelClient(apiKey: config.ApiKey))
            .GetAwaiter()
            .GetResult();

        LlmRunLogWriter.Write(arguments.OutputDirectory, result);
    }
}
