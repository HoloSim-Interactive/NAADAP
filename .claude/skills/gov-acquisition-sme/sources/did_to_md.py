import pymupdf, pathlib, re, subprocess, tempfile, os, datetime, sys
root = pathlib.Path(".")
today = datetime.date.today().isoformat()
def rev_key(f):
    m = re.search(r"_Revision_([A-Z])_", f.name)
    if m: return m.group(1)
    m = re.search(r"DI-[A-Z]{4}-\d{5}([A-Z])\.pdf$", f.name)
    return m.group(1) if m else "0"
for d in sorted(p for p in root.iterdir() if p.is_dir() and p.name.startswith("DI-")):
    cands = [f for f in d.glob("*.pdf") if "Notice" not in f.name and not f.name.endswith("-2009.pdf")]
    if not cands: continue
    latest = max(cands, key=rev_key)
    rev = rev_key(latest); rev = "" if rev == "0" else rev
    doc = pymupdf.open(latest)
    pages = []; method = "text"
    for i, page in enumerate(doc):
        t = page.get_text()
        body = [l for l in t.splitlines() if l.strip() and "assist.dla.mil" not in l and "Check the source to verify" not in l]
        if len(body) < 8:
            png = os.path.join(tempfile.mkdtemp(), f"p{i}.png"); page.get_pixmap(dpi=300).save(png)
            t = subprocess.run(["tesseract", png, "-", "--psm", "4"], capture_output=True, text=True).stdout
            method = "ocr"
        pages.append(t.strip())
    out = d / f"{d.name}{rev}.md"
    hdr = f"""# {d.name}{rev} — extracted text

| Field | Value |
| --- | --- |
| Source file | `{latest.name}` |
| Pages | {doc.page_count} |
| Extraction | {"OCR (tesseract, 300 dpi, page segmentation mode 4) because ASSIST serves this revision as a scanned image; expect character errors, especially in the DD Form 1664 header boxes and in tables. The PDF is authoritative." if method=="ocr" else "Embedded text layer (PyMuPDF); layout whitespace normalized. The PDF is authoritative."} |
| Extracted | {today} |
| Purpose | Searchable, quotable text for agents and humans. Not a substitute for the PDF when citing. |

"""
    body = "\n\n".join(f"---\n\n<!-- page {i+1} -->\n\n{p}" for i, p in enumerate(pages))
    out.write_text(hdr + body + "\n")
    print(f"{out}  [{method}, {doc.page_count}p, {len(body)} chars]", flush=True)
