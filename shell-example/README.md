# PCSX2 memory card sync

Double-click `sync-pcsx2-memcards.desktop` and choose **Launch** or **Allow
Launching** if your Linux desktop asks. The launcher opens a terminal and waits
for Enter after completion, including on failure. Some file managers require
launchers to be on the Desktop or marked trusted before they can run.

The launcher uses `sync-pcsx2-memcards.sh` in this checkout, so keep the script at
its current location or update the launcher's `Exec` and `Path` entries.
Both files must be executable (`chmod +x sync-pcsx2-memcards.sh
sync-pcsx2-memcards.desktop`).

The script requires Bash and rsync 3.1.3 or newer and synchronizes:

- `/home/zhu91/HotmailOneDrive/Documents/PCSX2/memcards`
- `/home/zhu91/.config/PCSX2/memcards`

Close PCSX2 and wait for OneDrive downloads to finish before running it. This is
a local folder sync; it does not start or wait for the OneDrive client.

Files and subdirectories missing from either side are copied across. For the
same relative file path, the newer modification time wins, including
subsecond differences where supported by the filesystem. Modification times are
preserved. Nothing is deleted, so deleting a file from only one folder restores
it on the next sync. Symlinks and special files are not copied.

If timestamps are equal, files are left untouched even if their contents differ.
Do not edit files or run another sync while this script is running. An error
stops the sync and is shown in the terminal; files already copied are not rolled
back. Both folders must already exist, to help catch missing mounts or typos.

To run from a terminal:

```bash
./sync-pcsx2-memcards.sh
```

For testing against other existing folders:

```bash
./sync-pcsx2-memcards.sh /tmp/memcards-a /tmp/memcards-b
```
