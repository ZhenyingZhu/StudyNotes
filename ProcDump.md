# ProdDump

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
