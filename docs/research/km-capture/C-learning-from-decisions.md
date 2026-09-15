# Area C — Learning a Decision Policy from Recorded Expert Decisions

Research file for NAADAP (NAVAIR/NAWCAD prize challenge). Scope: how the literature says
to learn a vehicle-selection policy from a corpus of recorded contracting-officer choices
(FPDS/USAspending "Referenced IDV PIID"), subject to NAADAP's hard constraints —
offline, deterministic (>=95% identical top-5), 1 core / 2 GB / 30 min per 20 docs,
no LLM in core path, C#/.NET 9, minimal deps, citable evidence.

**Verification protocol.** Every citation below was retrieved. Bibliographic facts come
from Crossref/dblp/publisher records; all formulas quoted were extracted from the actual
PDF text, not from memory. Items I could not machine-verify are marked explicitly.

---

## 0. Executive verdict (read this first)

The single most important structural fact: **the NAADAP decision is single-step.**
A contracting officer faces one requirement, one (implicit) set of eligible vehicles,
and picks one. There is no state transition, no trajectory, no credit assignment
over time.

That fact collapses the problem space dramatically:

| Frame | What it becomes at horizon 1 | Verdict |
|---|---|---|
| Inverse RL (MaxEnt) | Exactly conditional logit (proof in §5.3) | **Reject** — strictly more machinery, zero extra content |
| Behavioral cloning | Plain supervised classification; the `T²ε` compounding-error pathology vanishes (§5.4) | Acceptable but weaker than a choice model |
| Learning-to-rank (LambdaMART) | Works, but optimizes a *graded-relevance* objective NAADAP does not have | **Second choice** — strong C# story, weaker evidence story |
| Discrete choice / conditional logit (McFadden) | Native fit: varying choice sets, shared coefficients, closed-form likelihood, globally concave | **Primary recommendation** |
| Listwise LTR (ListMLE) | *Is* Plackett-Luce MLE, i.e. the rank-ordered extension of the same model (§3.4) | **Use as the top-5 extension of the primary** |

**Recommendation:** build a **conditional logit (McFadden) model with office-level
regression controls**, estimated by Newton-Raphson on a globally concave log-likelihood,
reimplemented in C# (~200-300 LOC, no dependency). Use **ML.NET FastTree (LambdaMART)**
as a validation benchmark and optional non-linear residual model, not as the primary.
Reject IRL. Treat the counterfactual/unbiased-LTR machinery as a *diagnostic and a
caveat generator*, not as a correction you can actually certify (§7).

---

## 1. The problem, stated in the language of the literature

Each FPDS order record gives a tuple `(C_i, j_i, x_i)`:

- `j_i` — the chosen parent vehicle (Referenced IDV PIID). **Observed.**
- `x_i` — requirement features: description text, PSC, NAICS, issuing office (DoDAAC),
  dollar value, competition codes, set-aside. **Observed.**
- `C_i` — the set of vehicles the CO actually considered. **NOT observed.** This is the
  central methodological wound; see §6 and §8.

This is precisely the *discrete choice* data structure (Tomlinson et al. 2021, §2:
"a chooser `a ∈ A` is presented a nonempty choice set `C ⊆ U` and they choose one item
`i ∈ C`"). It is only secondarily a ranking problem: we have one positive per query and
no graded relevance labels. That asymmetry drives the whole recommendation.

---

## 2. Learning to rank — formulations and actual losses

### 2.1 The three formulations

- **Pointwise** — regress/classify a relevance score per (query, item) independently.
  Loses the query grouping. For NAADAP it degenerates into "is this vehicle the chosen
  one, yes/no", i.e. a badly-calibrated binary classifier with ~1:N class imbalance.
- **Pairwise** — learn from preference pairs `(i ≻ j)` within a query. RankNet, RankSVM.
- **Listwise** — define a loss over the whole permutation/list. ListNet, ListMLE, LambdaRank.

### 2.2 RankNet (Burges et al. 2005) — exact loss and gradient

Verified from MSR-TR-2010-82 (text extracted from the PDF).

Model maps `x ∈ R^n → f(x)`; scores `s_i = f(x_i)`, `s_j = f(x_j)`. Learned pairwise
probability via a sigmoid:

```
P_ij ≡ P(U_i ▷ U_j) ≡ 1 / (1 + e^{−σ(s_i − s_j)})
```

Cross-entropy cost against known probability `P̄_ij`:

```
C = −P̄_ij log P_ij − (1 − P̄_ij) log(1 − P_ij)
```

With `S_ij ∈ {0, ±1}` and the deterministic-ranking assumption `P̄_ij = ½(1 + S_ij)`:

```
C = ½(1 − S_ij) σ(s_i − s_j) + log(1 + e^{−σ(s_i − s_j)})
```

Note the margin property, quoted verbatim from the report: *"when `s_i = s_j`, the cost is
`log 2`, so the model incorporates a margin"*, and asymptotically the cost is linear
(wrong order) or zero (right order).

Gradient:

```
∂C/∂s_i = σ ( ½(1 − S_ij) − 1/(1 + e^{σ(s_i − s_j)}) ) = −∂C/∂s_j        (Eq. 1)
```

### 2.3 The λ factorization — the key computational trick

Define

```
λ_ij ≡ ∂C(s_i − s_j)/∂s_i = σ ( ½(1 − S_ij) − 1/(1 + e^{σ(s_i − s_j)}) )   (Eq. 3)
```

Let `I` be the set of index pairs `{i,j}` with `U_i ▷ U_j` (so `S_ij = 1`). Then the
weight update factorizes per-document rather than per-pair:

```
δw_k = −η Σ_{{i,j}∈I} ( λ_ij ∂s_i/∂w_k − λ_ij ∂s_j/∂w_k ) ≡ −η Σ_i λ_i ∂s_i/∂w_k

λ_i = Σ_{j:{i,j}∈I} λ_ij − Σ_{j:{j,i}∈I} λ_ij                              (Eq. 4)
```

Burges: *"training time dropped from close to quadratic in the number of urls per query,
to close to linear."* The `λ_i` are interpretable as forces/arrows on each document.

**Relevance to NAADAP:** this factorization is the reason pairwise LTR is affordable at
all. With ~50-200 candidate vehicles per requirement, the quadratic pair count is
manageable anyway, so the trick buys less here than in web search.

### 2.4 LambdaRank — the `|ΔNDCG|` reweighting

The insight is that IR metrics are flat or discontinuous in the scores, so you cannot
differentiate them. LambdaRank sidesteps this by *writing down the gradient directly*
rather than deriving it from a loss. Verified (Eq. 6):

```
λ_ij = ∂C(s_i − s_j)/∂s_i = ( −σ / (1 + e^{σ(s_i − s_j)}) ) · |ΔNDCG|
```

where `|ΔNDCG|` is the NDCG change from swapping the rank positions of `U_i` and `U_j`.
Burges notes `|ΔNDCG|` may be substituted by the change in any chosen IR measure.

The report is candid that this is empirically, not analytically, justified: it cites
Donmez, Svore & Burges (SIGIR 2009) on *local* optimality, and raises the question
*"Suppose you write down some arbitrary λ's. Does there necessarily exist a cost for
which those λ's are the gradients?"* — a Poincaré-lemma/integrability question. For
LambdaRank specifically such a cost does exist: *"it is the RankNet cost multiplied by
ΔNDCG."*

**Caveat for a legal determination:** LambdaRank's objective is defined by its gradient,
not by a likelihood. There is no probability statement to report, no standard errors,
no coefficient with units. That is a real defensibility cost.

### 2.5 LambdaMART — MART + LambdaRank

Verified from MSR-TR-2010-82 §7. Gradients `ȳ_i` are the `λ_i`; least squares computes
the splits; each tree models the `λ_i` for the *entire dataset*, not one query.

The implied utility and its derivatives:

```
C   = Σ_{{i,j}⇌I} |ΔZ_ij| log(1 + e^{−σ(s_i − s_j)})
ρ_ij ≡ 1/(1 + e^{σ(s_i − s_j)}) = −λ_ij / (σ|Z_ij|)
∂C/∂s_i   = Σ_{{i,j}⇌I} −σ |ΔZ_ij| ρ_ij
∂²C/∂s_i² = Σ_{{i,j}⇌I} σ² |ΔZ_ij| ρ_ij (1 − ρ_ij)
```

Newton step for leaf `k` of tree `m`:

```
γ_km = − Σ_{x_i∈R_km} Σ_{{i,j}⇌I} |ΔZ_ij| ρ_ij
       ─────────────────────────────────────────────────
         Σ_{x_i∈R_km} Σ_{{i,j}⇌I} |ΔZ_ij| σ ρ_ij(1 − ρ_ij)
```

Two properties worth recording:

1. *"the choice of σ will make no difference to the training, since the `γ_km`'s scale as
   `1/σ`"* — one fewer hyperparameter to tune, and one fewer source of run-to-run drift.
2. *"LambdaMART is able to choose splits and leaf values that may decrease the utility for
   some queries, as long as the overall utility increases."* This is a **fairness/defensibility
   hazard for NAADAP**: a split that systematically degrades recommendations for, say,
   small-business set-aside requirements would be accepted by training if it improves the
   aggregate. A conditional logit with an explicit coefficient cannot hide that as easily.

Algorithm (verbatim structure): for each of `N` trees, set `y_i = λ_i`,
`w_i = ∂y_i/∂F_{k−1}(x_i)`, build an `L`-leaf regression tree on `{x_i, y_i}`, set
`γ_lk = Σ y_i / Σ w_i` per leaf, then `F_k(x_i) = F_{k−1}(x_i) + η Σ_l γ_lk I(x_i ∈ R_lk)`.

### 2.6 Provenance note — Yahoo! LTR Challenge

*"an ensemble of LambdaMART rankers won Track 1 of the 2010 Yahoo! Learning To Rank
Challenge"* (Burges 2010, abstract; cites Chapelle, Chang & Liu 2010). This is the usual
justification for LambdaMART's status as the default strong baseline.

---

## 3. Listwise methods — and the bridge to discrete choice

### 3.1 ListNet (Cao, Qin, Liu, Tsai & Li, ICML 2007)

Verified from the paper PDF.

**Permutation probability.** For scores `s` and increasing strictly positive `φ`:

```
P_s(π) = Π_{j=1..n}  φ(s_{π(j)}) / Σ_{k=j..n} φ(s_{π(k)})
```

**Top-k probability** (Definition 7, Theorem 8):

```
P_s(G_k(j_1,...,j_k)) = Π_{t=1..k}  φ(s_{j_t}) / Σ_{l=t..n} φ(s_{j_l})
```

reducing the cost from `n!` permutations to `n!/(n−k)!` subgroups, computed in `O(nk)`.

**Loss** (Eq. 4) — cross-entropy between ground-truth and predicted top-k distributions,
with `φ = exp`:

```
L(y^(i), z^(i)(f_ω)) = − Σ_{g∈G_k} P_{y^(i)}(g) log P_{z^(i)(f_ω)}(g)
```

Gradient (Eq. 5):

```
Δω = ∂L/∂ω = − Σ_{g∈G_k} (∂P_{z^(i)(f_ω)}(g)/∂ω) · P_{y^(i)}(g) / P_{z^(i)(f_ω)}(g)
```

Optimized with plain gradient descent on a linear neural network.

> **The critical observation for NAADAP.** At `k = 1` — which is exactly what ListNet's
> own experiments used (*"we implemented ListNet with k = 1"*) — the top-one probability
> is
> ```
> P_s(j) = exp(s_j) / Σ_k exp(s_k)
> ```
> This is *identically* McFadden's conditional logit choice probability (§4.1). And
> NAADAP's data has exactly one positive per query — i.e. a top-1 label. **ListNet at
> k=1 on single-positive data and conditional logit are the same estimator.** The
> econometric framing is therefore not a detour; it is the same mathematics with 50 extra
> years of inference theory, interpretation, and specification testing attached.

Also verified: *"when there are only two documents for each query ... the listwise loss
function in ListNet becomes equivalent to the pairwise loss function in RankNet."*

### 3.2 ListMLE (Xia, Liu, Wang, Zhang & Li, ICML 2008)

Verified from the ICML 2008 PDF. The likelihood loss (Eq. 9):

```
φ(g(x), y) = − log P(y | x; g),    P(y|x;g) = Π_{i=1..n} exp(g(x_{y(i)})) / Σ_{k=i..n} exp(g(x_{y(k)}))
```

And, stated in the paper verbatim: *"The probability distribution turns out to be a
**Plackett-Luce model** (Marden, 1995)."*

Properties the paper proves/claims for the likelihood loss: **consistent, sound, continuous,
differentiable, convex, `O(n)` complexity** (their Table; cosine loss fails soundness,
cross-entropy is `O(n·n!)` in general). ListMLE is the only one of the three that is
all-green.

**This is the top-5 story for NAADAP.** If you want a defensible *ranked* output rather
than a single argmax, ListMLE = Plackett-Luce MLE = the **rank-ordered / "exploded" logit**
of Beggs, Cardell & Hausman (1981) and Hausman & Ruud (1987). Same model, same convex
objective, and the econometrics literature supplies specification tests for it.

### 3.3 LETOR

Qin, Liu, Xu & Li (2010), *Information Retrieval* 13:346–374, DOI 10.1007/s10791-009-9123-y.
The benchmark line that standardized LTR evaluation: fixed document corpora, query sets,
extracted features, and fold partitions, enabling the MAP/NDCG comparisons reported across
the RankNet/ListNet/ListMLE papers.

**Relevance to NAADAP: mostly cautionary.** LETOR's features and its graded 0-4 relevance
judgments do not transfer. NAADAP has binary, single-positive, implicit labels. Do not
import LETOR-tuned hyperparameters or expect LETOR-like NDCG numbers; construct your own
held-out temporal split instead (§8.4).

### 3.4 Summary of the listwise → choice-model bridge

```
ListNet (k=1)  ==  softmax over scores  ==  conditional logit (McFadden 1974)
ListMLE        ==  Plackett-Luce        ==  rank-ordered logit (Beggs et al. 1981)
```

Everything else in the listwise literature is a variation on these two.

---

## 4. Discrete choice / conditional logit — the recommended frame

### 4.1 The model

McFadden (1974), "Conditional Logit Analysis of Qualitative Choice Behavior," in
P. Zarembka (ed.), *Frontiers in Econometrics*, Academic Press, pp. 105–142.

*Verification note:* the canonical PDF at Berkeley's official McFadden reprint archive
(`eml.berkeley.edu/reprints/mcfadden/zarembka.pdf`) is a **38-page scan with no text
layer**, so I could not extract formulas from the primary. All formulas below are quoted
from Train (2009) Ch. 3, which I did extract in full, and which cites McFadden directly.

Random utility: `U_nj = V_nj + ε_nj` with `ε_nj` iid Type-I extreme value (Gumbel),
density `f(ε_nj) = e^{−ε_nj} e^{−e^{−ε_nj}}`, variance `π²/6`. Differences of two iid
extreme-value variables are logistic. With `V_nj = β'x_nj` (linear in parameters):

```
P_ni = e^{β'x_ni} / Σ_j e^{β'x_nj}
```

### 4.2 Why this is the right fit for NAADAP — five concrete reasons

**(a) Global concavity ⇒ determinism.** Quoted verbatim from Train (2009) §3.1:

> *"Importantly, McFadden (1974) demonstrated that the log-likelihood function with these
> choice probabilities is globally concave in parameters β, which helps in the numerical
> maximization procedures."*

This is the strongest determinism guarantee available anywhere in this research area.
A globally concave objective has a **unique maximum**. Initialization does not matter,
optimizer path does not matter, there are no local optima to land in. Given fixed data
and a fixed convergence tolerance, the fitted `β` is unique up to numerical tolerance —
and the resulting top-5 ordering is stable far beyond the 95% requirement. Contrast with
LambdaMART, whose determinism is *procedural* (fixed data order, fixed seed, fixed thread
count) rather than *mathematical*.

**(b) Varying choice sets are native.** The denominator `Σ_{j∈C_i}` ranges over whatever
set is eligible for requirement `i`. NAADAP genuinely needs this: a vehicle is ineligible
if it is expired, out of scope, below ceiling, or wrong set-aside. A fixed-class
multiclass softmax (e.g. ML.NET's `LbfgsMaximumEntropy`) **cannot express this** — see §4.5.

**(c) Shared `β` across alternatives ⇒ generalization to new vehicles.** Coefficients
attach to *attributes* (PSC match score, NAICS match, scope-text similarity, ceiling
headroom, office affinity), not to vehicle identities. A newly-awarded IDIQ with no order
history still gets a utility. A per-class model cannot score an unseen class at all. For a
system whose whole purpose is recommending among a changing vehicle inventory, this is
close to decisive.

**(d) Readable coefficients — the evidence requirement.** The output is not a bare score.
It is `V_j = β₁·(PSC match) + β₂·(scope similarity) + β₃·(ceiling headroom) + ...` with
each `β` estimable, signed, and reportable with a standard error from the inverse Hessian.
A determination can state: *"SeaPort-NxG ranked first primarily because of exact PSC
alignment (contribution +1.82) and prior use by this office (+0.94), against a ceiling-
headroom penalty of −0.31."* That is a defensible artifact. NAADAP's requirement that
"a bare score is not acceptable" is satisfied structurally, not by bolting on a
post-hoc explainer.

**(e) Interpretable probabilities.** Because `Σ_i P_ni = 1` and `P_ni ∈ (0,1)`, the top-5
comes with a calibrated confidence distribution, not an arbitrary score scale.

### 4.3 The estimator, concretely (this is what gets written in C#)

Negative log-likelihood over `N` observed choices:

```
ℓ(β) = − Σ_{i=1..N} [ β'x_{i,c(i)} − log Σ_{j∈C_i} exp(β'x_ij) ]
```

Gradient — **observed minus expected feature counts**, the classic exponential-family form:

```
∇ℓ(β) = − Σ_i [ x_{i,c(i)} − Σ_{j∈C_i} P_ij x_ij ]
```

Hessian — a sum of covariance matrices, hence positive semi-definite (⇒ `ℓ` convex,
log-likelihood concave):

```
∇²ℓ(β) = Σ_i [ Σ_{j∈C_i} P_ij x_ij x_ij' − (Σ_j P_ij x_ij)(Σ_j P_ij x_ij)' ]
```

Newton-Raphson: `β ← β − [∇²ℓ]^{-1} ∇ℓ`. Converges in typically 5–15 iterations.
Add L2 (`+ λ‖β‖²`) for strict concavity and to handle separation; this also guarantees
a unique optimum even if some feature is collinear.

**Implementation assessment for C#/.NET 9:**
- ~200–300 LOC. Dense `double[,]` Hessian of size `p×p` where `p` = number of features
  (realistically 20–60). Cholesky solve, no external linear algebra package needed —
  `System.Numerics` / hand-rolled Cholesky is sufficient at this size.
- Determinism: fix the summation order over observations (sort by a stable key), use
  `double` throughout, avoid parallel reductions in the accumulation loops. Then the
  result is bit-reproducible.
- Cost: training is offline and one-time (ship the fitted `β` in the container).
  **Inference is a dot product per candidate vehicle plus one softmax** — microseconds.
  The 30-min / 20-doc budget is not remotely stressed.
- **No runtime dependency at all**, which is the best possible answer to "minimal
  dependencies" and "no network."

### 4.4 The IIA problem — the one real weakness, and its remedy

Verified from Train (2009) §3.3.2. The logit model implies **Independence from Irrelevant
Alternatives**: the ratio `P_ni/P_nk` is unaffected by any other alternative. The
red-bus/blue-bus example (Chipman 1960; Debreu 1960): if car and blue bus each have
probability ½ and a red bus identical to the blue bus is introduced, logit predicts
`1/3, 1/3, 1/3` where the correct answer is `½, ¼, ¼`.

Equivalently, **proportional substitution** — the cross-elasticity

```
E_{iz_nj} = − β_z z_nj P_nj
```

*"is the same for all `i`: `i` does not enter the formula."* An improvement in one
alternative draws proportionately from all others.

**Why this bites NAADAP specifically.** Contract vehicles are *not* well-separated
alternatives. SeaPort-NxG and a NAWCAD-specific MAC covering overlapping services are
near-substitutes — a red bus and a blue bus. Naive conditional logit will over-predict
the combined probability of the cluster and under-predict a structurally different
alternative (e.g. GSA MAS or a new competition).

**Remedies, in order of cost:**
1. **Nested logit** — group vehicles into nests (Navy MACs / GWACs / GSA MAS / OTAs) with
   a within-nest correlation parameter per nest. Still closed-form, still MLE, still
   deterministic; loses global concavity in general but is well-behaved with good starting
   values (use the conditional-logit fit as the start). This is the standard fix.
2. **Report the IIA exposure rather than fixing it.** Tests of IIA exist (Train §3.3.2
   describes Hausman-McFadden-style re-estimation on a subset of alternatives; McFadden
   1987 developed a further procedure). Running the test and *disclosing* which vehicle
   pairs fail it is itself defensible evidence.
3. Do **not** reach for mixed logit — it requires simulation (random draws), which
   directly attacks the determinism requirement unless you fix the Halton/quasi-random
   sequence, and it costs interpretability.

### 4.5 ML.NET does NOT ship conditional logit — an important negative finding

`LbfgsMaximumEntropyMulticlassTrainer` (Microsoft.ML.Trainers) is a **multinomial**
logit / maximum-entropy classifier: a *fixed* set of classes, one shared feature vector
per example, and a *separate* coefficient vector per class. Verified from Microsoft Learn:
"predict a target using a maximum entropy multiclass classifier trained with L-BFGS";
"input label column data must be key type."

Conditional logit needs the opposite: *alternative-specific* feature vectors `x_ij`, a
*varying* choice set `C_i`, and a *single shared* `β`. These are different models, and
McFadden's own terminology separates them. **You must implement conditional logit
yourself.** §4.3 shows this is genuinely easy; the good news is L-BFGS/Newton on a concave
objective is textbook.

---

## 5. Inverse reinforcement learning and imitation learning — reasoned rejection

The task brief asked for a verdict rather than enthusiasm. Here it is, with the argument.

### 5.1 Ng & Russell (2000) — the ill-posedness

Ng, A.Y. & Russell, S., "Algorithms for Inverse Reinforcement Learning," *ICML 2000*,
pp. 663–670 (dblp `conf/icml/NgR00`; ACM DL DOI 10.5555/645529.657801).

*Verification note:* the Stanford PDF uses embedded Type-1 fonts with a custom encoding
and no ToUnicode map; neither my extractor nor WebFetch could recover its text. The
bibliographic record is verified; **the degeneracy claim is corroborated from a primary
source that cites it** — Ziebart et al. (2008), verbatim:

> *"Unfortunately, both the IRL concept and the matching of feature counts are ambiguous.
> Each policy can be optimal for many reward functions (e.g., all zeros) and many policies
> lead to the same feature counts. ... No method is proposed to resolve the ambiguity."*

The all-zero reward makes every policy optimal. IRL is ill-posed without a
disambiguating principle.

### 5.2 Abbeel & Ng (2004) — apprenticeship learning

Abbeel, P. & Ng, A.Y., "Apprenticeship Learning via Inverse Reinforcement Learning,"
*ICML 2004*, DOI 10.1145/1015330.1015430. Assumes reward linear in known features and
matches **feature expectations** between expert and learner. Ziebart et al. note this
matching is necessary and sufficient for equal performance under that linearity assumption
— but does not identify the reward.

### 5.3 Ziebart et al. (2008) MaxEnt IRL — and why it *is* conditional logit at horizon 1

Ziebart, B.D., Maas, A.L., Bagnell, J.A. & Dey, A.K., "Maximum Entropy Inverse
Reinforcement Learning," *AAAI 2008*, pp. 1433–1438.

Their distribution over paths (Eq. 2), extracted verbatim:

```
P(ζ_i | θ) = (1/Z(θ)) e^{θ'f_{ζ_i}} = (1/Z(θ)) e^{Σ_{s_j∈ζ_i} θ'f_{s_j}}
```

and the gradient (Eq. 6):

```
∇L(θ) = f̃ − Σ_ζ P(ζ|θ) f_ζ = f̃ − Σ_{s_i} D_{s_i} f_{s_i}
```

> **The reduction.** Set the horizon to 1. A "path" `ζ` is a single action — choosing one
> vehicle `j`. Then `f_ζ = x_j`, and `Z(θ) = Σ_{k∈C} exp(θ'x_k)`. Equation 2 becomes
> ```
> P(j | θ) = exp(θ'x_j) / Σ_{k∈C} exp(θ'x_k)
> ```
> which is **exactly** McFadden's conditional logit (§4.1), with `θ ≡ β`. And Eq. 6
> becomes `∇L = x_{chosen} − Σ_j P_j x_j` — **exactly** the conditional-logit score
> equation from §4.3.
>
> MaxEnt IRL at horizon 1 *is* conditional logit. Not "similar to." Identical.

Everything MaxEnt IRL adds beyond that — the partition function over trajectories, the
expected state-visitation-frequency computation `D_{s_i}`, the forward/backward dynamic
program, the transition model `T` and its intractable marginalization (their Eq. 3, which
they must approximate in Eq. 4) — exists **solely to handle sequential structure NAADAP
does not have.** At `T=1` all of it is either vacuous or degenerate.

This equivalence is not my inference alone; it is now established in the literature.
Kang, E.H., "A Lecture Note on Offline RL and IRL, Part II: Foundations of Inverse
Reinforcement Learning and Dynamic Discrete Choice Models," arXiv:2605.30843 (29 May 2026),
states that *"two communities, structural econometricians studying dynamic discrete choice
(DDC) and machine learners studying entropy-regularized IRL, have been working on exactly
the same probabilistic model under different names,"* and *"begin[s] by proving their
equivalence."* (Related: arXiv:2512.24407, "Efficient Inference for Inverse Reinforcement
Learning and Dynamic Discrete Choice Models.") The DDC↔MaxEnt-IRL correspondence holds
with Gumbel shocks at unit scale and entropy coefficient `λ=1`.

**Verdict: reject IRL.** Adopting it would mean importing a framework that is (i) formally
ill-posed without the max-entropy fix, (ii) after the fix, mathematically identical to a
model econometrics has had since 1974, (iii) carrying substantial extra machinery that
does nothing at horizon 1, and (iv) far harder to explain to a contracting officer or a
GAO protest reviewer than "a regression that weighs PSC match, scope similarity, and
ceiling headroom." The econometric presentation is the *same model with a better
defensibility story*. There is no scenario in which IRL is the right answer for NAADAP.

*(The only frame under which IRL would earn its keep: if NAADAP modelled a multi-step
acquisition strategy — e.g. consolidate now vs. wait vs. re-compete, with downstream
consequences. That is a different product.)*

### 5.4 Behavioral cloning — acceptable, and here is the precise reason

Behavioral cloning = supervised learning of `π(a|s)` from expert demonstrations. Its
canonical failure is **compounding error / covariate shift**: the learner drifts to states
the expert never visited, and errors accumulate.

Ross, Gordon & Bagnell (2011), "A Reduction of Imitation Learning and Structured Prediction
to No-Regret Online Learning," *AISTATS 2011*, PMLR 15:627–635. Theorem 2.1 (verified
verbatim from the PDF, attributed there to Ross & Bagnell 2010):

```
Let E_{s∼d_{π*}}[ℓ(s,π)] = ε,  then  J(π) ≤ J(π*) + T²ε
```

The paper notes this *"extra cost that grows quadratically in T"* is tight: they exhibit
an example where `J(π̂_sup) = (1−εT)J(π*) + T²ε`, i.e. `Θ(T²ε)`.

> **At `T = 1`, `T²ε = ε`.** The entire justification for DAgger, SEARN, and the
> interactive-imitation literature evaporates. For a single-step decision, plain
> supervised learning on logged decisions is *not* a degraded approximation — it attains
> the bound. There is no distribution shift to compound because there is no second step.

So behavioral cloning is legitimate here. But note that a "behavioral cloning" classifier
over vehicles with fixed classes is strictly *worse* than conditional logit on three counts
(§4.2b, c, d: no varying choice sets, no unseen vehicles, weaker evidence). Conditional
logit is behavioral cloning done properly for this data structure.

---

## 6. Counterfactual / unbiased learning to rank — useful framing, unusable guarantee

### 6.1 The machinery (Joachims, Swaminathan & Schnabel 2017)

WSDM '17, pp. 781–789, DOI 10.1145/3018661.3018699. Extracted verbatim, the IPS estimator:

```
Δ̂_IPS(y | x_i, ȳ_i, o_i) = Σ_{y: o_i(y)=1}  rank(y|y)·r_i(y) / Q(o_i(y)=1 | x_i, ȳ_i, r_i)
```

with the unbiasedness proof turning entirely on two conditions, both of which the paper
states explicitly:

1. **Positivity:** *"unbiased ... if `Q(o_i(y)=1|x_i,ȳ_i,r_i) > 0` for all `y` that are
   relevant."* And for convergence, *"propensities bounded away from 0."*
2. **Unconfoundedness:** *"an additional requirement for making `Δ̂_IPS` computable while
   remaining unbiased is that the propensities only depend on observable information
   (i.e., unconfoundedness)."*

Empirical risk: `R̂_IPS(S) = (1/N) Σ_i Σ_{y:o_i(y)=1 ∧ r_i(y)=1} rank(y|S(x_i)) / Q(...)`,
and ERM over it, yielding Propensity SVM-Rank.

Related: Schnabel, Swaminathan, Singh, Chandak & Joachims, "Recommendations as Treatments:
Debiasing Learning and Evaluation," *ICML 2016*, PMLR 48:1670–1679 — the same IPS treatment
for recommender selection bias. And Swaminathan & Joachims, "The Self-Normalized Estimator
for Counterfactual Learning," *NIPS 2015* — identifies **propensity overfitting** (with
capacity to overfit the weights, the learner picks hypotheses that avoid or overrepresent
the data) and fixes it with a multiplicative control variate (Norm-POEM).

### 6.2 Why this does not transfer cleanly to FPDS — be honest about this

Joachims' setting: a ranker *presented* a ranking `ȳ_i`, the system *logged* it, and
position-based examination probability `p_r` is estimable by swap interventions. **None of
those three things exist in FPDS.**

- There was no presented ranking. A CO did not scroll a SERP of vehicles.
- There is no logging policy with a known propensity. The "propensity" a vehicle was
  considered is entirely latent.
- The choice set `C_i` is unobserved (§1), so you cannot even enumerate the denominator
  over which propensities would be defined.

You would therefore have to **estimate the propensity from scratch from the same
observational data**, which reintroduces exactly the confounding you are trying to remove,
and unconfoundedness is fundamentally untestable.

Positivity is also likely violated *by construction*: a vehicle that a given office has
never used has an estimated propensity at or near zero, so its inverse weight explodes or
is undefined. This is not a tuning problem; see §8.2.

Furthermore, Oosterhuis, H., "Reaching the End of Unbiasedness: Uncovering Implicit
Limitations of Click-Based Learning to Rank," *ICTIR '22*, arXiv:2206.12204, establishes
that *"counterfactual estimation can only produce unbiased methods for click behavior
based on affine transformations"* and that *"it is impossible for existing approaches to
provide unbiasedness guarantees for all plausible click behavior models."* Even in its home
domain, ULTR's unbiasedness is conditional on a behavior model you must assume.

### 6.3 What to take from it anyway

Not the estimator — the **vocabulary and the diagnostics**:

- Name the bias explicitly in the documentation: heavily-used vehicles are chosen more
  partly because they are familiar/administratively easy, not because they are the best
  fit. This is a *popularity/incumbency* confound.
- **Do include a usage-frequency feature and read its coefficient.** In conditional logit
  a "prior orders on this vehicle by this office" term gets an estimated `β`. If that
  coefficient is large, you have *measured* the incumbency effect rather than silently
  absorbing it.
- Then offer a **counterfactual/"de-incumbency" view**: re-score with the incumbency
  coefficient zeroed out. This directly answers "is this recommendation just path
  dependence?" and is cheap, deterministic, and explainable. This is the single most
  valuable idea to carry over from this literature.

---

## 7. Choice set confounding — the most directly relevant paper found

Tomlinson, K., Ugander, J. & Benson, A.R., "Choice Set Confounding in Discrete Choice,"
*KDD '21*, pp. 1571–1581, DOI 10.1145/3447548.3467378, arXiv:2105.07959.

This paper is about NAADAP's exact situation, in NAADAP's exact model class.

**Observation 1** (extracted verbatim):

> *If, for all `a ∈ A`, `C ∈ C_D`, `i ∈ C`, at least one of
> (1) `Pr(C) = Pr(C|a)` (chooser-independent choice sets) or
> (2) `Pr(i|a,C) = Pr(i|C)` (chooser-independent preferences)
> holds, then `Pr(i|C) = E_a[Pr(i|a,C)]`. If both conditions are violated, then this
> equality can fail.*

**Apply it to NAADAP.** The "chooser" `a` is the issuing contracting office.

- Condition (1) fails: choice sets *are* office-dependent. A NAWCAD office has a different
  menu of ordering-eligible vehicles than a NAVSUP office — by delegation, by DPA, by
  established ordering procedures.
- Condition (2) fails: preferences *are* office-dependent. Offices have habits,
  relationships, and local guidance.

**Both conditions are violated, so the observed `Pr(vehicle | requirement)` in FPDS is
not the quantity NAADAP wants.** This is the rigorous statement of the observational-data
caveat, and it is citable.

The paper's toy example (cat/dog/fish) is worth reproducing in NAADAP's documentation: it
shows a *spurious context effect* that **improves test-set predictive performance on data
from the same distribution** while making biased counterfactual predictions. Verbatim:
*"This spurious context effect would be seized upon by context-based models and even
result in improved predictive performance on test data drawn from the same distribution.
However, these models would make biased predictions on counterfactual examples where sets
are chosen from a different distribution."*

> **Direct consequence for NAADAP's validation plan:** held-out accuracy on FPDS is
> *not* evidence that the model recommends well. A model can score high precisely by
> learning the confound. Say so in the methodology, and do not let held-out NDCG be the
> headline metric.

**Their remedies, and which one NAADAP should use.** They adapt two causal-inference
tools: IPW and **regression controls** — *"we can incorporate covariates into the choice
model itself, recovering individual preferences as long as those covariates capture
preference heterogeneity."* They note *"The use of regression controls in discrete choice
(i.e., including chooser covariates in the utility function) is standard in econometrics."*

**Recommendation: use regression controls, not IPW.** Include issuing-office covariates
(DoDAAC, major command, service) directly in the utility function, interacted with vehicle
family. This is deterministic, adds no variance, requires no propensity model, and is
standard practice. IPW here would require modelling `Pr(C|a)` which is unobserved (§6.2).

They also connect this to the context-dependent random utility model (CDM) for the case
where chooser covariates are unavailable — relevant as a fallback if office codes turn
out to be too sparse.

Repo: `tomlinsonk/choice-set-confounding` — Python, 3 stars, last pushed 2021-10-04,
**no LICENSE file declared** (treat as all-rights-reserved research code; read it for
method, do not vendor it). Not a dependency candidate; the method is a modelling choice,
not a library.

**Historical note:** Manski, C.F. & Lerman, S.R., "The Estimation of Choice Probabilities
from Choice Based Samples," *Econometrica* 45(8):1977–1988 (1977), DOI 10.2307/1914121 —
the original weighted-likelihood treatment of samples drawn conditional on the chosen
alternative. Cited by Tomlinson et al. as the precursor to their IPW. Relevant if NAADAP
ever subsamples FPDS stratified by chosen vehicle (which it should **not** do without
applying their correction).

---

## 8. The observational-data caveat, assembled

### 8.1 What the data actually records

Historical awards record what **was** chosen, not what was **optimal**. A CO's choice
reflects: fit, but also expedience, delegated authority, existing relationships,
time pressure, aversion to a new vehicle's learning curve, and whatever the last similar
requirement used. Nothing in FPDS separates these.

### 8.2 Positivity / overlap fails in high dimensions

D'Amour, A., Ding, P., Feller, A., Lei, L. & Sekhon, J., "Overlap in Observational Studies
with High-Dimensional Covariates," *Journal of Econometrics* 221(2):644–654 (2021),
DOI 10.1016/j.jeconom.2019.10.014, arXiv:1711.02582. Abstract, verbatim:

> *"Our key innovation is to explore how strict overlap restricts global discrepancies
> between the covariate distributions in the treated and control populations. Exploiting
> results from information theory, we derive explicit bounds on the average imbalance in
> covariate means under strict overlap and show that these bounds become more restrictive
> as the dimension grows large."*

**Consequence for NAADAP:** any propensity-weighted or causal-adjustment scheme over a
rich feature set (text embeddings, PSC, NAICS, office, dollar bands) is fighting a
curse-of-dimensionality result. Strict overlap — every vehicle having non-trivial
probability of being chosen for every requirement type — is *false on its face* for
contract vehicles, and gets more false as you add features. This is an independent reason
(beyond §6.2) to prefer regression controls over reweighting.

### 8.3 Policy learning from observational data — what is legitimate

Athey, S. & Wager, S., "Policy Learning With Observational Data," *Econometrica*
89(1):133–161 (2021), DOI 10.3982/ECTA15732. The credible approach: doubly-robust
empirical welfare maximization over a *restricted* policy class (e.g. depth-limited
decision trees), with causal effects identified via selection-on-observables or
instrumental variables, and regret bounds relative to the best policy in that class.

Two lessons NAADAP should adopt even without adopting their estimator:
1. **Restrict the policy class deliberately.** A simple, inspectable scoring function is
   not a compromise — in the policy-learning literature it is the thing that makes regret
   guarantees and interpretability possible at once. This supports conditional logit over
   a 500-tree ensemble.
2. **Identification is an assumption you state, not a property you obtain.** NAADAP should
   write down its identifying assumption explicitly ("conditional on PSC, NAICS, dollar
   band, and issuing office, vehicle selection is as-if independent of unobserved
   suitability") and state plainly that it is not testable.

### 8.4 Recognized dangers, checklist form

| Danger | Mechanism in FPDS | Mitigation |
|---|---|---|
| **Incumbency / path dependence** | Vehicle chosen because previously used by this office | Explicit usage feature + counterfactual re-score with it zeroed (§6.3) |
| **Choice-set confounding** | Office-dependent eligible sets AND office-dependent preferences (both Observation-1 conditions violated) | Office regression controls (§7) |
| **Popularity feedback loop** | Recommending popular vehicles makes them more popular, which the next retrain absorbs | Freeze training data vintage; version and date-stamp the fitted model; do not retrain on NAADAP-influenced awards |
| **Survivorship / eligibility drift** | Vehicles expire; historical choices include vehicles no longer available | Filter candidate set by validity at recommendation time, not training time |
| **Spurious context effects** | Model scores well on held-out data by learning the confound (Tomlinson §3) | Do not headline held-out NDCG; validate against SME-labelled ground truth |
| **Positivity violation** | Office–vehicle pairs with zero historical support | Do not invert propensities; use regression controls; flag zero-support recommendations in output |
| **Aggregate-vs-subgroup harm** | LambdaMART explicitly accepts splits that hurt some queries if aggregate improves (§2.5) | Prefer a model whose per-feature effect is uniform and inspectable; if using trees, report per-segment metrics |

**Validation implication.** The honest evaluation design is a **temporal holdout** (train
on awards through year `T`, test on `T+1`) plus **SME adjudication of a sample**, reported
separately. Agreement-with-history is a *consistency* metric, not a *correctness* metric,
and NAADAP's documentation should use exactly that language.

---

## 9. Evaluation metrics

### 9.1 Definitions

**DCG / NDCG** (Järvelin & Kekäläinen, "Cumulated Gain-based Evaluation of IR Techniques,"
*ACM TOIS* 20(4):422–446, 2002, DOI 10.1145/582415.582418). Burges' form (Eq. 5):

```
DCG@T = Σ_{i=1..T} (2^{l_i} − 1) / log(1 + i)
NDCG@T = DCG@T / maxDCG@T  ∈ [0,1]
```

**MRR** — mean of `1/rank_first_relevant`. **MAP** — mean of average precision.
Burges notes both *"are designed for binary relevance levels"*, whereas NDCG and ERR
*"handle multiple levels of relevance"*.

**ERR** (Chapelle, Metzler, Zhang & Grinspan, CIKM 2009) — cascade-model-based; Burges
notes *"unlike NDCG, the change in ERR induced by swapping two urls"* is not local.

### 9.2 Properties, and which to use

All of NDCG/MRR/MAP/ERR share the defect Burges states plainly: *"viewed as functions of
the model scores, they are everywhere either discontinuous or flat, so gradient descent
appears to be problematic, since the gradient is everywhere either zero or not defined."*
This is the entire reason LambdaRank exists.

Theoretical properties: Wang, Y., Wang, L., Li, Y., He, D. & Liu, T.-Y., "A Theoretical
Analysis of NDCG Type Ranking Measures," *COLT 2013*, PMLR 30 (arXiv:1304.6480). Findings:
standard NDCG with logarithmic discount **converges to 1 as the number of items grows,
regardless of the ranking function**; whether NDCG retains *consistent distinguishability*
depends on how fast the discount decays, with `1/r` as the critical point. Logarithmic-
discount NDCG does have consistent distinguishability despite the convergence.

> **For NAADAP, use MRR and Precision@5, not NDCG.** Reasons:
> 1. NAADAP has **exactly one positive per query** and no graded relevance. NDCG's
>    multi-level machinery is inert; with a single relevant item NDCG@k reduces to a
>    monotone function of the rank of that item — i.e. it *is* reciprocal-rank-like, but
>    with an opaque normalizer.
> 2. MRR is directly interpretable to a non-technical reviewer: "the correct vehicle
>    appeared at position 1.4 on average."
> 3. The challenge asks for a **top-5**, so `Recall@5` / `Precision@5` ("was the
>    historically-chosen vehicle in our five?") is the metric that matches the deliverable.
> 4. Report NDCG@5 as a secondary for comparability with the LTR literature, but do not
>    let it drive model selection.
>
> And per §7: label all of these as **agreement-with-historical-practice**, not accuracy.

---

## 10. Implementations — C#/.NET assessment

### 10.1 ML.NET FastTree (the serious C# option)

`Microsoft.ML.Trainers.FastTree.FastTreeRankingTrainer`, assembly `Microsoft.ML.FastTree.dll`,
NuGet `Microsoft.ML.FastTree` (v4.0.1 stable; v5.0.0-preview.1.25125.4).
Repo: **dotnet/machinelearning — C#, MIT, 9,358 stars, last push 2026-09-14 (actively
maintained).**

**It really is LambdaMART.** Verified by reading
`src/Microsoft.ML.FastTree/FastTreeRanking.cs` on `main`:
- Trainer summary string: *"Trains gradient boosted decision trees to the LambdaRank
  quasi-gradient."*
- Objective class `LambdaRankObjectiveFunction : ObjectiveFunctionBase, IStepSearch`.
- `NdcgTruncationLevel`, `FastNdcgTest`, `DCGCalculator.MaxDCG`, per-query
  `DcgPermutationComparer`.
- The pair loop computes `lambdaP` and `weightP = lambdaP * (2.0 - lambdaP)`, then
  `deltaLambdasHigh += lambdaP * deltaNdcgP; pLambdas[low] -= lambdaP * deltaNdcgP;`
  — i.e. exactly `λ_ij · |ΔNDCG|` accumulated per §2.4/2.5.

**Determinism assessment — good, with named conditions:**

*Favourable:*
- `FastTreeArguments.cs` defaults: `Seed = 123`, `FeatureSelectionSeed = 123`,
  `FeatureFraction = 1` (no feature subsampling), `BaggingSize = 0` (bagging **disabled**).
  **With defaults there is no stochasticity in tree construction at all.**
- `NumberOfThreads` is exposed (`public int? NumberOfThreads = null`). NAADAP runs on
  **1 CPU core**, so set it to `1` explicitly. Single-threaded execution removes
  floating-point reduction-order nondeterminism outright.
- Gradient computation is `GetGradientInOneQuery(int query, int threadIndex)` with
  per-thread buffers writing into disjoint per-query slices — so even multi-threaded, the
  λ accumulation has no shared-scalar race. (Histogram construction in the tree builder
  I did **not** audit; setting threads=1 makes the question moot.)
- Pure managed C#. No native binary.

*Caveats:*
- The sigmoid is a **1,000,000-bin lookup table** (`_sigmoidBins = 1000000`, built by
  `FillSigmoidTable`), and the comment notes it *"is built for a specific sigmoid
  parameter, so assumes this will be constant throughout computation."* Deterministic, but
  an *approximation* — bitwise results are tied to that table, hence to the library version.
- **Not exportable to ONNX** (per Microsoft's trainer-characteristics table). You are
  pinned to the ML.NET runtime for inference.
- Pin the exact NuGet version in the Dockerfile; a FastTree version bump can change
  numeric output.
- **Documentation citation error worth knowing:** the ML.NET FastTree docs cite
  `arXiv:1505.01866` for "MART." That paper is actually **DART: Dropouts meet Multiple
  Additive Regression Trees** (Rashmi & Gilad-Bachrach) — DART is a *modification* of MART,
  not MART. The correct primary citation is **Friedman, J.H., "Greedy Function
  Approximation: A Gradient Boosting Machine," *Annals of Statistics* 29(5), 2001,
  DOI 10.1214/aos/1013203451.** Do not propagate Microsoft's mis-citation into NAADAP's
  documentation.

**Verdict:** credible, deterministic under stated settings, MIT, actively maintained,
zero native deps. Suitable as a **benchmark/secondary model**. Not preferred as primary
because of the evidence problem (§2.4, §2.5) and the labels mismatch (§9.2).

### 10.2 ML.NET LightGBM — recommend AGAINST

`Microsoft.ML.Trainers.LightGbm.LightGbmRankingTrainer`, NuGet `Microsoft.ML.LightGbm`.
Wraps **lightgbm-org/LightGBM** (C++, 18,766 stars, active; note the repo **moved from
`microsoft/LightGBM` to `lightgbm-org/LightGBM`**).

**Disqualifying determinism finding.** LightGBM's own docs describe a `deterministic`
parameter:

> *`deterministic` [default=false, type=bool]* — *"used only with cpu device type";
> "setting this to true should ensure the stable results when using the same data and the
> same parameters (and different num_threads)"; "when you use the different seeds,
> different LightGBM versions, the binaries compiled by different compilers, or in
> different systems, the results are expected to be different."*
> Notes: *"setting this to true may slow down the training"; "to avoid potential
> instability due to numerical issues, please set force_col_wise=true or force_row_wise=true
> when setting deterministic=true."*

**But `LightGbmRankingTrainer.Options` does not expose it.** I enumerated the full
published Options surface: `BatchSize, CategoricalSmoothing, CustomGains,
EarlyStoppingRound, EvaluationMetric, ExampleWeightColumnName, FeatureColumnName,
HandleMissingValue, L2CategoricalRegularization, LabelColumnName, LearningRate,
MaximumBinCountPerFeature, MaximumCategoricalSplitPointCount, MinimumExampleCountPerGroup,
MinimumExampleCountPerLeaf, NumberOfIterations, NumberOfLeaves, NumberOfThreads,
RowGroupColumnName, Seed, Sigmoid, Silent, UseCategoricalSplit, UseZeroAsMissingValue,
Verbose, Booster`. **There is no `Deterministic` field**, and no `force_col_wise` /
`force_row_wise`. You cannot set the flag LightGBM itself says you need.

Additional strikes: requires the **native `lib_lightgbm` binary** in the container
(violates "minimal dependencies," adds a platform-specific artifact to an offline image);
and determinism is only guaranteed within a fixed compiler/platform/version anyway.

**Verdict: recommend against.** FastTree gives you the same algorithm family in pure
managed C# with better determinism control.

### 10.3 Other

- **allegro/allRank** — Python, Apache-2.0, 1,009 stars, last push 2024-08-06 (drifting).
  PyTorch neural LTR (ListNet/ListMLE/NDCG-Loss2 etc.). **Not usable** — Python, PyTorch,
  and GPU-oriented. Useful only as a *reference implementation* to check your loss
  derivations against.
- **codelibs/ranklib** — Java, 110 stars, updated 2026-06-05. Maintained fork of the
  classic RankLib (RankNet/LambdaMART/ListNet/AdaRank/Coordinate Ascent). **Not usable**
  in-process from .NET; valuable as an offline cross-check of your LambdaMART numbers.
- **Conditional logit in C#: nothing exists.** No maintained .NET library implements
  conditional/nested logit. This is the §4.3 reimplementation, and it is genuinely small.
  Cross-check the C# implementation against R's `survival::clogit` / `mlogit`, or Stata's
  `cmclogit`, during development (offline afterwards).

---

## 11. Determinism scorecard

| Method | Determinism basis | Rating under NAADAP constraints |
|---|---|---|
| **Conditional logit (Newton)** | **Mathematical** — globally concave LL, unique optimum (McFadden 1974 via Train 2009). Fix summation order ⇒ bit-reproducible | **Best.** Far exceeds 95% |
| **ListMLE / Plackett-Luce** | Convex loss (Xia et al. 2008 prove convexity); same guarantee | **Best** |
| **Nested logit** | Closed-form MLE; not globally concave, but deterministic given fixed start (use CL fit) | Very good |
| **ML.NET FastTree LambdaMART** | **Procedural** — defaults have no randomness (FeatureFraction=1, BaggingSize=0, Seed=123); set NumberOfThreads=1; pin NuGet version | Good; version-pinned |
| **ML.NET LightGBM** | `deterministic` flag **not exposed**; native binary; cross-compiler variance | **Unacceptable** |
| **IPS / propensity-weighted ERM** | Deterministic *arithmetic*, but estimates have high variance from near-zero propensities (§6.2, §8.2); propensity overfitting (Swaminathan & Joachims 2015) | Unstable in practice |
| **MaxEnt IRL** | Convex for deterministic MDPs — but reduces to conditional logit at T=1 anyway | Moot; reject |
| **Mixed logit** | Simulation-based; deterministic only with frozen quasi-random draws | Avoid |
| **Neural LTR (ListNet/RankNet as NN)** | SGD, init seeds, nondeterministic kernels | Avoid |

---

## 12. Final recommendation, ranked

### Recommended, in order

**1. Conditional logit (McFadden) with office regression controls — PRIMARY.**
Fit `P(j|C_i) = exp(β'x_ij)/Σ_{k∈C_i} exp(β'x_ik)` by Newton-Raphson with L2. Include
office/command covariates per Tomlinson et al. (§7) and an explicit incumbency feature
per §6.3. Output = top-5 by `P`, with per-feature utility contributions as the evidence.
- Meets determinism *mathematically*, not procedurally (§11).
- ~250 LOC C#, zero dependencies, inference in microseconds (§4.3).
- Produces the citable evidence NAADAP requires **by construction** (§4.2d).
- Handles varying eligible-vehicle sets and never-before-used vehicles (§4.2b,c).

**2. ListMLE / rank-ordered (exploded) logit — the top-5 extension.**
If SME-labelled *ranked* ground truth becomes available, ListMLE (= Plackett-Luce =
Beggs/Cardell/Hausman rank-ordered logit) extends model 1 to full rankings with the same
convexity and the same coefficients. Zero conceptual switching cost (§3.2, §3.4).

**3. Nested logit — the IIA remedy, if diagnostics demand it.**
Group vehicles into families (Navy MACs / GWACs / GSA MAS / OTA). Deploy only if IIA tests
show material violation among near-substitute vehicles (§4.4).

**4. ML.NET FastTree LambdaMART — benchmark and non-linearity probe.**
Run it to answer "is a flexible non-linear model materially better than our linear
utility?" If the gap is small, that is itself strong evidence for shipping the
interpretable model. If large, inspect which interactions it found and add them as
explicit terms to model 1. `NumberOfThreads=1`, defaults elsewhere, pinned NuGet (§10.1).

### Recommending AGAINST

- **Inverse reinforcement learning (any variant).** Mathematically identical to
  conditional logit at horizon 1 (§5.3, proven by reduction and corroborated by
  arXiv:2605.30843), formally ill-posed without the max-entropy fix (§5.1), and strictly
  harder to defend. No upside whatsoever for a single-step decision.
- **ML.NET LightGBM ranking.** `deterministic` is not exposed through the Options surface
  (§10.2), plus a native binary dependency. FastTree dominates it here.
- **Mixed logit.** Simulation-based; trades determinism and interpretability for a
  flexibility NAADAP has not demonstrated it needs.
- **IPS / propensity-weighted ERM as a correction you certify.** Positivity and
  unconfoundedness are not satisfiable on FPDS (§6.2), positivity degrades with
  dimension (§8.2), and unbiasedness guarantees are themselves conditional
  (Oosterhuis 2022). Use the *framing* and the counterfactual re-score (§6.3); do not
  claim an unbiased estimator.
- **Neural listwise rankers (RankNet/ListNet as neural nets, transformer rankers).**
  SGD nondeterminism, no readable coefficients, no benefit at this data scale.
- **Pointwise LTR.** Discards the within-requirement competition that is the entire signal.
- **Headlining held-out NDCG.** Per Tomlinson et al. §3, a confounded model can score
  *better* on same-distribution holdout precisely by learning the confound (§7).

### One-line summary

The experts' recorded choices should be read as a **discrete choice dataset**, not as a
ranking dataset and not as expert trajectories: fit McFadden's conditional logit with
office controls, report the coefficients as the evidence, disclose the choice-set
confounding, and use LambdaMART only to check you have not left accuracy on the table.

---

## 13. Verified bibliography

Every entry below was retrieved. "Extracted" = I pulled and read the actual PDF text.
"Record verified" = bibliographic record confirmed via Crossref/dblp/publisher.

### Learning to rank
1. Burges, C.J.C., Shaked, T., Renshaw, E., Lazier, A., Deeds, M., Hamilton, N. & Hullender, G. (2005). "Learning to Rank using Gradient Descent." *ICML '05*, pp. 89–96. DOI **10.1145/1102351.1102363**. *Record verified; PDF located at icml.cc proceedings.*
2. Burges, C.J.C. (2010). "From RankNet to LambdaRank to LambdaMART: An Overview." *Microsoft Research Technical Report* **MSR-TR-2010-82**. *Extracted in full (19 pp.) — all formulas in §2 come from this text.*
3. Cao, Z., Qin, T., Liu, T.-Y., Tsai, M.-F. & Li, H. (2007). "Learning to Rank: From Pairwise Approach to Listwise Approach." *ICML '07*, pp. 129–136. DOI **10.1145/1273496.1273513**. *Extracted.*
4. Xia, F., Liu, T.-Y., Wang, J., Zhang, W. & Li, H. (2008). "Listwise Approach to Learning to Rank — Theory and Algorithm." *ICML '08*, pp. 1192–1199. DOI **10.1145/1390156.1390306**. *Extracted.*
5. Qin, T., Liu, T.-Y., Xu, J. & Li, H. (2010). "LETOR: A benchmark collection for research on learning to rank for information retrieval." *Information Retrieval* **13**:346–374. DOI **10.1007/s10791-009-9123-y**. *Record verified.*
6. Joachims, T. (2002). "Optimizing Search Engines using Clickthrough Data." *KDD '02*, pp. 133–142. DOI **10.1145/775047.775067**. *Record verified. (RankSVM — the pairwise baseline.)*
7. Friedman, J.H. (2001). "Greedy Function Approximation: A Gradient Boosting Machine." *Annals of Statistics* **29**(5). DOI **10.1214/aos/1013203451**. *Record verified. The true MART citation.*
8. Rashmi, K.V. & Gilad-Bachrach, R. (2015). "DART: Dropouts meet Multiple Additive Regression Trees." **arXiv:1505.01866**. *Verified — and noted as mis-cited by the ML.NET docs as "MART" (§10.1).*

### Metrics
9. Järvelin, K. & Kekäläinen, J. (2002). "Cumulated gain-based evaluation of IR techniques." *ACM TOIS* **20**(4):422–446. DOI **10.1145/582415.582418**. *Record verified.*
10. Wang, Y., Wang, L., Li, Y., He, D. & Liu, T.-Y. (2013). "A Theoretical Analysis of NDCG Type Ranking Measures." *COLT 2013*, PMLR 30. **arXiv:1304.6480**. *Verified.*
11. Chapelle, O., Metzler, D., Zhang, Y. & Grinspan, P. (2009). "Expected Reciprocal Rank for Graded Relevance Measures." *CIKM 2009*. *Cited in Burges (2010) ref [6]; record not independently pulled — **secondary citation**.*

### Discrete choice / preference learning
12. McFadden, D. (1974). "Conditional Logit Analysis of Qualitative Choice Behavior." In P. Zarembka (ed.), *Frontiers in Econometrics*, Academic Press, pp. 105–142. *PDF located at Berkeley's official McFadden reprint archive; **scan has no text layer**, formulas sourced from Train (2009) which cites it directly.*
13. Train, K.E. (2009). *Discrete Choice Methods with Simulation*, 2nd ed., Cambridge University Press. Ch. 3 "Logit." *Extracted (42 pp.) — source for global concavity, IIA, red-bus/blue-bus, proportional substitution.*
14. Bradley, R.A. & Terry, M.E. (1952). "Rank Analysis of Incomplete Block Designs: I. The Method of Paired Comparisons." *Biometrika* **39**(3–4):324–345. DOI **10.1093/biomet/39.3-4.324** (also **10.2307/2334029**). *Record verified.*
15. Plackett, R.L. (1975). "The Analysis of Permutations." *Journal of the Royal Statistical Society Series C (Applied Statistics)* **24**(2):193–202. DOI **10.2307/2346567**. *Record verified.*
16. Luce, R.D. (1959). *Individual Choice Behavior: A Theoretical Analysis*. John Wiley & Sons. *Record verified (choice axiom; source of the Plackett-Luce name alongside #15).*
17. Beggs, S., Cardell, S. & Hausman, J. (1981). "Assessing the potential demand for electric cars." *Journal of Econometrics* **17**(1):1–19. DOI **10.1016/0304-4076(81)90056-7**. *Record verified. Origin of exploded/rank-ordered logit.*
18. Hausman, J.A. & Ruud, P.A. (1987). "Specifying and testing econometric models for rank-ordered data." *Journal of Econometrics* **34**(1–2):83–104. DOI **10.1016/0304-4076(87)90068-6**. *Record verified. Includes a Hausman specification test for IIA.*
19. Manski, C.F. & Lerman, S.R. (1977). "The Estimation of Choice Probabilities from Choice Based Samples." *Econometrica* **45**(8):1977–1988. DOI **10.2307/1914121**. *Record verified.*

### IRL / imitation learning
20. Ng, A.Y. & Russell, S. (2000). "Algorithms for Inverse Reinforcement Learning." *ICML 2000*, pp. 663–670. DOI **10.5555/645529.657801** (dblp `conf/icml/NgR00`). *Record verified; **PDF text not machine-extractable** (custom font encoding). Degeneracy claim corroborated verbatim from #22.*
21. Abbeel, P. & Ng, A.Y. (2004). "Apprenticeship Learning via Inverse Reinforcement Learning." *ICML '04*. DOI **10.1145/1015330.1015430**. *Extracted (header/abstract confirmed).*
22. Ziebart, B.D., Maas, A.L., Bagnell, J.A. & Dey, A.K. (2008). "Maximum Entropy Inverse Reinforcement Learning." *AAAI 2008*, pp. 1433–1438. *Extracted — source for Eqs. 2, 4, 6 and the ambiguity quote.*
23. Ross, S., Gordon, G.J. & Bagnell, J.A. (2011). "A Reduction of Imitation Learning and Structured Prediction to No-Regret Online Learning." *AISTATS 2011*, PMLR **15**:627–635. *Extracted — Theorem 2.1 `J(π) ≤ J(π*) + T²ε`.*
24. Kang, E.H. (2026). "A Lecture Note on Offline RL and IRL, Part II: Foundations of Inverse Reinforcement Learning and Dynamic Discrete Choice Models." **arXiv:2605.30843**, submitted 29 May 2026. *Verified via arXiv abstract page. Establishes the DDC ↔ MaxEnt-IRL equivalence.*
25. "Efficient Inference for Inverse Reinforcement Learning and Dynamic Discrete Choice Models." **arXiv:2512.24407**. *Listed in search results; **abstract page not independently fetched — treat as UNVERIFIED** pending a direct pull. Cited only as corroborating context for #24.*

### Counterfactual / unbiased LTR
26. Joachims, T., Swaminathan, A. & Schnabel, T. (2017). "Unbiased Learning-to-Rank with Biased Feedback." *WSDM '17*, pp. 781–789. DOI **10.1145/3018661.3018699**. (Extended abstract: *IJCAI 2018*, DOI 10.24963/ijcai.2018/738.) *Extracted — IPS estimator, unbiasedness proof, positivity & unconfoundedness conditions.*
27. Schnabel, T., Swaminathan, A., Singh, A., Chandak, N. & Joachims, T. (2016). "Recommendations as Treatments: Debiasing Learning and Evaluation." *ICML 2016*, PMLR **48**:1670–1679. *Record verified via PMLR and dblp (`conf/icml/SchnabelSSCJ16`). No Crossref DOI (PMLR).* 
28. Swaminathan, A. & Joachims, T. (2015). "The Self-Normalized Estimator for Counterfactual Learning." *NIPS 2015*. *Record verified via dblp (`conf/nips/SwaminathanJ15`) and NeurIPS proceedings. Source for propensity overfitting / Norm-POEM.*
29. Oosterhuis, H. (2022). "Reaching the End of Unbiasedness: Uncovering Implicit Limitations of Click-Based Learning to Rank." *ICTIR '22*. **arXiv:2206.12204**. *Verified via arXiv abstract page.*

### Causal inference / observational policy learning
30. Tomlinson, K., Ugander, J. & Benson, A.R. (2021). "Choice Set Confounding in Discrete Choice." *KDD '21*, pp. 1571–1581. DOI **10.1145/3447548.3467378**, **arXiv:2105.07959**. *Extracted — Observation 1, Theorem 2, cat/dog/fish example, IPW vs regression controls. **The most directly relevant paper in this file.*** Code: `tomlinsonk/choice-set-confounding` (Python, no declared license, last push 2021-10-04).
31. Athey, S. & Wager, S. (2021). "Policy Learning With Observational Data." *Econometrica* **89**(1):133–161. DOI **10.3982/ECTA15732**. *Record verified.*
32. D'Amour, A., Ding, P., Feller, A., Lei, L. & Sekhon, J. (2021). "Overlap in Observational Studies with High-Dimensional Covariates." *Journal of Econometrics* **221**(2):644–654. DOI **10.1016/j.jeconom.2019.10.014**, **arXiv:1711.02582**. *Verified; abstract extracted.*

### Software
33. **dotnet/machinelearning** — ML.NET. C#, **MIT**, 9,358 stars, last push 2026-09-14, not archived. `Microsoft.ML.FastTree` / `Microsoft.ML.LightGbm`. *Source files `FastTreeRanking.cs` and `FastTreeArguments.cs` read directly from `main`.*
34. **lightgbm-org/LightGBM** — C++, 18,766 stars, last update 2026-09-15, active. *Note the org move from `microsoft/LightGBM`.* `deterministic` parameter documented at lightgbm.readthedocs.io.
35. **allegro/allRank** — Python/PyTorch, **Apache-2.0**, 1,009 stars, last push 2024-08-06. Reference only.
36. **codelibs/ranklib** — Java, 110 stars, updated 2026-06-05. Maintained RankLib fork. Cross-check only.

### Unverifiable / secondary
- **#11** (Chapelle et al. 2009, ERR) — known only through Burges (2010)'s reference list; not independently retrieved.
- **#25** (arXiv:2512.24407) — **UNVERIFIED**; appeared in search results, abstract page not fetched.
- **#12** (McFadden 1974) — publication record verified and the authoritative PDF located, but it is an **image-only scan**; no formula was transcribed from it. All logit formulas are cited to Train (2009) Ch. 3 (#13), which attributes them to McFadden.
- **#20** (Ng & Russell 2000) — bibliographic record verified, **PDF text not extractable**; the degeneracy claim is quoted from Ziebart et al. (#22) citing it.
