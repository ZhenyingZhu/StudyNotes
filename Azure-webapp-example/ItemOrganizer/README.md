# Item Organizer

This repository implements the planning and development-environment outputs for
Milestones 0 and 1, the domain and persistence foundation for Milestone 2, the
secure read API and authorization requirements for Milestone 3, the manual
inventory workflows for Milestone 4, and secure photo ingestion and storage for
Milestone 5. It also implements the deterministic asynchronous analysis
pipeline for Milestone 6 of the accepted `PLAN.md`.

Milestone 0 is captured in `docs/milestone-0-decisions.md`. It resolves the product, security, workflow, retention, authorization, queue/worker, and acceptance-criteria decisions that were intentionally left open during planning.

Milestone 1 establishes a reproducible local development environment based on a VS Code Development Container and Docker Compose. The stack uses PostgreSQL so the complete local environment can run on both x64 and ARM64 hosts.

## Repository layout

- `PLAN.md` - accepted milestone plan
- `docs/milestone-0-decisions.md` - approved product and security decisions
- `.devcontainer/` - development container definition and Dockerfile
- `docker-compose.yml` - local development services
- `.env.example` - local environment template
- `scripts/install-prerequisites.ps1` - Windows host prerequisite installer
- `scripts/start-environment.ps1` - Windows environment bootstrap
- `scripts/reset-and-seed.ps1` - deterministic local database reset and seed
- `scripts/smoke-test.sh` - environment smoke test for use inside the workspace container
- `src/ItemOrganizer.Domain/` - inventory domain model and state transitions
- `src/ItemOrganizer.Infrastructure/` - EF Core PostgreSQL mappings and migrations
- `src/ItemOrganizer.Database/` - migration and seed command-line entry point
- `src/ItemOrganizer.Api/` - versioned read/write API, authorization, health, and OpenAPI endpoints
- `web/` - React and TypeScript inventory web application
- `tests/` - domain and PostgreSQL integration tests

## Host prerequisites

The only required host tools are:

- WSL 2
- Docker Desktop configured for Linux containers
- Visual Studio Code with the Dev Containers extension
- Git

No project SDKs need to be installed directly on the host.

The workspace, PostgreSQL, and Azurite images support x64 and ARM64.

## Quick start

On Windows, install or update the host prerequisites from PowerShell running
as Administrator:

```powershell
.\scripts\install-prerequisites.ps1
```

The installer enables WSL 2 without installing a separate Linux distribution,
then uses WinGet to install Git, Visual Studio Code, Docker Desktop, and the VS
Code Dev Containers extension. It is safe to run more than once. Restart
Windows when requested, complete any Docker Desktop first-run prompts, and wait
until Docker Desktop reports that its Linux engine is running.

Then run the environment bootstrap:

```powershell
.\scripts\start-environment.ps1
```

The script verifies Docker Desktop and Linux-container mode, creates `.env`
from `.env.example` when needed, builds and starts the Compose stack, waits for
healthy services, and runs the environment smoke test. It never overwrites an
existing `.env`.

Use `-NoBuild` to start existing images without rebuilding or
`-SkipSmokeTest` to omit validation:

```powershell
.\scripts\start-environment.ps1 -NoBuild -SkipSmokeTest
```

Then open the repository in Visual Studio Code and reopen it in the Development
Container.

The equivalent manual sequence is:

```powershell
Copy-Item .env.example .env
docker compose config --quiet
docker compose up -d --build --wait
docker compose exec -T workspace bash scripts/smoke-test.sh
docker compose ps
```

The bootstrap downloads the pinned EF Core CLI package using Windows
certificate validation and passes it through an ignored, temporary build
context file. A throwaway Docker stage supplies the package only while the CLI
is installed, so the package is not retained in the final image. This avoids
certificate-chain failures from NuGet's CDN without disabling TLS verification.

If an organization intercepts other HTTPS traffic during the image build, pass
its trusted PEM certificate to BuildKit without copying it into the repository:

```powershell
.\scripts\start-environment.ps1 -CustomCaPath C:\path\to\organization-ca.crt
```

The source file is mounted only while the image is built and is not copied into
the repository or build context. The certificate is installed in the image's
trust store so later HTTPS requests can use it.

## Local services

`docker-compose.yml` starts these services:

- `workspace` - the Development Container used for builds, tests, and local development
- `postgres` - local PostgreSQL matching the production database engine
- `azurite` - local Blob, Queue, and Table Storage emulator

Published host ports:

- PostgreSQL: `54320 -> 5432`
- Azurite Blob: `10000 -> 10000`
- Azurite Queue: `10001 -> 10001`
- Azurite Table: `10002 -> 10002`

Container-to-container traffic must use the Compose service names `postgres` and `azurite`, not `127.0.0.1`.

## Data persistence

The environment uses named Docker volumes:

- `postgres-data`
- `azurite-data`
- `nuget-cache`
- `pnpm-store`

The PostgreSQL and Azurite volumes preserve local state across container recreation. The cache volumes speed up repeated package restores without depending on host-specific paths.

## Smoke-test expectations

The smoke test passes when:

- the pinned toolchain is available in `workspace`
- `postgres` is reachable by service name on port `5432`
- `azurite` is reachable by service name on ports `10000`, `10001`, and `10002`
- the committed connection strings use `postgres` and `azurite` instead of `127.0.0.1`

## Database development

Apply pending EF Core migrations:

```powershell
docker compose exec -T workspace dotnet run --project src/ItemOrganizer.Database -- migrate
```

Reset the local database, apply all migrations, and load deterministic
representative data:

```powershell
.\scripts\reset-and-seed.ps1
```

Query the local PostgreSQL database interactively:

```powershell
docker compose exec postgres psql -U itemorganizer -d itemorganizer
```

Useful commands within `psql` include:

```sql
\dt
\d containers
SELECT * FROM containers;
SELECT * FROM photos;
SELECT * FROM analyses;
SELECT * FROM items;
SELECT * FROM item_assignments;
\q
```

Run a single query directly from PowerShell:

```powershell
docker compose exec -T postgres psql -U itemorganizer -d itemorganizer -c "SELECT name, location FROM containers;"
```

Run the backend unit, PostgreSQL integration, and API contract tests:

```powershell
docker compose exec -T workspace dotnet test ItemOrganizer.sln
```

Run the API locally after configuring the database and Entra settings:

```powershell
docker compose exec -T workspace dotnet run --project src/ItemOrganizer.Api
```

Milestone 3 exposes `/api/v1` health, summary, container, photo, analysis, and
item reads. Resource reads require a delegated token with the
`ItemOrganizer.Read` scope, the configured tenant ID, and valid `tid` and `oid`
claims. Development OpenAPI is available at `/openapi/v1.json`.

Configure these settings through local environment variables or user secrets:

- `Authentication__Authority`
- `Authentication__Audience`
- `Authentication__AllowedTenantId`

Milestone 5 adds `POST /api/v1/photos` multipart uploads, authorized short-lived
photo content URLs, and retryable `DELETE /api/v1/photos/{photoId}` deletion.
Uploads accept one `file` part in JPEG, PNG, or WebP format, enforce the 10 MiB
and 512-8000 pixel limits, reject animated or malformed images, and support an
optional `Idempotency-Key` header. Blob names are server-generated and the
storage container is created without public access.

Photo storage behavior can be configured with:

- `PhotoStorage__ContainerName` - private blob container name
- `PhotoStorage__ReadUrlMinutes` - read URL lifetime, clamped to 1-15 minutes
- `PhotoStorage__MaximumPhotosPerOwner` - active-photo upload quota
- `PhotoStorage__CleanupEnabled` - enable expired/pending photo cleanup
- `PhotoStorage__CleanupIntervalMinutes` - cleanup cadence, clamped to 1-15 minutes

Start an analysis with a token containing the `ItemOrganizer.Analyze` scope:

```http
POST /api/v1/photos/{photoId}/analyses
Idempotency-Key: optional-owner-scoped-key
```

The API atomically creates the queued analysis and SQL outbox record, then
returns `202 Accepted` with `Location` and `ETag` headers. The background
pipeline dispatches the analysis ID to Azure Storage Queue, processes it with
the deterministic mock provider, and persists detected items transactionally.
Poll `GET /api/v1/analyses/{analysisId}` with `ItemOrganizer.Read`. Cancel with
`POST /api/v1/analyses/{analysisId}/cancel`, `ItemOrganizer.Analyze`, and the
current `If-Match` value.

Analysis pipeline settings:

- `Analysis__WorkerEnabled` - enable outbox dispatch and queue processing
- `Analysis__PollingSeconds` - idle polling interval, clamped to 1-30 seconds
- `Analysis__QueueName` - Azure Storage Queue containing analysis IDs
- `Analysis__DeadLetterQueueName` - queue for permanently failed work
- `Analysis__PromptVersion` - application-owned prompt version
- `Analysis__SchemaVersion` - required structured-output schema version
- `Analysis__Model` - provider/model identifier stored with each analysis

Start the Milestone 4 frontend from the Development Container:

```powershell
Copy-Item .env.example .env
.\scripts\restore-frontend.ps1
docker compose exec workspace pnpm -C web dev
```

The restore script securely prompts for an Azure DevOps PAT with Packaging Read
permission. It sends the credential to the running Development Container over
standard input, temporarily adds feed authentication to the container user's
`.npmrc`, restores packages, and restores or removes that file before exiting.
The PAT is not passed as a process argument or written into the repository.
For unattended local use with an existing protected PAT file, pass its path
with `-PatPath`; the file contents are not displayed:

```powershell
.\scripts\restore-frontend.ps1 -PatPath C:\secure\azure-devops-pat.txt
```

The committed `web/.npmrc` reads the registry from
`ITEMORGANIZER_NPM_REGISTRY`. `.env.example` selects the approved Azure DevOps
`One_PublicPackages` feed by default, TLS verification remains enabled, and
there is no public registry fallback. Registry credentials, when required,
must be supplied through the approved local or organizational package-manager
configuration and must not be committed.

Copy `web/.env.example` to `web/.env.local` and configure the API URL, Entra
client and tenant IDs, and the delegated API scope before signing in. The web
application supports container and manual-item creation, editing, deletion,
inventory search, assignment, unassignment, and conflict feedback. It also
supports photo preview and upload, standard or container-targeted analysis,
automatic status polling, cancellation, detected-item review, suggestion
acceptance or rejection, and explicit reassignment.

Create a migration after changing the persistence model:

```powershell
docker compose exec -T workspace dotnet ef migrations add MigrationName --project src/ItemOrganizer.Infrastructure --startup-project src/ItemOrganizer.Database --output-dir Migrations
```

## Milestone 1 validation status

Validation performed on September 16, 2026 confirmed:

- the Development Container image builds on Windows ARM64
- the pinned workspace tool versions are available
- PostgreSQL and Azurite become healthy and are reachable by service name
- the complete environment smoke test passes on Windows ARM64
- Compose configuration and local-secret ignore rules are valid

Milestone 1 remains incomplete until a second supported machine reproduces the
environment from a clean checkout.

## Implementation status

Milestone status follows the exit criteria in `PLAN.md`. `Code-complete` means
the planned code and isolated automated tests exist. A milestone is called
`implemented`, `complete`, or `done` only after its required end-to-end
validation has passed. A blocked or unrun integration gate keeps the milestone
incomplete.

- Milestone 2 provides the approved schema, state transitions, ownership
  constraints, optimistic concurrency, transactional analysis-result
  persistence, and deterministic seed data.
- Milestone 3 is implemented. Its read routes, authorization, ownership
  isolation, paging, headers, health endpoints, and Problem Details behavior
  are covered by the passing API contract tests.
- Milestone 4 is complete. The API and web application support
  container and manual-item creation, editing, deletion, inventory search,
  assignment, unassignment, optimistic concurrency, and conflict feedback.
  Frontend component coverage includes container creation, inventory search,
  item editing, assignment, conflict feedback, and stale-record feedback.
- Milestone 5 is complete. Photo APIs provide bounded format and dimension
  validation, private blob storage, idempotent upload replay, per-owner quota
  enforcement, short-lived authorized reads, retryable deletion, and retention
  cleanup.
- Milestone 6 is complete. Analysis creation uses owner-scoped idempotency
  and a transactional SQL outbox. A hosted dispatcher and worker use Azure
  Storage Queue, deterministic structured results, bounded retries,
  dead-letter handling, cancellation checks, and atomic item persistence.
  Its complete HTTP photo-to-analysis flow passed against PostgreSQL and
  Azurite on September 29, 2026.
- Milestone 7 is in progress. The convenience upload-and-analyze API, photo
  analysis frontend, polling, cancellation, detected-item review, and explicit
  suggestion decisions are implemented and covered by API and component
  tests. A real PostgreSQL/Azurite convenience upload completed through the
  queue worker and confirmed both detected items in the selected container.
  The milestone remains incomplete because the required Playwright
  upload-through-review workflow has not been configured or run.
- Milestone 1 still requires clean-checkout validation on a second supported
  machine before its exit criteria are formally complete.

Validation on September 27, 2026 passed the Development Container smoke test,
9 domain tests, 7 PostgreSQL/Azurite integration tests, 26 API tests, 6
frontend component tests, and the frontend type-check and production build.

Validation completed on September 29, 2026:

- 9 domain tests, 41 API/analysis-pipeline tests, and 11 frontend component
  tests passed.
- All 8 PostgreSQL/Azurite integration tests passed, including private Blob
  Storage and Azure Queue delivery/dead-letter behavior.
- A real HTTP upload started an analysis through the SQL outbox and Azurite
  queue, reached `completed`, and persisted two detected items atomically.
- A real convenience upload reached `completed` and persisted two confirmed
  assignments in the explicitly selected container.
- The complete .NET solution tests and frontend type-check and production
  build passed.

The next gate is the Milestone 7 Playwright workflow from photo upload through
analysis polling, review, and assignment.
