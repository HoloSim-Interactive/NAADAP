# Sources, authority, currency, and what could not be reached

## Authority ladder

When sources disagree, higher wins. Verify any deadline, threshold, or
legal claim against a primary source before relying on it in a deliverable.

1. The specific solicitation, contract, or vehicle ordering guide.
2. Statute: 10 U.S.C., 41 U.S.C., 15 U.S.C. (Small Business Act), NDAAs.
3. FAR and the agency supplement (DFARS, PGI, NMCARS) on acquisition.gov.
4. Other CFR titles (13 CFR 121/125 SBA; 32 CFR).
5. DoD issuances (DoDI 5000.74, 5000.88, 1100.22), NAVAIR instructions
   (4355.19), official agency program pages, final rules.
6. OMB memoranda (M-19-13).
7. NIST publications.
8. GAO and Court of Federal Claims decisions.
9. Award data (USAspending, FPDS, SAM.gov, CPARS).
10. Trade press and law-firm bulletins (Federal News Network,
    Washington Technology, GovConWire, Breaking Defense).
11. Common practice (Shipley, AcqNotes, DAU/WarU guides).
12. This skill's reference files.

## Source roles

- **eCFR**: codified baseline, not the official legal edition; often behind
  a bot challenge, so use acquisition.gov or law.cornell.edu mirrors.
- **acquisition.gov**: FAR, DFARS, PGI, NMCARS, and posted class
  deviations. A deviation's model text is not operative for an agency
  merely because it is published.
- **Federal Register**: rulemaking history and effective dates; keyless
  JSON API works.
- **Regulations.gov**: dockets and comments; stakeholder evidence, not
  authority.
- **USAspending / SAM.gov Contract Awards**: ground truth for which vehicle
  absorbed a requirement (Referenced IDV PIID).

## Currency stamps

All figures in these references were gathered on 2026-09-04, then a second
pass on 2026-09-14 verified many of them against primary documents the
client supplied directly (see below), and a third pass on 2026-09-15
resolved eight more newly-supplied documents (see "Second round" below).
Known moving parts: FAR inflation adjustments (last 1 Oct 2025);
SeaPort-NxG ordering ends 1 Jan 2029 with a follow-on RFI under way;
whether SeaPort-NxG task-order activity has moved to PIEE remains
unconfirmed even after two more documents were checked (see
contract-vehicles.md); NITAAC last order 29 Oct 2026; SEWP VI go-live 1
Nov 2026; ENCORE III option end May 2027; RS3 and ITES-3S ending 2027;
Alliant 2 sunset Jun 2028; NAWCTSD TSC IV ends Nov 2027, FTSS V ends Aug
2030, PACRM ends Apr 2028; the DON PAE reorganization of May 2026 is still
settling; GenAI.mil's model roster changes monthly. Re-verify before any
figure enters a deliverable, and record the date of verification next to
it.

## Client-supplied primary sources (2026-09-14)

The client supplied 26 documents directly, in `.claude/skills/
gov-acquisition-sme/sources/`, covering most of the items this file had
listed as blocked plus several not previously requested. Four parallel
reviews checked every document against what this skill claimed from
secondary sources. Full findings, including page/block-level detail not
repeated here, are preserved outside the repo in the session's research
notes; the corrections themselves are already folded into the other
reference files. This section records what was resolved, what a document's
own fetch quality was, and one standing file-mapping error that is now
fixed.

**Resolved — real content obtained and cross-checked, corrections applied
throughout the other reference files:** DoDI 5000.74 and DoDI 5000.88 (the
real esd.whs.mil PDFs), DD Form 1423 (the real FEB 2024 form), a real
filled IPMDAR CDRL example, NAVAIRINST 4355.19E and the SETR Process
Handbook v1.0 (both full text, both searched directly for "PEDDAL" — zero
hits, confirming the standing open question against primary sources rather
than web search), DAG Chapter 10, the NAWCTSD MAC list page (revealed two
vehicles, FTSS V and PACRM, not previously in the catalog, plus full
awardee rosters for TSC IV/FTSS V/PACRM), the NAVAIR 2023 LRAE spreadsheet
(corrected a materially wrong claim — see navair-navy-context.md), the DON
CIO's 06 Sept 2023 GenAI/LLM guidance memo, the DAF "Guide to AI for
Contracting Officers" (Oct 2024), the DON CIO ESL tag-results page, the DoD
IGCE Handbook for Services Acquisition (resolved with a major dating
correction — it is Dec 2017/Feb 2018 content, re-posted not revised in Oct
2025), the DON Best Practices Handbook for the Management of Services
Acquisitions (FY2024) — see the file-mapping fix below — and GSA's
government-wide Acquisition Playbook Module 2 on Implementing Category
Management (not previously requested, genuinely new and useful).

**File-mapping fix.** This file previously mapped the item
"`sources/don-services-acquisition-handbook.pdf`" to the saved filename
`igce-handbook-oct2025_...pdf`. That mapping was wrong — the IGCE handbook
is not a services-acquisition-process document. The actual DON services
acquisition handbook is `DON Best Practices Handbook for the Mgmt of
Services Acquisitions (FY 2024).pdf`; the IGCE handbook is its own,
correctly distinct source. Treat any earlier reference to "the DON services
acquisition handbook" as pointing to the FY2024 handbook, not the IGCE one.

**Fetched but not usable — saved a near-empty page instead of real
content**, both apparently captured during an attempted-but-blocked fetch
rather than supplied deliberately: `navairinst-4355-19e.pdf` (the
3.59MB/5-page copy — a bare waru.edu homepage capture with zero
4355.19E content; the separate 2.64MB/61-page copy with the full filename
is the genuine instruction and is what "resolved" above refers to) and
`navair-osbp.pdf` (a 3-page nav-chrome-only capture of the OSBP homepage,
no small-business process content, no DoDAAC table). OSBP organizational
facts instead came from the NAWCTSD MAC list page, which fetched
correctly. **Still effectively blocked**, despite a file being present:
`don-sbir-page.pdf` (the navysbir.com homepage/news snapshot — no Phase
III, data-rights, or J&A content; the Navy SBIR/STTR Phase III Guidebook
Ver 2 (Mar 2020) is named on the page but not supplied, and remains the
right primary source to seek) and `don-category-management-tiers.pdf` (a
browser print of the DON CMPO homepage whose FAQ answers are JS-rendered
and were not captured — no tier definitions, no NAVAIR-vehicle tier
ratings; GSA's Playbook Module 2, above, is what actually supplied the
verbatim tier language).

**Two distinct files behind similar names**, not duplicates:
`doncio-esl-memos.pdf` is the real DON CIO ESL tag-results page (resolved,
confirms the 2012 ELA mandate almost verbatim, adds Oracle/Microsoft ELA
dates); `doncio-esl-memos_...IT Policy & Guidance.pdf` is a different
capture, the general DON CIO policy index (title-only listing across all
topics) — useful only incidentally, for surfacing that newer 2026 DON CIO
AI-adoption memos exist by title (content still unread).

**Not supplied at all, as of 2026-09-14** (no filename mapped to these two
rows from the original request): the SeaPort-NxG official portal capture
and the DFARS PGI 237.102-74 services-taxonomy spreadsheet. **Both
resolved 2026-09-15** — see "Second round" immediately below.
navair.navy.mil, seaport.navy.mil, secnav.navy.mil, acq.osd.mil, and
doncio.navy.mil (beyond the ESL/GenAI items above) remain generally
unreachable from this environment directly; what is known about these
domains now comes entirely from client-supplied captures.

## Second round of client-supplied primary sources (2026-09-15)

Eight more documents landed in `sources/`, reviewed by two parallel
passes plus direct extraction of the SBIR guidebook. Full per-document
findings, including OCR/extraction method notes, are preserved outside the
repo in the session's research notes; corrections are folded into the
other reference files.

**Resolved, real content, corrections applied:**
- **DFARS PGI 237.102-74 taxonomy memo** (`DFARS PGI 237.102-74 taxonomy
  spreadshee USA004219-12-DPAP.pdf`) — a scanned/image PDF requiring OCR
  (tesseract); the real 27 Aug 2012 OUSD(AT&L)/DPAP memo. **Confirms** the
  skill's 9-services-group list exactly and adds the 7 S&E groups, full
  16-group/70-portfolio structure, and per-group PSC/category/portfolio
  counts — see glossary.md. Drop any earlier "not obtained"/"unverified"
  framing for this taxonomy.
- **OMB M-25-31** (18 Jul 2025, "Consolidating Federal Procurement
  Activities," fully text-extractable) — read in full; does not redefine
  SUM tiers, does not supersede M-19-13, implements EO 14240 via two
  GSA-centralization workstreams (pending FAR 8.004 mandatory-use
  amendment; requirements/organizational centralization to GSA). See
  far-consolidation-rules.md for the full correction — the earlier
  "unread"/"Category Management Policy" framing was wrong on both counts.
- **MIL-HDBK-46855A** — confirmed relevant, but the guessed title was
  wrong: it is "Human Engineering Program Process and Procedures" (17 May
  1999), guidance-only per its own foreword, not "Human Engineering
  Requirements for Military Systems" (that is MIL-STD-1472). Contributed
  the DI-HFAC DID list and a "-HDBK- cited as a requirement" drafting-error
  signal — see requirements-documents.md.
- **NAVAIR OSBP FAQ Sheet** (`NAVAIR OSBP small-business content NAVAIR
  FAQ Sheet_Final.pdf`, Public Release 2022-656) — genuinely useful real
  content, unlike the earlier nav-chrome-only OSBP capture. Strongly
  confirms the DoDAAC table (all six UICs verbatim) and adds "COMFRC" as
  N68520's short name; contributes NAVAIR Enterprise definition, OSBP
  mission, and the small-business registration URL — see
  navair-navy-context.md.
- **Navy SBIR/STTR Phase III Guidebook Ver 2 (Mar 2020)** — genuine
  content this time, unlike the earlier `don-sbir-page.pdf` homepage
  snapshot; 34 pages, confirmed genuine by PDF metadata (created 24 Mar
  2020). Substantially expanded the SBIR Phase III entry in
  contract-vehicles.md: the SBA PD 4(c)(7) sole-source mandate, why no J&A
  is required (15 U.S.C. 638(r)(4)), the DFARS 252.227-7018 data-rights
  clause and its no-waiver/mandatory-flow-down rules, and the five-question
  Phase III eligibility check for PMs/DPMs/KOs.

**Checked but still not corroborating the PIEE claim:**
- **"SeaPort-NxG official portal on SAM.gov.pdf"** — this is the exact
  SAM.gov page the client flagged as corroboration; it landed and turned
  out to be the original 2018 SeaPort-NxG base-solicitation notice
  (Notice ID N0017818R7000), silent on PIEE.
- **"SeaPort-NxG official portal_...Contract for Engineering and Program
  Management.pdf"** — not a Navy or PIEE page despite the filename; a
  third-party contractor marketing page (eTRANSERVICES Corp, © 2023),
  also silent on PIEE. Did add a few minor confirmed/new facts (example
  base contract N00178-19-D-7621, 5-year task-order periods, unlimited
  teaming partners) and flagged a "labor hour terms" discrepancy against
  the skill's "no T&M" claim — see contract-vehicles.md.

**Fetched but low-value, parallel to the earlier navair-osbp.pdf issue:**
`NAVAIR OSBP small-business content Contracting _ SAM.gov.pdf` — a
generic SAM.gov "Contracting" help/navigation page with no NAVAIR or OSBP
content at all; appears to have been captured while looking for NAVAIR
OSBP contracting detail but landed on SAM.gov's generic hub page instead.

## Prior art this skill draws on

- 1102tools/federal-contracting-skills and acqagent/skills (MIT): boundary
  rules, reserved-decision registry, hard stops, deterministic clause
  checking, professional-product standard.
- danielkinneyspears/govcon-pursuit-brain (Apache-2.0): authority ladder,
  freshness stamps, knowledge taxonomy.
- NPS ARP SYM-AM-25-316 (Nangia et al., 2025): requirement-by-requirement
  grading with certainty scores for Navy acquisition-package review.
- AIRC/IDA TR-005 (2026) and WRT-1097: multi-agent decomposition and the
  warning that generated citations cannot be trusted.
- NPS/AFICC PSC prediction (Muir, Westermeyer, Reich, 2021): character CNN,
  hierarchical decoding, ONNX serving.
- GSA Solicitation Review Tool, 18F Discovery, GSA MRAS: the only public
  federal pipelines that ingest SAM attachments or recommend vehicles.
- GAO-14-36, GAO-21-40, GAO-25-108638; CRS R41133 and IF12374.

No published skill or agent definition for a general federal acquisition
SME existed as of the research date. This one is the first that covers
the consolidation and vehicle-recommendation problem.

## Sources that could not be reached from the research environment

These returned 403, 503, a WAF rejection, a bot challenge, or a
JavaScript-only shell as of the original 2026-09-04 research pass. The
client-supplied-sources section above resolves several of these directly
(marked below); the rest are still open — a person with a browser or a
.mil-permitted network can supply them. Highest value first.

| Source | What it holds | Result |
| --- | --- | --- |
| navair.navy.mil, all HTML (LRAF/LRAE page, OSBP, NAWCTSD MAC list, news) | **Partly resolved 2026-09-14**: NAWCTSD MAC list and the LRAE spreadsheet were supplied directly and are now in the skill; the OSBP page and one 4355.19E copy were re-attempted and only returned nav-chrome (see "fetched but not usable" above) | 403 (live); nav-chrome-only on retry |
| seaport.navy.mil | SeaPort-NxG official portal, functional-area text, ordering offices — **partly resolved 2026-09-15**: two client-supplied captures (a 2018 SAM.gov base-solicitation page and a 2023 third-party vendor page) landed but neither is the live seaport.navy.mil portal itself and neither addresses the PIEE transition question; the NAVSEA brief already in the skill remains the best source for vehicle mechanics | 503 |
| secnav.navy.mil (DASN(P), Category Management office, SBIR, LRAE hub, SeaPort brief) | **Partly resolved 2026-09-14**: the DON Best Practices Handbook for Services Acquisitions FY2024 was supplied directly and covers CSWG/SRRB/CM/SeaPort-waiver content; the CMPO homepage itself still only yields JS-rendered content, and the SBIR page is a content-free homepage snapshot | WAF or captcha (live); JS-only on retry |
| doncio.navy.mil | **Resolved 2026-09-14** for the GenAI/LLM guardrails memo (06 Sep 2023) and the ESL tag-results page, both supplied directly; a general policy index page was also supplied (title-only, confirms newer 2026 AI memos exist without their content) | WAF (live); resolved via direct supply |
| dau.edu / waru.edu | **Resolved 2026-09-14** for NAVAIRINST 4355.19E, the Naval SETR Handbook, DAG Chapter 10, and the "Guide to AI for DAF Contracting Officers" (Oct 2024) — all supplied directly and now cited throughout. | 403 (live); resolved via direct supply |
| acq.osd.mil (DPC) | The DoD IGCE Handbook was supplied directly (**resolved, with a dating correction** — see requirements-documents.md). The 2012 DFARS PGI 237.102-74 taxonomy memo was also supplied and **resolved 2026-09-15** (see glossary.md) — a scanned OCR'd copy, not from this domain directly, but the same memo. The category-management page and the PSC quick guide are still not supplied. | 503 |
| esd.whs.mil | **Resolved 2026-09-14**: the real DoDI 5000.74, DoDI 5000.88, and DD Form 1423 PDFs were supplied directly, replacing the AcqNotes mirror previously used | 403 (live); resolved via direct supply |
| navsea.navy.mil, navfac.navy.mil, navsup.navy.mil, niwcatlantic.navy.mil | LRAFs, NAVFAC environmental contract list, NSWCDD forecasts — still not supplied | 403 |
| esi.mil | DoD ESI agreements | 503 |
| ecfr.gov, federalregister.gov developer docs | Title 48 text | bot challenge (mirrors used) |
| army.mil (two CamoGPT contracting articles), acc.army.mil EXPRESS | Army AI-in-contracting use cases | 403 / 429 |
| rand.org RR-1704, apps.dtic.mil (AD1146124, others) | RAND services-inventory report, AFIT extraction thesis | 403 |
| sam.gov opportunity pages, buy.gsa.gov, acquisitiongateway.gov resources, usaspending download pages | JavaScript shells; the APIs work, the pages do not | no content |
| GitHub code search API | SKILL.md sweep across GitHub | 503 (topic pages used instead) |
| federalnewsnetwork.com (three articles), nationaldefensemagazine.org, asksage.ai, actiac.org, dl.acm.org | AI-in-procurement commentary | 403 / Incapsula |
| hallways.cap.gsa.gov, asap.gsa.gov | decommissioned | DNS |
| NCMA PromptMaster, wingovsolutions | JS-only prompt libraries | no content |

Web-search budgets were exhausted in every research track, so the
following were left unverified: the current Acquisition Gateway BIC list,
SEWP VI fee percentage, OTA consortium ceilings, AMCOM EXPRESS ceiling,
RS3 and ITES-4S follow-ons. **Update, 2026-09-14:** the nine defense-centric
category names are now confirmed, but from DoDI 5000.74's own "Federal
Category Structure" (Figure 3) — a different, primary-sourced 19-category
list (see glossary.md), not the DFARS PGI 237.102-74 "DoD services
portfolio groups" this file previously cited from a secondary source.
**Update, 2026-09-15:** the PGI 237.102-74 taxonomy memo itself was
subsequently obtained and OCR'd (see "Second round" above) — its 9-group
services list is now independently confirmed, matching what this file
previously carried from a secondary source. **Do not treat the two lists
(PGI 237.102-74 and DoDI 5000.74 Fig. 3) as interchangeable** — that
caution stands, but now because the two primary sources are 27 years
apart in origin and neither cross-references the other, not because
either was unverified.
