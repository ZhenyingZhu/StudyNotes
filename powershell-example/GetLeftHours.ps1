$StartDate = (Get-Date)
$EndDate = [DateTime]::Today.AddDays(1)
New-TimeSpan -Start $StartDate -End $EndDate

Read-Host