import { ipcMain, dialog, BrowserWindow } from 'electron';
import { getFiles, setRating } from '../db/database';
import { scanFolder, getScanStatus } from '../services/scanner';

export function registerIpcHandlers(): void {
  ipcMain.handle('select-folder', async () => {
    const win = BrowserWindow.getFocusedWindow();
    if (!win) return null;

    const result = await dialog.showOpenDialog(win, {
      properties: ['openDirectory'],
      title: 'Select Folder to Index',
    });

    if (result.canceled || result.filePaths.length === 0) {
      return null;
    }
    return result.filePaths[0];
  });

  ipcMain.handle('scan-folder', async (_event, folderPath: string) => {
    await scanFolder(folderPath);
  });

  ipcMain.handle('get-files', (_event, options: {
    folderPath: string;
    sortBy?: string;
    sortOrder?: string;
    search?: string;
    page?: number;
    pageSize?: number;
  }) => {
    return getFiles(options);
  });

  ipcMain.handle('set-rating', (_event, filePath: string, rating: number) => {
    setRating(filePath, rating);
  });

  ipcMain.handle('get-scan-status', () => {
    return getScanStatus();
  });
}
