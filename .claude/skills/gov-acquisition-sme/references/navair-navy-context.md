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
- Contracting offices by DoDAAC. Two independently sourced counts exist and
  disagree in absolute numbers (different years, different report scopes) but
  agree on rank order — N00019 dominates, then the NAWCAD sites:

| DoDAAC | Activity | 2019 command LRAF rows | 2023 LRAE rows |
| --- | --- | --- | --- |
| N00019 | NAVAIR HQ, Pax River (platform and weapons buys for PEOs/PMAs) | 706 | 1,075 |
| N68335 | NAWCAD Lakehurst (aircraft launch/recovery, support equipment) | 320 | 434 |
| N00421 | NAWCAD Patuxent River (NAWCAD contracts dept; BAAs, RAPID, PSMI, SCI MACs) | 109 | 198 |
| N61340 | NAWCTSD Orlando (training systems) | 99 | 146 |
| N68936 | NAWCWD China Lake / Point Mugu | 43 | 48 |
| N68520 | Fleet Readiness Centers HQ | not in 2019 report | 68 |

  N61339, paired with N61340 in the 2019 report, does not appear anywhere in
  the 2023 LRAE — unconfirmed whether it was retired or merged. N68520 (FRC
  HQ) appears only in the 2023 data. Both counts are primary-sourced (the
  2019 NAVAIR command LRAF PDF and the 2023-12-18 NAVAIR LRAE Annex 25
  spreadsheet, both in `sources/`); treat the 2023 figures as current.

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

## The Long Range Acquisition Estimate — a real spreadsheet, verified, corrects an earlier claim

**Correction:** an earlier version of this file claimed NAVAIR's forecast
has a "current/anticipated vehicle type" column with values like "SeaPort"
or "MAC" — that claim was built from a different, older command LRAF PDF
found on the web and does **not** match the real document. It is wrong and
is replaced below by what NAVAIR's own 2023-12-18 spreadsheet
(`navair-Dec 18_2023_NAVAIR_LRAE_Report_FINAL.xlsx` in `sources/`) actually
contains.

The document is titled **Long-Range Acquisition Estimate (LRAE), Annex 25**
— not "LRAF." ("Annex 25" likely ties to an NMCARS annex; unconfirmed.) The
NAVAIR-command LRAE holds 2,001 rows (not ~1,200) and its 26 columns are:
Requirement Title, Requirement Description, Associated Program or
Requirement Office, Anticipated Total Value (a banded range like "$7.5M -
$50M", not a point figure), Anticipated Procurement Method, Anticipated
Contract Type (FAR 16), **Anticipated Procurement Instrument (FAR 4.1603)**,
Contracting Office UIC, Anticipated Solicitation FY/Qtr, Anticipated Award
FY/Qtr, Anticipated Period of Performance (months), Follow-on or New,
Existing Contract Number, Incumbent Contractor, Anticipated Place of
Performance, Anticipated NAICS, Anticipated PSC, two POC name/contact
pairs, facility- and personnel-clearance flags, and a free-text Comments
field.

**There is no vehicle-name column.** The closest field, Anticipated
Procurement Instrument, holds award-mechanism types, not vehicle brand
names — by frequency: "C" Type Contract (418), TBD (382), Basic Ordering
Agreement Order (378), Delivery/Task Order not under FSS (292), Single-
Award Indefinite-Delivery (283), Basic Ordering Agreement (186), Other
Transaction Authority (23), Purchase Order (19), BAA contract action (11),
BPA Call/Agreement (9). "SeaPort" appears only incidentally inside nine
free-text Requirement Title strings (e.g. "Logistics Seaport-E CSE (C)").
**A vehicle recommender built against this data has to infer the vehicle
from the title/description text, the same way it does from any other
document — the LRAE gives no structured shortcut.** That is a real
constraint on the design, not a convenience.

What the LRAE is genuinely useful for: `Associated Program or Requirement
Office` is a structured code of the form `<code> - <sub-code> - <office>`
(e.g. `4K0000T - 4.11 - NAWCAD PAX`, `APM299 - APM299 - PEO A`) — a clean
PEO/PMA/competency-code signal the skill did not previously note.
`Anticipated Procurement Method` values (Sole Source 1,306; Full and Open
290; TBD 230; SB Set-Aside 80; 8(a) Sole Source 49; 8(a) Competitive 21;
smaller set-aside categories under 5 each) show most NAVAIR requirements
in this snapshot are sole-source, which matters for consolidation
feasibility. Only 16% of rows have an incumbent contract number filled in
and only 13% name an incumbent contractor — most rows describe new
requirements, not follow-ons, so do not assume every LRAE row has lineage
to trace.

Field activities also post LRAFs/LRAEs to SAM.gov (NAWCAD FY26 posted
2026-02-24, quarterly updates); those postings were not independently
verified to carry the same column set as the command-level file above.

## Navy-specific rules that decide where a services requirement goes

- **SeaPort-NxG is mandatory consideration.** NMCARS 5237.102: considering
  SeaPort for the functional areas in NMCARS Annex 22 is mandatory, except
  FAR 6.302 sole-source actions, below-SAT buys, 8(a) set-asides, FAR Part 12
  commercial, and Part 13 simplified. Not using SeaPort requires a D&F to
  DASN(P) (5237.103). For any Navy professional-services requirement, the
  first question is therefore "why not SeaPort-NxG?" **Where to look for
  that activity is changing in 2026** — see contract-vehicles.md's SeaPort
  entry: task-order solicitations are reportedly moving to PIEE (the
  Procurement Integrated Enterprise Environment) ahead of a staged
  decommissioning of the seaport.navy.mil portal, per a single dated
  secondary source not yet primary-confirmed. The vehicle and its rules are
  unaffected; only the posting system is.
- **Consolidation approvals.** NMCARS 5207.107-2 (consolidation
  determinations by DASN(P) or HCA) and 5207.107-3 (bundling, via DASN(P))
  implement FAR 7.107's $2M consolidation threshold — DFARS 207.170, which
  used to carry a DoD-specific consolidation rule, was deleted in 2018 (see
  far-consolidation-rules.md); consolidation is governed government-wide by
  FAR 7.107 alone. A consolidation recommendation is not free: it triggers a
  written determination and small-business impact analysis.
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
  areas (Annex 22), awarded to approximately **1,870 contractors** in
  December 2018, grown to about 2,400 by the Aug 2024 NAVSEA brief (see
  contract-vehicles.md for the fuller entry). Has generated over
  **$62 billion** in contract actions since FY2001; about 85% of contract
  holders are small businesses. Task orders competed; ordered by NAVAIR,
  NAVSEA, NAVWAR and others.
- **NAWCAD SCI MAC**: about $249M, five years, 40 to 41 awardees (15 small),
  N00421, "support services at multiple classification levels for all
  aspects of the acquisition life cycle" across NAVAIR. A textbook
  enterprise CSS vehicle.
- NAWCAD WOLF RAPID MAC (N00421-19-R-0074), PSMI MAC (N0042121R0115), ASI
  MAC; NAWCAD Mission Systems Software Engineering single-award IDIQ
  (recompete 2026, PSC R425).
- **NAWCTSD's three current MACs** (N61340, Orlando — confirmed against
  `nawctsd-mac-list.pdf`, the command's own vehicle page):
  - **TSC IV**: "Training System design, development, production, test and
    evaluation, delivery, modification, and support." 23 basic contracts,
    contract numbers **N61340-18-D-5001 through -5024** (-5010 skipped) —
    an earlier note here said "N61340-19-D," which was wrong; the award year
    is 2018. Ceiling $980M, awarded November 2018, ordering ends by November
    2027, performance complete by 2029. Full 23-company awardee roster is in
    `sources/TSC IV Awardee Listing 01.13.20.pdf`.
  - **FTSS V** (Fielded Training Systems Support V): "Contractor Operation
    and Maintenance Services and Contractor Instructional Services in
    support of Training Systems." Nine basic contracts — seven unrestricted
    (Lot 1, N6134022D1001-D1007) and two small-business set-aside (Lot 2,
    N6134022D2001-D2002). Ceiling **$1.31B**, larger than TSC IV, awarded
    July 2022, ordering ends by August 2030, performance complete by 2033.
    Not previously in this catalog.
  - **PACRM** (Pilot and Aircrew Curriculum Revision and Maintenance):
    "planned and unplanned revision and maintenance of Pilot and Aircrew
    curriculum for the U.S. Navy, U.S. Marine Corps, and foreign military."
    Four basic contracts, N6134021D0005-D0008. Ceiling $90M, awarded May
    2021, ordering ends by 30 April 2028, performance complete by 2032. The
    narrowest-scope of the three (curriculum content, not systems or
    devices). Not previously in this catalog.
  - All three share the numbering pattern `N61340<YY>D<NNNN>` where YY is
    the award fiscal year (18, 21, or 22) — a clean signal for which of the
    three vehicles an order belongs to.
- NAVAIR Basic Ordering Agreements (e.g., N00019-20-G-0005).
- **NASC** (Naval Aviation Systems Consortium) OTA: 600+ members, five-year
  OTA awarded 2019, administered by NAWCAD Pax, open to all NAWCs, Fleet
  Readiness Centers, and NAVAIR contracting offices. The OTA path for
  prototypes and follow-on production (10 USC 4022(f)).
- Government-wide Best-in-Class vehicles (GSA MAS, OASIS+, Alliant,
  NASA SEWP, NIH CIO-SP) are candidates when Navy category management pushes
  spend to tiered solutions. See contract-vehicles.md for the catalog.

## SETR, the review process the client's documentation follows

**Corrected against the primary source.** NAVAIRINST 4355.19E (FEB 06 2015,
Commander NAVAIR, letterhead AIR-4.0/5.0/6.0, supersedes and cancels
4355.19D) and its companion SETR Process Handbook v1.0 are both in
`sources/` and were read in full. An earlier version of this file cited
4355.19D's "para 4.d" for a list of "fourteen essential reviews" — that
citation was wrong on the paragraph, the count, and the word "essential."

The real list is **4355.19E para 5.d ("SETR Events and Audits"), under
section 5 "Policy"**, introduced as *"a standard set of SETR events and
audits that may be tailored for each program"* — not called essential
anywhere in the instruction or the Handbook. It has **18 events**, not 14:
ITR, ASR, SRR-I, SRR-II, SFR, SSR, PDR (I/II), **RBR** (Release Backlog
Review), CDR, IRR, TRR, FRR, **FCA** (Functional Configuration Audit), SVR,
PRR, PCA, ISR. The earlier 14-item list was missing FCA and RBR entirely,
and collapsed SVR and PRR into one item — they are formally distinct
reviews with separate entry-criteria sections (Handbook §7.1.13 and
§7.1.14), though the instruction's own timing chart does bracket them at a
shared point, which is why the shorthand "SVR/PRR" is common in practice.
RBR is new to the E revision, added for Agile: *"incorporated as a new
SETR event to better manage the software design as it matures through
numerous software releases."* Two other explicit D→E changes are worth
knowing: guidance for Agile/incremental software development was added,
and the Handbook — an enclosure to 4355.19D — became a standalone document
in 4355.19E, which is why it now exists as a separate source.

For rapid-acquisition/non-ACAT programs, the Handbook's own minimum list
(§6.4) is **SRR (I/II), PDR, CDR, and TRR** — not SVR as an earlier version
of this file stated. Tailoring guidance for that path: combine ASR+SSR
into PDR, fold SFR activities into SRR-II or PDR, fold IRR into CDR for
high-off-the-shelf content. Programs also hold IBRs, OTRRs, TRAs, and (per
the Handbook, not the instruction's core list) an MRA (Manufacturing
Readiness Assessment).

Baselines, confirmed: SRR-I/II sets the performance baseline (also tied to
a capabilities/objective baseline at ITR/ASR), SFR the functional, PDR the
allocated, CDR the initial product baseline — and DoDI 5000.88 adds that
the PM assumes control of Class I configuration changes from the
contractor at CDR completion — SVR/FCA verifies the product baseline, PCA
finalizes it. Reviews are event-driven, entered when SEP entrance criteria
are met (three categories per Handbook §5.2: requirements/traceability/
design; test/evaluation/certification; project management and execution),
chaired by an independent TRB chair requested from AIR-4.1 at least 90
days before the review (AIR-4.1 may delegate software-only reviews to
AIR-4.9). Prep lead time scales with ACAT: 4 months for ACAT I-II, 3 for
ACAT III-IV, 2 for non-ACAT. Reviews produce a Technical Review Summary
Report (attendees, presentations, updated risk assessment, signed RFAs on
NAVAIR form 4355/4, minutes, recommendation) plus the completed checklist;
RFAs are triaged Category I (in scope, proceed), II (out of scope, needs
KO direction), or III (rejected), at urgency Level 1 (safety of flight),
2 (mission performance), or 3 (desired only).

Acronym traps: "SDD" is not confirmed as System Development and
Demonstration in the primary sources now in hand (the string does not
appear in either 4355.19E or the Handbook); treat that as unconfirmed, not
retracted, and still confirm with the client which meaning is intended
before relying on it. **"PEDDAL" — closed, 2026-09-15, do not pursue further.** Confirmed absent
from the full text of 4355.19E, the SETR Process Handbook v1.0, and every
other primary source obtained; the client's own recollection is that it
appeared as a deliverable on a prior NAWCTSD project, referring to
engineering design drawings and logistics content, expansion of "P" not
recalled, and it does not appear in any documentation the client could
locate either. The client's working theory is that it may have been an
informal, fleet-procedures-level companion document to the design
drawings rather than a defined SETR artifact or acronym with a formal
source — plausible, and not contradicted by anything found in 4355.19E or
the Handbook, which name no companion-document convention by that name.
Treat "PEDDAL" as client-specific institutional terminology of uncertain
formal standing, not a general Navy or NAVAIR SETR term. If it resurfaces
in project documentation, note it as such rather than researching it
again from scratch.

**NAVAIR's 18-event list is a Component-level superset, not a contradiction
of DoD policy.** DoDI 5000.88 (the DoD-wide engineering instruction, not
Navy-specific) requires only six, "or equivalent," waivable through the SEP
approval process: system requirements or functional review; PDR; CDR;
system verification review or functional configuration audit; production
readiness review; physical configuration audit. NAVAIRINST 4355.19E
tailors that floor up to its own fuller review sequence. When a document
cites "the required technical reviews" without saying which authority, ask
which level — DoD floor or NAVAIR tailoring — since the two lists are not
interchangeable.

## What "USN-approved models or microservices within an IL4 environment" can mean

**The policy has a documented lineage; do not treat it as one undifferentiated
block.** The base layer is the DON CIO's original interim guardrails,
Memorandum "Department of the Navy Guidance on the Use of Generative
Artificial Intelligence and Large Language Models," **06 September 2023**,
signed by Jane O. Rathbun (Acting DON CIO) — full text in `sources/`. It
predates GenAI.mil entirely and sets no formal approval process and no
named-tool list (ChatGPT, Bard, and LLaMA appear only as background
examples of commercial AI, not as approved tools). Its actual rules: no
blanket prohibition, but "commercial AI language models are not recommended
for operational use-cases until security control requirements have been
fully investigated, identified and approved"; non-operational use requires
"a robust review process which includes the critical thinking skills of
human expertise"; data handling is governed by existing policy, specifically
DoDI 5200.48 (CUI), not a new AI-specific rule — "this policy extends to the
use of unregulated AI LLM and includes source code"; AI-generated code
requires review in a controlled non-production environment before use, and
integrating generative AI into an application "warrants consideration of an
ATO assessment"; accountability for misuse "resides with each individual
organization's respective leadership," not a central AI office. The memo
names **Jupiter, the DON Enterprise data and analytics platform**, as the
documented origin point for what later became the DON's enterprise AI
tooling.

That 2023 memo is now superseded-looking, not withdrawn: the DON CIO's
public policy index lists newer AI-specific memos, including "Accelerating
AI Adoption, Free Training Resources, and Measuring Time Savings Across the
DON" (16 Mar 2026) and "Acceptable Use of DON Information Technology" (14
Jul 2026), plus a referenced "Strategy to Weaponize Data and Artificial
Intelligence." Only the titles are confirmed; **the content of these 2026
memos is unverified** — do not cite specifics from them until their text is
obtained. The GenAI.mil build-out below is the later, better-documented
layer.

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
- DIDs common on NAVAIR CDRLs: **DI-MGMT-81861C is IPMDAR** (Integrated
  Program Management Data and Analysis Report) — an earlier version of this
  file called it "IPMR," which is wrong and not a minor labeling slip: IPMR
  (Integrated Program Management Report, historically the DI-MGMT-81466
  series) is a different, narrative/format-based EVM report that IPMDAR's
  XML/UN CEFACT schema superseded. Confirmed against a real filled CDRL for
  this exact DID in `sources/IPMDAR CDRL Example_Incremental Dec 2021
  FINAL.pdf`, which also shows real-world CDRL practice worth knowing:
  header fields (contract line item, exhibit, contract number) are commonly
  left blank on a template/pre-award CDRL, and most tailoring detail lives
  in Block 16 Remarks rather than the structured frequency/date fields. Also
  DI-SESS-81785 (SEP), DI-MISC-80508 (technical report), DI-IPSC-81435A
  (SDD), DI-IPSC-81433A (SRS). MBSE and SysML use follows NAVAIR digital
  engineering guidance and SEP Outline 4.1 Appendix E. Per DoDI 5000.88, a
  SEP's required content includes explicit entrance/exit criteria for
  technical reviews (delegated to each program's own SEP, not prescribed
  centrally), and SEPs must be approved before RFP release for each major
  program phase.
- NAVAIR's Small Business office documents sources-sought outcomes; NAWCAD
  Lakehurst holds an annual small-business industry day; NAWCTSD publishes
  a virtual PALT (Procurement Administrative Lead Time) meeting.
