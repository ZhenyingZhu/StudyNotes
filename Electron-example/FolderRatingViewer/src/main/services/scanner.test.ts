import { beforeEach, describe, expect, it, vi } from 'vitest';
import * as path from 'path';

const readdirMock = vi.fn();
const statMock = vi.fn();
const upsertFilesMock = vi.fn();
const markStaleFilesDeletedMock = vi.fn();

vi.mock('fs', () => ({
  promises: {
    readdir: readdirMock,
    stat: statMock,
  },
}));

vi.mock('../db/database', () => ({
  upsertFiles: upsertFilesMock,
  markStaleFilesDeleted: markStaleFilesDeletedMock,
}));

type MockDirent = {
  name: string;
  isDirectory: () => boolean;
  isFile: () => boolean;
};

function dir(name: string): MockDirent {
  return {
    name,
    isDirectory: () => true,
    isFile: () => false,
  };
}

function file(name: string): MockDirent {
  return {
    name,
    isDirectory: () => false,
    isFile: () => true,
  };
}

function normalizePath(value: string): string {
  return value.replace(/\\/g, '/');
}

describe('scanFolder', () => {
  beforeEach(() => {
    vi.resetModules();
    readdirMock.mockReset();
    statMock.mockReset();
    upsertFilesMock.mockReset();
    markStaleFilesDeletedMock.mockReset();
  });

  it('indexes only archive files and leaf folders', async () => {
    const root = '/root';

    readdirMock.mockImplementation(async (target: string) => {
      const normalized = normalizePath(target);
      if (normalized === '/root') {
        return [dir('A'), dir('node_modules'), dir('.hidden'), file('readme.txt'), file('pack.zip')];
      }
      if (normalized === '/root/A') {
        return [dir('B'), file('notes.txt')];
      }
      if (normalized === '/root/A/B') {
        return [file('code.tar.gz'), file('image.png')];
      }
      return [];
    });

    statMock.mockImplementation(async (target: string) => {
      const normalized = normalizePath(target);
      if (normalized === '/root/pack.zip') return { size: 123 };
      if (normalized === '/root/A/B/code.tar.gz') return { size: 456 };
      throw new Error(`Unexpected stat call: ${target}`);
    });

    const { scanFolder, getScanStatus } = await import('./scanner');

    await scanFolder(root);

    expect(upsertFilesMock).toHaveBeenCalledTimes(1);
    expect(upsertFilesMock).toHaveBeenCalledWith([
      {
        filePath: path.join(root, 'pack.zip'),
        fileName: 'pack.zip',
        fileType: 'Archive',
        fileSize: 123,
        folderPath: root,
      },
      {
        filePath: path.join(root, 'A', 'B', 'code.tar.gz'),
        fileName: 'code.tar.gz',
        fileType: 'Archive',
        fileSize: 456,
        folderPath: root,
      },
      {
        filePath: path.join(root, 'A', 'B'),
        fileName: 'B',
        fileType: 'Folder',
        fileSize: 0,
        folderPath: root,
      },
    ]);

    expect(markStaleFilesDeletedMock).toHaveBeenCalledTimes(1);
    expect(markStaleFilesDeletedMock).toHaveBeenCalledWith(root, expect.any(String));

    expect(getScanStatus()).toEqual({ scanning: false, scannedCount: 3 });
  });

  it('skips inaccessible directories and stat errors', async () => {
    const root = '/root';

    readdirMock.mockImplementation(async (target: string) => {
      const normalized = normalizePath(target);
      if (normalized === '/root') {
        return [dir('badDir'), dir('leaf'), file('broken.zip')];
      }
      if (normalized === '/root/leaf') {
        return [file('plain.txt')];
      }
      if (normalized === '/root/badDir') {
        throw new Error('permission denied');
      }
      return [];
    });

    statMock.mockImplementation(async (target: string) => {
      if (normalizePath(target) === '/root/broken.zip') {
        throw new Error('stat failed');
      }
      return { size: 0 };
    });

    const { scanFolder, getScanStatus } = await import('./scanner');

    await scanFolder(root);

    expect(upsertFilesMock).toHaveBeenCalledTimes(1);
    expect(upsertFilesMock).toHaveBeenCalledWith([
      {
        filePath: path.join(root, 'leaf'),
        fileName: 'leaf',
        fileType: 'Folder',
        fileSize: 0,
        folderPath: root,
      },
    ]);

    expect(markStaleFilesDeletedMock).toHaveBeenCalledWith(root, expect.any(String));
    expect(getScanStatus()).toEqual({ scanning: false, scannedCount: 1 });
  });
});
