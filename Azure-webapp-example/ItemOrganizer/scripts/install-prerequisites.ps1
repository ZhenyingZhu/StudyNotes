[CmdletBinding()]
param(
    [switch]$SkipDockerLaunch
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Invoke-CheckedCommand {
    param(
        [Parameter(Mandatory)]
        [string]$Command,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [int[]]$SuccessExitCodes = @(0)
    )

    Write-Host "> $Command $($Arguments -join ' ')" -ForegroundColor Cyan
    & $Command @Arguments
    $exitCode = $LASTEXITCODE

    if ($exitCode -notin $SuccessExitCodes) {
        throw "$Command failed with exit code $exitCode."
    }
}

function Install-WinGetPackage {
    param(
        [Parameter(Mandatory)]
        [string]$Id,

        [Parameter(Mandatory)]
        [string]$Name
    )

    Write-Host ""
    Write-Host "Installing or updating $Name..." -ForegroundColor Green
    Invoke-CheckedCommand `
        -Command "winget" `
        -Arguments @(
            "install",
            "--exact",
            "--id",
            $Id,
            "--source",
            "winget",
            "--accept-package-agreements",
            "--accept-source-agreements",
            "--disable-interactivity"
        ) `
        -SuccessExitCodes @(0, -1978335189)
}

function Update-ProcessPath {
    $machinePath = [Environment]::GetEnvironmentVariable("Path", "Machine")
    $userPath = [Environment]::GetEnvironmentVariable("Path", "User")
    $env:Path = "$machinePath;$userPath"
}

function Test-IsAdministrator {
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = [Security.Principal.WindowsPrincipal]::new($identity)
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Find-VsCodeCommand {
    $command = Get-Command code -ErrorAction SilentlyContinue
    if ($command) {
        return $command.Source
    }

    $candidates = @(
        (Join-Path $env:LOCALAPPDATA "Programs\Microsoft VS Code\bin\code.cmd"),
        (Join-Path $env:ProgramFiles "Microsoft VS Code\bin\code.cmd")
    )

    foreach ($candidate in $candidates) {
        if (Test-Path -LiteralPath $candidate -PathType Leaf) {
            return $candidate
        }
    }

    throw "Visual Studio Code was installed, but its command-line launcher could not be found."
}

if ($env:OS -ne "Windows_NT") {
    throw "This prerequisite installer supports Windows only."
}

if (-not (Test-IsAdministrator)) {
    throw "Run this script from PowerShell as Administrator so it can install WSL 2."
}

if (-not (Get-Command winget -ErrorAction SilentlyContinue)) {
    throw "WinGet is unavailable. Install or update App Installer from the Microsoft Store, then retry."
}

$wslFeature = Get-WindowsOptionalFeature -Online -FeatureName "Microsoft-Windows-Subsystem-Linux"
$virtualMachinePlatformFeature = Get-WindowsOptionalFeature -Online -FeatureName "VirtualMachinePlatform"
$restartRequired = $false

if (
    $wslFeature.State -ne "Enabled" -or
    $virtualMachinePlatformFeature.State -ne "Enabled"
) {
    Write-Host ""
    Write-Host "Installing WSL 2..." -ForegroundColor Green
    Invoke-CheckedCommand `
        -Command "wsl.exe" `
        -Arguments @("--install", "--no-distribution") `
        -SuccessExitCodes @(0, 3010)
    $restartRequired = $true
}
else {
    Write-Host "WSL 2 Windows features are already enabled." -ForegroundColor Green
}

Install-WinGetPackage -Id "Git.Git" -Name "Git"
Install-WinGetPackage -Id "Microsoft.VisualStudioCode" -Name "Visual Studio Code"
Install-WinGetPackage -Id "Docker.DockerDesktop" -Name "Docker Desktop"

Update-ProcessPath

$codeCommand = Find-VsCodeCommand
Write-Host ""
Write-Host "Installing the VS Code Dev Containers extension..." -ForegroundColor Green
Invoke-CheckedCommand $codeCommand @(
    "--install-extension",
    "ms-vscode-remote.remote-containers",
    "--force"
)

$dockerDesktopPath = Join-Path $env:ProgramFiles "Docker\Docker\Docker Desktop.exe"
if (-not $SkipDockerLaunch -and -not $restartRequired) {
    if (-not (Test-Path -LiteralPath $dockerDesktopPath -PathType Leaf)) {
        throw "Docker Desktop was installed, but its executable was not found at $dockerDesktopPath."
    }

    Write-Host ""
    Write-Host "Launching Docker Desktop..." -ForegroundColor Green
    Start-Process -FilePath $dockerDesktopPath
}

Write-Host ""
Write-Host "Windows prerequisites are installed." -ForegroundColor Green
if ($restartRequired) {
    Write-Host "Restart Windows to finish enabling WSL 2, then launch Docker Desktop."
}
else {
    Write-Host "Complete any Docker Desktop first-run prompts and restart if Windows requests one."
}
Write-Host "After Docker Desktop reports that its Linux engine is running, execute:"
Write-Host "  .\scripts\start-environment.ps1" -ForegroundColor Cyan
