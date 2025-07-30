"""
Logging utilities for the folder comparison tool.
"""
import os
from datetime import datetime
from typing import List


class ScanLogger:
    """Handles logging for scan operations."""
    
    def __init__(self, log_file: str = "ScanLog.txt"):
        """
        Initialize scan logger.
        
        Args:
            log_file: Path to the log file
        """
        self.log_file = log_file
    
    def log_scan_start(self, folder_path: str):
        """
        Log the start of a scan operation.
        
        Args:
            folder_path: Path being scanned
        """
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        message = f"[{timestamp}] Scan started for folder: {folder_path}\n"
        self._write_log(message)
    
    def log_scan_finish(self, folder_path: str, file_count: int, duration: float):
        """
        Log the completion of a scan operation.
        
        Args:
            folder_path: Path that was scanned
            file_count: Number of files processed
            duration: Time taken in seconds
        """
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        message = f"[{timestamp}] Scan completed for folder: {folder_path}\n"
        message += f"[{timestamp}] Files processed: {file_count}\n"
        message += f"[{timestamp}] Duration: {duration:.2f} seconds\n\n"
        self._write_log(message)
    
    def _write_log(self, message: str):
        """Write message to log file."""
        with open(self.log_file, 'a', encoding='utf-8') as f:
            f.write(message)


class CompareLogger:
    """Handles logging for compare operations."""
    
    def __init__(self, log_file: str = "CompareLog.txt"):
        """
        Initialize compare logger.
        
        Args:
            log_file: Path to the log file
        """
        self.log_file = log_file
        # Clear the log file at the start of each comparison
        with open(self.log_file, 'w', encoding='utf-8') as f:
            timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
            f.write(f"[{timestamp}] Comparison started\n\n")
    
    def log_missing_from_filesystem(self, file_paths: List[str]):
        """
        Log files that exist in database but not in filesystem.
        
        Args:
            file_paths: List of file paths missing from filesystem
        """
        if not file_paths:
            return
            
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        message = f"[{timestamp}] FILES MISSING FROM FILESYSTEM ({len(file_paths)} files):\n"
        for path in file_paths:
            message += f"  - {path}\n"
        message += "\n"
        self._write_log(message)
    
    def log_missing_from_database(self, file_paths: List[str]):
        """
        Log files that exist in filesystem but not in database.
        
        Args:
            file_paths: List of file paths missing from database
        """
        if not file_paths:
            return
            
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        message = f"[{timestamp}] FILES MISSING FROM DATABASE ({len(file_paths)} files):\n"
        for path in file_paths:
            message += f"  - {path}\n"
        message += "\n"
        self._write_log(message)
    
    def log_metadata_mismatches(self, mismatches: List[dict]):
        """
        Log files with metadata mismatches.
        
        Args:
            mismatches: List of dictionaries containing mismatch details
        """
        if not mismatches:
            return
            
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        message = f"[{timestamp}] METADATA MISMATCHES ({len(mismatches)} files):\n"
        
        for mismatch in mismatches:
            path = mismatch['path']
            differences = mismatch['differences']
            message += f"  File: {path}\n"
            
            for diff_type, values in differences.items():
                if diff_type == 'size':
                    message += f"    Size: DB={values['db']}, FS={values['fs']}\n"
                elif diff_type == 'modified_time':
                    db_time = datetime.fromtimestamp(values['db']).strftime("%Y-%m-%d %H:%M:%S")
                    fs_time = datetime.fromtimestamp(values['fs']).strftime("%Y-%m-%d %H:%M:%S")
                    message += f"    Modified Time: DB={db_time}, FS={fs_time}\n"
                elif diff_type == 'checksum':
                    message += f"    Checksum: DB={values['db']}, FS={values['fs']}\n"
            message += "\n"
        
        self._write_log(message)
    
    def log_comparison_summary(self, total_files_fs: int, total_files_db: int, 
                             missing_from_fs: int, missing_from_db: int, 
                             mismatched: int):
        """
        Log a summary of the comparison results.
        
        Args:
            total_files_fs: Total files found in filesystem
            total_files_db: Total files found in database
            missing_from_fs: Number of files missing from filesystem
            missing_from_db: Number of files missing from database
            mismatched: Number of files with metadata mismatches
        """
        timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        message = f"[{timestamp}] COMPARISON SUMMARY:\n"
        message += f"  Files in filesystem: {total_files_fs}\n"
        message += f"  Files in database: {total_files_db}\n"
        message += f"  Missing from filesystem: {missing_from_fs}\n"
        message += f"  Missing from database: {missing_from_db}\n"
        message += f"  Metadata mismatches: {mismatched}\n"
        
        if missing_from_fs == 0 and missing_from_db == 0 and mismatched == 0:
            message += "  Status: FOLDERS ARE IN SYNC\n"
        else:
            message += "  Status: DIFFERENCES FOUND\n"
        
        message += "\n"
        self._write_log(message)
    
    def _write_log(self, message: str):
        """Write message to log file."""
        with open(self.log_file, 'a', encoding='utf-8') as f:
            f.write(message)
