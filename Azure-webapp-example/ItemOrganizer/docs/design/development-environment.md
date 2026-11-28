# Development environment

[High-level plan](../../PLAN.md) | [Milestone 1](milestone-1-environment.md)

## Scope

Provide the same containerized development workflow from Windows PowerShell
and native Ubuntu. Four commands cover prerequisite installation, application
startup, environment checks and explicit database reset. The project manifest
references external NuGet/npm files and directly selects other sources;
package managers perform their own downloads.

## Runtime architecture

| Concern | Windows | Native Ubuntu |
|---|---|---|
| User command entry | Windows PowerShell | Ubuntu terminal |
| Entry scripts | Thin `.ps1` launchers | Shared `.sh` scripts |
| Docker Engine host | Ubuntu inside WSL 2 | Ubuntu directly |
| Linux container execution | Existing WSL distribution is invoked automatically | Direct Docker invocation |

```text
Windows PowerShell -> .ps1 launcher -> WSL 2 Ubuntu -> shared .sh script
Native Ubuntu terminal ----------------------------> shared .sh script
                                                           |
                                                     Docker Engine
                                                           |
                          +--------------------------------+-----------+
                          |                                |           |
                     Workspace                         PostgreSQL   Azurite
                 tools, API, frontend                   metadata   blobs/queue
```

The Windows launcher invokes WSL automatically. The user does not open an
Ubuntu terminal first. Docker Engine runs Linux containers on both platforms;
WSL supplies the Linux kernel on Windows. Docker Desktop and native Windows
container images are outside the design.

Both platforms share the Debian-based workspace, PostgreSQL and Azurite
containers. SDKs, restores, migrations, builds, tests, API and frontend servers
run inside workspace, not on the host. Windows launchers dispatch to the
shared Linux logic rather than duplicating it.

Ubuntu is the Docker host, not the distribution inside every container.
The workspace and PostgreSQL images provide Debian userspace and share the
Linux host kernel. Docker Engine is not installed inside workspace, and
container/host distributions do not need to match.

### Windows dispatch contract

Each `.ps1` entry selects the Ubuntu distribution, translates Windows paths,
and invokes the matching `.sh` command with the original arguments.
Linux absolute paths remain unchanged. Paths with spaces and argument values
must be forwarded as literal values, not interpolated into executable shell
text. The Linux command's output and exit status are returned to PowerShell.

`--distribution` selects an existing Ubuntu distribution. Without explicit
selection, use the sole eligible Ubuntu WSL 2 distribution. If selection is
ambiguous, stop and request an explicit distribution; do not choose another
host or Docker context silently.

Only the prerequisite installer may provision a missing WSL/Ubuntu
environment. Other commands reject missing prerequisites before performing
application work.
The prerequisite `.ps1` entry also owns Windows npm-helper provisioning;
the shared Linux installer provisions Docker and the workspace NuGet provider.

## Prerequisite installation

Installation is a separate, explicit administrator-approved operation.
Normal startup never provisions prerequisites, changes host tooling, grants
Docker access, or escalates privileges.

On Windows, the opt-in installer:

- Sets up WSL 2 and Ubuntu when missing, then installs standalone Docker
  Engine and its required CLI, Compose and Buildx components inside Ubuntu.
- Reuses an existing Ubuntu WSL 2 distribution. Installs Ubuntu 24.04 LTS
  only when none exists. Requires selection when several distributions exist;
  it never guesses, replaces or deletes an existing distribution.
- Stops with restart instructions if Windows needs a restart. It never
  restarts Windows automatically; the user restarts and reruns the installer.
- Reports Windows elevation, Ubuntu account initialization, systemd, Docker
  service/access and source failures explicitly. It does not silently grant
  daemon access or make sockets world-writable.
- Provisions the Windows-only `vsts-npm-auth` executable for npm feed
  authentication. This is an explicit host authentication-tool exception;
  it does not install host Node.js, npm, pnpm or the .NET SDK. Provision the
  packaged executable and its dependencies from approved installation
  inputs, and report unsupported Windows runtime prerequisites explicitly.

On native Ubuntu, the opt-in installer installs Docker Engine directly.
Installing or reconfiguring project SDKs on the host is not part of setup.
On both platforms, it provisions the Azure Artifacts Credential Provider
inside the Debian workspace, where NuGet restores and EF-tool installation
run. Installing the provider only on Windows or the WSL host is insufficient.
Native Ubuntu does not install or attempt to run `vsts-npm-auth`.
Normal startup uses the selected, already provisioned environment and reports
missing prerequisites without invoking the installer.

The installation flow is:

1. Require `--install`; validate the project manifest, referenced NuGet/npm
   files and administrative prerequisites before changing the host.
2. On Windows, inspect WSL and Ubuntu. Reuse the selected distribution, or
   provision the missing platform/distribution from configured installation
   inputs. If a Windows restart is required, stop and report that installation
   must resume after the user restarts.
3. Inside the selected Ubuntu environment, check systemd and existing Docker
   packages. Report incompatible installations rather than uninstalling them.
4. Generate native apt repository configuration from the project manifest,
   provision the configured signing keys, and use apt to install
   Docker Engine, CLI, containerd, Compose and Buildx; start the Docker service.
5. Verify the Engine connection and plugins. Report any remaining
   administrator-managed Docker access requirements.
6. On Windows, provision and verify the npm authentication helper. On both
   platforms, build or prepare the workspace tool environment with the
   NuGet credential provider available before any authenticated package
   installation. Verify provider discovery as the workspace development user.
7. Report helper installation and feed authorization separately. If sign-in
   or token refresh is required, report the explicit authentication steps;
   do not claim private feeds are accessible merely because tools exist.

Rerunning after a restart or partial installation inspects existing state and
continues without recreating distributions or deleting data. Installation
inputs are explicit; a native installer's unconfigured download defaults are
not a fallback.

### Credential-helper provisioning

| Component | Installation location | Responsibility |
|---|---|---|
| Azure Artifacts Credential Provider | Workspace development user's NuGet plugin/tool discovery location | Acquire/cache credentials for workspace NuGet operations |
| `vsts-npm-auth` | Windows user-accessible tool location | Acquire/refresh npm credentials in the external file referenced by `NPM_CONFIG` |

Helper versions and integrity checks are repository-owned pins. Bootstrap
download inputs belong directly in `sources.conf`, not another external
source file. Helper provisioning must work before private-feed authorization;
do not require an unauthenticated private feed to install its own
authentication helper or silently switch to an unconfigured public feed.
Validate installation inputs, expected executable packaging and runtime
requirements before provisioning.

The installer prepares tools, not application startup or database changes.
Rerunning reuses compatible helper installations and preserves user-edited
NuGet/npm files and credential caches. Authentication runs as the normal
Windows/workspace development user, not the elevated installer account.

## Four script responsibilities

| Script pair | Responsibility |
|---|---|
| `install-prerequisites.ps1` / `.sh` | Explicit `--install` Docker/platform and workspace NuGet-provider provisioning; Windows additionally provisions missing WSL/Ubuntu and the npm authentication helper |
| `start-environment.ps1` / `.sh` | Complete non-destructive startup: check prerequisites, prepare configuration, build/reuse images, start services, restore packages, apply migrations, start API/frontend, verify readiness |
| `smoke-test.ps1` / `.sh` | Check existing workspace tools and dependency connectivity; no installation, service startup or database reset |
| `reset-and-seed.ps1` / `.sh` | Separate destructive database reset and representative seeding; requires `--confirm` |

All four have Windows entry points. The smoke-test entry must dispatch into
workspace; checking SDKs on Windows or the WSL host would test the wrong layer.

Startup runs in the foreground by default. Ctrl+C stops its API/frontend,
leaving database/storage containers and data intact. Explicit `--detach`
leaves application servers running after startup exits. Failures clean up
newly started servers without stopping unrelated processes. Routine startup
applies migrations; it never silently resets data.

## Application startup sequence

1. Load the project manifest and validate the inputs needed for the selected
   mode. On Windows, dispatch to the selected existing WSL 2 distribution.
2. Check Ubuntu, Docker Engine, Compose and Buildx. Missing prerequisites
   produce an error directing the user to the separate installer.
3. Create missing local application configuration without overwriting
   existing files. Resolve repository-pinned image/tool versions and the
   manifest-selected sources.
4. Build or reuse the local workspace image and start workspace, PostgreSQL
   and Azurite. Wait for service health before continuing.
5. Run workspace smoke checks. Restore backend packages through NuGet and
   frontend packages through pnpm, using the referenced native configurations.
6. In live mode, verify container-side Azure sign-in and deployment readiness.
   Apply database migrations without resetting application data.
7. Start API/frontend, wait for their readiness endpoints, and return their
   addresses. Retain ownership information for cleanup.
8. Remain attached until interrupted, unless `--detach` was requested.

The workspace image is built and reused locally; no project-image publishing
or prebuilt project-image registry dependency is introduced. `--no-build`
requires an existing compatible workspace image and does not skip package
restore, migrations or readiness checks.

## Failure and lifecycle behavior

Any prerequisite, source, restore, migration or readiness failure returns a
nonzero exit status and identifies the failed step. Do not continue to the
next step or report success with missing dependencies.

Cleanup removes temporary credentials and only the servers started or owned
by that invocation. It does not reset data, remove distributions, or terminate
unrelated processes. A foreground interrupt propagates through the Windows
launcher to the shared Linux command so the same cleanup runs on both hosts.
Detached success releases the launcher while retaining the application
servers; detached startup failure still performs cleanup.

## Source configuration

The manifest selects configuration; the native tools interpret sources and
download packages. NuGet restores application packages and installs the pinned
EF tool through the same external `NuGet.Config`. pnpm reads the external
`.npmrc`. Scripts do not derive NuGet endpoints or retrieve EF packages
through a custom HTTP implementation.

The checked-in project `sources.conf` is the active manifest. It references
only external `NuGet.Config` and `.npmrc` files by absolute path. All other
source values are stored directly in the manifest. There are no required
separate `--nuget-config` or `--npm-config` parameters. The manifest is checked
into the repository;
actual native configuration files and credentials remain outside it.

Manifest entries for package managers and image registries:

```text
NUGET_CONFIG=D:\ItemOrganizerConfig\NuGet.Config
NPM_CONFIG=D:\ItemOrganizerConfig\.npmrc
UBUNTU_APT_ARCHIVE_URI=https://archive.ubuntu.com/ubuntu/
UBUNTU_APT_SECURITY_URI=https://security.ubuntu.com/ubuntu/
UBUNTU_APT_PORTS_URI=https://ports.ubuntu.com/ubuntu-ports/
WORKSPACE_APT_URI=https://deb.debian.org/debian
WORKSPACE_APT_SECURITY_URI=https://deb.debian.org/debian-security
DOCKER_APT_URI=https://download.docker.com/linux/ubuntu
AZURE_CLI_APT_URI=https://packages.microsoft.com/repos/azure-cli/
DOCKER_SIGNING_KEY_URI=https://download.docker.com/linux/ubuntu/gpg
AZURE_CLI_SIGNING_KEY_URI=https://packages.microsoft.com/keys/microsoft.asc
WORKSPACE_REGISTRY=mcr.microsoft.com
POSTGRES_REGISTRY=docker.io
AZURITE_REGISTRY=mcr.microsoft.com
```

This is an excerpt. Installation and standalone archive inputs are also
explicit entries in the project manifest. Each operation validates its
required inputs before performing downloads or host/application changes.

Native configuration references point to files, not endpoint names. Registry
entries identify Docker registry hosts; repository-owned image names and
versions determine which images Docker pulls. NuGet/npm feed URLs belong in
their external native files. apt repository URIs, signing-key URIs and all
installer/archive sources belong directly in the manifest. No other external
source configuration files are accepted.

The installer/build generates native apt configuration from the manifest.
Suites, components, architectures and local signing-key paths follow the
host release or pinned workspace image; source changes do not change these
platform selections. Use Ubuntu archive/security sources for amd64 and the
ports source for arm64. Provision Docker/Microsoft keys from their configured
URIs before apt uses the corresponding local `Signed-By` paths.
Only configured apt repositories participate in project installation; do not
fall back to inherited feeds. Generating apt configuration does not replace
apt's metadata, dependency, authentication or download handling.

| Input | Source/download mechanism |
|---|---|
| Application NuGet packages and EF tool | Native external `NuGet.Config`; `dotnet restore` and `dotnet tool install` select/download packages |
| Frontend packages | Native external `.npmrc`; pnpm handles registry metadata and packages |
| Ubuntu/Debian packages, Docker installation, Azure CLI | Manifest repository/key URIs generate native apt configuration; apt handles packages |
| Container images | Docker handles pulls from configured registries using repository-pinned image names/versions |
| WSL/Ubuntu setup | Supported native installation mechanisms using explicit manifest inputs |
| Standalone SDK/Bicep/browser inputs | Supported installers with configurable sources when available; otherwise a simple pinned-file download, not a custom package resolver |

For example, the external `NuGet.Config` could contain this public-only source:

```xml
<configuration>
  <packageSources>
    <clear />
    <add key="approved" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

The external `.npmrc` could contain:

```ini
registry=https://registry.npmjs.org/
strict-ssl=true
```

These examples contain no credentials. Developers configure their environment's
approved sources in the external files.

NuGet/npm examples in [docs/source-examples](../source-examples/README.md)
contain public sources only and must be explicitly copied and configured
outside the checkout. There is no separate manifest example. Runtime selection
never falls back to an example, hardcoded service, inherited environment override or
unconfigured source after a download/authentication failure.

Keep artifact versions pinned in the repository. Changing source configuration
must not silently change versions. Let package managers handle metadata,
dependencies, authentication and downloads. Native feed metadata and redirects
still require approved proxy/network policy; URL syntax alone is not source
allowlisting.

## Credentials and Azure authentication

Public feeds normally need no credentials. Private feeds may use credentials
in their native configuration or a supported external credential mechanism.
Any credential-bearing file must be protected and outside the repository.
The manifest contains source values and NuGet/npm paths, never passwords or
tokens. Credentials for other sources use supported external authentication
files, separate from source selection. Package-token prompts
and secret command arguments are not part of setup.

Use appropriate file-based secret handling for builds and containers.
Credentials must not enter `.env`, logs, image layers or build arguments.
Remove transferred temporary credential inputs on success and failure.
Windows source paths must be translated without weakening Linux credential
permission checks; Linux-native credential paths remain usable from Windows.

### Package-feed authentication

NuGet uses the workspace-installed Azure Artifacts Credential Provider.
Initial authorization is an explicit interactive NuGet operation using the
mounted external `NuGet.Config`. Browser/device-code sign-in must work with
the host browser and the account authorized for the feed; Windows sign-in
and Windows credential caches do not automatically authenticate Linux.
Subsequent operations use the provider's cache until authorization expires.
Protect caches under the workspace development user, outside image layers;
do not rely on an unprotected Linux MSAL cache fallback.

On Windows, explicitly acquire or refresh npm credentials with the installed
helper, using the same external file for source selection and token output:

```powershell
vsts-npm-auth -Config D:\ItemOrganizerConfig\.npmrc -TargetConfig D:\ItemOrganizerConfig\.npmrc -ReadOnly
```

Use the actual `NPM_CONFIG` path when it differs from this example. Preserve
existing source settings; do not silently write tokens into an unrelated
Windows user `.npmrc`. The generated file contains credentials, not merely
registry settings; protect it and transfer it to workspace through secret
handling. Linux npm/pnpm reads these credentials without running the Windows
helper. Refresh on Windows when tokens expire. Do not treat base64 encoding
as encryption.

Native Ubuntu uses feed-authorized credentials in a protected external
`.npmrc`, following the Azure Artifacts npm setup. The Windows helper is not
a Linux credential provider, and NuGet's provider does not automatically
authenticate npm. Normal startup does not install helpers or prompt for
package tokens; missing/expired authorization produces an actionable error.

Validate authentication with real native NuGet and npm/pnpm package metadata
requests or restores inside workspace. Check reported feed errors as well
as process exit status: a package search can exit zero while reporting a
source failure. HTTP 401/403, token expiry, insufficient feed permissions
and TLS/proxy failures must be distinguished. Neither login nor helper
installation bypasses certificate validation or grants feed permissions.

### Azure resource authentication

Azure resource sign-in is separate from package/download authentication.
Live startup may use container-side Azure device-code browser login.
Authentication may survive stop/start of the same container, subject to
expiry, but is not baked into an image or persisted across recreation.

Mock AI is the startup default. Live AI requires explicit mode selection,
Azure authentication and deployment-readiness checks, and a clear warning
that photos are sent to Azure OpenAI and usage charges apply. Explicit
review remains mandatory in either mode.

## Command interface

`sources.conf` must contain every required input for the selected operation.

From Windows PowerShell:

```powershell
.\scripts\install-prerequisites.ps1 --install --sources .\sources.conf
.\scripts\start-environment.ps1 --sources .\sources.conf --mode mock
.\scripts\smoke-test.ps1
.\scripts\reset-and-seed.ps1 --confirm
```

From native Ubuntu:

```bash
sudo bash scripts/install-prerequisites.sh --install --sources ./sources.conf
bash scripts/start-environment.sh --sources ./sources.conf --mode mock
bash scripts/smoke-test.sh --host
bash scripts/reset-and-seed.sh --confirm
```

The Ubuntu installer requires appropriate administrator invocation, such as
`sudo`; normal startup does not. Windows installation may request Windows
elevation and Ubuntu administrator authentication only during opt-in setup.
Use `--distribution` on Windows to explicitly select an existing Ubuntu
distribution when needed. Application startup supports `--no-build` to reuse
the local workspace image and `--detach` for explicit background operation.

## Acceptance gates

- Test all four exact Windows commands and native Ubuntu counterparts, path
  translation, spaces/literal arguments, exit codes and prerequisite failures.
- Validate opt-in provisioning, existing-distribution reuse, ambiguous
  selection, restart/resume, administrator/account boundaries and no silent
  host changes during normal startup.
- Verify native source configuration, credentials, pinned versions, missing
  inputs, unavailable sources and absence of unconfigured fallback.
- Verify pinned helper installation and rerun behavior, Windows executable
  provisioning without host SDK/npm installation, workspace NuGet provider
  discovery, and native Ubuntu's absence of the Windows helper.
- Validate authorized private-feed access from workspace, first sign-in,
  expired-token refresh, Windows npm-file transfer and protected cache/file
  handling. Test denied permissions, HTTP 401/403 and TLS/proxy failures
  without exposing credentials or treating a zero exit code as sufficient.
- On both supported platforms, verify clean-checkout builds, healthy services,
  restores, migrations, non-destructive startup and browser access.
- Verify real Ctrl+C/process cleanup, detached operation, ownership and
  secret cleanup, including failure/interruption cases.
- Validate live Azure login/deployment checks and stop/restart versus
  recreation authentication separately with authorized Azure access.

Passing mocked dispatch or syntax checks does not satisfy runtime acceptance.
