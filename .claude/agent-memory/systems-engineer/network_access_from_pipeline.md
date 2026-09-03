---
name: network-access-from-pipeline
description: Which external domains are/aren't reachable from this agent pipeline's sandbox, learned while researching NAADAP RTVM sources.
metadata:
  type: project
---

While researching public SOW/PWS/CDRL corpora and the SETR process for
the NAADAP RTVM (issue #2, 2026-09-03):

- `navsea.navy.mil` and `navair.navy.mil` return **HTTP 403** from this
  pipeline's sandbox (Akamai edge block — "Access Denied", reference
  `errors.edgesuite.net`). This looks like origin/bot filtering on the
  `.mil` side, not a content issue — a request from an actually
  permitted network (or the client themselves) would likely succeed.
  Don't burn time retrying user-agent/header tricks against `.mil`
  hosts from here; it isn't a client-detection problem, it's an
  edge-network block.
- General web **is** reachable (`google.com` 200), but major search
  engines (`duckduckgo.com/html`, `bing.com/search`) throw bot
  challenges/anomaly pages against scripted `curl` requests — don't
  rely on scraping search-result HTML for research; go straight to a
  specific known site instead.
- Confirmed reachable and useful for DoD-acquisition-adjacent research:
  `sam.gov` (public opportunities + a working JSON search API at
  `sam.gov/api/prod/sgs/v1/search/`, live solicitation data including
  attachments), `acqnotes.com` (public DoD-acquisition encyclopedia,
  good for SETR/technical-review terminology), `dau.edu`,
  `acquisition.gov`.

**Why:** the client pointed Systems Engineer at a specific
`navsea.navy.mil` page as an example corpus source; it isn't fetchable
by any agent working this pipeline, so any RTVM/SDD work that assumes
live access to `.mil` sources needs a same-content alternative (SAM.gov
attachments, in this case) or a human with a permitted network pulling
it manually.

**How to apply:** before spending effort trying to reach a `.mil`
domain directly from a pipeline run, check this note first — try the
non-`.mil` alternative or flag it to Product Manager as a
human-network task instead of retrying.
