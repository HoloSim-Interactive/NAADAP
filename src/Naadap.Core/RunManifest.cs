namespace Naadap.Core;

/// <summary>
/// OUT-440's single indexing artifact: the shape written as
/// <c>manifest.json</c> at the root of a run's output bundle, referencing
/// every other OUT-4xx/DATA-OUT-300 artifact the bundle contains. Lives in
/// <c>Naadap.Core</c> alongside its field types — see this role's memory
/// note "NAADAP shared DTOs live in Core".
/// </summary>
/// <param name="Candidates">DATA-OUT-300's ranked candidate-vehicle list.</param>
/// <param name="MethodVisualizationPath">
/// Path (relative to the output directory) of OUT-400's method/pipeline
/// visualization artifact.
/// </param>
/// <param name="ResultVisualizationPath">
/// Path (relative to the output directory) of OUT-410's results
/// visualization artifact.
/// </param>
/// <param name="SummaryMetric">OUT-420's headline metric plus raw counts.</param>
/// <param name="ValidationMethodologyPath">
/// Path (relative to the output directory) of OUT-430's validation-
/// methodology document, copied into every run's bundle.
/// </param>
/// <param name="SkippedFiles">
/// Every input file DATA-IN-110 flagged and skipped during this run, with
/// its reason — carried into the manifest so a reviewer has one file to
/// read for the full run outcome.
/// </param>
public sealed record RunManifest(
    IReadOnlyList<CandidateVehicle> Candidates,
    string MethodVisualizationPath,
    string ResultVisualizationPath,
    Metric SummaryMetric,
    string ValidationMethodologyPath,
    IReadOnlyList<SkippedFile> SkippedFiles);
