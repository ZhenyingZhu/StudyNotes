import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { expect, test, vi } from 'vitest'
import {
  Analysis,
  ApiError,
  Container,
  InventoryApi,
  InventoryItem,
  Summary,
} from './api'
import { InventoryApp } from './InventoryApp'

const summary: Summary = {
  containers: 1,
  photos: 0,
  items: 1,
  analysesInProgress: 0,
  unassignedItems: 1,
}

const container: Container = {
  id: 'container-1',
  name: 'Office',
  description: null,
  location: 'Upstairs',
  labels: ['work'],
  itemCount: 0,
  createdAt: '2026-01-01T00:00:00Z',
  updatedAt: '2026-01-01T00:00:00Z',
}

const item: InventoryItem = {
  id: 'item-1',
  name: 'Tape',
  description: null,
  category: 'Supplies',
  quantity: 1,
  confidence: null,
  photoId: null,
  analysisId: null,
  containerId: null,
  suggestedContainerId: null,
  assignmentStatus: 'unassigned',
  createdAt: '2026-01-01T00:00:00Z',
  updatedAt: '2026-01-01T00:00:00Z',
}

const completedAnalysis: Analysis = {
  id: 'analysis-1',
  photoId: 'photo-1',
  status: 'completed',
  itemIds: ['item-1'],
  warnings: [],
  errorCode: null,
  errorMessage: null,
  correlationId: null,
  startedAt: '2026-01-01T00:00:01Z',
  completedAt: '2026-01-01T00:00:02Z',
  cancellationRequestedAt: null,
  cancelledAt: null,
  createdAt: '2026-01-01T00:00:00Z',
  updatedAt: '2026-01-01T00:00:02Z',
}

function createApi(): InventoryApi {
  return {
    getSummary: vi.fn().mockResolvedValue(summary),
    listContainers: vi.fn().mockResolvedValue([container]),
    listItems: vi
      .fn()
      .mockResolvedValue({ items: [item], continuationToken: null }),
    listContainerItems: vi
      .fn()
      .mockResolvedValue({ items: [item], continuationToken: null }),
    createContainer: vi.fn().mockResolvedValue(container),
    updateContainer: vi.fn().mockResolvedValue(container),
    deleteContainer: vi.fn().mockResolvedValue(undefined),
    createItem: vi.fn().mockResolvedValue(item),
    updateItem: vi.fn().mockResolvedValue(item),
    deleteItem: vi.fn().mockResolvedValue(undefined),
    assignItem: vi
      .fn()
      .mockResolvedValue({ ...item, assignmentStatus: 'confirmed' }),
    acceptSuggestion: vi
      .fn()
      .mockResolvedValue({ ...item, assignmentStatus: 'confirmed' }),
    unassignItem: vi.fn().mockResolvedValue(item),
    uploadPhoto: vi.fn().mockResolvedValue({
      id: 'photo-1',
      contentType: 'image/png',
      contentLength: 1024,
      width: 512,
      height: 512,
      retentionState: 'active',
      analysisStatus: null,
      retainUntil: '2026-02-01T00:00:00Z',
      createdAt: '2026-01-01T00:00:00Z',
      updatedAt: '2026-01-01T00:00:00Z',
    }),
    startAnalysis: vi.fn().mockResolvedValue(completedAnalysis),
    uploadAndAnalyze: vi.fn().mockResolvedValue(completedAnalysis),
    getAnalysis: vi.fn().mockResolvedValue(completedAnalysis),
    cancelAnalysis: vi.fn().mockResolvedValue({
      ...completedAnalysis,
      status: 'cancelled',
      itemIds: [],
    }),
    getItems: vi.fn().mockResolvedValue([item]),
  }
}

test('creates a container from the webpage', async () => {
  const user = userEvent.setup()
  const api = createApi()
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  const nameInputs = screen.getAllByLabelText('Name')
  await user.type(nameInputs[0], 'Garage shelf')
  await user.click(screen.getByRole('button', { name: 'Add container' }))

  await waitFor(() =>
    expect(api.createContainer).toHaveBeenCalledWith(
      expect.objectContaining({ name: 'Garage shelf' }),
    ),
  )
  expect(await screen.findByRole('status')).toHaveTextContent(
    'Container created.',
  )
})

test('searches inventory and assigns an item', async () => {
  const user = userEvent.setup()
  const api = createApi()
  render(<InventoryApp api={api} />)

  await screen.findByText('Tape')
  await user.type(
    screen.getByRole('textbox', { name: 'Search inventory' }),
    'tape',
  )
  await user.click(screen.getByRole('button', { name: 'Search' }))
  await waitFor(() => expect(api.listItems).toHaveBeenCalledWith('tape'))

  await user.selectOptions(
    screen.getByRole('combobox', { name: 'Container for Tape' }),
    'container-1',
  )
  await waitFor(() =>
    expect(api.assignItem).toHaveBeenCalledWith('item-1', 'container-1'),
  )
})

test('opens a container-specific inventory view', async () => {
  const user = userEvent.setup()
  const api = createApi()
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  await user.click(screen.getByRole('button', { name: 'View items' }))

  await waitFor(() =>
    expect(api.listContainerItems).toHaveBeenCalledWith(
      'container-1',
      '',
    ),
  )
  expect(
    screen.getByRole('button', { name: 'View all inventory' }),
  ).toBeInTheDocument()
})

test('edits an inventory item', async () => {
  const user = userEvent.setup()
  const api = createApi()
  render(<InventoryApp api={api} />)

  const itemRow = (await screen.findByText('Tape')).closest('article')
  expect(itemRow).not.toBeNull()
  await user.click(within(itemRow!).getByRole('button', { name: 'Edit' }))
  const nameInputs = screen.getAllByLabelText('Name')
  await user.clear(nameInputs[1])
  await user.type(nameInputs[1], 'Packing tape')
  await user.click(screen.getByRole('button', { name: 'Save item' }))

  await waitFor(() =>
    expect(api.updateItem).toHaveBeenCalledWith(
      'item-1',
      expect.objectContaining({ name: 'Packing tape' }),
    ),
  )
  expect(await screen.findByRole('status')).toHaveTextContent('Item updated.')
})

test('shows a conflict message from a failed mutation', async () => {
  const user = userEvent.setup()
  const api = createApi()
  vi.mocked(api.deleteItem).mockRejectedValue(
    new ApiError('The item is still referenced.', 409),
  )
  render(<InventoryApp api={api} />)

  const itemRow = (await screen.findByText('Tape')).closest('article')
  expect(itemRow).not.toBeNull()
  await user.click(within(itemRow!).getByRole('button', { name: 'Delete' }))

  expect(await screen.findByRole('alert')).toHaveTextContent(
    'Conflict: The item is still referenced.',
  )
})

test('shows a stale-record message', async () => {
  const user = userEvent.setup()
  const api = createApi()
  vi.mocked(api.updateItem).mockRejectedValue(
    new ApiError('The resource changed.', 412),
  )
  render(<InventoryApp api={api} />)

  const itemRow = (await screen.findByText('Tape')).closest('article')
  expect(itemRow).not.toBeNull()
  await user.click(within(itemRow!).getByRole('button', { name: 'Edit' }))
  await user.click(screen.getByRole('button', { name: 'Save item' }))

  expect(await screen.findByRole('alert')).toHaveTextContent(
    'This record changed elsewhere. Refresh and try again.',
  )
})

test('uploads a photo and starts analysis for later review', async () => {
  const user = userEvent.setup()
  const api = createApi()
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  const file = new File(['image'], 'inventory.png', {
    type: 'image/png',
  })
  await user.upload(screen.getByLabelText('Photo'), file)
  await user.click(
    screen.getByRole('button', { name: 'Upload and analyze' }),
  )

  await waitFor(() => expect(api.uploadPhoto).toHaveBeenCalledWith(
    file,
    expect.any(String),
  ))
  expect(api.startAnalysis).toHaveBeenCalledWith(
    'photo-1',
    expect.any(String),
  )
  expect(await screen.findByText('Analysis completed')).toBeInTheDocument()
  expect(api.getItems).toHaveBeenCalledWith(['item-1'])
})

test('uses convenience workflow for an explicitly selected container', async () => {
  const user = userEvent.setup()
  const api = createApi()
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  const file = new File(['image'], 'inventory.png', {
    type: 'image/png',
  })
  await user.upload(screen.getByLabelText('Photo'), file)
  await user.selectOptions(
    screen.getByLabelText(
      'Assign all detected items to a container (optional)',
    ),
    'container-1',
  )
  await user.click(
    screen.getByRole('button', { name: 'Upload and analyze' }),
  )

  await waitFor(() =>
    expect(api.uploadAndAnalyze).toHaveBeenCalledWith(
      'container-1',
      file,
      expect.any(String),
    ),
  )
  expect(api.uploadPhoto).not.toHaveBeenCalled()
})

test('cancels a queued analysis from the webpage', async () => {
  const user = userEvent.setup()
  const api = createApi()
  vi.mocked(api.startAnalysis).mockResolvedValue({
    ...completedAnalysis,
    status: 'queued',
    itemIds: [],
    completedAt: null,
  })
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  await user.upload(
    screen.getByLabelText('Photo'),
    new File(['image'], 'inventory.png', { type: 'image/png' }),
  )
  await user.click(
    screen.getByRole('button', { name: 'Upload and analyze' }),
  )
  await user.click(
    await screen.findByRole('button', { name: 'Cancel analysis' }),
  )

  await waitFor(() =>
    expect(api.cancelAnalysis).toHaveBeenCalledWith('analysis-1'),
  )
  expect(await screen.findByText('Analysis cancelled')).toBeInTheDocument()
})

test('accepts an analysis suggestion explicitly', async () => {
  const user = userEvent.setup()
  const suggestedItem: InventoryItem = {
    ...item,
    suggestedContainerId: 'container-1',
    assignmentStatus: 'suggested',
  }
  const api = createApi()
  vi.mocked(api.getItems).mockResolvedValue([suggestedItem])
  vi.mocked(api.acceptSuggestion).mockResolvedValue({
    ...suggestedItem,
    suggestedContainerId: null,
    containerId: 'container-1',
    assignmentStatus: 'confirmed',
  })
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  await user.upload(
    screen.getByLabelText('Photo'),
    new File(['image'], 'inventory.png', { type: 'image/png' }),
  )
  await user.click(
    screen.getByRole('button', { name: 'Upload and analyze' }),
  )
  await user.click(
    await screen.findByRole('button', { name: 'Accept suggestion' }),
  )

  await waitFor(() =>
    expect(api.acceptSuggestion).toHaveBeenCalledWith(
      'item-1',
      'container-1',
    ),
  )
})

test('rejects an analysis suggestion explicitly', async () => {
  const user = userEvent.setup()
  const suggestedItem: InventoryItem = {
    ...item,
    suggestedContainerId: 'container-1',
    assignmentStatus: 'suggested',
  }
  const api = createApi()
  vi.mocked(api.getItems).mockResolvedValue([suggestedItem])
  render(<InventoryApp api={api} />)

  await screen.findByRole('heading', { name: 'Office' })
  await user.upload(
    screen.getByLabelText('Photo'),
    new File(['image'], 'inventory.png', { type: 'image/png' }),
  )
  await user.click(
    screen.getByRole('button', { name: 'Upload and analyze' }),
  )
  await user.click(
    await screen.findByRole('button', { name: 'Reject suggestion' }),
  )

  await waitFor(() =>
    expect(api.unassignItem).toHaveBeenCalledWith('item-1'),
  )
})
