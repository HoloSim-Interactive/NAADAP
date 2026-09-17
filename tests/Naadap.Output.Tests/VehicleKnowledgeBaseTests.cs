using System.Reflection;
using System.Security.Cryptography;

namespace Naadap.Output.Tests;

/// <summary>
/// Knowledge-base integrity and content (derived requirements KB-600, KB-610,
/// KB-630; gate G5). The base is embedded in <c>Naadap.Output</c>, so these
/// tests exercise the shipped artifact itself, not a fixture.
/// </summary>
public class VehicleKnowledgeBaseTests
{
    [Fact]
    public void Load_ShippedKnowledgeBase_VerifiesManifestAndParses()
    {
        var kb = VehicleKnowledgeBase.Load();

        Assert.Matches(@"^\d{4}-\d{2}-\d{2}\.\d+$", kb.Version);
        Assert.Equal(64, kb.ManifestSha256.Length);
        Assert.True(kb.Vehicles.Count > 100, "expected the data-derived rows, not only the catalog families");
        Assert.Contains(kb.Vehicles, v => v.VehicleId == "SEAPORT-NXG" && v.RowKind == "family");
        Assert.Contains(kb.Vehicles, v => v.RowKind == "parent-idv");
        Assert.NotEmpty(kb.Affinity);
        Assert.True(kb.OrdersByOffice.ContainsKey("N00421"));
    }

    [Fact]
    public void Load_EveryRow_HasNonEmptyRequiredFields()
    {
        var kb = VehicleKnowledgeBase.Load();

        foreach (var v in kb.Vehicles)
        {
            Assert.False(string.IsNullOrWhiteSpace(v.VehicleId), "vehicle_id");
            Assert.False(string.IsNullOrWhiteSpace(v.Name), v.VehicleId + " name");
            Assert.False(string.IsNullOrWhiteSpace(v.Family), v.VehicleId + " family");
            Assert.False(string.IsNullOrWhiteSpace(v.ScopeText), v.VehicleId + " scope_text");
            Assert.False(string.IsNullOrWhiteSpace(v.OrderingPeriodEnd), v.VehicleId + " ordering_period_end");
            Assert.NotEmpty(v.EligibleOrderingActivities);
            Assert.InRange(v.AcquisitionPathTier, 1, 5);
        }

        Assert.Equal(kb.Vehicles.Count, kb.Vehicles.Select(v => v.VehicleId).Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void ManifestSha256_EveryHashedFile_MatchesTheEmbeddedBytes()
    {
        // The same check Load() performs, done independently here so a
        // regression in Load()'s verification would still be caught.
        var assembly = typeof(VehicleKnowledgeBase).Assembly;
        var manifest = ReadResource(assembly, "Naadap.Output.kb.manifest.sha256");
        var entries = System.Text.Encoding.UTF8.GetString(manifest)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split("  ", 2, StringSplitOptions.TrimEntries))
            .ToList();

        Assert.True(entries.Count >= 5, "manifest lists every knowledge-base file");
        foreach (var e in entries)
        {
            var bytes = ReadResource(assembly, "Naadap.Output.kb." + e[1]);
            Assert.Equal(e[0], Convert.ToHexStringLower(SHA256.HashData(bytes)));
        }
    }

    [Fact]
    public void AsOfDate_IsParsedFromTheVersion()
    {
        var kb = VehicleKnowledgeBase.Load();

        Assert.Equal(kb.Version.Split('.')[0], kb.AsOfDate.ToString("yyyy-MM-dd"));
    }

    private static byte[] ReadResource(Assembly assembly, string name)
    {
        using var s = assembly.GetManifestResourceStream(name) ?? throw new InvalidOperationException(name);
        using var ms = new MemoryStream();
        s.CopyTo(ms);
        return ms.ToArray();
    }
}
