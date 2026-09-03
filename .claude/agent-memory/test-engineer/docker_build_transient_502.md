---
name: docker-build-transient-502
description: docker build occasionally fails on the first attempt with a docker.io auth 502, unrelated to the Dockerfile itself — retry before concluding it's a real failure.
metadata:
  type: feedback
---

On issue #5 (NAADAP), `docker build .` failed on the first attempt with
`failed to authorize: failed to fetch oauth token: unexpected status
from POST request to https://auth.docker.io/token: 502 Bad Gateway`.
This happened at the `resolve image config for
docker-image://docker.io/docker/dockerfile:1` step, before any of the
project's own Dockerfile instructions ran. A plain retry of the same
command succeeded immediately with no other change.

**Why:** this is Docker Hub's registry/auth layer being flaky in the
sandboxed runner environment, not a defect in the Dockerfile being
tested. Reporting it as a FAIL on the first attempt would have sent a
false failure back to Software Engineer for a problem that doesn't
exist in their code.

**How to apply:** when `docker build` fails specifically at an image
*resolve/pull/auth* stage (not a `RUN`/`COPY` step inside the build
itself), retry once before treating it as a real failure. If it
persists across retries, then it's worth reporting — but note in the
report whether the failure is in Docker's own registry plumbing vs.
the project's build steps, so Software Engineer isn't debugging the
wrong layer.
