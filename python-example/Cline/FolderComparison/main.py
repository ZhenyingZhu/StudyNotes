"""
Main CLI interface for the folder comparison tool.
"""
import argparse
import sys
import os
from database import DatabaseManager
from scanner import FileScanner
from comparator import FolderComparator
from logger import ScanLogger, CompareLogger


def scan_mode(folder_path: str, db_path: str, threads: int = 8):
    """
    Execute scan mode operation.
    
    Args:
        folder_path: Path to the folder to scan
        db_path: Path to the SQLite database
        threads: Number of threads to use
    """
    try:
        # Initialize components
        db_manager = DatabaseManager(db_path)
        scanner = FileScanner(threads)
        logger = ScanLogger()
        
        # Perform scan
        file_count = scanner.scan_folder(folder_path, db_manager, logger)
        
        print(f"\nScan completed successfully!")
        print(f"Database: {db_path}")
        print(f"Files processed: {file_count}")
        print(f"Log file: ScanLog.txt")
        
    except Exception as e:
        print(f"Error during scan: {e}")
        sys.exit(1)


def compare_mode(folder_path: str, db_path: str, threads: int = 8):
    """
    Execute compare mode operation.
    
    Args:
        folder_path: Path to the folder to compare
        db_path: Path to the SQLite database
        threads: Number of threads to use
    """
    try:
        # Check if database exists
        if not os.path.exists(db_path):
            print(f"Error: Database file not found: {db_path}")
            print("Please run scan mode first to create the database.")
            sys.exit(1)
        
        # Initialize components
        db_manager = DatabaseManager(db_path)
        comparator = FolderComparator(threads)
        logger = CompareLogger()
        
        # Check if database has any data
        file_count = db_manager.get_file_count()
        if file_count == 0:
            print(f"Error: Database is empty: {db_path}")
            print("Please run scan mode first to populate the database.")
            sys.exit(1)
        
        # Perform comparison
        stats = comparator.compare_folder_with_database(folder_path, db_manager, logger)
        
        print(f"\nComparison completed successfully!")
        print(f"Database: {db_path}")
        print(f"Log file: CompareLog.txt")
        
    except Exception as e:
        print(f"Error during comparison: {e}")
        sys.exit(1)


def main():
    """Main entry point for the CLI application."""
    parser = argparse.ArgumentParser(
        description="Folder Comparison Tool - Compare folders and their backups",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  # Scan a folder and create database
  python main.py scan /path/to/folder --db backup.db
  
  # Compare folder with existing database
  python main.py compare /path/to/folder --db backup.db
  
  # Use custom number of threads
  python main.py scan /path/to/folder --db backup.db --threads 16
        """
    )
    
    # Add subcommands
    subparsers = parser.add_subparsers(dest='mode', help='Operation mode')
    subparsers.required = True
    
    # Scan mode
    scan_parser = subparsers.add_parser('scan', help='Scan folder and create database')
    scan_parser.add_argument('folder', help='Path to the folder to scan')
    scan_parser.add_argument('--db', required=True, help='Path to the SQLite database file')
    scan_parser.add_argument('--threads', type=int, default=8, 
                           help='Number of threads to use (default: 8)')
    
    # Compare mode
    compare_parser = subparsers.add_parser('compare', help='Compare folder with database')
    compare_parser.add_argument('folder', help='Path to the folder to compare')
    compare_parser.add_argument('--db', required=True, help='Path to the SQLite database file')
    compare_parser.add_argument('--threads', type=int, default=8,
                              help='Number of threads to use (default: 8)')
    
    # Parse arguments
    args = parser.parse_args()
    
    # Validate folder path
    if not os.path.exists(args.folder):
        print(f"Error: Folder not found: {args.folder}")
        sys.exit(1)
    
    if not os.path.isdir(args.folder):
        print(f"Error: Path is not a directory: {args.folder}")
        sys.exit(1)
    
    # Validate thread count
    if args.threads < 1:
        print("Error: Thread count must be at least 1")
        sys.exit(1)
    
    if args.threads > 32:
        print("Warning: Using more than 32 threads may not improve performance")
    
    # Execute the appropriate mode
    if args.mode == 'scan':
        print(f"Starting scan mode...")
        print(f"Folder: {args.folder}")
        print(f"Database: {args.db}")
        print(f"Threads: {args.threads}")
        print()
        scan_mode(args.folder, args.db, args.threads)
        
    elif args.mode == 'compare':
        print(f"Starting compare mode...")
        print(f"Folder: {args.folder}")
        print(f"Database: {args.db}")
        print(f"Threads: {args.threads}")
        print()
        compare_mode(args.folder, args.db, args.threads)


if __name__ == '__main__':
    main()
