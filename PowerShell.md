# Debug
[src](https://technet.microsoft.com/en-us/library/ff730925.aspx)

```
Set-PSBreakpoint -script C:\Scripts\Test.ps1 -line 4
C:\Scripts\Test.ps1
```

```
{break}
```

# env variable
```
Get-ChildItem Env:

$envvar = $env:SOMETHING
```

# hashtable
```
$states = @{"Washington" = "Olympia"; "Oregon" = "Salem"; California = "Sacramento"}
```

# Check properfy
`$obj | fl`