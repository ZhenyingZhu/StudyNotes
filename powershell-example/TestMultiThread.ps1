$ScriptBlock =
{
    param ($num)

    Write-Host "start $num"
    Start-Sleep 5
    Write-Host "end $num"
}

$threadNum = 3

$currentThreadCnt = 0
for ($i = 1; $i -le 10; $i++)
{
    Start-Job $ScriptBlock -ArgumentList $i
    $currentThreadCnt++
    if ($currentThreadCnt -eq $threadNum)
    {
        $currentThreadCnt = 0
        Get-Job | Wait-Job
        Get-Job | Receive-Job
    }
}

