using Naadap.Core;

namespace Naadap.LlmStep.Tests;

/// <summary>
/// TP-250: covers CORE-250's config-flag gate, the allowlist fail-closed
/// path, the token-budget fail-closed path, and the success path's token
/// accounting/audit trail — every case the real network-call audit and
/// run-log JSON in <c>LlmRunLogWriter</c> are built from.
/// </summary>
public class LlmSummarizationStepTests
{
    private static readonly IReadOnlyList<CandidateVehicle> sampleCandidates =
    [
        new CandidateVehicle("CLUSTER-0001-ALPHA", 0.9, ["doc-a.txt", "doc-b.txt"]),
    ];

    [Fact]
    public async Task RunAsync_Disabled_SkipsAndNeverCallsClient()
    {
        var config = new LlmStepConfig(
            Enabled: false,
            Endpoint: "https://approved.example.mil/v1/complete",
            Model: "model-1",
            ApiKey: null,
            AllowedEndpoints: ["https://approved.example.mil/v1/complete"],
            MaxTokenBudget: LlmStepConfig.Sn2TokenBudgetCeiling);
        var client = new FakeModelClient();

        var result = await LlmSummarizationStep.RunAsync(config, sampleCandidates, client);

        Assert.False(result.Enabled);
        Assert.True(result.Skipped);
        Assert.Equal(0, result.TotalTokensUsed);
        Assert.Empty(result.NetworkCalls);
        Assert.Empty(client.Calls);
    }

    [Fact]
    public async Task RunAsync_EnabledWithNoEndpointConfigured_SkipsAndNeverCallsClient()
    {
        var config = new LlmStepConfig(
            Enabled: true,
            Endpoint: null,
            Model: null,
            ApiKey: null,
            AllowedEndpoints: [],
            MaxTokenBudget: LlmStepConfig.Sn2TokenBudgetCeiling);
        var client = new FakeModelClient();

        var result = await LlmSummarizationStep.RunAsync(config, sampleCandidates, client);

        Assert.True(result.Enabled);
        Assert.True(result.Skipped);
        Assert.Contains("NAADAP_LLM_ENDPOINT", result.SkipReason);
        Assert.Empty(client.Calls);
    }

    [Fact]
    public async Task RunAsync_EndpointNotOnAllowlist_SkipsAndNeverCallsClient()
    {
        // NFR-510/CORE-250 fail-closed: an endpoint outside the configured
        // allowlist must never reach the network, not even once.
        var config = new LlmStepConfig(
            Enabled: true,
            Endpoint: "https://not-approved.example.com/v1/complete",
            Model: "model-1",
            ApiKey: null,
            AllowedEndpoints: ["https://approved.example.mil/v1/complete"],
            MaxTokenBudget: LlmStepConfig.Sn2TokenBudgetCeiling);
        var client = new FakeModelClient();

        var result = await LlmSummarizationStep.RunAsync(config, sampleCandidates, client);

        Assert.True(result.Skipped);
        Assert.Contains("not on the configured", result.SkipReason);
        Assert.Empty(client.Calls);
        Assert.Empty(result.NetworkCalls);
    }

    [Fact]
    public async Task RunAsync_PromptWouldExceedTokenBudget_SkipsAndNeverCallsClient()
    {
        var config = new LlmStepConfig(
            Enabled: true,
            Endpoint: "https://approved.example.mil/v1/complete",
            Model: "model-1",
            ApiKey: null,
            AllowedEndpoints: ["https://approved.example.mil/v1/complete"],
            MaxTokenBudget: 1); // any real prompt exceeds a 1-token budget
        var client = new FakeModelClient();

        var result = await LlmSummarizationStep.RunAsync(config, sampleCandidates, client);

        Assert.True(result.Skipped);
        Assert.Contains("token run budget", result.SkipReason);
        Assert.Empty(client.Calls);
    }

    [Fact]
    public async Task RunAsync_AllowlistedEndpointWithinBudget_CallsClientAndRecordsUsage()
    {
        var config = new LlmStepConfig(
            Enabled: true,
            Endpoint: "https://approved.example.mil/v1/complete",
            Model: "model-1",
            ApiKey: null,
            AllowedEndpoints: ["https://approved.example.mil/v1/complete"],
            MaxTokenBudget: LlmStepConfig.Sn2TokenBudgetCeiling);
        var client = new FakeModelClient { NextResult = new ModelCallResult("a summary", 100, 40) };

        var result = await LlmSummarizationStep.RunAsync(config, sampleCandidates, client);

        Assert.False(result.Skipped);
        Assert.Equal("a summary", result.SummaryText);
        Assert.Equal(140, result.TotalTokensUsed);
        Assert.True(result.TotalTokensUsed < LlmStepConfig.Sn2TokenBudgetCeiling);

        var call = Assert.Single(client.Calls);
        Assert.Equal("https://approved.example.mil/v1/complete", call.Endpoint);

        var auditedCall = Assert.Single(result.NetworkCalls);
        Assert.Equal("https://approved.example.mil/v1/complete", auditedCall.Endpoint);
        Assert.True(auditedCall.Allowed);
    }

    [Fact]
    public async Task RunAsync_NoCandidates_StillProducesADeterministicPromptAndDoesNotThrow()
    {
        var config = new LlmStepConfig(
            Enabled: true,
            Endpoint: "https://approved.example.mil/v1/complete",
            Model: "model-1",
            ApiKey: null,
            AllowedEndpoints: ["https://approved.example.mil/v1/complete"],
            MaxTokenBudget: LlmStepConfig.Sn2TokenBudgetCeiling);
        var client = new FakeModelClient();

        var result = await LlmSummarizationStep.RunAsync(config, [], client);

        Assert.False(result.Skipped);
        Assert.Single(client.Calls);
    }
}
