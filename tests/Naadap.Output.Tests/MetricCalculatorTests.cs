using Naadap.Core;

namespace Naadap.Output.Tests;

/// <summary>
/// TP-420 (OUT-420): exercises <see cref="MetricCalculator"/> and
/// <see cref="GroundTruth"/> loading, against both the real
/// <c>reference-20/ground-truth.json</c> fixture and hand-built scenarios
/// for the precision@5 scoring rules themselves.
/// </summary>
public class MetricCalculatorTests
{
    [Fact]
    public void ComputePrecisionAtFive_NoGroundTruth_ReportsNotComputed()
    {
        var candidates = new[]
        {
            new CandidateVehicle("V1", 0.9, ["a.pdf"]),
        };

        var metric = MetricCalculator.ComputePrecisionAtFive(candidates, groundTruth: null);

        Assert.Equal(MetricCalculator.MetricName, metric.Name);
        Assert.Null(metric.Value);
        Assert.Equal(0, metric.CorrectCount);
        Assert.Equal(0, metric.TotalCount);
        Assert.Contains("not computed", metric.Definition, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComputePrecisionAtFive_AllTopFiveMatchDistinctVehicles_IsPerfect()
    {
        var groundTruth = LoadGroundTruthFromJson(
            """
            { "documents": [
                { "file": "a.pdf", "vehicleId": "V1" },
                { "file": "b.pdf", "vehicleId": "V2" },
                { "file": "c.pdf", "vehicleId": "V3" }
            ]}
            """);

        var candidates = new[]
        {
            new CandidateVehicle("cluster-1", 0.9, ["a.pdf"]),
            new CandidateVehicle("cluster-2", 0.8, ["b.pdf"]),
            new CandidateVehicle("cluster-3", 0.7, ["c.pdf"]),
        };

        var metric = MetricCalculator.ComputePrecisionAtFive(candidates, groundTruth);

        Assert.Equal(3, metric.TotalCount);
        Assert.Equal(3, metric.CorrectCount);
        Assert.Equal(1.0, metric.Value);
    }

    [Fact]
    public void ComputePrecisionAtFive_DuplicateVehicleAcrossCandidates_OnlyCreditsHighestRanked()
    {
        var groundTruth = LoadGroundTruthFromJson(
            """
            { "documents": [
                { "file": "a.pdf", "vehicleId": "V1" },
                { "file": "b.pdf", "vehicleId": "V1" }
            ]}
            """);

        var candidates = new[]
        {
            new CandidateVehicle("cluster-1", 0.9, ["a.pdf"]),
            new CandidateVehicle("cluster-2", 0.8, ["b.pdf"]),
        };

        var metric = MetricCalculator.ComputePrecisionAtFive(candidates, groundTruth);

        Assert.Equal(2, metric.TotalCount);
        Assert.Equal(1, metric.CorrectCount);
        Assert.Equal(0.5, metric.Value);
    }

    [Fact]
    public void ComputePrecisionAtFive_UnknownDocuments_DoNotCount()
    {
        var groundTruth = LoadGroundTruthFromJson(
            """
            { "documents": [
                { "file": "known.pdf", "vehicleId": "V1" }
            ]}
            """);

        var candidates = new[]
        {
            new CandidateVehicle("cluster-1", 0.9, ["unknown.pdf"]),
        };

        var metric = MetricCalculator.ComputePrecisionAtFive(candidates, groundTruth);

        Assert.Equal(1, metric.TotalCount);
        Assert.Equal(0, metric.CorrectCount);
        Assert.Equal(0.0, metric.Value);
    }

    [Fact]
    public void GroundTruth_TryLoad_Reference20Fixture_LoadsAllTwentyDocuments()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "fixtures", "reference-20");

        var groundTruth = GroundTruth.TryLoad(directory);

        Assert.NotNull(groundTruth);
        Assert.Equal(20, groundTruth!.VehicleIdByFilename.Count);
        Assert.Equal("SEAPORT-NXG", groundTruth.VehicleIdByFilename["v1-01-nswccd-pws.pdf"]);
        Assert.Equal("GSA-MAS", groundTruth.VehicleIdByFilename["v4-02-title-escrow-solicitation.pdf"]);
    }

    [Fact]
    public void GroundTruth_TryLoad_DirectoryWithNoFile_ReturnsNull()
    {
        var directory = Path.Combine(Path.GetTempPath(), "naadap-empty-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);

        try
        {
            Assert.Null(GroundTruth.TryLoad(directory));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static GroundTruth LoadGroundTruthFromJson(string json)
    {
        var directory = Path.Combine(Path.GetTempPath(), "naadap-gt-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);
        try
        {
            File.WriteAllText(Path.Combine(directory, GroundTruth.FileName), json);
            return GroundTruth.TryLoad(directory)!;
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
