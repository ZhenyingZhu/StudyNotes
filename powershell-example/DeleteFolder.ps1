<#
    .SYNOPSIS
        Test folder deletion.

    .DESCRIPTION
        Test deletion.
#>

# https://stackoverflow.com/questions/38141528/cannot-remove-item-the-directory-is-not-empty

param
(
    [Parameter(Mandatory=$true, HelpMessage='The path')]
    [ValidateNotNullOrEmpty()]
    $FolderToDelete
)

Get-ChildItem -Path $FolderToDelete -Recurse |
# Write-Host
Remove-Item -Confirm:$false -Force -Recurse
