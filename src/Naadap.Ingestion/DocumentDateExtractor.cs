using System.Globalization;
using System.Text.RegularExpressions;

namespace Naadap.Ingestion;

/// <summary>
/// Best-effort extraction of the <c>Date?</c> field on <see
/// cref="Naadap.Core.DocumentRecord"/> (DATA-IN-100: "date if present in
/// the source"). Deliberately conservative: returns null rather than
/// guessing when no clearly-dated text is found, since an absent date is
/// an explicitly valid, schema-supported outcome (nullable field), not an
/// ingestion failure.
/// </summary>
public static class DocumentDateExtractor
{
    // Matches "January 5, 2024", "Jan 5, 2024", "5 January 2024", and
    // "2024-01-05" / "01/05/2024" style dates — the formats acquisition
    // documents (solicitation issue dates, CDRL due dates) typically carry.
    private static readonly Regex longForm = new(
        @"\b(January|February|March|April|May|June|July|August|September|October|November|December)\s+\d{1,2},?\s+\d{4}\b",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex iso = new(@"\b\d{4}-\d{2}-\d{2}\b", RegexOptions.Compiled);

    private static readonly Regex slash = new(@"\b\d{1,2}/\d{1,2}/\d{4}\b", RegexOptions.Compiled);

    public static DateOnly? Extract(string text)
    {
        foreach (var regex in new[] { longForm, iso, slash })
        {
            var match = regex.Match(text);
            if (match.Success && TryParse(match.Value, out var date))
            {
                return date;
            }
        }

        return null;
    }

    private static bool TryParse(string candidate, out DateOnly date)
    {
        var normalized = candidate.TrimEnd(',');
        if (DateOnly.TryParse(normalized, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            return true;
        }

        date = default;
        return false;
    }
}
