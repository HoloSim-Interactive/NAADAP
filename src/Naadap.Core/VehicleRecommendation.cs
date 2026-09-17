namespace Naadap.Core;

/// <summary>
/// One cluster's vehicle recommendation: the strategic contract vehicles from
/// the shipped knowledge base that could absorb the cluster's shared
/// requirement, ranked, with the evidence for each and the reason every
/// other vehicle was ruled out. The tool identifies suitable vehicles; the
/// contracting officer and the FAR 7.107 / NMCARS 5237.102 approval
/// authorities decide. Nothing here states a Government determination.
/// </summary>
/// <param name="ClusterId">The <see cref="DocumentCluster.ClusterId"/> this record is for.</param>
/// <param name="RequestingOffices">Contracting-office DoDAACs found in the cluster's documents (empty if none).</param>
/// <param name="Candidates">Ranked candidates at or above the evidence floor, best first (at most <c>MaxCandidates</c>).</param>
/// <param name="BelowEvidenceFloor">The next candidates below the floor, best first. Recorded, never suppressed (client direction 2026-09-17), so the record shows they were considered.</param>
/// <param name="BelowEvidenceFloorTotal">How many scored vehicles fell below the floor in total; the full ranking is in the run's vehicle-ranking file.</param>
/// <param name="Eliminations">Every vehicle removed by a hard constraint, with the constraint that removed it.</param>
/// <param name="NewVehicleIndicated">True when no candidate reached the evidence floor and the cluster has two or more members: the reserved identifier <c>NEW-VEHICLE-INDICATED</c> applies.</param>
/// <param name="EvidenceFloor">The floor value applied.</param>
public sealed record VehicleRecommendation(
    string ClusterId,
    IReadOnlyList<string> RequestingOffices,
    IReadOnlyList<VehicleCandidate> Candidates,
    IReadOnlyList<VehicleCandidate> BelowEvidenceFloor,
    int BelowEvidenceFloorTotal,
    IReadOnlyList<VehicleElimination> Eliminations,
    bool NewVehicleIndicated,
    double EvidenceFloor);

/// <summary>
/// One vehicle scored against one cluster, with per-channel evidence.
/// </summary>
/// <param name="VehicleId">Knowledge-base row identifier (a family id such as <c>SEAPORT-NXG</c>, or a parent IDV PIID).</param>
/// <param name="Name">The vehicle's name from the knowledge base.</param>
/// <param name="Family">Family identifier (equals <paramref name="VehicleId"/> for single-IDV rows).</param>
/// <param name="AcquisitionPathTier">1 to 5 per derived requirement CORE-273.</param>
/// <param name="Score">Composite score in [0, 1]: weighted sum of the channel scores, weights recorded in the run's method visualization.</param>
/// <param name="Lexical">Lexical scope-overlap channel.</param>
/// <param name="OfficeAffinity">Office-affinity channel.</param>
/// <param name="ConstraintsPassed">Hard constraints evaluated and passed, by code.</param>
/// <param name="ConstraintsNotEvaluated">Hard constraints the run could not evaluate (no document-side signal), by code.</param>
/// <param name="MarginToNext">This candidate's score minus the next-ranked candidate's score; 0 for the last.</param>
/// <param name="DecidingKey">The key that placed this candidate above the next one: <c>score</c>, <c>tier</c> (within the near-tie epsilon), or <c>vehicle_id</c>.</param>
public sealed record VehicleCandidate(
    string VehicleId,
    string Name,
    string Family,
    int AcquisitionPathTier,
    double Score,
    LexicalEvidence Lexical,
    OfficeAffinityEvidence OfficeAffinity,
    IReadOnlyList<string> ConstraintsPassed,
    IReadOnlyList<string> ConstraintsNotEvaluated,
    double MarginToNext,
    string DecidingKey);

/// <summary>
/// Lexical channel: cosine similarity between the cluster's TF-IDF vector and
/// the vehicle's scope text (name, curated scope, observed FPDS base
/// descriptions), in a vector space built over both. The matched terms are
/// the highest-weighted terms the two share, so a reviewer can see what
/// drove the score.
/// </summary>
public sealed record LexicalEvidence(double Cosine, IReadOnlyList<string> MatchedTerms);

/// <summary>
/// Office-affinity channel: the share of the requesting office's FY2025
/// orders under vehicles that went to this vehicle, from the knowledge base's
/// office-affinity table. <paramref name="Office"/> is empty and
/// <paramref name="Share"/> is 0 when the cluster names no known office.
/// </summary>
public sealed record OfficeAffinityEvidence(string Office, int Orders, double Share);

/// <summary>A vehicle removed from a cluster's candidate list by a hard constraint (derived requirement CORE-272).</summary>
/// <param name="VehicleId">The knowledge-base row removed.</param>
/// <param name="Constraint">Constraint code, e.g. <c>ORDERING-PERIOD-CLOSED</c>.</param>
/// <param name="Detail">The values that failed the check.</param>
public sealed record VehicleElimination(string VehicleId, string Constraint, string Detail);
