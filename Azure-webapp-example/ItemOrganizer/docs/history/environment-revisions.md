# Historical environment recipes and revision evidence

[High-level plan](../../PLAN.md) | [Current environment design](../design/development-environment.md)

This archive preserves earlier environment/source decisions and their dated validation. It is not the current design or a working setup guide.

## Historical environment design and revision evidence

## Local development environment

Local development will use a VS Code Development Container to avoid installing project SDKs and tools directly on the host.

The host environment must not be modified for project development or
validation. Do not install, uninstall, upgrade, or reconfigure project tooling
on the host to complete repository work. If the Development Container or
Docker services are unavailable, stop and restore those existing services or
report the blocked validation; do not fall back to changing the host.

The host requires only the container-platform and editor prerequisites below;
application SDKs and tools remain inside the Development Container:

- On Windows, WSL 2 with Ubuntu and systemd enabled
- Docker Engine, including the `dockerd` daemon, Docker CLI, containerd,
  Docker Buildx plugin, and Docker Compose v2 plugin, installed inside that
  Ubuntu distribution (or directly on a native Ubuntu host)
- Visual Studio Code with the Dev Containers extension and, on Windows, the
  WSL extension
- Git in the environment where checkout and Git operations are performed

Docker Desktop is not a prerequisite and must not be installed as part of
project setup. These are platform setup requirements, not authorization for
repository tasks to install or reconfigure host software.

### Approved environment and download-source revision (November 27, 2026)

**Decision status:** Confirmed with the user and approved for implementation.
This section supersedes earlier Windows-only launcher and package-source
assumptions. The scripts and checked-in source settings have been migrated;
implementation evidence and blocked runtime gates are recorded below.

#### Supported platforms and container boundaries

Support Windows with Ubuntu inside WSL 2, and native Ubuntu. Run the same Linux
scripts in either environment. Keep the existing Debian-based workspace and
PostgreSQL containers; the host and container distributions need not match.
Continue running Azurite as a separate container. Project SDKs, builds, tests,
migrations, and application servers remain inside the workspace container.

Use standard Docker Engine, the Docker CLI, containerd, Buildx, and Compose v2,
not Docker Desktop. A separate, explicitly invoked installer targets Ubuntu;
Windows WSL 2 provisioning is documented separately. Installation and Docker
access permissions are administrator-managed. Normal project startup must not
install host software, use sudo, add users to groups, or alter permissions.
Rootless Docker is not required.

Access to the standard Docker daemon is root-equivalent on its Linux host.
Mounting only the project directory in this Compose recipe does not restrict
a daemon-authorized user to that directory. Keep the existing published-port
bindings rather than making them localhost-only; firewall and network policy
determine reachability. Not publishing an image does not make running services
private. Verify Windows-browser access through WSL separately.

#### Final script responsibilities

| Script | Approved responsibility |
|---|---|
| `scripts\install-prerequisites.sh` | Opt-in, administrator-approved Ubuntu Docker Engine prerequisite installation using externally configured sources. Never install or launch Docker Desktop or automatically grant daemon access. |
| `scripts\start-environment.sh` | Single startup entry point: check prerequisites, prepare configuration, build or reuse the local workspace image, start containers, check the environment, restore backend and frontend packages, apply migrations, launch the API/frontend, and verify readiness. Explicit mock/live modes; live additionally checks Azure authentication and deployment readiness. |
| `scripts\reset-and-seed.sh` | Explicitly reset the application database and create representative seed data. Keep this destructive action separate from startup and smoke testing. |
| `scripts\smoke-test.sh` | Check workspace tool versions, dependency connectivity, and container connection settings without resetting application data. |

Replace the PowerShell workflow with the shared Linux scripts. Incorporate
`restore-frontend.ps1`, `start-live-ai.ps1`, and `start-application.sh` into
`start-environment.sh`, rather than retaining separate restore, live-startup,
or application-launcher scripts. Preserve useful diagnostics, readiness checks,
logging, and credential cleanup while removing duplicated startup paths.

Both mock and live startup run in the foreground by default. Ctrl+C stops the
API and frontend while leaving PostgreSQL/Azurite running. An explicit
`--detach` option leaves application servers running after startup exits.
Startup failures must be reported and must clean up newly started servers.
Routine startup applies migrations, not a reset. Existing data is seed-only
and disposable for this migration; no backup procedure is required, but this
does not authorize silently resetting data during normal startup.

#### External sources and credentials

Every download source must be defined in a local configuration file outside
the repository, whose path is explicitly supplied to setup/startup. Source
selection must not come from environment variables, tracked runtime defaults,
or hard-coded public or internal endpoints. This includes NuGet configuration,
npm registry configuration, the EF-tool download, third-party container images,
the Dockerfile build frontend if downloaded, Ubuntu/Debian installation
repositories and signing-key downloads, Docker installation sources, SDK/tool
archives and repositories, and test-browser binaries and dependencies.

Commit an example configuration containing public sources only. Developers
must explicitly copy and configure it outside the checkout before use; it is
not an automatic runtime fallback. Do not commit internal feed URLs or
organization/project names. Remove existing internal references and hard-coded
runtime sources as part of implementation. Keep tool/image versions pinned;
changing where an input is downloaded must not silently change its version.
The external file uses host names only (for example, `NUGET_SOURCE=nuget.org`),
not URLs or artifact paths. Repository-owned adapters resolve standard endpoint
paths, pinned image identifiers, and versioned artifacts. Custom mirrors must
expose these standard paths; custom feed paths are not supported.

Credentials for authenticated downloads come only from protected files outside
the repository. Accept credential-file paths, never tokens in command arguments,
and do not prompt interactively for package-feed tokens. Do not put credentials
in source control, `.env`, logs, image layers, or build arguments. Supply
package-manager configuration and credentials inside builds/containers through
appropriate file-based secret handling, with temporary credential cleanup.
NuGet and npm authentication are separate from Azure resource authentication.

Use only explicitly configured sources. Missing configuration, authentication
failure, or an unavailable source is an error; do not switch to an unconfigured
public or internal source. Environments must provide reachable approved network
sources or mirrors for all required inputs. Fully offline first-time setup and
offline dependency bundles are not required.

#### Image reuse, Azure login, and deployment

Build this project's workspace image locally and reuse it locally when
appropriate. Do not add an image-publishing workflow, public or private, or a
prebuilt project-image registry dependency. Third-party images, SDKs, tools,
Linux packages, application dependencies, and browser binaries are still
downloaded from configured sources when not already cached.

Preserve container-side Azure CLI device-code login for live mode when its
session is unavailable. Authentication may remain in the writable filesystem
of the same stopped or paused container, subject to expiry/revocation. Never
bake authentication into the image. Do not add Azure-login persistence across
container removal/recreation; that remains distinct from stopping/restarting
the same container. Package credentials remain non-interactive external files.

Docker is the local development platform, not a new production requirement.
The planned deployment remains Azure App Service for the API, Azure Static Web
Apps for the built frontend, managed PostgreSQL, Azure Storage, and Azure
OpenAI. Align runtime/database compatibility and application behavior; local
emulators and mock AI do not replace real Azure integration validation.

#### Implementation sequence and acceptance gates

1. Implement the external source-file contract and public-only example, remove
   tracked internal/default source settings, and route every required download
   through that contract with protected external credential files.
2. Implement the four-script Ubuntu/WSL Docker Engine workflow, including local
   image reuse, integrated restores, foreground/detached lifecycle, live login
   checks, and separate non-destructive smoke/destructive reset commands.
3. Update README/development instructions and directly dependent test commands;
   replace launcher regressions for the removed PowerShell scripts.
4. Validate missing/invalid sources and credentials, unavailable feeds, absence
   of unconfigured fallback, secret cleanup, mode selection, readiness failure,
   Ctrl+C cleanup, detached startup, database migration/reset separation, and
   container stop/restart versus recreation authentication behavior.
5. On both native Ubuntu and Windows/Ubuntu WSL 2, verify a clean-checkout image
   build, tool versions, healthy Compose dependencies, backend/frontend restore,
   migrations, mock end-to-end startup, and host-browser access. Verify live
   authentication/deployment checks with authorized Azure access separately.

**Current validation limitation:** This session has no available supported
Ubuntu Docker Engine development environment. The discovered WSL distributions
belong to Docker Desktop. Do not provision or reconfigure this host to bypass
that limitation. Script-only/mocked checks cannot establish the end-to-end exit
criteria; record remaining gates explicitly until they pass.

#### Implementation progress and validation (November 27, 2026)

**Code-complete; environment acceptance remains blocked.**

The four-script workflow is implemented. Startup integrates backend/frontend
restore, mock/live selection, migrations, bounded readiness checks, foreground
ownership and signal cleanup, and explicit detached operation. Reset requires
`--confirm` and remains separate from non-destructive smoke testing. The
opt-in installer uses administrator-approved Ubuntu/Docker sources, starts
standard Engine, and does not grant user access or install project SDKs.
Legacy PowerShell launchers and the separate application launcher are removed.

`download-sources.example.conf` is the only checked-in source example and
contains public source hosts only. Startup/installer require an explicitly passed
external source file and reject in-repository files, unknown/duplicate/empty
entries, URLs, paths, credentials, and invalid host/port values. Source files
are data, not executable shell configuration. Image identifiers and pinned
tags/digests are repository-owned rather than external source settings.
Pinned versions remain intact. The checked-in NuGet source configuration and
registry environment-variable mechanism are removed.

Docker builds receive source and download/apt credentials through secret
mounts; only the file-derived base-image identifier and non-secret source hash
are build arguments. The Dockerfile uses the built-in build frontend and
isolated configured apt indexes, removes inherited active apt source lists,
and does not retain secret inputs in image layers. Third-party service images
receive local aliases; Compose prohibits image pulls and uses the locally
built workspace image. No project-image publishing workflow was added.

Workspace runs as the existing `node` development user. NuGet/npm user
configuration is generated from the external source file; temporary package
credentials are removed on restore success/failure, and transferred inputs are
deleted before application startup. Explicit external netrc, apt auth and
Docker client configuration paths support other authenticated downloads.
Unix credential mode checks are enforced, not bypassed for Windows-mounted
files. Azure login remains container-local without an authentication volume.

Frontend install lifecycle scripts are disabled to prevent dependency-owned
unconfigured downloads. Browser binaries are prepared only with
`--prepare-browser`, from configured architecture-specific archives, and their
version is checked against the locked Playwright metadata. Chromium libraries
are installed during the configured image build; the browser test command no
longer performs automatic Playwright/browser/apt downloads. The existing
locked Playwright version is unchanged. Automatic editor-extension downloads
were removed from the container recipe; approved editor provisioning is
documented separately.

**Checks performed:**

- **Host-only correction (November 27, 2026):** external settings now contain
  only source hosts, such as `NUGET_SOURCE=nuget.org`. Shared adapters supply
  endpoint paths and artifact versions for startup, installation, and builds.
  EF uses the configured NuGet host instead of a separate artifact URL.
  SDK/Bicep downloads follow repository pins; browser paths follow locked
  Playwright metadata. Old URL-based files are rejected. Custom mirrors must
  expose standard paths. All 55 mocked script regressions passed, including
  host validation, public/custom host resolution, architecture/version
  selection, and generated package configuration. Script/Dockerfile shell
  syntax and `git diff --check` passed. Real Docker/download gates remain
  blocked; this is not end-to-end environment validation.
- All 42 cases in `tests\Scripts\start-environment.Tests.sh` passed using
  existing Git Bash. Docker and Azure were mocked; the suite covers source
  validation, external-file boundaries, permission rejection, paired
  credentials, engine/plugin/image/restore/migration failures, Azure session
  rechecking, login failure and tenant forwarding, foreground/detached mode, interruption
  cleanup, source-environment override rejection, credential cleanup on
  success/failure, stdin container dispatch, readiness diagnostics, ownership
  isolation, opt-in installation, and explicit reset confirmation.
- Bash syntax checks passed for all four scripts and the Dockerfile build
  shell. On this Windows/NTFS test host, credential mode bits are simulated
  in the test fixture; this is not real Unix permission validation.
- Dev Container/package JSON parsing passed using PowerShell's native JSON
  parser, and `git diff --check` passed. Application type-check/build and real
  backend/frontend/browser tests require the supported container environment.

**Blocker:** Only Docker Desktop-owned WSL distributions are available; no
developer Ubuntu distribution with standalone Docker Engine is available.
No host software was installed, uninstalled, or reconfigured to work around it.

**Remaining acceptance gates:** Administrator-provided native Ubuntu and
Windows/Ubuntu WSL 2 environments must validate real image/package downloads
from approved sources, clean-checkout builds, Unix permissions, bind-mount/cache
ownership, smoke checks, migrations, mock end-to-end application/browser tests,
foreground signal/process-group cleanup, detached operation, and Windows
localhost forwarding. Validate real Azure login/deployment checks and
stop/restart versus recreation authentication behavior with authorized access.
The frontend build/type-check and existing backend/frontend suites have not
been run for this revision. These gates are blocked, not waived or passed.

### Why WSL 2 and Linux containers are used

On Windows, WSL 2 provides the Linux kernel needed to run this project's Linux
containers. Docker Engine runs directly inside a developer-managed WSL 2 Linux
distribution, with `dockerd` managed by systemd. The Docker CLI and Compose
plugin run in that same distribution and connect to its local Unix socket.
Docker Engine is the container runtime; `dockerd` is its daemon, not a separate
replacement product. A native Windows `dockerd` installation is not the runtime
for this project's Linux containers.

Open the repository through VS Code's WSL connection before reopening it in
the Development Container. Run bootstrap and Compose commands from that WSL
environment, not from a Windows Docker Desktop context. Replace the existing
Windows-only launchers with the approved shared Linux workflow; prior Windows
validation does not establish that the revised setup has passed.

Linux containers are the project default because the .NET SDK, Node.js, PostgreSQL, Azurite, Azure CLI, and Bicep support them well. They also provide a broad image ecosystem, generally use fewer resources than Windows containers, align with common GitHub Actions runners and possible Linux App Service deployment, and expose Linux-specific path, casing, permission, and shell issues during development. This is a development and testing choice; it does not require the production API to use a custom container. Production may still use either the App Service managed .NET runtime or a separate Linux production image.

### Windows 10 compatibility

WSL 2 is available on 64-bit Windows 10 version 2004 or later, build 19041 or later, when hardware virtualization is enabled. Use `winver` to check the installed version and build. Windows 10 reached the end of standard support on October 14, 2025, so a supported Windows 11 installation is preferred. A Windows 10 development machine should use an applicable Microsoft Extended Security Updates program and must support a current WSL release with systemd and a Linux distribution supported by Docker Engine. WSL 2 availability alone does not establish compatibility with the complete development setup.

### Set up Docker Engine and dockerd on Windows through WSL 2

1. Confirm that hardware virtualization is enabled in UEFI/BIOS and that the Windows version supports WSL 2.
2. During administrator-managed prerequisite setup, install or update WSL and
  Ubuntu, and verify that the
  distribution uses WSL 2. Restart Windows if required.
3. Enable systemd in that distribution following Microsoft's WSL guidance,
  restart the distribution if required, and verify systemd is running.
4. Inside Ubuntu, use the approved opt-in prerequisite installer once it is
  implemented, or administrator-managed installation using the externally
  configured signed package repository. Install
  Docker Engine and CLI, containerd, the Buildx plugin, and the Compose v2
  plugin. For Ubuntu, the official packages are `docker-ce`, `docker-ce-cli`,
  `containerd.io`, `docker-buildx-plugin`, and `docker-compose-plugin`.
  Do not install Docker Desktop or a second competing daemon.
5. Enable and start the systemd `docker` service, which runs `dockerd`. Ensure
  the developer account can access Docker using an organization-approved
  configuration. Membership in the `docker` group grants root-equivalent
  privileges; do not make the socket world-writable or expose an unsecured
  TCP daemon endpoint.
6. From a WSL terminal, verify the daemon with `systemctl status docker` and
  `docker info`, the client/server connection with `docker version`, and the
  plugins with `docker compose version` and `docker buildx version`.
7. Use the external source configuration to confirm Docker can pull and start
  an explicitly configured Linux image; do not rely on an implicit public
  test-image source.
8. Open the repository in VS Code through the WSL extension, then reopen it
  in the Development Container. Verify Compose services and the environment
  smoke test using this Docker Engine setup before recording it as validated.

The WSL distribution and its `dockerd` service must be running before opening
the Development Container or starting Docker Compose services. Published ports
belong to the WSL host; verify Windows localhost forwarding and firewall policy
before relying on Windows-browser access. Organization-approved installation,
proxy, network, and resource-allocation policies take precedence. On a native
Ubuntu host, use the same Docker Engine prerequisites without the WSL steps.

### Install and run Azurite locally

Azurite will not be installed directly on Windows or inside the Development Container. It will run from an externally configured Azurite Linux container image as a Docker Compose service. The public-source example will identify Microsoft's official image. Pin the image to a tested version rather than relying on the `latest` tag.

The planned Compose service will:

- Start the Blob, Queue, and Table endpoints.
- Publish ports `10000`, `10001`, and `10002` to the Windows host for host-based diagnostics when needed.
- Listen on `0.0.0.0` inside its container so other Compose services can connect.
- Store emulator data in a named Docker volume so it survives container recreation.
- Use a service name such as `azurite`, which is also its hostname on the Compose network.

No separate Azurite installation command is required. The unified startup script will resolve the external image configuration and start the Compose services from the repository directory in WSL or native Ubuntu. Verify them with `docker compose ps` and inspect Azurite startup failures with `docker compose logs azurite`. Docker Engine's `dockerd` service must already be running.

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
