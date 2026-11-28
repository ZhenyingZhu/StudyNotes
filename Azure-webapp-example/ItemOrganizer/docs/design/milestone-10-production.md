# Milestone 10 - Infrastructure, CI/CD, and production readiness

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Provision and release the approved system safely and repeatably.

## Requirements

- Bicep validation and what-if complete successfully for each environment without unintended resource changes.
- CI restores, lints, builds, tests, validates OpenAPI and Bicep, and publishes versioned artifacts.
- Workload identity federation deploys without long-lived deployment secrets.
- Database migrations run as a controlled release step and are backward-compatible with the deployment sequence.
- Search deployment provisions the approved PostgreSQL vector extension and
  embedding configuration, runs resumable backfill/reindex work, and verifies
  feature-flag rollback to keyword search without losing inventory records.
- The API passes staging health, contract, dependency, upload, authorization, and smoke tests before promotion.
- The frontend is deployed with environment-specific API and Entra settings and passes Playwright smoke tests.
- Rollback, failed migration, failed slot swap, dependency outage, and graceful shutdown procedures are tested and documented.
- Managed-runtime configuration, Swagger exposure, backup validation, data retention, and alert thresholds are finalized.

## Deployment approach

Azure resources are provisioned with Bicep and separated by environment. The deployment includes Azure Static Web Apps Standard, Azure App Service, Azure Database for PostgreSQL Flexible Server, private Blob Storage, Azure OpenAI or a reference to an approved deployment, Application Insights, Log Analytics, Key Vault, managed identities, role assignments, health checks, CORS settings, and diagnostic settings.

The API uses managed identity for Azure resources wherever supported. Secrets, storage keys, database passwords, and AI keys must not be committed or placed directly in ordinary deployment configuration. Entra application registrations, scopes, consent, and service principals are handled through controlled Microsoft Graph automation or documented administrative steps where Bicep cannot manage them.

The release pipeline uses GitHub Actions with workload identity federation:

1. Restore, lint, build, and test the backend and frontend.
2. Validate the OpenAPI document and Bicep templates.
3. Run a Bicep what-if operation.
4. Deploy or update infrastructure.
5. Produce and apply an EF Core migration bundle as a controlled release step.
6. Deploy the API to an App Service staging slot.
7. Run readiness, API, and dependency smoke tests.
8. Swap the validated staging slot into production.
9. Deploy the frontend with environment-specific API and Entra configuration.
10. Run post-deployment Playwright smoke tests.

The production API uses App Service's managed .NET runtime. Custom API-container deployment is outside this milestone's scope and requires a separate runtime/release design. Workspace SDKs and development services are never deployed as production artifacts.

App Service uses `/api/v1/health/ready` for health checks and enables Always On. Monitoring and alerts cover HTTP failures and latency, dependency health, failed analyses, and Azure OpenAI throttling. Production Swagger access, data retention, SQL backup validation, rate limits, and upload limits are release prerequisites.

## Search rollout and rollback

Provision the selected PostgreSQL extension and embedding configuration with
the search release. Run versioned, resumable indexing/backfill and validate
the same model/document space used by queries. Keep keyword search available
through a feature flag; rollback disables smart search without deleting
canonical inventory or requiring a database reset.

## Promotion and recovery

Apply backward-compatible migrations as a controlled release step rather
than an uncontrolled server-startup side effect. Verify staging health,
contracts, authorization, uploads and dependencies before swapping slots.
Failed validation prevents promotion.

Retain versioned artifacts and rehearse failed migration, failed slot swap,
dependency outage and graceful shutdown. Recovery procedures must distinguish
application rollback from database restoration and preserve inventory.

## Acceptance criteria

A staging-to-production rehearsal succeeds, all release gates pass, and an owner signs the production readiness checklist.
