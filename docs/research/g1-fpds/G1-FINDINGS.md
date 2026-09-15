# G1 — FPDS premise check: findings

Date 2026-09-15. Gate G1 in
<a href="https://github.com/HoloSim-Interactive/NAADAP/blob/main/docs/design/vehicle-recommendation-pipeline.md" target="_blank">docs/design/vehicle-recommendation-pipeline.md</a>.
Question: does the public award record hold enough NAVAIR orders-under-vehicle,
with the catalog's vehicles present, to fit the recommendation model and
build the knowledge base from data?

## Verdict

**Pass on volume and on vehicle presence. Fail on catalog coverage as
curated.** The fitted model is feasible for v1. The knowledge base cannot be
hand-curated from the catalog alone; it must be derived from this data with
vehicle-family grouping. Details and consequences below.

## Source and method

| Item | Value |
| --- | --- |
| Archive | `https://files.usaspending.gov/award_data_archive/FY2025_097_Contracts_Full_20260906.zip` |
| Agency | 097 (DoD); Navy filtered as `awarding_sub_agency_code == 1700` |
| Size | 1.04 GB compressed; five CSVs, 8.63 GB uncompressed; 297 columns |
| Rows | 4,489,958 DoD transactions (actions, including modifications) |
| Pass 1 | Stream-filter to NAVAIR offices {N00019, N00421, N68335, N61340, N68936, N68520} with non-null `parent_award_id_piid`; 106 s single-threaded |
| Pass 2 | Dedupe on `award_id_piid`; distinct parents; catalog-pattern match on hyphen-stripped PIIDs; PSC, size, set-aside, description sampling |
| Pass 3 | Per-parent breadth: distinct offices, distinct `dod_acquisition_program_description`, services share, small-business share |
| Pass 4 | Re-stream for `type_of_contract_pricing_code` on distinct orders by vehicle family |
| Outputs | `navair_orders_fy2025.csv` (15,162 rows, 26 columns), `g1_pass1_summary.json`, `g1_pass2_summary.json`, `g1_pass3_breadth.json`, `g1_pass4_pricing.json` — all in this directory |

Dedupe keeps the first row seen per `award_id_piid`, which may be a
modification; `prime_award_base_transaction_description` is stable across
modifications and is used for description analysis.

## Findings

**F1. Volume — pass.** DON: 213,554 transactions, 134,789 (63%) under a
parent IDV. NAVAIR six offices: 22,867 transactions, 15,162 (66%) under a
parent IDV, deduplicating to **6,778 distinct orders under 1,589 distinct
parent IDVs** in one fiscal year. Conditional logit with 20–60 parameters
is well within this.

**F2. All six offices present.**

| Office | Distinct orders | Name in FPDS |
| --- | --- | --- |
| N00421 | 2,095 | Naval Air Warfare Center Air Div |
| N00019 | 1,536 | Naval Air Systems Command |
| N68936 | 1,380 | Naval Air Warfare Center |
| N68335 | 1,058 | NAVAIR Warfare Ctr Aircraft Div |
| N61340 | 358 | NAWC Training Systems Div |
| N68520 | 351 | Fleet Readiness Center |

**F3. Catalog vehicles present — pass.** Every NAWC vehicle in
contract-vehicles.md appears as a parent PIID on FY2025 NAVAIR orders:

| Vehicle | Distinct orders | Distinct parent PIIDs | Note |
| --- | --- | --- | --- |
| NAWCAD SCI MAC (N00421-25-D-00nn) | 140 | 59 | Awarded 2025; already active |
| SeaPort-NxG (N00178-yy-D-nnnn) | 134 | 80 | 60 of 80 PIIDs are FY19 bases; legacy SeaPort-e FY04–16 bases still carry orders |
| NAWCTSD TSC IV (N61340-18-D-50nn) | 20 | 9 | |
| NAWCTSD FTSS V (N61340-22-D-1nnn/2nnn) | 20 | 7 | |
| NAWCTSD PACRM (N61340-21-D-000n) | 7 | 4 | |
| NAWCAD Cyber IDIQ (N00421-23-D-0021) | 4 | 1 | |
| GSA MAS (GS-xxF / 47Q…) | 188 | 119 | |
| NASA SEWP (NNG15S…) | 187 | 42 | |
| GSA OASIS+ (47QRC) | 0 | 0 | No NAVAIR use in FY2025 |
| NAVAIR HQ BOAs and IDVs (N00019-yy-[DG]-nnnn) | 1,514 | 214 | BOAs (G) dominate; per-vendor, not multiple-award |

**F4. Catalog coverage — fail as curated.** The patterns above account for
2,214 of 6,778 orders (33%). **4,564 orders (67%) sit under 1,054 parent
PIIDs the catalog does not name.** The knowledge base built from
contract-vehicles.md alone would have no row for two-thirds of what NAVAIR
actually ordered. The KB has to be derived from this list.

**F5. Breadth must be computed per vehicle family, not per PIID.** A
multiple-award contract has one PIID per awardee — SCI MAC is 59 PIIDs, SEWP
is 42, SeaPort-NxG is 80. Per-PIID, each looks like a low-volume
single-office contract; per-family, they are the enterprise vehicles they
are. The KB needs a `family` key (regex over the PIID, e.g.
`^N0042125D00\d{2}$` → SCI MAC) and every breadth, headroom, and affinity
statistic computed at family level. The per-PIID tiers below are therefore
a lower bound on breadth.

**F6. Per-PIID breadth tiers** (offices = distinct `awarding_office_code`;
programs = distinct non-empty `dod_acquisition_program_description`):

| Tier | Parent PIIDs | Orders | Share of orders |
| --- | --- | --- | --- |
| Enterprise (≥3 offices, or 2 offices and ≥3 programs) | 37 | 686 | 10% |
| Multi (2 offices, or ≥3 programs) | 84 | 376 | 6% |
| Single-office, high-volume (≥10 orders) | 134 | 2,749 | 41% |
| Single-office, low-volume | 1,334 | 2,967 | 44% |

**F7. The broadest-shared vehicles are COTS software and IT hardware, not
services.** The enterprise tier's top entries are NIWC Pacific (N66001)
enterprise-software BPAs — VMware, Azure, AutoCAD, Splunk, Creo, SAP — and
NASA SEWP contracts for routers and switches, each ordered by four or five
NAVAIR offices. That is the DON ESL / DoD ESI pattern in the skill, seen
live. For *services*, NAVAIR's pattern is one IDIQ per office:

| Parent | Office | Orders | Base description |
| --- | --- | --- | --- |
| N0042122D0099 | N00421 | 103 | Install ACS PC workstation upgrade equip |
| N6893619D0007 | N68936 | 64 | Modification of equipment — aircraft components |
| N6893618D0015 | N68936 | 58 | GQM-163A O&M program |
| N0042120D0123 | N00421 | 51 | Labor — RDT&E funding |
| N0042124D0012 | N00421 | 51 | SSCO program task order |
| N0042121D0008 | N00421 | 41 | Fighter jet services — Type III aircraft |
| N6852023D0001 | N68520 | 41 | FRCE engineering support services |
| N6852023D0111 | N68520 | 39 | O&M funded logistics support services |

COMFRC has its own engineering-services and logistics-services IDIQs;
NAWCWD has its own; NAWCAD has several. Each is a consolidation *source* by
the breadth test, and collectively they are the empirical form of the
problem the challenge states: each requirement owner established their own
contract.

**F8. The sponsor's illustrative example exists in the data.**
`N6134019D1010`, base description "Naval Test Wing Pacific Aircraft
Maintenance (VX-30 and VX-31)," 100% services PSC, awarded by NAWCTSD and
ordered in FY2025 by N00019, N00421, and N68936 — three offices. A
flight-line services vehicle for VX squadrons, already shared across
commands. It is the "VX-XX" paragraph of the problem statement as an FPDS
row.

**F9. SeaPort-NxG is 2% of NAVAIR orders.** 134 of 6,778. Consistent with
NAVAIR declining to join SeaPort-NxG in 2018 and building its own MACs.
contract-vehicles.md's "presumptive Navy services vehicle" framing needs a
NAVAIR-specific caveat; NMCARS 5237.102 mandatory consideration is a
question NAVAIR answers "no" far more often than the DON-wide framing
implies.

**F10. Descriptions — use the base field.** `transaction_description` is
modification noise ("FUNDING", "DE-OBLIGATE FUNDING", "ADMINISTRATIVE MOD",
"FUNDING DEOB"). `prime_award_base_transaction_description` carries the
work ("Engineering services", "Task order — PMA 275", "Delivery order for
the procurement of engineering services for the PMA-299 H-60 program").
Median length 34 characters, maximum 436, none empty. Terse, uppercase,
abbreviated — the RAND PE-A926-1 and Zhang/Zhao/LeCun findings on lexical
methods apply.

**F11. `dod_acquisition_program_description` is unreliable.** Empty on most
orders; populated only for a few programs (F-35, AIM-9X). PMA identity has
to come from the description text ("PMA 275", "PMA-299"), as the LRAE
analysis already found for the forecast data.

**F12. Business size is present on every order.** 3,724 other-than-small,
3,054 small (45%). The small-business impact note (needs 5 and 13) is
computable from primary data. Set-aside code is sparse: 5,041 blank, 1,145
NONE, 541 SBA, 21 8(a) non-competitive.

**F13. Half the orders are services.** 3,452 of 6,778 carry a letter-prefixed
(services) PSC; R425 engineering/technical support leads at 652. The rest
is hardware (1680 aircraft accessories, 1510/1520 aircraft, 5821 airborne
comms). The model is fit on the services half.

**F14. Pricing — resolves the flagged "no T&M" discrepancy.** On distinct
FY2025 NAVAIR orders:

| Family | n | Pricing |
| --- | --- | --- |
| SeaPort-NxG | 134 | 126 CPFF, 6 cost-no-fee, 2 FFP. **Zero T&M, zero labor-hour.** |
| NAWCAD SCI MAC | 140 | 55 CPFF, 49 cost-no-fee, 36 FFP |
| TSC IV | 20 | 19 FFP, 1 CPFF |
| FTSS V | 20 | 20 FFP |
| NAVAIR HQ BOAs | 762 | 415 CPFF, 324 FFP, 10 CPIF, 6 FPI, 5 cost-no-fee, **2 T&M** |

The vendor page's "labor hour terms" claim is not borne out on NAVAIR's
FY2025 SeaPort orders. The skill's "no T&M" statement stands on primary
data, with the caveat that this is one fiscal year and NAVAIR offices only.

## Design consequences

1. **KB is data-derived, family-grouped.** G5 starts from the parent-PIID
   list ranked by family order volume, not from the web-curated catalog.
   The catalog supplies scope narrative, ordering-period end, and
   eligibility for the families it knows; FPDS supplies the families it
   does not, with office affinity, PSC mix, size mix, and pricing mix
   observed rather than asserted.
2. **Breadth, headroom, and affinity are family-level statistics.** A
   `family` column and its regex are part of the KB schema (need 9).
3. **The eligible set per office is large and mostly single-office.** Hard
   constraints and the breadth test do most of the elimination before the
   model scores anything; the model ranks the survivors. This is the
   criticality-versus-predictive-strength split from research pass E, now
   forced by the data shape.
4. **Filter to services PSCs** for fitting and for the v1 lookup table.
5. **Use `prime_award_base_transaction_description`** for text features;
   treat `transaction_description` as a modification-type signal only.
6. **The v1 P(vehicle | office) lookup table is computable now** from
   `navair_orders_fy2025.csv` — family by office by count. The fitted β is
   feasible on volume and can move into v1 on schedule grounds alone.
7. **Vehicle promotion (need 15) has a concrete candidate list**: the
   single-office high-volume tier, 134 PIIDs, 2,749 orders, ranked by order
   count with services share and office. `N6134019D1010` is the worked
   example.

## Caveats

- One fiscal year. Ordering-period ends, ceilings, and multi-year affinity
  need FY2022–FY2025 at least; the pull is 106 s per year.
- Transactions were deduplicated to orders by first row seen; obligation
  sums were not computed and would need base-plus-modification aggregation.
- Breadth tiers are per PIID and therefore understate multiple-award
  families (F5).
- `awarding_office_code` identifies the contracting office, not the
  requiring activity; the VX squadron in F8 is inferred from the
  description, not from a field.
- No GFI. Everything here is public and stays public.
