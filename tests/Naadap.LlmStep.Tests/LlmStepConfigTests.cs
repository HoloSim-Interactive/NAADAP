namespace Naadap.LlmStep.Tests;

/// <summary>
/// <see cref="LlmStepConfig.FromEnvironment"/> reads process-wide
/// environment variables, so this class is marked non-parallel with itself
/// (xUnit already serializes tests within one class by default) and always
/// restores every variable it touches in a <c>finally</c>, so it cannot
/// leak state into any other test class running in the same process.
/// </summary>
public class LlmStepConfigTests
{
    private const string enabledVariable = "NAADAP_LLM_ENABLED";
    private const string endpointVariable = "NAADAP_LLM_ENDPOINT";
    private const string modelVariable = "NAADAP_LLM_MODEL";
    private const string apiKeyVariable = "NAADAP_LLM_API_KEY";
    private const string allowedEndpointsVariable = "NAADAP_LLM_ALLOWED_ENDPOINTS";

    [Fact]
    public void FromEnvironment_NoCliFlagAndNoEnvironmentVariables_IsDisabledWithDefaults()
    {
        WithCleanEnvironment(() =>
        {
            var config = LlmStepConfig.FromEnvironment(cliFlagEnabled: false);

            Assert.False(config.Enabled);
            Assert.Null(config.Endpoint);
            Assert.Empty(config.AllowedEndpoints);
            Assert.Equal(LlmStepConfig.Sn2TokenBudgetCeiling, config.MaxTokenBudget);
        });
    }

    [Fact]
    public void FromEnvironment_CliFlagAlone_IsEnabled()
    {
        WithCleanEnvironment(() =>
        {
            var config = LlmStepConfig.FromEnvironment(cliFlagEnabled: true);
            Assert.True(config.Enabled);
        });
    }

    [Fact]
    public void FromEnvironment_EnvironmentVariableAlone_IsEnabledAndParsesAllowlist()
    {
        WithCleanEnvironment(() =>
        {
            Environment.SetEnvironmentVariable(enabledVariable, "true");
            Environment.SetEnvironmentVariable(endpointVariable, "https://approved.example.mil/v1");
            Environment.SetEnvironmentVariable(modelVariable, "model-1");
            Environment.SetEnvironmentVariable(apiKeyVariable, "secret");
            Environment.SetEnvironmentVariable(
                allowedEndpointsVariable,
                "https://approved.example.mil/v1, https://approved2.example.mil/v1");

            var config = LlmStepConfig.FromEnvironment(cliFlagEnabled: false);

            Assert.True(config.Enabled);
            Assert.Equal("https://approved.example.mil/v1", config.Endpoint);
            Assert.Equal("model-1", config.Model);
            Assert.Equal("secret", config.ApiKey);
            Assert.Equal(
                new[] { "https://approved.example.mil/v1", "https://approved2.example.mil/v1" },
                config.AllowedEndpoints);
        });
    }

    private static void WithCleanEnvironment(Action test)
    {
        var variables = new[] { enabledVariable, endpointVariable, modelVariable, apiKeyVariable, allowedEndpointsVariable };
        var originalValues = variables.ToDictionary(v => v, Environment.GetEnvironmentVariable);

        try
        {
            foreach (var variable in variables)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }

            test();
        }
        finally
        {
            foreach (var (variable, value) in originalValues)
            {
                Environment.SetEnvironmentVariable(variable, value);
            }
        }
    }
}
