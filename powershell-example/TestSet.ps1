$correlationIdSet = New-Object System.Collections.Generic.HashSet[string]

$correlationIdSet.Add("a")
If ($correlationIdSet.Add("a"))
{
    Write-Host "Success"
}
Else
{
    Write-Host "Fail"
}