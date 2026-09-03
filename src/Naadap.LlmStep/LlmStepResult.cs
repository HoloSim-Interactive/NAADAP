namespace Naadap.LlmStep;

/// <summary>
/// Outcome of one <see cref="LlmSummarizationStep"/> run — always returned,
/// never thrown, so <c>Naadap.Cli</c> can log it and continue to
/// OUT-440 bundling regardless of whether the LLM step actually ran (the
/// pipeline's "never abort the run" spirit, applied here to an optional
/// step rather than a document-ingestion failure). Written to the run's
/// output directory by <c>LlmRunLogWriter</c> so TP-250's token-usage and
/// network-call audit can be read directly off disk.
/// </summary>
/// <param name="Enabled">Whether the config-flag gate was on for this run.</param>
/// <param name="Skipped">
/// <see langword="true"/> if the step did not produce a summary — either
/// because it was disabled, or because it was enabled but a precondition
/// (endpoint configured, endpoint allowlisted, within token budget) was not
/// met. <see cref="SkipReason"/> explains which.
/// </param>
/// <param name="SkipReason">Human-readable reason, set whenever <see cref="Skipped"/> is <see langword="true"/>.</param>
/// <param name="SummaryText">The model's generated summary, if the step actually ran.</param>
/// <param name="TotalTokensUsed">Actual tokens spent (0 if skipped) — SN-2/CORE-250's headline number.</param>
/// <param name="MaxTokenBudget">The budget this run was checked against.</param>
/// <param name="NetworkCalls">Every outbound call attempt considered during this step — TP-250's audit trail.</param>
public sealed record LlmStepResult(
    bool Enabled,
    bool Skipped,
    string? SkipReason,
    string? SummaryText,
    int TotalTokensUsed,
    int MaxTokenBudget,
    IReadOnlyList<NetworkCallRecord> NetworkCalls);
