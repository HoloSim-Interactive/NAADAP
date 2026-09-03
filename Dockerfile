# syntax=docker/dockerfile:1
#
# Multi-stage build for the NAADAP batch pipeline (UI-001 entrypoint).
# Stage 1 (build) restores/builds/publishes with the full SDK image.
# Stage 2 (runtime) only ships the published output on the smaller
# runtime-only image, so nothing is restored at container run time
# (NFR-500).

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files first so `dotnet restore` is cached independently of
# source-code changes.
COPY Naadap.sln ./
COPY src/Naadap.Ingestion/Naadap.Ingestion.csproj src/Naadap.Ingestion/
COPY src/Naadap.Core/Naadap.Core.csproj src/Naadap.Core/
COPY src/Naadap.Alternative/Naadap.Alternative.csproj src/Naadap.Alternative/
COPY src/Naadap.LlmStep/Naadap.LlmStep.csproj src/Naadap.LlmStep/
COPY src/Naadap.Output/Naadap.Output.csproj src/Naadap.Output/
COPY src/Naadap.Cli/Naadap.Cli.csproj src/Naadap.Cli/
COPY tests/Naadap.Ingestion.Tests/Naadap.Ingestion.Tests.csproj tests/Naadap.Ingestion.Tests/
COPY tests/Naadap.Core.Tests/Naadap.Core.Tests.csproj tests/Naadap.Core.Tests/
COPY tests/Naadap.Alternative.Tests/Naadap.Alternative.Tests.csproj tests/Naadap.Alternative.Tests/
COPY tests/Naadap.LlmStep.Tests/Naadap.LlmStep.Tests.csproj tests/Naadap.LlmStep.Tests/
COPY tests/Naadap.Output.Tests/Naadap.Output.Tests.csproj tests/Naadap.Output.Tests/
COPY tests/Naadap.Cli.Tests/Naadap.Cli.Tests.csproj tests/Naadap.Cli.Tests/
RUN dotnet restore Naadap.sln

# Now bring in the rest of the source and publish just the CLI entrypoint
# (and, transitively, everything it references: Ingestion, Core, Output,
# LlmStep).
COPY src/ src/
COPY tests/ tests/
RUN dotnet publish src/Naadap.Cli/Naadap.Cli.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# UI-001: `docker run naadap --input <dir> --output <dir>`. No interactive
# stdin is read, so the container is safe to run with </dev/null.
ENTRYPOINT ["dotnet", "Naadap.Cli.dll"]
