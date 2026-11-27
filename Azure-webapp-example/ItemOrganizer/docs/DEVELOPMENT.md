# Development guide

For the product overview, see [README](../README.md). Decisions, progress,
validation history, and remaining gates belong in [PLAN.md](../PLAN.md).

## Contents

- [Environment setup](#before-you-start)
- [Startup details](#start-the-application)
- [Troubleshooting](#troubleshooting-identify-the-failing-step-first)
- [Everyday development](#everyday-development)
- [Configuration](#configuration)
- [Photo-analysis API](#photo-analysis-api)
- [Repository map](#repository-map)

## Before you start

Use native Ubuntu or Ubuntu inside WSL 2, with systemd, standalone Docker
Engine, the Docker CLI, Compose v2, Buildx, Git, and VS Code. On Windows,
connect VS Code to Ubuntu through its WSL extension before using Dev Containers.
The workspace and PostgreSQL images remain Debian-based. Application SDKs and
tools run in the workspace, not on the host.

Normal startup never installs software, uses sudo, or grants Docker permissions.
For explicitly authorized machine provisioning only:

```bash
sudo bash scripts/install-prerequisites.sh --install --sources /external/sources.conf
```

The installer targets Ubuntu, requires systemd, uses only the configured signed
repositories, and refuses conflicting packages instead of uninstalling them.
It installs/starts Docker Engine, CLI, containerd, Compose and Buildx. It does
not install Docker Desktop, Git, editors or application SDKs, or add users to
the docker group. Git/editor and Windows WSL 2 prerequisites are
administrator-managed separately. Future Docker upgrades also need approved
source configuration; this installer does not persist its temporary apt lists.

Docker daemon access is root-equivalent on the Linux host, not limited to this
project's bind mount. Request administrator-approved access. Do not make the
socket world-writable or expose an unauthenticated daemon TCP endpoint.
Published development ports retain their existing all-interface bindings;
firewall/network policy determines access. No project image is published.

### Choose the package sources for your environment

Copy `download-sources.example.conf` **outside the entire Git checkout** and
pass its path with `--sources`. Configure approved public sources or internal
mirrors/feeds for every required input. Only the example contains public source
defaults; it is never loaded automatically. Do not commit internal endpoint,
organization/project information, or credentials.

The format is unquoted `KEY=value`, one entry per line. Blank lines and full-line
`#` comments are accepted, including CRLF files. There is no shell evaluation,
variable substitution, inline-comment syntax, or environment-based source
selection. URLs must use HTTPS without credentials or query strings. Image
references need an explicit registry and a pinned tag/digest, not `latest`.
Required keys are validated; missing/unavailable sources fail without fallback.

| Input | External keys |
|---|---|
| Third-party images | `WORKSPACE_BASE_IMAGE`, `POSTGRES_IMAGE`, `AZURITE_IMAGE` |
| Ubuntu Docker installation | Architecture-specific `UBUNTU_APT_URI_*`, `UBUNTU_SECURITY_URI_*`, `DOCKER_APT_URI`, `DOCKER_APT_KEY_URL` |
| Workspace Debian packages | `DEBIAN_APT_URI`, `DEBIAN_SECURITY_URI` |
| Azure CLI packages and key | `AZURE_CLI_APT_URI`, `AZURE_CLI_KEY_URL` |
| SDK, EF tool, Bicep | Architecture-specific `DOTNET_SDK_URL_*`, `BICEP_URL_*`, and `DOTNET_EF_PACKAGE_URL` |
| Backend/frontend packages | `NUGET_SOURCE`, `NPM_REGISTRY` |
| Optional test browser | Architecture-specific `PLAYWRIGHT_CHROMIUM_URL_*` |

Use `AMD64` or `ARM64` suffixes. Tool/archive versions must match the pins in
`.env`; the browser must match the locked Playwright version. Repository paths
and signed package formats must remain compatible with Ubuntu/Debian and the
requested tools. An internal npm feed does not configure NuGet or EF downloads.
Feed metadata may reference artifact/CDN endpoints: configure a proxying feed
and network policy that keep those artifact requests within approved sources.
The project does not rewrite feed metadata or administrator-managed proxies.

Sources are parsed from the external file. The base-image reference is passed
to Docker as a file-derived build argument; the remaining source file and
download credentials use BuildKit secret mounts. The Dockerfile uses Engine's
built-in frontend, not a separately downloaded syntax image. No secret file is
copied into an image. Source-file changes invalidate the tool-install build
cache through a non-secret hash.
Workspace apt indexes use isolated configured repositories; inherited active
apt source lists are removed from the final image. Further OS package additions
belong in the configured image build, not an unconfigured runtime apt install.

Startup builds the workspace locally. It pulls configured third-party images
when uncached and gives PostgreSQL/Azurite local aliases. Compose uses local
images with `pull_policy: never`; it cannot fall back to public project image
names. Run startup once before VS Code's **Reopen in Container**. Dev Containers
reuses that prepared image. Editor extensions are installed separately through
your approved editor setup, not downloaded automatically by this recipe.

Network downloads are required when inputs are uncached. There is no offline
bundle or project-image publishing/prebuilt-registry workflow.

### Credential files and certificate trust

For authenticated package feeds, pass `--credentials /external/credentials.conf`.
Only the following optional pairs are accepted:

```text
NUGET_USERNAME=<feed username>
NUGET_PASSWORD=<feed password or Packaging Read token>
NPM_USERNAME=<feed username>
NPM_PASSWORD=<feed password or Packaging Read token>
```

This is a format illustration, not a file to commit. Both fields of a pair are
required. Use protected external files (`chmod 600`) and secure transfer/storage
appropriate to your organization. No package-token prompts are implemented.
On WSL, keep credential files in Ubuntu's filesystem: Windows-mounted files may
not provide enforceable Unix mode bits, and startup will not bypass that check.

| Authentication need | Optional external file/directory |
|---|---|
| Archive/signing-key downloads via curl | `--download-auth /external/download.netrc` in curl netrc format |
| Authenticated apt repositories | `--apt-auth /external/apt-auth.conf` in apt auth.conf format |
| Authenticated image registries | `--docker-config /external/docker-client`, containing a protected `config.json` understood by Docker |
| Container certificate trust | `--custom-ca /external/approved-ca.crt`, a PEM CA supplied during image build |

Pass paths, never tokens as command arguments. Credential files must be outside
the Git checkout and inaccessible to group/others. Registry authentication is
separate from package authentication. Keep TLS verification enabled; a PAT does
not fix certificate trust. The Docker daemon's image-pull trust and Ubuntu's
installation trust remain administrator-managed.
Without `--docker-config`, startup uses an empty temporary Docker client
directory, not inherited registry credentials. Authenticated registries require
the explicit protected external `config.json` with file-based `auths`; Docker
credential-helper programs are not part of this workflow.

Startup supplies temporary package credentials inside workspace, writes a
NuGet config containing `<clear/>` and the configured feed, and writes npm
registry/strict-TLS configuration. Authenticated content is removed on restore
success or failure; configured source-only user files remain in the container.
The file-derived registry and strict TLS are also enforced on the pnpm command,
so a project or environment setting cannot override the selected registry.
Transferred credential inputs are deleted before server launch. Installation
lifecycle scripts are disabled during pnpm restore to avoid dependency-owned
downloads outside configured feeds.

Azure CLI device-code sign-in is different: live mode signs in **inside
workspace** when needed. Azure login may survive stop/start or pause/unpause of
the same container, subject to expiry/revocation. It is never baked into an
image and is lost when the container is removed/recreated. No Azure-auth volume
is added. Azure resource login does not authenticate package feeds.

## Start the application

Run commands in the Item Organizer directory from Ubuntu/WSL Ubuntu, not a
Windows Docker Desktop context.

### Start with mock photo analysis

```bash
bash scripts/start-environment.sh --sources /external/sources.conf --mode mock
```

Missing `.env` and `web/.env.local` are copied from examples without overwriting
existing files. Startup checks Docker/plugins, prepares images, starts healthy
dependencies, runs smoke checks, restores both package sets, applies migrations,
and launches API/frontend. Explicit mock mode overrides a previously selected
live provider. It does not make paid AI calls.

Open **http://localhost:5173**. Both modes run in the foreground by default.
Ctrl+C stops the managed API/frontend, not PostgreSQL/Azurite. `--detach` exits
after readiness while leaving the servers running. Stop all containers with
`docker compose stop`; data is preserved.

| Option | Effect |
|---|---|
| `--no-build` | Reuse an existing local workspace image; package restore still runs. |
| `--detach` | Leave servers running after startup exits. |
| `--validate-only` | Check existing workspace/tool/dependency and optional live prerequisites; no build, package restore, migration, server launch or Azure login. Configured missing dependency images may still be pulled and containers started. |
| `--skip-application-start` | Build/prepare/check the container environment without application restore/migration/startup. |
| `--prepare-browser` | Download the explicitly configured Chromium archive, validate its locked version and prepare it for browser tests. |
| `--custom-ca` | Add an approved container CA during build; incompatible with `--no-build`. |

Logs are inside workspace at `/tmp/itemorganizer/api.log` and `frontend.log`.
Readiness failures include recent log output. An existing foreground launcher
owns the server lifecycle; concurrent startup fails. Stop containers before
switching mock/live configuration. A stopped existing container normally retains
its writable filesystem; Compose recreation does not.

### Start with real photo identification

```bash
bash scripts/start-environment.sh --sources /external/sources.conf --mode live --no-build \
  --endpoint <resource-https-endpoint> --deployment-name <deployment-name> \
  --resource-name <resource-name> --resource-group <resource-group> \
  --tenant-id <tenant-id>
```

Resource values have no personal/project-specific defaults. Live mode checks
the workspace Azure session, starts device-code login if missing, rechecks
login, and requires deployment provisioning state `Succeeded`. For a
non-interactive check, append `--validate-only`; missing login is an error.
Endpoint/model/provider overrides apply to the API process, not your `.env`.

**Live analysis sends uploaded photos to Azure OpenAI and incurs Azure usage
charges.** Upload JPEG, PNG or WebP up to 10 MiB, 512-8000 pixels per side.
Review/correct suggestions and bounding boxes before confirming inventory.
Invalid model geometry may be omitted with a warning; draw/correct it manually.

## Troubleshooting: identify the failing step first

| Symptom | Action |
|---|---|
| Missing/inaccessible Docker Engine | Ask the administrator to verify systemd Docker service and approved user access. Normal startup does not change host permissions. |
| Desktop or Windows container engine detected | Use standalone Docker Engine inside Ubuntu, not Desktop's context/distributions. |
| Missing/invalid source key | Correct the external file; no runtime default or environment override exists. |
| HTTP 401/403 | Check the correct credential file, token validity/read scope and account feed access. Azure login is separate. |
| TLS error | Check the actual requested endpoint and approved host/daemon/container CA trust. Never disable verification. |
| Local workspace image missing | Run startup without `--no-build`; the project image is never pulled from a registry. |
| Azure session/deployment check fails | Sign in inside workspace and verify tenant, subscription, resource identifiers and permissions. |
| Another configuration/server owns ports | Stop the existing launcher/containers before changing modes. |
| Browser archive/version error | Match the external Chromium URL to the locked Playwright browser version and rerun `--prepare-browser`. |
| Cleanup fails | Stop/remove workspace before sharing it; inspect the reported error and avoid leaving temporary credentials behind. |

```bash
docker compose ps
docker compose logs --tail 50 workspace postgres azurite
docker compose exec -T workspace az account show --output table
```

Windows browser access additionally requires working WSL localhost forwarding
and compatible firewall policy. Container readiness does not prove that host
forwarding works.

## Everyday development

### Restore dependencies

Rerun the unified startup with the external files and `--no-build`. Source-only
NuGet/npm user configuration remains available inside workspace; authenticated
restore requires supplying credential files again. Avoid implicit package
restores in ordinary commands by using `--no-restore`.

### Run the API or frontend separately

For debugging inside an already prepared workspace:

```bash
docker compose exec -T workspace dotnet run --project src/ItemOrganizer.Api --no-restore
docker compose exec workspace pnpm -C web dev
```

These are manual commands, not separate startup scripts. Stop the managed
launcher first to avoid port conflicts.

### Work with the database

```bash
docker compose exec -T workspace dotnet run --project src/ItemOrganizer.Database --no-restore -- migrate
bash scripts/reset-and-seed.sh --confirm
```

**Resetting removes database data.** It requires explicit confirmation and does
not pull/build images or restore packages. Run startup first. A smoke test never
resets data.

Open a PostgreSQL shell with
`docker compose exec postgres psql -U itemorganizer -d itemorganizer`.
After model changes, create migrations with the existing EF tooling; any
implicit authenticated restore still requires the configured source/credentials.

### Run tests

```bash
docker compose exec -T workspace dotnet test ItemOrganizer.sln --no-restore
docker compose exec -T workspace pnpm -C web test
bash tests/Scripts/start-environment.Tests.sh
```

Prepare browser tests through unified startup with `--prepare-browser` and the
external source/credential files. Then stop its API/frontend (Ctrl+C, or
`docker compose stop` for detached startup and restart dependency containers):

```bash
docker compose up -d --wait --no-build workspace
docker compose exec -T workspace pnpm -C web test:e2e
```

The browser command resets/seeds the database and starts test servers, but no
longer invokes Playwright's automatic browser/dependency downloads. Chromium
Linux libraries are installed during workspace build from configured Debian
sources. Test traces/reports remain ignored. Script regressions use mocked
Docker/Azure and do not establish native Ubuntu/WSL end-to-end validation.

## Configuration

| Service | Purpose | Host address |
|---|---|---|
| `workspace` | Tools, API, frontend | API http://localhost:5000; frontend http://localhost:5173 |
| `postgres` | Local database | `localhost:54320` |
| `azurite` | Blob/Queue/Table emulator | Ports `10000`, `10001`, `10002` |

Inside Compose, use `postgres`/`azurite`, not `127.0.0.1`, for dependencies.
Named data/cache volumes preserve PostgreSQL/Azurite state and package caches.
`.env` contains application settings and pinned versions, not download sources.
Remove legacy image/registry variables from existing local `.env` files; they
are no longer used for source selection. `web/.env.local` configures frontend
authentication/API access. Local development identities must never be deployed.

Existing authentication, storage, queue, analysis, and Azure OpenAI application
settings remain supported. Production uses managed Azure application, database,
storage and AI services; Docker Engine is a local development prerequisite.

## Photo-analysis API

This workflow summary is not the full API contract. Use development OpenAPI
at `/openapi/v1.json`. API routes are under `/api/v1`. Outside development
authentication, reads require delegated `ItemOrganizer.Read`, the configured
tenant, and valid `tid`/`oid` claims.

| Operation | Endpoint and behavior |
|---|---|
| Upload photo | `POST /api/v1/photos`, multipart `file`; JPEG/PNG/WebP up to 10 MiB, 512-8000 pixels per side; optional `Idempotency-Key` |
| Delete photo | `DELETE /api/v1/photos/{photoId}`; retryable |
| Start analysis | `POST /api/v1/photos/{photoId}/analyses`; delegated analysis scope, optional idempotency key, returns `202`, `Location`, `ETag` |
| Poll analysis | `GET /api/v1/analyses/{analysisId}` |
| Cancel analysis | `POST /api/v1/analyses/{analysisId}/cancel`; analysis scope and current `If-Match` |
| Read item crop | `GET /api/v1/items/{itemId}/crop`; authorized, short-lived access streamed through the web client |

Photos/crops remain private. The SQL outbox and storage queue drive analysis;
explicit user review and confirmation are required before inventory creation.

## Repository map

| Path | Contents |
|---|---|
| `README.md` | Purpose, usage, quick start |
| `PLAN.md` | Decisions, milestones, progress and validation |
| `.devcontainer/`, `docker-compose.yml` | Workspace recipe and local-only service images |
| `download-sources.example.conf` | Public-only external-source format example |
| `.env.example`, `web/.env.example` | Application/version configuration templates |
| `scripts/` | Installer, unified startup, explicit reset and smoke test |
| `src/ItemOrganizer.Domain/` | Domain model/state transitions |
| `src/ItemOrganizer.Infrastructure/` | Persistence/migrations |
| `src/ItemOrganizer.Database/` | Migrate/reset/seed commands |
| `src/ItemOrganizer.Api/` | REST, auth, health, OpenAPI |
| `web/` | React/TypeScript app |
| `tests/` | Backend/integration/script regressions |
