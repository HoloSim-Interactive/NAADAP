namespace Naadap.LlmStep.Tests;

/// <summary>
/// Test double that records every call it actually receives and returns a
/// canned <see cref="ModelCallResult"/>. Used to assert both "a call was
/// made with these exact arguments" and — just as importantly for
/// NFR-510/CORE-250 — "no call was made at all" for the fail-closed paths.
/// </summary>
public sealed class FakeModelClient : IModelClient
{
    public List<(string Endpoint, string Model, string Prompt)> Calls { get; } = [];

    public ModelCallResult NextResult { get; set; } = new("summary text", PromptTokens: 10, CompletionTokens: 5);

    public Task<ModelCallResult> CompleteAsync(
        string endpoint,
        string model,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        Calls.Add((endpoint, model, prompt));
        return Task.FromResult(NextResult);
    }
}
