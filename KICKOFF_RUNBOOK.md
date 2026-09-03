# Kickoff Runbook

Generic, reusable version — fill in the fields below, then follow the
numbered steps. This file lives at the root of every project cloned
from `=TEMPLATE=`; there's nothing project-specific left in it until
you fill in the table.

---

## Fill in before starting

| Field | Value |
| --- | --- |
| Project Name | NAADAP |
| Client Name | Kyle (Human) on behalf of NAVAIR/NAWCAD |
| Preferred Software Programming Language | TBD |
| Description of deliverable(s) | Software Application and source code |
| General budget (time/tokens) | Full budget considerations - This is for real money, so let's make this clean, and make it right. |
| User documentation needed? | Full documentation course, following Government Acquisition standards, SETR documentation, and MBSE |

**Notes on specific fields:**

- **User documentation needed?** — Not just a user manual this time. We're going full pedantic mode on this, completely defining what we are going to build, validating that what we are building is the right thing, and completely verifying through testing rigor and traceability that we built what we said we would.

- **Client Name** — For this effort you can continue to refer to me (Kyle) as the client. But this application will ultimate go directly to the US Navy's Naval Air Systems Command (NAVAIR) and Naval Air Warfare Center Aircraft Division (NAWCAD). 
- **Description of deliverable(s)** — A software application that represents an innovative solution for the NAADAP challenge. The Navy seeks intelligent recommender systems that leverage advanced data science, natural language processing, and machine learning to analyze procurement documentation, identify common acquisition requirements, and recommend strategic contract vehicles that improve efficiency and streamline the acquisition process. The final product need to function perfectly, with stable useability and conformity to government standards. More than that, the source code must be documented in a professional manner following industry standard best practices.  And just as important as the functional deliverable itself is the documentation that must be provided to go along with it demonstrating the path to completion, including following US Navy Systems Engineering Technical Review process (SETR) and MBSE practices. Ideally this will also demonstrate a strong underlying Agile Systems Engineering approach.
- **General budget (time/tokens)** — 
Project Timeline:
7/30/26: Prize Challenge Release and Pre-Screening application portal open on USA.gov and the Tech Grove Website.
10/2/26 11:59PM EDT: Deadline for submission of solution package by 11:59 pm Eastern Time. Submissions must meet the requirements outlined in the Initial Submission Requirements.
10/26/26: Pre-Screening Evaluation selection by NAWCAD and other DoD SMEs based on the technical requirements and stated judging criteria. Up to 7 semi-finalists will be selected for participation in the final Presentation/Demo Day.
10/26/26: Notification to semi-finalists of selection to participate in Presentation/Demo Day and instructions for submission of Demo Day materials.
11/12/26: Semi-finalists will submit all deliverables for Demo Day by 11:59 pm Eastern Time per instructions provided via email, meeting the Finals Submission Requirements.
11/19/26: Final Demo Day at the Central Florida Tech Grove. Finalists will present in person, and two winners will be selected and announced to receive the $100,000 and $50,000 prizes.

Project Budget: This takes precendence over all other projects in the queue. Whatever it takes to complete the research, plan and execute the product, and document the process CORRECTLY following US Navy standards, THAT's what we need to do.

---

## 0. Naming — check before creating anything

Confirm the intended repo name doesn't collide with an existing local
folder or GitHub repo before proceeding. If it does, resolve the
naming conflict now rather than partway through setup.

---

## 1. Create the repository from `=TEMPLATE=`

On GitHub: open the `=TEMPLATE=` repo → **Use this template** →
**Create a new repository** → name it per the **Project Name** field
above → create.

Clone it locally:

```bash
cd C:\_Dev\GIT
git clone https://github.com/<your-username>/<Project Name>.git
cd <Project Name>
```

---

## 2. Secrets — none of these carry over from the template automatically

Go to the new repo's **Settings → Secrets and variables → Actions**.

**Both parts below are required — this is not a choice between them.**
They authenticate with two different services for two different
purposes: the Anthropic credential (2a) lets Claude Code actually run;
`RELAY_TOKEN` (2b) lets GitHub's own label and comment operations
correctly trigger the *next* workflow run, which the default GitHub
token is deliberately blocked from doing. The only genuine either/or
in this step is *within* 2a — which single Anthropic credential to
use, not whether to also do 2b.

### 2a. Choose a credential — decide this from your General Budget entry above

This is a genuine time-vs-money tradeoff, not just a convenience
choice, and the **General budget** field you filled in above is what
should decide it:

- **Ample funding, time pressure** → **`ANTHROPIC_API_KEY`** (separate
  API billing). Nothing throttles concurrent throughput the way a
  subscription's shared usage window does — many agents can run in
  parallel without competing with each other or with your own
  interactive use, which gets the project done faster. You pay for
  that speed directly.
- **Funding-constrained, time is more flexible** → **subscription**
  (`CLAUDE_CODE_OAUTH_TOKEN`, the default). Work proceeds within your
  existing subscription's usage window rather than incurring new
  cost — a deliberately slower burn, and the right choice when the
  budget itself is the binding constraint rather than the calendar.

Also worth weighing: the subscription avoids the budget-management
overhead (expiration dates, a separate balance to track and top up)
that caused real friction on an earlier project — a genuine point in
its favor even setting the time/money tradeoff aside. Reconsider
either default specifically if the project needs queryable,
programmatic usage tracking — that capability currently exists more
reliably for API-key billing than for subscription usage, and remains
an open question for how Product Manager's future throttling work
will actually query remaining capacity.

**If using the subscription (default):**

```
claude setup-token
```

Opens a browser, logs in with your subscription account, and prints a
token starting `sk-ant-oat01-...`. Copy it immediately — shown once.

Store as **New repository secret**: name exactly
`CLAUDE_CODE_OAUTH_TOKEN`.

**If using separate API billing instead:** generate a key at
console.anthropic.com → Settings → API keys, and store it as
`ANTHROPIC_API_KEY`. Then in step 6 below, make that the active
(uncommented) line instead.

Don't add both unless you specifically want a fallback — an unused
extra credential is harmless, but the active one should be
deliberate, not whichever happens to load first.

### 2b. `RELAY_TOKEN` — a fine-grained personal access token

GitHub → profile picture → **Settings** → **Developer settings** →
**Personal access tokens** → **Fine-grained tokens** → **Generate new
token**.

- Token name: `agent-relay-token` (or similar)
- Expiration: choose a long window, or no expiration — a short window
  caused a full, hard-to-diagnose account-wide outage on an earlier
  project when it silently expired mid-project
- Repository access: **Only select repositories** → this repo only
- Permissions: **Contents** (Read and write), **Issues** (Read and
  write), **Pull requests** (Read and write)
- Generate, copy the value immediately (`github_pat_...`, shown once)

Store as **New repository secret**: name exactly `RELAY_TOKEN`.

---

## 3. Confirm the GitHub App covers this repo

Settings → **GitHub Apps** → find the Claude app → **Configure**.
Confirm the new repo is included in its repository access.

---

## 4. Confirm Actions are enabled

Settings → **Actions → General** → confirm "Allow all actions and
reusable workflows" (or an equivalent allow-list) is selected.

---

## 5. Create the labels

```bash
gh auth login   # one-time, if not already authenticated
./scripts/setup-labels.sh
```

Confirm with `gh label list` — expect 22 labels, including
`agent:product-manager`.

---

## 6. Verify the credential line in `agent-relay.yml`

Open `.github/workflows/agent-relay.yml`, check the `with:` block
under the "Run Claude Code as..." step, and confirm it matches
whichever credential you chose in step 2a — exactly one of
`claude_code_oauth_token` or `anthropic_api_key` active
(uncommented), the other commented out. If it doesn't match:

```bash
git add .github/workflows/agent-relay.yml
git commit -m "Set active credential to match this project's choice"
git push
```

---

## 7. Submit the kickoff issue

On the new repo: **Issues → New issue → Project Kickoff**. This
auto-applies `agent:product-manager` the moment it's submitted.

In the "What are we building?" field, use the **Description of
deliverable(s)** value from the table above, expanded to full
sentences if it was kept brief there. If you're tracking a budget and
want it recorded as project context, mention it explicitly here too.

---

## 8. Where the interview happens, and how to answer it

The Product Manager's questions appear as a comment on the kickoff
issue — check the **Issues** tab, open the issue, scroll to comments.

To reply: **just write a plain comment** — no special mention syntax
needed. Comments don't trigger anything on their own; only labels do.
After posting your reply:

1. Open the issue's **Labels** section
2. Remove `agent:product-manager`
3. Immediately re-add `agent:product-manager`

That relabel is what wakes the agent back up to read what you wrote.
Repeat for as many rounds as the interview takes.

If a run seems to be taking a while, check the **Actions** tab for
current status before assuming something's wrong.

---

## 9. What "done" with kickoff looks like

Once scope is fully defined and confirmed, the Product Manager closes
the kickoff issue and opens a new one titled **"RTVM"**, labeled
`agent:systems-engineer` — expected behavior, not an error. That's
where requirements decomposition begins, and it proceeds on its own
from there.

## Default Task for the Human Client: UE host project (added 2026-08-27)

For any Unreal Engine plugin project, BEFORE the Generate Code Base
issue runs: confirm a barebones UE host project exists for build and
test staging, or copy `resources/ue-host-project/` into the staging
location (its README has the two commands). Agents must never spend
budget regenerating this boilerplate from scratch — the resource is
text-only, engine-pinned, and proven against UE 5.6 headless runs.
If the client prefers to generate it themselves in the editor, that
is equally fine; what matters is it exists before code generation.
