[CmdletBinding()]
param(
    [string]$Registry = "https://pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/npm/registry/",
    [string]$PatPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$credential = $null
$plainTextPat = $null
$encodedPat = $null
$locationPushed = $false
$containerCredentialPath = $null

try {
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw "Docker CLI was not found. Install and start Docker Desktop, then retry."
    }

    Push-Location $projectRoot
    $locationPushed = $true

    $workspaceId = & docker compose ps --status running --quiet workspace
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($workspaceId)) {
        throw "The workspace container is not running. Start the development environment first."
    }

    if ([string]::IsNullOrWhiteSpace($PatPath)) {
        $credential = Read-Host `
            "Azure DevOps PAT with Packaging Read permission" `
            -AsSecureString
        $plainTextPat = [System.Net.NetworkCredential]::new(
            "",
            $credential).Password
    }
    else {
        if (-not (Test-Path -LiteralPath $PatPath -PathType Leaf)) {
            throw "PAT file was not found."
        }

        $plainTextPat = (Get-Content -LiteralPath $PatPath -Raw).Trim()
    }
    if ([string]::IsNullOrWhiteSpace($plainTextPat)) {
        throw "A PAT is required."
    }

    $encodedPat = [Convert]::ToBase64String(
        [Text.Encoding]::UTF8.GetBytes($plainTextPat))
    $containerCredentialPath =
        "/tmp/itemorganizer-npm-auth-$([Guid]::NewGuid().ToString('N'))"

    $containerScript = @'
set -eu

credential_path="$1"
encoded_pat="$(cat "$credential_path")"

npmrc="$HOME/.npmrc"
backup="$(mktemp)"
had_npmrc=false

cleanup() {
    rm -f "$credential_path"
    if [ "$had_npmrc" = true ]; then
        cat "$backup" > "$npmrc"
    else
        rm -f "$npmrc"
    fi
    rm -f "$backup"
}
trap cleanup EXIT HUP INT TERM

if [ -f "$npmrc" ]; then
    cat "$npmrc" > "$backup"
    had_npmrc=true
fi

umask 077
printf '%s\n' \
    '//pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/npm/registry/:username=azure' \
    "//pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/npm/registry/:_password=${encoded_pat}" \
    '//pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/npm/registry/:email=npm@example.invalid' \
    '//pkgs.dev.azure.com/msazure/one/_packaging/one_PublicPackages/npm/registry/:always-auth=true' \
    >> "$npmrc"

cd /workspace/web
pnpm install
'@
    $containerScript = $containerScript -replace "`r", ""

    Write-Host "Restoring frontend packages from the approved Azure DevOps feed." `
        -ForegroundColor Cyan

    $encodedPat | & docker compose exec -T workspace sh -c `
        "umask 077; cat > '$containerCredentialPath'"
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to transfer temporary feed credentials to the container."
    }

    $containerScript | & docker compose exec -T `
        -e "ITEMORGANIZER_NPM_REGISTRY=$Registry" `
        workspace sh -c `
        "tr -d '\r' | sh -s -- '$containerCredentialPath'"
    if ($LASTEXITCODE -ne 0) {
        throw "Frontend package restore failed."
    }

    Write-Host `
        "Frontend packages restored; temporary feed credentials were removed." `
        -ForegroundColor Green
}
finally {
    $plainTextPat = $null
    $encodedPat = $null
    $credential = $null

    if (-not [string]::IsNullOrWhiteSpace($containerCredentialPath)) {
        & docker compose exec -T workspace `
            rm -f $containerCredentialPath 2>$null
    }

    if ($locationPushed) {
        Pop-Location
    }
}
