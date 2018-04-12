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

# List or Array
https://stackoverflow.com/questions/24754822/powershell-remove-item-0-from-an-array

# Set system path
For example set python:
```
[Environment]::SetEnvironmentVariable("Path", "$env:Path;C:\Python3.6;C:\Python3.6\Scripts")
```
