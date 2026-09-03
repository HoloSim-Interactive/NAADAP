---
name: dockerfile-docs-embedded-resource-and-dockerignore
description: A csproj EmbeddedResource pointing outside src/ (e.g. into docs/) silently breaks `docker build` even though `dotnet build` on the host works fine — two separate causes, both must be fixed.
metadata:
  type: project
---

Naadap.Output embeds `docs/VALIDATION_METHODOLOGY.md` at build time (see
[[embed_static_doc_as_resource_for_runtime_bundle]], OUT-430). That works
fine with a plain host-side `dotnet build`, but `docker build` failed with
`CS1566: Error reading resource ... Could not find a part of the path
'/src/docs/VALIDATION_METHODOLOGY.md'` — this went undetected from #9
(when the resource was added) all the way to #11 (NFR-500's Docker
packaging issue), because nobody had actually run `docker build` in
between.

Two independent things had to be fixed, not just one:
1. The Dockerfile's build stage only `COPY`'d `src/` and `tests/` — never
   `docs/` — into the build context, so the file the `.csproj` reaches for
   simply wasn't there. Fix: add a *targeted* `COPY
   docs/VALIDATION_METHODOLOGY.md docs/VALIDATION_METHODOLOGY.md` (not the
   whole `docs/` tree — nothing else under `docs/` is needed at build
   time, and pulling all of it in would also bust the Docker layer cache
   on every unrelated doc edit).
2. `.dockerignore` excluded `docs/` *and* `*.md` wholesale, so even the
   targeted `COPY` above got silently skipped ("Attempting to Copy file
   ... that is excluded by .dockerignore" — a warning, not a build
   failure, easy to miss in scrollback). Fix: add a negation line
   `!docs/VALIDATION_METHODOLOGY.md` after the exclusion patterns.

**Lesson: whenever a `.csproj` embeds a resource from outside its own
project directory, `docker build` needs to actually be run (not just
`dotnet build`) to catch a missing-file break — the two build contexts
diverge in what's present on disk, and CI at the time only ran `dotnet
build`/`dotnet test`, never `docker build`.** Worth a real `docker build .`
smoke test any time a Dockerfile or an out-of-tree `EmbeddedResource` is
touched, not just for the packaging issue itself.

**Related:** [[embed_static_doc_as_resource_for_runtime_bundle]]
