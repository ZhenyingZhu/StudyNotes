# Item Organizer

This repository currently implements the planning outputs for Milestones 0 and 1 of the accepted `PLAN.md`.

Milestone 0 is captured in `docs/milestone-0-decisions.md`. It resolves the product, security, workflow, retention, authorization, queue/worker, and acceptance-criteria decisions that were intentionally left open during planning.

Milestone 1 establishes a reproducible local development environment based on a VS Code Development Container and Docker Compose. The stack uses PostgreSQL so the complete local environment can run on both x64 and ARM64 hosts.

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

The workspace, PostgreSQL, and Azurite images support x64 and ARM64.

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

After Milestone 1 completes its second-machine validation, the next step is
Milestone 2: the domain model and persistence foundation.
