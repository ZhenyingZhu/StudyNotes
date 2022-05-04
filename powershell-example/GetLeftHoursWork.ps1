$StartDate = (Get-Date)
$EndDate = [DateTime]::Today.AddHours(18.5)
New-TimeSpan -Start $StartDate -End $EndDate

Read-Host