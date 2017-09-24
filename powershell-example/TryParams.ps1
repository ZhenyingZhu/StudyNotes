param (
    [Parameter(Mandatory=$true, HelpMessage='name')]
    [string] $name,
    
    [Parameter(Mandatory=$false, HelpMessage='age')]
    [string] $age = 17,

    [Parameter(Mandatory=$false, HelpMessage='number')]
    [string] $number = 110
)

Write-Host("Result: {0}, {1}, {2}" -f $name, $age, $number)
