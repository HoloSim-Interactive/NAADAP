using Naadap.Core;

namespace Naadap.LlmStep;

/// <summary>
/// CORE-250: the optional summarization/interpretation step, invoked (per
/// <c>docs/SDD.md</c>'s activity diagram) after DATA-OUT-300's ranking and
/// before OUT-400/410's visualizations, gated behind
/// <see cref="LlmStepConfig.Enabled"/>. Turns the run's ranked
/// <see cref="CandidateVehicle"/> list into a short natural-language
/// interpretation for a human reviewer — the "interpretation" leg of
/// CORE-250's "summarization/interpretation/visualization step only" scope.
/// Every precondition below fails closed (skip, never a network attempt),
/// so a misconfigured or disallowed run degrades to "no summary produced"
/// rather than either crashing the pipeline or reaching the network
/// anyway.
/// </summary>
public static class LlmSummarizationStep
{
    /// <summary>
    /// Conservative token estimate (characters / 4, the common rule-of-thumb
    /// documented by most vendor tokenizers) used to reserve budget
    /// <em>before</em> a call is placed — so a prompt that would blow SN-2's
    /// budget is refused pre-flight rather than discovered only after the
    /// network round-trip. The endpoint's actual reported usage (from
    /// <see cref="ModelCallResult"/>) is always what gets recorded as
    /// <see cref="LlmStepResult.TotalTokensUsed"/>; this estimate only gates
    /// whether the call is attempted at all.
    /// </summary>
    private const double charsPerTokenEstimate = 4.0;

    public static async Task<LlmStepResult> RunAsync(
        LlmStepConfig config,
        IReadOnlyList<CandidateVehicle> candidates,
        IModelClient rawClient,
        CancellationToken cancellationToken = default)
    {
        var auditLog = new NetworkCallAuditLog();

        if (!config.Enabled)
        {
            return Skip(
                config,
                enabled: false,
                "LLM step disabled (default). Enable with --enable-llm-step or NAADAP_LLM_ENABLED=true.",
                auditLog);
        }

        if (string.IsNullOrWhiteSpace(config.Endpoint))
        {
            return Skip(config, enabled: true, "LLM step enabled but NAADAP_LLM_ENDPOINT is not configured.", auditLog);
        }

        if (config.AllowedEndpoints.Count == 0 ||
            !config.AllowedEndpoints.Contains(config.Endpoint, StringComparer.OrdinalIgnoreCase))
        {
            // NFR-510 fail-closed: never even construct a client for a
            // target outside the configured allowlist, so this case makes
            // zero network attempts and records no call at all.
            return Skip(
                config,
                enabled: true,
                $"Configured endpoint '{config.Endpoint}' is not on the configured USN-approved " +
                "allowlist (NAADAP_LLM_ALLOWED_ENDPOINTS); LLM step skipped, no network call attempted.",
                auditLog);
        }

        var prompt = BuildPrompt(candidates);
        var estimatedTokens = EstimateTokens(prompt);
        var budget = new TokenBudget(config.MaxTokenBudget);

        if (!budget.TryReserve(estimatedTokens))
        {
            return Skip(
                config,
                enabled: true,
                $"Estimated prompt token count ({estimatedTokens}) would exceed the " +
                $"{config.MaxTokenBudget}-token run budget (SN-2/CORE-250); LLM step skipped, no network " +
                "call attempted.",
                auditLog);
        }

        var client = new AllowlistEnforcingModelClient(rawClient, config.AllowedEndpoints, auditLog);

        try
        {
            var result = await client
                .CompleteAsync(config.Endpoint, config.Model ?? "default", prompt, cancellationToken)
                .ConfigureAwait(false);

            return new LlmStepResult(
                Enabled: true,
                Skipped: false,
                SkipReason: null,
                SummaryText: result.Content,
                TotalTokensUsed: result.TotalTokens,
                MaxTokenBudget: config.MaxTokenBudget,
                NetworkCalls: auditLog.Calls);
        }
        catch (LlmAllowlistViolationException ex)
        {
            // Defense-in-depth path only (see AllowlistEnforcingModelClient
            // remarks) -- the check above should already have skipped
            // before reaching here.
            return Skip(config, enabled: true, ex.Message, auditLog);
        }
    }

    private static LlmStepResult Skip(LlmStepConfig config, bool enabled, string reason, NetworkCallAuditLog auditLog) =>
        new(
            Enabled: enabled,
            Skipped: true,
            SkipReason: reason,
            SummaryText: null,
            TotalTokensUsed: 0,
            MaxTokenBudget: config.MaxTokenBudget,
            NetworkCalls: auditLog.Calls);

    private static int EstimateTokens(string text) => (int)Math.Ceiling(text.Length / charsPerTokenEstimate);

    /// <summary>
    /// Builds a deterministic prompt from the run's ranked candidates —
    /// same input, same prompt, every time (consistent with CORE-210's
    /// determinism spirit applied to this optional step).
    /// </summary>
    private static string BuildPrompt(IReadOnlyList<CandidateVehicle> candidates)
    {
        if (candidates.Count == 0)
        {
            return "No candidate contract vehicles were identified for this run. " +
                   "Write one sentence noting that no consolidation opportunity was found.";
        }

        var lines = candidates
            .Take(5)
            .Select(c =>
                $"- {c.VehicleId}: score {c.Score:F2}, evidence from {c.ContributingDocuments.Count} document(s).");

        return "Summarize, in plain English for a NAVAIR acquisition reviewer, why the following " +
               "candidate contract vehicles were recommended for consolidation, based only on the " +
               "evidence given (do not invent additional facts):\n" +
               string.Join('\n', lines);
    }
}
