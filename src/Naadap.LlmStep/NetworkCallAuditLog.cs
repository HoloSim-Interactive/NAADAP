namespace Naadap.LlmStep;

/// <summary>
/// One outbound network-call attempt this run made or considered making —
/// TP-250's "network-call audit shows every outbound connection matches the
/// configured allowlist, none other" is read directly off a list of these.
/// </summary>
/// <param name="Endpoint">The call target that was checked against the allowlist.</param>
/// <param name="TimestampUtc">When the attempt was evaluated.</param>
/// <param name="Allowed">
/// Whether <paramref name="Endpoint"/> matched the configured allowlist. A
/// call is only actually placed on the wire when this is
/// <see langword="true"/> — see <see cref="AllowlistEnforcingModelClient"/>.
/// </param>
public sealed record NetworkCallRecord(string Endpoint, DateTimeOffset TimestampUtc, bool Allowed);

/// <summary>
/// Accumulates every <see cref="NetworkCallRecord"/> for one run, in call
/// order. Written into the run log (see <c>LlmRunLogWriter</c>) so a
/// reviewer can audit outbound connections without packet-capturing the
/// container.
/// </summary>
public sealed class NetworkCallAuditLog
{
    private readonly List<NetworkCallRecord> calls = [];

    /// <summary>Every call attempt recorded so far, in the order they were evaluated.</summary>
    public IReadOnlyList<NetworkCallRecord> Calls => calls;

    public void Record(string endpoint, bool allowed) =>
        calls.Add(new NetworkCallRecord(endpoint, DateTimeOffset.UtcNow, allowed));
}
