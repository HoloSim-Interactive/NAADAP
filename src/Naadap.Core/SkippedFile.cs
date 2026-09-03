namespace Naadap.Core;

/// <summary>
/// Records one input file that Ingestion could not turn into a
/// <see cref="DocumentRecord"/> — malformed, unsupported, or corrupt
/// (DATA-IN-110) — and why, so the run report can list every skipped file
/// without terminating the batch. Also part of <c>RunManifest</c>'s
/// <c>SkippedFiles</c> field per docs/SDD.md's data schema.
/// </summary>
/// <param name="SourceFilename">File name (not full path) that was skipped.</param>
/// <param name="Reason">Human-readable reason the file was skipped (e.g. "unable to parse: truncated PDF stream").</param>
public sealed record SkippedFile(string SourceFilename, string Reason);
