# Development guide

Use this guide to configure package access, troubleshoot startup, and work on
Item Organizer. For the product overview and first-run workflow, see the
[README](../README.md). Project progress and acceptance criteria are in
[PLAN.md](../PLAN.md).

## Contents

- [Environment setup](#before-you-start)
- [Startup details](#start-the-application)
- [Troubleshooting](#troubleshooting-identify-the-failing-step-first)
- [Everyday development](#everyday-development)
- [Configuration](#configuration)
- [Photo-analysis API](#photo-analysis-api)
- [Repository map](#repository-map)

## Before you start

### Choose the package sources for your environment

This project is developed in multiple environments. A network restriction in
one environment is **not** a project-wide ban on a package source.
Package access is independent of the choice between mock and live photo
analysis; both modes require development dependencies.

| Environment | Where to get packages | Authentication |
|---|---|---|
| Public package access is permitted | nuget.org may be used for .NET packages. Use the npm registry permitted by that environment. | Ordinary public nuget.org downloads do not require an Azure DevOps PAT. Other registries have their own requirements. |
| Direct public package access is restricted | Use that environment's approved internal feeds, such as Azure DevOps `One_PublicPackages`. | Authenticated Azure DevOps feeds require feed read access and package credentials, such as a PAT with **Packaging: Read** scope. |

Check the configuration in your checkout **before starting a build or restore**:

| Download | Configuration to check |
|---|---|
| Backend .NET dependencies | `NuGet.config` currently selects public nuget.org. Its `<clear />` removes inherited package sources; do not assume a host-level NuGet setting will override it. |
| EF Core command-line tool used to build the workspace image | `scripts\start-environment.ps1` currently downloads it from public nuget.org using Windows certificate validation. `.devcontainer\Dockerfile` also has a public-download fallback. This is separate from backend restore. |
| Frontend npm dependencies | `web\.npmrc` reads `ITEMORGANIZER_NPM_REGISTRY` from the container environment. `.env.example` supplies an Azure DevOps default. |
| Frontend feed credentials | `scripts\restore-frontend.ps1` supplies credentials for the `One_PublicPackages` npm endpoint. It is not a general-purpose login helper for every registry. |

For environments using Azure DevOps, `One_PublicPackages` has separate
endpoints. The npm endpoint is the frontend default; listing the NuGet endpoint
here does not configure backend restore to use it:

```text
NuGet: https://pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/nuget/v3/index.json
npm:   https://pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/npm/registry/
```

**The startup scripts are not automatic environment selectors.** There is no
single switch that changes all package sources. Changing the npm registry does
not change NuGet or the EF-tool download. The frontend feed-authentication
helper targets `One_PublicPackages`; it does not supply NuGet credentials.

**Restricted-network setup is not a ready-to-run bootstrap path in this
checkout.** Backend restore and the EF-tool download need approved,
environment-specific source and credential configuration first. The bootstrap
has no EF-tool source override, and this guide does not provide a complete
restricted-network procedure. Stop before bootstrap if these public downloads
are prohibited; obtain the environment's approved configuration rather than
bypassing the restriction. A frontend PAT alone is insufficient.

### Understand the three kinds of authentication

| What you are doing | Credential | What it does not do |
|---|---|---|
| Downloading packages from an authenticated Azure DevOps feed | Azure DevOps PAT with **Packaging: Read**, from an account with feed read access | Does not sign you in to Azure resources or Item Organizer |
| Calling the live Azure OpenAI deployment | Azure identity, normally an Azure CLI session **inside the workspace container** during development | Does not authenticate npm or NuGet to Azure DevOps |
| Using Item Organizer | Local development identity, or Microsoft Entra sign-in when configured | Does not authenticate development package downloads |

A PAT is needed because the **feed requires authentication**, even if it
contains public upstream packages. It is not an inherent requirement of .NET,
npm, Docker, or photo analysis. Write permissions are not needed for downloads.

Keep tokens out of source control, `.env`, command arguments, and screenshots.
The frontend restore helper prompts securely or accepts the path to an
existing protected PAT file. Supplying that file does not grant feed access
your account lacks. NuGet feed authentication must be configured separately
when your environment uses an authenticated NuGet source.

### Understand where commands run

Run the PowerShell commands in this guide from the Item Organizer directory
on the Windows host. Commands beginning with `docker compose exec` execute
the following program **inside** the named container.

The `workspace` container contains the project SDKs and tools. Builds, tests,
migrations, and application servers run there, not directly on Windows.
Host and container Azure sessions and certificate trust stores are separate.
A successful host login or download does not prove the container can do it.

## Start the application

### Prerequisites

Use an existing Windows setup with Git, WSL 2, Docker Desktop in
Linux-container mode, and Visual Studio Code with the Dev Containers extension.
The development images support x64 and ARM64.

Do not install or reconfigure host tooling as part of normal project work.
For a **new host only**, and only when host provisioning is explicitly
permitted, `scripts\install-prerequisites.ps1` is an Administrator-level
installer for the host prerequisites. It is not a package-restore fix.

Before continuing, start Docker Desktop and confirm that your checkout's
package sources and feed credentials match the environment described above.

### Start with mock photo analysis

```powershell
.\scripts\start-environment.ps1
```

With the default `.env.example` settings, this starts the application using
**predictable mock detections**. Mock mode exercises upload, review, and
inventory workflows; it does not identify the actual objects in your photo
and makes no paid AI calls.

The bootstrap creates missing `.env` and `web\.env.local` files without
overwriting existing ones. It builds and starts the Docker services, checks
the environment, restores dependencies, applies database migrations, and
starts the API and frontend.

When frontend dependencies are missing, the bootstrap prompts for the
Packaging Read PAT for the default Azure DevOps npm feed. This token is not
used for backend NuGet restore. To supply an existing protected file instead:

```powershell
.\scripts\start-environment.ps1 -FrontendPatPath C:\secure\azure-devops-pat.txt
```

The PAT file is a local secret; do not add it to the repository.

Open **http://localhost:5173**. Create a container, add an item, and search for
it. For the photo workflow, upload a photo, review the suggestions, choose
container assignments, and explicitly confirm the items.

Useful bootstrap options:

| Option | Effect |
|---|---|
| `-NoBuild` | Reuse the existing workspace image. Application package restore still runs unless application startup is skipped. |
| `-SkipApplicationStart` | Prepare the containers and toolchain without restoring application packages, migrating, or starting the application. Building the image can still download the EF tool. |
| `-SkipSmokeTest` | Skip the environment smoke test. It does not fix an environment problem. |
| `-CustomCaPath C:\path\to\organization-ca.crt` | Include an approved PEM CA certificate in the workspace image trust store during a build. Cannot be combined with `-NoBuild`. |

After the workspace image is prepared, you can also use VS Code's **Reopen in
Container** command. The bootstrap stages the pinned EF-tool package for the
image build and removes the temporary package file afterward.

To stop the local services without deleting their stored data:

```powershell
docker compose stop
```

Rerun the bootstrap with `-NoBuild` to resume using the existing image.

### Start with real photo identification

First complete the environment setup and frontend package restore. You also
need access to an Azure OpenAI resource and a ready model deployment.
If mock servers are running, use `docker compose stop` first so live mode can
start its own API and frontend.

```powershell
.\scripts\start-live-ai.ps1 -TenantId <your-tenant-id>
```

Replace `<your-tenant-id>` with the tenant that owns the Azure OpenAI resource.
If the workspace lacks an Azure CLI session, the script starts device-code
login. Follow the displayed URL and enter the displayed code in your browser.
**This signs in the container, not the Windows host.**

The script verifies the deployment and frontend dependencies, restores backend
packages, applies migrations, and starts the API and frontend. Backend restore
uses `NuGet.config`. Restore missing frontend packages with the
[frontend helper](#restore-dependencies) before using this script.

The script defaults to the development deployment
`gpt-5.4-mini-itemorganizer-dev` and the resource configured in
`scripts\start-live-ai.ps1`. For your own resource, use the
[explicit resource parameters in the README](../README.md#enable-real-photo-identification):
`-Endpoint`, `-DeploymentName`, `-ResourceGroup`, and `-ResourceName`.
The deployment name is the name assigned in Azure, not just the model family.

**Live analysis sends the uploaded photo to Azure OpenAI and incurs Azure
usage charges.** At http://localhost:5173, open **Photo analysis**, choose a
JPEG, PNG, or WebP image, and select **Upload and analyze**. Images must be
512-8000 pixels on each side and no larger than 10 MiB.

The page polls for results. Correct detected names, quantities, categories,
bounding boxes, and container assignments before confirming. AI suggestions
are not guaranteed accurate and do not become inventory items automatically.
If the model returns a bounding box outside the photo or with nonpositive
dimensions, the app keeps the detection without that box and adds a warning.
Review and draw its box manually before confirming if a photo crop is needed.
Press **Ctrl+C** in the live-startup terminal to stop its API and frontend.
Use `docker compose stop` afterward if you also want to stop the local services.

To check live prerequisites without restoring packages, prompting for login,
or starting the application servers:

```powershell
.\scripts\start-live-ai.ps1 -ValidateOnly
```

This check can start the Compose services. It reports a missing Azure session
as an error rather than starting device-code login.

## Troubleshooting: identify the failing step first

| Symptom | What to check |
|---|---|
| Docker engine unavailable or `dockerDesktopLinuxEngine` not found | Start Docker Desktop, select Linux containers, and wait for the engine to be ready. |
| Package request goes to a source not permitted **in this environment** | Check the actual URL and the source settings listed above, including the separate EF-tool download. Do not infer a repository-wide rule from the local restriction. |
| Azure DevOps feed returns HTTP 401 or 403 | Check PAT validity, **Packaging: Read** scope, and the account's read access to that feed. Azure resource login is separate. |
| `SSL connection could not be established` or certificate-chain error | Inspect the underlying TLS error and requested host. The container may lack an organization-approved CA. A PAT does not fix certificate trust. |
| Azure CLI session missing or expired | Sign in inside `workspace`, using the resource's tenant. Host sign-in is not sufficient. |
| Azure OpenAI deployment cannot be read | Check subscription, tenant, resource group, resource name, deployment name, and Azure permissions. |
| Frontend dependencies missing | Restore from the environment's configured npm feed before retrying startup. |
| Live prerequisites pass but startup fails at backend restore | The Azure check succeeded; diagnose package source, feed authentication, and TLS separately. Do not repeat Azure login merely because restore failed. |

For Docker service status:

```powershell
docker compose ps
docker compose logs --tail 50 workspace postgres azurite
```

For container-side Azure sign-in and subscription selection:

```powershell
docker compose exec -T workspace az login --use-device-code --tenant <your-tenant-id>
docker compose exec -T workspace az account show --output table
docker compose exec -T workspace az account set --subscription <your-subscription-id>
```

If your organization requires a custom CA, obtain its approved PEM certificate
and rebuild using the existing bootstrap:

```powershell
.\scripts\start-environment.ps1 -CustomCaPath C:\path\to\organization-ca.crt -SkipApplicationStart
```

The certificate is supplied to BuildKit as a secret and installed in the
image's trust store. Its source file is not copied into the repository.
Do not disable TLS verification. Container recreation may require Azure login
again. A certificate fix does not authorize access to a prohibited source.

## Everyday development

Run these commands after the environment and dependencies are ready.
`--no-restore` prevents these .NET commands from implicitly attempting another
package restore.

### Restore dependencies

For backend packages, first ensure `NuGet.config` and any container-side
credentials match your [environment's package sources](#choose-the-package-sources-for-your-environment).
With the checked-in configuration, this command contacts **public nuget.org**:

```powershell
docker compose exec -T workspace dotnet restore ItemOrganizer.sln
```

For the supplied Azure DevOps npm feed:

```powershell
.\scripts\restore-frontend.ps1
```

This helper temporarily adds feed credentials to the container user's
`.npmrc`, runs `pnpm install`, then restores or removes that user-level file.
The token is not included in process arguments or committed files.

The frontend helper accepts
`-PatPath C:\secure\azure-devops-pat.txt`.

### Run the API or frontend separately

```powershell
docker compose exec -T workspace dotnet run --project src/ItemOrganizer.Api --no-restore
docker compose exec workspace pnpm -C web dev
```

Run these in separate terminals if both servers are needed.

### Work with the database

Apply migrations, or reset the local database and seed representative data:

```powershell
docker compose exec -T workspace dotnet run --project src/ItemOrganizer.Database --no-restore -- migrate
.\scripts\reset-and-seed.ps1
```

**Resetting removes existing local data.** Use only the first command if you
want to apply migrations without resetting it.

Open a PostgreSQL shell with
`docker compose exec postgres psql -U itemorganizer -d itemorganizer`.
Use `\dt` to list tables, `\d containers` to inspect a table, and `\q` to exit.

After changing the persistence model, create an EF migration:

```powershell
docker compose exec -T workspace dotnet ef migrations add MigrationName --project src/ItemOrganizer.Infrastructure --startup-project src/ItemOrganizer.Database --output-dir Migrations
```

EF tooling builds the project and may restore packages, so its configured
sources and credentials must also be appropriate for your environment.

### Run tests

Backend unit, PostgreSQL integration, and API contract tests:

```powershell
docker compose exec -T workspace dotnet test ItemOrganizer.sln --no-restore
```

Browser workflows:

```powershell
docker compose exec -T workspace pnpm -C web test:e2e
```

The browser command downloads the matching Chromium runtime and Linux
dependencies, resets and seeds the local database, starts the test servers,
and exercises upload, analysis polling, review, assignment, and convenience
workflows. Ensure those additional downloads are permitted in your environment.
Failure traces, screenshots, and the HTML report are ignored by Git.

## Configuration

### Local services and files

| Service | Purpose | Host address |
|---|---|---|
| `workspace` | Development tools, API, and frontend | API: http://localhost:5000; frontend: http://localhost:5173 |
| `postgres` | Local PostgreSQL database | `localhost:54320` |
| `azurite` | Local Azure Blob, Queue, and Table Storage emulator | Ports `10000`, `10001`, and `10002` respectively |

Inside Compose, connect to `postgres` and `azurite` by service name, not
`127.0.0.1`. Named volumes `postgres-data` and `azurite-data` preserve local
state across container recreation; `nuget-cache` and `pnpm-store` cache packages.

`.env` configures the Compose environment and backend settings.
`web\.env.local` configures the frontend. The example files enable a local
development identity; configure Entra settings before using real application
sign-in. Never use the local development identity in a deployed environment.

| Area | Settings |
|---|---|
| Frontend | `VITE_API_BASE_URL`, `VITE_DEVELOPMENT_AUTHENTICATION`, `VITE_ENTRA_CLIENT_ID`, `VITE_ENTRA_TENANT_ID`, `VITE_API_SCOPE` |
| API authentication | `Authentication__Authority`, `Authentication__Audience`, `Authentication__AllowedTenantId` |
| Private photo storage | `PhotoStorage__ContainerName`, `PhotoStorage__ReadUrlMinutes`, `PhotoStorage__MaximumPhotosPerOwner`, `PhotoStorage__CleanupEnabled`, `PhotoStorage__CleanupIntervalMinutes` |
| Analysis worker | `Analysis__WorkerEnabled`, `Analysis__PollingSeconds`, `Analysis__QueueName`, `Analysis__DeadLetterQueueName` |
| Analysis provider and result metadata | `Analysis__Provider`, `Analysis__Model`, `Analysis__PromptVersion`, `Analysis__SchemaVersion` |
| Live Azure OpenAI | `AzureOpenAI__Endpoint`; optional `AzureOpenAI__ApiKey` only through an approved local secret source |

For manually configured live analysis, use `Analysis__Provider=azure-openai`,
set `Analysis__Model` to the deployment name, and set `AzureOpenAI__Endpoint`
to the resource's HTTPS endpoint. Authentication uses `DefaultAzureCredential`
unless an approved API-key configuration is supplied. The live-startup script
sets the provider, model, and endpoint for its API process.

## Photo-analysis API

This is a workflow-specific summary, not a complete inventory API reference.
Use the development OpenAPI document for the full endpoint contract.

The API is under `/api/v1`. Development OpenAPI is at `/openapi/v1.json`.
Outside local development authentication, resource reads require a delegated
token with `ItemOrganizer.Read`, the configured tenant, and valid `tid` and
`oid` claims.

| Operation | Endpoint and behavior |
|---|---|
| Upload a photo | `POST /api/v1/photos`, multipart field `file`; accepts JPEG, PNG, or WebP, up to 10 MiB and 512-8000 pixels per side. Animated or malformed images are rejected. Optional `Idempotency-Key`. |
| Delete a photo | `DELETE /api/v1/photos/{photoId}`; retryable deletion |
| Start analysis | `POST /api/v1/photos/{photoId}/analyses`, with `ItemOrganizer.Analyze`; optional `Idempotency-Key`; returns `202 Accepted`, `Location`, and `ETag` |
| Poll analysis | `GET /api/v1/analyses/{analysisId}`, with `ItemOrganizer.Read` |
| Cancel analysis | `POST /api/v1/analyses/{analysisId}/cancel`, with `ItemOrganizer.Analyze` and current `If-Match` |
| Read a confirmed item crop | `GET /api/v1/items/{itemId}/crop`; authorized, short-lived access; the web client uses streamed content |

Photos and crops remain private. The background analysis pipeline uses a SQL
outbox and Azure Storage Queue, then saves detection drafts. Explicit review
and confirmation are required to create inventory items from those drafts.

## Repository map

| Path | Contents |
|---|---|
| `README.md` | Product overview, usage, and quick start |
| `docs\DEVELOPMENT.md` | Environment setup, troubleshooting, development commands, and reference |
| `PLAN.md` | Product decisions, milestones, exit criteria, and validation history |
| `.devcontainer\`, `docker-compose.yml` | Workspace image and local services |
| `.env.example`, `web\.env.example` | Backend/Compose and frontend configuration templates |
| `NuGet.config`, `web\.npmrc` | Package-source configuration |
| `scripts\` | Startup, package restore, environment checks, and database helpers |
| `src\ItemOrganizer.Domain\` | Domain model and state transitions |
| `src\ItemOrganizer.Infrastructure\` | EF Core mappings and migrations |
| `src\ItemOrganizer.Database\` | Migration and seed entry point |
| `src\ItemOrganizer.Api\` | REST API, authorization, health, and OpenAPI |
| `web\` | React and TypeScript application |
| `tests\` | Backend, integration, and script tests |
