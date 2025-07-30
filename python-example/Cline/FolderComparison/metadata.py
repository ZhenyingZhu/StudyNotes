"""
File metadata handling for the folder comparison tool.
"""
import os
import hashlib
import zlib
from dataclasses import dataclass
from typing import Optional


@dataclass
class FileMetadata:
    """Represents metadata for a single file."""
    path: str
    size: int
    modified_time: float
    checksum: str
    
    def __post_init__(self):
        """Normalize the path after initialization."""
        self.path = os.path.normpath(self.path)


class MetadataCollector:
    """Collects metadata for files."""
    
    @staticmethod
    def get_file_metadata(file_path: str) -> Optional[FileMetadata]:
        """
        Get metadata for a single file.
        
        Args:
            file_path: Path to the file
            
        Returns:
            FileMetadata object or None if file cannot be accessed
        """
        try:
            # Skip symlinks as per requirements
            if os.path.islink(file_path):
                return None
                
            # Get file stats
            stat = os.stat(file_path)
            
            # Calculate fast checksum using CRC32
            checksum = MetadataCollector._calculate_checksum(file_path)
            if checksum is None:
                return None
                
            return FileMetadata(
                path=file_path,
                size=stat.st_size,
                modified_time=stat.st_mtime,
                checksum=checksum
            )
            
        except (OSError, IOError, PermissionError) as e:
            print(f"Warning: Cannot access file {file_path}: {e}")
            return None
    
    @staticmethod
    def _calculate_checksum(file_path: str) -> Optional[str]:
        """
        Calculate CRC32 checksum for a file.
        
        Args:
            file_path: Path to the file
            
        Returns:
            Hexadecimal checksum string or None if error
        """
        try:
            crc = 0
            with open(file_path, 'rb') as f:
                while True:
                    chunk = f.read(8192)  # Read in 8KB chunks
                    if not chunk:
                        break
                    crc = zlib.crc32(chunk, crc)
            
            # Convert to unsigned 32-bit integer and then to hex
            return format(crc & 0xffffffff, '08x')
            
        except (OSError, IOError, PermissionError) as e:
            print(f"Warning: Cannot calculate checksum for {file_path}: {e}")
            return None
