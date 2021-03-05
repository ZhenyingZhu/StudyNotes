<#
    .SYNOPSIS
        Test Compress.

    .DESCRIPTION
        Test Compress.
#>

# https://stackoverflow.com/questions/18847145/loop-through-files-in-a-directory-using-powershell

param
(
    [Parameter(Mandatory=$true, HelpMessage='The path')]
    [ValidateNotNullOrEmpty()]
    $FoldersToZip,

    [Parameter(Mandatory=$true, HelpMessage='The path')]
    [ValidateNotNullOrEmpty()]
    $DestinationPath
)

Get-ChildItem -Path "FoldersToZip" -Recurse |
ForEach-Object {
    $itemName = $_.FullName
    $isFolder = Test-Path -Path $itemName -PathType Container # Leaf is file
    if ($isFolder)
    {
        Write-Host "zipping $itemName..."
        Compress-Archive -Force -Path $itemName -DestinationPath "$itemName.zip"
    }
    elseif (-Not $isFolder)
    {
        
        # print a string with parameters and underscore:
        Write-Host "${itemName}_ is not a folder."
    }
}

# Powershell.exe -command "& { Compress-Archive -Force -path "xyz" -DestinationPath xyz.zip }"
