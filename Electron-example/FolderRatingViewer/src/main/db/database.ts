import Database from 'better-sqlite3';
import * as path from 'path';
import { app } from 'electron';

let db: Database.Database | null = null;

export function initDatabase(): void {
  const dbPath = path.join(app.getPath('userData'), 'file-indexer.db');
  db = new Database(dbPath);

  // Enable WAL mode for better performance with many writes
  db.pragma('journal_mode = WAL');

  db.exec(`
    CREATE TABLE IF NOT EXISTS files (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      file_path TEXT NOT NULL UNIQUE,
      file_name TEXT NOT NULL,
      file_type TEXT NOT NULL DEFAULT '',
      file_size INTEGER NOT NULL DEFAULT 0,
      folder_path TEXT NOT NULL,
      last_seen TEXT NOT NULL,
      created_at TEXT NOT NULL DEFAULT (datetime('now'))
    );

    CREATE INDEX IF NOT EXISTS idx_files_folder ON files(folder_path);
    CREATE INDEX IF NOT EXISTS idx_files_path ON files(file_path);
    CREATE INDEX IF NOT EXISTS idx_files_type ON files(file_type);

    CREATE TABLE IF NOT EXISTS ratings (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      file_path TEXT NOT NULL UNIQUE,
      rating INTEGER NOT NULL DEFAULT 0 CHECK(rating >= 0 AND rating <= 5),
      updated_at TEXT NOT NULL DEFAULT (datetime('now'))
    );

    CREATE INDEX IF NOT EXISTS idx_ratings_path ON ratings(file_path);
  `);
}

export function getDatabase(): Database.Database | null {
  return db;
}

export interface FileRecord {
  id: number;
  file_path: string;
  file_name: string;
  file_type: string;
  file_size: number;
  folder_path: string;
  rating: number;
}

export function upsertFiles(files: { filePath: string; fileName: string; fileType: string; fileSize: number; folderPath: string }[]): void {
  if (!db) throw new Error('Database not initialized');

  const now = new Date().toISOString();
  const stmt = db.prepare(`
    INSERT INTO files (file_path, file_name, file_type, file_size, folder_path, last_seen)
    VALUES (?, ?, ?, ?, ?, ?)
    ON CONFLICT(file_path) DO UPDATE SET
      file_name = excluded.file_name,
      file_type = excluded.file_type,
      file_size = excluded.file_size,
      folder_path = excluded.folder_path,
      last_seen = excluded.last_seen
  `);

  const batchInsert = db.transaction((items: typeof files) => {
    for (const f of items) {
      stmt.run(f.filePath, f.fileName, f.fileType, f.fileSize, f.folderPath, now);
    }
  });

  // Process in batches of 500 for performance
  for (let i = 0; i < files.length; i += 500) {
    batchInsert(files.slice(i, i + 500));
  }
}

export function removeStaleFiles(folderPath: string, currentScanTime: string): number {
  if (!db) throw new Error('Database not initialized');
  const result = db.prepare(`DELETE FROM files WHERE folder_path = ? AND last_seen < ?`).run(folderPath, currentScanTime);
  return result.changes;
}

export function getFiles(options: {
  folderPath: string;
  sortBy?: string;
  sortOrder?: string;
  search?: string;
  page?: number;
  pageSize?: number;
}): { files: FileRecord[]; total: number } {
  if (!db) throw new Error('Database not initialized');

  const { folderPath, sortBy = 'file_name', sortOrder = 'ASC', search = '', page = 1, pageSize = 100 } = options;

  // Whitelist sortBy columns to prevent SQL injection
  const allowedSortColumns: Record<string, string> = {
    file_name: 'f.file_name',
    file_type: 'f.file_type',
    file_size: 'f.file_size',
    file_path: 'f.file_path',
    rating: 'COALESCE(r.rating, 0)',
  };
  const sortCol = allowedSortColumns[sortBy] || 'f.file_name';
  const order = sortOrder === 'DESC' ? 'DESC' : 'ASC';

  let whereClause = 'WHERE f.folder_path = ?';
  const params: any[] = [folderPath];

  if (search) {
    whereClause += ' AND (f.file_name LIKE ? OR f.file_path LIKE ? OR f.file_type LIKE ?)';
    const searchParam = `%${search}%`;
    params.push(searchParam, searchParam, searchParam);
  }

  const countStmt = db.prepare(`SELECT COUNT(*) as total FROM files f ${whereClause}`);
  const { total } = countStmt.get(...params) as { total: number };

  const offset = (page - 1) * pageSize;
  const dataStmt = db.prepare(`
    SELECT f.id, f.file_path, f.file_name, f.file_type, f.file_size, f.folder_path,
           COALESCE(r.rating, 0) as rating
    FROM files f
    LEFT JOIN ratings r ON f.file_path = r.file_path
    ${whereClause}
    ORDER BY ${sortCol} ${order}
    LIMIT ? OFFSET ?
  `);
  params.push(pageSize, offset);
  const files = dataStmt.all(...params) as FileRecord[];

  return { files, total };
}

export function setRating(filePath: string, rating: number): void {
  if (!db) throw new Error('Database not initialized');
  if (rating < 0 || rating > 5) throw new Error('Rating must be between 0 and 5');

  db.prepare(`
    INSERT INTO ratings (file_path, rating, updated_at)
    VALUES (?, ?, datetime('now'))
    ON CONFLICT(file_path) DO UPDATE SET
      rating = excluded.rating,
      updated_at = excluded.updated_at
  `).run(filePath, rating);
}
