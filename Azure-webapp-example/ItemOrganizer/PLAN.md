# Item Organizer — Plan

## Status

**Awaiting guidance. No implementation is authorized.**

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
- **Database:** Azure SQL with Entity Framework Core
- **Photo storage:** Azure Blob Storage
- **AI:** Azure OpenAI with `gpt-5.4-mini` for vision-based item identification and structured outputs
- **Authentication:** Microsoft Entra ID
- **API contract:** REST with OpenAPI/Swagger
- **Hosting:** Azure Static Web Apps Standard for the frontend; Azure App Service for the API
- **Testing:** xUnit for the backend; Vitest and Playwright for the frontend
- **Infrastructure as code:** Bicep

## Local development environment

Local development will use a VS Code Development Container to avoid installing project SDKs and tools directly on the host.

The host requires only:

- Docker Desktop
- Visual Studio Code with the Dev Containers extension
- Git, when Git operations are performed on the host

### Why WSL 2 and Linux containers are used

Docker Desktop uses WSL 2 to provide the Linux kernel needed to run this project's Linux containers on Windows. Developers do not need to work directly inside a user-installed Linux distribution; Docker Desktop can manage its own internal WSL distributions. Enabling Docker Desktop integration with a separate Linux distribution is optional and is needed only when Docker commands will be run from that distribution.

Linux containers are the project default because the .NET SDK, Node.js, SQL Server, Azurite, Azure CLI, and Bicep support them well. They also provide a broad image ecosystem, generally use fewer resources than Windows containers, align with common GitHub Actions runners and possible Linux App Service deployment, and expose Linux-specific path, casing, permission, and shell issues during development. This is a development and testing choice; it does not require the production API to use a custom container. Production may still use either the App Service managed .NET runtime or a separate Linux production image.

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

The development container will include pinned versions of:

- .NET 10 SDK
- Node.js and the frontend package manager
- EF Core CLI
- Azure CLI and Bicep CLI
- Git and required development utilities

Docker Compose will coordinate:

- The development container, with the repository mounted as its workspace
- SQL Server for local Azure SQL-compatible development
- Azurite for local Blob Storage emulation
- A queue emulator if an Azure Storage Queue-based analysis worker is selected

Named Docker volumes will be used where appropriate for NuGet packages, frontend packages, SQL data, and Azurite data. Builds, migrations, tests, debuggers, the API, and the frontend development server will run inside the development container. No .NET, Node.js, EF Core, SQL Server, Azurite, Azure CLI, or Bicep installation will be required on Windows.

Routine development will use a deterministic mock implementation of Azure OpenAI. Explicit integration tests may use a shared Azure development deployment and credentials supplied through environment variables or a local secret store; credentials must never be committed.

The development container is distinct from the production API image. It contains SDKs, debuggers, shells, and development tools and must not be deployed. If a production container is selected, it will use a separate multi-stage build containing only the published API and ASP.NET Core runtime, run as a non-root user, and be tested independently.

## Local testing

Local testing will use the following layers:

- **Unit tests:** xUnit tests that do not require Docker services and cover validation, authorization policies, assignment rules, analysis state transitions, and AI-result mapping.
- **API integration tests:** xUnit with `WebApplicationFactory`, a test authentication handler, SQL Server, and Azurite. Azure OpenAI responses will normally come from fixed structured-output fixtures.
- **Contract tests:** Verify routes, OpenAPI 3.1, status codes, headers, RFC 9457 Problem Details, paging, ETags, idempotency, and upload behavior.
- **Frontend tests:** Vitest for components and client logic; Playwright for end-to-end workflows against the local API.
- **Live Azure integration tests:** An optional, separately invoked suite for Entra ID, Azure OpenAI, Azure SQL, and Blob Storage integration. It will not be part of the default local test run.
- **Production artifact tests:** CI will build and start the production API image, when container deployment is selected, and verify Linux compatibility, configuration, health checks, upload limits, non-root execution, dependency connectivity, and graceful shutdown.

The default local workflow is:

1. Open the repository in the Development Container.
2. Start SQL Server and Azurite through Docker Compose.
3. Apply EF Core migrations to a dedicated local database and load representative test data.
4. Run the API and frontend development server inside the Development Container.
5. Use Swagger UI and Playwright to exercise container creation, photo upload, analysis polling, item review, assignment, and deletion.
6. Test invalid files, missing scopes, stale ETags, retries, cancellation, conflicts, and unavailable dependencies.

Swagger UI is enabled only in the Development environment. Local integration tests use test authentication; manual Entra ID testing uses dedicated development app registrations and access tokens.

## Deployment approach

Azure resources will be provisioned with Bicep and separated by environment. The deployment will include Azure Static Web Apps Standard, Azure App Service, Azure SQL, private Blob Storage, Azure OpenAI or a reference to an approved deployment, Application Insights, Log Analytics, Key Vault, managed identities, role assignments, health checks, CORS settings, and diagnostic settings.

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
| `POST` | `/api/v1/containers/{containerId}/photo-analyses` | Upload and analyze one photo, then assign every detected item to the specified container |
| `GET` | `/api/v1/analyses/{analysisId}` | Poll analysis state and retrieve results when complete |
| `POST` | `/api/v1/analyses/{analysisId}/cancel` | Request cancellation of queued or running analysis |

Starting analysis returns `202 Accepted`, a `Location` header for the analysis resource, and:

```json
{
	"id": "analysis UUID",
	"photoId": "photo UUID",
	"status": "queued",
	"createdAt": "UTC timestamp"
}
```

Analysis states are `queued`, `running`, `completed`, `failed`, and `cancelled`. A completed analysis includes detected item IDs. A failed analysis includes a safe error code and message, but never provider credentials or raw internal exceptions.

`POST /api/v1/containers/{containerId}/photo-analyses` is a convenience workflow using `multipart/form-data` with a required `file` part. It validates the container, stores the photo, creates an analysis, and returns `202 Accepted` with the analysis resource in the same form as the standard analysis endpoint. When analysis completes, all items detected in that photo are assigned to the specified container with `assignmentStatus` set to `confirmed`. The explicit container selection in this request constitutes user confirmation; it is not an AI suggestion. Creation of the detected items and their assignments is committed atomically. If upload or analysis fails, no detected items or assignments are created; a stored photo may remain available for retry or deletion.

### Items and assignments

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/items` | Search inventory; filter by `search`, `category`, `containerId`, `photoId`, or assignment state |
| `POST` | `/api/v1/items` | Manually create an item not detected from a photo |
| `GET` | `/api/v1/items/{itemId}` | Get an item, its source photo, and assignment details |
| `PATCH` | `/api/v1/items/{itemId}` | Correct an item's editable properties |
| `DELETE` | `/api/v1/items/{itemId}` | Remove an item from the catalog |
| `PUT` | `/api/v1/items/{itemId}/container` | Assign or move an item to a container |
| `DELETE` | `/api/v1/items/{itemId}/container` | Mark an item as unassigned |

An AI-detected item contains:

- `name`
- `description`
- `category`
- `quantity`
- `confidence` from `0` to `1`
- `photoId`
- `analysisId`
- `containerId`, when confirmed or assigned
- `suggestedContainerId`, when AI finds a likely match
- `assignmentStatus`: `unassigned`, `suggested`, or `confirmed`

AI suggestions do not silently become confirmed assignments. A client or user confirms them through `PUT /api/v1/items/{itemId}/container`.

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

## Next step

Revise this plan from the user's guidance. Do not create application code, configuration, dependencies, or additional files until the plan is explicitly accepted.
