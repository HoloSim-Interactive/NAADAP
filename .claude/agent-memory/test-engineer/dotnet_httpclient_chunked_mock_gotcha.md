---
name: dotnet-httpclient-chunked-mock-gotcha
description: when standing up a throwaway HTTP mock server to test a .NET HttpClient/JsonContent call path end-to-end, the mock must handle chunked Transfer-Encoding, not just Content-Length
metadata:
  type: feedback
---

.NET's `HttpClient` + `JsonContent.Create(...)` (as used by e.g.
`Naadap.LlmStep.HttpModelClient`) sends the POST body with
`Transfer-Encoding: chunked` rather than a `Content-Length` header, at
least for local/loopback HTTP/1.1 requests in this sandbox. A quick
mock server built on Python's `http.server.BaseHTTPRequestHandler`
that only reads `Content-Length` will see an empty body and throw a
JSON-decode error server-side, which surfaces client-side as a
confusing `HttpIOException: The response ended prematurely
(ResponseEnded)` — easy to misread as a product bug in the client
code under test.

**Why:** hit this verifying TP-250 (issue #10, CORE-250's live-network
path) — `curl` against the same mock worked fine (curl sends
Content-Length), which briefly made it look like a dotnet/sandbox
networking issue rather than a mock-server gap. Confirmed by reading
raw bytes off a plain `socket` server: the request was well-formed
chunked-encoded JSON: the production code was correct all along.

**How to apply:** when building a one-off HTTP mock to exercise a
`HttpModelClient`-style call end-to-end, either (a) parse
`Transfer-Encoding: chunked` explicitly (read hex chunk-size lines
until a `0\r\n\r\n` terminator), or (b) use a mock library/framework
that already handles both encodings, rather than a minimal
hand-rolled `BaseHTTPRequestHandler`. Don't conclude "sandbox network
issue" or "product bug" from a `ResponseEnded`/premature-close error
against a hand-rolled mock until you've read the raw bytes received
server-side.

Separately: in this sandbox, background server processes started via
`nohup ... & disown` inside a compound Bash command are unreliable
(the whole command can report a spurious exit code like 144 without
it being clear whether the background process actually started) —
use the Bash tool's own `run_in_background: true` parameter to start
long-lived helper servers instead, and `kill <pid>` (found via `ps
aux`) rather than `pkill` to stop them, since `pkill` also produced
unexplained exit 144s here even when it matched nothing.
