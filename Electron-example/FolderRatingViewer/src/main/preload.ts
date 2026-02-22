import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('api', {
  selectFolder: (): Promise<string | null> => ipcRenderer.invoke('select-folder'),
  scanFolder: (folderPath: string): Promise<void> => ipcRenderer.invoke('scan-folder', folderPath),
  getFiles: (options: {
    folderPath: string;
    sortBy?: string;
    sortOrder?: string;
    search?: string;
    page?: number;
    pageSize?: number;
  }): Promise<{ files: any[]; total: number }> => ipcRenderer.invoke('get-files', options),
  setRating: (filePath: string, rating: number): Promise<void> => ipcRenderer.invoke('set-rating', filePath, rating),
  getScanStatus: (): Promise<{ scanning: boolean; scannedCount: number }> => ipcRenderer.invoke('get-scan-status'),
});
