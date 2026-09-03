# Maintainer's Guide — Adding a New Extension (DELIV-940)

<!--
Owned by the Software Engineer. Satisfies DELIV-940/DATA-IN-120: a
concrete walkthrough — files to add/edit, interface to implement —
for adding (a) a new input document type and (b) a new
clustering/algorithm component, without editing either extension
point's dispatch/core logic. TP-940 (Inspection) and TP-120
(Demonstration) verify this walkthrough is actually followable.
-->

NAADAP has exactly two extension points. Both follow the same shape:
implement an interface, register the implementation where the caller
composes the pipeline — never add a branch inside existing dispatch
logic.

## 1. Adding a new input document type

**Interface:** `Naadap.Ingestion.IDocumentParser`
(`src/Naadap.Ingestion/IDocumentParser.cs`)

```csharp
public interface IDocumentParser
{
    bool CanParse(string filePath);
    string ExtractText(string filePath);
}
```

### Steps

1. **Add a new file** under `src/Naadap.Ingestion/Parsers/`, e.g.
   `MarkdownDocumentParser.cs`, implementing `IDocumentParser`:
   - `CanParse` — a cheap, no-I/O check (typically the file
     extension). Look at `src/Naadap.Ingestion/Parsers/` for the
     existing PDF/DOCX/plain-text parsers as a template.
   - `ExtractText` — read the file and return its raw text. If the
     file is malformed/corrupt, throw
     `Naadap.Ingestion.DocumentParseException` with a human-readable
     reason (DATA-IN-110) — do not let any other exception escape;
     `IngestionRunner.IngestDirectory` already treats an unhandled
     exception as a skip, but a `DocumentParseException` with a good
     message is what makes the skip reason useful to a reviewer.
2. **Register it** wherever the pipeline is composed — either:
   - add it to the list `IngestionRunner.CreateDefault()` builds
     (`src/Naadap.Ingestion/IngestionRunner.cs`), if it should be part
     of every default run, or
   - pass it into `new IngestionRunner(parserList)` directly at the
     call site, if it's situational.
3. **Do not touch** `IngestionRunner.IngestDirectory`'s dispatch loop.
   It already does exactly one relevant thing: for each file, ask
   every registered parser `CanParse(filePath)` and use the first
   match. Adding a new format never requires changing this loop, a
   `switch` on extension, or anything inside `Naadap.Core`.
4. **Optional:** if the new format implies a new content category, add
   a case to `Naadap.Ingestion.DocumentTypeClassifier` — this is a
   separate, orthogonal step (content classification, which runs
   *after* a parser has already produced text) from the format-parsing
   extension point above; most new formats don't need a
   classifier change at all.

### Worked example

`tests/Naadap.Ingestion.Tests/IngestionRunnerExtensibilityTests.cs`
(`FakeMarkdownDocumentParser`) does exactly this, deliberately defined
*outside* `Naadap.Ingestion` (in the test project) to prove the point:
a `.md` parser is wired in purely via `IngestionRunner`'s public
constructor, with zero edits to `IngestionRunner` itself. This is the
literal TP-120 demonstration.

## 2. Adding a new clustering / algorithm component

**Interface:** `Naadap.Core.IClusteringComponent`
(`src/Naadap.Core/IClusteringComponent.cs`)

```csharp
public interface IClusteringComponent
{
    IReadOnlyList<DocumentCluster> Cluster(IReadOnlyList<DocumentRecord> documents);
}
```

### Steps

1. **Add a new class** implementing `IClusteringComponent`. Where it
   lives depends on whether it's a production candidate or an
   analysis-only comparison:
   - A production clustering strategy that should stay within
     CORE-240's zero-third-party-dependency rule belongs in
     `src/Naadap.Core/`.
   - An LLM-assisted, retrieval/RAG-based, or otherwise
     dependency-heavy alternative — the CORE-260 comparison pattern —
     belongs in `src/Naadap.Alternative/` instead, and must never be
     referenced by `Naadap.Core` or `Naadap.Cli`. See
     `src/Naadap.Alternative/RetrievalAugmentedClusteringComponent.cs`
     for the existing example of this shape.
2. **Implement `Cluster`** deterministically (CORE-210): the same
   input list, in the same order, must produce the same output every
   call — no randomness, wall-clock time, or hash-based iteration
   order affecting the result.
3. **Wire it in** at the call site that composes the pipeline
   (`src/Naadap.Cli/Program.cs` for the production path, or
   `src/Naadap.Alternative/ApproachRunner.cs` for an offline
   comparison run) by constructing the new component instead of — or
   alongside — the existing `TfIdfCosineClusteringComponent`. No
   existing `IClusteringComponent` implementation needs to change.

### Worked example

`tests/Naadap.Core.Tests/ClusteringExtensibilityTests.cs`
(`SingleClusterComponent`) does exactly this, again deliberately
defined outside `Naadap.Core`: a trivial "everything in one cluster"
strategy plugs in purely via the interface, with zero changes to
`TfIdfCosineClusteringComponent` or any other existing `Naadap.Core`
source.

## Running the extension tests

```bash
dotnet test tests/Naadap.Ingestion.Tests
dotnet test tests/Naadap.Core.Tests
```

Both extensibility tests above are part of the normal test suite —
`dotnet test Naadap.sln` from the repo root runs them along with
everything else.
