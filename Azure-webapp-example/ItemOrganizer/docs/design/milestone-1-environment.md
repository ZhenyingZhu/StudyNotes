# Milestone 1 - Development environment

[High-level plan](../../PLAN.md) | [Environment architecture and commands](development-environment.md)

## Scope

Provide reproducible development on Windows and native Ubuntu without
installing application SDKs on the host. Both platforms use the same Linux
workspace, PostgreSQL and Azurite containers.

## Components and responsibilities

| Component | Responsibility |
|---|---|
| Windows entry scripts | Invoke the selected WSL 2 Ubuntu distribution, translate paths and supervise the shared Linux command |
| Linux entry scripts | Check/install prerequisites when explicitly requested, coordinate containers and application processes |
| Workspace image | Pinned .NET SDK, Node.js, pnpm, EF CLI, Azure CLI, Bicep and development utilities |
| PostgreSQL | Relational inventory, ownership, operation state and outbox |
| Azurite | Private photo/crop storage and asynchronous analysis queues |
| Project source manifest | Reference only external NuGet/npm source files; store all other source values directly |

The detailed dispatch, installer, configuration and lifecycle contracts are
defined in the [environment design](development-environment.md). All four
commands have Windows and Ubuntu entry points; startup never installs host
prerequisites or silently resets application data.

## Container recipe

Compose defines workspace, PostgreSQL and Azurite. Workspace mounts the project
and runs as the development user. Tools and third-party images are pinned to
repository-owned versions. Build the workspace image locally and reuse it
when compatible; do not publish it or make development depend on a prebuilt
project-image registry.

Containers reach dependencies by Compose service names (`postgres` and
`azurite`), not host loopback addresses. Expose API/frontend and emulator
diagnostic ports through the host; network/firewall policy controls access.
Verify Windows-browser access through WSL independently.

Named volumes preserve PostgreSQL and Azurite data across container
recreation. Separate package-cache volumes accelerate restores without
making clean builds depend on machine-specific state. Keep real `.env`,
credentials, database files, emulator data and generated outputs out of Git.

## Configuration and startup

The checked-in project manifest and external native source files are required inputs, not
automatic defaults. Feed/image selection is independent of repository-owned
version pins. Native tools handle downloads and authentication; build secrets
and temporary container files carry credentials without retaining them in
image layers.

Normal startup creates only missing application configuration, starts healthy
dependencies, restores packages, applies migrations and launches API/frontend.
Readiness checks precede success. Foreground interruption cleans up owned
application processes; `--detach` explicitly leaves them running.

## Testing model

- xUnit unit tests cover validation, ownership policies, assignment rules,
  analysis transitions and result mapping without live Azure dependencies.
- API integration tests use `WebApplicationFactory`, test authentication,
  PostgreSQL and Azurite with deterministic provider fixtures.
- Contract tests cover routes, OpenAPI, status codes, Problem Details,
  pagination, concurrency, idempotency and upload behavior.
- Vitest covers frontend components/client logic; Playwright exercises the
  complete local upload, review, confirmation and inventory workflow.
- Live Azure tests are explicitly invoked, isolated from the default run,
  and use authorized resources and approved fixtures.
- A production container, if separately adopted, requires independent
  runtime, non-root, readiness and graceful-shutdown tests. Workspace is
  never a production artifact.

## Acceptance criteria

On native Ubuntu and Windows/WSL 2, a clean checkout must build the workspace,
verify pinned versions, start healthy dependencies, restore packages, apply
migrations, complete mock startup and expose the application to the host
browser. A second supported machine must reproduce this without
machine-specific source defaults.

Verify data persistence, cache ownership, external configuration, credentials,
no unconfigured fallback, all four command entry points, prerequisite
installation/restart handling, foreground interruption and detached operation.
Live login and deployment checks require separately authorized Azure
validation. Mocked commands alone do not satisfy these criteria.
