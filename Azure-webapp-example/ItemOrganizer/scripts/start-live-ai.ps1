[CmdletBinding()]
param(
    [string]$Endpoint =
        "https://zhenying-gpt4o-resource.cognitiveservices.azure.com/",
    [string]$DeploymentName = "gpt-5.4-mini-itemorganizer-dev",
    [string]$ResourceGroup = "rg-zhenying-gpt4o",
    [string]$ResourceName = "zhenying-gpt4o-resource",
    [switch]$ValidateOnly,
    [switch]$ExitAfterReady,
    [string]$TenantId
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Invoke-CheckedCommand {
    param(
        [Parameter(Mandatory)]
        [string]$Command,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [Parameter(Mandatory)]
        [string]$FailureMessage,

        [Parameter(Mandatory)]
        [string]$Recovery,

        [switch]$DiscardOutput,

        [switch]$PassThru
    )

    $PSNativeCommandUseErrorActionPreference = $false
    if ($DiscardOutput -or $PassThru) {
        $output = @(& $Command @Arguments 2>&1)
    }
    else {
        & $Command @Arguments 2>&1 | Tee-Object -Variable output | Out-Host
    }

    if ($LASTEXITCODE -ne 0) {
        $details = ($output | ForEach-Object { "$_" }) -join [Environment]::NewLine
        throw @"
$FailureMessage
Command: $Command $($Arguments -join ' ')
Exit code: $LASTEXITCODE
$details
Recovery: $Recovery
"@
    }

    if ($PassThru) {
        return $output
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
$serverCleanupRequired = $false
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

    $dockerOs = Invoke-CheckedCommand -Command "docker" -Arguments @(
        "info", "--format", "{{.OSType}}"
    ) -PassThru `
        -FailureMessage "Cannot connect to the Docker engine." `
        -Recovery "Start Docker Desktop in Linux-container mode, wait for its engine to be ready, then retry."
    if (("$dockerOs").Trim() -ne "linux") {
        throw "Docker is using Windows containers. Switch Docker Desktop to Linux containers, then retry."
    }

    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose", "version"
    ) -DiscardOutput `
        -FailureMessage "Docker Compose v2 is unavailable." `
        -Recovery "Use a Docker Desktop installation with Compose v2, then retry."

    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose", "config", "--quiet"
    ) -DiscardOutput `
        -FailureMessage "The Docker Compose development configuration is invalid." `
        -Recovery "Complete the README Quick start setup, including .env, and correct the configuration error shown above."

    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose",
        "up",
        "-d",
        "--wait",
        "workspace"
    ) -DiscardOutput `
        -FailureMessage "Could not start the Docker development services." `
        -Recovery "Resolve the Docker error above. Inspect services with 'docker compose ps --all' and 'docker compose logs --tail 50 workspace postgres azurite', then retry."

    $azureAccountArguments = @(
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
    )
    $azureLoginRecovery = "Run 'docker compose exec -T workspace az login --use-device-code --tenant <your-tenant-id>' using the tenant that owns the Azure OpenAI resource, then retry. Signing in on the Windows host does not sign in the container."
    try {
        Invoke-CheckedCommand -Command "docker" -Arguments $azureAccountArguments `
            -DiscardOutput `
            -FailureMessage "The Docker workspace Azure CLI session is unavailable or expired." `
            -Recovery $azureLoginRecovery
    }
    catch {
        if ($ValidateOnly) {
            throw
        }

        Write-Warning $_.Exception.Message
        Write-Host "Starting Azure device-code sign-in inside the Docker workspace. Complete the browser sign-in to continue." -ForegroundColor Cyan
        $azureLoginArguments = @(
            "compose", "exec", "-T",
            "-e", "AZURE_CORE_LOGIN_EXPERIENCE_V2=off",
            "workspace", "az", "login", "--use-device-code", "--output", "none"
        )
        if (-not [string]::IsNullOrWhiteSpace($TenantId)) {
            $azureLoginArguments += @("--tenant", $TenantId)
        }
        Invoke-CheckedCommand -Command "docker" -Arguments $azureLoginArguments `
            -FailureMessage "Azure device-code sign-in failed in the Docker workspace." `
            -Recovery "Complete the browser sign-in using an account with access to the Azure OpenAI resource, then retry. Use -TenantId <your-tenant-id> to select its tenant."

        Invoke-CheckedCommand -Command "docker" -Arguments $azureAccountArguments `
            -DiscardOutput `
            -FailureMessage "Azure sign-in completed, but the Docker workspace still has no usable Azure CLI session." `
            -Recovery $azureLoginRecovery
    }

    $deploymentState = Invoke-CheckedCommand -Command "docker" -Arguments @(
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
    ) -PassThru `
        -FailureMessage "Unable to read Azure OpenAI deployment '$DeploymentName' in resource '$ResourceName' (resource group '$ResourceGroup')." `
        -Recovery "Check the deployment and resource names, workspace Azure subscription ('docker compose exec -T workspace az account show --output table'), and your permission to read the deployment."
    if (("$deploymentState").Trim() -ne "Succeeded") {
        throw "Azure OpenAI deployment '$DeploymentName' is not ready (state: '$deploymentState'). Wait for provisioning to succeed or resolve the deployment failure in Azure, then retry."
    }

    Invoke-CheckedCommand -Command "docker" -Arguments @(
        "compose",
        "exec",
        "-T",
        "workspace",
        "sh",
        "-c",
        "test -d /workspace/web/node_modules"
    ) -DiscardOutput `
        -FailureMessage "The frontend package check failed in the Docker workspace; web/node_modules may be missing." `
        -Recovery "If packages are missing, run .\scripts\restore-frontend.ps1, then retry. Otherwise resolve the Docker error above."

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
    ) -FailureMessage "Backend package restore failed in the Docker workspace." `
        -Recovery "Resolve the package restore error above, then retry."

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
    ) -FailureMessage "Database migrations failed in the Docker workspace." `
        -Recovery "Check 'docker compose ps --all' and the database connection in .env, resolve the migration error above, then retry."

    $serverCleanupRequired = $true
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
    if ($serverCleanupRequired) {
        Stop-ContainerProcess -PidFile $frontendPidFile
        Stop-ContainerProcess -PidFile $apiPidFile
    }
    Stop-DevelopmentProcess $frontendProcess
    Stop-DevelopmentProcess $apiProcess

    if ($locationPushed) {
        Pop-Location
    }
}
