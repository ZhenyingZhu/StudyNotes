param
(
    [Parameter(Mandatory=$false, HelpMessage='A param with default var')]
    [string] $InputVar = "0"
)

if($InputVar -ne "0")
{
    Write-Host("InputVar is {0}" -f $InputVar)
}
else
{
    Write-Host("InputVar is default")
}
