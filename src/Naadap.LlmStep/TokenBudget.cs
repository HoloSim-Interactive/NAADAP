namespace Naadap.LlmStep;

/// <summary>
/// Tracks cumulative token spend against SN-2/CORE-250's per-run budget
/// (<see cref="LlmStepConfig.MaxTokenBudget"/>, ceilinged at
/// <see cref="LlmStepConfig.Sn2TokenBudgetCeiling"/>). Checked
/// <em>before</em> a call is issued (using a conservative prompt-size
/// estimate — see <c>LlmSummarizationStep</c>) so a call that would exceed
/// the budget is never attempted, not just flagged after the fact.
/// </summary>
public sealed class TokenBudget(int maxTokens)
{
    /// <summary>The run's total token ceiling.</summary>
    public int MaxTokens { get; } = maxTokens;

    /// <summary>Tokens reserved so far.</summary>
    public int Used { get; private set; }

    /// <summary>
    /// Attempts to reserve <paramref name="tokens"/> more against the
    /// budget. Returns <see langword="false"/> (reserving nothing) if doing
    /// so would exceed <see cref="MaxTokens"/>.
    /// </summary>
    public bool TryReserve(int tokens)
    {
        if (tokens < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tokens), tokens, "Token count cannot be negative.");
        }

        if (Used + tokens > MaxTokens)
        {
            return false;
        }

        Used += tokens;
        return true;
    }
}
