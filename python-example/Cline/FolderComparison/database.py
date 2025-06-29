"""
SQLite database operations for the folder comparison tool.
"""
import sqlite3
import threading
from typing import List, Optional, Dict
from metadata import FileMetadata


class DatabaseManager:
    """Manages SQLite database operations for file metadata."""
    
    def __init__(self, db_path: str):
        """
        Initialize database manager.
        
        Args:
            db_path: Path to the SQLite database file
        """
        self.db_path = db_path
        self._lock = threading.Lock()
        self._init_database()
    
    def _init_database(self):
        """Initialize the database schema."""
        with sqlite3.connect(self.db_path) as conn:
            conn.execute('''
                CREATE TABLE IF NOT EXISTS files (
                    path TEXT PRIMARY KEY,
                    size INTEGER NOT NULL,
                    modified_time REAL NOT NULL,
                    checksum TEXT NOT NULL
                )
            ''')
            conn.commit()
    
    def insert_file_metadata(self, metadata: FileMetadata):
        """
        Insert a single file metadata record.
        
        Args:
            metadata: FileMetadata object to insert
        """
        with self._lock:
            with sqlite3.connect(self.db_path) as conn:
                conn.execute('''
                    INSERT OR REPLACE INTO files 
                    (path, size, modified_time, checksum) 
                    VALUES (?, ?, ?, ?)
                ''', (metadata.path, metadata.size, metadata.modified_time, metadata.checksum))
                conn.commit()
    
    def insert_file_metadata_batch(self, metadata_list: List[FileMetadata]):
        """
        Insert multiple file metadata records in a batch.
        
        Args:
            metadata_list: List of FileMetadata objects to insert
        """
        if not metadata_list:
            return
            
        with self._lock:
            with sqlite3.connect(self.db_path) as conn:
                data = [(m.path, m.size, m.modified_time, m.checksum) for m in metadata_list]
                conn.executemany('''
                    INSERT OR REPLACE INTO files 
                    (path, size, modified_time, checksum) 
                    VALUES (?, ?, ?, ?)
                ''', data)
                conn.commit()
    
    def get_all_files(self) -> Dict[str, FileMetadata]:
        """
        Get all file metadata from the database.
        
        Returns:
            Dictionary mapping file paths to FileMetadata objects
        """
        with sqlite3.connect(self.db_path) as conn:
            cursor = conn.execute('SELECT path, size, modified_time, checksum FROM files')
            result = {}
            for row in cursor:
                path, size, modified_time, checksum = row
                result[path] = FileMetadata(path, size, modified_time, checksum)
            return result
    
    def get_file_metadata(self, file_path: str) -> Optional[FileMetadata]:
        """
        Get metadata for a specific file.
        
        Args:
            file_path: Normalized path to the file
            
        Returns:
            FileMetadata object or None if not found
        """
        with sqlite3.connect(self.db_path) as conn:
            cursor = conn.execute(
                'SELECT path, size, modified_time, checksum FROM files WHERE path = ?',
                (file_path,)
            )
            row = cursor.fetchone()
            if row:
                path, size, modified_time, checksum = row
                return FileMetadata(path, size, modified_time, checksum)
            return None
    
    def clear_database(self):
        """Clear all records from the database."""
        with self._lock:
            with sqlite3.connect(self.db_path) as conn:
                conn.execute('DELETE FROM files')
                conn.commit()
    
    def get_file_count(self) -> int:
        """Get the total number of files in the database."""
        with sqlite3.connect(self.db_path) as conn:
            cursor = conn.execute('SELECT COUNT(*) FROM files')
            return cursor.fetchone()[0]
