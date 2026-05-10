import * as fs from 'fs';
import * as path from 'path';
import { upsertFiles, removeStaleFiles } from '../db/database';

const DB_BATCH_SIZE = 1000;
const STAT_CONCURRENCY = 32;

const ARCHIVE_EXTENSIONS = new Set([
  '.zip', '.tar', '.gz', '.tgz', '.rar', '.7z', '.bz2', '.xz', '.zst', '.tar.gz', '.tar.bz2', '.tar.xz',
]);

function isCompressedFile(fileName: string): boolean {
  const lowerName = fileName.toLowerCase();
  if (lowerName.endsWith('.tar.gz') || lowerName.endsWith('.tar.bz2') || lowerName.endsWith('.tar.xz')) {
    return true;
  }

  const ext = path.extname(lowerName);
  return ARCHIVE_EXTENSIONS.has(ext);
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
    const directoriesToVisit: string[] = [folderPath];

    const flushBatch = (): void => {
      if (batch.length === 0) return;
      upsertFiles([...batch]);
      batch.length = 0;
    };

    const pushRecord = (record: { filePath: string; fileName: string; fileType: string; fileSize: number; folderPath: string }): void => {
      batch.push(record);
      scanState.scannedCount++;

      if (batch.length >= DB_BATCH_SIZE) {
        flushBatch();
      }
    };

    async function processArchiveFiles(dir: string, entries: fs.Dirent[]): Promise<void> {
      const archiveEntries = entries.filter((entry) => entry.isFile() && isCompressedFile(entry.name));
      for (let i = 0; i < archiveEntries.length; i += STAT_CONCURRENCY) {
        const slice = archiveEntries.slice(i, i + STAT_CONCURRENCY);
        const records = await Promise.all(
          slice.map(async (entry) => {
            const fullPath = path.join(dir, entry.name);
            try {
              const stats = await fs.promises.stat(fullPath);
              return {
                filePath: fullPath,
                fileName: entry.name,
                fileType: 'Archive',
                fileSize: stats.size,
                folderPath,
              };
            } catch {
              return null;
            }
          })
        );

        for (const record of records) {
          if (record) {
            pushRecord(record);
          }
        }
      }
    }

    while (directoriesToVisit.length > 0) {
      const dir = directoriesToVisit.pop();
      if (!dir) continue;

      let entries: fs.Dirent[];
      try {
        entries = await fs.promises.readdir(dir, { withFileTypes: true });
      } catch {
        continue;
      }

      const visibleDirectories = entries.filter(
        (entry) => entry.isDirectory() && !entry.name.startsWith('.') && entry.name !== 'node_modules'
      );

      await processArchiveFiles(dir, entries);

      if (visibleDirectories.length === 0) {
        pushRecord({
          filePath: dir,
          fileName: path.basename(dir),
          fileType: 'Folder',
          fileSize: 0,
          folderPath,
        });
        continue;
      }

      for (const entry of visibleDirectories) {
        directoriesToVisit.push(path.join(dir, entry.name));
      }
    }

    // Flush remaining
    flushBatch();

    // Remove files that no longer exist on disk
    removeStaleFiles(folderPath, scanTime);
  } finally {
    scanState.scanning = false;
  }
}
