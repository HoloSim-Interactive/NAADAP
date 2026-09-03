namespace Naadap.LlmStep;

/// <summary>
/// One completed call to an <see cref="IModelClient"/> — the generated text
/// plus the token accounting SN-2/CORE-250's &lt;50,000-token budget is
/// measured against.
/// </summary>
/// <param name="Content">The model's response text.</param>
/// <param name="PromptTokens">Tokens consumed by the request/prompt, as reported by the endpoint.</param>
/// <param name="CompletionTokens">Tokens consumed by the generated response, as reported by the endpoint.</param>
public sealed record ModelCallResult(string Content, int PromptTokens, int CompletionTokens)
{
    /// <summary>Total tokens this call spent against the run's <see cref="LlmStepConfig.MaxTokenBudget"/>.</summary>
    public int TotalTokens => PromptTokens + CompletionTokens;
}
