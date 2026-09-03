namespace Naadap.LlmStep;

/// <summary>
/// Decorates an <see cref="IModelClient"/> with NFR-510/CORE-250's
/// allowlist gate: every call is checked against the configured allowlist
/// and recorded to a <see cref="NetworkCallAuditLog"/> before the inner
/// client is ever invoked. A disallowed target is refused outright — the
/// inner client's <see cref="IModelClient.CompleteAsync"/> is never called
/// for it, so no network connection is attempted, matching NFR-510's "no
/// outbound calls to industry-hosted models" for anything not on the list.
/// </summary>
public sealed class AllowlistEnforcingModelClient(
    IModelClient inner,
    IReadOnlyList<string> allowedEndpoints,
    NetworkCallAuditLog auditLog) : IModelClient
{
    public Task<ModelCallResult> CompleteAsync(
        string endpoint,
        string model,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        var allowed = allowedEndpoints.Contains(endpoint, StringComparer.OrdinalIgnoreCase);
        auditLog.Record(endpoint, allowed);

        if (!allowed)
        {
            throw new LlmAllowlistViolationException(endpoint, allowedEndpoints);
        }

        return inner.CompleteAsync(endpoint, model, prompt, cancellationToken);
    }
}
