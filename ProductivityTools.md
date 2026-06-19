# Productivity Tools

## Auto Hot Key

<https://autohotkey.com/docs/Tutorial.htm>

## AWK

<http://www.grymoire.com/Unix/Awk.html#uh-1>

line oriented.

### form

```awk
pattern { action }
```

test each line with pattern. If true, do action.

```awk
BEGIN { }
      { }
END   { }
```

actions taken before first/after last lines are read.

- `\t` tab.
- `$n` the nth argument.

### execute

```awk
awk 'script' file
```

### pattern

regular express in '/ /'

```awk
awk '/^1/ { print $1 }' file
```

Not logic for pattern

```awk
awk '! /^1/ {}' file
```

### Seperate char

<https://www.gnu.org/software/gawk/manual/html_node/Field-Separators.html>

```awk
awk 'BEGIN { FS = "," } ; { print $3, $5 }'
```

## Depressurizer

<https://github.com/Depressurizer/Depressurizer>

GameData.cs

`public int UpdateGameListFromOwnedPackageInfo( Int64 accountId, SortedSet<int> ignored, AppTypes includedTypes, out int newApps )` read games

1. read from `appcache\packageinfo.vdf`. This file contains all packages
2. read from `userdata\{userid}\config\localconfig.vdf`. This file contains all licenses

## Cron

<http://www.cyberciti.biz/faq/how-do-i-add-jobs-to-cron-under-linux-or-unix-oses/>

`/etc/crontab file`, `/etc/cron.*/` and `/var/spool/cron/`

use `crontab -e` to create a cron job.

fields:

```bash
* * * * * command to be executed
- - - - -
| | | | |
| | | | ----- Day of week (0 - 7) (Sunday=0 or 7)
| | | ------- Month (1 - 12)
| | --------- Day of month (1 - 31)
| ----------- Hour (0 - 23)
------------- Minute (0 - 59)
```

to disable emails:

```bash
0 3 * * * /root/backup.sh >/dev/null 2>&1
```

to run as a user:

```bash
* * * * * username command
```
