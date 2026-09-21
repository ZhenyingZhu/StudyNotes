[CmdletBinding()]
param(
    [switch]$NoBuild,
    [switch]$SkipSmokeTest,
    [string]$CustomCaPath
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
$resolvedCustomCaPath = $null
$dotnetEfPackagePath = $null

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

    if (-not [string]::IsNullOrWhiteSpace($CustomCaPath)) {
        if ($NoBuild) {
            throw "-CustomCaPath cannot be used with -NoBuild because the certificate is only available during image builds."
        }

        if (-not (Test-Path -LiteralPath $CustomCaPath -PathType Leaf)) {
            throw "Custom CA certificate was not found: $CustomCaPath"
        }

        $resolvedCustomCaPath = (Resolve-Path -LiteralPath $CustomCaPath).ProviderPath
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
        $composeConfigJson = & docker compose config --format json
        if ($LASTEXITCODE -ne 0) {
            throw "Unable to resolve the Docker Compose build configuration."
        }

        $composeConfig = $composeConfigJson | ConvertFrom-Json
        $workspaceBuild = $composeConfig.services.workspace.build
        $workspaceImage = $composeConfig.services.workspace.image
        if (-not $workspaceBuild -or [string]::IsNullOrWhiteSpace($workspaceImage)) {
            throw "Docker Compose does not define the workspace build or image configuration."
        }

        $dotnetEfVersion = $workspaceBuild.args.DOTNET_EF_VERSION
        if ([string]::IsNullOrWhiteSpace($dotnetEfVersion)) {
            throw "Docker Compose does not define DOTNET_EF_VERSION."
        }

        $dotnetEfPackagePath = Join-Path $projectRoot ".devcontainer\.build\dotnet-ef.nupkg"
        $dotnetEfPackageUri = "https://www.nuget.org/api/v2/package/dotnet-ef/$dotnetEfVersion"
        Write-Host "> Downloading dotnet-ef $dotnetEfVersion with Windows certificate validation" -ForegroundColor Cyan
        Invoke-WebRequest -UseBasicParsing -Uri $dotnetEfPackageUri -OutFile $dotnetEfPackagePath

        $packageFile = Get-Item -LiteralPath $dotnetEfPackagePath
        $packageStream = [System.IO.File]::OpenRead($dotnetEfPackagePath)
        try {
            $packageSignature = New-Object byte[] 4
            [void]$packageStream.Read($packageSignature, 0, $packageSignature.Length)
        }
        finally {
            $packageStream.Dispose()
        }

        if (
            $packageFile.Length -lt 1024 -or
            $packageSignature[0] -ne 0x50 -or
            $packageSignature[1] -ne 0x4B
        ) {
            throw "Downloaded dotnet-ef package is not a valid NuGet archive."
        }

        $buildArguments = @("build")

        if ($resolvedCustomCaPath) {
            $buildArguments += @("--secret", "id=custom_ca,src=$resolvedCustomCaPath")
        }

        foreach ($buildArgument in $workspaceBuild.args.PSObject.Properties) {
            $buildArguments += @("--build-arg", "$($buildArgument.Name)=$($buildArgument.Value)")
        }

        $buildArguments += @(
            "--tag",
            $workspaceImage,
            "--file",
            $workspaceBuild.dockerfile,
            $workspaceBuild.context
        )

        Invoke-CheckedCommand "docker" $buildArguments
        $upArguments += "--no-build"
    }
    else {
        $upArguments += "--no-build"
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
    if ($dotnetEfPackagePath -and (Test-Path -LiteralPath $dotnetEfPackagePath)) {
        Remove-Item -LiteralPath $dotnetEfPackagePath -Force
    }

    if ($locationPushed) {
        Pop-Location
    }
}
