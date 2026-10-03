[CmdletBinding()]
param(
    [string]$Endpoint =
        "https://zhenying-gpt4o-resource.cognitiveservices.azure.com/",
    [string]$DeploymentName = "gpt-5.4-mini-itemorganizer-dev",
    [string]$ResourceGroup = "rg-zhenying-gpt4o",
    [string]$ResourceName = "zhenying-gpt4o-resource",
    [switch]$ValidateOnly,
    [switch]$ExitAfterReady
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Invoke-CheckedCommand {
    param(
        [Parameter(Mandatory)]
        [string]$Command,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [switch]$DiscardOutput
    )

    if ($DiscardOutput) {
        & $Command @Arguments *> $null
    }
    else {
        & $Command @Arguments
    }

    if ($LASTEXITCODE -ne 0) {
        throw "$Command failed with exit code $LASTEXITCODE."
    }
}

function Wait-ForHttp {
    param(
        [Parameter(Mandatory)]
        [Uri]$Uri,

        [Parameter(Mandatory)]
        [System.Diagnostics.Process]$Process,

        [int]$TimeoutSeconds = 120
    )

    $deadline = [DateTimeOffset]::UtcNow.AddSeconds($TimeoutSeconds)
    while ([DateTimeOffset]::UtcNow -lt $deadline) {
        if ($Process.HasExited) {
            throw "A development server exited before $Uri became ready."
        }

        try {
            $response = Invoke-WebRequest `
                -UseBasicParsing `
                -Uri $Uri `
                -TimeoutSec 3
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 500) {
                return
            }
        }
        catch {
            Start-Sleep -Seconds 2
        }
    }

    throw "Timed out waiting for $Uri."
}

function Stop-DevelopmentProcess {
    param(
        [System.Diagnostics.Process]$Process
    )

    if ($null -ne $Process -and -not $Process.HasExited) {
        Stop-Process -Id $Process.Id -Force -ErrorAction SilentlyContinue
        [void]$Process.WaitForExit(5000)
    }
}

function Start-DockerProcess {
    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    $startInfo = [System.Diagnostics.ProcessStartInfo]::new()
    $startInfo.FileName = "docker"
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true
    foreach ($argument in $Arguments) {
        [void]$startInfo.ArgumentList.Add($argument)
    }

    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $startInfo
    if (-not $process.Start()) {
        throw "Unable to start Docker."
    }

    return $process
}

function Stop-ContainerProcess {
    param(
        [Parameter(Mandatory)]
        [string]$PidFile
    )

    $stopScript = @'
set -eu
pid_file="$1"

if [ ! -f "$pid_file" ]; then
    exit 0
fi

pid="$(cat "$pid_file")"
case "$pid" in
    *[!0-9]*|'')
        rm -f "$pid_file"
        exit 0
        ;;
esac

if kill -0 -"$pid" 2>/dev/null; then
    kill -TERM -"$pid"
    attempts=0
    while kill -0 -"$pid" 2>/dev/null && [ "$attempts" -lt 50 ]; do
        sleep 0.1
        attempts=$((attempts + 1))
    done
    if kill -0 -"$pid" 2>/dev/null; then
        kill -KILL -"$pid"
    fi
fi

rm -f "$pid_file"
'@
    $stopScript = $stopScript -replace "`r", ""

    $stopScript | & docker compose exec -T workspace sh -c `
        "tr -d '\r' | sh -s -- '$PidFile'" 2>$null
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$locationPushed = $false
$apiProcess = $null
$frontendProcess = $null
$apiPidFile = "/tmp/itemorganizer-live-ai-api.pid"
$frontendPidFile = "/tmp/itemorganizer-live-ai-frontend.pid"

try {
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw "Docker CLI was not found. Install and start Docker Desktop, then retry."
    }

    $parsedEndpoint = $null
    if (
        -not [Uri]::TryCreate(
            $Endpoint,
            [UriKind]::Absolute,
            [ref]$parsedEndpoint) -or
        -not $Endpoint.StartsWith("https://", [StringComparison]::OrdinalIgnoreCase)
    ) {
        throw "Endpoint must be an absolute HTTPS URI."
    }

    Push-Location $projectRoot
    $locationPushed = $true

    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose",
        "up",
        "-d",
        "--wait",
        "workspace"
    ) -DiscardOutput

    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose",
        "exec",
        "-T",
        "workspace",
        "az",
        "account",
        "show",
        "--only-show-errors",
        "--output",
        "none"
    ) -DiscardOutput

    $deploymentState = & docker @(
        "compose",
        "exec",
        "-T",
        "workspace",
        "az",
        "cognitiveservices",
        "account",
        "deployment",
        "show",
        "--name",
        $ResourceName,
        "--resource-group",
        $ResourceGroup,
        "--deployment-name",
        $DeploymentName,
        "--only-show-errors",
        "--query",
        "properties.provisioningState",
        "--output",
        "tsv"
    )
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to read Azure OpenAI deployment $DeploymentName."
    }
    if ($deploymentState.Trim() -ne "Succeeded") {
        throw "Azure OpenAI deployment $DeploymentName is $deploymentState."
    }

    & docker @(
        "compose",
        "exec",
        "-T",
        "workspace",
        "sh",
        "-c",
        "test -d /workspace/web/node_modules"
    ) *> $null
    if ($LASTEXITCODE -ne 0) {
        throw "Frontend packages are missing. Run .\scripts\restore-frontend.ps1, then retry."
    }

    if ($ValidateOnly) {
        Write-Host "Live AI prerequisites are valid." -ForegroundColor Green
        return
    }

    Write-Host "Restoring backend packages." -ForegroundColor Cyan
    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose",
        "exec",
        "-T",
        "workspace",
        "dotnet",
        "restore",
        "ItemOrganizer.sln",
        "--nologo"
    )

    Write-Host "Applying database migrations." -ForegroundColor Cyan
    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose",
        "exec",
        "-T",
        "workspace",
        "dotnet",
        "run",
        "--project",
        "src/ItemOrganizer.Database",
        "--no-restore",
        "--",
        "migrate"
    )

    Stop-ContainerProcess -PidFile $frontendPidFile
    Stop-ContainerProcess -PidFile $apiPidFile

    $apiArguments = @(
        "compose",
        "exec",
        "-T",
        "-e",
        "ASPNETCORE_ENVIRONMENT=Development",
        "-e",
        "ASPNETCORE_URLS=http://0.0.0.0:5000",
        "-e",
        "DevelopmentAuthentication__Enabled=true",
        "-e",
        "DevelopmentAuthentication__TenantId=10000000-0000-0000-0000-000000000001",
        "-e",
        "DevelopmentAuthentication__OwnerObjectId=20000000-0000-0000-0000-000000000001",
        "-e",
        "Authentication__AllowedTenantId=10000000-0000-0000-0000-000000000001",
        "-e",
        "Analysis__Provider=azure-openai",
        "-e",
        "Analysis__Model=$DeploymentName",
        "-e",
        "AzureOpenAI__Endpoint=$($Endpoint.TrimEnd('/'))/",
        "workspace",
        "sh",
        "-c",
        "printf '%s' `"`$`$`" > '$apiPidFile'; exec dotnet run --project src/ItemOrganizer.Api --no-restore"
    )
    $apiProcess = Start-DockerProcess -Arguments $apiArguments
    Wait-ForHttp `
        -Uri "http://localhost:5000/api/v1/health/ready" `
        -Process $apiProcess

    $frontendArguments = @(
        "compose",
        "exec",
        "-T",
        "-e",
        "VITE_API_BASE_URL=http://localhost:5000",
        "-e",
        "VITE_DEVELOPMENT_AUTHENTICATION=true",
        "workspace",
        "sh",
        "-c",
        "printf '%s' `"`$`$`" > '$frontendPidFile'; exec pnpm -C web dev"
    )
    $frontendProcess = Start-DockerProcess -Arguments $frontendArguments
    Wait-ForHttp `
        -Uri "http://localhost:5173" `
        -Process $frontendProcess

    Write-Host ""
    Write-Host "Item Organizer live AI is ready:" -ForegroundColor Green
    Write-Host "  http://localhost:5173"
    Write-Host ""
    Write-Host "Open Photo analysis, choose a real photo, and select Upload and analyze."
    Write-Host "Press Ctrl+C to stop the API and frontend."

    if ($ExitAfterReady) {
        return
    }

    while (-not $apiProcess.HasExited -and -not $frontendProcess.HasExited) {
        Start-Sleep -Seconds 1
    }

    throw "A development server exited unexpectedly."
}
finally {
    Stop-ContainerProcess -PidFile $frontendPidFile
    Stop-ContainerProcess -PidFile $apiPidFile
    Stop-DevelopmentProcess $frontendProcess
    Stop-DevelopmentProcess $apiProcess

    if ($locationPushed) {
        Pop-Location
    }
}
