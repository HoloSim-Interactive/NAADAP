namespace Naadap.Cli.Tests;

public class CliArgumentParserTests
{
    [Fact]
    public void TryParse_WithBothRequiredArguments_Succeeds()
    {
        var ok = CliArgumentParser.TryParse(
            ["--input", "/tmp/in", "--output", "/tmp/out"],
            out var arguments,
            out var error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.Equal("/tmp/in", arguments!.InputDirectory);
        Assert.Equal("/tmp/out", arguments.OutputDirectory);
    }

    [Fact]
    public void TryParse_WithoutEnableLlmStepFlag_DefaultsToDisabled()
    {
        var ok = CliArgumentParser.TryParse(
            ["--input", "/tmp/in", "--output", "/tmp/out"],
            out var arguments,
            out _);

        Assert.True(ok);
        Assert.False(arguments!.EnableLlmStep);
    }

    [Fact]
    public void TryParse_WithEnableLlmStepFlag_SetsFlagTrue()
    {
        // CORE-250's explicit config-flag gate: a bare switch, order-independent
        // relative to --input/--output.
        var ok = CliArgumentParser.TryParse(
            ["--input", "/tmp/in", "--enable-llm-step", "--output", "/tmp/out"],
            out var arguments,
            out var error);

        Assert.True(ok);
        Assert.Null(error);
        Assert.True(arguments!.EnableLlmStep);
    }

    [Fact]
    public void TryParse_MissingInput_Fails()
    {
        var ok = CliArgumentParser.TryParse(
            ["--output", "/tmp/out"],
            out var arguments,
            out var error);

        Assert.False(ok);
        Assert.Null(arguments);
        Assert.Contains("--input", error);
    }

    [Fact]
    public void TryParse_MissingOutput_Fails()
    {
        var ok = CliArgumentParser.TryParse(
            ["--input", "/tmp/in"],
            out var arguments,
            out var error);

        Assert.False(ok);
        Assert.Null(arguments);
        Assert.Contains("--output", error);
    }

    [Fact]
    public void TryParse_UnrecognizedArgument_Fails()
    {
        var ok = CliArgumentParser.TryParse(
            ["--bogus", "value"],
            out var arguments,
            out var error);

        Assert.False(ok);
        Assert.Null(arguments);
        Assert.Contains("--bogus", error);
    }

    [Fact]
    public void TryParse_NoArguments_EmptyPipelineStillParsesAsMissing()
    {
        var ok = CliArgumentParser.TryParse([], out var arguments, out var error);

        Assert.False(ok);
        Assert.Null(arguments);
        Assert.NotNull(error);
    }
}
