---
name: csharp-on-ubuntu-is-not-a-port
description: A .sln/.csproj built on Ubuntu opens directly in Visual Studio — "refactor into VS later" is not a port, and C# does not use CMake.
metadata:
  type: reference
---

For the C#-mandated NAVAIR project, developing on Ubuntu runners and
delivering a Visual Studio solution are the *same artifact*, not two
phases. `dotnet new sln` / `dotnet new classlib` produce `.sln` and
`.csproj` files that Visual Studio on Windows opens directly. The .NET SDK
is preinstalled on `ubuntu-latest`.

Two corrections worth making early:

- **C# does not use CMake.** The build system is MSBuild, driven by
  `dotnet build` / `dotnet test` over `.csproj`. The client's kickoff note
  said "C# on Ubuntu with CMAKE"; if that phrasing reaches the SDD it bakes
  a wrong toolchain convention into every downstream document.
- **The only real portability trap is a Windows-only target framework** —
  `net9.0-windows`, WPF, WinForms. Keep the core on plain `net9.0` and the
  VS hand-off costs nothing. This also matters because the deliverable must
  run in a Linux Docker container ([[navair-prize-challenge]]).

**Why:** budgeting a late "refactor into Visual Studio" phase would spend
schedule on work that does not exist, on a challenge with a hard external
deadline.
