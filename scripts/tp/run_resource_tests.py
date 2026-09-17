#!/usr/bin/env python3
"""TP-220, TP-230, TP-520 runner (docs/RTVM.md Test Procedures).

Two modes, chosen automatically:

  docker  A Docker daemon is reachable. The procedures run as written: the
          built image under --cpus / --memory limits (TP-220 at 1c/2GB;
          TP-230 at 1c/2GB, 4c/8GB, 8c/16GB; TP-520 with N containers).
          This is the formal execution.

  proxy   No daemon. The published CLI runs in-process with a CPU affinity
          mask (taskset), DOTNET_PROCESSOR_COUNT, and DOTNET_GCHeapHardLimit
          set to the tier, and peak resident set is read from
          /proc/<pid>/status (VmHWM). This bounds CPU and managed heap but
          is not a cgroup and cannot OOM-kill; results are labeled PROXY
          and do not close TP-220/230/520. Use it for early warning.

Usage:
  run_resource_tests.py --input tests/fixtures/reference-20 --out <dir>
                        [--image naadap:svr1] [--replicas 4] [--json results.json]

Every timing is wall-clock from process start to exit. Outputs equality is
checked on manifest.json with volatile paths ignored (there are none) so a
byte-identical manifest is the equality criterion.
"""
import argparse
import hashlib
import json
import os
import pathlib
import shutil
import subprocess
import sys
import threading
import time

TIERS = [("1c-2g", 1, 2), ("4c-8g", 4, 8), ("8c-16g", 8, 16)]
ROOT = pathlib.Path(__file__).resolve().parents[2]
CLI_DLL = ROOT / "src/Naadap.Cli/bin/Release/net9.0/Naadap.Cli.dll"


def docker_available():
    try:
        return subprocess.run(["docker", "info"], capture_output=True, timeout=20).returncode == 0
    except Exception:
        return False


def manifest_hash(out_dir):
    p = pathlib.Path(out_dir) / "manifest.json"
    return hashlib.sha256(p.read_bytes()).hexdigest() if p.exists() else None


def run_docker(image, inp, out, cores, gb, label):
    out = pathlib.Path(out); out.mkdir(parents=True, exist_ok=True)
    cmd = ["docker", "run", "--rm", "--network", "none", f"--cpus={cores}", f"--memory={gb}g", "--memory-swap", f"{gb}g",
           "-v", f"{pathlib.Path(inp).resolve()}:/data/in:ro", "-v", f"{out.resolve()}:/data/out",
           image, "--input", "/data/in", "--output", "/data/out"]
    t0 = time.time(); r = subprocess.run(cmd, capture_output=True, text=True); dt = time.time() - t0
    return {"label": label, "mode": "docker", "cores": cores, "memory_gb": gb, "exit": r.returncode,
            "wall_s": round(dt, 2), "manifest_sha256": manifest_hash(out), "stderr_tail": r.stderr[-400:]}


def run_proxy(inp, out, cores, gb, label):
    out = pathlib.Path(out); out.mkdir(parents=True, exist_ok=True)
    env = dict(os.environ, DOTNET_PROCESSOR_COUNT=str(cores), DOTNET_GCHeapHardLimit=hex(gb * (1 << 30)),
               DOTNET_TieredPGO="0")
    cpus = ",".join(str(i) for i in range(min(cores, os.cpu_count() or 1)))
    cmd = ["taskset", "-c", cpus, "dotnet", str(CLI_DLL), "--input", str(inp), "--output", str(out)]
    peak = {"kb": 0}
    t0 = time.time()
    proc = subprocess.Popen(cmd, env=env, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE, text=True)

    def sample():
        while proc.poll() is None:
            try:
                for line in open(f"/proc/{proc.pid}/status"):
                    if line.startswith("VmHWM:"):
                        peak["kb"] = max(peak["kb"], int(line.split()[1]))
            except FileNotFoundError:
                break
            time.sleep(0.05)
    th = threading.Thread(target=sample, daemon=True); th.start()
    _, err = proc.communicate(); th.join(timeout=1); dt = time.time() - t0
    return {"label": label, "mode": "proxy", "cores": cores, "memory_gb": gb, "exit": proc.returncode,
            "wall_s": round(dt, 2), "peak_rss_mb": round(peak["kb"] / 1024, 1),
            "manifest_sha256": manifest_hash(out), "stderr_tail": err[-400:]}


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--input", required=True); ap.add_argument("--out", required=True)
    ap.add_argument("--image", default="naadap:svr1"); ap.add_argument("--replicas", type=int, default=4)
    ap.add_argument("--json", default=None); ap.add_argument("--force-proxy", action="store_true")
    a = ap.parse_args()
    mode = "docker" if (docker_available() and not a.force_proxy) else "proxy"
    run = (lambda inp, out, c, g, l: run_docker(a.image, inp, out, c, g, l)) if mode == "docker" else run_proxy
    base = pathlib.Path(a.out); shutil.rmtree(base, ignore_errors=True); base.mkdir(parents=True)
    results = {"mode": mode, "input": a.input, "started": time.strftime("%Y-%m-%dT%H:%M:%SZ", time.gmtime()), "runs": []}
    print(f"mode: {mode}")

    # TP-230 tiers (the 1c/2GB run is also TP-220)
    for label, c, g in TIERS:
        r = run(a.input, base / f"tp230-{label}", c, g, f"TP-230 {label}")
        results["runs"].append(r); print(json.dumps(r))
    tier1 = results["runs"][0]
    results["tp220"] = {"wall_s": tier1["wall_s"], "limit_s": 1800, "pass": tier1["exit"] == 0 and tier1["wall_s"] <= 1800,
                        "design_target_s": 300, "within_design_target": tier1["wall_s"] <= 300}
    results["tp230"] = {"pass": all(r["exit"] == 0 and r["manifest_sha256"] for r in results["runs"][:3]),
                        "identical_manifests": len({r["manifest_sha256"] for r in results["runs"][:3]}) == 1}

    # TP-520 (a): N concurrent replicas, same input, invariance
    n = a.replicas; threads = []; conc = [None] * n
    def worker(i):
        conc[i] = run(a.input, base / f"tp520a-replica{i+1}", 1, 2, f"TP-520a replica {i+1}")
    t0 = time.time()
    for i in range(n):
        t = threading.Thread(target=worker, args=(i,)); threads.append(t); t.start()
    for t in threads:
        t.join()
    conc_wall = time.time() - t0
    results["runs"].extend(conc)
    results["tp520a"] = {"replicas": n, "identical_to_baseline": all(r["manifest_sha256"] == tier1["manifest_sha256"] for r in conc),
                         "all_exit_0": all(r["exit"] == 0 for r in conc), "concurrent_wall_s": round(conc_wall, 2)}

    # TP-520 (b): throughput, N sets sequential on one replica vs concurrent on N
    sets = []
    for i in range(n):
        d = base / f"set{i+1}"; shutil.copytree(a.input, d); sets.append(d)
    t0 = time.time(); seq = [run(sets[i], base / f"tp520b-seq{i+1}", 1, 2, f"TP-520b sequential set {i+1}") for i in range(n)]
    seq_wall = time.time() - t0
    par = [None] * n; threads = []
    def pworker(i):
        par[i] = run(sets[i], base / f"tp520b-par{i+1}", 1, 2, f"TP-520b concurrent set {i+1}")
    t0 = time.time()
    for i in range(n):
        t = threading.Thread(target=pworker, args=(i,)); threads.append(t); t.start()
    for t in threads:
        t.join()
    par_wall = time.time() - t0
    results["runs"].extend(seq + par)
    results["tp520b"] = {"sets": n, "sequential_wall_s": round(seq_wall, 2), "concurrent_wall_s": round(par_wall, 2),
                         "speedup": round(seq_wall / par_wall, 2) if par_wall else None,
                         "outputs_equal": all(seq[i]["manifest_sha256"] == par[i]["manifest_sha256"] for i in range(n)),
                         "pass": par_wall < seq_wall and all(seq[i]["manifest_sha256"] == par[i]["manifest_sha256"] for i in range(n))}
    for k in ("tp220", "tp230", "tp520a", "tp520b"):
        print(k, json.dumps(results[k]))
    if a.json:
        pathlib.Path(a.json).write_text(json.dumps(results, indent=2) + "\n")
    return 0


if __name__ == "__main__":
    sys.exit(main())
