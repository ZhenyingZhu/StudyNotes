[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
Push-Location $projectRoot

try {
    & docker compose up -d --wait workspace
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to start PostgreSQL."
    }

    & docker compose exec -T workspace dotnet run `
        --project src/ItemOrganizer.Database `
        --no-restore `
        -- reset-and-seed
    if ($LASTEXITCODE -ne 0) {
        throw "Database reset and seed failed."
    }
}
finally {
    Pop-Location
}
