"""
Comparison logic for the folder comparison tool.
"""
import os
from typing import List, Dict, Tuple
from metadata import FileMetadata
from database import DatabaseManager
from scanner import FileScanner
from logger import CompareLogger


class FolderComparator:
    """Handles folder comparison operations."""
    
    def __init__(self, num_threads: int = 8):
        """
        Initialize folder comparator.
        
        Args:
            num_threads: Number of threads to use for scanning
        """
        self.scanner = FileScanner(num_threads)
    
    def compare_folder_with_database(self, folder_path: str, db_manager: DatabaseManager,
                                   logger: CompareLogger) -> Dict[str, int]:
        """
        Compare a folder with the database and log differences.
        
        Args:
            folder_path: Path to the folder to compare
            db_manager: Database manager instance
            logger: Compare logger instance
            
        Returns:
            Dictionary with comparison statistics
        """
        folder_path = os.path.normpath(os.path.abspath(folder_path))
        
        if not os.path.exists(folder_path):
            raise FileNotFoundError(f"Folder not found: {folder_path}")
        
        if not os.path.isdir(folder_path):
            raise NotADirectoryError(f"Path is not a directory: {folder_path}")
        
        print(f"Comparing folder {folder_path} with database...")
        
        # Get current files from filesystem
        print("Scanning current filesystem...")
        current_files = self.scanner.get_current_files(folder_path)
        # Convert to relative paths for comparison
        for file_metadata in current_files:
            file_metadata.path = os.path.relpath(file_metadata.path, folder_path)
        current_files_dict = {f.path: f for f in current_files}
        
        # Get files from database
        print("Loading database records...")
        db_files_dict = db_manager.get_all_files()
        
        print(f"Found {len(current_files)} files in filesystem")
        print(f"Found {len(db_files_dict)} files in database")
        
        # Find differences
        missing_from_fs, missing_from_db, mismatches = self._find_differences(
            current_files_dict, db_files_dict
        )
        
        # Log results
        logger.log_missing_from_filesystem(missing_from_fs)
        logger.log_missing_from_database(missing_from_db)
        logger.log_metadata_mismatches(mismatches)
        
        # Log summary
        stats = {
            'total_files_fs': len(current_files),
            'total_files_db': len(db_files_dict),
            'missing_from_fs': len(missing_from_fs),
            'missing_from_db': len(missing_from_db),
            'mismatched': len(mismatches)
        }
        
        logger.log_comparison_summary(**stats)
        
        # Print summary to console
        print("\nComparison Summary:")
        print(f"  Files in filesystem: {stats['total_files_fs']}")
        print(f"  Files in database: {stats['total_files_db']}")
        print(f"  Missing from filesystem: {stats['missing_from_fs']}")
        print(f"  Missing from database: {stats['missing_from_db']}")
        print(f"  Metadata mismatches: {stats['mismatched']}")
        
        if stats['missing_from_fs'] == 0 and stats['missing_from_db'] == 0 and stats['mismatched'] == 0:
            print("  Status: FOLDERS ARE IN SYNC")
        else:
            print("  Status: DIFFERENCES FOUND")
            print("  Check CompareLog.txt for detailed differences")
        
        return stats
    
    def _find_differences(self, current_files: Dict[str, FileMetadata], 
                         db_files: Dict[str, FileMetadata]) -> Tuple[List[str], List[str], List[dict]]:
        """
        Find differences between current files and database files.
        
        Args:
            current_files: Dictionary of current filesystem files
            db_files: Dictionary of database files
            
        Returns:
            Tuple of (missing_from_filesystem, missing_from_database, metadata_mismatches)
        """
        current_paths = set(current_files.keys())
        db_paths = set(db_files.keys())
        
        # Files in database but not in filesystem
        missing_from_fs = sorted(list(db_paths - current_paths))
        
        # Files in filesystem but not in database
        missing_from_db = sorted(list(current_paths - db_paths))
        
        # Files present in both - check for metadata differences
        common_paths = current_paths & db_paths
        mismatches = []
        
        for path in common_paths:
            current_file = current_files[path]
            db_file = db_files[path]
            
            differences = self._compare_metadata(current_file, db_file)
            if differences:
                mismatches.append({
                    'path': path,
                    'differences': differences
                })
        
        # Sort mismatches by path for consistent output
        mismatches.sort(key=lambda x: x['path'])
        
        return missing_from_fs, missing_from_db, mismatches
    
    def _compare_metadata(self, current: FileMetadata, db: FileMetadata) -> Dict[str, Dict[str, any]]:
        """
        Compare metadata between two FileMetadata objects.
        
        Args:
            current: Current filesystem file metadata
            db: Database file metadata
            
        Returns:
            Dictionary of differences, empty if no differences
        """
        differences = {}
        
        # Compare file size
        if current.size != db.size:
            differences['size'] = {
                'fs': current.size,
                'db': db.size
            }
        
        # Compare modified time (with small tolerance for filesystem precision)
        time_diff = abs(current.modified_time - db.modified_time)
        if time_diff > 1.0:  # Allow 1 second tolerance
            differences['modified_time'] = {
                'fs': current.modified_time,
                'db': db.modified_time
            }
        
        # Compare checksum
        if current.checksum != db.checksum:
            differences['checksum'] = {
                'fs': current.checksum,
                'db': db.checksum
            }
        
        return differences
