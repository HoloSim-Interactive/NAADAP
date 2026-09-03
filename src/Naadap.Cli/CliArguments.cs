namespace Naadap.Cli;

/// <summary>
/// Parsed command-line invocation for the UI-001 entrypoint:
/// <c>naadap --input &lt;dir&gt; --output &lt;dir&gt; [--enable-llm-step]</c>.
/// </summary>
/// <param name="EnableLlmStep">
/// CORE-250's explicit config-flag gate. <see langword="false"/> unless
/// <c>--enable-llm-step</c> is passed — the <c>NAADAP_LLM_ENABLED</c>
/// environment variable is an equally-explicit alternative, read directly
/// by <c>Naadap.LlmStep.LlmStepConfig.FromEnvironment</c> rather than here.
/// </param>
public sealed record CliArguments(string InputDirectory, string OutputDirectory, bool EnableLlmStep = false);

/// <summary>
/// Parses the CLI's two required arguments. Kept free of <see cref="Console"/>
/// I/O so it can be unit tested directly (see Naadap.Cli.Tests).
/// </summary>
public static class CliArgumentParser
{
    public static bool TryParse(string[] args, out CliArguments? arguments, out string? error)
    {
        string? input = null;
        string? output = null;
        var enableLlmStep = false;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--input":
                    if (!TryTakeValue(args, ref i, out input))
                    {
                        arguments = null;
                        error = "Missing value for --input.";
                        return false;
                    }

                    break;

                case "--output":
                    if (!TryTakeValue(args, ref i, out output))
                    {
                        arguments = null;
                        error = "Missing value for --output.";
                        return false;
                    }

                    break;

                case "--enable-llm-step":
                    // CORE-250's explicit config-flag gate -- a bare switch,
                    // no value token follows it.
                    enableLlmStep = true;
                    break;

                default:
                    arguments = null;
                    error = $"Unrecognized argument '{args[i]}'.";
                    return false;
            }
        }

        if (input is null)
        {
            arguments = null;
            error = "Missing required --input <dir> argument.";
            return false;
        }

        if (output is null)
        {
            arguments = null;
            error = "Missing required --output <dir> argument.";
            return false;
        }

        arguments = new CliArguments(input, output, enableLlmStep);
        error = null;
        return true;
    }

    private static bool TryTakeValue(string[] args, ref int index, out string? value)
    {
        if (index + 1 >= args.Length)
        {
            value = null;
            return false;
        }

        index++;
        value = args[index];
        return true;
    }
}
