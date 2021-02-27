<#
    .SYNOPSIS
        Test Compress.

    .DESCRIPTION
        Test Compress.
#>

# https://stackoverflow.com/questions/18847145/loop-through-files-in-a-directory-using-powershell

Get-ChildItem -Path "FoldersToZip" -Recurse
#|
#ForEach-Object {
    # $folderName = $_.FullName
    # Compress-Archive -Force -Path $folderName -DestinationPath "$folderName.zip"
    # Write-Host $_
#}



