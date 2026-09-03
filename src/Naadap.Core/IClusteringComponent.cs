namespace Naadap.Core;

/// <summary>
/// Extension point for CORE-200's clustering/requirement-extraction step —
/// the other half of DATA-IN-120/DELIV-940's documented pair of extension
/// interfaces alongside <c>Naadap.Ingestion.IDocumentParser</c>. Adding a
/// new clustering strategy means writing a new implementation of this
/// interface and passing it wherever a caller composes the pipeline; it
/// never requires editing an existing implementation's dispatch logic.
/// </summary>
/// <remarks>
/// Every implementation referenced from the production pipeline path must
/// stay within <c>Naadap.Core</c>'s zero-third-party-dependency rule
/// (CORE-240) — an LLM-assisted or retrieval-based alternative
/// (CORE-260) implements this same interface but lives in
/// <c>Naadap.Alternative</c> instead, and is never referenced by
/// <c>Naadap.Core</c> or wired into a production run.
/// </remarks>
public interface IClusteringComponent
{
    /// <summary>
    /// Groups <paramref name="documents"/> into <see cref="DocumentCluster"/>s
    /// by shared acquisition-requirement content. Deterministic: the same
    /// input list, in the same order, must produce the same output every
    /// call (CORE-210) — implementations must not rely on randomness,
    /// wall-clock time, or hash-based iteration order for anything that
    /// affects the result.
    /// </summary>
    IReadOnlyList<DocumentCluster> Cluster(IReadOnlyList<DocumentRecord> documents);
}
