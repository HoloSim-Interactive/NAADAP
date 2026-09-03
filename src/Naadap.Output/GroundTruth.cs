using System.Text.Json;
using System.Text.Json.Serialization;

namespace Naadap.Output;

/// <summary>
/// The subset of <c>tests/fixtures/reference-20/ground-truth.json</c>'s
/// schema (see <c>tests/fixtures/README.md</c>) that
/// <see cref="MetricCalculator"/> needs: a source filename to its known
/// real-world candidate-vehicle ID. A real production input directory has no
/// such file — that is expected, not an error; see
/// docs/VALIDATION_METHODOLOGY.md.
/// </summary>
public sealed class GroundTruth
{
    private GroundTruth(IReadOnlyDictionary<string, string> vehicleIdByFilename)
    {
        VehicleIdByFilename = vehicleIdByFilename;
    }

    /// <summary>Source filename (as recorded in <see cref="Core.DocumentRecord.SourceFilename"/>) to ground-truth vehicle ID.</summary>
    public IReadOnlyDictionary<string, string> VehicleIdByFilename { get; }

    /// <summary>
    /// The filename this loader looks for directly inside a run's input
    /// directory — matches <c>tests/fixtures/reference-20/</c>'s own layout,
    /// where <c>ground-truth.json</c> sits alongside the documents it
    /// describes.
    /// </summary>
    public const string FileName = "ground-truth.json";

    /// <summary>
    /// Looks for <see cref="FileName"/> directly inside
    /// <paramref name="inputDirectory"/>. Returns <see langword="null"/> (not
    /// an error) when absent, malformed, or empty — a normal production run
    /// has no ground truth, and a malformed/unreadable file should degrade
    /// to "metric not computed" rather than fail the whole run (consistent
    /// with DATA-IN-110's "never abort the batch" spirit, applied here to an
    /// optional validation input rather than a document to ingest).
    /// </summary>
    public static GroundTruth? TryLoad(string inputDirectory)
    {
        var path = Path.Combine(inputDirectory, FileName);
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(path);
            var parsed = JsonSerializer.Deserialize<GroundTruthFile>(json);
            var documents = parsed?.Documents;
            if (documents is null || documents.Count == 0)
            {
                return null;
            }

            var map = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var entry in documents)
            {
                if (!string.IsNullOrWhiteSpace(entry.File) && !string.IsNullOrWhiteSpace(entry.VehicleId))
                {
                    map[entry.File] = entry.VehicleId;
                }
            }

            return map.Count == 0 ? null : new GroundTruth(map);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed class GroundTruthFile
    {
        [JsonPropertyName("documents")]
        public List<GroundTruthDocument>? Documents { get; set; }
    }

    private sealed class GroundTruthDocument
    {
        [JsonPropertyName("file")]
        public string? File { get; set; }

        [JsonPropertyName("vehicleId")]
        public string? VehicleId { get; set; }
    }
}
