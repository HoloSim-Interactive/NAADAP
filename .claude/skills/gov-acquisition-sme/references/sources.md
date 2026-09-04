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

All figures in these references were gathered on 2026-09-04. Known moving
parts: FAR inflation adjustments (last 1 Oct 2025); SeaPort-NxG ordering
ends 1 Jan 2029 with a follow-on RFI under way; NITAAC last order 29 Oct
2026; SEWP VI go-live 1 Nov 2026; ENCORE III option end May 2027; RS3 and
ITES-3S ending 2027; Alliant 2 sunset Jun 2028; NAWCTSD TSC IV ends Nov
2027; the DON PAE reorganization of May 2026 is still settling; GenAI.mil's
model roster changes monthly. Re-verify before any figure enters a
deliverable, and record the date of verification next to it.

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
JavaScript-only shell. A person with a browser or a .mil-permitted network
can supply them. Highest value first.

| Source | What it holds | Result |
| --- | --- | --- |
| navair.navy.mil, all HTML (LRAF page, OSBP, NAWCTSD MAC list, news) | NAVAIR LRAF Excel, MAC lists, vehicle news; PDFs under /sites/g/files/ do download | 403 |
| seaport.navy.mil | SeaPort-NxG official portal, functional-area text, ordering offices | 503 |
| secnav.navy.mil (DASN(P), Category Management office, SBIR, LRAE hub, SeaPort brief) | DON category management tier lists, CSWG charter, services handbook | WAF or captcha |
| doncio.navy.mil | GenAI guardrails, ESL memos | WAF |
| dau.edu / waru.edu | NAVAIRINST 4355.19E, Naval SETR Handbook, DAG Chapter 10, services taxonomy artifact, IGCE handbook, "Guide to AI for DAF Contracting Officers" (Oct 2024) | 403 |
| acq.osd.mil (DPC) | DoD services taxonomy spreadsheet and 2012 memo, IGCE Handbook Oct 2025, category-management page, PSC quick guide | 503 |
| esd.whs.mil | DoDI 5000.74 and 5000.88 PDFs, DD Form 1423 | 403 (AcqNotes mirror used) |
| navsea.navy.mil, navfac.navy.mil, navsup.navy.mil, niwcatlantic.navy.mil | LRAFs, NAVFAC environmental contract list, NSWCDD forecasts | 403 |
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
RS3 and ITES-4S follow-ons, the nine defense-centric category names, and
the DoD services portfolio-group list beyond DoDI 5000.74's references.
