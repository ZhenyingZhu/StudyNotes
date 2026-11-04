$exe='C:\Program Files\Microsoft OneDrive\OneDrive.exe';
Write-Output '--- Before restart ---';
Get-Process OneDrive -ErrorAction SilentlyContinue | Select-Object Id,StartTime,Responding | Format-Table | Out-String;
$assert=Join-Path $env:LOCALAPPDATA 'Microsoft\OneDrive\settings\Personal\assertInformation.ini';
if(Test-Path $assert) { 
   Write-Output '--- Current assertion records ---';
   [Text.Encoding]::Unicode.GetString([IO.File]::ReadAllBytes($assert))
};

if(-not(Test-Path $exe)) {
    throw 'Expected OneDrive executable not found. No changes made.'
};
& $exe /shutdown;

Start-Sleep -Seconds 5;

if(Get-Process OneDrive -ErrorAction SilentlyContinue) {
    Write-Output 'OneDrive has not exited. No forced termination or duplicate launch performed.'
} else {
    Start-Process -FilePath $exe;
    Start-Sleep -Seconds 12
};

Write-Output '--- After restart ---';
Get-Process OneDrive -ErrorAction SilentlyContinue | Select-Object Id,StartTime,Responding | Format-Table | Out-String;

Write-Output '--- Latest sync diagnostic timestamp ---';
$diag=Join-Path $env:LOCALAPPDATA 'Microsoft\OneDrive\logs\Personal\SyncDiagnostics.log';
if(Test-Path $diag) {
    Get-Item $diag | Select-Object LastWriteTime,Length | Format-List | Out-String;
    Get-Content $diag | Select-String 'UtcNow|syncProgressState|SyncProgressState|FilesToDownload|FilesToUpload|ChangesToProcess|wasFileDBReset' | ForEach-Object {$_.Line}
}

Write-Output '--- Assertion records after restart ---';
 $p=Join-Path $env:LOCALAPPDATA 'Microsoft\OneDrive\settings\Personal\assertInformation.ini';
 [Text.Encoding]::Unicode.GetString([IO.File]::ReadAllBytes($p));
 Write-Output '--- Fresh sync health indicators ---';
 Get-Content (Join-Path $env:LOCALAPPDATA 'Microsoft\OneDrive\logs\Personal\SyncDiagnostics.log') | Select-String 'UtcNow|drivesConnected|drivesWaitingForInitialSync|files =|folders =|fullScanCount|numLocalChanges|wasFileDBReset|scanState|BytesToUpload|BytesToDownload' | ForEach-Object {$_.Line}