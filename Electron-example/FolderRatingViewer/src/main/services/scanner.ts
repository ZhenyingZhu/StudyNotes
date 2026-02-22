import * as fs from 'fs';
import * as path from 'path';
import { upsertFiles, removeStaleFiles } from '../db/database';

// Map of common file extensions to human-readable types
const FILE_TYPE_MAP: Record<string, string> = {
  // Images
  '.jpg': 'Image', '.jpeg': 'Image', '.png': 'Image', '.gif': 'Image',
  '.bmp': 'Image', '.svg': 'Image', '.webp': 'Image', '.ico': 'Image', '.tiff': 'Image',
  // Documents
  '.pdf': 'PDF', '.doc': 'Document', '.docx': 'Document',
  '.xls': 'Spreadsheet', '.xlsx': 'Spreadsheet', '.csv': 'CSV',
  '.ppt': 'Presentation', '.pptx': 'Presentation',
  '.txt': 'Text', '.md': 'Markdown', '.rtf': 'Rich Text',
  // Code
  '.js': 'JavaScript', '.ts': 'TypeScript', '.jsx': 'React JSX', '.tsx': 'React TSX',
  '.py': 'Python', '.java': 'Java', '.c': 'C', '.cpp': 'C++', '.h': 'C Header',
  '.cs': 'C#', '.go': 'Go', '.rs': 'Rust', '.rb': 'Ruby', '.php': 'PHP',
  '.swift': 'Swift', '.kt': 'Kotlin', '.scala': 'Scala',
  '.html': 'HTML', '.css': 'CSS', '.scss': 'SCSS', '.less': 'LESS',
  '.json': 'JSON', '.xml': 'XML', '.yaml': 'YAML', '.yml': 'YAML',
  '.sql': 'SQL', '.sh': 'Shell Script', '.bash': 'Shell Script',
  '.ps1': 'PowerShell', '.bat': 'Batch',
  // Archives
  '.zip': 'Archive', '.tar': 'Archive', '.gz': 'Archive', '.rar': 'Archive',
  '.7z': 'Archive', '.bz2': 'Archive',
  // Audio
  '.mp3': 'Audio', '.wav': 'Audio', '.flac': 'Audio', '.aac': 'Audio',
  '.ogg': 'Audio', '.wma': 'Audio', '.m4a': 'Audio',
  // Video
  '.mp4': 'Video', '.avi': 'Video', '.mkv': 'Video', '.mov': 'Video',
  '.wmv': 'Video', '.flv': 'Video', '.webm': 'Video',
  // Executables
  '.exe': 'Executable', '.msi': 'Installer', '.deb': 'Package', '.rpm': 'Package',
  '.dmg': 'Disk Image', '.iso': 'Disk Image', '.AppImage': 'AppImage',
  // Config
  '.ini': 'Config', '.cfg': 'Config', '.conf': 'Config', '.env': 'Config',
  '.toml': 'TOML', '.properties': 'Properties',
  // Fonts
  '.ttf': 'Font', '.otf': 'Font', '.woff': 'Font', '.woff2': 'Font',
  // Data
  '.db': 'Database', '.sqlite': 'Database', '.sqlite3': 'Database',
  '.log': 'Log',
};

function getFileType(fileName: string): string {
  const ext = path.extname(fileName).toLowerCase();
  return FILE_TYPE_MAP[ext] || (ext ? ext.substring(1).toUpperCase() : 'Unknown');
}

let scanState = { scanning: false, scannedCount: 0 };

export function getScanStatus(): { scanning: boolean; scannedCount: number } {
  return { ...scanState };
}

export async function scanFolder(folderPath: string): Promise<void> {
  scanState = { scanning: true, scannedCount: 0 };
  const scanTime = new Date().toISOString();

  try {
    const batch: { filePath: string; fileName: string; fileType: string; fileSize: number; folderPath: string }[] = [];

    function walkDir(dir: string): void {
      let entries: fs.Dirent[];
      try {
        entries = fs.readdirSync(dir, { withFileTypes: true });
      } catch {
        // Skip directories we can't read (permission errors, etc.)
        return;
      }

      for (const entry of entries) {
        const fullPath = path.join(dir, entry.name);

        if (entry.isDirectory()) {
          // Skip hidden directories and common unneeded dirs
          if (!entry.name.startsWith('.') && entry.name !== 'node_modules') {
            walkDir(fullPath);
          }
        } else if (entry.isFile()) {
          try {
            const stats = fs.statSync(fullPath);
            batch.push({
              filePath: fullPath,
              fileName: entry.name,
              fileType: getFileType(entry.name),
              fileSize: stats.size,
              folderPath: folderPath,
            });
            scanState.scannedCount++;

            // Flush batch every 1000 files
            if (batch.length >= 1000) {
              upsertFiles([...batch]);
              batch.length = 0;
            }
          } catch {
            // Skip files we can't stat
          }
        }
      }
    }

    walkDir(folderPath);

    // Flush remaining
    if (batch.length > 0) {
      upsertFiles(batch);
    }

    // Remove files that no longer exist on disk
    removeStaleFiles(folderPath, scanTime);
  } finally {
    scanState.scanning = false;
  }
}
