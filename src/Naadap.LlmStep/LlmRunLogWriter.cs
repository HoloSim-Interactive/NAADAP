using System.Text.Json;
using System.Text.Json.Serialization;

namespace Naadap.LlmStep;

/// <summary>
/// Writes CORE-250's per-run audit artifacts directly into the run's output
/// directory, alongside (but independent of) OUT-440's <c>manifest.json</c>
/// bundle — TP-250 reads total token usage and the network-call audit off
/// <see cref="RunLogFileName"/> without needing to change
/// <c>Naadap.Core.RunManifest</c>'s already-verified schema (see this
/// role's memory: keep already-verified artifacts' shape stable when an
/// optional, independent feature can just add its own file instead).
/// </summary>
public static class LlmRunLogWriter
{
    /// <summary>Token-usage and network-call audit log for TP-250.</summary>
    public const string RunLogFileName = "llm-run-log.json";

    /// <summary>The model's generated summary text, when the step actually produced one.</summary>
    public const string SummaryFileName = "llm-summary.txt";

    private static readonly JsonSerializerOptions runLogJsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Writes <see cref="RunLogFileName"/> unconditionally (even when the
    /// step was skipped/disabled, so the audit trail always shows why), and
    /// <see cref="SummaryFileName"/> only when a summary was actually
    /// produced.
    /// </summary>
    public static void Write(string outputDirectory, LlmStepResult result)
    {
        var logPayload = new RunLogPayload(
            result.Enabled,
            result.Skipped,
            result.SkipReason,
            result.TotalTokensUsed,
            result.MaxTokenBudget,
            result.NetworkCalls);

        File.WriteAllText(
            Path.Combine(outputDirectory, RunLogFileName),
            JsonSerializer.Serialize(logPayload, runLogJsonOptions));

        if (!string.IsNullOrWhiteSpace(result.SummaryText))
        {
            File.WriteAllText(Path.Combine(outputDirectory, SummaryFileName), result.SummaryText);
        }
    }

    private sealed record RunLogPayload(
        bool Enabled,
        bool Skipped,
        string? SkipReason,
        int TotalTokensUsed,
        int MaxTokenBudget,
        IReadOnlyList<NetworkCallRecord> NetworkCalls);
}
