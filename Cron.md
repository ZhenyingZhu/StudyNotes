# Cron

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
