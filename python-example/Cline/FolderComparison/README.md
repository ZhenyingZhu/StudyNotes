# Folder Comparison Tool

A Python-based tool for comparing folders and their backups using multi-threaded scanning and SQLite database storage.

## Features

- **Scan Mode**: Traverse folders and subfolders, collect file metadata, and store in SQLite database
- **Compare Mode**: Compare current folder state with previously scanned database
- **Multi-threaded**: Uses 8 threads by default for efficient file processing
- **Fast Checksums**: Uses CRC32 for quick file integrity checking
- **Comprehensive Logging**: Detailed logs for both scan and compare operations
- **Cross-platform**: Works on Windows, macOS, and Linux

## Requirements

- Python 3.7 or higher
- No external dependencies (uses only Python standard library)

## Installation

1. Clone or download the project files
2. Ensure Python 3.7+ is installed
3. No additional installation required - uses only standard library modules

## Usage

### Scan Mode

Scan a folder and create a database with file metadata:

```bash
python main.py scan /path/to/folder --db backup.db
```

This will:
- Traverse all files and subfolders (including hidden files)
- Skip symbolic links
- Calculate file size, modification time, and CRC32 checksum
- Store metadata in SQLite database
- Log scan start/finish times to `ScanLog.txt`

### Compare Mode

Compare a folder with a previously created database:

```bash
python main.py compare /path/to/folder --db backup.db
```

This will:
- Scan the current folder structure
- Compare with database records
- Report differences in `CompareLog.txt`:
  - Files missing from filesystem
  - Files missing from database
  - Files with different metadata (size, modification time, checksum)

### Options

- `--threads N`: Use N threads for processing (default: 8)
- `--db FILE`: Specify database file path (required)

### Examples

```bash
# Scan with custom thread count
python main.py scan /home/user/documents --db docs_backup.db --threads 16

# Compare using existing database
python main.py compare /home/user/documents --db docs_backup.db

# Scan Windows folder
python main.py scan "C:\Users\Username\Documents" --db documents.db
```

## Output Files

### ScanLog.txt
Contains scan operation logs with timestamps:
- Scan start time
- Scan completion time
- Number of files processed
- Duration

### CompareLog.txt
Contains comparison results:
- Files missing from filesystem
- Files missing from database
- Metadata mismatches with details
- Comparison summary

### Database Structure
SQLite database with `files` table:
- `path` (TEXT PRIMARY KEY): Normalized file path
- `size` (INTEGER): File size in bytes
- `modified_time` (REAL): Last modification timestamp
- `checksum` (TEXT): CRC32 checksum in hexadecimal

## Architecture

The tool consists of several modular components:

- `main.py`: CLI interface and argument parsing
- `metadata.py`: File metadata collection and checksum calculation
- `database.py`: SQLite database operations with thread safety
- `scanner.py`: Multi-threaded file system traversal
- `comparator.py`: Folder comparison logic
- `logger.py`: Logging utilities for scan and compare operations

## Performance

- Uses multi-threading for I/O-bound operations
- Batch database inserts for better performance
- CRC32 checksums for fast file integrity checking
- Progress indicators for large folder scans

## Error Handling

- Graceful handling of permission denied errors
- Skips inaccessible files with warnings
- Validates input paths and database existence
- Thread-safe database operations

## Limitations

- Symbolic links are skipped (as per requirements)
- CRC32 checksums are fast but not cryptographically secure
- Memory usage scales with number of files being processed
- Database is cleared on each scan (not incremental)

## Troubleshooting

### Common Issues

1. **Permission Denied**: Run with appropriate permissions or skip protected folders
2. **Database Locked**: Ensure no other processes are using the database file
3. **Out of Memory**: Reduce thread count for very large folders
4. **Path Not Found**: Use absolute paths or verify folder existence

### Performance Tips

- Use SSD storage for better I/O performance
- Adjust thread count based on storage type and CPU cores
- Consider excluding temporary or cache folders from scans
