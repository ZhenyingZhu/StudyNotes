function Print-ShortString
{
    param
    (
        [string] $id
    )

    if($id.Length -lt 4)
    {
        Write-Host($id)
    }
    else
    {
        return
    }
}

$ids = @("a", "ab", "abc", "abcd", "abcde", "end")

foreach($id in $ids)
{
    Print-ShortString $id
}