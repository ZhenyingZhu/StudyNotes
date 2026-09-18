[CmdletBinding()]
param(
    [switch]$NoBuild,
    [switch]$SkipSmokeTest
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Invoke-CheckedCommand {
    param(
        [Parameter(Mandatory)]
        [string]$Command,

        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    Write-Host "> $Command $($Arguments -join ' ')" -ForegroundColor Cyan
    & $Command @Arguments

    if ($LASTEXITCODE -ne 0) {
        throw "$Command failed with exit code $LASTEXITCODE."
    }
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$environmentPath = Join-Path $projectRoot ".env"
$environmentExamplePath = Join-Path $projectRoot ".env.example"
$locationPushed = $false

try {
    if ($env:OS -ne "Windows_NT") {
        throw "This bootstrap script supports Windows only."
    }

    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw "Docker CLI was not found. Install and start Docker Desktop, then retry."
    }

    $dockerOs = & docker version --format "{{.Server.Os}}" 2>$null
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($dockerOs)) {
        throw "Docker Desktop is not running or its Linux engine is unavailable."
    }

    if ($dockerOs.Trim() -ne "linux") {
        throw "Docker Desktop is using Windows containers. Switch it to Linux containers and retry."
    }

    & docker compose version *> $null
    if ($LASTEXITCODE -ne 0) {
        throw "Docker Compose v2 is unavailable. Update Docker Desktop and retry."
    }

    if (-not (Test-Path -LiteralPath $environmentExamplePath -PathType Leaf)) {
        throw "Missing environment template: $environmentExamplePath"
    }

    if (-not (Test-Path -LiteralPath $environmentPath -PathType Leaf)) {
        Copy-Item -LiteralPath $environmentExamplePath -Destination $environmentPath
        Write-Host "Created .env from .env.example." -ForegroundColor Green
    }
    else {
        Write-Host "Using existing .env without modifying it." -ForegroundColor DarkGray
    }

    Push-Location $projectRoot
    $locationPushed = $true

    Invoke-CheckedCommand "docker" @("compose", "config", "--quiet")

    $upArguments = @("compose", "up", "-d", "--wait", "--remove-orphans")
    if (-not $NoBuild) {
        $upArguments += "--build"
    }

    Invoke-CheckedCommand "docker" $upArguments

    if (-not $SkipSmokeTest) {
        Invoke-CheckedCommand "docker" @(
            "compose",
            "exec",
            "-T",
            "workspace",
            "bash",
            "scripts/smoke-test.sh"
        )
    }

    Write-Host ""
    Write-Host "Item Organizer environment is ready." -ForegroundColor Green
    Invoke-CheckedCommand "docker" @("compose", "ps")
}
catch {
    Write-Error $_
    exit 1
}
finally {
    if ($locationPushed) {
        Pop-Location
    }
}
