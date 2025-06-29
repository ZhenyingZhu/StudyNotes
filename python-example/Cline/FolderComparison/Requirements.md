# Requirements

Use python to build a folder comparision tool to compare the folder and its backup on a different device. It supports 2 modes: 1. scan mode, 2. compare mode.

## Requirement Details

### Scan mode

1. Given a folder, traversal through all the files in it and in its subfolders, including hidden files. Don't follow symlinks.
1. Use 8 threads to efficiently traverse.
1. Gather the metadata: file name, file size, last modified timestamp. Also compute a fast checksum.
1. Record those file metadata in sqlite. File path is the key. Path should be normalized.
1. Record scan start and finish time in a log file: ScanLog.txt

### Compare mode

1. Given a folder, traversal through all the files in it and in its subfolders, including hidden files. Don't follow symlinks.
2. Given a sqlite DB created by scan mode, compare with data in the sqlite DB. If any files in the sqlite DB don't exist in the folder, report them in the log file: CompareLog.txt.
3. If any files don't exist in sqlite DB, also report them in the CompareLog.txt.
4. If any files have metadata not same, report them in the CompareLog.txt as well.

## Usage

Command to run: `python main.py scan test_folder --db test.db`
