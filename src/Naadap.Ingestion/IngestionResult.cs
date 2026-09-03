using Naadap.Core;

namespace Naadap.Ingestion;

/// <summary>
/// Output of one <see cref="IngestionRunner.IngestDirectory"/> call: every
/// successfully-normalized <see cref="DocumentRecord"/>, plus every file
/// that was flagged and skipped (DATA-IN-110) with a human-readable reason.
/// The run report (OUT-440's manifest) is built from
/// <see cref="SkippedFiles"/> downstream.
/// </summary>
public sealed record IngestionResult(
    IReadOnlyList<DocumentRecord> Records,
    IReadOnlyList<SkippedFile> SkippedFiles);
