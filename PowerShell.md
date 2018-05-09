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

# Get all commands in a module
```
Get-Command -Module <Module name>
```

# Write result to a file
```
Compare-Object $(Get-Content c:\user\documents\List1.txt) $(Get-Content c:\user\documents\List2.txt) | Out-File C:\filename.txt -Encoding utf8
```

# See port usage
[How can you find out which process is listening on a port on Windows](https://stackoverflow.com/questions/48198/how-can-you-find-out-which-process-is-listening-on-a-port-on-windows)
```
netstat -a -b -n -o
```

# where
Get all elements that return true.
```
$list = 1, 2, 3, 1
$list | where { $_ -eq 1 }
```

# Remove an app
[Full Fix: OneNote Issues in Windows 10](https://windowsreport.com/onenote-problems-windows-10/)
```
get-appxpackage *microsoft.office.onenote* | remove-appxpackage
remove-appxprovisionedpackage –Online –PackageName Microsoft.Office.OneNote_2014.919.2035.737_neutral_~_8wekyb3d8bbwe
```
then restart the computer.

# Run C# codes
```
Add-Type -Path "$packagesRoot\Newtonsoft.Json.dll"
$dataView = New-Object System.Data.DataView($dataTable)
```
