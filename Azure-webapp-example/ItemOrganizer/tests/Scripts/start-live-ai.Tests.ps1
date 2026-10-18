[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true
$launcher = Join-Path $PSScriptRoot "..\..\scripts\start-live-ai.ps1"

# Exercise the complete prerequisite flow without changing Docker or Azure state.
function docker {
    $commandLine = $args -join " "
    $calls.Add($commandLine)
    $global:LASTEXITCODE = 0

    if (
        $commandLine -eq $accountCommand -and
        -not $loginState.SignedIn
    ) {
        Write-Output "Simulated underlying diagnostic."
        $global:LASTEXITCODE = 17
        return
    }

    if ($commandLine -eq $testCase.Stage) {
        Write-Output "Simulated underlying diagnostic."
        $global:LASTEXITCODE = 17
        return
    }

    if ($commandLine -like "* az login --use-device-code*") {
        $loginState.SignedIn = $true
    }

    switch ($commandLine) {
        "info --format {{.OSType}}" { Write-Output $testCase.DockerOs }
        { $_ -like "*properties.provisioningState*" } {
            Write-Output $testCase.DeploymentState
        }
    }
}

$deploymentCommand = "compose exec -T workspace az cognitiveservices account deployment show --name zhenying-gpt4o-resource --resource-group rg-zhenying-gpt4o --deployment-name gpt-5.4-mini-itemorganizer-dev --only-show-errors --query properties.provisioningState --output tsv"
$accountCommand = "compose exec -T workspace az account show --only-show-errors --output none"
$loginCommand = "compose exec -T -e AZURE_CORE_LOGIN_EXPERIENCE_V2=off workspace az login --use-device-code --output none"
$frontendCommand = "compose exec -T workspace sh -c test -d /workspace/web/node_modules"
$cases = @(
    @{
        Name = "Docker engine unavailable"
        Stage = "info --format {{.OSType}}"
        Expected = "Cannot connect to the Docker engine.", "Start Docker Desktop"
    },
    @{
        Name = "Windows containers"
        DockerOs = "windows"
        Expected = "Switch Docker Desktop to Linux containers"
    },
    @{
        Name = "Compose unavailable"
        Stage = "compose version"
        Expected = "Docker Compose v2 is unavailable.", "Compose v2"
    },
    @{
        Name = "Invalid Compose configuration"
        Stage = "compose config --quiet"
        Expected = "configuration is invalid.", ".env"
    },
    @{
        Name = "Services failed to start"
        Stage = "compose up -d --wait workspace"
        Expected = "Could not start the Docker development services.", "docker compose logs"
    },
    @{
        Name = "Workspace Azure login missing"
        Stage = "compose exec -T workspace az account show --only-show-errors --output none"
        Expected = "Azure CLI session is unavailable or expired.", "az login --use-device-code --tenant <your-tenant-id>", "Windows host"
    },
    @{
        Name = "Deployment lookup failed"
        Stage = $deploymentCommand
        Expected = "Unable to read Azure OpenAI deployment", "permission to read"
    },
    @{
        Name = "Deployment not ready"
        DeploymentState = "Failed"
        Expected = "state: 'Failed'", "resolve the deployment failure"
    },
    @{
        Name = "Frontend packages missing"
        Stage = "compose exec -T workspace sh -c test -d /workspace/web/node_modules"
        Expected = "frontend package check failed", ".\scripts\restore-frontend.ps1"
    },
    @{
        Name = "Backend restore failed"
        Stage = "compose exec -T workspace dotnet restore ItemOrganizer.sln --nologo"
        ValidateOnly = $false
        Expected = "Backend package restore failed", "Resolve the package restore error"
    },
    @{
        Name = "Database migrations failed"
        Stage = "compose exec -T workspace dotnet run --project src/ItemOrganizer.Database --no-restore -- migrate"
        ValidateOnly = $false
        Expected = "Database migrations failed", "database connection in .env"
    },
    @{
        Name = "All prerequisites valid"
        Expected = @()
    },
    @{
        Name = "Automatic login continues startup"
        NeedsLogin = $true
        ValidateOnly = $false
        Stage = $frontendCommand
        Expected = "frontend package check failed"
        ExpectedLogin = $loginCommand
    },
    @{
        Name = "Automatic login uses selected tenant"
        NeedsLogin = $true
        ValidateOnly = $false
        TenantId = "test-tenant"
        Stage = $frontendCommand
        Expected = "frontend package check failed"
        ExpectedLogin = "$loginCommand --tenant test-tenant"
    },
    @{
        Name = "Automatic login failure stops startup"
        NeedsLogin = $true
        ValidateOnly = $false
        Stage = $loginCommand
        Expected = "Azure device-code sign-in failed", "Use -TenantId"
        ExpectedLogin = $loginCommand
    },
    @{
        Name = "ValidateOnly never starts login"
        NeedsLogin = $true
        Stage = $accountCommand
        Expected = "Azure CLI session is unavailable or expired."
    },
    @{
        Name = "Login result is rechecked"
        NeedsLogin = $true
        ValidateOnly = $false
        Stage = $accountCommand
        Expected = "still has no usable Azure CLI session"
        ExpectedLogin = $loginCommand
    }
)

foreach ($testCase in $cases) {
    if (-not $testCase.ContainsKey("Stage")) { $testCase.Stage = "" }
    if (-not $testCase.ContainsKey("DockerOs")) { $testCase.DockerOs = "linux" }
    if (-not $testCase.ContainsKey("DeploymentState")) {
        $testCase.DeploymentState = "Succeeded"
    }
    if (-not $testCase.ContainsKey("ValidateOnly")) { $testCase.ValidateOnly = $true }
    if (-not $testCase.ContainsKey("NeedsLogin")) { $testCase.NeedsLogin = $false }
    if (-not $testCase.ContainsKey("TenantId")) { $testCase.TenantId = "" }
    if (-not $testCase.ContainsKey("ExpectedLogin")) { $testCase.ExpectedLogin = "" }
    $loginState = @{ SignedIn = -not $testCase.NeedsLogin }
    $calls = [System.Collections.Generic.List[string]]::new()
    $originalLocation = Get-Location
    $message = ""
    try {
        & $launcher -ValidateOnly:$testCase.ValidateOnly -TenantId $testCase.TenantId
    }
    catch {
        $message = $_.Exception.Message
    }

    if (@($testCase.Expected).Count -eq 0 -and $message) {
        throw "$($testCase.Name): unexpected error: $message"
    }
    foreach ($expected in $testCase.Expected) {
        if (-not $message.Contains($expected)) {
            throw "$($testCase.Name): missing '$expected' in error: $message"
        }
    }
    if ($testCase.Stage) {
        foreach ($expected in @(
            "Command: docker $($testCase.Stage)",
            "Exit code: 17",
            "Simulated underlying diagnostic.",
            "Recovery:"
        )) {
            if (-not $message.Contains($expected)) {
                throw "$($testCase.Name): missing '$expected' in error: $message"
            }
        }
        if ($calls[$calls.Count - 1] -ne $testCase.Stage) {
            throw "$($testCase.Name): Docker commands continued after the failure."
        }
    }
    if ($calls | Where-Object { $_ -like "*sh -s --*" }) {
        throw "$($testCase.Name): prerequisite validation attempted server cleanup."
    }
    $loginCalls = @($calls | Where-Object { $_ -like "* az login --use-device-code*" })
    if ($testCase.ExpectedLogin) {
        if ($loginCalls.Count -ne 1 -or $loginCalls[0] -ne $testCase.ExpectedLogin) {
            throw "$($testCase.Name): expected exactly one login with the correct arguments."
        }
        $accountCalls = @($calls | Where-Object { $_ -eq $accountCommand })
        $expectedAccountCalls = if ($testCase.Stage -eq $loginCalls[0]) { 1 } else { 2 }
        if ($accountCalls.Count -ne $expectedAccountCalls) {
            throw "$($testCase.Name): login result was not checked as expected."
        }
    }
    elseif ($loginCalls.Count -ne 0) {
        throw "$($testCase.Name): unexpectedly started interactive Azure login."
    }
    if ((Get-Location).Path -ne $originalLocation.Path) {
        throw "$($testCase.Name): launcher did not restore the working directory."
    }
    Write-Host "PASS: $($testCase.Name)"
}
