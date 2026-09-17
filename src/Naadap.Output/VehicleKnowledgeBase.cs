using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Naadap.Output;

/// <summary>
/// The shipped vehicle knowledge base (gate G5; derived requirements KB-600
/// through KB-630; reopened DELIV-950). Built offline by
/// <c>scripts/kb/build_kb.py</c>, committed under <c>Resources/kb/</c>, and
/// embedded in this assembly, so a run reads it with no file or network
/// dependency (NFR-500). <see cref="Load"/> verifies every file's SHA-256
/// against <c>manifest.sha256</c> before parsing anything and throws
/// <see cref="KnowledgeBaseIntegrityException"/> on any mismatch (KB-630);
/// the CLI turns that into a non-zero exit with no candidate list written.
/// </summary>
public sealed class VehicleKnowledgeBase
{
    private const string resourcePrefix = "Naadap.Output.kb.";
    private const string manifestName = "manifest.sha256";

    private VehicleKnowledgeBase(
        string version,
        string manifestSha256,
        IReadOnlyList<VehicleRecord> vehicles,
        IReadOnlyDictionary<(string VehicleId, string Office), OfficeAffinityRow> affinity,
        IReadOnlyDictionary<string, int> ordersByOffice)
    {
        Version = version;
        ManifestSha256 = manifestSha256;
        Vehicles = vehicles;
        Affinity = affinity;
        OrdersByOffice = ordersByOffice;
    }

    /// <summary>The <c>kb_version</c> string, e.g. <c>2026-09-17.1</c>.</summary>
    public string Version { get; }

    /// <summary>SHA-256 of <c>manifest.sha256</c> itself, recorded in every run manifest (CORE-275).</summary>
    public string ManifestSha256 { get; }

    /// <summary>Every knowledge-base row, in file order (families first, then parent IDVs by descending order count).</summary>
    public IReadOnlyList<VehicleRecord> Vehicles { get; }

    /// <summary>Office-affinity table keyed by (vehicle id, contracting office code).</summary>
    public IReadOnlyDictionary<(string VehicleId, string Office), OfficeAffinityRow> Affinity { get; }

    /// <summary>Total FY2025 orders under vehicles per contracting office, derived from the affinity table.</summary>
    public IReadOnlyDictionary<string, int> OrdersByOffice { get; }

    /// <summary>
    /// The date the knowledge base was built, parsed from <see cref="Version"/>.
    /// Used as the deterministic "as of" date for ordering-period checks so a
    /// run's result does not depend on the wall clock (CORE-210).
    /// </summary>
    public DateOnly AsOfDate => DateOnly.ParseExact(Version.Split('.')[0], "yyyy-MM-dd");

    /// <summary>Loads and verifies the embedded knowledge base.</summary>
    /// <exception cref="KnowledgeBaseIntegrityException">A file's hash does not match the manifest, or a manifest entry is missing.</exception>
    public static VehicleKnowledgeBase Load()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var manifestText = ReadResourceText(assembly, manifestName);
        var manifestSha = Sha256Hex(Encoding.UTF8.GetBytes(manifestText));

        var expected = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var line in manifestText.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Split("  ", 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2)
            {
                expected[parts[1]] = parts[0];
            }
        }

        var verified = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        foreach (var (fileName, hash) in expected)
        {
            var bytes = ReadResourceBytes(assembly, fileName);
            var actual = Sha256Hex(bytes);
            if (!string.Equals(actual, hash, StringComparison.OrdinalIgnoreCase))
            {
                throw new KnowledgeBaseIntegrityException(
                    $"Knowledge-base file '{fileName}' has SHA-256 {actual}; manifest.sha256 expects {hash}. " +
                    "The pipeline refuses to recommend against a knowledge base that does not match its manifest (KB-630).");
            }

            verified[fileName] = bytes;
        }

        foreach (var required in new[] { "kb_version", "vehicles.jsonl", "office_affinity.tsv" })
        {
            if (!verified.ContainsKey(required))
            {
                throw new KnowledgeBaseIntegrityException(
                    $"Knowledge-base file '{required}' is not listed in manifest.sha256 (KB-600).");
            }
        }

        var version = Encoding.UTF8.GetString(verified["kb_version"]).Trim();
        var vehicles = ParseVehicles(Encoding.UTF8.GetString(verified["vehicles.jsonl"]));
        var (affinity, ordersByOffice) = ParseAffinity(Encoding.UTF8.GetString(verified["office_affinity.tsv"]));

        return new VehicleKnowledgeBase(version, manifestSha, vehicles, affinity, ordersByOffice);
    }

    private static IReadOnlyList<VehicleRecord> ParseVehicles(string jsonl)
    {
        var list = new List<VehicleRecord>();
        foreach (var line in jsonl.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            using var doc = JsonDocument.Parse(line);
            var e = doc.RootElement;
            list.Add(new VehicleRecord(
                VehicleId: e.GetProperty("vehicle_id").GetString()!,
                Family: e.GetProperty("family").GetString()!,
                RowKind: e.GetProperty("row_kind").GetString()!,
                Name: e.GetProperty("name").GetString()!,
                VehicleType: e.GetProperty("vehicle_type").GetString()!,
                OwnerOffice: e.GetProperty("owner_office").GetString()!,
                ScopeText: e.GetProperty("scope_text").GetString()!,
                ObservedDescriptions: StringList(e, "observed_descriptions"),
                OrderingPeriodEnd: e.GetProperty("ordering_period_end").GetString()!,
                EligibleOrderingActivities: StringList(e, "eligible_ordering_activities"),
                AcquisitionPathTier: e.GetProperty("acquisition_path_tier").GetInt32(),
                OrdersFy2025: e.GetProperty("orders_fy2025").GetInt32(),
                DodPrograms: StringList(e, "dod_programs")));
        }

        return list;
    }

    private static IReadOnlyList<string> StringList(JsonElement e, string name)
    {
        if (!e.TryGetProperty(name, out var arr) || arr.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return arr.EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToList();
    }

    private static (IReadOnlyDictionary<(string, string), OfficeAffinityRow>, IReadOnlyDictionary<string, int>) ParseAffinity(string tsv)
    {
        var rows = new Dictionary<(string, string), OfficeAffinityRow>();
        var byOffice = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var line in tsv.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1))
        {
            var f = line.Split('\t');
            if (f.Length < 4)
            {
                continue;
            }

            var row = new OfficeAffinityRow(f[0], f[1], int.Parse(f[2]), double.Parse(f[3], System.Globalization.CultureInfo.InvariantCulture));
            rows[(row.VehicleId, row.Office)] = row;
            byOffice[row.Office] = byOffice.GetValueOrDefault(row.Office) + row.Orders;
        }

        return (rows, byOffice);
    }

    private static string ReadResourceText(Assembly assembly, string fileName) =>
        Encoding.UTF8.GetString(ReadResourceBytes(assembly, fileName));

    private static byte[] ReadResourceBytes(Assembly assembly, string fileName)
    {
        using var stream = assembly.GetManifestResourceStream(resourcePrefix + fileName)
            ?? throw new KnowledgeBaseIntegrityException(
                $"Embedded knowledge-base resource '{resourcePrefix}{fileName}' not found; " +
                "Resources/kb must be embedded via Naadap.Output.csproj.");
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    private static string Sha256Hex(byte[] bytes) => Convert.ToHexStringLower(SHA256.HashData(bytes));
}

/// <summary>The subset of a knowledge-base row the matcher and the evidence record use. Full rows: <c>Resources/kb/vehicles.jsonl</c>; schema: <c>docs/KB_SCHEMA.md</c>.</summary>
public sealed record VehicleRecord(
    string VehicleId,
    string Family,
    string RowKind,
    string Name,
    string VehicleType,
    string OwnerOffice,
    string ScopeText,
    IReadOnlyList<string> ObservedDescriptions,
    string OrderingPeriodEnd,
    IReadOnlyList<string> EligibleOrderingActivities,
    int AcquisitionPathTier,
    int OrdersFy2025,
    IReadOnlyList<string> DodPrograms);

/// <summary>One row of <c>office_affinity.tsv</c>.</summary>
public sealed record OfficeAffinityRow(string VehicleId, string Office, int Orders, double ShareOfOfficeOrders);

/// <summary>Thrown when the embedded knowledge base fails its manifest check (KB-630) or is incomplete (KB-600).</summary>
public sealed class KnowledgeBaseIntegrityException(string message) : Exception(message);
