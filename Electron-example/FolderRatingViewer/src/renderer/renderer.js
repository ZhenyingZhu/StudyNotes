// @ts-check

/**
 * @typedef {{ file_path: string; file_name: string; file_type: string; file_size: number; rating: number }} FileRecord
 */

// ── State ──────────────────────────────────────────────────
let currentFolder = '';
let currentPage = 1;
let totalFiles = 0;
const PAGE_SIZE = 100;
let sortBy = 'file_name';
let sortOrder = 'ASC';
let searchQuery = '';
let searchTimeout = null;

// ── DOM Refs ───────────────────────────────────────────────
const btnSelectFolder = document.getElementById('btn-select-folder');
const btnRescan = document.getElementById('btn-rescan');
const folderPathEl = document.getElementById('folder-path');
const statusBar = document.getElementById('status-bar');
const statusText = document.getElementById('status-text');
const statusCount = document.getElementById('status-count');
const searchInput = document.getElementById('search-input');
const resultCount = document.getElementById('result-count');
const tbody = document.getElementById('file-tbody');
const btnPrev = document.getElementById('btn-prev');
const btnNext = document.getElementById('btn-next');
const pageInfo = document.getElementById('page-info');

// ── Helpers ────────────────────────────────────────────────
function formatSize(bytes) {
  if (bytes === 0) return '0 B';
  const units = ['B', 'KB', 'MB', 'GB', 'TB'];
  const i = Math.floor(Math.log(bytes) / Math.log(1024));
  return (bytes / Math.pow(1024, i)).toFixed(i === 0 ? 0 : 1) + ' ' + units[i];
}

function escapeHtml(str) {
  const div = document.createElement('div');
  div.textContent = str;
  return div.innerHTML;
}

function renderStars(filePath, rating) {
  let html = '<span class="star-rating">';
  for (let i = 1; i <= 5; i++) {
    const cls = i <= rating ? 'star active' : 'star';
    html += `<span class="${cls}" data-path="${escapeHtml(filePath)}" data-rating="${i}">★</span>`;
  }
  html += '</span>';
  return html;
}

// ── Load Files ─────────────────────────────────────────────
async function loadFiles() {
  if (!currentFolder) return;

  const result = await window.api.getFiles({
    folderPath: currentFolder,
    sortBy,
    sortOrder,
    search: searchQuery,
    page: currentPage,
    pageSize: PAGE_SIZE,
  });

  totalFiles = result.total;
  renderTable(result.files);
  updatePagination();
}

function renderTable(files) {
  if (files.length === 0) {
    tbody.innerHTML = `<tr class="empty-row"><td colspan="5">${
      searchQuery ? 'No files match your search' : 'No files found in this folder'
    }</td></tr>`;
    resultCount.textContent = '';
    return;
  }

  resultCount.textContent = `${totalFiles.toLocaleString()} file${totalFiles !== 1 ? 's' : ''}`;

  let html = '';
  for (const f of files) {
    html += `<tr>
      <td title="${escapeHtml(f.file_name)}">${escapeHtml(f.file_name)}</td>
      <td><span class="type-badge">${escapeHtml(f.file_type)}</span></td>
      <td>${formatSize(f.file_size)}</td>
      <td title="${escapeHtml(f.file_path)}">${escapeHtml(f.file_path)}</td>
      <td>${renderStars(f.file_path, f.rating)}</td>
    </tr>`;
  }
  tbody.innerHTML = html;
}

function updatePagination() {
  const totalPages = Math.max(1, Math.ceil(totalFiles / PAGE_SIZE));
  pageInfo.textContent = `Page ${currentPage} of ${totalPages}`;
  btnPrev.disabled = currentPage <= 1;
  btnNext.disabled = currentPage >= totalPages;
}

// ── Scan ───────────────────────────────────────────────────
async function doScan() {
  if (!currentFolder) return;

  btnRescan.disabled = true;
  btnSelectFolder.disabled = true;
  statusBar.classList.remove('hidden');
  statusText.textContent = 'Scanning...';
  statusCount.textContent = '';

  // Poll scan status
  const pollId = setInterval(async () => {
    const status = await window.api.getScanStatus();
    statusCount.textContent = `${status.scannedCount.toLocaleString()} files found`;
  }, 300);

  try {
    await window.api.scanFolder(currentFolder);
  } finally {
    clearInterval(pollId);
    statusBar.classList.add('hidden');
    btnRescan.disabled = false;
    btnSelectFolder.disabled = false;
  }

  currentPage = 1;
  await loadFiles();
}

// ── Event Listeners ────────────────────────────────────────
btnSelectFolder.addEventListener('click', async () => {
  const folder = await window.api.selectFolder();
  if (folder) {
    currentFolder = folder;
    folderPathEl.textContent = folder;
    folderPathEl.title = folder;
    btnRescan.disabled = false;
    currentPage = 1;
    searchQuery = '';
    searchInput.value = '';
    await doScan();
  }
});

btnRescan.addEventListener('click', () => {
  doScan();
});

// Debounced search
searchInput.addEventListener('input', () => {
  clearTimeout(searchTimeout);
  searchTimeout = setTimeout(() => {
    searchQuery = searchInput.value.trim();
    currentPage = 1;
    loadFiles();
  }, 300);
});

// Sorting
document.querySelectorAll('th.sortable').forEach((th) => {
  th.addEventListener('click', () => {
    const col = th.getAttribute('data-sort');
    if (sortBy === col) {
      sortOrder = sortOrder === 'ASC' ? 'DESC' : 'ASC';
    } else {
      sortBy = col;
      sortOrder = 'ASC';
    }
    // Update header arrows
    document.querySelectorAll('th.sortable').forEach((t) => {
      t.classList.remove('sorted-asc', 'sorted-desc');
    });
    th.classList.add(sortOrder === 'ASC' ? 'sorted-asc' : 'sorted-desc');
    currentPage = 1;
    loadFiles();
  });
});

// Star rating click
tbody.addEventListener('click', async (e) => {
  const star = e.target.closest('.star');
  if (!star) return;

  const filePath = star.getAttribute('data-path');
  const rating = parseInt(star.getAttribute('data-rating'), 10);

  await window.api.setRating(filePath, rating);

  // Update stars in the same row without full reload
  const row = star.closest('.star-rating');
  row.querySelectorAll('.star').forEach((s) => {
    const r = parseInt(s.getAttribute('data-rating'), 10);
    s.classList.toggle('active', r <= rating);
  });
});

// Pagination
btnPrev.addEventListener('click', () => {
  if (currentPage > 1) {
    currentPage--;
    loadFiles();
  }
});

btnNext.addEventListener('click', () => {
  const totalPages = Math.ceil(totalFiles / PAGE_SIZE);
  if (currentPage < totalPages) {
    currentPage++;
    loadFiles();
  }
});
