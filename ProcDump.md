# ProdDump

<https://docs.microsoft.com/en-us/sysinternals/downloads/procdump>

`c:\sysinternals\procdump.exe -ma <pid> d:\data\tools\`

Then open `c:\debuggers\windbg.exe`

```cmd
.sympath
.prefer_dml 1
.loadby sos clr
.load d:\data\tools\sos\sos.dll
.load d:\data\tools\sos\sosex.dll

!dumpheap -stat
```

## In windows, find process lock a file

Open Resource Monitor:

Press Ctrl + Shift + Esc to open the Task Manager.
Go to the Performance tab and click Open Resource Monitor at the bottom.
Search for the Hosts File:

In Resource Monitor, go to the CPU tab.
Expand the Associated Handles section.
In the search box under Associated Handles, type hosts and press Enter.
Find the Locking Process:

Resource Monitor will display the processes that are locking the hosts file under the Handle Name section.
You can see the name of the process that is holding onto the hosts file.
