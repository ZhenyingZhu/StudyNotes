# Item Organizer — Plan

## Status

**Accepted and implementation authorized. Milestones 0 and 1 are complete.
Milestone 1 is independently verified on Windows ARM64 and Windows AMD64.
Milestones 2 and 3 are implemented and verified. Milestones 4 and 5 are
complete and verified in the Development Container.**

## Goal

Plan an application that:

- Accepts photos.
- Allows containers to be entered.
- Uses AI to identify items in photos.
- Associates identified items with containers.
- Exposes web APIs for other tools.

## Approved technology stack

- **Frontend:** React with TypeScript
- **Backend/API:** ASP.NET Core Web API on .NET 10
- **Database:** Azure Database for PostgreSQL Flexible Server with Entity Framework Core and Npgsql
- **Photo storage:** Azure Blob Storage
- **AI:** Azure OpenAI with `gpt-5.4-mini` for vision-based item identification and structured outputs
- **Authentication:** Microsoft Entra ID
- **API contract:** REST with OpenAPI/Swagger
- **Hosting:** Azure Static Web Apps Standard for the frontend; Azure App Service for the API
- **Testing:** xUnit for the backend; Vitest and Playwright for the frontend
- **Infrastructure as code:** Bicep

## Local development environment

Local development will use a VS Code Development Container to avoid installing project SDKs and tools directly on the host.

The host environment must not be modified for project development or
validation. Do not install, uninstall, upgrade, or reconfigure project tooling
on the host to complete repository work. If the Development Container or
Docker services are unavailable, stop and restore those existing services or
report the blocked validation; do not fall back to changing the host.

The host requires only:

- Docker Desktop
- Visual Studio Code with the Dev Containers extension
- Git, when Git operations are performed on the host

### Why WSL 2 and Linux containers are used

Docker Desktop uses WSL 2 to provide the Linux kernel needed to run this project's Linux containers on Windows. Developers do not need to work directly inside a user-installed Linux distribution; Docker Desktop can manage its own internal WSL distributions. Enabling Docker Desktop integration with a separate Linux distribution is optional and is needed only when Docker commands will be run from that distribution.

Linux containers are the project default because the .NET SDK, Node.js, PostgreSQL, Azurite, Azure CLI, and Bicep support them well. They also provide a broad image ecosystem, generally use fewer resources than Windows containers, align with common GitHub Actions runners and possible Linux App Service deployment, and expose Linux-specific path, casing, permission, and shell issues during development. This is a development and testing choice; it does not require the production API to use a custom container. Production may still use either the App Service managed .NET runtime or a separate Linux production image.

### Windows 10 compatibility

WSL 2 is available on 64-bit Windows 10 version 2004 or later, build 19041 or later, when hardware virtualization is enabled. Use `winver` to check the installed version and build. Windows 10 reached the end of standard support on October 14, 2025, so a supported Windows 11 installation is preferred. A Windows 10 development machine should use an applicable Microsoft Extended Security Updates program and must satisfy the support requirements of the Docker Desktop version being installed.

### Install Docker Desktop on Windows

1. Confirm that hardware virtualization is enabled in UEFI/BIOS and that the Windows version supports WSL 2.
2. Open PowerShell as Administrator and run `wsl --install`. Restart Windows if prompted, then run `wsl --update` to install the latest WSL components.
3. Install Docker Desktop with `winget install --exact --id Docker.DockerDesktop`, or download the installer from the official Docker Desktop website when `winget` is unavailable.
4. Start Docker Desktop, accept its license terms, and complete its initial setup.
5. In Docker Desktop, open **Settings > General** and enable **Use the WSL 2 based engine**. Under **Settings > Resources > WSL Integration**, enable integration for the development Linux distribution if one will be used outside the Development Container.
6. Keep Docker Desktop configured for Linux containers, which are required by this project.
7. Open a new PowerShell window and verify the installation with `docker version` and `docker compose version`.
8. Run `docker run --rm hello-world` to confirm that Docker can pull and start a Linux container.

Docker Desktop must be running before opening the repository in its Development Container or starting the Docker Compose services. If installation is managed by an organization, its approved Docker Desktop licensing, sign-in, proxy, and resource-allocation policies take precedence.

### Install and run Azurite locally

Azurite will not be installed directly on Windows or inside the Development Container. It will run from Microsoft's official `mcr.microsoft.com/azure-storage/azurite` Linux container image as a Docker Compose service. The project configuration will pin the image to a tested version rather than relying on the `latest` tag.

The planned Compose service will:

- Start the Blob, Queue, and Table endpoints.
- Publish ports `10000`, `10001`, and `10002` to the Windows host for host-based diagnostics when needed.
- Listen on `0.0.0.0` inside its container so other Compose services can connect.
- Store emulator data in a named Docker volume so it survives container recreation.
- Use a service name such as `azurite`, which is also its hostname on the Compose network.

No separate Azurite installation command is required. After the project Docker Compose configuration is created, start its services from the Development Container or repository directory with `docker compose up -d`, verify them with `docker compose ps`, and inspect Azurite startup failures with `docker compose logs azurite`. Docker Desktop must already be installed and running.

The default host endpoints will be:

- Blob: `http://127.0.0.1:10000`
- Queue: `http://127.0.0.1:10001`
- Table: `http://127.0.0.1:10002`

Applications running directly on the Windows host may use `UseDevelopmentStorage=true`. Applications and tests running in the Development Container must not use `127.0.0.1` for Azurite because that address refers to the Development Container itself. They will use explicit development-storage endpoints whose hostname is the Compose service name, for example `azurite`, together with Azurite's well-known development account credentials. These emulator credentials are local-only and must never be reused for Azure resources.

The API will create required local blob containers and queues idempotently during development startup or test setup. Azurite is only an emulator; live Azure integration tests will continue to validate behavior that the emulator cannot reproduce exactly.

The development container will include pinned versions of:

- .NET 10 SDK
- Node.js and the frontend package manager
- EF Core CLI
- Azure CLI and Bicep CLI
- Git and required development utilities

Docker Compose will coordinate:

- The development container, with the repository mounted as its workspace
- PostgreSQL matching the production database engine
- Azurite for local Blob Storage emulation
- A queue emulator if an Azure Storage Queue-based analysis worker is selected

Named Docker volumes will be used where appropriate for NuGet packages, frontend packages, PostgreSQL data, and Azurite data. Builds, migrations, tests, debuggers, the API, and the frontend development server will run inside the development container. No .NET, Node.js, EF Core, PostgreSQL, Azurite, Azure CLI, or Bicep installation will be required on Windows.

Routine development will use a deterministic mock implementation of Azure OpenAI. Explicit integration tests may use a shared Azure development deployment and credentials supplied through environment variables or a local secret store; credentials must never be committed.

The development container is distinct from the production API image. It contains SDKs, debuggers, shells, and development tools and must not be deployed. If a production container is selected, it will use a separate multi-stage build containing only the published API and ASP.NET Core runtime, run as a non-root user, and be tested independently.

## Local testing

Local testing will use the following layers:

- **Unit tests:** xUnit tests that do not require Docker services and cover validation, authorization policies, assignment rules, analysis state transitions, and AI-result mapping.
- **API integration tests:** xUnit with `WebApplicationFactory`, a test authentication handler, PostgreSQL, and Azurite. Azure OpenAI responses will normally come from fixed structured-output fixtures.
- **Contract tests:** Verify routes, OpenAPI 3.1, status codes, headers, RFC 9457 Problem Details, paging, ETags, idempotency, and upload behavior.
- **Frontend tests:** Vitest for components and client logic; Playwright for end-to-end workflows against the local API.
- **Live Azure integration tests:** An optional, separately invoked suite for Entra ID, Azure OpenAI, Azure Database for PostgreSQL Flexible Server, and Blob Storage integration. It will not be part of the default local test run.
- **Production artifact tests:** CI will build and start the production API image, when container deployment is selected, and verify Linux compatibility, configuration, health checks, upload limits, non-root execution, dependency connectivity, and graceful shutdown.

The default local workflow is:

1. Open the repository in the Development Container.
2. Start PostgreSQL and Azurite through Docker Compose.
3. Apply EF Core migrations to a dedicated local database and load representative test data.
4. Run the API and frontend development server inside the Development Container.
5. Use Swagger UI and Playwright to exercise container creation, photo upload, analysis polling, item review, assignment, and deletion.
6. Test invalid files, missing scopes, stale ETags, retries, cancellation, conflicts, and unavailable dependencies.

Swagger UI is enabled only in the Development environment. Local integration tests use test authentication; manual Entra ID testing uses dedicated development app registrations and access tokens.

## Deployment approach

Azure resources will be provisioned with Bicep and separated by environment. The deployment will include Azure Static Web Apps Standard, Azure App Service, Azure Database for PostgreSQL Flexible Server, private Blob Storage, Azure OpenAI or a reference to an approved deployment, Application Insights, Log Analytics, Key Vault, managed identities, role assignments, health checks, CORS settings, and diagnostic settings.

The API will use managed identity for Azure resources wherever supported. Secrets, storage keys, database passwords, and AI keys must not be committed or placed directly in ordinary deployment configuration. Entra application registrations, scopes, consent, and service principals will be handled through controlled Microsoft Graph automation or documented administrative steps where Bicep cannot manage them.

The initial release pipeline will use GitHub Actions with workload identity federation and will:

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

Whether App Service uses the managed .NET runtime or a custom production container remains a deployment decision. If a custom container is selected, CI will publish the tested image to Azure Container Registry and the same immutable image will be promoted through environments. The Development Container will never be used as a deployment artifact.

App Service will use `/api/v1/health/ready` for health checks and enable Always On. Monitoring and alerts will cover HTTP failures and latency, dependency health, failed analyses, and Azure OpenAI throttling. Production Swagger access, data retention, SQL backup validation, rate limits, and upload limits must be finalized before release.

## Web API contract

### Conventions

- Base path: `/api/v1`
- Request and response format: JSON, except photo upload content
- Authentication: Microsoft Entra ID bearer access tokens
- Identifiers: server-generated UUIDs
- Dates and times: UTC in ISO 8601 format
- API definition: OpenAPI 3.1, exposed through Swagger UI in development
- Collection paging: `pageSize` and `continuationToken`
- Optional collection filtering: resource-specific query parameters
- Long-running AI analysis: asynchronous operation returning `202 Accepted`
- Successful creation: `201 Created` with a `Location` header
- Successful deletion: `204 No Content`
- Errors: RFC 9457 Problem Details (`application/problem+json`)
- Concurrent updates: `ETag` and `If-Match` headers to prevent lost changes
- Retried create and action requests: optional `Idempotency-Key` header

### System

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/health/live` | Confirm that the API process is running |
| `GET` | `/api/v1/health/ready` | Confirm access to required dependencies |
| `GET` | `/api/v1/summary` | Return counts of containers, photos, items, analyses in progress, and unassigned items |

### Containers

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/containers` | List containers; filter by `search` or `location` |
| `POST` | `/api/v1/containers` | Create a container |
| `GET` | `/api/v1/containers/{containerId}` | Get one container and its item count |
| `PATCH` | `/api/v1/containers/{containerId}` | Update selected container fields |
| `DELETE` | `/api/v1/containers/{containerId}` | Delete an empty container |
| `GET` | `/api/v1/containers/{containerId}/items` | List items assigned to a container |

Container creation fields:

- `name` — required display name
- `description` — optional description used by AI when suggesting assignments
- `location` — optional physical location
- `labels` — optional searchable labels

Deleting a non-empty container returns `409 Conflict`. Its items must first be moved or explicitly unassigned.

### Photos

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/photos` | List photos; filter by `status` or upload date |
| `POST` | `/api/v1/photos` | Upload one photo as `multipart/form-data` and create its record |
| `GET` | `/api/v1/photos/{photoId}` | Get photo metadata and processing status |
| `GET` | `/api/v1/photos/{photoId}/content` | Return a short-lived redirect or read URL for authorized viewing |
| `DELETE` | `/api/v1/photos/{photoId}` | Delete a photo when no analysis is running |

The upload request contains a required `file` part and an optional `containerId` hint. Accepted formats, maximum size, and image dimensions will be finalized with the validation requirements. Azure Blob Storage remains private; the API never returns a permanent public blob URL.

### AI analyses

| Method | Route | Purpose |
| --- | --- | --- |
| `POST` | `/api/v1/photos/{photoId}/analyses` | Start analysis of an uploaded photo |
| `POST` | `/api/v1/containers/{containerId}/photo-analyses` | Upload and analyze one photo with the specified container preselected for review |
| `GET` | `/api/v1/analyses/{analysisId}` | Poll analysis state and retrieve results when complete |
| `POST` | `/api/v1/analyses/{analysisId}/cancel` | Request cancellation of queued or running analysis |
| `POST` | `/api/v1/analyses/{analysisId}/confirm` | Confirm reviewed detections and atomically create canonical inventory items |

Starting analysis returns `202 Accepted`, a `Location` header for the analysis resource, and:

```json
{
	"id": "analysis UUID",
	"photoId": "photo UUID",
	"status": "queued",
	"createdAt": "UTC timestamp"
}
```

Analysis states are `queued`, `running`, `completed`, `failed`, and `cancelled`. A completed analysis contains persisted review drafts, not inventory item IDs. Drafts preserve the original AI prediction and remain excluded from inventory search, summaries, container counts, and assignment workflows until a user explicitly confirms them. A failed analysis includes a safe error code and message, but never provider credentials or raw internal exceptions.

`POST /api/v1/containers/{containerId}/photo-analyses` is a convenience workflow using `multipart/form-data` with a required `file` part. It validates the container, stores the photo, creates an analysis, and returns `202 Accepted` with the analysis resource in the same form as the standard analysis endpoint. The selected container is only a review default because the user has not yet seen or confirmed the detected identities. Analysis completion creates review drafts only.

During review, the user may correct a draft's name, description, category, quantity, and container; reject false positives; and leave valid items unassigned. `POST /api/v1/analyses/{analysisId}/confirm` accepts the complete review decision. In one transaction it creates inventory items and assignments only for accepted drafts, records rejected drafts without creating items, and marks the analysis review complete. Confirmation is idempotent and cannot partially promote a review. If analysis or confirmation fails, no unconfirmed detection appears in inventory.

Each AI detection may also include a proposed normalized bounding box:

```json
{
	"x": 0.18,
	"y": 0.32,
	"width": 0.41,
	"height": 0.12
}
```

`x` and `y` identify the top-left corner, and all four values are decimal fractions of the decoded source-photo width and height in the inclusive range `0` to `1`. `x + width` and `y + height` must not exceed `1`. The provider is instructed to return the smallest axis-aligned rectangle containing the visible portion of the item. Missing or uncertain localization is represented by `null`, not fabricated coordinates. Invalid, non-finite, zero-area, or out-of-bounds boxes reject the provider output before drafts are committed.

Bounding boxes are AI proposals rather than authoritative geometry. The review UI overlays each proposed box on the source photo and allows the user to move, resize, remove, or create a box before confirmation. The original predicted box and the reviewed box are stored separately. Only the reviewed box may be used to generate an item crop.

### Items and assignments

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/items` | Search inventory; filter by `search`, `category`, `containerId`, `photoId`, or assignment state |
| `POST` | `/api/v1/items` | Manually create an item not detected from a photo |
| `GET` | `/api/v1/items/{itemId}` | Get an item, its source photo, and assignment details |
| `GET` | `/api/v1/items/{itemId}/crop` | Return a short-lived authorized URL for the confirmed item crop, when present |
| `PATCH` | `/api/v1/items/{itemId}` | Correct an item's editable properties |
| `DELETE` | `/api/v1/items/{itemId}` | Remove an item from the catalog |
| `PUT` | `/api/v1/items/{itemId}/container` | Assign or move an item to a container |
| `DELETE` | `/api/v1/items/{itemId}/container` | Mark an item as unassigned |

An AI detection review draft contains:

- `name`
- `description`
- `category`
- `quantity`
- `confidence` from `0` to `1`
- `photoId`
- `analysisId`
- `suggestedContainerId`, when AI finds a likely match
- `predictedBoundingBox`, when the provider can localize the item
- `reviewedBoundingBox`, after the user accepts or adjusts the proposal
- `reviewStatus`: `pending`, `accepted`, or `rejected`
- `resultingItemId`, only after an accepted draft has been promoted

Only manually created items and explicitly accepted detection drafts are inventory items. AI suggestions never appear in `GET /api/v1/items`, summary counts, search results, or container contents before confirmation. Container assignment remains explicit; an accepted detection may be confirmed as unassigned.

When an accepted draft has a reviewed bounding box, confirmation crops the decoded source image using validated pixel coordinates, encodes the derivative in an approved non-animated format, and writes it to private Blob Storage under a server-generated name. PostgreSQL stores only crop metadata and the private blob reference, including source photo ID, reviewed normalized coordinates, pixel dimensions, content type, length, and hash. Crop creation and inventory promotion must have deterministic compensation behavior: a database failure removes the newly written crop, while a storage failure creates no item. Crop authorization, retention, and deletion follow the source item and photo ownership rules; deleting an item schedules its crop for deletion without silently deleting the source photo.

### Assignment request

```json
{
	"containerId": "container UUID",
	"acceptSuggestion": true
}
```

The assignment endpoint returns the updated item. Moving an item is atomic; no separate removal request is needed.

### Authorization policy

Initial delegated scopes:

- `ItemOrganizer.Read` — view containers, photos, analyses, and items
- `ItemOrganizer.Write` — upload photos and create or edit containers and items
- `ItemOrganizer.Analyze` — start or cancel AI analyses

The same application permissions may later be exposed for daemon tools. Tenant boundaries and ownership rules remain to be defined before implementation.

### Standard errors

Expected responses include:

- `400 Bad Request` — malformed request or invalid state transition
- `401 Unauthorized` — missing or invalid access token
- `403 Forbidden` — token lacks the required scope or resource access
- `404 Not Found` — resource does not exist or is not visible to the caller
- `409 Conflict` — duplicate, non-empty container deletion, or incompatible operation state
- `412 Precondition Failed` — stale `ETag`
- `413 Content Too Large` — photo exceeds the configured limit
- `415 Unsupported Media Type` — unsupported photo format
- `422 Unprocessable Content` — semantically invalid fields
- `429 Too Many Requests` — caller or AI quota exceeded; includes `Retry-After`
- `500 Internal Server Error` — unexpected server failure with a correlation ID
- `503 Service Unavailable` — required Azure dependency is unavailable

## Guidance to capture

The following decisions are intentionally open:

1. User experience and workflow
2. Azure OpenAI item-identification behavior
3. Container-association rules
4. Data model and storage details
5. Tenant boundaries, ownership, and detailed authorization rules
6. Photo validation and retention requirements
7. Managed-runtime versus custom-container production deployment
8. Detailed acceptance criteria and release thresholds
9. Durable queue and worker design for asynchronous AI analyses

## Milestones and testable outcomes

The milestones below are ordered so that each stage produces a demonstrable, testable result. A milestone is complete only when its outcome and exit criteria have been met; code, infrastructure, or configuration must not be created before the plan is explicitly accepted.

### Milestone 0 — Confirm product and security decisions

**Purpose:** Resolve the open decisions that affect the data model, API contract, user experience, and deployment design.

**Testable outcomes:**

- A written workflow describes photo upload, container creation, analysis polling, item review, assignment, correction, deletion, and retry behavior.
- The approved AI output schema defines required fields, confidence handling, malformed-output handling, prompt/version tracking, and provider failure behavior.
- Container-association rules distinguish explicit user confirmation from AI suggestions and define duplicate-item behavior.
- Tenant, ownership, delegated-scope, and resource-authorization rules are documented with allow and deny examples.
- Photo format, size, dimension, retention, deletion, and privacy requirements are documented.
- The queue/worker model, cancellation semantics, retry policy, idempotency behavior, and deployment mode are selected.
- Acceptance criteria are recorded for functional, security, performance, reliability, and accessibility requirements.

**Exit criteria:** Product owner and technical owner approve the decisions, including all items in the open-guidance list.

### Milestone 1 — Repository and development environment

**Purpose:** Establish a reproducible development environment without deploying development tooling to production.

**Testable outcomes:**

- The repository contains the reusable environment recipe, including a Development Container definition, a development Dockerfile, Docker Compose configuration, an example environment file, and ignore rules for local secrets and generated state.
- Development images and tools are pinned to tested versions rather than floating `latest` tags, including the .NET SDK, Node.js, PostgreSQL, Azurite, Azure CLI, Bicep, EF Core tooling, and package managers.
- A new developer can open the repository in the Development Container and obtain the pinned .NET, Node.js, Azure CLI, Bicep, Git, and EF Core tooling versions.
- Docker Compose starts the development container, PostgreSQL, and Azurite; service health and published ports are documented and verified.
- Named volumes preserve PostgreSQL and Azurite data across service recreation.
- Optional named cache volumes are defined for NuGet and frontend package caches without making builds depend on machine-specific paths.
- Applications and tests running in containers use Compose service names such as `postgres` and `azurite` instead of machine-specific addresses or `127.0.0.1` for container-to-container dependencies.
- Applications in the Development Container reach Azurite through the Compose service hostname rather than `127.0.0.1`.
- A documented command sequence starts all local dependencies and a clean checkout passes the environment smoke test.
- The committed environment files define the recipe only; real `.env` files, secrets, production credentials, database files, emulator data, and generated application code are not committed.
- The environment can be recreated on another supported machine from a clean clone using only Docker Desktop, Visual Studio Code with the Dev Containers extension, and Git.

**Exit criteria:** The environment smoke test passes on a clean checkout using only the documented host prerequisites, and a second supported machine can rebuild the Development Container, start Docker Compose services, verify tool versions, connect to PostgreSQL and Azurite by service name, and run the documented smoke-test command sequence without any machine-specific configuration.

**Current validation status (October 3, 2026):**

- The Development Container image builds successfully on Windows ARM64.
- The pinned .NET SDK, EF Core CLI, Git, Node.js, pnpm, Azure CLI, and Bicep versions were verified inside the workspace image.
- PostgreSQL starts healthy and is reachable through its Compose service name.
- Azurite starts healthy and is reachable through its Compose service name on the Blob, Queue, and Table ports.
- The complete environment smoke test passes on Windows ARM64.
- On October 3, 2026, a second Windows AMD64 machine removed ignored files,
  containers, PostgreSQL and Azurite data, and package-cache volumes before
  running the documented bootstrap from the committed clean worktree.
- The second machine rebuilt the Development Container, verified the pinned
  tools, started healthy PostgreSQL and Azurite services, reached both by
  Compose service name, passed the smoke test, restored dependencies, applied
  migrations, and started the API and frontend successfully.
- Host requests returned HTTP 200 from the frontend, API readiness endpoint,
  and authenticated API summary endpoint. Milestone 1 therefore satisfies its
  exit criteria and is complete.

### Milestone 2 — Domain model and persistence foundation

**Purpose:** Implement the approved inventory, photo, analysis-review, assignment, and idempotency persistence model.

**Testable outcomes:**

- EF Core migrations create the approved schema in a clean local PostgreSQL database.
- Constraints and indexes enforce identifier, ownership, assignment, status, uniqueness, and timestamp rules.
- Valid and invalid state transitions are covered by unit tests, including analysis cancellation, completion, failure, retry, and deletion conflicts.
- Transaction tests demonstrate atomic review-draft persistence at analysis completion and atomic inventory creation only after explicit confirmation.
- Concurrent update tests demonstrate stale `ETag` protection and prevent lost changes.
- A reset-and-seed procedure produces representative data deterministically.

**Exit criteria:** Persistence unit and integration tests pass against the local PostgreSQL service, including rollback and concurrency cases.

**Current implementation status (September 18, 2026):**

- The current domain model covers containers, photos, analyses, items, assignments, outbox messages, and idempotency records.
- The initial EF Core migration defines PostgreSQL ownership keys, foreign keys, checks, uniqueness rules, indexes, and optimistic concurrency tokens.
- Domain tests cover analysis cancellation, completion conflicts, retries, assignment transitions, and deletion conflicts.
- PostgreSQL integration tests cover migration creation, ownership enforcement, idempotency uniqueness, atomic result persistence, rollback, and stale-version rejection.
- The database command and PowerShell wrapper provide deterministic reset, migration, and representative seed data.
- **Plan revision (October 6, 2026):** analysis detection drafts are now
  persisted separately, existing unreviewed AI items are migrated back to
  pending review, and confirmation transaction tests pass. This milestone
  remains reopened for reviewed bounding-box and private crop metadata,
  constraints, lineage, and cleanup persistence.

### Milestone 3 — Read-only API and authorization

**Purpose:** Expose secure read operations and system health endpoints using the approved API conventions.

**Testable outcomes:**

- Health, summary, container, photo, analysis, and item read routes use the `/api/v1` base path and documented response shapes.
- OpenAPI 3.1 describes routes, schemas, security requirements, paging, `ETag`, and RFC 9457 Problem Details responses.
- Requests without a token receive `401`; requests without the required read scope receive `403`.
- A caller cannot read another tenant's or owner's resources under the approved authorization model.
- Collection paging, filtering, UTC timestamps, correlation IDs, and cache/concurrency headers behave as documented.
- Readiness reports dependency failure without exposing secrets or internal exception details.

**Exit criteria:** Contract tests pass for successful reads, authorization failures, not-found behavior, paging, headers, and Problem Details.

### Milestone 4 — Container, item, and assignment workflows

**Purpose:** Deliver the manually managed inventory workflow before introducing AI processing.

**Testable outcomes:**

- Authorized clients can create, list, update, and delete containers according to validation and empty-container rules.
- Authorized clients can create, list, update, and delete manually managed items.
- Items can be assigned, moved, or unassigned atomically through the documented endpoints.
- AI suggestions remain distinguishable from confirmed assignments and are not silently promoted.
- Invalid fields, missing resources, duplicate requests, stale `ETag`s, and non-empty-container deletion return the documented status and Problem Details responses.
- Frontend tests cover container creation, inventory search, item editing, assignment, and conflict messages.

**Exit criteria:** Backend unit, API contract, and frontend component tests pass for the complete non-AI workflow.

**Current implementation status (September 25, 2026):**

- Container, manual-item, and assignment mutation routes enforce ownership,
  delegated write scope, optimistic concurrency, validation, and conflict
  responses.
- Manual items now have explicit persistence support without fabricated photo,
  analysis, or confidence values.
- The React and TypeScript frontend covers container and item creation,
  editing, deletion, inventory search, assignment, unassignment, and conflict
  feedback.
- Domain and API contract tests pass, and PostgreSQL integration tests passed
  during the September 23 validation. Frontend component tests cover container
  creation, inventory search, item editing, assignment, container inventory
  views, conflict feedback, and stale-record feedback.
- Frontend dependency restoration must continue to use
  `ITEMORGANIZER_NPM_REGISTRY` with an approved internal registry; public npm
  access is intentionally not used.
- Validation on September 27, 2026 passed 9 domain tests, 7 PostgreSQL/Azurite
  integration tests, 26 API tests, 6 frontend component tests, the frontend
  type-check and production build, and the environment smoke test.

### Milestone 5 — Secure photo ingestion and storage

**Purpose:** Accept, validate, store, retrieve, and delete private photos safely.

**Testable outcomes:**

- Valid multipart uploads create photo metadata and private blobs, returning `201 Created` and a `Location` header.
- Unsupported media, oversized files, invalid dimensions, malformed multipart requests, and upload quota violations return the approved errors without persisting partial records.
- Blob names and metadata do not expose tenant identifiers, credentials, or user-controlled path traversal.
- Authorized content retrieval uses a short-lived protected read mechanism; unauthorized callers cannot read the blob.
- Confirmed item crops use separate server-generated private blobs, retain source-photo lineage and reviewed coordinates, and cannot be created from an unreviewed AI box.
- Crop encoding, hash, dimensions, cleanup, storage-failure compensation, and authorization are covered by storage tests.
- Photo deletion is blocked while analysis is running and is idempotent where the contract requires it.
- Upload, download, deletion, retention, and storage-failure tests pass against Azurite, with explicitly documented emulator limitations.

**Exit criteria:** API integration and security tests pass for accepted and rejected uploads, authorization, cleanup, retry, and deletion behavior.

**Current implementation status (September 25, 2026):**

- Multipart photo uploads validate the declared and decoded JPEG, PNG, or WebP
  format, the 10 MiB limit, 512-8000 pixel dimensions, malformed content, and
  multi-frame images before persistence.
- Uploads use server-generated blob names, private Blob Storage, per-owner
  quotas, and optional owner-scoped `Idempotency-Key` replay without persisting
  partial metadata after storage failure.
- Authorized content requests return short-lived read-only SAS URLs. Photo
  deletion blocks active analyses and referenced items, records pending
  deletion before touching storage, and safely retries failed or repeated
  deletion attempts.
- A background retention worker processes expired and pending deletions at a
  configurable interval of no more than 15 minutes.
- API and security tests pass against an in-memory storage test double. A
  dedicated integration test verifies private upload, SAS download, and
  idempotent deletion against Azurite.
- Validation on September 27, 2026 passed the complete backend,
  PostgreSQL/Azurite, frontend component, type-check, build, and environment
  smoke-test gates.

### Milestone 6 — Deterministic asynchronous analysis pipeline

**Purpose:** Process analyses asynchronously with a deterministic mock AI implementation before enabling live Azure OpenAI calls.

**Testable outcomes:**

- Starting an analysis returns `202 Accepted`, a `Location` header, and the documented queued resource.
- The worker transitions analyses only through approved states and records safe failure information, retry count, timestamps, and correlation data.
- Fixed structured-output fixtures produce deterministic review drafts, confidence values, suggestions, source links, and optional normalized bounding boxes.
- Bounding-box validation rejects non-finite values, non-positive areas, values outside `0` to `1`, and boxes whose right or bottom edge exceeds the image boundary.
- Invalid, incomplete, unsafe, or schema-incompatible AI output is rejected without creating partial inventory data.
- Retries are bounded and idempotent; duplicate delivery does not duplicate review drafts.
- Cancellation works for queued and eligible running work, and cancellation races have deterministic results.
- Polling returns the documented state and result shape, including failure and cancellation responses.

**Exit criteria:** Worker, transaction, retry, cancellation, fixture, and API integration tests pass without network access to Azure OpenAI.

**Current implementation status (September 28, 2026):**

- Analysis creation requires `ItemOrganizer.Analyze`, supports owner-scoped
  idempotency, and atomically persists the queued analysis and SQL outbox
  record before returning `202 Accepted`, `Location`, and `ETag`.
- A hosted dispatcher forwards analysis IDs to Azure Storage Queue. The worker
  processes deterministic structured results, validates schema and version
  metadata, persists items atomically, and handles cancellation before commit.
- Temporary failures use exponential retry through the sixth delivery attempt;
  permanent or exhausted failures use safe error details and dead-letter
  handling. Duplicate delivery does not recreate completed items.
- End-to-end validation on September 29, 2026 passed against the Docker Compose
  PostgreSQL and Azurite services. A real HTTP photo upload created a queued
  analysis and SQL outbox entry; the hosted dispatcher delivered it through
  Azure Storage Queue; the worker completed one delivery and atomically
  persisted two detected items with `suggested` and `unassigned` states.
- The analysis polling resource reached `completed`, PostgreSQL recorded the
  outbox as `dispatched`, the main queue drained, no dead-letter message was
  created, and all 8 PostgreSQL/Azurite integration tests passed. Milestone 6
  therefore satisfies its exit criteria and is complete.
- **Plan revision (October 6, 2026):** the pipeline now persists review drafts
  without creating inventory. This milestone remains reopened until the
  structured-output contract, deterministic fixtures, and validation cover
  optional normalized bounding boxes.

### Milestone 7 — AI-assisted review and convenience workflow

**Purpose:** Require explicit human confirmation before any AI detection becomes canonical inventory.

**Testable outcomes:**

- Completed analyses display pending review drafts with source photo, confidence, category, quantity, and container suggestions.
- Completed analyses overlay proposed bounding boxes on the source photo and remain usable when a provider returns no box.
- Users can correct draft fields, accept valid detections, reject false positives, choose or remove container assignments, and create, move, resize, or remove bounding boxes before confirmation.
- Pending and rejected detections never appear in inventory search, summaries, container counts, or item routes.
- One explicit confirmation operation creates inventory items and private crops only for accepted drafts and preserves original predictions plus reviewed values and geometry for evaluation.
- The convenience upload-and-analyze endpoint validates the container, stores the photo, queues analysis, and returns the same asynchronous contract.
- The convenience workflow preselects the requested container for review but does not bypass item confirmation.
- Retry, refresh, duplicate submission, stale `ETag`, invalid geometry, crop-storage failure, and partial dependency failure scenarios are covered by API and frontend tests.
- Playwright verifies upload, bounding-box review, rejection, correction, confirmation, crop display, and inventory persistence.

**Exit criteria:** The full local photo-to-review-to-inventory workflow passes with the deterministic mock AI, tests prove that no AI detection or crop enters canonical inventory before explicit confirmation, and confirmed crop lifecycle tests pass against Azurite.

**Current implementation status (October 1, 2026):**

- The web application uploads or captures JPEG, PNG, and WebP files, previews
  the selected source photo, starts standard or container-targeted analyses,
  polls queued and running work, displays failures and warnings, and permits
  cancellation.
- Completed analyses display detected names, categories, quantities,
  confidence, assignment status, and container suggestions. Users can accept
  or reject a suggestion or explicitly select a different container.
- `POST /api/v1/containers/{containerId}/photo-analyses` reuses the secure
  upload pipeline, validates owner-visible containers before storage, supports
  idempotent replay, creates the queued analysis and outbox record, and marks
  all completed detections as confirmed convenience-workflow assignments.
- API tests cover successful creation, invalid containers, duplicate
  submission, storage failure without analysis creation, and atomic confirmed
  result persistence. Frontend component tests cover standard upload,
  convenience upload, cancellation, and explicit suggestion acceptance and
  rejection.
- Live validation against PostgreSQL and Azurite completed a real convenience
  upload through the SQL outbox and queue worker, then returned two items
  confirmed in the selected container.
- Playwright Chromium validation resets the deterministic database, starts the
  development-authenticated API and Vite frontend, generates and uploads a
  valid 512-pixel PNG, polls through queue-worker completion, accepts an AI
  suggestion, explicitly assigns an unassigned item, refreshes the inventory,
  and verifies the container-targeted convenience workflow confirms every
  detected item.
- On October 1, 2026, both Playwright workflows, all 41 API tests, all 8
  PostgreSQL/Azurite integration tests, all 9 domain tests, all 11 frontend
  component tests, and the frontend production build passed. Milestone 7
  therefore satisfies its exit criteria and is complete.
- **Plan revision (October 6, 2026):** pending detections are now editable and
  only accepted detections are promoted to inventory through explicit
  confirmation.
- **Plan revision (October 6, 2026):** editable bounding-box review and
  confirmed derivative-crop storage were added to the milestone.
- **Completion validation (October 7, 2026):** the review UI overlays
  predicted boxes and permits numeric move, resize, creation, and removal;
  confirmation persists reviewed geometry and creates private PNG crops only
  for accepted detections. Crop upload or database failure creates no
  inventory, item deletion schedules crop cleanup, authorized inventory
  thumbnails load through the API, and manual items remain photo-free.
  All 10 domain tests, 48 API tests, 8 PostgreSQL/Azurite integration tests,
  13 frontend component tests, the frontend production build, and both
  Playwright Chromium workflows passed in the Docker Development Container.
  Milestone 7 therefore satisfies its reopened exit criteria and is complete.

### Milestone 8 — Live AI feasibility gate

**Purpose:** Prove that reliable automatic photo identification meets the mandatory product value proposition before further production-infrastructure investment.

**Testable outcomes:**

- A representative, versioned evaluation dataset covers ordinary inventory photos plus clutter, occlusion, poor lighting, visually similar objects, and multiple quantities.
- The live Azure OpenAI provider reads the private source photo, uses strict structured output, records model and prompt versions, and does not persist raw provider payloads.
- Evaluation reports item precision and recall, quantity accuracy, false detections, schema-valid response rate, bounding-box coverage, localization Intersection over Union against reviewed ground truth, user box-adjustment rate, latency percentiles, and estimated cost per analysis.
- Acceptance thresholds are documented before evaluation and distinguish model errors from workflow correction behavior.
- The model abstains through omitted or low-confidence detections rather than inventing items, and unsafe or malformed output creates no inventory records.
- The feasibility decision evaluates whether Azure OpenAI localization is sufficiently accurate for editable proposals. If localization misses the approved threshold, the plan must use a dedicated object-detection model for geometry while retaining Azure OpenAI only for labeling and descriptions.
- Prompt or model changes can be compared against the same dataset without changing the default deterministic test suite.

**Exit criteria:** Stakeholders approve measured acceptance thresholds, the live evaluation meets every mandatory threshold, and the result is recorded as an explicit go decision. Failure is a no-go requiring model, workflow, or product redesign before Milestone 9.

**Current implementation status (October 1, 2026):**

- The API includes a configurable Azure OpenAI Responses provider using strict
  structured output, private blob download, managed identity or an explicitly
  supplied development API key, and safe transient, rejected, and malformed
  output classifications.
- A development `gpt-5.4-mini` deployment was provisioned with GlobalStandard
  capacity and passed an Entra-authenticated Responses API test using real
  image input and strict JSON-schema output.
- The default remains the deterministic provider so ordinary local and CI
  suites do not consume Azure resources.
- The representative dataset, acceptance thresholds, live measurements, and
  stakeholder go/no-go decision are not yet complete. Milestone 8 is therefore
  in progress and must not be called complete.

### Milestone 9 — Entra ID, remaining live Azure integrations, and observability

**Purpose:** After AI feasibility passes, validate cloud identity, remaining managed-resource access, monitoring, and operational behavior.

**Testable outcomes:**

- Entra ID tokens with each approved scope produce the documented allow/deny behavior.
- The deployed API accesses Azure Database for PostgreSQL Flexible Server, private Blob Storage, and the proven Azure OpenAI deployment through managed identity or the approved secret mechanism; no credentials are present in source or ordinary configuration.
- Live integration tests are explicitly invoked, isolated from the default local test run, and clean up their test data.
- Application Insights and Log Analytics capture correlation IDs, dependency failures, analysis failures, throttling, latency, and authorization failures without recording photo content or secrets.
- Readiness and liveness checks, alerts, retention settings, rate limits, and upload limits are verified in a non-production environment.

**Exit criteria:** The live integration and observability checklist passes in a dedicated Azure development environment with evidence retained for review.

### Milestone 10 — Infrastructure, CI/CD, and production readiness

**Purpose:** Provision and release the approved system safely and repeatably.

**Testable outcomes:**

- Bicep validation and what-if complete successfully for each environment without unintended resource changes.
- CI restores, lints, builds, tests, validates OpenAPI and Bicep, and publishes versioned artifacts.
- Workload identity federation deploys without long-lived deployment secrets.
- Database migrations run as a controlled release step and are backward-compatible with the deployment sequence.
- The API passes staging health, contract, dependency, upload, authorization, and smoke tests before promotion.
- The frontend is deployed with environment-specific API and Entra settings and passes Playwright smoke tests.
- Rollback, failed migration, failed slot swap, dependency outage, and graceful shutdown procedures are tested and documented.
- Production decisions for managed runtime versus custom container, Swagger exposure, backup validation, data retention, and alert thresholds are approved.

**Exit criteria:** A staging-to-production rehearsal succeeds, all release gates pass, and an owner signs the production readiness checklist.

### Cross-milestone quality gates

Every milestone must preserve these requirements:

- A milestone may be described as `code-complete` when its planned code and
  automated isolated tests are present, but it must not be described as
  `implemented`, `complete`, or `done` until its documented exit criteria have
  passed through the required end-to-end environment. Any blocked or unrun
  integration gate must be stated explicitly and keeps the milestone
  incomplete.
- No secrets, tokens, private blob URLs, raw provider errors, or sensitive photo content appear in source control, API errors, logs, or test artifacts.
- Automated tests are repeatable and distinguish unit, local integration, contract, frontend, live Azure, and production-artifact suites.
- API changes update the OpenAPI contract and corresponding contract tests.
- Authorization is tested for both permitted and denied access, including cross-tenant or cross-owner cases where applicable.
- Failures leave no unintended partial state, and asynchronous operations are safe to retry.
- Accessibility, keyboard operation, responsive behavior, and useful error messages are included in frontend acceptance testing.

## Recorded validation history

These records describe the behavior at the time of each validation, not the
current review contract. The October 6 review revisions and October 7
completion validation are recorded under Milestone 7. Environment validation
on Windows ARM64 and the October 3 clean-checkout Windows AMD64 validation are
recorded under Milestone 1.

**September 16, 2026:**

- The Development Container image built on Windows ARM64, pinned workspace
  tool versions were available, and PostgreSQL and Azurite became healthy and
  were reachable by service name.
- The complete environment smoke test passed on Windows ARM64.
- Compose configuration and local-secret ignore rules were valid.

**September 27, 2026:**

- The Development Container smoke test, 9 domain tests, 7 PostgreSQL/Azurite
  integration tests, 26 API tests, 6 frontend component tests, and the frontend
  type-check and production build passed.

**September 29, 2026:**

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

**October 1, 2026:**

- All 9 domain, 8 PostgreSQL/Azurite integration, 41 API/pipeline, and 11
  frontend component tests passed.
- The frontend type-check and production build passed.
- Both Playwright Chromium workflows passed against the real local frontend,
  API, PostgreSQL database, private Azurite Blob Storage, SQL outbox, Azurite
  Queue, and deterministic analysis worker.
- The standard workflow uploaded a generated valid PNG, reached completed
  analysis, accepted the suggested container for one item, explicitly assigned
  the unassigned item, refreshed, and retained both confirmed assignments.
- The convenience workflow completed with both detected items confirmed in the
  explicitly selected container.

## Next step

Complete Milestone 8 by defining the mandatory recognition thresholds,
building the representative evaluation dataset, and running the live Azure
OpenAI evaluation before beginning identity or production infrastructure work.
