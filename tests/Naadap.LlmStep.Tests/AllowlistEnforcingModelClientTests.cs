namespace Naadap.LlmStep.Tests;

/// <summary>
/// Defense-in-depth layer directly under <c>LlmSummarizationStep</c> (see
/// its remarks) — covers both the allowed pass-through path and the refusal
/// path in isolation from the orchestration logic above it.
/// </summary>
public class AllowlistEnforcingModelClientTests
{
    [Fact]
    public async Task CompleteAsync_AllowlistedEndpoint_DelegatesAndRecordsAllowedCall()
    {
        var inner = new FakeModelClient();
        var auditLog = new NetworkCallAuditLog();
        var client = new AllowlistEnforcingModelClient(inner, ["https://approved.example.mil/v1"], auditLog);

        var result = await client.CompleteAsync("https://approved.example.mil/v1", "model-1", "prompt");

        Assert.Equal("summary text", result.Content);
        Assert.Single(inner.Calls);

        var record = Assert.Single(auditLog.Calls);
        Assert.True(record.Allowed);
        Assert.Equal("https://approved.example.mil/v1", record.Endpoint);
    }

    [Fact]
    public async Task CompleteAsync_DisallowedEndpoint_ThrowsAndNeverDelegates()
    {
        var inner = new FakeModelClient();
        var auditLog = new NetworkCallAuditLog();
        var client = new AllowlistEnforcingModelClient(inner, ["https://approved.example.mil/v1"], auditLog);

        await Assert.ThrowsAsync<LlmAllowlistViolationException>(
            () => client.CompleteAsync("https://not-approved.example.com/v1", "model-1", "prompt"));

        Assert.Empty(inner.Calls);

        var record = Assert.Single(auditLog.Calls);
        Assert.False(record.Allowed);
        Assert.Equal("https://not-approved.example.com/v1", record.Endpoint);
    }
}
