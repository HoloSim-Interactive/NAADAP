---
name: samgov-public-api-for-document-sourcing
description: How to search SAM.gov and download real solicitation attachments (PDF/DOCX) from this build environment for validation-corpus fixture work.
metadata:
  type: reference
---

Reachable and usable from this environment (verified 2026-09-03,
issue #6, `HoloSim-Interactive/NAADAP`). Two undocumented-but-live
endpoints, both wanting `Accept: application/hal+json` (they 406 on
plain `application/json`):

1. **Search**: `GET https://sam.gov/api/prod/sgs/v1/search/?index=opp&q=<term>&page=0&sort=-modifiedDate&size=<n>`
   - The `q` param is the only filter that reliably works — it does
     real relevance search against org names/titles (e.g. `q=NAVFAC`
     returns real NAVFAC results). Other params that look like filters
     (`organizationId`, `naics`, `is_active`) are silently ignored —
     confirmed by identical result sets across different values.
   - Each result has an opaque `_id` (the opportunity ID) and
     `organizationHierarchy` (list of org names from DEPT OF DEFENSE
     down to the issuing office) — use org names to filter for
     Navy/NAVFAC/NAVSEA/NAVSUP/NAVAIR after the fact, client-side.

2. **Attachment listing**: `GET https://sam.gov/api/prod/opps/v3/opportunities/{opportunityId}/resources`
   → `_embedded.opportunityAttachmentList[0].attachments[]`, each with
   `type` (`"file"` = real downloadable document, `"link"` = external
   redirect — often to PIEE, see below), `name`, `resourceId`.

3. **Download**: `GET https://sam.gov/api/prod/opps/v3/opportunities/resources/files/{resourceId}/download`
   → HTTP 303 redirect to a presigned S3 URL. `curl -sL` follows it
   fine, no auth needed for public opportunities.

4. **Public opportunity page** (for citing provenance in a README):
   `https://sam.gov/opp/{opportunityId}/view`.

5. **Opportunity detail/metadata**: `GET https://sam.gov/api/prod/opps/v2/opportunities/{opportunityId}`
   → JSON with `data2.title` (the SAM.gov *opportunity-listing* title,
   as entered by the issuing office) plus `description`,
   `solicitationNumber`, `postedDate`, POC info. Useful when a fixture's
   "why picked" rationale needs to cite the opportunity title as a
   fact distinct from the attached PDF's own in-document title —
   they can genuinely differ (issue #6: a SAM.gov listing title named
   a contract vehicle nickname, e.g. "(SBRAC VI)", that the attached
   177-page PDF's own title/body text never mentioned at all — don't
   assume the two are interchangeable when writing a ground-truth
   rationale; check the PDF's actual extracted text before claiming a
   name is "in the document," and cite the SAM.gov title separately if
   that's actually where it comes from).

**Gotcha — NAVAIR specifically**: most NAVAIR opportunities' real
attachments are `type: "link"` pointing at the PIEE Solicitation
Module (`piee.eb.mil`), not `type: "file"` — i.e. not actually hosted
on SAM.gov and not fetchable from this environment. `navair.navy.mil`/
`navsea.navy.mil` are also both HTTP-403-blocked directly (Akamai bot
filtering, pre-existing finding from the RTVM issue). For a Navy/
NAVAIR-flavored corpus that actually needs real downloadable files,
search NAVFAC/NAVSEA/NAVSUP-issuing offices instead — still genuinely
Navy, still produces real SOW/PWS/CDRL attachments as `type: "file"`.

**Non-SAM.gov open-source text**: `https://api.govinfo.gov/search`
(POST, `api_key=DEMO_KEY` works for light use) searches GovInfo's
Congressional Hearings (`CHRG`) collection; each result gives a direct
`pdfLink`. Good source for the "open-source text" document-type
category when a procurement source doesn't apply.

The ground-truth candidate-vehicle taxonomy derived from this corpus
(SeaPort-NxG / NAVFAC MACC-JOC / SBRAC / GSA MAS) lives in
`tests/fixtures/reference-20/ground-truth.json` and
`tests/fixtures/README.md` — read those directly rather than this
memory if a later issue needs to extend the N=20 reference set.
