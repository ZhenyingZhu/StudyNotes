cls
Write-Host "Copying folder."
Copy-Item E:\Logfiles -destination E:\Backup
Write-Host "Deleting folder."
Remove-Item E:\Logfiles
