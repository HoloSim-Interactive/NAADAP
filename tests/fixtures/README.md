# Validation Corpus & Test Fixtures

This directory is the checked-in fixture set referenced by
`docs/RTVM.md`'s test procedures. It exists so those procedures don't
each assemble their own ad hoc document set — see issue #6.

All real (non-synthetic) documents are sourced from **SAM.gov's public
opportunities search/API** (`https://sam.gov`), per the corpus-sourcing
recommendation in `docs/RTVM.md`'s "Research notes" section, filtered
to Navy-issuing offices and to solicitation types that carry
SOW/PWS/CDRL-style attachments. One document (the open-source-text
sample) comes from GovInfo's Congressional Hearings collection
instead — SAM.gov doesn't carry that document type. As U.S. Government
works, none of these are subject to copyright restriction on
redistribution (17 U.S.C. § 105).

## Layout

```
tests/fixtures/
  smoke/                 TP-100, TP-110
  reference-20/          TP-210, TP-220, TP-230, TP-420 (also usable for TP-260)
  synthetic-core200/      TP-200
  README.md               this file
```

## `smoke/` — TP-100 / TP-110

Six documents matching TP-100's exact required mix (2 SOW, 1 PWS,
1 CDRL, 1 sources-sought notice, 1 open-source text, mixed PDF/DOCX),
plus one corrupted file for TP-110. Five of the six are reused from
`reference-20/` (see that section for full provenance/rationale) —
reuse is intentional, not accidental duplication: it keeps the corpus
internally consistent and avoids sourcing the same document twice.

| File | Doc type | Format | Source |
| --- | --- | --- | --- |
| `sow-01-beq-m400-paintflooring.pdf` | SOW | PDF | = `reference-20/v3-01` |
| `sow-02-advanced-power.docx` | SOW | DOCX | = `reference-20/v1-06` |
| `pws-01-nswccd-ta-instruments.pdf` | PWS | PDF | = `reference-20/v1-01` |
| `cdrl-01-nswccd-waters-inspection.pdf` | CDRL | PDF | = `reference-20/v1-02` |
| `sources-sought-01-rot-frcs.pdf` | Sources-sought notice | PDF | = `reference-20/v3-08` |
| `open-source-text-01-v22-osprey-testimony.pdf` | Open-source text (Congressional testimony) | PDF | GovInfo `CHRG-118hhrg56063` — "Addressing Oversight and Safety Concerns in the Department of Defense's V-22 Osprey Program," House Armed Services Committee hearing, 2024-06-12. `https://api.govinfo.gov/packages/CHRG-118hhrg56063/granules/CHRG-118hhrg56063/pdf`. Picked because the V-22 program is a NAVAIR-managed acquisition — thematically consistent with the rest of the corpus (V-22 solicitations also show up organically in the NAVAIR SAM.gov search results used to build `reference-20/`), and it's genuinely public open-source text distinct from a procurement document, satisfying DATA-IN-100's fifth doc-type category. |
| `corrupted-truncated.pdf` | — (TP-110 negative case) | PDF (truncated) | Synthetic corruption: `head -c 20000` of `sow-01-beq-m400-paintflooring.pdf` (a valid ~157KB PDF cut to 20KB, mid-stream, before its xref table/EOF marker). A real parser will fail on this predictably; not a real-world found file, deliberately constructed per the issue's ask. |

## `reference-20/` — TP-210 / TP-220 / TP-230 / TP-420 (and TP-260's comparison run)

The fixed N=20 set docs/RTVM.md's "Reference scale for test data" calls
for. `ground-truth.json` in this directory carries the derived
candidate-vehicle mapping this section explains.

### Source documents

All fetched from SAM.gov opportunity attachments (via
`https://sam.gov/api/prod/opps/v3/opportunities/{opportunityId}/resources`
→ file download redirect to the S3-hosted attachment). Each row names
the SAM.gov opportunity the file came from and why it was picked.

| File | Solicitation | SAM.gov opportunity | Why picked |
| --- | --- | --- | --- |
| `v1-01-nswccd-pws.pdf`, `v1-02-nswccd-cdrl-a001-inspection.pdf`, `v1-03-nswccd-cdrl-a002-calibration.pdf` | NSWCCD C07 (PR 1301408430), TA Instruments/Waters equipment maintenance | `https://sam.gov/opp/718e9398c04946f280fa3cebaf1b421a/view` | A single Navy solicitation that happens to carry all three of PWS, CDRL — real, attached, small, text-extractable files, directly useful for DATA-IN-100's PWS/CDRL doc types. |
| `v1-04-fusionsupport-sow.pdf`, `v1-05-fusionsupport-cdrl.pdf` | N5523626Q0102 FUSION SUPPORT (system/software engineering & technical writing services) | `https://sam.gov/opp/163559872c7e4ed3b1ec7486b5bb8203/view` | Real SOW+CDRL pair for a professional/technical-services requirement — the SeaPort-NxG-shaped scope this corpus needed more examples of. |
| `v1-06-advanced-power-sow.docx`, `v1-07-advanced-power-followon-rfi.docx` | PD-44-0123 Advanced Electrical Power Systems Services (Sources Sought) | `https://sam.gov/opp/613d814a8f344eb3b9c8a1b4295b27c7/view` | Only DOCX-format SOW found with real content during this search — needed for DATA-IN-100's DOCX format coverage; also a genuine sources-sought SOW. |
| `v2-01-sbrac-vi-solicitation.pdf` | N6274226R1801, SBRAC VI (ID/IQ Small Business Environmental Remedial Action Contract, Hawaii/Guam/Pacific) | `https://sam.gov/opp/01e61934220a4be4a428e53633e23598/view` | The solicitation for a real, currently-active vehicle, named in its own title — the strongest, least-inferred ground-truth anchor in the set. |
| `v2-02-ev35-biosparge-sow.pdf`, `v2-03-ev35-ppi-log02.pdf` | N4008525R0058, EV35 NLON Site 11 Biosparge O&M, Naval Submarine Base New London | `https://sam.gov/opp/f1ab24dcbab4411f84f7312474dff79d/view` | Real environmental-remediation SOW to pair with the SBRAC solicitation above — same requirement category, different document. |
| `v3-01-beq-m400-paintflooring-sow.pdf` | 26M087CN, NAVFAC Mid-Atlantic BEQ M400 Paint-Flooring Repairs | `https://sam.gov/opp/b2a0ee6d49244b12a35cba64985fcad0/view` | Small, clean facility-repair SOW — also reused in `smoke/` for TP-100's SOW slot. |
| `v3-02-parkinglot-repairs-sow.pdf`, `v3-03-parkinglot-repairs-rfp.pdf` | 26M092CN, FY26 Multiple Location BEQ Parking Lot Repairs | `https://sam.gov/opp/f3454dad786e4e4ba4c9af334d4b9c29/view` | Same NAVFAC office/category as the paint-flooring SOW, deepening that cluster with a second real solicitation. |
| `v3-04-as4108-oilfiltration-sow.pdf` | 25-0433 AS4108 Oil Filtration Cart Power | `https://sam.gov/opp/87d5806a9cfa4e7b8b739dd00575d6a2/view` | Third real NAVFAC facility-repair SOW, different equipment/building. |
| `v3-05-firepumps-r38-sow.pdf`, `v3-06-firepumps-r38-solicitation.pdf` | N4008526R0270, Inspection & Testing of Fire Pumps, NWS Earle | `https://sam.gov/opp/a0b33d6c5a8b435dbaaacc39b931aec6/view` | Real building-systems inspection/testing SOW — same NAVFAC facility-repair category, different task type (inspection vs. renovation) for realism. |
| `v3-07-forklift-lease-sow.pdf` | Lease of Electric Forklift | `https://sam.gov/opp/e08483aadb8d42cdac6560cf1c70d7a7/view` | Real facility-equipment SOW, short and clean. |
| `v3-08-rot-frcs-sources-sought.pdf` | Sources Sought Notice, Facility Related Control Systems, NAVSTA Rota (NAVFAC Europe Africa Central) | `https://sam.gov/opp/58c7b24fcbeb46a4a497b014b1930cd8/view` | Real sources-sought notice with facility building-controls content — also reused in `smoke/` for TP-100's sources-sought slot. |
| `v4-01-title-escrow-sow.pdf`, `v4-02-title-escrow-solicitation.pdf` | Real Property Title Evidence, Insurance, Closing, and Escrow Services, Tarrant County, TX | `https://sam.gov/opp/1d4f90e3fc634cf28a92caa82cde93e0/view` | Deliberately chosen as a real Navy solicitation whose content does *not* fit the engineering/facilities pattern of the rest of the corpus — needed at least one genuinely different requirement theme so CORE-2xx's clustering has more than one axis to separate on, and so the ground-truth vehicle list isn't trivially one-dimensional. |

`navair.navy.mil` and `navsea.navy.mil` (the client's original suggested
source) returned HTTP 403 from this environment when the Systems
Engineer checked during the RTVM issue — SAM.gov was used as the
documented substitute, consistent with that decision. NAVAIR-specific
SAM.gov attachments were checked directly during this issue's sourcing
work and found to mostly redirect to the PIEE Solicitation Module (an
external system this environment can't reach), which is why the
corpus below leans on NAVFAC/NAVSEA/NAVSUP-adjacent Navy offices for
actual downloadable file attachments rather than NAVAIR narrowly —
still Navy-issued, still PWS/SOW/CDRL-carrying, consistent with the
RTVM's "Navy/NAVAIR-issuing offices" framing (NAVAIR being the named
example, not an exclusive filter).

### Ground truth derivation methodology

`ground-truth.json` maps each of the 20 documents to one of four
candidate contract vehicles. This mapping is **not fabricated** — it's
inspection-based, following each document's actual stated scope and,
in one case (SBRAC), the vehicle's own name appearing directly in the
solicitation title:

1. Read each document's title, scope section, and issuing office.
2. Grouped documents whose stated scope matches a real, named
   acquisition vehicle's actual purpose (e.g., an environmental
   remediation SOW → SBRAC, the vehicle NAVFAC Pacific actually uses
   for that work).
3. Where no single named vehicle obviously fit (the title/escrow
   documents), chose the closest general-purpose vehicle (GSA MAS)
   rather than inventing a vehicle name — documented as an inference,
   not a certainty, in that document's `rationale` field.
4. Four vehicles emerged from this process, not a predetermined count
   — this is fewer than the "top-5" language in OUT-420/DATA-OUT-300
   might suggest, and `ground-truth.json`'s `precisionAt5Note` explains
   how precision@5 should be scored against a 4-vehicle ground truth.

This reasoning — not just the resulting JSON — is what OUT-430's
validation-methodology write-up should point back to.

## `synthetic-core200/` — TP-200

CORE-200 (per `docs/RTVM.md`) is deliberately *constructed*, not
sourced: `doc-a.txt`, `doc-c.txt`, `doc-e.txt` share one requirement
theme (flight-line engineering disposition support), `doc-b.txt` and
`doc-d.txt` share a different theme (shipboard fire-control system
maintenance), and `doc-f.txt`..`doc-j.txt` are unrelated distractors
(custodial, IT help desk, medical logistics, cybersecurity training,
landscaping). `ground-truth.json` in this directory records the
expected cluster assignment. Every file is plain text and entirely
authored for this fixture set — no external source, matching the
issue's explicit instruction that this set doesn't come from SAM.gov.

## Mapping to test procedures

| Test Procedure | Fixture(s) used |
| --- | --- |
| TP-100 (DATA-IN-100) | `smoke/` — the 6 non-corrupted files |
| TP-110 (DATA-IN-110) | `smoke/` — the 6 files above + `corrupted-truncated.pdf` |
| TP-200 (CORE-200) | `synthetic-core200/` |
| TP-210 (CORE-210) | `reference-20/` |
| TP-220 (CORE-220) | `reference-20/` |
| TP-230 (CORE-230) | `reference-20/` |
| TP-260 (CORE-260) | `reference-20/` + its `ground-truth.json` (Alternative-Approach Harness comparison run) |
| TP-420 (OUT-420) | `reference-20/ground-truth.json` |

## A note for the Ingestion issue (#7)

This layout (`tests/fixtures/...`) was chosen because no Ingestion
test project convention exists yet at the time of this issue — per
issue #6's own scope note, if `tests/Naadap.Ingestion.Tests/` ends up
wanting a different fixture location/convention once that issue is
under way, move these directories to match rather than leaving two
conventions in the repo.
