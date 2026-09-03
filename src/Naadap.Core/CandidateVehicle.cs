namespace Naadap.Core;

/// <summary>
/// DATA-OUT-300's ranked recommendation output: one candidate contract
/// vehicle the pipeline inferred from a group of documents that share
/// acquisition-requirement content, with a numeric confidence score and the
/// source documents that drove that inference. Produced by
/// <c>Naadap.Output</c> (per docs/SDD.md's block diagram, the Recommend
/// stage consumes Core's <see cref="DocumentCluster"/>s) and consumed by
/// <c>Naadap.Output</c>'s bundler/metric/visualization steps and eventually
/// serialized into <see cref="RunManifest"/>. Lives in <c>Naadap.Core</c>
/// rather than <c>Naadap.Output</c> for the same reason
/// <see cref="DocumentRecord"/> does — see this role's memory note
/// "NAADAP shared DTOs live in Core": it keeps every cross-stage contract on
/// the dependency-free side of the graph, so CORE-240's inspection never has
/// to reason about which stage a shared record "really" belongs to.
/// </summary>
/// <param name="VehicleId">
/// Deterministic, human-readable identifier derived from the underlying
/// cluster's evidence (its top TF-IDF terms) — not a lookup against a
/// real-world contract-vehicle name/database, since the pipeline has no such
/// reference data and CORE-240 forbids an LLM call to synthesize one. This
/// is an inferred grouping label, auditable back to the terms that produced
/// it, exactly like <see cref="DocumentCluster.TopTerms"/>.
/// </param>
/// <param name="Score">
/// Confidence in [0, 1]: the cluster's mean pairwise cosine similarity
/// (cohesion) among its contributing documents, using the same TF-IDF
/// representation CORE-200's clustering step itself used. Higher means the
/// contributing documents are more tightly related in content, not a
/// probability in the statistical sense.
/// </param>
/// <param name="ContributingDocuments">
/// <see cref="DocumentRecord.SourceFilename"/> of every document that
/// contributed to this candidate. Always non-empty.
/// </param>
public sealed record CandidateVehicle(
    string VehicleId,
    double Score,
    IReadOnlyList<string> ContributingDocuments);
