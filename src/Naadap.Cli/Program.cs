namespace Naadap.Cli;

/// <summary>
/// UI-001 entrypoint. Reads no interactive input (safe to run with
/// <c>&lt;/dev/null</c>) and, at this scaffolding stage, runs an empty
/// pipeline: it parses arguments, ensures the output directory exists, and
/// exits 0. Real Ingestion -&gt; Core -&gt; Output (+ optional LlmStep) wiring
/// lands as each stage is implemented in later RTVM issues; see the
/// activity diagram in docs/SDD.md.
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

        // Pipeline stages are wired in one by one as their RTVM items land:
        // Ingestion (DATA-IN-1xx) -> Core (CORE-2xx) -> Output.Recommend
        // (DATA-OUT-300) -> Output visualization/metrics/bundler (OUT-4xx),
        // with the optional LlmStep (CORE-250) gated behind config between
        // Recommend and Viz. Nothing to run yet, so this is a clean no-op.
        return 0;
    }
}
