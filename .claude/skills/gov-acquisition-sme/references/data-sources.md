# Public data sources for an offline acquisition knowledge base

All U.S. Government works are free of copyright (17 U.S.C. 105); GSA
datasets on data.gov are CC0. api.data.gov keys default to 1,000 requests
per hour; SAM.gov keys are role-based per day (10 for a bare non-federal
key, 1,000 with a role, 10,000 for a federal system account). Pull once,
vendor, version.

| Source | Endpoint or file | Key fields for this project | Access |
| --- | --- | --- | --- |
| SAM.gov Opportunities v2 | `https://api.sam.gov/opportunities/v2/search`; description at `/prod/opportunities/v1/noticedesc?noticeid=` | `type`/`baseType` (ptype: r sources sought, p presolicitation, o solicitation, k combined, s special notice, a award, u J&A, i intent to bundle), `solicitationNumber`, `fullParentPathCode` (office), `naicsCode`, `classificationCode` (PSC), `typeOfSetAside`, `placeOfPerformance`, `award.number/amount/awardee.ueiSAM`, `resourceLinks[]` (attachment downloads) | api_key; `postedFrom`/`postedTo` required, one-year window; limit 1,000 |
| SAM.gov Data Services | `ContractOpportunitiesFullCSV.csv` and per-FY archives | notice metadata and description, no attachments | no key; JS page, direct file URLs |
| SAM.gov Contract Awards v1 | `https://api.sam.gov/contract-awards/v1/search` | `piid`, `referencedIdvPiid`, `contractingOfficeCode` (DoDAAC), PSC, NAICS, `awardOrIDV`, dollars, dates, awardee UEI/CAGE, size determination | api_key; DoD awards masked 90 days for non-federal; async CSV to 1M records |
| SAM.gov Entity v3/v4 | `https://api.sam.gov/entity-information/v4/entities` | UEI, CAGE, business types, NAICS and PSC assertions with small-business flags | api_key; async extract to 1M records |
| SAM.gov PSC API | `/prod/locationservices/v1/api/publicpscdetails` | PSC code, name, parent, active dates, Level 1 and 2 category | api_key; 10 per day non-federal |
| SAM.gov Federal Hierarchy | `/prod/federalorganizations/v1/orgs` | department and sub-tier only; office level not public | api_key |
| USAspending API | `GET /api/v2/awards/{id}/`; `POST /api/v2/idvs/awards/`; `POST /api/v2/search/spending_by_award/`; `/api/v2/references/award_types/` | `parent_award` (IDV type, PIID), `product_or_service_code`, `naics`, `type_set_aside`, `idv_type_description`, awarding office name, `last_date_to_order`, child orders with descriptions and obligations; award type codes A to D and IDV_A to IDV_E | no key |
| USAspending bulk | `https://files.usaspending.gov/award_data_archive/` monthly `FY{yyyy}_All_Contracts_Full_{yyyymmdd}.zip` (about 1.2 GB per FY); full PostgreSQL dump over 1.5 TB | FPDS-derived transaction columns including `parent_award_id_piid`, `awarding_office_code`, PSC, NAICS, set-aside, `award_description` | no key |
| GSA eLibrary | data.gov "GSA eLibrary Schedules and Contracts" XLSX (CC0, 2021) or eLibrary pages | MAS and GWAC holders by SIN | no key |
| GSA CALC+ | `https://api.gsa.gov/acquisition/calc/v3/api/ceilingrates/` | labor category, price, `idv_piid`, SIN, vendor, education, experience, clearance, worksite, size | no key |
| GSA OASIS+ workbook | buy.gsa.gov `OASIS PLUS Domain NAICS Codes and PSCs 9-15-22.xlsx` | domain to NAICS and PSC sets | direct file |
| GWCM PSC taxonomy guide | ag-dashboard.acquisitiongateway.gov PDF (Mar 2018) | PSC to 19-category mapping rules (lookup, PSC-by-NAICS overrides, keyword regexes) | direct file |
| PSC Manual | acquisition.gov `PSC April 2025.xlsx` | codes, names, start and end dates | direct file |
| NAICS 2022 | census.gov `2022_NAICS_Structure.xlsx`, `6-digit_2022_Codes.xlsx` | hierarchy 2 to 6 digits | direct file (landing page blocks bots) |
| DFARS PGI 237.102-74 taxonomy | `Acquisition_services_taxonomy.xlsx` on acq.osd.mil | PSC to DoD portfolio group | 503 at research time; retry |
| Navy LRAFs | ONR page (reachable); NAWCAD and others as SAM.gov special notices (`ptype=s`) or .mil PDFs (blocked to bots) | title, description, NAICS, value, anticipated vehicle, set-aside, office | browser download |
| Federal Register | `https://www.federalregister.gov/api/v1/documents.json` | rulemaking documents | no key |
| GovInfo | `https://api.govinfo.gov/` collections CHRG, GAOREPORTS, CFR, FR | Congressional hearings text and PDF, GAO reports | api_key |
| DLA DAASINQ | `home.daas.dla.mil/daashome/daasinq.asp` | DoDAAC lookup one at a time; bulk needs CAC | browser |

## Practical notes

- Contracting-office names: derive from distinct `awarding_office_code`
  and name pairs in the USAspending archive, since the public hierarchy API
  stops at sub-tier.
- Attachments: SAM.gov `resourceLinks[]` needs the api_key appended to each
  URL. GSA's Solicitation Review Tool pulls attachment ZIPs through the
  authenticated Opportunity Management API instead.
- FPDS `description` is capped at 250 characters since 2019 and is the
  only free text in FPDS. SAM.gov award notices carry longer titles and
  descriptions.
- Task-order solicitations for MACs often live only inside the vehicle
  portal (SeaPort, eBuy, CHESS IT e-mart), so absence from SAM.gov does
  not mean absence of competition.
- FPDS elements worth knowing by number: 1C Referenced IDV PIID, 6M
  Description of Requirement, 4B Contracting Office ID, 6D Type of IDC, 6E
  Multiple or Single Award, 6P Program Acronym (carries vehicle family
  names), 8P Consolidated Contract, 10N Type of Set Aside, 10R Fair
  Opportunity/Limited Sources, 1E Solicitation ID (joins to SAM.gov).
