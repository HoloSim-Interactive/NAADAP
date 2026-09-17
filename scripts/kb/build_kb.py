#!/usr/bin/env python3
"""Build the NAADAP vehicle knowledge base (gate G5).

Inputs
  scripts/kb/catalog.json                        hand-curated vehicle families with sources
  docs/research/g1-fpds/navair_orders_fy2025.csv FPDS extract: NAVAIR orders under vehicles, FY2025

Output (the shipped artifact; embedded into Naadap.Output as resources)
  src/Naadap.Output/Resources/kb/
    kb_version            one line: <date>.<n>
    README.md             schema summary (the full schema is docs/KB_SCHEMA.md)
    sources.jsonl         one record per provenance source: source_id, title, url/path, retrieved, sha256
    families.jsonl        one record per catalog family (regex-defined multiple-award vehicles)
    vehicles.jsonl        one record per knowledge-base row: a family, or a single parent IDV
                          with >= MIN_ORDERS distinct FY2025 NAVAIR orders not matched by a family
    office_affinity.tsv   vehicle_id, office_code, orders, share_of_office_orders
    manifest.sha256       sha256 of every other file under the root (KB-600)

Deterministic: fixed sort orders, no timestamps other than kb_version, no
random sampling. Standard library only. Re-running on the same inputs
reproduces every file byte-for-byte except kb_version if bumped.
"""
import collections
import csv
import hashlib
import json
import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parents[2]
CATALOG = ROOT / "scripts/kb/catalog.json"
ORDERS = ROOT / "docs/research/g1-fpds/navair_orders_fy2025.csv"
OUT = ROOT / "src/Naadap.Output/Resources/kb"
KB_VERSION = "2026-09-17.1"
MIN_ORDERS = 3          # a parent IDV needs this many distinct FY2025 orders to get its own row
MAX_SCOPE_DESCRIPTIONS = 6
NAVAIR = {"N00019", "N00421", "N68335", "N61340", "N68936", "N68520"}


def norm(s):
    return re.sub(r"[^A-Z0-9]", "", (s or "").upper())


def sha256_file(p):
    h = hashlib.sha256()
    with open(p, "rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def load_orders():
    rows = list(csv.DictReader(open(ORDERS, newline="")))
    orders = {}
    for r in rows:
        k = norm(r["award_id_piid"])
        if k and k not in orders:
            orders[k] = r          # first row seen per order, as in g1_extract.py
    return orders


def observed_stats(rs):
    """Statistics of a set of order rows (one row per distinct order)."""
    n = len(rs)
    offices = collections.Counter(r["awarding_office_code"] for r in rs)
    psc = collections.Counter(r["product_or_service_code"] for r in rs if r["product_or_service_code"])
    naics = collections.Counter(r["naics_code"] for r in rs if r["naics_code"])
    setaside = collections.Counter((r["type_of_set_aside_code"] or "NONE") for r in rs)
    small = sum(1 for r in rs if r["contracting_officers_determination_of_business_size_code"] == "S")
    services = sum(1 for r in rs if r["product_or_service_code"][:1].isalpha())
    ends = sorted(r["period_of_performance_current_end_date"] for r in rs if r["period_of_performance_current_end_date"])
    descs = collections.Counter(
        (r["prime_award_base_transaction_description"] or r["transaction_description"] or "").strip()
        for r in rs)
    descs.pop("", None)
    programs = collections.Counter(
        (r["dod_acquisition_program_description"] or "").strip() for r in rs)
    for k in list(programs):
        if not k or k.upper() in ("NONE", "N/A"):
            programs.pop(k)
    obligations = 0.0
    for r in rs:
        try:
            obligations += float(r["federal_action_obligation"] or 0)
        except ValueError:
            pass
    return {
        "orders_fy2025": n,
        "ordering_offices_fy2025": [{"office_code": o, "orders": c} for o, c in sorted(offices.items(), key=lambda x: (-x[1], x[0]))],
        "psc_set": [{"psc": p, "orders": c} for p, c in sorted(psc.items(), key=lambda x: (-x[1], x[0]))[:10]],
        "naics_set": [{"naics": p, "orders": c} for p, c in sorted(naics.items(), key=lambda x: (-x[1], x[0]))[:10]],
        "set_aside_mix": [{"code": p, "orders": c} for p, c in sorted(setaside.items(), key=lambda x: (-x[1], x[0]))],
        "small_business_share": round(small / n, 3) if n else 0.0,
        "services_share": round(services / n, 3) if n else 0.0,
        "latest_order_pop_end": ends[-1] if ends else "unknown",
        "obligations_fy2025_usd": round(obligations, 2),
        "dod_programs": [p for p, _ in sorted(programs.items(), key=lambda x: (-x[1], x[0]))[:5]],
        "_descs": [d for d, _ in sorted(descs.items(), key=lambda x: (-x[1], x[0]))],
    }


def infer_type(piid):
    c = piid[8:9] if len(piid) >= 9 and piid[:1] == "N" else ""
    return {"D": "Indefinite-delivery contract (IDIQ)", "A": "Blanket purchase agreement",
            "G": "Basic ordering agreement"}.get(c, "unknown")


def infer_tier(piid, owner):
    if owner in NAVAIR and piid[8:9] == "D":
        return 1
    if piid[8:9] == "A":
        return 2
    if piid[8:9] == "G":
        return 5
    if owner.startswith("N"):
        return 1          # another DON office's IDIQ: in-scope agency vehicle
    return 3              # non-DON vehicle


def main():
    cat = json.load(open(CATALOG))
    orders = load_orders()
    per_parent = collections.defaultdict(list)
    for r in orders.values():
        per_parent[norm(r["parent_award_id_piid"])].append(r)

    # ---- sources with hashes computed at build time
    sources = []
    for s in cat["sources"]:
        s = dict(s)
        if "path" in s:
            p = ROOT / s["path"]
            s["sha256"] = sha256_file(p) if p.exists() else "missing-at-build"
        sources.append(s)
    src_ids = {s["source_id"] for s in sources}

    fam_rx = [(f["family_id"], re.compile(f["piid_regex"]), f) for f in cat["families"]]

    def family_of(piid):
        for fid, rx, f in fam_rx:
            if rx.match(piid):
                return fid
        return None

    fam_members = collections.defaultdict(list)
    uncovered = {}
    for piid, rs in per_parent.items():
        fid = family_of(piid)
        if fid:
            fam_members[fid].append(piid)
        else:
            uncovered[piid] = rs

    fpds_prov = {"source_ids": ["S-FPDS-FY2025-097", "S-G1-EXTRACT"], "retrieved": "2026-09-15",
                 "note": "Observed on distinct FY2025 orders by the six NAVAIR contracting offices; a proxy, not the vehicle's own terms."}

    vehicles, families = [], []
    # ---- family rows
    for f in cat["families"]:
        fid = f["family_id"]
        members = sorted(fam_members.get(fid, []))
        rs = [r for m in members for r in per_parent[m]]
        st = observed_stats(rs) if rs else observed_stats([])
        curated_fields = ["name", "vehicle_type", "owner_office", "owner_name", "scope_text", "ordering_period_end",
                          "ceiling_usd", "eligible_ordering_activities", "socioeconomic_pools", "fee", "acquisition_path_tier"]
        curated = {"source_ids": f["source_ids"], "retrieved": "2026-09-17"}
        prov = {"default": fpds_prov, "overrides": {k: curated for k in curated_fields}}
        if "ordering_period_end_note" in f:
            prov["overrides"]["ordering_period_end"] = dict(curated, note=f["ordering_period_end_note"])
        rec = {
            "vehicle_id": fid, "family": fid, "row_kind": "family",
            "name": f["name"], "vehicle_type": f["vehicle_type"], "owner_office": f["owner_office"],
            "owner_name": f["owner_name"], "scope_text": f["scope_text"],
            "ordering_period_end": f["ordering_period_end"], "ceiling_usd": f["ceiling_usd"],
            "eligible_ordering_activities": f["eligible_ordering_activities"],
            "socioeconomic_pools": f["socioeconomic_pools"], "fee": f["fee"],
            "acquisition_path_tier": f["acquisition_path_tier"],
            "base_piids": members,
            **{k: v for k, v in st.items() if not k.startswith("_")},
            "observed_descriptions": st["_descs"][:MAX_SCOPE_DESCRIPTIONS],
            "provenance": prov,
        }
        vehicles.append(rec)
        families.append({"family_id": fid, "name": f["name"], "piid_regex": f["piid_regex"],
                         "member_piids_fy2025": len(members), "orders_fy2025": st["orders_fy2025"],
                         "source_ids": f["source_ids"]})

    # ---- data-derived rows for uncovered parents with enough orders
    for piid, rs in sorted(uncovered.items(), key=lambda x: (-len(x[1]), x[0])):
        if len(rs) < MIN_ORDERS:
            continue
        st = observed_stats(rs)
        owner = piid[:6] if piid[:1] == "N" else piid[:4]
        vtype = infer_type(piid)
        # Provenance: every field defaults to the FPDS extract; the overrides
        # name the fields whose value is a proxy or a literal.
        prov = {"default": fpds_prov, "overrides": {
            "ordering_period_end": dict(fpds_prov, note="Proxy: latest period-of-performance end among FY2025 orders. The IDV's own last-date-to-order was not retrieved (USAspending /api/v2/idvs/awards/ is an Increment 2 build step)."),
            "ceiling_usd": {"source_ids": [], "note": "Not available from order rows; literal 'unknown'."},
            "fee": {"source_ids": [], "note": "Agency vehicles observed carry no fee; literal 'none'."},
            "vehicle_type": dict(fpds_prov, note="Inferred from the ninth character of the PIID (D, A, G)."),
            "acquisition_path_tier": dict(fpds_prov, note="Inferred from owner office and PIID type per CORE-273."),
        }}
        vehicles.append({
            "vehicle_id": piid, "family": piid, "row_kind": "parent-idv",
            "name": (st["_descs"][0][:120] if st["_descs"] else piid),
            "vehicle_type": vtype, "owner_office": owner,
            "owner_name": cat["navair_offices"].get(owner, "unknown"),
            "scope_text": " | ".join(st["_descs"][:MAX_SCOPE_DESCRIPTIONS]) or "unknown",
            "ordering_period_end": st["latest_order_pop_end"], "ceiling_usd": "unknown",
            "eligible_ordering_activities": [o["office_code"] for o in st["ordering_offices_fy2025"]],
            "socioeconomic_pools": [m["code"] for m in st["set_aside_mix"]],
            "fee": "none", "acquisition_path_tier": infer_tier(piid, owner),
            "base_piids": [piid],
            **{k: v for k, v in st.items() if not k.startswith("_")},
            "observed_descriptions": st["_descs"][:MAX_SCOPE_DESCRIPTIONS],
            "provenance": prov,
        })

    # ---- office affinity over KB rows
    office_total = collections.Counter(r["awarding_office_code"] for r in orders.values())
    aff = []
    for v in vehicles:
        for o in v["ordering_offices_fy2025"]:
            aff.append((v["vehicle_id"], o["office_code"], o["orders"], round(o["orders"] / office_total[o["office_code"]], 6)))
    aff.sort(key=lambda x: (x[1], -x[2], x[0]))

    covered = sum(v["orders_fy2025"] for v in vehicles)
    coverage = covered / len(orders)

    # ---- write
    OUT.mkdir(parents=True, exist_ok=True)
    for old in OUT.glob("*"):
        old.unlink()
    (OUT / "kb_version").write_text(KB_VERSION + "\n")
    with open(OUT / "sources.jsonl", "w") as f:
        for s in sources:
            f.write(json.dumps(s, sort_keys=True) + "\n")
    with open(OUT / "families.jsonl", "w") as f:
        for r in families:
            f.write(json.dumps(r, sort_keys=True) + "\n")
    with open(OUT / "vehicles.jsonl", "w") as f:
        for r in vehicles:
            f.write(json.dumps(r, sort_keys=True) + "\n")
    with open(OUT / "office_affinity.tsv", "w") as f:
        f.write("vehicle_id\toffice_code\torders\tshare_of_office_orders\n")
        for a in aff:
            f.write("\t".join(str(x) for x in a) + "\n")
    (OUT / "README.md").write_text(f"""# NAADAP vehicle knowledge base, version {KB_VERSION}

Built by `scripts/kb/build_kb.py` from `scripts/kb/catalog.json` and the FY2025
FPDS extract `docs/research/g1-fpds/navair_orders_fy2025.csv`. Schema and
build pipeline: `docs/KB_SCHEMA.md`. Every file here except this README and
`manifest.sha256` is hashed in `manifest.sha256`; the pipeline verifies the
hashes before reading (derived requirement KB-630).

| File | Records | Content |
| --- | --- | --- |
| `families.jsonl` | {len(families)} | Catalog families defined by a PIID regex |
| `vehicles.jsonl` | {len(vehicles)} | {len(families)} family rows plus {len(vehicles)-len(families)} parent-IDV rows with at least {MIN_ORDERS} distinct FY2025 NAVAIR orders |
| `office_affinity.tsv` | {len(aff)} | Orders per (vehicle, contracting office) and the share of that office's FY2025 orders under vehicles |
| `sources.jsonl` | {len(sources)} | Provenance sources with SHA-256 |

Coverage: rows account for {covered:,} of {len(orders):,} distinct FY2025 NAVAIR
orders under vehicles ({coverage:.1%}). The literal `unknown` marks a field
the sources do not supply; nothing is inferred into it.
""")
    manifest = []
    for p in sorted(OUT.glob("*")):
        if p.name in ("manifest.sha256", "README.md"):
            continue
        manifest.append(f"{sha256_file(p)}  {p.name}")
    (OUT / "manifest.sha256").write_text("\n".join(manifest) + "\n")
    print(f"kb {KB_VERSION}: {len(families)} families, {len(vehicles)} vehicle rows, "
          f"{len(aff)} affinity rows, coverage {covered}/{len(orders)} = {coverage:.1%}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
