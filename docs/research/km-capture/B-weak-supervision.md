# Area B — Weak Supervision, Data Programming, Programmatic Labeling

**Research date:** 2026-09-15
**Scope:** Can the weak-supervision literature turn stated SME heuristics into training signal for NAADAP, under: offline Docker, deterministic (same top-5 in ≥95% of runs), 1 CPU / 2 GB / 30 min per 20-doc set, no LLM in the core path, C#/.NET 9, minimal deps, citable evidence for every output?

**Citation policy used here:** every paper below was fetched. Where I quote a number or an equation I pulled it from the paper's own text (ar5iv HTML or the PDF, extracted locally with PyMuPDF), not from memory. Anything I could not retrieve is marked **UNVERIFIED**. Claims that are my own engineering judgment rather than a finding in a paper are marked **[assessment]**.

---

## 0. Executive answer

The *framing* of weak supervision is an excellent fit for NAADAP and the *machinery* is mostly a bad fit.

- **Good fit:** the labeling-function abstraction — a named, versioned, testable function `LF: Document → VehicleLabel | ABSTAIN` that encodes exactly one SME heuristic. This is a knowledge-capture format, it is trivially deterministic, it is trivially auditable, and each firing is a citable evidence record ("LF `psc_r_nawcad_office` fired because PSC=R425 and issuing office DoDAAC=N00421"). This part of the literature survives every criticism leveled at the rest of it.
- **Bad fit as specified:** the *label model* — the latent-variable generative model that de-noises LF votes without ground truth. Every label model in this literature has a sample-complexity bound in the number of **unlabeled data points `n`**, and the bounds are `O(n^{-1/2})`. At NAADAP's stated working-set size (20 documents) they are vacuous. They also all require a known class balance `P(Y)`, and they all degrade badly under correlated labeling functions — and SME acquisition heuristics keyed on PSC / issuing office / NAICS / dollar threshold are *strongly* correlated given the true vehicle.
- **The specific recommendation** (detail in §9): adopt the LF abstraction now with a deterministic weighted-vote aggregator; hold the label model in reserve behind a feature flag; if and when NAADAP has ≥ ~5,000 historical records to fit on, implement **the FlyingSquid triplet estimator** (closed-form, no SGD, no random init, ~500 lines of C# with no linear-algebra dependency in the conditionally independent case), **fit offline at container-build time**, and ship the fitted parameters as a versioned data artifact so that run-time inference is pure arithmetic.

---

## 1. Data Programming (Ratner et al., NeurIPS 2016)

### 1.1 Citation — VERIFIED

> Alexander J. Ratner, Christopher M. De Sa, Sen Wu, Daniel Selsam, Christopher Ré.
> **"Data Programming: Creating Large Training Sets, Quickly."**
> *Advances in Neural Information Processing Systems 29 (NIPS 2016).*
> arXiv:1605.07723.
> Verified at `https://arxiv.org/abs/1605.07723` and at the NeurIPS proceedings page
> `https://proceedings.neurips.cc/paper/2016/hash/6709e8d64a5f47269ed5cea9f625f7ab-Abstract.html`
> (author list and title confirmed identical on both).

Mechanism below extracted from the paper's own text (ar5iv HTML of 1605.07723, cross-checked against the PDF downloaded locally).

### 1.2 What a labeling function is

```
λ_i : X → {−1, 0, +1}
```

A user-defined function encoding one domain heuristic. It returns a non-zero label on some subset of objects and `0` (abstain) elsewhere. It has **unknown accuracy** and **unknown correlation** with other LFs. There are `m` of them, applied to an unlabeled set `S`, producing an `|S| × m` label matrix `Λ`.

The output domain is binary-plus-abstain. Multi-class is not in the original formulation.

### 1.3 The independent generative model

The joint density over LF outputs `Λ` and the latent true label `Y`:

```
μ_{α,β}(Λ, Y) = ½ · ∏_{i=1..m} [ β_i α_i · 1{Λ_i = Y}
                                + β_i (1 − α_i) · 1{Λ_i = −Y}
                                + (1 − β_i) · 1{Λ_i = 0} ]
```

- `α_i` = **accuracy** of LF `i` — P(correct | it fired).
- `β_i` = **coverage / labeling propensity** of LF `i` — P(it fires).
- The `½` is the assumed class balance (`P(Y=1) = ½`).
- The product form *is* the conditional-independence assumption: LFs are independent given `Y`.

**Important caveat that is usually dropped when this model is cited.** The paper explicitly constrains the parameter range *for the theory*:

> "we will assume here that `0.3 ≤ β_i ≤ 0.5` and `0.8 ≤ α_i ≤ 0.9`. We note that while these arbitrary constraints can be changed, they are roughly consistent with our applied experience, where users tend to write high-accuracy and high-coverage labeling functions."

So Theorem 1 holds in a regime where every LF is already 80–90% accurate and covers 30–50% of the corpus. **[assessment]** NAADAP's SME heuristics will be far outside this: an office-code heuristic covers maybe 2–5% of a mixed corpus.

### 1.4 The objective — learning `(α, β)` with no ground truth

Maximum marginal likelihood, marginalizing out the unobserved `Y`:

```
(α̂, β̂) = argmax_{α,β}  Σ_{x∈S}  log P_{(Λ,Y)~μ_{α,β}} ( Λ = λ(x) )

         = argmax_{α,β}  Σ_{x∈S}  log ( Σ_{y' ∈ {−1,+1}} μ_{α,β}( λ(x), y' ) )
```

This is the whole trick: you never see `Y`, but the *pattern of agreements and disagreements* among LFs identifies their accuracies, because a high-accuracy LF agrees with the consensus more often than a low-accuracy one.

### 1.5 How it is optimized — **stochastic gradient descent**

The paper states plainly: "we use stochastic gradient descent to solve this problem." For the dependent (correlated) model it is worse: "we typically invoke stochastic gradient descent, using Gibbs sampling to sample from the distributions used in the gradient update."

**Determinism verdict: FAIL.** SGD over a non-convex marginal likelihood, with Gibbs sampling in the correlated case, is doubly stochastic. See §7 for the measured consequences in the Snorkel reference implementation.

### 1.6 The noise-aware discriminative loss

Once `(α̂, β̂)` are estimated, the downstream model is trained on *expected* loss under the generative posterior rather than on hard labels:

```
ŵ = argmin_w  (1/|S|) Σ_{x∈S}  E_{(Λ,Y) ~ μ_{α̂,β̂}} [ log(1 + e^{−w^T f(x) Y}) | Λ = λ(x) ]  +  ρ‖w‖²
```

i.e. the standard logistic loss with `Y` replaced by its posterior given the observed LF votes. Equivalent in practice to training on *soft* probabilistic labels. WRENCH (§4) later confirms empirically that soft labels beat hard labels, especially for deeper end models.

### 1.7 Theorem 1 (sample complexity) — exact statement and assumptions

From the PDF:

> **Theorem 1.** Suppose that we run data programming, solving the problems in (2) and (3) using stochastic gradient descent to produce `(α̂, β̂)` and `ŵ`. Suppose further that our setup satisfies the conditions (4), (5), and (6), and suppose that `m ≥ 2000`. Then for any `ε > 0`, if the number of labeling functions `m` and the size of the input dataset `S` are large enough that
>
> ```
> |S| ≥ (356 / ε²) · log( m / 3ε )
> ```
>
> then `E[‖α̂ − α*‖²] ≤ mε²`, `E[‖β̂ − β*‖²] ≤ mε²`, and the generalization risk is bounded by `χ + O(...)`.

Assumptions (4)(5)(6):
1. The data is actually generated by some `μ_{α*,β*}` in the family (well-specification).
2. `Y ⊥ f(x) | λ(x)` — the LF outputs are a sufficient statistic for the label given the features.
3. The downstream learner has bounded generalization risk `χ`.
Plus the parameter-range restriction of §1.3 and `m ≥ 2000` labeling functions.

**The scaling result is `O(1/ε²)` in the number of UNLABELED points — the same asymptotic rate as supervised learning in labeled points.** That is the paper's headline and it is a genuine result.

**[assessment] Why this kills the naive NAADAP application.** `m ≥ 2000` labeling functions is not a typo; it is a condition of the simplified theorem. NAADAP will have perhaps 20–80 SME heuristics. And `|S| ≥ 356/ε² · log(m/3ε)`: for `ε = 0.1` that is `35,600 · log(...)` unlabeled documents. Against a 20-document working set the bound says nothing at all. Weak supervision is a *large-unlabeled-corpus* technique. If NAADAP cannot assemble thousands of historical procurement records to fit on, the label model has no statistical basis.

### 1.8 Handling LF dependencies — the dependent model

Replaces the factored form with a general exponential family:

```
μ_θ(Λ, Y) = Z_θ^{−1} · exp( θ^T h(Λ, Y) )
```

where `h` contains base factors `(Λ_i, Λ_i Y, Λ_i²)` plus user-declared dependency factors:

- `Similar(i,j)` → factor `1{Λ_i = Λ_j}`
- `Fixes(i,j)` → `λ_j` corrects `λ_i`'s errors
- `Reinforces(i,j)` → rewards agreement
- `Excludes(i,j)` → penalizes both firing

The user must **declare** the dependency structure. Same `O(1/ε²)` scaling under analogous conditions; same SGD-plus-Gibbs optimizer.

**[assessment]** In NAADAP the dependency structure is severe and mostly *unknown*. "PSC starts with R" and "issuing office is N00421" are not conditionally independent given the vehicle — the same office issues the same PSCs. So is "NAICS 541330" and "PSC R425." You would have to declare a dense dependency graph, which makes the model nearly unidentifiable (see §3.4 identifiability).

---

## 2. Snorkel (Ratner et al., VLDB 2017/2018)

### 2.1 Citation — VERIFIED

> Alexander Ratner, Stephen H. Bach, Henry Ehrenberg, Jason Fries, Sen Wu, Christopher Ré.
> **"Snorkel: Rapid Training Data Creation with Weak Supervision."**
> *Proceedings of the VLDB Endowment*, 11(3), pp. 269–282, 2017.
> arXiv:1711.10160.
> Verified at `https://arxiv.org/abs/1711.10160`; full text via ar5iv.

### 2.2 Architecture — three stages

1. **Write labeling functions.** Patterns, heuristics, external knowledge bases, third-party models. Snorkel provides LF-analysis tooling: coverage, overlap, conflict, and (given a small gold dev set) empirical accuracy per LF.
2. **Model accuracies and correlations.** Learn the generative label model over `Λ` without ground truth.
3. **Train a discriminative end model** on the resulting probabilistic labels. The end model is the deliverable; it generalizes *beyond* the LFs' coverage.

The **label-model / end-model split** is the load-bearing architectural idea. The label model's only job is to turn an `n × m` vote matrix into `n` soft labels. The end model then learns features the LFs never mentioned. WRENCH later shows the end model contributes most of the accuracy (§4.4).

### 2.3 Label-model training in Snorkel v0.7 (the VLDB paper)

Factor graph with three factor types per data point: labeling propensity, accuracy, pairwise correlation. Objective is the negative log marginal likelihood:

```
ŵ = argmin_w  − log Σ_Y  p_w(Λ, Y)
```

Optimized by "interleaving stochastic gradient descent steps with Gibbs sampling ones, similar to contrastive divergence," implemented on **Numbskull**, a NUMBA-based Gibbs sampler.

**Determinism verdict: FAIL.** Gibbs sampling is a Monte Carlo procedure. Reproducing a top-5 ranking in ≥95% of runs from a Gibbs-fitted label model would require pinning the RNG *and* the thread schedule of a parallel sampler.

### 2.4 The modeling advantage and Snorkel's own optimizer — **the most important finding for NAADAP**

Snorkel contains an explicit decision rule for *when not to bother with a label model*.

Define the **modeling advantage `A_w`**: the gain in accuracy of a weighted majority vote (using true LF accuracies) over an *unweighted* majority vote — i.e. the value added by learning accuracies at all.

- **Low-density regime (Proposition 1).** With `d̄` = mean number of non-abstaining LF votes per data point, the expected advantage scales `O(d̄²)`. When LFs are sparse, there is almost nothing to gain — most points get 0 or 1 vote and there is nothing to weight.
- **High-density regime (Theorem 1).** The expected advantage is bounded by `exp(−2 p_l (ᾱ* − ½)² d̄)` — it decays **exponentially** in label density. With many votes per point and average accuracy above ½, unweighted majority vote is already near-optimal.

So the modeling advantage is a hump: negligible at both ends, non-trivial only in a middle band.

**Snorkel's optimizer (Algorithm 1)** computes a cheap upper bound `Ã*(Λ)` on the advantage from vote-count ratios under best-case weights, and:

> if `Ã*(Λ) < γ` → **use unweighted majority vote and skip the generative model entirely.**

The paper reports this yields up to **1.8× pipeline speedup**, and names the Chem task (density 1.2) as one where no generative modeling was warranted. It also honestly reports that density alone is not a sufficient statistic: Chem and EHR both had density 1.2 but advantages of 0.1% vs 2.8%.

**[assessment] This matters enormously.** NAADAP's SME heuristics are narrow and high-precision by construction ("if PSC starts with R *and* office is N00421..."). Each covers a small slice. Expected density per document is low — probably 1–3 firing LFs. That is squarely in the `O(d̄²)`-is-tiny regime. **Snorkel's own published decision rule would tell NAADAP to use majority vote.**

### 2.5 Structure learning for LF dependencies

Pseudolikelihood estimation with a sparsity threshold `ε` selecting which pairwise correlations to model. Snorkel sweeps `ε` and picks the "elbow" where the number of selected correlations sharply increases. Reported to recover **60–70% of the benefit of correlation learning while saving up to 61% of training time.**

### 2.6 The documented failure mode (Example 3.1 in the paper)

This is the cleanest statement of how weak supervision breaks:

> Take 5 *correlated* LFs each 50% accurate (i.e. useless), plus 5 *independent* LFs each 99% accurate. If the correlation among the first five is not modeled, the independent generative model **inverts** the estimated accuracies — it concludes `α̂ = 100%` for the correlated junk (because they always agree with each other, which the model reads as agreement with truth) and `α̂ = 50%` for the genuinely accurate ones.

A block of mutually-agreeing bad LFs looks exactly like truth to a conditional-independence model. **[assessment] NAADAP is a textbook setup for this:** several SME heuristics keyed on overlapping administrative codes (PSC, NAICS, office DoDAAC, funding appropriation) will agree with each other constantly because they are all proxies for "which program office wrote this," not because they are right about the vehicle.

### 2.7 Reported gains — VERIFIED numbers

| Comparison | Result |
| --- | --- |
| vs. prior heuristic / distant-supervision baselines | **+132%** average F1 |
| vs. large hand-curated training sets | within **3.60%** average |
| SME user study: development speed | **2.8×** faster than 7 hours hand-labeling |
| SME user study: predictive performance | **+45.5%** average |
| Generative model lift over unweighted labels | **+5.81%** |
| Optimizer speedup | up to **1.8×** |

Note the fifth row: **the generative model itself is worth +5.81%.** The other 132% comes from the LF paradigm and the end model. **[assessment] The expensive, non-deterministic component buys ~6%; the cheap, deterministic component buys the rest.**

### 2.8 Industrial deployment evidence — Snorkel DryBell

> Stephen H. Bach, Daniel Rodriguez, Yintao Liu, Chong Luo, Haidong Shao, Cassandra Xia, Souvik Sen, Alex Ratner, Braden Hancock, Houman Alborzi, Rahul Kuchhal, Chris Ré, Rob Malkin.
> **"Snorkel DryBell: A Case Study in Deploying Weak Supervision at Industrial Scale."**
> *SIGMOD '19*, Amsterdam. DOI: `10.1145/3299869.3314036`. arXiv:1812.00417.
> Verified via search result metadata and the ACM DOI listing. **UNVERIFIED:** I did not fetch the full text, so the mechanism details below are from the abstract only.

Three classification tasks at Google. Reported: classifiers of comparable quality to ones trained on *tens of thousands* of hand-labeled examples; conversion of non-servable organizational resources to servable models for an average **52%** performance improvement; execution over millions of data points in tens of minutes. Notably it introduced **"scalable, sampling-free execution"** — i.e. Google's production deployment specifically removed the Gibbs sampler. **[assessment] That is a strong independent signal that the sampling-based label model is an operational liability, which is exactly NAADAP's determinism concern.**

---

## 3. Later refinements — and the closed-form question

This is the section that matters most for NAADAP's determinism constraint, so I dug hardest here.

### 3.1 Snorkel MeTaL / multi-task weak supervision

> Alexander Ratner, Braden Hancock, Jared Dunnmon, Frederic Sala, Shreyash Pandey, Christopher Ré.
> **"Training Complex Models with Multi-Task Weak Supervision."**
> arXiv:1810.02840 (dated 10 Dec 2018 on the PDF cover). Published at **AAAI 2019**.
> **VERIFIED** — PDF downloaded and text extracted locally; all equations below are transcribed from it.

**Setting.** `t` related sub-tasks `Y = [Y_1..Y_t]` with a task graph `G_task` and a feasible set `𝒴` of logically-consistent label vectors; `m` sources, each with a coverage set `τ_i ⊆ {1..t}`; a source dependency graph `G_source`. Handles **multi-class natively** via a *class-conditional* model: "we learn one accuracy parameter for each label value `λ_i` that each source `s_i` emits" — i.e. a source may have a different accuracy per class, but when wrong it errs uniformly. The paper notes this "in particular allows us to capture the **unipolar** setting" — LFs that only ever vote for one class. **[assessment] NAADAP's heuristics are exactly unipolar** ("...probably goes on a NAWCAD MAC" — it never votes *against* anything), so this expressiveness is a real advantage of MeTaL over binary data programming.

**The mechanism — inverse generalized covariance / matrix completion.** Transcribed from the PDF:

Let `O ⊆ C` be the *observable* cliques (those not containing `Y`) and `S` the junction-tree separator set (here `S = {Y}`). Write the covariance of the indicator variables `ψ(O ∪ S)` in block form:

```
Cov[ψ(O ∪ S)] ≡ Σ = [ Σ_O    Σ_OS ]        K = Σ^{-1} = [ K_O    K_OS ]
                     [ Σ_OS^T Σ_S  ]                      [ K_OS^T K_S  ]
```

- `Σ_O = Cov[ψ(O)] ∈ R^{d_O × d_O}` is **observable**, with `d_O = Σ_{C∈O} ∏_{i∈C} (|Y_{τ_i}| − 1)`.
- `Σ_OS = Cov[ψ(O), ψ(S)]` is **unobserved** and is a function of `μ`, the parameters we want.
- `Σ_S = Cov[ψ(Y)]` is a **scalar** under the simplified class-conditional model, and is a function of the **class balance `P(Y)`**, which "we assume is either known, or has been estimated according to the unsupervised approach we detail in Appendix A.3.5."

Apply the block matrix inversion lemma:

```
K_O = Σ_O^{-1} + c · Σ_O^{-1} Σ_OS Σ_OS^T Σ_O^{-1} ,   c = (Σ_S − Σ_OS^T Σ_O^{-1} Σ_OS)^{-1} ∈ R+     (3)
```

Set `z = √c · Σ_O^{-1} Σ_OS`. Then:

```
K_O = Σ_O^{-1} + z z^T                                                                             (4)
```

The right-hand side is an **observable term plus a rank-one term**. The left-hand side, by a result of Loh & Wainwright (their ref [22]) extended in their Appendix A.3.2, has **graph-structured sparsity**: `(K_O)_{i,j} = 0` whenever there is no edge between `λ_i` and `λ_j` in `G_source`. Let `Ω` be the index set of those known zeros. Then:

```
0 = (Σ_O^{-1})_{i,j} + (z z^T)_{i,j}   for (i,j) ∈ Ω                                                (5)
```

which, writing `‖A‖_Ω` for the Frobenius norm restricted to `Ω`, is the matrix-completion problem

```
ẑ = argmin_z  ‖ Σ̂_O^{-1} + z z^T ‖_Ω
```

**Algorithm 1 (verbatim from the PDF):**

```
Input: observed labeling rates Ê[ψ(O)] and covariance Σ̂_O;
       class balance Ê[ψ(Y)] and variance Σ_S;
       correlation sparsity structure Ω
  ẑ   ← argmin_z  ‖ Σ̂_O^{-1} + z z^T ‖_Ω
  ĉ   ← Σ_S^{-1} (1 + ẑ^T Σ̂_O ẑ)
  Σ̂_OS ← Σ̂_O ẑ / √ĉ
  μ̂′  ← Σ̂_OS + Ê[ψ(Y)] Ê[ψ(O)]
return ExpandTied(μ̂′)
```

**Is there a closed form?** *Yes, and the paper states it — it is buried in the identifiability discussion, not in the algorithm box.* Transcribed:

> "Taking the log of the squared entries of (5), we get a system of **linear equations `M_Ω l = q_Ω`**, where `l_i = log(z_i²)` and `q_{(i,j)} = log( ((Σ_O^{-1})_{i,j})² )`. Assuming we can solve this system (which we can always ensure by adding sources; see Appendix), we can uniquely recover the `z_i²`, meaning our model is **identifiable up to sign**."

So: take logs of squares, and the rank-one matrix-completion problem becomes an **ordinary linear least-squares problem** — solvable in closed form by pseudoinverse. No SGD required. `M_Ω` is `|Ω| × d_O` with two 1's per row.

**Sign resolution.** "the sign of a single `z_i` determines the sign of all other `z_j` reachable from `z_i` in `G_inv`", where `G_inv` is the inverse graph of `G_source`. So you pick one sign per connected component of `G_inv`. For conditionally independent sources, the paper prescribes the standard symmetry-breaking assumption: sources are **on average non-adversarial** — pick the sign that yields higher average accuracies. It notes that "even a single source that is conditionally independent from all the other sources will cause `G_inv` to be fully connected," so in most practical cases there is one component and one sign choice.

**What the released implementation actually does.** SGD. The paper says it solves "the proposed problem directly with SGD, leading to over 100× faster runtimes compared to prior Gibbs-sampling based approaches," and Appendix hyperparameters specify "stochastic gradient descent with a step size, step number, and `ℓ2` penalty."

**Theorems (transcribed).**

> **Theorem 1.** `E[ l(ŵ, X, Y) − l(w*, X, Y) ] ≤ γ + 4|Y| · ‖μ̂ − μ*‖`

> **Theorem 2.** With `a := ( d_O/Σ_S + (d_O/Σ_S)² λ_max(K_O) )^{1/2}` and `b := ‖Σ_O^{-1}‖² / (Σ_O^{-1})_min`,
> `E[‖μ̂ − μ*‖] ≤ 16(r−1) d_O² √(32π/n) · a · b · σ_max(M_Ω⁺) · ( 3√(d_O a λ_min^{-1}(Σ_O)) + 1 ) · ( κ(Σ_O) + λ_min^{-1}(Σ_O) )`

The paper's own interpretation: "our primary result is that the estimation error scales as `n^{-1/2}`." `σ_max(M_Ω⁺)`, the largest singular value of the pseudoinverse, "has a deep connection to the density of the graph `G_inv`" — **the denser `G_source` (the more LF correlations you must model), the worse the bound.**

**Class balance estimation without labels (Appendix A.3.5).** Take a conditionally independent subset `s_1..s_k`. Let `A_{i,j} = E[φ_i φ_j^T]` be the observed overlaps. Because of conditional independence, `A_{i,j} = B_i P B_j^T` where `(B_i)_{j,k} = P(λ_i = y_j | Y = y_k)` and `P = diag(P(Y = y_i))`. Setting `B̃_i = B_i √P` gives `A_{i,j} = B̃_i B̃_j^T` (eq. 16), and `P = diag( (B̃_i^T 1)² )` (eq. 17). Notably the paper warns that **"simply taking the majority vote of these sources is a biased estimator"** of the class balance.

**Reported results.** "Average gains of 20.2 points in accuracy over a traditional supervised approach, 6.8 points over a majority vote baseline, and 4.1 points over a previously proposed weak supervision method" on three fine-grained classification tasks (NER, relation extraction, medical document classification). Over 100× faster than Gibbs-based Snorkel.

### 3.2 FlyingSquid — the triplet method (the closed-form answer)

> Daniel Y. Fu, Mayee F. Chen, Frederic Sala, Sarah M. Hooper, Kayvon Fatahalian, Christopher Ré.
> **"Fast and Three-rious: Speeding Up Weak Supervision with Triplet Methods."**
> *ICML 2020* (PMLR v119). arXiv:2002.11955.
> **VERIFIED** — PDF downloaded and text extracted locally; also `https://proceedings.mlr.press/v119/fu20a.html`.

**This is the method NAADAP should care about.** It is the only label model in the mainstream literature whose parameter estimation is genuinely closed-form.

**Model.** A binary Ising model over `G = (V, E)`, `V = {Y, v}`. Each source `λ_i ∈ {−1, 0, +1}` is *augmented* into two binary observed variables:

```
λ_i = +1   →  (v_{2i−1}, v_{2i}) = (+1, −1)
λ_i = −1   →  (v_{2i−1}, v_{2i}) = (−1, +1)
λ_i =  0   →  (v_{2i−1}, v_{2i}) = (+1,+1) or (−1,−1) with equal probability   [abstain]
```

Joint: `f_G(Y, v) = Z^{-1} exp( Σ θ_{Y_i} Y_i + Σ θ_{Y_k Y_l} Y_k Y_l + Σ θ_i v_i Y(i) + Σ θ_{k,l} v_k v_l )`.

Inference uses the **junction tree formula** (eq. 1 in the paper):

```
P(Y, λ) = ∏_{C ∈ C̃_dep} μ_C  /  ∏_{S ∈ S_dep} μ_S^{d(S)−1}
```

where `μ_C` is the marginal of clique `C`, `μ_S` of separator `S`, `d(S)` the number of maximal cliques adjacent to `S`. These marginals `μ` **are** the label model parameters.

**Proposition 1 (the key identity).** If `v_i ⊥ v_j | Y(i)`, then `v_i Y(i) ⊥ v_j Y(i)`. This follows because `Y(i)² = 1` for binary variables.

**The three-equation system.** Define the *unobservable* accuracies `a_i := E[v_i Y(i)]`. Then:

```
a_i · a_j = E[v_i Y(i)] E[v_j Y(i)] = E[v_i v_j Y(i)²] = E[v_i v_j]     ← OBSERVABLE
a_i · a_k = E[v_i v_k]                                                  ← OBSERVABLE
a_j · a_k = E[v_j v_k]                                                  ← OBSERVABLE
```

Three equations, three unknowns, all products. Solve:

```
|a_i| = √| E[v_i v_j] · E[v_i v_k] / E[v_j v_k] |
|a_j| = √| E[v_i v_j] · E[v_j v_k] / E[v_i v_k] |
|a_k| = √| E[v_i v_k] · E[v_j v_k] / E[v_i v_j] |
```

**Algorithm 1** (transcribed): while there is an unassigned `v_i`, choose `v_j, v_k` pairwise conditionally independent given `Y(i)`; estimate the three pairwise agreement rates as `Ê[v_i v_j] = (1/n) Σ_t L̃_{it} L̃_{jt}`; apply the three formulas; mark the triplet assigned. Finally call `ResolveSigns`.

Crucially, the paper notes: **"variables can appear in multiple triplets... Different triplets give different accuracy values, so we compute accuracy values from all possible triplets and use the mean or median over all triplets."**

**Complexity.** `O(mn)` per statistic, `O(m²)` space for pairwise statistics; one pass over the data to form all pairwise products. No iteration, no learning rate, no epochs.

**Degenerate cases (Appendix C.2), transcribed.** If there are fewer than 3 conditionally independent subgraphs, fall back on `v_i Y(i) ⊥ Y(i)`, giving `a_i = E[v_i] / E[Y(i)]` — but "this approach fails in the presence of singleton potentials and can be very inaccurate when `E[Y(i)]` is close to 0." **[assessment] `E[Y(i)]` close to 0 is precisely the rare-vehicle case in NAADAP.**

**Multi-class (Appendix C.2), transcribed:** "We have given an algorithm for binary classes for `Y`... To extend this to higher-class cases, we can apply a **one-versus-all reduction repeatedly** to apply our core algorithm." So multi-class is a wrapper, not native — unlike MeTaL.

**Singleton potentials / alternative parametrization.** For the case where sources have singleton potentials, they give a quadratic version: with `μ_i` the 2×2 matrix of class-conditional probabilities `P(v_i = ±1 | Y(i) = ±1)`, `O_{ij}` the observed 2×2 matrix of `P(λ_i = ±1 | λ_j = ±1)`, and `P = diag(P(Y=1), P(Y=−1))`, conditional independence gives `μ_i P μ_j^T = O_{ij}` (eq. 12). They then state: **"there is a closed form solution to the resulting system of non-linear equations"** and show it by reducing the top row of `μ_i` to `[α, c_i − d_i α]` with `c_i = P(v_i=1)/P(Y(i)=−1)` and `d_i = P(Y(i)=1)/P(Y(i)=−1)` both known.

**Theory.**
- **Theorem 1 (sampling error).** `E[‖μ̂ − μ‖₂] ≤ (1/a_min⁵) · ( 3.19 C₁ √(m/n) + 6.35 C₂ (1/√r) (m/√n) )`, where `a_min` is the minimum absolute source accuracy and `r` the minimum abstain frequency. The second term vanishes with no abstentions. **[assessment] Note the `1/a_min⁵` prefactor — with a marginal LF (`a_min` small), the bound explodes.**
- **Theorem 2 (lower bound).** `inf_μ̂ sup_P E‖μ̂ − μ‖₂ ≥ (e_min/8)√(m/n)` — the triplet estimator is **minimax optimal up to constants.**
- **Theorem 3 (generalization under misspecification).** `E[l(ŵ) − l(w*)] ≤ γ(n) + (8|Y|/e_min)‖μ̂ − μ‖₂ + δ(D, P_μ)` with `δ = 2√(2 KL(D(Y|X) ‖ P_μ(Y|X)))`. **This is the first bound in the line of work that does *not* assume the graphical model exactly parameterizes the data distribution** — prior analyses implicitly set `δ = 0`. Real advance.

**Reported speedups (transcribed from the results tables).** Spouse 440×, Spam 54×, Weather 5.2×; video tasks 74.5×–350× vs. data programming and 607×–4,000× vs. sequential DP. Accuracy: Spouse 49.6 vs DP 44.7 vs MV 19.3; Spam 92.3 vs DP 91.8 vs MV 88.3; Interview 91.9 vs DP 8.7 vs MV 58.0; Commercial 92.3 vs DP 90.5 vs MV 91.8.

**The buried supervision requirement — transcribed verbatim from Appendix E.2:**

> "For our label model, we use **class balance from the dev set**, or **tune the class balance ourselves with a grid search**. We also **tune which triplets we use for parameter recovery on the dev set.**"

**[assessment] This is a significant honesty problem with the reported FlyingSquid numbers, and it matters directly for NAADAP.** A method advertised as needing no labels is, in the reported experiments, tuned twice against labeled dev data: once for the class prior and once for triplet selection. If NAADAP adopts FlyingSquid it must either (a) fix the class prior from an external, citable source and (b) use *all* triplets with a median rather than tuning the selection — which is both more honest and more deterministic, at some cost in the reported accuracy.

### 3.3 Other closed-form / non-SGD label models

- **Dawid–Skene (1979)** — the ancestor. **VERIFIED:** A. P. Dawid & A. M. Skene, "Maximum Likelihood Estimation of Observer Error-Rates Using the EM Algorithm," *Journal of the Royal Statistical Society, Series C (Applied Statistics)*, 28(1), pp. 20–28, 1979. DOI `10.2307/2346806`; JSTOR `2346806`. Full per-annotator `K × K` confusion matrices, estimated by EM. **Native multi-class.** Not closed-form, but EM is *deterministic given a deterministic initialization* — and the standard initialization is majority vote, which is itself deterministic. With a fixed max-iteration count and a fixed convergence tolerance, Dawid–Skene is bit-reproducible. The paper's own caveat — "the EM algorithm is shown to provide a **slow but sure** way of obtaining maximum likelihood estimates" — is fine on NAADAP's compute budget.

- **Learning Hyper Label Model** — **VERIFIED:** Renzhi Wu, Shen-En Chen, Jieyu Zhang, Xu Chu, "Learning Hyper Label Model for Programmatic Weak Supervision," *ICLR 2023*; arXiv:2207.13545; dblp `conf/iclr/WuCZC23`; repo `wurenzhi/hyper_label_model`. Amortizes label-model inference into a single forward pass of a permutation-equivariant **GNN**, pre-trained across datasets; reported +1.4 points average accuracy over existing methods and 6× faster on 14 datasets. **Deterministic at inference** (it is a fixed forward pass), but it requires shipping a pre-trained graph neural network and a deep-learning runtime. **[assessment] Reject for NAADAP** — violates "minimal third-party dependencies" and would need ONNX Runtime or TorchSharp in the container.

- **Reliable PWS with confidence intervals** — **VERIFIED:** Verónica Álvarez, Santiago Mazuelas, Steven An, Sanjoy Dasgupta, "Reliable Programmatic Weak Supervision with Confidence Intervals for Label Probabilities," arXiv:2508.03896 (2025), stat.ML. Builds **uncertainty sets of distributions** encapsulating LF information with unrestricted LF behavior and typology, yielding confidence intervals on label probabilities rather than point estimates; explicitly does *not* assume conditional independence. **[assessment] Interesting for NAADAP's "defensible in a legal determination" requirement** — an interval is a more honest artifact than a point probability. But it is a 2025 preprint with no established implementation and the method is a distributional-uncertainty optimization, not closed form. Track, do not build.

### 3.4 Identifiability — the constraint nobody mentions

Both MeTaL and FlyingSquid require that the dependency structure admit a solution:

- **MeTaL:** the linear system `M_Ω l = q_Ω` must be solvable. `Ω` is the edge set of `G_inv` (the *inverse* of `G_source`) expanded over indicator variables. **The denser `G_source` is — i.e. the more LF correlations you declare — the sparser `G_inv` is, the fewer equations you have, and the sooner the system becomes unsolvable.** The paper's remedy is "adding sources."
- **FlyingSquid:** needs at least 3 conditionally independent subgraphs to run Algorithm 1 at all, and at least 2 with no singleton potentials for the fallback.

**[assessment] This is a hard blocker for NAADAP's most natural LF design.** If an SME gives you 30 heuristics and 25 of them are functions of the same three administrative fields (PSC, NAICS, office DoDAAC), an honest `G_source` is nearly complete, `G_inv` is nearly empty, and neither label model is identifiable. The only fix is to deliberately engineer LFs that read genuinely different evidence — e.g. one family from administrative metadata, one family from PWS body text (task verbs, deliverable language), one family from CDRL/DD-1423 structure. That is a *design constraint on the SME elicitation process*, and it is the single most actionable thing this literature says to NAADAP.

---

## 4. WRENCH — what actually wins

### 4.1 Citation — VERIFIED

> Jieyu Zhang, Yue Yu, Yinghao Li, Yujing Wang, Yaming Yang, Mao Yang, Alexander Ratner.
> **"WRENCH: A Comprehensive Benchmark for Weak Supervision."**
> *NeurIPS 2021 Datasets and Benchmarks Track* (Oral). arXiv:2109.11377.
> Official PDF: `https://datasets-benchmarks-proceedings.neurips.cc/paper_files/paper/2021/file/1c9ac0159c94d8d0cbedc973445af2da-Paper-round2.pdf`
> **VERIFIED** — PDF downloaded and text extracted locally; all tables below transcribed from it.

**22 datasets**: 13 classification (Census, IMDb, Yelp, YouTube, SMS, AGNews, TREC, Spouse, CDR, SemEval, ChemProt, Commercial, Tennis Rally, Basketball) and 9 sequence-tagging (CoNLL-03, WikiGold, OntoNotes 5.0, BC5CDR, NCBI-Disease, Laptop-Review, MIT-Restaurant, MIT-Movies). Modalities: tabular, text, biomedical, video.

**Label models compared:** MV, WMV, Dawid–Skene (DS), Data Programming (DP), MeTaL, FlyingSquid (FS); plus HMM and CHMM for sequence tagging. Later additions in the repo: EBCC, IBCC, FABLE, Hyper Label Model.

### 4.2 Headline finding — no universal winner

> "For classification tasks, **MeTaL and MV are the most worth-a-try label models** and for end model, deeper is better. According to the model performance averaged over datasets, we find MeTaL and MV are the best label models when using different end models or directly applying label models on test set."

Per-dataset winners are genuinely scattered. Transcribed from the classification table (best/2nd-best label model per dataset): Yelp→FS, FS, DS; YouTube→MV; SMS→WMV, MeTaL; AGNews→DS, MV, WMV; TREC→DP, MeTaL; Spouse→FS, MeTaL, MV; CDR→MeTaL, DP; SemEval→DP, MV; ChemProt→DP, MV; Commercial→MV; Tennis Rally→FS, MeTaL; Basketball→FS, WMV, DP; Census→MeTaL.

**[assessment] Majority vote appears as a best-or-runner-up label model on roughly half the classification datasets.** That is the empirical form of Snorkel's own modeling-advantage theory.

### 4.3 The sequence-tagging table — the sharpest warning

Transcribed label-model-only F1 (no end model), entity-level:

| Label model | CoNLL-03 | WikiGold | BC5CDR | NCBI-Dis | Laptop-Rev | MIT-Rest | MIT-Movies | OntoNotes | **Avg** |
|---|---|---|---|---|---|---|---|---|---|
| MV | 60.36 | 52.24 | 83.49 | 78.44 | 73.27 | 48.71 | 59.68 | 58.85 | **64.38** |
| WMV | 60.26 | 52.87 | 83.49 | 78.44 | 73.27 | 48.19 | 60.37 | 57.58 | **64.31** |
| DS | 46.76 | 42.17 | 83.49 | 78.44 | 73.27 | 46.81 | 54.06 | 37.70 | **57.84** |
| DP | 62.43 | 54.81 | 83.50 | 78.44 | 73.27 | 47.92 | 59.92 | 61.85 | **65.27** |
| MeTaL | 60.32 | 52.09 | 83.50 | 78.44 | 64.36 | 47.66 | 56.60 | 58.27 | **62.66** |
| **FS** | 62.49 | 58.29 | **56.71** | **40.67** | **28.74** | **13.86** | 43.04 | **5.31** | **38.64** |
| HMM | 62.18 | 56.36 | 71.57 | 66.80 | 73.63 | 42.65 | 60.56 | 55.67 | **61.88** |
| CHMM | 63.22 | 58.89 | 83.66 | 78.74 | 73.26 | 47.34 | 61.38 | 64.06 | **66.32** |

**FlyingSquid scores 5.31 F1 on OntoNotes where majority vote scores 58.85, and 38.64 average vs. MV's 64.38.** MeTaL and DS are both *below* plain majority vote on average. **[assessment] A sophisticated label model is not a safe default. On this benchmark, choosing the wrong one costs you 26 F1 points relative to doing the dumbest possible thing.** Note that the one-vs-all multi-class reduction is precisely what is being stressed here — which is directly relevant to NAADAP's multi-vehicle setting.

### 4.4 Other WRENCH findings, transcribed

- **"Strong weakly supervised models rely on high-quality supervision sources."** For datasets with very noisy LFs (Basketball) or very limited coverage (MIT-Restaurants), there remains a large gap to full supervision. "It is still necessary to check the quality of initial labeling functions before applying weak supervision models for new tasks. Otherwise, directly adopting these models may not lead to satisfactory results, and **may even hurt the performance.**"
- **End models dominate.** "fine-tuning a pre-trained language model is, not surprisingly, much better than directly applying label model on test data in most cases." **[assessment] For NAADAP, whose core path must exclude LLMs, this is a real loss — the single biggest source of gain in the WS pipeline is exactly the component NAADAP is not allowed to use.**
- **Soft labels beat hard labels**, especially for deeper end models (label-smoothing effect).
- **Uncovered data should be used** when training end models, not discarded.
- **Synthetic LF study (§5.1–5.2).** "the comparative performance of label models are largely dependent on the **variance of accuracy and propensity of LFs**." When LF accuracy variance is large or propensity (coverage) is small, label model performance **diverges** sharply. With *top-k accurate* LFs, "the label models perform similarly" — i.e. when LFs are good, nothing beyond MV matters. With *correlated* or *data-dependent* LFs, gaps open and the sophisticated models (DP, MeTaL, FS) pull ahead — but inconsistently across datasets.
- They explicitly flag the two assumptions most often violated in practice: conditional independence of LFs, and **uniform LF accuracy across the dataset** ("specific LFs are often more accurate on some subset of data than the other"). **[assessment] The latter is guaranteed in NAADAP — an SME heuristic about NAWCAD office codes is accurate on NAWCAD documents and meaningless on NAVSUP documents.**

### 4.5 A counterweight — BOXWRENCH

> Tianyi Zhang, Linrong Cai, Jeffrey Li, Nicholas Roberts, Neel Guha, Jinoh Lee, Frederic Sala.
> **"Stronger Than You Think: Benchmarking Weak Supervision on Realistic Tasks."**
> *NeurIPS 2024 Datasets and Benchmarks Track*. arXiv:2501.07727.
> **VERIFIED** at `https://arxiv.org/abs/2501.07727`.

Argues WRENCH-era conclusions understate WS because the benchmark tasks are too easy. BOXWRENCH features **higher class imbalance, genuine domain-expertise requirements, and multilingual LF reuse**. Headline: **supervised learning requires 1000+ labeled examples to match weak supervision in many settings.**

**[assessment] This is the single most favorable finding for NAADAP in the whole literature, because BOXWRENCH's stated design axes — class imbalance, domain expertise — are exactly NAADAP's profile.** Acquisition vehicle assignment is severely imbalanced (a handful of vehicles absorb most requirements) and requires expertise a crowd worker cannot supply. It directly rebuts the Zhu et al. critique in §6.1 for tasks of NAADAP's shape.

---

## 5. skweak and sequence-labeling weak supervision

### 5.1 Citation — VERIFIED

> Pierre Lison, Jeremy Barnes, Aliaksandr Hubin.
> **"skweak: Weak Supervision Made Easy for NLP."**
> *Proceedings of ACL-IJCNLP 2021: System Demonstrations*, pp. 337–346.
> ACL Anthology `2021.acl-demo.40`. DOI `10.18653/v1/2021.acl-demo.40`. arXiv:2104.09683.
> **VERIFIED** at the ACL Anthology landing page and via ar5iv.

### 5.2 Mechanism

Aggregation is a **Hidden Markov Model** over token-level latent states:

- **Emissions:** for each token `i` and LF `λ_j`, `p(λ_j^{(i)} = Y_{ij} | P_j^{s_i}) = Multinomial(P_j^{s_i})` — i.e. each LF gets a full **confusion matrix conditioned on the latent state**. This is Dawid–Skene emissions inside an HMM.
- **Transitions:** `p(s_i = k | s_{i−1} = l) = τ_{lk}` — standard HMM transition matrix. This is what MV and the classification label models cannot do: enforce label-sequence coherence (no `I-ORG` after `B-PER`).
- **Abstention** handled via a special "void" label.
- **Underspecified labels** supported — an LF may emit a coarse type (`ENT`) that subsumes finer types (`COMPANY`). **[assessment] This is directly useful for NAADAP: an SME heuristic often says "this is a services requirement" without saying which services vehicle — a hierarchical/underspecified vote.**

- **Estimation:** **Baum–Welch** (EM with forward–backward), **initialized from a majority voter** which supplies initial transition and emission probabilities, then "refined through several EM passes." Implemented with `hmmlearn`'s C routines.

**Results (MUC-6, 52 labeling functions):** HMM 0.83 token-F1 / 0.75 entity-F1; majority vote 0.64 / 0.62; a neural model trained on the HMM labels 0.83 / 0.76. So the HMM aggregator beats MV by ~19 token-F1 points — a much bigger margin than any classification label model achieves over MV.

**Determinism:** the paper makes no reproducibility claim. **[assessment] But Baum–Welch initialized from majority vote *is* deterministic**, provided you fix the iteration count / tolerance and the MV tie-break. There is no random initialization in the described procedure. This is a favorable determinism profile.

### 5.3 Repository

`NorskRegnesentral/skweak` — Python, **MIT** license, ~926 stars. Dependencies: spacy ≥3.0, hmmlearn ≥0.3, pandas, numpy. **The README states the project is no longer actively maintained**, with an invitation for someone to take it over. **[assessment] Do not take a dependency on it; it is a reference implementation to read, not to use. Irrelevant anyway — it is Python.**

### 5.4 Weak supervision on legal / contractual text — **a genuine gap**

I searched specifically for programmatic weak supervision applied to legal, contractual, or government-procurement text and **did not find a peer-reviewed paper that does this**. What exists:

- **CUAD** (Hendrycks et al., arXiv:2103.06268) — the Contract Understanding Atticus Dataset: 500+ commercial contracts, 41 clause types, 13,000+ expert annotations. This is *expert manual annotation*, the opposite of weak supervision, and it is the reference point for what "SMEs actually labeling contracts" costs. **UNVERIFIED** — I saw the metadata in search results but did not fetch the paper; treat the counts as approximate.
- Adjacent applied WS work exists in clinical notes (arXiv:2206.12088), bank transactions (arXiv:2305.18430), and climate/infrastructure literature (arXiv:2302.01887). **UNVERIFIED** — surfaced in search, not fetched.

**[assessment] Conclusion: NAADAP would be doing something novel, not following a paved path.** That is a legitimate research contribution to claim in the prize submission, but it also means there is no prior art telling you what LF coverage and accuracy look like on SOW/PWS/CDRL text. Budget for that discovery.

---

## 6. Honest failure analysis

### 6.1 The strongest published critique

> Dawei Zhu, Xiaoyu Shen, Marius Mosbach, Andreas Stephan, Dietrich Klakow.
> **"Weaker Than You Think: A Critical Look at Weakly Supervised Learning."**
> *Proceedings of ACL 2023*, July 2023, pp. 14229–14253. Anthology `2023.acl-long.796`.
> DOI `10.18653/v1/2023.acl-long.796`. arXiv:2305.17442. Code: `uds-lsv/critical_wsl`.
> **ACL 2023 Theme Paper Award.** **VERIFIED** at the ACL Anthology landing page and via ar5iv.

Findings, from the paper:

1. **WSL methods secretly depend on clean validation data.** COSINE, L2R and MLC "completely fail without access to clean validation samples — performing at or below weak label baselines." When the validation set is itself weakly labeled, all WSL methods become ineffective. Roughly **30 clean samples per class** is where WSL starts working, after which gains plateau.
2. **Just training on that clean set beats the whole pipeline.** Fine-tuning directly on the validation set outperforms all the sophisticated WSL approaches "in almost all cases" across eight datasets. With **10 clean samples per class**, parameter-efficient fine-tuning (LoRA, adapters, BitFit) surpasses COSINE on 3 of 4 text classification tasks. For relation extraction, **10–20 samples per class** suffices.
3. Their own simple baseline **FTW+CFT** (train on weak labels, then continue fine-tuning on the clean set) matches or exceeds the sophisticated methods at a fraction of the cost. With just 5 clean samples per class after CFT, plain FTW is comparable to COSINE and L2R.
4. Datasets: AGNews, IMDb, Yelp, TREC, SemEval, ChemProt, CoNLL-03, OntoNotes 5.0 — all from WRENCH.
5. Recommendation: report how much clean validation data your method needs and what it does without it; always compare against training directly on those clean samples; and accept that "if thousands of weakly annotated samples are comparable to a handful of clean samples, WSL may not suit the given low-resource scenario."

**[assessment] Applied to NAADAP:** if an acquisition SME can label 20–30 documents per vehicle class — and for the top 5–10 vehicles that is maybe 200–300 documents, a week of one person's time — then this paper says direct supervised learning wins and the whole weak-supervision apparatus is overhead. That is a real and uncomfortable possibility that the team should test before building a label model. **Counter-consideration:** Zhu et al.'s end models are all fine-tuned pretrained language models, which is the regime where 10 clean examples go a very long way. NAADAP's core path forbids LLMs, so a 10-example fine-tune is not available to it; a from-scratch classifier on 200 examples over 30 imbalanced classes is a different and much weaker proposition. And §4.5 (BOXWRENCH) argues the opposite conclusion holds on imbalanced expert tasks. **The honest position is that this is an open empirical question for NAADAP specifically, and it is cheap to settle: have one SME label 200 documents and run the comparison.**

### 6.2 The assumption inventory — what breaks, and how

| Assumption | Stated where | What breaks when it fails | NAADAP risk |
|---|---|---|---|
| **Conditional independence of LFs given `Y`** | DP eq. (1); FS Prop. 1; MeTaL `G_source` | Snorkel Example 3.1: a block of mutually-agreeing bad LFs is read as truth; accuracies **invert** (`α̂ = 100%` for the junk, `50%` for the good ones) | **SEVERE.** SME heuristics all key on the same few administrative fields. |
| **LF accuracy > 50%** (non-adversarial) | DP theory assumes `α ∈ [0.8, 0.9]`; MeTaL sign-resolution uses "sources are on average non-adversarial" | Sign ambiguity resolves the wrong way → label model outputs the *inverse* of the truth, globally | **MODERATE.** Some SME rules-of-thumb are stale (vehicles expire, offices reorganize). |
| **Adequate coverage / label density `d̄`** | Snorkel Prop. 1 (advantage is `O(d̄²)`) | Modeling advantage → 0; you did all the work for nothing | **SEVERE.** Narrow high-precision heuristics ⇒ low density. |
| **Known class balance `P(Y)`** | MeTaL §4 (`Σ_S` is a function of `P(Y)`); FS uses "class balance from the dev set" | Systematic bias in all accuracies; MeTaL warns MV of sources is a **biased** estimator of `P(Y)` | **MODERATE.** Mitigable — historical award data gives a defensible empirical prior. |
| **LF accuracy uniform across the dataset** | flagged as commonly violated in WRENCH §5.2 | Label model under/over-weights LFs on subpopulations; performance diverges | **SEVERE.** Office-code heuristics are accurate only within that office's documents. |
| **Model well-specification** | DP Thm 1 condition (4) | Bounds void. FS **Theorem 3** is the only result that relaxes this, with an explicit `2√(2 KL(D(Y|X) ‖ P_μ(Y|X)))` penalty | Unknowable a priori. |
| **Identifiability of `G_source`** | MeTaL (`M_Ω l = q_Ω` solvable); FS (≥3 cond. indep. subgraphs) | No solution exists; the estimator silently returns garbage | **SEVERE** given the previous rows. |
| **Large `n` unlabeled** | DP Thm 1 (`\|S\| ≥ 356/ε² log(m/3ε)`); FS Thm 1 (`O(m/√n)`, prefactor `1/a_min⁵`); MeTaL Thm 2 (`n^{-1/2}`, prefactor `d_O²`) | Estimator variance swamps the accuracy differences being estimated | **FATAL at n=20.** Pairwise agreement rates from 20 samples have SE ≈ 0.22. |
| **`m ≥ 2000` LFs** | DP Thm 1, literally | Theorem does not apply | **Always violated** in practice, including in the original paper's own experiments. |

### 6.3 When weak supervision loses to simple rules or direct labeling

Synthesizing across the sources above:

1. **When you can get labels.** Zhu et al. 2023: 10–30 clean examples per class, plus a pretrained model to fine-tune, beats the pipeline.
2. **When LF density is very low or very high.** Snorkel's own Prop. 1 / Thm 1: the label model's advantage is `O(d̄²)` at low density and `exp(−c·d̄)` at high density. Snorkel ships an optimizer that skips the label model in these regimes.
3. **When LFs are correlated and you cannot declare the structure.** Snorkel Example 3.1; WRENCH §5.2 Fig. 4.
4. **When LFs are simply bad.** WRENCH: "may even hurt the performance." Basketball and MIT-Restaurants show large residual gaps.
5. **When the task is multi-class with a one-vs-all reduction.** WRENCH sequence-tagging table: FlyingSquid at 5.31 F1 vs MV's 58.85 on OntoNotes.
6. **When you cannot use a strong end model.** WRENCH: most of the reported gain comes from the end model, not the label model. NAADAP's no-LLM core path forfeits this.
7. **When `n` is small.** All three sample-complexity theorems scale as `n^{-1/2}`; none of them say anything useful at `n` in the tens.

### 6.4 The NAADAP-specific argument that nobody in this literature makes — **[assessment]**

**The premise that NAADAP has no labels deserves challenge.** Federal award data (FPDS-NG / SAM.gov / USAspending) records, for every award, the referenced parent IDIQ contract number. That is *literally the answer* to "which vehicle did this requirement go on," at a scale of hundreds of thousands of records, for free, and it is citable in a legal determination because it is the authoritative government record.

If that is true, NAADAP is not in a weak-supervision setting at all. It is in a **distant supervision** setting with abundant, authoritative labels, where the right move is ordinary supervised learning (or nearest-neighbour retrieval) over award history, and SME heuristics belong as **features, priors, and guardrails** — not as a substitute for labels. The SME-knowledge-capture goal is still served: the heuristics become auditable, versioned rules whose agreement with historical outcomes can be *measured* rather than assumed.

I flag this because it inverts the whole premise of this research area, and it is cheap to check before committing engineering effort.

---

## 7. Determinism assessment — method by method

NAADAP requires the same top-5 in ≥95% of runs. Verdicts:

| Method | Estimation | Random init? | Deterministic? | Verdict |
|---|---|---|---|---|
| **Majority vote** | none | no | **Yes**, modulo tie-breaking | **SAFE.** Document a total order for ties (e.g. vehicle ID lexicographic, then LF priority). |
| **Weighted MV, SME-assigned weights** | none | no | **Yes** | **SAFE.** |
| **Dawid–Skene EM** | EM | no, if MV-initialized | **Yes** with MV init + fixed max-iter + fixed tolerance | **SAFE.** Note EM has local optima — pinning init pins the optimum. |
| **skweak HMM (Baum–Welch)** | EM | no, MV-initialized | **Yes**, same conditions | **SAFE** in principle. |
| **FlyingSquid triplet** | closed-form | **no** | **Yes**, with care (below) | **SAFE — recommended.** |
| **MeTaL closed-form route** (`M_Ω l = q_Ω` least squares) | closed-form | **no** | **Yes** | **SAFE**, needs a linear-algebra kernel. |
| **MeTaL as released** (SGD on `‖Σ̂_O^{-1} + zz^T‖_Ω`) | SGD | yes | No | **REJECT.** |
| **Snorkel `LabelModel` v0.9** | SGD | **yes** | **No by default** | **REJECT.** See below. |
| **Snorkel v0.7 generative model** | SGD + Gibbs (Numbskull) | yes | No | **REJECT.** |
| **Hyper Label Model** | pre-trained GNN forward pass | no at inference | Yes at inference | **REJECT** on dependency grounds (needs a DL runtime). |

**Snorkel `LabelModel` — verified from source.** I read `snorkel/labeling/model/label_model.py` on `snorkel-team/snorkel@main`:

- `TrainConfig` defaults: `n_epochs = 100`, `lr = 0.01`, `optimizer = "sgd"`, and **`seed: int = np.random.randint(1e6)`** — the default seed is itself random.
- `_init_params()` sets `self.mu = nn.Parameter(self.mu_init.clone() * np.random.random()).float()` — **the initialization is a deterministic quantity multiplied by a uniform random scalar.**
- `fit()` does seed `random`, `np.random` and `torch.manual_seed` from `train_config.seed`.

So: **deterministic if and only if you pass an explicit seed**, and even then it is SGD on a non-convex objective — the answer depends on `lr`, `n_epochs`, and float accumulation order. Not acceptable for a defensible government determination, where "we got a different top-5 because the RNG seeded differently" is not a sentence anyone wants to write in a J&A.

**FlyingSquid determinism caveats — [assessment].** The closed form itself is exact, but four things must be pinned to guarantee bit-reproducibility:

1. **Triplet enumeration order** — enumerate all valid triplets in a fixed canonical order (sorted LF indices), never a hash-set iteration order.
2. **Median tie-breaking** — with an even number of triplets the median is an average; define it explicitly (lower median, or mean-of-two with a fixed summation order).
3. **Sign resolution** — the "on average non-adversarial" rule can tie. Define a deterministic fallback (e.g. positive sign, then the LF with the lowest index anchors the component).
4. **Floating-point accumulation order** — sum pairwise products in a fixed index order; avoid parallel reduction, or use a deterministic tree reduction. At NAADAP's scale (1 core) this is free.

The paper's own "tune which triplets we use for parameter recovery on the dev set" must **not** be reproduced; use all triplets with a fixed median.

**The architectural move that dissolves the problem — [assessment].** Fit the label model **offline, at container-build time**, on the historical corpus, and ship `μ̂` as a versioned, checksummed data artifact inside the image. Run-time inference then consists of: apply LFs → look up `μ̂` → evaluate the junction-tree formula → rank. That is pure arithmetic with no estimation at all, and it is deterministic *regardless of which estimator produced `μ̂`*. It also makes `μ̂` an auditable artifact with a provenance record ("fitted from FPDS-NG extract dated 2026-03-01, SHA-256 …"), which is exactly what a legal determination wants. **This should be the design regardless of which label model is chosen**, and it is the single highest-leverage decision in this area.

---

## 8. C# reimplementability assessment

The question is whether the *method* ports, not whether a library exists. None of these have a .NET implementation; all would be written from scratch.

| Method | What it needs | Est. C# LOC | External deps | Verdict |
|---|---|---|---|---|
| **Majority / weighted MV** | integer counting, sort | ~100 | none | Trivial. |
| **LF coverage/overlap/conflict analysis** | counting over the label matrix | ~200 | none | Trivial and high-value (see §9). |
| **Dawid–Skene EM** | `m` confusion matrices `K×K`, forward E-step, closed-form M-step, log-sum-exp | **~200–300** | none | **Easy.** Pure arrays. Native multi-class. |
| **FlyingSquid triplet (independent case)** | pairwise product matrix `O(m²n)`, `sqrt`, median, sign propagation over `G_inv`, junction-tree formula for inference | **~400–600** | **none** | **Easy–medium. No matrix inversion needed in the conditionally independent case.** This is the sweet spot. |
| **FlyingSquid with dependencies** | junction tree construction, triangulation, clique/separator marginals | +400–600 | none | Medium–hard. Graph algorithms, but standard. |
| **MeTaL closed-form** | `Σ̂_O^{-1}` (`d_O × d_O` inverse), then least-squares solve of `M_Ω l = q_Ω` via QR/SVD pseudoinverse, then sign resolution | **~600–900** plus a linear-algebra kernel | MathNet.Numerics (MIT, pure managed) or ~400 LOC hand-rolled QR | Medium. Native multi-class is the payoff. |
| **skweak-style HMM aggregation** | forward–backward, Baum–Welch, log-space scaling | **~400–500** | none | Medium. Only if NAADAP does span extraction. |
| **DP / Snorkel generative model** | SGD over a factor graph + Gibbs sampler | ~800–1200 | none, but nondeterministic | **Reject** on determinism. |
| **Hyper Label Model** | pre-trained GNN inference | — | ONNX Runtime / TorchSharp (tens of MB) | **Reject** on dependencies. |

**Notes on the linear algebra.** MeTaL's `Σ_O` is `d_O × d_O` where `d_O = Σ_{C∈O} ∏_{i∈C}(|Y_{τ_i}| − 1)`. For `m = 50` unipolar LFs over 30 vehicle classes this gets large fast and the inversion becomes the dominant cost *and* the dominant conditioning risk (Theorem 2's bound contains `λ_min^{-1}(Σ_O)` and `κ(Σ_O)`). FlyingSquid's triplet route needs no inverse at all. **[assessment] That is a decisive practical advantage for a from-scratch C# implementation: no matrix inversion means no conditioning failures, no pivoting strategy to get right, and no third-party numerics dependency.**

**Compute budget.** Fitting a triplet label model over `m = 50` LFs and `n = 50,000` historical records costs `O(m²n) ≈ 1.25 × 10⁸` multiply-accumulates — a few seconds single-threaded in C#, and it happens at build time anyway. Run-time inference over 20 documents is microseconds. **The 1-core / 2 GB / 30-minute budget is not remotely a constraint for this family of methods.** The constraint is entirely statistical, not computational.

---

## 9. Recommendation for NAADAP

### 9.1 Adopt now — the labeling-function abstraction (not the label model)

Model every SME heuristic as a first-class object:

```csharp
public interface ILabelingFunction {
    string   Id            { get; }   // stable, citable: "psc-r-nawcad-office-v2"
    string   SmeStatement  { get; }   // the SME's own words, verbatim
    string   Provenance    { get; }   // who, when, which interview/document
    int      Version       { get; }
    LfVote   Apply(ProcurementDocument doc);   // { VehicleId | ABSTAIN, Evidence[] }
}
```

`Evidence[]` must carry the *span-level* justification — field name, value, and document location — so that every vote is independently checkable. This gives NAADAP what it actually needs: a durable, versioned, testable encoding of expert knowledge that survives the expert's retirement, with every output traceable to a named rule and a named document field. **This is the part of the literature with real value here, and it requires no statistical machinery whatsoever.**

Ship LF analysis alongside it — coverage, overlap, conflict, and (on whatever gold set exists) empirical accuracy per LF. Snorkel's most durable contribution is arguably this tooling, not the generative model: it is the feedback loop that lets an SME see that their heuristic fires on 3% of documents and conflicts with another heuristic 40% of the time.

### 9.2 Aggregate with a deterministic weighted vote — v1

Given low expected label density, Snorkel's own modeling-advantage analysis (§2.4) says the label model buys almost nothing. Ship weighted majority vote with SME-assigned or coverage-derived weights and a documented total order for tie-breaking. Then **measure `Ã*`** — Snorkel's own advantage upper bound — on real data. If it is below threshold, the question is settled empirically and you can say so in the submission with a citation.

### 9.3 Hold the label model behind a flag — v2, gated on data

**Precondition:** ≥ ~5,000 historical records with LF votes applied. Below that, all three sample-complexity theorems are vacuous and you would be fitting noise.

**If the precondition is met, implement the FlyingSquid triplet estimator**, because:
- It is the only genuinely **closed-form** estimator in the literature — no SGD, no random initialization, no learning rate, no epoch count. This directly satisfies the determinism requirement rather than working around it.
- It needs **no matrix inversion** in the conditionally independent case — ~500 lines of C# with zero third-party dependencies.
- It is **minimax optimal** (Theorem 2) and has the only generalization bound that does not assume model well-specification (Theorem 3).
- `O(mn)` time, `O(m²)` space.

**With these deviations from the paper, all for determinism and honesty:**
- Use **all** valid triplets with a fixed-order median. Do **not** tune triplet selection on a dev set.
- Fix the class prior `P(Y)` from historical award distributions — a defensible, citable external source — rather than grid-searching it.
- Pin the four determinism hazards in §7.
- Fit **offline at build time**; ship `μ̂` as a checksummed artifact.

**Consider Dawid–Skene as the multi-class alternative.** It handles many classes natively with full confusion matrices (no one-vs-all reduction — and §4.3 shows one-vs-all is where FlyingSquid catastrophically fails), it is ~250 lines, and MV-initialized EM with a fixed iteration budget is fully deterministic. It is slower to converge and has no closed form, but "slow but sure" is free on this compute budget. **[assessment] Given that NAADAP's task is multi-class over many vehicles, I would prototype both and select on held-out data — and I would not be surprised if Dawid–Skene wins.**

**Reject outright:** Snorkel's `LabelModel` (non-deterministic by default, verified in source), the v0.7 Gibbs generative model, MeTaL-as-released (SGD), and the Hyper Label Model (needs a DL runtime).

### 9.4 The validation NAADAP must do before building any of this

1. **Check whether labels already exist.** Pull FPDS-NG/USAspending parent-IDIQ references. If (requirement text → awarded vehicle) pairs are available at scale, this is a supervised problem and the label model is unnecessary. **[assessment] Highest-priority check; cheapest; most likely to change the plan.**
2. **Run the Zhu et al. comparison.** Have one SME label ~200 documents. Compare direct supervised learning on those against the full WS pipeline. ACL 2023's Theme Paper Award says direct labeling wins; BOXWRENCH says it loses on imbalanced expert tasks. Settle it with NAADAP's own data rather than with a citation.
3. **Measure LF conditional dependence.** Compute conditional mutual information between LF pairs (WRENCH's own diagnostic). If the heuristics are as correlated as I expect, neither MeTaL nor FlyingSquid is identifiable (§3.4) and MV is the only defensible aggregator.
4. **Deliberately diversify LF evidence sources during SME elicitation.** Ask for heuristics that read *different* things — administrative metadata, PWS body language, CDRL structure, dollar thresholds. This is a requirement imposed by the identifiability condition, and it is the most actionable thing this literature says to the elicitation process.

### 9.5 The honest case against

State it plainly in the submission:

- Every label-model guarantee is asymptotic in unlabeled data (`n^{-1/2}`); NAADAP's stated working set is 20 documents, where these estimators are pure noise.
- Snorkel's own published decision rule (`Ã* < γ` → use majority vote) probably fires for NAADAP's low-density, high-precision heuristics.
- The generative model was worth **+5.81%** in Snorkel's own headline evaluation; the LF paradigm and the end model account for the rest.
- WRENCH shows the wrong label model loses to majority vote by up to **26 F1 points** on average, and FlyingSquid's one-vs-all multi-class reduction is exactly where it collapses (5.31 vs 58.85 on OntoNotes).
- Most of the WS pipeline's gain comes from a strong pretrained end model, which NAADAP's no-LLM core path forbids.
- SME heuristics keyed on overlapping administrative codes violate conditional independence in the precise way Snorkel's Example 3.1 shows inverts the estimated accuracies.
- ACL 2023's Theme Paper Award finds WSL gains are "significantly overestimated" and that 10–30 clean labels per class, used directly, beat the pipeline.
- A probabilistic label from a latent-variable model is not citable evidence. The LFs are. Keep the LFs in the evidence path no matter what aggregator sits on top.

---

## 10. Repository inventory

| Repo | Language | License | Stars | Status |
|---|---|---|---|---|
| `snorkel-team/snorkel` | Python | Apache-2.0 | ~6,000 | Not archived, but README states the team "is now focusing their efforts on **Snorkel Flow**," a commercial platform. **[assessment] Effectively in maintenance.** |
| `HazyResearch/flyingsquid` | Python | Apache-2.0 | ~315 | 69 commits; last substantive update noted **June 2020**. Deps: pgmpy, PyTorch (optional). **[assessment] Dormant — read it as a reference implementation.** |
| `JieyuZ2/wrench` | Python | Apache-2.0 | ~231 | 173 commits; latest noted additions **Jan 2023** (Hyper Label Model). **[assessment] Lightly maintained; valuable as a *benchmark and datasets* resource for validating a C# reimplementation.** |
| `NorskRegnesentral/skweak` | Python | **MIT** | ~926 | README: **"Skweak is no longer actively maintained."** Deps: spacy ≥3.0, hmmlearn ≥0.3. |
| `wurenzhi/hyper_label_model` | Python | **UNVERIFIED** — not fetched | — | Companion to ICLR 2023 paper. Requires a GNN. |
| `uds-lsv/critical_wsl` | Python | **UNVERIFIED** — not fetched | — | Companion to ACL 2023 Theme Paper Award. |
| `dallascard/dawid_skene` | Python | **UNVERIFIED** — not fetched | — | Reference implementation of Dawid–Skene (1979). |

**[assessment] None of these is usable in NAADAP** — all Python, all would need network egress or a Python runtime in the container. Their value is as reference implementations to port from and, in WRENCH's case, as a source of public benchmark datasets against which a C# reimplementation can be validated for correctness. Porting `flyingsquid` and a Dawid–Skene reference to C# and reproducing WRENCH numbers on one or two public datasets would be strong evidence of implementation correctness for the prize submission.

---

## 11. Full verified citation list

Every item below was retrieved during this research. Items marked UNVERIFIED were surfaced in search results but not fetched in full.

1. **VERIFIED** — Ratner, A. J., De Sa, C. M., Wu, S., Selsam, D., Ré, C. "Data Programming: Creating Large Training Sets, Quickly." *NeurIPS 29 (NIPS 2016)*. arXiv:1605.07723.
2. **VERIFIED** — Ratner, A., Bach, S. H., Ehrenberg, H., Fries, J., Wu, S., Ré, C. "Snorkel: Rapid Training Data Creation with Weak Supervision." *PVLDB* 11(3):269–282, 2017. arXiv:1711.10160.
3. **VERIFIED** — Ratner, A., Hancock, B., Dunnmon, J., Sala, F., Pandey, S., Ré, C. "Training Complex Models with Multi-Task Weak Supervision." *AAAI 2019*. arXiv:1810.02840.
4. **VERIFIED** — Fu, D. Y., Chen, M. F., Sala, F., Hooper, S. M., Fatahalian, K., Ré, C. "Fast and Three-rious: Speeding Up Weak Supervision with Triplet Methods." *ICML 2020*, PMLR v119. arXiv:2002.11955.
5. **VERIFIED** — Zhang, J., Yu, Y., Li, Y., Wang, Y., Yang, Y., Yang, M., Ratner, A. "WRENCH: A Comprehensive Benchmark for Weak Supervision." *NeurIPS 2021 Datasets & Benchmarks Track* (Oral). arXiv:2109.11377.
6. **VERIFIED** — Lison, P., Barnes, J., Hubin, A. "skweak: Weak Supervision Made Easy for NLP." *ACL-IJCNLP 2021: System Demonstrations*, 337–346. ACL `2021.acl-demo.40`. DOI `10.18653/v1/2021.acl-demo.40`. arXiv:2104.09683.
7. **VERIFIED** — Zhu, D., Shen, X., Mosbach, M., Stephan, A., Klakow, D. "Weaker Than You Think: A Critical Look at Weakly Supervised Learning." *ACL 2023*, 14229–14253. ACL `2023.acl-long.796`. DOI `10.18653/v1/2023.acl-long.796`. arXiv:2305.17442. *(ACL 2023 Theme Paper Award.)*
8. **VERIFIED** — Zhang, T., Cai, L., Li, J., Roberts, N., Guha, N., Lee, J., Sala, F. "Stronger Than You Think: Benchmarking Weak Supervision on Realistic Tasks." *NeurIPS 2024 Datasets & Benchmarks Track*. arXiv:2501.07727.
9. **VERIFIED** — Dawid, A. P., Skene, A. M. "Maximum Likelihood Estimation of Observer Error-Rates Using the EM Algorithm." *JRSS Series C (Applied Statistics)* 28(1):20–28, 1979. DOI `10.2307/2346806`.
10. **VERIFIED** (metadata only; full text not fetched) — Bach, S. H., Rodriguez, D., Liu, Y., Luo, C., Shao, H., Xia, C., Sen, S., Ratner, A., Hancock, B., Alborzi, H., Kuchhal, R., Ré, C., Malkin, R. "Snorkel DryBell: A Case Study in Deploying Weak Supervision at Industrial Scale." *SIGMOD '19*. DOI `10.1145/3299869.3314036`. arXiv:1812.00417.
11. **VERIFIED** (metadata only) — Wu, R., Chen, S.-E., Zhang, J., Chu, X. "Learning Hyper Label Model for Programmatic Weak Supervision." *ICLR 2023*. arXiv:2207.13545. dblp `conf/iclr/WuCZC23`.
12. **VERIFIED** (abstract only) — Álvarez, V., Mazuelas, S., An, S., Dasgupta, S. "Reliable Programmatic Weak Supervision with Confidence Intervals for Label Probabilities." arXiv:2508.03896, 2025.
13. **VERIFIED** (metadata only) — Zhang, J., Hsieh, C.-Y., Yu, Y., Zhang, C., Ratner, A. "A Survey on Programmatic Weak Supervision." arXiv:2202.05433, 2022. *(Useful as an orientation map; I did not rely on it for any mechanism above.)*
14. **UNVERIFIED** — Hendrycks, D., et al. "CUAD: An Expert-Annotated NLP Dataset for Legal Contract Review." arXiv:2103.06268. Cited in §5.4 for the cost of expert contract annotation; metadata from search results only.
15. **UNVERIFIED** — Loh, P.-L., Wainwright, M. J. — the graph-structured inverse-covariance result that MeTaL's §4 relies on (their reference [22]). I read MeTaL's use of it but did not fetch the original.
16. **UNVERIFIED** (surfaced in search, not fetched) — applied WS in adjacent domains: clinical notes arXiv:2206.12088; bank transactions arXiv:2305.18430; climate/infrastructure arXiv:2302.01887.

### Source-code artifacts read directly

- `snorkel-team/snorkel@main : snorkel/labeling/model/label_model.py` — read to establish the determinism verdict in §7 (`TrainConfig` defaults, `_init_params()` random scaling of `mu_init`, seeding behaviour in `fit()`).

### Local working files

Downloaded PDFs and extracted text used for verbatim transcription:
`/tmp/claude-0/-home-user-NAADAP/404d06b0-acd3-541d-9590-ed4aa6fd068d/scratchpad/`
— `dp.pdf` / `dp.txt` (1605.07723), `metal2.pdf` / `metal2.txt` (1810.02840),
`2002.11955.pdf` / `.txt` (FlyingSquid), `2109.11377.pdf` / `.txt` (WRENCH).
