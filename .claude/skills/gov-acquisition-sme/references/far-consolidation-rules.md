# FAR, DFARS, NMCARS: the rules that govern a consolidation recommendation

Compiled 2026-09-04 from acquisition.gov (FAR, DFARS, PGI, NMCARS),
15 U.S.C. 644(e) and 657q, 13 CFR 125, OMB M-19-13, DoDI 5000.74, GAO and
CRS reports. Dollar thresholds are as displayed on acquisition.gov on that
date. FAR 1.109 adjusts statutory thresholds for inflation every five
years, last effective 1 Oct 2025, so re-verify any figure before it goes
into a formal determination. The $2M consolidation threshold is statutory
and not inflation-adjusted.

## The definitions that decide which rule applies (FAR 2.101)

- **Consolidation**: a solicitation for a single contract, MAC, task order,
  or delivery order "to satisfy two or more requirements of the Federal
  agency for supplies or services that have been provided to or performed
  for the Federal agency under two or more separate contracts, each of
  which was lower in cost than the total cost of the contract for which
  offers are solicited." Triggers on value and prior separate performance,
  regardless of business size.
- **Bundling**: a subset of consolidation that combines requirements
  "previously provided or performed under separate smaller contracts" into
  a single contract, MAC, or order "that is likely to be unsuitable for
  award to a small business concern." Bundling adds the small-business
  history and suitability tests. If both apply, follow the bundling rules.
- **Multiple-award contract**: a GSA MAS contract, a 16.5 multiple-award
  task/delivery-order contract, or any IDIQ with two or more sources.
- **GWAC**: an IT task/delivery-order contract established by one agency
  for government-wide use under an OMB executive-agent designation.

So the pipeline should flag, for every incumbent requirement, the contract
number, the incumbent's business size, and the aggregate value including
options. Those three facts route the analysis.

## FAR 7.107: the compliance spine

| Rule | Threshold | What is required |
| --- | --- | --- |
| 7.107-2 Consolidation | over $2M (15 U.S.C. 657q) | SPE or CAO written determination that consolidation is "necessary and justified" after market research, identification of alternatives "that would involve a lesser degree of consolidation," coordination with the small-business office, identification of negative impacts on small business, and steps to include small business. Benefits must "substantially exceed" the alternatives. |
| "Substantial" benefit | 10% of value at or under $94M; the greater of 5% or $9.4M above $94M | Benefit categories: cost savings or price reduction, quality improvements that save time or enhance performance, reduced acquisition cycle time, better terms and conditions, other. |
| 7.107-3 Bundling | any bundled requirement | Written "necessary and justified" determination with "measurably substantial benefits" at the same 10% / 5%-or-$9.4M levels. "Reduction of administrative or personnel costs alone is not sufficient justification for bundling unless the cost savings are expected to be at least ten percent." |
| 7.107-4 Substantial bundling | DoD $8M; NASA/GSA/DOE $6M; others $2.5M, cumulative including options | Acquisition strategy must add specific benefits, impediments to small-business primes, actions to maximize small-business primes including teaming, actions to maximize small-business subcontractors, justification, and alternative strategies with reasons for rejection. |
| 7.107-5 Notifications | as above | Notify each incumbent small business at least 30 days before solicitation, with SBA PCR contact; publish a GPE notice of the determination and wait 7 days before publicizing the solicitation; notify the PCR 30 days before a follow-on bundled solicitation with achieved savings; annual public list of bundled requirements. |
| 7.107-6 | MAC solicitations above the substantial-bundling threshold | Insert provision 52.207-6 soliciting small-business teaming and joint ventures. |

7.107-1 exempts orders under a single-agency IDIQ whose base contract
consolidation was already justified, and mandatory sources under 8.002 and
8.003. That exemption is why "order under an existing MAC" is the
lowest-friction consolidation path.

Related hooks: FAR 10.001(a)(2) requires market research "before
soliciting offers for acquisitions that could lead to consolidation or
bundling." FAR 19.202-1(e) requires at least 30 days' notice to the PCR when
work currently performed by small business is proposed in a consolidation
unlikely for small-business award. FAR 15.304(c)(4) makes small-business
subcontracting participation an evaluation factor in unrestricted
consolidated or bundled acquisitions. GAO-14-36 (2013) found a third of
DoD contracts reported as consolidated were not, and some determinations
were signed below the required authority. Protests on bundling "seldom
prevail" because the protester must show the agency's benefit analysis was
unreasonable (CRS R41133).

## Navy approvals (NMCARS)

- **5207.107-2**: the FAR 7.107-2 determination is approved by DASN(P)
  when DASN(P) or the Navy Senior Services Manager approves the acquisition
  strategy; otherwise by the HCA, delegable once to the Deputy or Assistant
  Commander for Contracts or a flag/SES acquisition professional. Bundling
  determinations (5207.107-3) go through DASN(P).
- **5207.103**: streamlined acquisition plans (STRAPs, Annexes 17 to 20)
  at DFARS 207.103(d)(i) thresholds; DASN(P) approves any action at or
  above $250M; MOPAS-S (Annex 21) for services under $50M total or $25M per
  year.
- **5237.102**: SeaPort consideration is mandatory for Annex 22 functional
  areas. Exceptions: FAR 6.302 actions, under SAT, 8(a) set-asides, Part 12
  commercial, Part 13 simplified — **confirmed** against the DON Best
  Practices Handbook for Services Acquisitions FY2024, which also bundles
  FAR 8.4 into the Part 12 commercial exception (so the exception list is
  more precisely "FAR 6.302, below-SAT, 8(a) set-asides, and FAR Part 12/8.4
  commercial buys, plus Part 13 simplified"). Not using SeaPort without a
  J&A requires a D&F to DASN(P) with HCA endorsement covering commerciality,
  contract type, competitiveness, vehicle, and prior contract history
  (5237.103).

  **The actual SeaPort waiver D&F checklist** (from the DON Best Practices
  Handbook — this is genuinely new, concrete content, not previously in
  this file): the HCA's D&F must address (1) does the requirement fit
  Engineering Services or Program Management Services under Annex 22; (2) is
  it commercial or non-commercial, and would another CM-tiered solution
  work; (3) is this a follow-on, and are prior contract numbers provided;
  (4) if a follow-on, is the incumbent available in SeaPort; (5) were prior
  awardees large or small businesses; (6) was there a previous SeaPort
  waiver; (7) per EDA/SAM.gov, how many orders were placed under the
  previous MAC; (8) did prior orders include options or surge CLINs; (9) per
  EDA/SAM.gov, how many prior orders were FFP versus cost-type; (10) will
  the new contract be single- or multiple-award; (11) if SeaPort genuinely
  cannot support the requirement, is there another tiered contract solution
  that meets mission needs. Treat this as the concrete evidence set a
  vehicle recommendation should assemble before anyone drafts the D&F.
- **5216.504**: single-award IDIQ D&F approved by the Deputy or Assistant
  Commander for Contracts; DASN(P) is the task-order ombudsman and
  Competition Advocate General.

## DoD layers

- **DFARS 207.170 was deleted in 2018** (DFARS Case 2017-D004, 83 FR
  15776). 10 U.S.C. 2382 was repealed; consolidation is governed by 15
  U.S.C. 657q and FAR 7.107 government-wide. Do not cite DFARS 207.170 as
  live authority.
- **DFARS 207.103(d)(i)**: written acquisition plan at $10M development or
  $50M total / $25M any year for production and services.
- **DFARS 216.504/216.505**: SPE holds single-award IDIQ authority above
  $150M ("so integrally related that only a single source can efficiently
  perform"); orders at or above $15M get debriefings; one-offer competitive
  orders above SAT follow 215.371; orders above SAT on non-DoD contracts
  follow subpart 217.7.
- **DFARS 217.770**: before using a non-DoD vehicle (GSA MAS, GWACs,
  another agency's MAC) above SAT, a documented best-interest review
  covering customer requirements, schedule, cost effectiveness including
  fees, administration and oversight, scope, and funding. A GSA vehicle is
  a legitimate recommendation for a Navy requirement but never a free one.
- **DFARS 237**: 237.102-74 services taxonomy; 237.170-2 non-performance-
  based services need approval (up to $100M designated official, above
  that SPE); 237.172 QASP prepared with the SOW; PGI 237.102-77 ARRT tool
  for PWS/QASP/PRS.
- **DoDI 5000.74** (services at or above SAT; verified against the real
  esd.whs.mil PDF, effective 10 Jan 2020, Change 1 24 Jun 2021). S-CAT tiers
  on the IGCE, **each with its own decision authority** (an earlier version
  of this file gave the thresholds without authorities, which invites
  assuming USD(A&S) approves ordinary S-CAT I — wrong):

  | S-CAT | Threshold | Decision authority |
  | --- | --- | --- |
  | Special Interest | any value, designated by ASD(A) | USD(A&S) or designee — the *only* tier USD(A&S) itself decides |
  | I | ≥ $1B, or > $300M in any one year | SAE/CAE or designee |
  | II | $250M to < $1B | SAE/CAE or designee |
  | III | $100M to < $250M | Component SSM or designee |
  | IV | $10M to < $100M | Component SSM or designee |
  | V | SAT to < $10M | Component SSM or designee |

  Services Requirements Review Board at or above $10M including IDIQ base
  and each order at or above $10M, approved before Step 5 (Develop
  Acquisition Strategy) of the **Seven Steps to the Services Acquisition
  Process** (DAG Chapter 10, confirmed): Plan phase — 1 Form the Team, 2
  Review Current Strategy, 3 Perform Market Research (explicitly including
  "whether or not any existing contract vehicles are available to execute
  the requirement," and required before soliciting anything that "could
  lead to a consolidation of contract requirements" or a bundled contract);
  Develop phase — 4 Define Requirements, 5 Develop Acquisition Strategy
  (gated by SRRB approval); Execute phase — 6 Execute Strategy, 7 Manage
  Performance.

  **SRRB content** (tailored, not limited to): mission need; strategic
  alignment; issues and risks; workforce analysis (insource/outsource);
  relationship to other requirements; projected cost through the FYDP (5
  years); prioritization; contract and work functions (inherently-
  governmental screen); metrics.

  **Acquisition-strategy content** for services above SAT but below the
  full acquisition-plan threshold, four parts: (a) requirements development
  — source of requirement, outcomes/metrics, how previously satisfied,
  market research summary, **"a summary that addresses consolidated or
  bundled requirements, if it applies"** (verbatim), analysis-of-
  alternatives summary; (b) acquisition planning — approach/milestones,
  cost estimate, funding, PBA implementation or rationale for not using it,
  small-business opportunities, evaluation criteria, other-than-full-and-
  open rationale, multi-year contracting, waivers; (c) solicitation and
  award — business arrangement type (single contract, MATOC, order under an
  existing MAC), duration, pricing arrangement; (d) risk/issue/opportunity
  summary and a performance-management/metrics plan.

  Existing government-wide contracts "should be used to the maximum extent
  practicable"; requirements addressed at an enterprise level (Component-
  wide, DoD-wide, or Best-in-Class).

  **Two more DoD-level triggers not previously in this file:** an
  Independent Management Review is required post-award for services
  contracts of **$100M or more**, evaluating cost/schedule/requirements
  performance, contracting-mechanism choice, subcontractor management,
  oversight staffing, and pass-through charges — distinct from the S-CAT and
  SRRB thresholds above. And a Portfolio Manager must run a **business case
  analysis at $50M** for any acquisition that "may significantly overlap
  with the requirements of an existing contract, DoD or government-wide
  acquisition contract, or BIC contract" — a second, DoD-level overlap-
  detection threshold, separate from FAR 7.107-2's $2M consolidation
  determination. A bridge contract used because of inadequate planning also
  triggers a notification duty (10 U.S.C. 2329(e)): first use, notify the
  commander/senior civilian for contracts under $10M or the SAE/agency
  head/COCOM/USD(A&S) for $10M or more; a second use under $10M escalates
  further.
- **DFARS 208.74**: commercial software must go through DoD ESI enterprise
  agreements; DON ESL is mandatory for DON software by 2012 memo.

## Category management (OMB M-19-13, plus later amendments)

**Amendments not previously noted here, both primary-sourced:** OMB
M-19-13 was amended by **M-22-03, "Advancing Equity in Federal
Procurement" (2 December 2021)**, which aligned category management with
Executive Order 13985 to increase awards to socio-economic small
businesses — the DON's own services handbook calls the combined result
"M-19-13-1." A further memo, **M-25-31, "Category Management Policy" (18
July 2025)**, is referenced by the DON Category Management Program Office
as a current governing document under a "Revolutionary FAR Overhaul"
heading — **its text was not obtained and its content is unverified; treat
its existence as confirmed but do not cite specific rules to it** until the
memo itself is read.

**Spend Under Management tiers, verbatim from GSA's own government-wide
Category Management PMO playbook** (March 2024, corrects an earlier
paraphrase in this file that was imprecise on Tier 2): Tier 3 – Best-in-
Class, "dollars obligated on government-wide contracts that satisfy the
most rigorous standards set for leadership, strategy, data, tools, and
metrics"; Tier 2 – Multi-Agency Solutions, "dollars obligated on multi-
agency contracts that satisfy rigorous standards" (its own bar, distinct
from and lower than BIC's — not simply "not BIC"); Tier 1 – Mandatory-Use
Agency-Wide Solutions, "dollars obligated on agency-wide contracts with
mandatory use or mandatory-consideration policies"; Tier 0 – spend not
aligned to CM principles. SUM = Tier 1 + Tier 1 SB + Tier 2 + Tier 2 SB +
BIC; Tier 0 is excluded from SUM. Analyses of alternatives are required 18
to 24 months before award for planned acquisitions over $50M that would be
Tier 0 and over $100M that would be Tier 1, and must explain why Tier 2 or
BIC vehicles are unsuitable. Not required for defense-centric spend.
Statutory small-business goals are unchanged and agencies must prioritize
them over BIC goals when both cannot be met (CRS IF12374).

Addressable spend is identified by PSC. **Whether Navy-specific vehicles
(SeaPort-NxG, NAVFAC MACCs, NAWCAD MACs) are formally Tier 1 or Tier 2 is
not confirmed by any primary source obtained** — no document in hand names
a specific tier for a specific Navy vehicle; treat the "Tier 1 or 2, not
BIC" claim as a reasonable inference from SeaPort's DON-mandatory-
consideration status, not a sourced fact. DON has its own CM priority rule
beyond the OMB text: "tiered DON contracts shall have priority
consideration" over tiered contracts written by other agencies (DON Best
Practices Handbook FY2024) — a reason to check DON vehicles before
government-wide ones even when both are equally tiered.

**Real government tooling for finding duplicative spend**, from GSA's CM
PMO playbook (a genuinely new source, not DON-specific but directly
relevant to how NAADAP should think about consolidation evidence): the
**CM Reporting Workbench (Awards Explorer)** on the Acquisition Gateway/D2D
portal pulls FPDS award data enriched with CM fields, including **"BIC
Addressable"** and **"Tier 2 Addressable"** flags — "indicate that the
requirement could potentially be met on a particular solution because it
has been used to meet similar requirement(s) previously... a starting
point for market research using historical data from FPDS." A worked
**Contract Reduction Playbook**: identify top vendors by total spend,
identify vendors with the most contracts, identify vendors with the most
Tier-0 contracts and Tier-0 spend, export vendor/requirement-description/
POC/addressability/completion-date detail, then validate by contacting the
POC and the target solution's program office about migration. A **SUM-
Increase Playbook**: search the Acquisition Gateway **Solutions Finder**
(over 300 government-wide solutions, filterable by keyword/agency/PSC/
NAICS) before soliciting, and use Awards Explorer's "Ultimate/Final
completion date" and IDV "Expiration date" fields to flag *approaching*
contract expirations as the trigger to check addressability, not only
active re-competes. This is a real, if login-gated, government system
doing close to what NAADAP's own vehicle recommender aims to do —
reinforces vehicle-matching.md's framing of the project as "an automated
MRAS."

## Vehicle taxonomy by friction, lowest first

1. Order under an existing agency MAC the requirement is already in scope
   of (SeaPort-NxG, NAWCAD MACs, NAVFAC MACCs). Fair opportunity under
   16.505(b); no synopsis under 5.202(a)(6); 7.107 already satisfied at the
   base contract if justified there. Check scope, ceiling remaining,
   ordering period against the intended period of performance, NAICS pool,
   and set-aside status of the pool.
2. FSS BPA or MAS order under FAR 8.4 (per-order competition, 8.405-2
   three-quote or eBuy; single-award BPA over $150M needs head-of-agency
   determination). DoD adds 217.770.
3. GWAC or other-agency BIC vehicle (Part 17.5 plus 217.770; CAF fees).
4. New agency IDIQ, multiple award preferred (16.504(c)); single award over
   $150M needs SPE D&F.
5. New stand-alone contract, full and open, with a 7.107 determination.

## Contract-type constraints that decide whether requirements can share a vehicle

- Commercial services (Part 12) must be FFP or FP-EPA, or T&M with a D&F;
  cost-type is prohibited (12.207(e)). Developmental cost-type work should
  stay off a commercial vehicle.
- Performance-based services first, FFP first (37.102(a)); non-PBA needs
  DFARS 237.170-2 approval. A consolidated PWS needs one integrated QASP.
- T&M and LH need a D&F that no other type is suitable, a ceiling price,
  and HCA approval beyond three years (16.601). SeaPort-NxG permits no T&M
  orders and caps profit and pass-through at 8%.
- Cost-reimbursement requires an approved plan, an adequate accounting
  system, and Government surveillance resources (16.301-3).
- Personal-services indicators in an incumbent SOW (on-site, Government
  supervision, comparable civil-service work, over a year) must be
  scrubbed before consolidation (37.104).
- A sole-source component (OEM sustainment, SBIR Phase III) forces a J&A
  for the whole or a carve-out; a logical follow-on order under a MAC uses
  16.505(b)(2)(i)(C) instead of Part 6.

## Small-business tests that can kill a consolidation

- Rule of two (19.502-2) above SAT; automatic small-business reservation
  between MPT and SAT (13.003). Many sub-SAT buys are the classic Tier 0
  consolidation target, and consolidating them above SAT removes the
  automatic reservation and can trigger 7.107.
- 13 CFR 125.2(e): the CO "must set-aside a Multiple Award Contract if the
  requirements for a set-aside are met," may partially set aside or
  reserve, and may set aside orders on unrestricted MACs.
- Prefer structures that preserve set-asides: partial set-aside (19.502-3),
  reserved awards (19.502-4), order-level set-asides on the MAC (19.504),
  small-business-only pools (SeaPort-NxG determines size at order level).
- NAICS (19.102) sets the size standard by "principal purpose"; from
  1 Oct 2028 MACs may carry multiple NAICS by portion.

## Consolidation-analysis checklist

1. Inventory each sub-requirement: incumbent contract and order numbers,
   vehicle, competition type, NAICS and PSC, incumbent size, value,
   period-of-performance end, contract type, SOW or PWS form, security
   level, place of performance.
2. Classify: DoD portfolio group and government-wide category from PSC;
   commercial or not; PBA feasibility.
3. Aggregate value including options: over $2M means 7.107-2; any small
   incumbent or suitable lot means bundling analysis; DoD $8M or more
   means substantial bundling.
4. Rule of two per lot and for the whole; list set-aside-preserving
   structures.
5. Candidate vehicles in friction order, with scope, ceiling, ordering
   period, NAICS pool, fee, and ordering eligibility checked.
6. Quantify benefits against the 10% / 5%-or-$9.4M test and list
   lesser-consolidation alternatives.
7. Approvals: DASN(P) or HCA determination; S-CAT decision authority
   (Component SSM through S-CAT III-V, SAE/CAE for I-II, USD(A&S) only for
   Special Interest); SRRB at $10M; a $50M DoD-level business-case analysis
   if the acquisition may significantly overlap an existing contract or
   BIC/GWAC solution; non-PBA approval; T&M D&F; J&A or fair-opportunity
   exception; 217.770 review; DD 254; CMMC level; an Independent Management
   Review at $100M or more post-award.
8. Timeline: 30-day incumbent and PCR notices; 7-day GPE notice; 15/30-day
   synopsis rules; category-management AoA 18 to 24 months out for over
   $50M/$100M; if a bridge contract results from inadequate planning, a
   10 U.S.C. 2329(e) notification (commander/SAE tier depends on value).

## FAR parts in one line each, for orientation

Part 1 authority (only the CO signs determinations; CORs hold no
commitment authority). Part 5 synopsis at $25K, 15/30/45-day timing;
orders under existing vehicles need no synopsis. Part 6 competition and
the seven 6.302 exceptions, J&A content at 6.303-2, approval tiers at
6.304 ($900K CO, $20M competition advocate, $90M HCA, $150M DoD). Part 8
required sources and FSS ordering. Part 10 market research, reusable
within 18 months. Part 11 state needs by function, performance, or
essential characteristics; order of precedence favors PWS/SOO over design
documents. Part 12 commercial. Part 13 simplified, BPAs at 13.303. Part 15
negotiated procurement, RFI at 15.201(e), Uniform Contract Format at
15.204-1, evaluation at 15.304. Part 16 contract types, IDIQ at 16.504,
fair opportunity at 16.505, T&M at 16.601. Part 17.5 interagency
acquisition. Part 19 small business. Part 37 services, performance-based
acquisition at 37.6, SOO minimum content at 37.602(c). Part 42
administration, CPARS at 42.15. Part 46.4 QASP with the SOW.
