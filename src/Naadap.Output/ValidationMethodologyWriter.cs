using System.Reflection;

namespace Naadap.Output;

/// <summary>
/// OUT-430: copies the validation-methodology document (authored once in
/// docs/VALIDATION_METHODOLOGY.md, embedded into this assembly at build
/// time — see Naadap.Output.csproj) into every run's output bundle, so it
/// "accompanies every run's output" per the requirement text, without this
/// assembly depending on the repo's docs/ directory existing at runtime.
/// </summary>
public static class ValidationMethodologyWriter
{
    public const string FileName = "validation-methodology.md";

    private const string resourceName = "Naadap.Output.ValidationMethodology.md";

    public static string Write(string outputDirectory)
    {
        using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded resource '{resourceName}' not found — docs/VALIDATION_METHODOLOGY.md " +
                "must be embedded via Naadap.Output.csproj's <EmbeddedResource> entry.");

        using var destination = File.Create(Path.Combine(outputDirectory, FileName));
        resourceStream.CopyTo(destination);

        return FileName;
    }
}
