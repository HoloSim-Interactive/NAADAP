---
name: repo-gitignore-missing-dotnet-patterns
description: template repos start with a C/C++-flavored .gitignore; add bin/, obj/, *.user, *.pdb, .vs/ yourself the first time you introduce .NET projects.
metadata:
  type: project
---

The NAADAP repo's root `.gitignore` (from the project skeleton) only covers
C/C++ build byproducts (`*.o`, `*.obj`, `*.so`, `*.exe`, `*.dll`, ...). It has
no .NET-specific entries. `*.dll`/`*.exe` incidentally hide most of a `bin/`
folder, but plenty of MSBuild/NuGet artifacts survive uncaught: `obj/`,
`*.pdb`, `*.json` restore caches (`project.assets.json`,
`*.nuget.g.props`), `.vs/`.

**Why it matters:** `git status` will look clean right up until someone runs
`git add -A` without checking, and a full `bin/`+`obj/` tree gets committed.

**How to apply:** the first time you scaffold any .NET project into a repo
like this, append a `.NET / Visual Studio build output` block to
`.gitignore` (`bin/`, `obj/`, `*.user`, `*.pdb`, `*.nupkg`, `.vs/`) rather
than assuming the skeleton already handles it. Verify with
`git status --short` after a real `dotnet build`/`dotnet test`, not just a
glance at the tracked file list.
