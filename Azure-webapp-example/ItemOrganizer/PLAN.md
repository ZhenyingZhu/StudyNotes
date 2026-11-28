# Item Organizer - High-level plan

## Goal

Help users record what they own and where it is stored. Support manual
inventory and photo-assisted identification, require human review before AI
detections become inventory, and expose APIs for other tools.

## Architecture

- React/TypeScript frontend and ASP.NET Core API on .NET 10.
- PostgreSQL stores inventory, ownership, review drafts and operation state.
- Private Azure Blob Storage holds photos and confirmed crops; Storage Queue
  and a SQL outbox support asynchronous analysis.
- Azure OpenAI identifies items; deterministic mock AI is the local default.
- Microsoft Entra ID provides cloud authentication and per-user access.
- Production targets Azure Static Web Apps and App Service's managed runtime,
  with Bicep infrastructure and GitHub Actions delivery.

Development supports Windows and native Ubuntu using the same Linux
containers. Windows commands automatically invoke Docker Engine in WSL 2
Ubuntu; native Ubuntu runs Docker Engine directly. SDKs, builds, tests and
application servers remain inside the workspace container.

## Current status

The plan is accepted. Manual inventory, secure photo ingestion, asynchronous
analysis, explicit review and private crops have historical validation.
Keyword search exists; semantic search is not implemented.

The revised Windows/Ubuntu setup and tool-native external source configuration
design is **approved, not implemented or end-to-end validated**. Provisional
launcher/source changes are not proof of a working environment. Application
and launcher implementation remain paused during this documentation task.

Detailed specifications live in `docs/design`; dated progress, revisions
and validation evidence live in `docs/history`, not in the designs.

| Milestone | Outcome | Status | Detailed plan |
|---|---|---|---|
| 0 | Product, ownership and safety decisions | Approved and complete | [Product decisions](docs/design/milestone-0-decisions.md) |
| 1 | Reproducible development environment | Historically complete; revised workflow acceptance blocked | [Environment](docs/design/milestone-1-environment.md) |
| 2 | Domain model and persistence | Historically verified; reopened persistence scope | [Persistence](docs/design/milestone-2-persistence.md) |
| 3 | Read API and authorization | Implemented and verified | [Read API](docs/design/milestone-3-read-api.md) |
| 4 | Manual inventory and assignments | Complete and verified | [Inventory](docs/design/milestone-4-inventory.md) |
| 5 | Private photo ingestion and storage | Complete and verified | [Photos](docs/design/milestone-5-photos.md) |
| 6 | Deterministic asynchronous analysis | Reopened for revised structured-output/bounding-box gate | [Analysis pipeline](docs/design/milestone-6-analysis-pipeline.md) |
| 7 | Explicit review and private crops | Complete, including reopened scope; October 7 validation | [Review workflow](docs/design/milestone-7-review.md) |
| 8 | Measured live AI feasibility | Remaining gate waived, not validated | [Live AI](docs/design/milestone-8-live-ai.md) |
| 8A | Hybrid semantic inventory search | Planned; not implemented | [Semantic search](docs/design/milestone-8a-semantic-search.md) |
| 9 | Remaining Azure integrations and observability | Planned | [Azure integrations](docs/design/milestone-9-azure-integrations.md) |
| 10 | Infrastructure, CI/CD and production readiness | Planned | [Production](docs/design/milestone-10-production.md) |

## Shared designs and evidence

- [Confirmed Windows/Ubuntu setup and native source configuration](docs/design/development-environment.md):
  installer responsibilities, automatic Windows dispatch, external
  NuGet/npm configurations, credential-helper provisioning, lifecycle and
  command examples.
- [API contract](docs/design/api-contract.md): routes, representations,
  authorization, concurrency and errors.
- [Milestone status and revision records](docs/history/milestone-status.md).
- [Historical environment recipes](docs/history/environment-revisions.md).
- [Historical validation records](docs/history/validation-history.md):
  evidence at the time of each run, not validation of subsequent revisions.
- [Development guide](docs/DEVELOPMENT.md): current usage and diagnostics.
  Approved designs and current implementation may differ until validated.

## Delivery sequence and quality gates

Complete and validate the approved environment revision when implementation
resumes. Resolve Milestone 6's remaining gate. The next product capability is
Milestone 8A; complete its search gate before Milestones 9 and 10. Milestone
8's waived feasibility gate does not block later work.

- A milestone is complete only after its documented end-to-end exit criteria
  pass. Otherwise report code-complete, blocked or unvalidated work explicitly.
- Preserve owner/scope authorization, private storage, explicit review,
  atomic changes, safe errors, retry safety and accessibility.
- Keep credentials and sensitive photo/provider content out of source,
  ordinary logs and test artifacts.
- Update API contracts and relevant tests with behavioral changes. Keep
  deterministic local tests separate from explicitly invoked live Azure tests.
- Historical results and mocked regressions do not validate a revised runtime.
