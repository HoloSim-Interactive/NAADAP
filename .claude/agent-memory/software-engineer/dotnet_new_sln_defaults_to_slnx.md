---
name: dotnet-new-sln-defaults-to-slnx
description: dotnet new sln (SDK 10.x) writes a .slnx file by default, not .sln — force the classic format when a .sln is required.
metadata:
  type: platform
---

On this environment's default SDK (10.0.400, even when the project targets
net9.0), `dotnet new sln -n Foo` silently creates `Foo.slnx` (the newer XML
solution format), not `Foo.sln`. Running it again without noticing produces
a confusing "would overwrite Foo.slnx" error while `Foo.sln` doesn't exist
anywhere.

**Why it matters:** SDDs/RTVMs that specify `Naadap.sln` (or any `<name>.sln`)
by name, and CI/Visual-Studio-open steps that expect the classic format, will
silently fail to find anything if you don't force it.

**How to apply:** always pass `-f sln` explicitly:
```
dotnet new sln -n Naadap -f sln
```
Verify with `ls *.sln` right after — don't assume the CLI's "created
successfully" message means the expected filename exists.
