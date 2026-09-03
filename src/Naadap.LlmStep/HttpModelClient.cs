using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Naadap.LlmStep;

/// <summary>
/// Default production <see cref="IModelClient"/>: a plain JSON-over-HTTP
/// POST to whatever endpoint it is given, using <see cref="HttpClient"/>
/// from the net9.0 BCL (no NuGet dependency — see
/// <c>Naadap.LlmStep.csproj</c>). Only ever reached through
/// <see cref="AllowlistEnforcingModelClient"/> in production wiring (see
/// <c>Naadap.Cli.Program</c>), so this class itself does no allowlist
/// checking.
/// </summary>
/// <remarks>
/// The request/response shape here (<see cref="ModelRequestPayload"/>/
/// <see cref="ModelResponsePayload"/>) is a minimal, generic
/// completion-style contract, not a specific vendor's API — the concrete
/// USN-approved model/microservice this calls is a deployment-time
/// decision (<see cref="LlmStepConfig"/>'s <c>Endpoint</c>/<c>Model</c>),
/// not something this codebase can know in advance. If the actual
/// IL4-approved endpoint's API differs, this is the one class that needs to
/// change — <see cref="IModelClient"/> is the seam that isolates that
/// integration detail from the rest of CORE-250.
/// </remarks>
public sealed class HttpModelClient(HttpClient? httpClient = null, string? apiKey = null) : IModelClient
{
    private readonly HttpClient httpClient = httpClient ?? new HttpClient();

    public async Task<ModelCallResult> CompleteAsync(
        string endpoint,
        string model,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new ModelRequestPayload(model, prompt)),
        };

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content
            .ReadFromJsonAsync<ModelResponsePayload>(cancellationToken)
            .ConfigureAwait(false);

        if (payload is null)
        {
            throw new InvalidOperationException($"'{endpoint}' returned an empty/unparseable response.");
        }

        return new ModelCallResult(payload.Content ?? string.Empty, payload.PromptTokens, payload.CompletionTokens);
    }

    private sealed record ModelRequestPayload(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("prompt")] string Prompt);

    private sealed record ModelResponsePayload(
        [property: JsonPropertyName("content")] string? Content,
        [property: JsonPropertyName("promptTokens")] int PromptTokens,
        [property: JsonPropertyName("completionTokens")] int CompletionTokens);
}
