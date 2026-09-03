namespace Naadap.LlmStep;

/// <summary>
/// Thrown if a call somehow reaches <see cref="AllowlistEnforcingModelClient"/>
/// for a target outside the configured allowlist. In normal operation this
/// should never surface: <c>LlmSummarizationStep</c> checks the allowlist
/// itself before ever constructing a client, so no network attempt is made
/// in the first place. This exception exists as defense-in-depth at the
/// client layer, not as the primary enforcement mechanism.
/// </summary>
public sealed class LlmAllowlistViolationException(string endpoint, IReadOnlyList<string> allowedEndpoints)
    : Exception(
        $"Refused to call '{endpoint}': not on the configured USN-approved allowlist " +
        $"({string.Join(", ", allowedEndpoints)}) (NFR-510/CORE-250).")
{
    /// <summary>The call target that was rejected.</summary>
    public string Endpoint { get; } = endpoint;

    /// <summary>The allowlist it was checked against.</summary>
    public IReadOnlyList<string> AllowedEndpoints { get; } = allowedEndpoints;
}
