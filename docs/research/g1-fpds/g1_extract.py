#!/usr/bin/env python3
"""G1 premise check: extract NAVAIR orders-under-vehicles from the USAspending
DoD bulk archive and produce the four summaries cited in G1-FINDINGS.md.

Recovered verbatim (logic, constants, seeds) from the 2026-09-15 session in
which the findings were produced, so that the committed outputs in this
directory are reproducible. Standard library only.

Usage:
    g1_extract.py pass1 <FYyyyy_097_Contracts_Full_yyyymmdd.zip>   # ~2 min
    g1_extract.py pass2                                            # reads navair_orders_fy2025.csv
    g1_extract.py pass3                                            # reads navair_orders_fy2025.csv
    g1_extract.py pass4 <archive.zip>                              # ~2 min
    g1_extract.py all   <archive.zip>

Outputs (written to the current directory):
    navair_orders_fy2025.csv, g1_pass1_summary.json, g1_pass2_summary.json,
    g1_pass3_breadth.json, g1_pass4_pricing.json
"""
import collections
import csv
import io
import json
import random
import re
import sys
import time
import zipfile

NAVAIR = {"N00019", "N00421", "N68335", "N61340", "N68936", "N68520"}
DON_SUB_AGENCY = "1700"
ORDERS_CSV = "navair_orders_fy2025.csv"

KEEP = [
    "award_id_piid", "parent_award_id_piid", "awarding_office_code", "awarding_office_name",
    "awarding_sub_agency_code", "product_or_service_code", "product_or_service_code_description",
    "naics_code", "naics_description", "transaction_description",
    "prime_award_base_transaction_description", "type_of_set_aside_code", "idv_type_code",
    "award_type_code", "action_type_code", "action_date", "period_of_performance_start_date",
    "period_of_performance_current_end_date", "federal_action_obligation", "extent_competed_code",
    "number_of_offers_received", "primary_place_of_performance_state_code", "recipient_uei",
    "contracting_officers_determination_of_business_size_code",
    "dod_acquisition_program_description", "dod_claimant_program_description",
]

# Catalog-vehicle PIID patterns from contract-vehicles.md, applied to hyphen-stripped PIIDs.
CATALOG = {
    "SeaPort-NxG (N00178-yy-D-nnnn)": re.compile(r"^N00178\d{2}D\d{4}$"),
    "NAWCTSD TSC IV (N61340-18-D-50nn)": re.compile(r"^N6134018D50\d{2}$"),
    "NAWCTSD FTSS V (N61340-22-D-1nnn/2nnn)": re.compile(r"^N6134022D[12]\d{3}$"),
    "NAWCTSD PACRM (N61340-21-D-000n)": re.compile(r"^N6134021D000\d$"),
    "NAWCAD SCI MAC (N00421-25-D-00nn)": re.compile(r"^N0042125D00\d{2}$"),
    "NAWCAD Cyber IDIQ (N00421-23-D-0021)": re.compile(r"^N0042123D0021$"),
    "GSA MAS (GS-xxF / 47QxCA/RAA)": re.compile(r"^(GS\d{2}F|47Q[A-Z]{3})"),
    "GSA OASIS+ (47QRC)": re.compile(r"^47QRC"),
    "NASA SEWP (NNG15SC)": re.compile(r"^NNG15SC"),
    "NAVAIR HQ BOA/IDV (N00019-yy-[DG]-)": re.compile(r"^N00019\d{2}[DG]\d{4}$"),
}

# Vehicle families for the pricing pass.
FAMILIES = [
    ("SeaPort-NxG", re.compile(r"^N00178\d{2}D\d{4}$")),
    ("NAWCAD SCI MAC", re.compile(r"^N0042125D00\d{2}$")),
    ("TSC IV", re.compile(r"^N6134018D50\d{2}$")),
    ("FTSS V", re.compile(r"^N6134022D[12]\d{3}$")),
    ("NAVAIR HQ BOA (G)", re.compile(r"^N00019\d{2}G\d{4}$")),
]


def norm(s):
    return re.sub(r"[^A-Z0-9]", "", (s or "").upper())


def csv_members(z):
    return [m for m in z.infolist() if m.filename.endswith(".csv")]


def load_orders():
    """Distinct orders: first row seen per hyphen-stripped award_id_piid."""
    rows = list(csv.DictReader(open(ORDERS_CSV, newline="")))
    orders = {}
    for r in rows:
        k = norm(r["award_id_piid"])
        if k and k not in orders:
            orders[k] = r
    return rows, orders


def pass1(archive):
    t0 = time.time()
    z = zipfile.ZipFile(archive)
    n_dod = n_don = n_don_parent = n_navair = n_navair_parent = 0
    don_office = collections.Counter()
    out = open(ORDERS_CSV, "w", newline="")
    w = None
    for m in csv_members(z):
        with z.open(m) as f:
            r = csv.reader(io.TextIOWrapper(f, encoding="utf-8", errors="replace"))
            hdr = next(r)
            idx = {h: i for i, h in enumerate(hdr)}
            if w is None:
                w = csv.writer(out)
                w.writerow(KEEP)
            ki = [idx[k] for k in KEEP]
            i_sub, i_off, i_par = idx["awarding_sub_agency_code"], idx["awarding_office_code"], idx["parent_award_id_piid"]
            for row in r:
                n_dod += 1
                if len(row) <= max(i_sub, i_off, i_par):
                    continue
                if row[i_sub] == DON_SUB_AGENCY:
                    n_don += 1
                    has_par = bool(row[i_par].strip())
                    if has_par:
                        n_don_parent += 1
                        don_office[row[i_off]] += 1
                    if row[i_off] in NAVAIR:
                        n_navair += 1
                        if has_par:
                            n_navair_parent += 1
                            w.writerow([row[i] for i in ki])
        print(f"  done {m.filename}  cum DoD rows={n_dod:,}  elapsed={time.time()-t0:.0f}s", file=sys.stderr)
    out.close()
    summary = {
        "dod_transactions": n_dod, "don_transactions": n_don,
        "don_transactions_with_parent": n_don_parent, "navair_transactions": n_navair,
        "navair_transactions_with_parent": n_navair_parent,
        "top_don_offices_with_parent": don_office.most_common(25),
        "elapsed_s": round(time.time() - t0),
    }
    json.dump(summary, open("g1_pass1_summary.json", "w"), indent=2)
    print(json.dumps(summary, indent=2))
    print("PASS1 COMPLETE")


def pass2():
    rows, orders = load_orders()
    print(f"NAVAIR transactions with parent PIID: {len(rows):,}")
    print(f"Distinct orders (dedupe on award_id_piid): {len(orders):,}")
    parents = collections.Counter(norm(r["parent_award_id_piid"]) for r in orders.values())
    print(f"Distinct parent IDVs (vehicles): {len(parents):,}")
    print("\n=== distinct orders per NAVAIR office ===")
    for off, c in collections.Counter(r["awarding_office_code"] for r in orders.values()).most_common():
        nm = next(r["awarding_office_name"] for r in orders.values() if r["awarding_office_code"] == off)
        print(f"  {off}  {c:5,}  {nm}")
    print("\n=== catalog-vehicle matches among parent IDVs (distinct orders / distinct parent PIIDs) ===")
    matched = set()
    for name, rx in CATALOG.items():
        hits = {p: c for p, c in parents.items() if rx.match(p)}
        matched |= set(hits)
        print(f"  {sum(hits.values()):5,} orders  {len(hits):4,} PIIDs  {name}")
        for p, c in sorted(hits.items(), key=lambda x: -x[1])[:3]:
            print(f"           {p}  {c}")
    un = sum(c for p, c in parents.items() if p not in matched)
    print(f"  {un:5,} orders under parents matching no catalog pattern ({len(parents)-len(matched):,} PIIDs)")
    print("\n=== top 15 parent IDVs overall by distinct orders ===")
    for p, c in parents.most_common(15):
        off = collections.Counter(r["awarding_office_code"] for r in orders.values()
                                  if norm(r["parent_award_id_piid"]) == p).most_common(2)
        print(f"  {p:16} {c:5,}  ordered by {off}")
    print("\n=== top 12 PSC among orders ===")
    for psc, c in collections.Counter(r["product_or_service_code"] for r in orders.values()).most_common(12):
        d = next((r["product_or_service_code_description"] for r in orders.values()
                  if r["product_or_service_code"] == psc), "")
        print(f"  {psc:5} {c:5,}  {d[:60]}")
    print("\n=== award_type_code / idv_type_code / business size ===")
    print("  award_type:", collections.Counter(r["award_type_code"] for r in orders.values()).most_common())
    print("  idv_type  :", collections.Counter(r["idv_type_code"] for r in orders.values()).most_common())
    print("  biz size  :", collections.Counter(
        r["contracting_officers_determination_of_business_size_code"] for r in orders.values()).most_common())
    print("  set-aside :", collections.Counter(r["type_of_set_aside_code"] for r in orders.values()).most_common(6))
    print("\n=== description quality: 8 samples (transaction_description | base_description) ===")
    random.seed(7)
    for r in random.sample(list(orders.values()), 8):
        print(f"  [{r['awarding_office_code']} -> {norm(r['parent_award_id_piid'])}] {r['transaction_description'][:110]!r}")
        base = r["prime_award_base_transaction_description"]
        if base and base != r["transaction_description"]:
            print(f"      base: {base[:110]!r}")
    empty = sum(1 for r in orders.values() if not r["transaction_description"].strip())
    lens = sorted(len(r["transaction_description"]) for r in orders.values())
    print(f"\n  empty descriptions: {empty:,} / {len(orders):,}   median len {lens[len(lens)//2]}   max {lens[-1]}")
    json.dump({"distinct_orders": len(orders), "distinct_parents": len(parents),
               "top_parents": parents.most_common(40)}, open("g1_pass2_summary.json", "w"), indent=2)
    print("\nPASS2 COMPLETE")


def pass3():
    _, orders = load_orders()

    def is_services(psc):
        return bool(psc) and psc[0].isalpha()

    by_parent = collections.defaultdict(lambda: {
        "orders": 0, "offices": set(), "programs": set(), "svc": 0, "small": 0,
        "psc": collections.Counter(), "base_desc": collections.Counter()})
    for r in orders.values():
        b = by_parent[norm(r["parent_award_id_piid"])]
        b["orders"] += 1
        b["offices"].add(r["awarding_office_code"])
        prog = (r["dod_acquisition_program_description"] or "").strip()
        if prog and prog.upper() not in ("NONE", "N/A", ""):
            b["programs"].add(prog[:40])
        if is_services(r["product_or_service_code"]):
            b["svc"] += 1
        if r["contracting_officers_determination_of_business_size_code"] == "S":
            b["small"] += 1
        b["psc"][r["product_or_service_code"]] += 1
        bd = (r["prime_award_base_transaction_description"] or r["transaction_description"] or "").strip()[:70]
        if bd:
            b["base_desc"][bd] += 1

    T_ENT = "ENTERPRISE (>=3 offices or 2 offices+3 programs)"
    T_MULTI = "MULTI (2 offices or >=3 programs)"
    T_SOHV = "SINGLE-OFFICE HIGH-VOLUME (>=10 orders)"
    T_LOW = "SINGLE/LOW"

    def tier(b):
        o, p, n = len(b["offices"]), len(b["programs"]), b["orders"]
        if o >= 3 or (o >= 2 and p >= 3):
            return T_ENT
        if o >= 2 or p >= 3:
            return T_MULTI
        if n >= 10:
            return T_SOHV
        return T_LOW

    tiers, tier_orders = collections.Counter(), collections.Counter()
    for p, b in by_parent.items():
        t = tier(b)
        tiers[t] += 1
        tier_orders[t] += b["orders"]
    print(f"=== breadth tiers across {len(by_parent):,} parent IDVs ===")
    for t in [T_ENT, T_MULTI, T_SOHV, T_LOW]:
        print(f"  {tiers[t]:5,} PIIDs  {tier_orders[t]:5,} orders  {t}")

    def pct(b, k):
        return 100 * b[k] // max(1, b["orders"])

    print("\n=== ENTERPRISE-tier parents ranked by distinct offices then orders (top 25) ===")
    ent = [(p, b) for p, b in by_parent.items() if tier(b) == T_ENT]
    ent.sort(key=lambda x: (-len(x[1]["offices"]), -x[1]["orders"]))
    for p, b in ent[:25]:
        top_desc = b["base_desc"].most_common(1)[0][0] if b["base_desc"] else ""
        print(f"  {p:16} offs={len(b['offices'])} progs={len(b['programs']):2} orders={b['orders']:4} "
              f"svc={pct(b,'svc'):3}% SB={pct(b,'small'):3}%  {sorted(b['offices'])}  {top_desc!r}")
    print("\n=== SINGLE-OFFICE HIGH-VOLUME parents, top 15 by orders (candidate program-specific IDIQs = consolidation SOURCES) ===")
    sv = [(p, b) for p, b in by_parent.items() if tier(b) == T_SOHV]
    sv.sort(key=lambda x: -x[1]["orders"])
    for p, b in sv[:15]:
        top_desc = b["base_desc"].most_common(1)[0][0] if b["base_desc"] else ""
        print(f"  {p:16} orders={b['orders']:4} svc={pct(b,'svc'):3}% off={list(b['offices'])[0]} "
              f"progs={list(b['programs'])[:2]}  {top_desc!r}")
    print("\n=== SeaPort-NxG base PIIDs by award FY (from N00178-yy) ===")
    sp = collections.Counter(p[6:8] for p in by_parent if re.match(r"^N00178\d{2}D\d{4}$", p))
    print("  ", dict(sorted(sp.items())))
    print("\n=== services-vs-hardware split of all orders ===")
    svc_total = sum(b["svc"] for b in by_parent.values())
    print(f"  services PSC (letter-prefixed): {svc_total:,} / {len(orders):,} = {100*svc_total//len(orders)}%")
    json.dump({
        "tiers": dict(tiers), "tier_orders": dict(tier_orders),
        "enterprise": [{
            "piid": p, "offices": sorted(b["offices"]), "n_programs": len(b["programs"]),
            "orders": b["orders"], "svc_pct": pct(b, "svc"), "sb_pct": pct(b, "small"),
            "top_desc": (b["base_desc"].most_common(1)[0][0] if b["base_desc"] else ""),
            "top_psc": b["psc"].most_common(3)} for p, b in ent],
    }, open("g1_pass3_breadth.json", "w"), indent=2)
    print("\nPASS3 COMPLETE")


def pass4(archive):
    z = zipfile.ZipFile(archive)
    pricing = collections.defaultdict(collections.Counter)
    seen = set()
    for m in csv_members(z):
        with z.open(m) as f:
            r = csv.reader(io.TextIOWrapper(f, encoding="utf-8", errors="replace"))
            hdr = next(r)
            idx = {h: i for i, h in enumerate(hdr)}
            i_sub, i_off, i_par, i_aw = (idx["awarding_sub_agency_code"], idx["awarding_office_code"],
                                         idx["parent_award_id_piid"], idx["award_id_piid"])
            i_pc, i_pd = idx.get("type_of_contract_pricing_code"), idx.get("type_of_contract_pricing")
            if i_pc is None:
                print("NO type_of_contract_pricing_code COLUMN", file=sys.stderr)
                break
            for row in r:
                if len(row) <= max(i_sub, i_off, i_par, i_pc):
                    continue
                if row[i_sub] != DON_SUB_AGENCY or row[i_off] not in NAVAIR or not row[i_par].strip():
                    continue
                aw, par = norm(row[i_aw]), norm(row[i_par])
                if aw in seen:
                    continue
                seen.add(aw)
                for name, rx in FAMILIES:
                    if rx.match(par):
                        pricing[name][f"{row[i_pc]} {row[i_pd][:28]}"] += 1
                        break
    print("=== type_of_contract_pricing on distinct orders, by vehicle family (FY2025, NAVAIR offices) ===")
    for name, _ in FAMILIES:
        c = pricing[name]
        print(f"\n  {name}  (n={sum(c.values())})")
        for k, v in c.most_common(8):
            print(f"     {v:4}  {k}")
    json.dump({k: dict(v) for k, v in pricing.items()}, open("g1_pass4_pricing.json", "w"), indent=2)
    print("\nPASS4 COMPLETE")


def main(argv):
    if len(argv) < 2 or argv[1] not in ("pass1", "pass2", "pass3", "pass4", "all"):
        print(__doc__)
        return 2
    cmd = argv[1]
    if cmd in ("pass1", "pass4", "all") and len(argv) < 3:
        print("archive path required", file=sys.stderr)
        return 2
    if cmd == "pass1":
        pass1(argv[2])
    elif cmd == "pass2":
        pass2()
    elif cmd == "pass3":
        pass3()
    elif cmd == "pass4":
        pass4(argv[2])
    else:
        pass1(argv[2]); pass2(); pass3(); pass4(argv[2])
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
