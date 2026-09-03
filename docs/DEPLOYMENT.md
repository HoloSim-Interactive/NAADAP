# Deployment Instructions (DELIV-970)

<!--
Owned by the Software Engineer. The discrete, individually locatable
"deployment instructions" artifact `docs/PROJECT_DEFINITION.md` SN-5
and `docs/RTVM.md` DELIV-970 require, distinct from the algorithm
documentation (`docs/ALGORITHM_COMPARISON.md`) and dependency
documentation (`docs/DEPENDENCIES.md`).

This document is the operator/deployment-environment reference (image
build, resource tiers, network policy, IL4 considerations). For the
first-time "clone it and run it once" walkthrough, see the root
`README.md` (DELIV-930) — that document is self-contained on its own
for TP-930's clean-machine test; this one goes deeper on the
deployment surface a real IL4 operator configures.
-->

## Packaging model

NAADAP ships as a single Docker image, built via the multi-stage
`Dockerfile` at the repo root:

1. **Build stage** (`mcr.microsoft.com/dotnet/sdk:9.0`) — restores and
   publishes `src/Naadap.Cli/Naadap.Cli.csproj` (and everything it
   transitively references: Ingestion, Core, Output, LlmStep) in
   `Release` configuration.
2. **Runtime stage** (`mcr.microsoft.com/dotnet/runtime:9.0`, no SDK)
   — only the published output is copied in. Nothing is restored or
   fetched at container run time (NFR-500): the image is fully
   self-contained the moment `docker build` finishes.

The entrypoint is `dotnet Naadap.Cli.dll`, i.e. the container itself
*is* the `naadap` CLI — `docker run <image> --input <dir> --output
<dir> [--enable-llm-step]` is the whole invocation surface (UI-001).
The container never reads stdin, so it's safe to run in any
non-interactive scheduler (`</dev/null` is the literal test TP-001
uses to confirm this).

## Building the image

```bash
docker build -t naadap:latest .
```

or, equivalently, via Compose (see below):

```bash
docker compose build
```

## Resource tiers (NFR-530)

`docker-compose.yml` defines three resource profiles, matching the
challenge's exact scored tiers (`docs/PROJECT_DEFINITION.md` SN-2) —
**no profile anywhere in this repo requests more than 8 cores / 16GB
RAM**, the hard ceiling above which the challenge scores zero:

| Service | CPUs | Memory | Notes |
| --- | --- | --- | --- |
| `naadap` (default) | 1.0 | 2G | Lowest footprint — best-scoring tier (SN-2). What a bare `docker compose up`/`run naadap` uses. |
| `naadap-4c8g` | 4.0 | 8G | Mid tier. |
| `naadap-8c16g` | 8.0 | 16G | Top of the scored range — the ceiling itself, not an example of exceeding it. |
| `naadap-llm` | 4.0 | 8G | Same ceiling, with the optional LLM step enabled (see below). |

An operator deploying outside Compose (e.g. directly on an IL4
orchestrator) should apply the equivalent CPU/memory limit at that
platform's own resource-limiting mechanism (Kubernetes
`resources.limits`, ECS task size, etc.) — the number that matters is
the ceiling, not the Compose syntax.

## Network policy (NFR-500 / NFR-510 / SN-3)

- **Default (LLM step disabled, the default):** `network_mode: none`
  in Compose — the container has **zero** network access, because the
  default code path makes zero outbound connections by construction
  (`src/Naadap.Cli/Program.cs`'s `RunOptionalLlmStep` returns before
  constructing any HTTP client unless the step is explicitly enabled).
  This satisfies SN-3's "must not call any external service" for the
  default deployment mode without relying on a firewall rule alone.
- **LLM step enabled (`naadap-llm` profile):** the only profile with
  networking attached (default bridge networking). Every outbound
  call target is restricted at the application layer to the
  operator-supplied allowlist (`NAADAP_LLM_ALLOWED_ENDPOINTS`, enforced
  by `Naadap.LlmStep.AllowlistEnforcingModelClient`) — this is where
  the operator supplies the actual USN-approved model/microservice
  endpoint(s); this codebase deliberately does not hardcode a vendor
  endpoint, since only the deploying IL4 environment knows what's
  actually approved.

## Enabling the optional LLM step

Off by default (NFR-510, CORE-250). To enable it, an operator sets:

| Environment variable | Purpose |
| --- | --- |
| `NAADAP_LLM_ENABLED=true` | Explicit opt-in gate (equivalent to the CLI's `--enable-llm-step` flag). |
| `NAADAP_LLM_ENDPOINT` | The single model/microservice endpoint this run is configured to call. |
| `NAADAP_LLM_ALLOWED_ENDPOINTS` | Comma-separated USN-approved allowlist. `NAADAP_LLM_ENDPOINT` must appear in this list or the step is skipped (fails closed, not open — see `src/Naadap.LlmStep/LlmStepConfig.cs`). |
| `NAADAP_LLM_MODEL` | Model identifier, if the endpoint's API distinguishes multiple models. |
| `NAADAP_LLM_API_KEY` | Credential for the endpoint, if required. Never logged or written to any run artifact. |

Token spend is hard-capped at 50,000 tokens/run
(`LlmStepConfig.Sn2TokenBudgetCeiling`, SN-2) — not operator-raisable,
since raising it would silently violate the requirement it exists to
enforce.

Example:

```bash
NAADAP_LLM_ENDPOINT=https://approved-model.example.mil/v1/chat \
NAADAP_LLM_ALLOWED_ENDPOINTS=https://approved-model.example.mil/v1/chat \
NAADAP_LLM_MODEL=approved-model-name \
NAADAP_LLM_API_KEY=*** \
  docker compose --profile llm run --rm naadap-llm
```

## IL4 deployment (SN-3)

The image has no dependency on any specific cloud provider or
orchestrator — it is a standard OCI image with no host-network,
privileged-mode, or GPU requirement. Deploying it into a U.S.
Government-owned/operated IL4-accredited cloud is a matter of that
environment's own image-registry and container-scheduling process;
nothing in this image assumes internet reachability beyond the
explicitly-configured LLM allowlist above, and the default
(LLM-disabled) mode requires no network reachability at all.

## Volumes / input-output contract

Every Compose service mounts:

- `${NAADAP_INPUT_DIR:-./input}` → `/data/in` (read-only)
- `${NAADAP_OUTPUT_DIR:-./output}` → `/data/out` (read-write)

See the root `README.md` for the expected contents of the input
directory and a worked example, and `docs/RTVM.md` OUT-440 / this
repo's `RunManifest` shape for the output bundle's contents.
