# PowerShell

## Debug

[src](https://technet.microsoft.com/en-us/library/ff730925.aspx)

```ps1
Set-PSBreakpoint -script C:\Scripts\Test.ps1 -line 4
C:\Scripts\Test.ps1
```

```ps1
{break}
```

## env variable

```ps1
Get-ChildItem Env:

$envvar = $env:SOMETHING
```

## hashtable

```ps1
$states = @{"Washington" = "Olympia"; "Oregon" = "Salem"; California = "Sacramento"}
```

## Check properfy

`$obj | fl`

## List or Array

<https://stackoverflow.com/questions/24754822/powershell-remove-item-0-from-an-array>

## Set system path

For example set python:

```ps1
[Environment]::SetEnvironmentVariable("Path", "$env:Path;C:\Python3.6;C:\Python3.6\Scripts")
```

## Get all commands in a module

```ps1
Get-Command -Module <Module name>
```

## Write result to a file

```ps1
Compare-Object $(Get-Content c:\user\documents\List1.txt) $(Get-Content c:\user\documents\List2.txt) | Out-File C:\filename.txt -Encoding utf8
```

## See port usage

[How can you find out which process is listening on a port on Windows](https://stackoverflow.com/questions/48198/how-can-you-find-out-which-process-is-listening-on-a-port-on-windows)

```ps1
netstat -a -b -n -o
```

## where

Get all elements that return true.

```ps1
$list = 1, 2, 3, 1
$list | where { $_ -eq 1 }
```

## Remove an app

[Full Fix: OneNote Issues in Windows 10](https://windowsreport.com/onenote-problems-windows-10/)

```ps1
get-appxpackage *microsoft.office.onenote* | remove-appxpackage
remove-appxprovisionedpackage –Online –PackageName Microsoft.Office.OneNote_2014.919.2035.737_neutral_~_8wekyb3d8bbwe
```

then restart the computer.

## Run C# codes

```ps1
Add-Type -Path "$packagesRoot\Newtonsoft.Json.dll"
$dataView = New-Object System.Data.DataView($dataTable)
```

## Edit reg

```ps1
reg add "HKEY_CURRENT_USER\Software\Valve\Steam" /v AutoLoginUser /t REG_SZ /d [DATA] /f
```

## Check not in

`list -notcontains item`

## Create a List

`$list = New-Object Collections.Generic.List[System.Guid]`

## Powershell Promote

with just this:
`Remove-Item $test`
If there are files in the folder, then the error: "Remove-item : No PromptForChoice Handler set on the Powershell UI"

`Remove-Item $test -Confirm:$false -Force -Recurse`

## PSSession

<https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_pssessions?view=powershell-7.3>
