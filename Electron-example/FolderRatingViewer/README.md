# Folder Rating Viewer

A cross-platform desktop app (Windows & Ubuntu) that indexes all files in a selected folder and lets you rate them.

## Features

- **Folder scanning** — select any folder and scan it to index all files recursively
- **File metadata** — shows file name, type, size, and full path
- **Star ratings** — rate any file from 1–5 stars; ratings persist across sessions
- **Search** — filter files by name, path, or type
- **Sorting** — click column headers to sort by name, type, size, path, or rating
- **Pagination** — handles large folders (10k–100k files) with paginated results
- **SQLite database** — all data stored locally in your user data directory

## Getting Started

### Prerequisites

- Node.js 18+ and npm

### Install

```bash
npm install
```

### Run in Development

```bash
npm start
```

### Package for Distribution

```bash
# Linux (AppImage + deb)
npm run dist:linux

# Windows (NSIS installer + portable)
npm run dist:win

# Both
npm run dist
```

Packaged output goes into the `release/` directory.

## Project Structure

```
src/
├── main/
│   ├── index.ts          # Electron main process entry
│   ├── preload.ts        # Context bridge (secure IPC)
│   ├── db/
│   │   └── database.ts   # SQLite schema, queries, ratings
│   ├── ipc/
│   │   └── handlers.ts   # IPC handler registration
│   └── services/
│       └── scanner.ts    # Recursive file scanner
└── renderer/
    ├── index.html        # App shell
    ├── styles.css        # Dark-themed UI styles
    └── renderer.js       # Frontend logic
```

## Tech Stack

- **Electron** — cross-platform desktop shell
- **better-sqlite3** — fast synchronous SQLite bindings
- **TypeScript** — main process
- **Vanilla JS** — renderer (no framework overhead)
- **electron-builder** — packaging for Windows & Linux
