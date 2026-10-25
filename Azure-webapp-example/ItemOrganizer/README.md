# Item Organizer

Item Organizer helps you remember what you own and where you put it. Place items into containers, record their locations, and search your inventory when you need something.

Add items manually or upload a photo for AI-assisted identification. Review and correct the suggestions before confirming them into inventory; confirmed items can include private photo crops to help you recognize them.

## How to use it

For example, record a tent and a headlamp in "Camping gear", with the
container's location set to "Garage, top shelf". Search for the headlamp later
to find its recorded container and location.

1. **Create containers.** Give each one a name and location; add descriptions
   and labels when useful.
2. **Add items.** Enter names, quantities, and categories manually, or use
   **Photo analysis** to upload a photo.
3. **Review photo suggestions.** Correct names, quantities, categories, and
   container assignments. Each item shows a labeled box on the source photo:
   drag the box to move it or its corner to resize it, rather than entering
   crop coordinates. You can also focus the box or corner and use arrow keys,
   or add or remove a crop. Then explicitly confirm the items.
4. **Find your belongings.** Search the inventory or browse a container's items.
   Keep records up to date when you move things.

AI suggestions are not guaranteed accurate and never become inventory items
automatically. The default development setup uses **mock detections**, not
actual photo recognition. Use live Azure OpenAI analysis to identify real
objects.

## Quick start: try the workflow with mock analysis

### Requirements

Use an existing Windows setup with Git, WSL 2, Docker Desktop running Linux
containers, and VS Code with the Dev Containers extension. The development
images support x64 and ARM64. Project SDKs, builds, and servers run in the
workspace container; do not install or reconfigure host tooling for project work.

**Check package access before starting.** The checked-in setup downloads backend
packages and the EF tool from public nuget.org, and frontend packages from an
authenticated Azure DevOps npm feed. For that feed, you need account read access
and a PAT with **Packaging: Read** scope.

If these sources are unavailable or prohibited in your environment, read
[package-source setup](docs/DEVELOPMENT.md#choose-the-package-sources-for-your-environment)
first. Restricted-network bootstrap is not fully documented or automatically
configured; do not run it unchanged or bypass your environment's restrictions.

### Start and explore

Run this in PowerShell from the Item Organizer directory on the Windows host:

```powershell
.\scripts\start-environment.ps1
```

The bootstrap creates missing local configuration files without overwriting
existing ones, starts the Docker services, restores dependencies, applies
database migrations, and starts the API and frontend. It securely prompts for
the feed PAT if frontend dependencies are missing. Keep tokens out of source
control, `.env`, command arguments, and screenshots.

With the default example configuration, photo analysis returns predictable
mock suggestions and makes no paid AI calls. Existing configuration is
preserved, so a previously configured checkout may use a different provider.

Open **http://localhost:5173**. Create a container and add an item, then search
for it. To explore photo review, open **Photo analysis**, choose a photo, and
select **Upload and analyze**. Review the suggestions, assign containers, and
confirm. In mock mode, the suggestions do not describe the actual photo.

Photos must be JPEG, PNG, or WebP, no larger than 10 MiB, and 512-8000 pixels
on each side.

### Stop and restart

Stop the local services without deleting stored data:

```powershell
docker compose stop
```

To start again with the existing workspace image:

```powershell
.\scripts\start-environment.ps1 -NoBuild
```

If startup fails, use the [troubleshooting guide](docs/DEVELOPMENT.md#troubleshooting-identify-the-failing-step-first)
to identify the failing step before changing credentials or configuration.

## Enable real photo identification

Complete the quick-start setup and package restore first. Stop the mock
services with `docker compose stop` before starting live mode.

You need access to an Azure OpenAI resource and a ready vision-capable model
deployment. Supply your resource details in PowerShell:

```powershell
.\scripts\start-live-ai.ps1 `
    -TenantId <your-tenant-id> `
    -Endpoint https://<your-resource-name>.cognitiveservices.azure.com/ `
    -DeploymentName <your-deployment-name> `
    -ResourceGroup <your-resource-group> `
    -ResourceName <your-resource-name>
```

Use the deployment name assigned in Azure, not just the model family. If the
workspace container needs an Azure session, the script starts device-code
login: follow the displayed URL and enter the code in your browser. This
authenticates the container, separately from host login and package-feed access.

The script checks the deployment, restores backend packages, applies migrations,
and starts the application at **http://localhost:5173**. Use the same photo
upload and review workflow described above.

**Live analysis sends uploaded photos to Azure OpenAI and incurs Azure usage
charges.** Photos and crops remain private within the application's storage
and access controls; review the contents before sending them to the AI service.

Press **Ctrl+C** in the live-startup terminal to stop its API and frontend.
Run `docker compose stop` to stop the remaining local services.
See [live startup details](docs/DEVELOPMENT.md#start-with-real-photo-identification)
for validation-only mode and script defaults.

## Development and reference

The frontend is React with TypeScript; the API is ASP.NET Core with PostgreSQL.
Local photo and queue storage uses Azurite. The example configuration enables a
local development identity; it is not suitable for a deployed environment.
Microsoft Entra sign-in requires additional configuration.

| Need | Documentation |
|---|---|
| Package sources, feed credentials, and container setup | [Environment setup](docs/DEVELOPMENT.md#before-you-start) |
| Startup options and certificate configuration | [Startup details](docs/DEVELOPMENT.md#start-the-application) |
| Diagnose Docker, restore, TLS, or Azure failures | [Troubleshooting](docs/DEVELOPMENT.md#troubleshooting-identify-the-failing-step-first) |
| Restore packages, run servers, manage migrations, or run tests | [Everyday development](docs/DEVELOPMENT.md#everyday-development) |
| Service addresses and environment settings | [Configuration](docs/DEVELOPMENT.md#configuration) |
| Photo-analysis endpoints | [Photo-analysis API](docs/DEVELOPMENT.md#photo-analysis-api); full development OpenAPI at http://localhost:5000/openapi/v1.json |
| Find source code and scripts | [Repository map](docs/DEVELOPMENT.md#repository-map) |
| Product decisions, milestone status, recognition-quality gates, and validation history | [PLAN.md](PLAN.md) |
