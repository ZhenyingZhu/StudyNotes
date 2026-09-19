# Item Organizer

This repository implements the planning and development-environment outputs for
Milestones 0 and 1 and the domain and persistence foundation for Milestone 2 of
the accepted `PLAN.md`.

Milestone 0 is captured in `docs/milestone-0-decisions.md`. It resolves the product, security, workflow, retention, authorization, queue/worker, and acceptance-criteria decisions that were intentionally left open during planning.

Milestone 1 establishes a reproducible local development environment based on a VS Code Development Container and Docker Compose. The stack uses PostgreSQL so the complete local environment can run on both x64 and ARM64 hosts.

## Repository layout

- `PLAN.md` - accepted milestone plan
- `docs/milestone-0-decisions.md` - approved product and security decisions
- `.devcontainer/` - development container definition and Dockerfile
- `docker-compose.yml` - local development services
- `.env.example` - local environment template
- `scripts/start-environment.ps1` - Windows environment bootstrap
- `scripts/reset-and-seed.ps1` - deterministic local database reset and seed
- `scripts/smoke-test.sh` - environment smoke test for use inside the workspace container
- `src/ItemOrganizer.Domain/` - inventory domain model and state transitions
- `src/ItemOrganizer.Infrastructure/` - EF Core PostgreSQL mappings and migrations
- `src/ItemOrganizer.Database/` - migration and seed command-line entry point
- `tests/` - domain and PostgreSQL integration tests

## Host prerequisites

The only required host tools are:

- Docker Desktop configured for Linux containers
- Visual Studio Code with the Dev Containers extension
- Git

No project SDKs need to be installed directly on the host.

The workspace, PostgreSQL, and Azurite images support x64 and ARM64.

## Quick start

On Windows, run the bootstrap from PowerShell:

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

If an organization intercepts HTTPS traffic, pass its trusted PEM certificate
to BuildKit without copying it into the repository:

```powershell
docker build --secret id=custom_ca,src=C:\path\to\organization-ca.crt -t itemorganizer-workspace -f .devcontainer\Dockerfile .
docker compose up -d --no-build
```

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

Run the Milestone 2 domain and PostgreSQL integration tests:

```powershell
docker compose exec -T workspace dotnet test ItemOrganizer.sln
```

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

## Next implementation milestone

Milestone 2 now provides the approved schema, state transitions, ownership
constraints, optimistic concurrency, transactional analysis-result persistence,
and deterministic seed data. Milestone 1 still requires clean-checkout
validation on a second machine before its exit criteria are formally complete.

The next implementation step is Milestone 3: read-only APIs and authorization.
