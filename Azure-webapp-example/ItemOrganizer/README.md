# Item Organizer

This repository currently implements the planning outputs for Milestones 0 and 1 of the accepted `PLAN.md`.

Milestone 0 is captured in `docs/milestone-0-decisions.md`. It resolves the product, security, workflow, retention, authorization, queue/worker, and acceptance-criteria decisions that were intentionally left open during planning.

Milestone 1 establishes a reproducible local development environment based on a VS Code Development Container and Docker Compose. Its workspace toolchain and Azurite services have been verified; final SQL Server and complete smoke-test verification remains pending on a supported x64 host.

## Repository layout

- `PLAN.md` - accepted milestone plan
- `docs/milestone-0-decisions.md` - approved product and security decisions
- `.devcontainer/` - development container definition and Dockerfile
- `docker-compose.yml` - local development services
- `.env.example` - local environment template
- `scripts/smoke-test.sh` - environment smoke test for use inside the workspace container

## Host prerequisites

The only required host tools are:

- Docker Desktop configured for Linux containers
- Visual Studio Code with the Dev Containers extension
- Git

No project SDKs need to be installed directly on the host.

The workspace and Azurite images support x64 and ARM64. Microsoft's supported
SQL Server Linux image is x64-only. Use an x64 host for the complete local
stack; SQL Server 2022 is not reliable under Docker Desktop's Windows ARM64
emulation, and the retired Azure SQL Edge image is intentionally not used as a
fallback.

## Quick start

1. Copy the example environment file:

   ```powershell
   Copy-Item .env.example .env
   ```

2. Open the repository in Visual Studio Code.

3. Reopen the workspace in the Development Container.

4. Start the local dependencies:

   ```powershell
   docker compose up -d --build
   ```

5. Verify service status:

   ```powershell
   docker compose ps
   ```

6. Verify the pinned toolchain inside the workspace container:

   ```powershell
   docker compose exec workspace bash -lc "dotnet --version && dotnet ef --version && node --version && pnpm --version && az version --output json && bicep --version"
   ```

7. Run the environment smoke test:

   ```powershell
   docker compose exec workspace bash scripts/smoke-test.sh
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
- `sqlserver` - local SQL Server for Azure SQL-compatible development
- `azurite` - local Blob, Queue, and Table Storage emulator

Published host ports:

- SQL Server: `14333 -> 1433`
- Azurite Blob: `10000 -> 10000`
- Azurite Queue: `10001 -> 10001`
- Azurite Table: `10002 -> 10002`

Container-to-container traffic must use the Compose service names `sqlserver` and `azurite`, not `127.0.0.1`.

## Data persistence

The environment uses named Docker volumes:

- `sqlserver-data`
- `azurite-data`
- `nuget-cache`
- `pnpm-store`

The SQL Server and Azurite volumes preserve local state across container recreation. The cache volumes speed up repeated package restores without depending on host-specific paths.

## Smoke-test expectations

The smoke test passes when:

- the pinned toolchain is available in `workspace`
- `sqlserver` is reachable by service name on port `1433`
- `azurite` is reachable by service name on ports `10000`, `10001`, and `10002`
- the committed connection strings use `sqlserver` and `azurite` instead of `127.0.0.1`

## Milestone 1 validation status

Validation performed on September 15, 2026 confirmed:

- the Development Container image builds on Windows ARM64
- the pinned workspace tool versions are available
- Azurite becomes healthy and is reachable by service name
- Compose configuration and local-secret ignore rules are valid

The complete smoke test cannot pass on the current Windows ARM64 validation
machine because Microsoft's supported SQL Server 2022 Linux image is x64-only
and terminates under Docker Desktop's emulation. Milestone 1 is not complete
until the stack passes on a supported x64 host and a second supported machine
reproduces the environment from a clean checkout.

## Next implementation milestone

After Milestone 1 completes its x64 and second-machine validation, the next
step is Milestone 2: the domain model and persistence foundation.
