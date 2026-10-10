# Item Organizer

Item Organizer is a web application for keeping track of your belongings and
the containers where you store them. Instead of remembering which box contains
an item, you can record your containers, add items manually or identify them
from photos, and search your inventory later.

## What you can do

- Create containers with names and locations, such as "Tool box" in "Garage".
- Add, edit, search, and delete inventory items, and assign or move them between
  containers.
- Upload a photo and ask AI to suggest item names, categories, quantities, and
  container assignments.
- Review the suggestions, correct details and bounding boxes, and reject false
  detections before confirming items into your inventory.
- See private photo crops next to confirmed items that have reviewed bounding
  boxes.
- Use the REST API to access inventory from other tools.

For example, create a "Camping gear" container, upload a photo of its contents,
review the detected items, and confirm their assignments. Later, search for
"flashlight" to find its recorded container.

## Choose how to run it

The local application runs in Docker on x64 or ARM64 machines. Open
`http://localhost:5173` after starting it with one of these scripts:

| Command | Photo analysis behavior |
|---|---|
| `.\scripts\start-environment.ps1` | Uses predictable mock results by default, for trying the workflow without paid AI calls. It does not identify the actual objects in your photo. |
| `.\scripts\start-live-ai.ps1` | Uses the configured Azure OpenAI deployment to analyze the actual photo. Requires Azure access and incurs usage charges. |

AI suggestions require your review; do not treat them as guaranteed accurate.
See [Live AI photo analysis](#live-ai-photo-analysis) for the live setup.
Project progress, milestone completion, validation history, and the
recognition-quality acceptance gate are tracked in [PLAN.md](PLAN.md).

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
Code Dev Containers and Playwright Test extensions. It is safe to run more
than once. Restart Windows when requested, complete any Docker Desktop
first-run prompts, and wait until Docker Desktop reports that its Linux engine
is running. The Playwright package and browser binaries remain project-scoped
inside the Development Container rather than being installed globally on the
Windows host.

Then run the environment bootstrap:

```powershell
.\scripts\start-environment.ps1
```

The script verifies Docker Desktop and Linux-container mode, creates `.env`
from `.env.example` when needed, builds and starts the Compose stack, waits for
healthy services, runs the environment smoke test, restores .NET dependencies,
applies pending database migrations, restores missing frontend dependencies,
and starts the API and Vite frontend. It never overwrites an existing `.env` or
`web/.env.local`. On the first frontend restore, it securely prompts for an
Azure DevOps PAT with Packaging Read permission.

Use `-NoBuild` to start existing images without rebuilding or
`-SkipSmokeTest` to omit validation:

```powershell
.\scripts\start-environment.ps1 -NoBuild -SkipSmokeTest
```

When startup completes, open `http://localhost:5173`. To prepare only the
containers and toolchain without restoring, migrating, or starting the
application, use `-SkipApplicationStart`. For unattended first-time frontend
restore, pass the protected PAT file with `-FrontendPatPath`:

```powershell
.\scripts\start-environment.ps1 -FrontendPatPath C:\secure\azure-devops-pat.txt
```

You can also open the repository in Visual Studio Code and reopen it in the
Development Container.

The equivalent environment-only manual sequence, matching
`-SkipApplicationStart`, is:

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

The API exposes `/api/v1` health, summary, container, photo, analysis, and
item reads. Resource reads require a delegated token with the
`ItemOrganizer.Read` scope, the configured tenant ID, and valid `tid` and `oid`
claims. Development OpenAPI is available at `/openapi/v1.json`.

Configure these settings through local environment variables or user secrets:

- `Authentication__Authority`
- `Authentication__Audience`
- `Authentication__AllowedTenantId`

Photo endpoints provide `POST /api/v1/photos` multipart uploads, authorized
short-lived photo content URLs, and retryable `DELETE /api/v1/photos/{photoId}`
deletion.
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
the configured provider, and persists detection drafts transactionally.
Detections become inventory items only after explicit review and confirmation.
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

Start the frontend from the Development Container:

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

Run the complete local browser workflow from the Development Container:

```powershell
docker compose exec -T workspace pnpm -C web test:e2e
```

The command installs the matching Chromium runtime and Linux dependencies,
resets and seeds PostgreSQL, starts the development-authenticated API and Vite
frontend, and runs the Playwright upload, polling, review, assignment, and
container-targeted convenience workflows. Failure traces, screenshots, and
the HTML report are ignored by Git.

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
acceptance or rejection, explicit reassignment, editable normalized bounding
boxes, and confirmed item-crop display in inventory. Confirmed crops remain
private; `GET /api/v1/items/{itemId}/crop` returns short-lived authorized
access, while the web client uses the authorized streamed-content mode.

Create a migration after changing the persistence model:

```powershell
docker compose exec -T workspace dotnet ef migrations add MigrationName --project src/ItemOrganizer.Infrastructure --startup-project src/ItemOrganizer.Database --output-dir Migrations
```

## Live AI photo analysis

To invoke the live provider in an explicitly selected development environment,
set `Analysis__Provider=azure-openai`, set `Analysis__Model` to the Azure
OpenAI deployment name, and set `AzureOpenAI__Endpoint` to the resource HTTPS
endpoint. Authentication uses `DefaultAzureCredential`; an
`AzureOpenAI__ApiKey` may be supplied only through an approved local secret
source. The default configuration remains deterministic and makes no live AI
calls.

The development deployment is `gpt-5.4-mini-itemorganizer-dev`. Live analysis
sends your uploaded photo to Azure OpenAI and uses strict structured output for
the detection suggestions. Azure usage is billed to the configured subscription.

Run the complete local application against that live deployment:

```powershell
.\scripts\start-live-ai.ps1
```

The script starts the Docker Compose development services, verifies the current
Azure CLI login and model deployment, checks that frontend packages are
installed, applies database migrations, and starts the development-authenticated
API and frontend. Open `http://localhost:5173`, choose **Photo analysis**,
select a JPEG, PNG, or WebP photo between 512 and 8000 pixels and no larger
than 10 MiB, then select **Upload and analyze**. The page polls until the live
analysis completes and displays detected names, categories, quantities,
confidence values, warnings, and container suggestions. Review and correct the
detections, adjust bounding boxes when needed, choose container assignments,
and explicitly confirm accepted items to add them to inventory. Press `Ctrl+C`
in the script terminal to stop both development servers.

To check prerequisites without starting the servers:

```powershell
.\scripts\start-live-ai.ps1 -ValidateOnly
```
