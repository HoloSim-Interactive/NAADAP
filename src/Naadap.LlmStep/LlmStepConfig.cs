namespace Naadap.LlmStep;

/// <summary>
/// CORE-250's runtime configuration: whether the optional LLM summarization/
/// interpretation step runs at all, and, if so, which endpoint/model it may
/// call. Deliberately config-driven rather than hardcoding a specific
/// vendor's endpoint — TP-250 audits that the outbound call target matches
/// "the configured allowlist", meaning the actual USN-approved model list is
/// something this deployment's operator supplies at deploy time (they know
/// the real IL4-accredited list; this codebase does not and must not guess
/// at one), not a constant baked into source.
/// </summary>
/// <param name="Enabled">
/// The explicit config-flag gate (SN-3/CORE-250): <see langword="false"/> by
/// default. Set via <c>--enable-llm-step</c> on the CLI or the
/// <c>NAADAP_LLM_ENABLED=true</c> environment variable (either counts as
/// "explicit"; see <see cref="FromEnvironment"/>). When
/// <see langword="false"/>, <c>Naadap.Cli</c> never constructs a model
/// client or attempts a network call at all — this is what makes NFR-510's
/// "zero outbound network connections" default path a structural guarantee
/// rather than a runtime check.
/// </param>
/// <param name="Endpoint">
/// The single model/microservice endpoint this run is configured to call.
/// Must appear in <paramref name="AllowedEndpoints"/> or the LLM step is
/// skipped entirely (see <c>LlmSummarizationStep</c>).
/// </param>
/// <param name="Model">Model identifier passed to the endpoint, if the endpoint's API distinguishes multiple models.</param>
/// <param name="ApiKey">Credential for <paramref name="Endpoint"/>, if required. Never logged or written to any run artifact.</param>
/// <param name="AllowedEndpoints">
/// The USN-approved allowlist for this deployment (NFR-510/CORE-250) —
/// every outbound call target this run may legally reach. Supplied by the
/// operator, not this codebase, since only the operator's IL4 deployment
/// knows which models/microservices are actually approved.
/// </param>
/// <param name="MaxTokenBudget">
/// SN-2/CORE-250's hard ceiling: a single run's total LLM token spend must
/// stay under this. Fixed at <see cref="Sn2TokenBudgetCeiling"/> — not
/// operator-raisable, since raising it would silently violate the
/// requirement it exists to enforce.
/// </param>
public sealed record LlmStepConfig(
    bool Enabled,
    string? Endpoint,
    string? Model,
    string? ApiKey,
    IReadOnlyList<string> AllowedEndpoints,
    int MaxTokenBudget)
{
    /// <summary>SN-2/CORE-250: "a single run's token spend is &lt;50,000 tokens".</summary>
    public const int Sn2TokenBudgetCeiling = 50_000;

    private const string enabledVariable = "NAADAP_LLM_ENABLED";
    private const string endpointVariable = "NAADAP_LLM_ENDPOINT";
    private const string modelVariable = "NAADAP_LLM_MODEL";
    private const string apiKeyVariable = "NAADAP_LLM_API_KEY";
    private const string allowedEndpointsVariable = "NAADAP_LLM_ALLOWED_ENDPOINTS";

    /// <summary>
    /// Builds the run's <see cref="LlmStepConfig"/> from environment
    /// variables (the deployment-config surface a Docker/IL4 operator
    /// actually uses) plus the CLI's own <c>--enable-llm-step</c> flag.
    /// </summary>
    /// <param name="cliFlagEnabled">
    /// Whether <c>--enable-llm-step</c> was passed on the command line. Read
    /// alongside <c>NAADAP_LLM_ENABLED</c> — either one being set is enough
    /// to count as the "explicit config flag" CORE-250 requires; this run is
    /// enabled if <em>either</em> is set (not both required).
    /// </param>
    public static LlmStepConfig FromEnvironment(bool cliFlagEnabled)
    {
        var enabled = cliFlagEnabled || IsTruthy(Environment.GetEnvironmentVariable(enabledVariable));

        var allowedEndpoints = (Environment.GetEnvironmentVariable(allowedEndpointsVariable) ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        return new LlmStepConfig(
            enabled,
            Environment.GetEnvironmentVariable(endpointVariable),
            Environment.GetEnvironmentVariable(modelVariable),
            Environment.GetEnvironmentVariable(apiKeyVariable),
            allowedEndpoints,
            Sn2TokenBudgetCeiling);
    }

    private static bool IsTruthy(string? value) => string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
}
