"""
File scanning logic for the folder comparison tool.
"""
import os
import time
from concurrent.futures import ThreadPoolExecutor, as_completed
from queue import Queue
from typing import List, Set, Generator, Optional
from metadata import MetadataCollector, FileMetadata
from database import DatabaseManager
from logger import ScanLogger


class FileScanner:
    """Handles multi-threaded file system scanning."""
    
    def __init__(self, num_threads: int = 8):
        """
        Initialize file scanner.
        
        Args:
            num_threads: Number of threads to use for scanning
        """
        self.num_threads = num_threads
    
    def scan_folder(self, folder_path: str, db_manager: DatabaseManager, 
                   logger: ScanLogger) -> int:
        """
        Scan a folder and store metadata in database.
        
        Args:
            folder_path: Path to the folder to scan
            db_manager: Database manager instance
            logger: Scan logger instance
            
        Returns:
            Number of files processed
        """
        folder_path = os.path.normpath(os.path.abspath(folder_path))
        
        if not os.path.exists(folder_path):
            raise FileNotFoundError(f"Folder not found: {folder_path}")
        
        if not os.path.isdir(folder_path):
            raise NotADirectoryError(f"Path is not a directory: {folder_path}")
        
        logger.log_scan_start(folder_path)
        start_time = time.time()
        
        # Clear existing data for this scan
        db_manager.clear_database()
        
        # Get all files to process
        print(f"Discovering files in {folder_path}...")
        file_paths = list(self._discover_files(folder_path))
        print(f"Found {len(file_paths)} files to process")
        
        if not file_paths:
            logger.log_scan_finish(folder_path, 0, time.time() - start_time)
            return 0
        
        # Process files with multiple threads
        processed_count = 0
        batch_size = 100  # Process files in batches for better database performance
        metadata_batch = []
        
        print("Processing files...")
        with ThreadPoolExecutor(max_workers=self.num_threads) as executor:
            # Submit all file processing tasks
            future_to_path = {
                executor.submit(self._get_file_metadata_with_relative_path, file_path, folder_path): file_path
                for file_path in file_paths
            }
            
            # Process completed tasks
            for future in as_completed(future_to_path):
                file_path = future_to_path[future]
                try:
                    metadata = future.result()
                    if metadata:
                        metadata_batch.append(metadata)
                        processed_count += 1
                        
                        # Insert batch when it reaches the batch size
                        if len(metadata_batch) >= batch_size:
                            db_manager.insert_file_metadata_batch(metadata_batch)
                            metadata_batch = []
                        
                        # Progress indicator
                        if processed_count % 1000 == 0:
                            print(f"Processed {processed_count}/{len(file_paths)} files...")
                            
                except Exception as e:
                    print(f"Error processing {file_path}: {e}")
            
            # Insert remaining metadata
            if metadata_batch:
                db_manager.insert_file_metadata_batch(metadata_batch)
        
        duration = time.time() - start_time
        logger.log_scan_finish(folder_path, processed_count, duration)
        
        print(f"Scan completed: {processed_count} files processed in {duration:.2f} seconds")
        return processed_count
    
    def _discover_files(self, folder_path: str) -> Generator[str, None, None]:
        """
        Discover all files in the folder and subfolders.
        
        Args:
            folder_path: Root folder path
            
        Yields:
            File paths
        """
        try:
            for root, dirs, files in os.walk(folder_path):
                # Process files in current directory
                for file_name in files:
                    file_path = os.path.join(root, file_name)
                    
                    # Skip symlinks as per requirements
                    if not os.path.islink(file_path):
                        yield file_path
                
                # Handle hidden directories on Unix-like systems
                # os.walk already includes hidden directories by default
                
        except (OSError, PermissionError) as e:
            print(f"Warning: Cannot access directory {folder_path}: {e}")
    
    def get_current_files(self, folder_path: str) -> List[FileMetadata]:
        """
        Get current file metadata for a folder (used in compare mode).
        
        Args:
            folder_path: Path to the folder to scan
            
        Returns:
            List of FileMetadata objects
        """
        folder_path = os.path.normpath(os.path.abspath(folder_path))
        
        if not os.path.exists(folder_path):
            raise FileNotFoundError(f"Folder not found: {folder_path}")
        
        if not os.path.isdir(folder_path):
            raise NotADirectoryError(f"Path is not a directory: {folder_path}")
        
        # Get all files to process
        file_paths = list(self._discover_files(folder_path))
        
        if not file_paths:
            return []
        
        # Process files with multiple threads
        current_files = []
        
        with ThreadPoolExecutor(max_workers=self.num_threads) as executor:
            # Submit all file processing tasks
            future_to_path = {
                executor.submit(MetadataCollector.get_file_metadata, file_path): file_path
                for file_path in file_paths
            }
            
            # Process completed tasks
            for future in as_completed(future_to_path):
                file_path = future_to_path[future]
                try:
                    metadata = future.result()
                    if metadata:
                        current_files.append(metadata)
                        
                except Exception as e:
                    print(f"Error processing {file_path}: {e}")
        
        return current_files
    
    def _get_file_metadata_with_relative_path(self, file_path: str, root_folder: str) -> Optional[FileMetadata]:
        """
        Get file metadata with relative path from root folder.
        
        Args:
            file_path: Absolute path to the file
            root_folder: Root folder path to make relative to
            
        Returns:
            FileMetadata object with relative path or None if file cannot be accessed
        """
        metadata = MetadataCollector.get_file_metadata(file_path)
        if metadata:
            # Convert absolute path to relative path from root folder
            relative_path = os.path.relpath(file_path, root_folder)
            metadata.path = relative_path
        return metadata
