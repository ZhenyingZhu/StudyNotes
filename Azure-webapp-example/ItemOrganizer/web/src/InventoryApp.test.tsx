import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { expect, test, vi } from 'vitest'
import {
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
    unassignItem: vi.fn().mockResolvedValue(item),
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
