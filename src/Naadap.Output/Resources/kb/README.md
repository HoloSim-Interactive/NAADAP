# NAADAP vehicle knowledge base, version 2026-09-17.1

Built by `scripts/kb/build_kb.py` from `scripts/kb/catalog.json` and the FY2025
FPDS extract `docs/research/g1-fpds/navair_orders_fy2025.csv`. Schema and
build pipeline: `docs/KB_SCHEMA.md`. Every file here except this README and
`manifest.sha256` is hashed in `manifest.sha256`; the pipeline verifies the
hashes before reading (derived requirement KB-630).

| File | Records | Content |
| --- | --- | --- |
| `families.jsonl` | 11 | Catalog families defined by a PIID regex |
| `vehicles.jsonl` | 484 | 11 family rows plus 473 parent-IDV rows with at least 3 distinct FY2025 NAVAIR orders |
| `office_affinity.tsv` | 528 | Orders per (vehicle, contracting office) and the share of that office's FY2025 orders under vehicles |
| `sources.jsonl` | 6 | Provenance sources with SHA-256 |

Coverage: rows account for 5,919 of 6,778 distinct FY2025 NAVAIR
orders under vehicles (87.3%). The literal `unknown` marks a field
the sources do not supply; nothing is inferred into it.
