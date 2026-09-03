namespace Naadap.Core;

/// <summary>
/// One group of documents an <see cref="IClusteringComponent"/> judged to
/// share acquisition-requirement content (CORE-200). This is Core's
/// internal clustering output — the next pipeline stage (DATA-OUT-300's
/// candidate-vehicle ranking, in <c>Naadap.Output</c>, per docs/SDD.md's
/// block diagram) maps clusters onto ranked <c>CandidateVehicle</c>
/// entries; that mapping is out of this record's scope.
/// </summary>
/// <param name="ClusterId">
/// Stable, deterministically assigned identifier (e.g. "cluster-0001") —
/// ordering is derived from the clustered documents' filenames, not from
/// hash-based iteration, so the same input set always yields the same IDs
/// (CORE-210).
/// </param>
/// <param name="DocumentFilenames">
/// <see cref="DocumentRecord.SourceFilename"/> of every document assigned
/// to this cluster, ordinal-sorted. Always non-empty — a cluster with no
/// members is not produced.
/// </param>
/// <param name="TopTerms">
/// The highest-weighted terms driving this cluster, most significant
/// first — the human-readable explanation of what content the cluster
/// shares, and the concrete evidence behind CORE-200's "documented
/// similarity/score threshold" requirement for a given run's output.
/// </param>
public sealed record DocumentCluster(
    string ClusterId,
    IReadOnlyList<string> DocumentFilenames,
    IReadOnlyList<string> TopTerms);
