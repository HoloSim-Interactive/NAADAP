namespace Naadap.LlmStep;

/// <summary>
/// Extension point for CORE-250's outbound model/microservice call — the
/// LlmStep analog of <c>Naadap.Ingestion.IDocumentParser</c> and
/// <c>Naadap.Core.IClusteringComponent</c> (DELIV-940's documented extension
/// interface pattern). A caller never invokes this directly with a raw
/// <see cref="Endpoint"/>-less call; every production call path goes through
/// <see cref="AllowlistEnforcingModelClient"/>, which is what actually
/// enforces NFR-510/CORE-250's allowlist before an implementation of this
/// interface is ever given a chance to open a connection.
/// </summary>
public interface IModelClient
{
    /// <summary>
    /// Requests a completion from the model/microservice at
    /// <paramref name="endpoint"/>. Implementations must not call any
    /// target other than <paramref name="endpoint"/> — allowlist
    /// enforcement happens one layer up
    /// (<see cref="AllowlistEnforcingModelClient"/>), not here.
    /// </summary>
    Task<ModelCallResult> CompleteAsync(
        string endpoint,
        string model,
        string prompt,
        CancellationToken cancellationToken = default);
}
