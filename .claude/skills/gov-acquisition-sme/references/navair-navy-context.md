# NAVAIR, NAWCAD, and Department of the Navy acquisition context

Use this when a document, requirement, or vehicle needs to be placed inside
the Navy's actual organization and process. Facts below carry a source;
anything marked *unverified* must not be stated as fact in a deliverable.

## Who buys what (organization and contracting offices)

- **NAVAIR** (Naval Air Systems Command) is a Head of Contracting Activity
  under NMCARS 5201.601-90. Its contracting competency is **AIR-2.0**
  (Contracts); research and engineering is **AIR-4.0**; AIR-4.1 supplies
  independent Technical Review Board chairs for SETR reviews.
- **NAWCAD** (Naval Air Warfare Center Aircraft Division) sites: Patuxent
  River MD, Lakehurst NJ, and Orlando FL (NAWCTSD, the Training Systems
  Division, which is Tech Grove's parent organization). **NAWCWD** is the
  Weapons Division (China Lake / Point Mugu CA).
- **PGIL** = Procurement Group Innovation Lab, the NAVAIR procurement cell
  that predetermines the challenge's correct answers. No public page exists.
  It is not the DHS Procurement Innovation Lab.
- Contracting offices by DoDAAC, with their share of the 2019 command-wide
  Long Range Acquisition Forecast (LRAF, about 1,200 rows):

| DoDAAC | Activity | LRAF rows |
| --- | --- | --- |
| N00019 | NAVAIR HQ, Pax River (platform and weapons buys for PEOs/PMAs) | 706 |
| N68335 | NAWCAD Lakehurst (aircraft launch/recovery, support equipment) | 320 |
| N00421 | NAWCAD Patuxent River (NAWCAD contracts dept; BAAs, RAPID, PSMI, SCI MACs) | 109 |
| N61340 / N61339 | NAWCTSD Orlando (training systems) | 99 |
| N68936 | NAWCWD China Lake / Point Mugu | 43 |

  A contracting-office code in a document is a strong vehicle-affinity
  signal: N00421 requirements land on NAWCAD MACs, N61340 on NAWCTSD CSS
  vehicles, N00019 on PEO/PMA platform contracts or SeaPort-NxG.

- **2026 reorganization.** On 2026-05-11 the DON stood up Portfolio
  Acquisition Executive Aviation, PAE(A), with deputy portfolios for Carrier
  Strike, Marine Corps Aviation, and Maritime ISR & NC3. Roughly 70% of
  technical, contracting, and sustainment functions are moving from the
  SYSCOMs into PAEs, with a mandate for commercial solutions, MOSA, and
  faster mechanisms such as OTAs. Legacy PEO names (PEO(T), PEO(A),
  PEO(U&W), PEO(CS), PEO(JSF)) and PMA-numbered program offices still
  appear in documents; treat them as requirement owners migrating into
  PAE(A). Source: DVIDS 564931; Seapower; Breaking Defense (May 2026).

## The Long Range Acquisition Forecast is a ready-made feature schema

The LRAF columns are exactly the attributes a vehicle recommender should
reason over. NAVAIR command LRAF: title, estimated dollar threshold,
anticipated procurement method (full and open, SB set-aside, 8(a),
sole source), projected contracting office, solicitation and award
quarter, incumbent contract number and contractor, place of performance,
requirement description, NAICS, requirement organization (PEO/PMA). NAWCAD
LRAF adds: group, department code (4.x), dollar band ($10-50M, $50-100M,
over $100M), **current vehicle type and anticipated vehicle type** ("SA
IDIQ", "SeaPort", "MAC"), PSC (R425, J059...), competition type. Field
activities post LRAFs to SAM.gov (NAWCAD FY26 LRAF posted 2026-02-24,
quarterly updates).

## Navy-specific rules that decide where a services requirement goes

- **SeaPort-NxG is mandatory consideration.** NMCARS 5237.102: considering
  SeaPort for the functional areas in NMCARS Annex 22 is mandatory, except
  FAR 6.302 sole-source actions, below-SAT buys, 8(a) set-asides, FAR Part 12
  commercial, and Part 13 simplified. Not using SeaPort requires a D&F to
  DASN(P) (5237.103). For any Navy professional-services requirement, the
  first question is therefore "why not SeaPort-NxG?"
- **Consolidation approvals.** NMCARS 5207.107-2 (consolidation
  determinations by DASN(P) or HCA) and 5207.107-3 (bundling, via DASN(P))
  sit on top of DFARS 207.170 ($2M DoD consolidation threshold) and FAR
  7.107. A consolidation recommendation is not free: it triggers a written
  determination and small-business impact analysis.
- **Services oversight tiers.** NMCARS 5237.5: MOPAS-S below $50M total or
  $25M per year; PSTRAP-M / ISTRAP-M above; Services Acquisition Workshop
  at $500M total or $250M per year (5237.192). Acquisition-strategy formats
  (STRAP) are NMCARS Annexes 17 through 20. Approval: DASN(P) at or above
  $250M, HCA/PEO below (5207.103(j)).
- **Small business record.** Market research and set-aside outcomes are
  documented on DD Form 2579 through the SBCR application (NMCARS 5219.201,
  SeaPort excepted), with SBA PCR review. Sources-sought responses drive
  the LRAF's "anticipated procurement method".
- **Contractor Support Services (CSS).** In NAVAIR usage CSS means
  contractor support services bought at enterprise scale (NAWCTSD CSS
  N6134019R0061; NAWCAD SCI MAC). Workforce-mix limits come from DoDI
  1100.22 (inherently governmental and closely associated functions) and
  DFARS 207.5. The Navy's enterprise answer to CSS proliferation is
  SeaPort-NxG plus HCA-level MACs; a 2026 NAWCAD RFI sought a five-year
  services acquisition strategy (outcome-based contracting, less contract
  administration, faster awards, more competition). The prize challenge is
  the same policy impulse.
- **Category management.** The DON Category Management Program Office under
  DASN(P) manages tier-rated contracts (OMB tiers 0 to 3; Tier 3 is
  Best-in-Class) per OMB M-19-13. Express vehicle recommendations in that
  vocabulary: spend-under-management tier, government-wide category and PSC,
  mandatory-consideration status, HCA ownership, ordering eligibility.

## Representative NAVAIR/NAWCAD strategic vehicles

- **SeaPort-NxG**: DON-wide MAC IDIQ, 22 professional-services functional
  areas (Annex 22), about 1,800 primes, task orders competed; ordered by
  NAVAIR, NAVSEA, NAVWAR and others.
- **NAWCAD SCI MAC**: about $249M, five years, 40 to 41 awardees (15 small),
  N00421, "support services at multiple classification levels for all
  aspects of the acquisition life cycle" across NAVAIR. A textbook
  enterprise CSS vehicle.
- NAWCAD WOLF RAPID MAC (N00421-19-R-0074), PSMI MAC (N0042121R0115), ASI
  MAC; NAWCAD Mission Systems Software Engineering single-award IDIQ
  (recompete 2026, PSC R425).
- NAWCTSD CSS (N6134019R0061); NAVAIR Basic Ordering Agreements
  (e.g., N00019-20-G-0005).
- **NASC** (Naval Aviation Systems Consortium) OTA: 600+ members, five-year
  OTA awarded 2019, administered by NAWCAD Pax, open to all NAWCs, Fleet
  Readiness Centers, and NAVAIR contracting offices. The OTA path for
  prototypes and follow-on production (10 USC 4022(f)).
- Government-wide Best-in-Class vehicles (GSA MAS, OASIS+, Alliant,
  NASA SEWP, NIH CIO-SP) are candidates when Navy category management pushes
  spend to tiered solutions. See contract-vehicles.md for the catalog.

## SETR, the review process the client's documentation follows

NAVAIRINST 4355.19D para 4.d lists fourteen essential reviews: ITR, ASR,
SRR-I, SRR-II, SFR, SSR, PDR, CDR, IRR, TRR, FRR (airborne), SVR/PRR, PCA,
ISR. Non-ACAT programs conduct at minimum SRR, PDR, CDR, SVR. Programs also
hold IBRs, OTRRs, and TRAs. Baselines: SRR sets the performance baseline,
SFR the functional, PDR the allocated, CDR the initial product, SVR/FCA
verifies the product baseline, PCA finalizes it. Reviews are event-driven,
entered when SEP entrance criteria are met, chaired by an independent TRB
chair, and produce a Technical Review Summary Report (attendees,
presentations, updated risk assessment, signed RFAs, minutes,
recommendation) plus the completed checklist. 4355.19E (2015) is the latest
revision but is hosted only on the DAU/WarU site, which blocks fetches;
4355.19D full text was obtained.

Acronym traps: in 4355.19D "SDD" means System Development and
Demonstration, the pre-2015 EMD phase, not Software Design Description
(DI-IPSC-81435A). Confirm which the client means. **"PEDDAL" is defined in
no source found** (not in 4355.19D, the Naval SETR Handbook, DoDI 5000.74/
5000.88 results, MIL-HDBK-245D, or acronym databases). The nearest real
term phonetically is the pre-EMD review. Never invent an expansion; ask.

## What "USN-approved models or microservices within an IL4 environment" can mean

- **GenAI.mil** is the DoW enterprise generative-AI platform (IL5, CUI). A
  28 Jan 2026 memo from ASN(RDA), PMD, and DON CIO made it the DON
  Enterprise IT Service for generative AI, with transition by 30 Apr 2026.
  Models offered have included Gemini for Government, Grok for Government,
  ChatGPT (Feb 2026), with Claude announced.
- **Mission-owner tenants** with DoD IL4/IL5 provisional authorizations:
  Azure OpenAI in Azure Government (IL4/IL5, later IL6) on Flank Speed;
  Amazon Bedrock in AWS GovCloud (FedRAMP High and IL4/IL5 for Claude,
  Llama, and later GPT, Nemotron, Mistral, Cohere, Titan embeddings, Grok).
- **Locally packaged open-weight models** inside the container.
- **No LLM at all**, which the rubric rewards most.

Excluded by the rules: any call to a commercial SaaS endpoint. NIPRGPT was
sunset 2025-12-31; do not target it. DON GPT (Flank Speed) is legacy or
transitioning after the GenAI.mil mandate. Impact levels: IL4 covers CUI
and non-CUI mission-critical data; IL5 adds higher-sensitivity CUI and
national security systems (DoD Cloud Computing SRG).

## Navy document conventions worth recognizing in text

- SOW structure per MIL-HDBK-245D: Scope, Applicable Documents,
  Requirements, with tasks in section 3 cross-referenced to CDRLs. PWS per
  FAR 37.6 with a Performance Requirements Summary and QASP. SOO in
  solicitations.
- NAVAIR services solicitations: Section C SOW/PWS, Exhibit A CDRLs (one
  DD Form 1423-1 per data item; Item 4 is the DID number, Item 5 the SOW
  paragraph), Sections L and M, NMCARS and DFARS clauses.
- DIDs common on NAVAIR CDRLs: DI-MGMT-81861 (IPMR), DI-SESS-81785 (SEP),
  DI-MISC-80508 (technical report), DI-IPSC-81435A (SDD), DI-IPSC-81433A
  (SRS). MBSE and SysML use follows NAVAIR digital engineering guidance and
  SEP Outline 4.1 Appendix E.
- NAVAIR's Small Business office documents sources-sought outcomes; NAWCAD
  Lakehurst holds an annual small-business industry day; NAWCTSD publishes
  a virtual PALT (Procurement Administrative Lead Time) meeting.
