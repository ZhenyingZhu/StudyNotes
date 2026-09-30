export type Summary = {
  containers: number
  photos: number
  items: number
  analysesInProgress: number
  unassignedItems: number
}

export type Container = {
  id: string
  name: string
  description: string | null
  location: string | null
  labels: string[]
  itemCount: number
  createdAt: string
  updatedAt: string
}

export type InventoryItem = {
  id: string
  name: string
  description: string | null
  category: string | null
  quantity: number
  confidence: number | null
  photoId: string | null
  analysisId: string | null
  containerId: string | null
  suggestedContainerId: string | null
  assignmentStatus: 'unassigned' | 'suggested' | 'confirmed'
  createdAt: string
  updatedAt: string
}

export type Photo = {
  id: string
  contentType: string
  contentLength: number
  width: number
  height: number
  retentionState: 'active' | 'pendingDeletion' | 'deleted'
  analysisStatus: AnalysisStatus | null
  retainUntil: string
  createdAt: string
  updatedAt: string
}

export type AnalysisStatus =
  | 'queued'
  | 'running'
  | 'completed'
  | 'failed'
  | 'cancelled'

export type Analysis = {
  id: string
  photoId: string
  status: AnalysisStatus
  itemIds: string[]
  warnings: string[]
  errorCode: string | null
  errorMessage: string | null
  correlationId: string | null
  startedAt: string | null
  completedAt: string | null
  cancellationRequestedAt: string | null
  cancelledAt: string | null
  createdAt: string
  updatedAt: string
}

export type ContainerInput = {
  name: string
  description: string
  location: string
  labels: string[]
}

export type ItemInput = {
  name: string
  description: string
  category: string
  quantity: number
}

export type Page<T> = {
  items: T[]
  continuationToken: string | null
}

type Resource<T> = {
  value: T
  etag: string
}

export interface InventoryApi {
  getSummary(): Promise<Summary>
  listContainers(): Promise<Container[]>
  listItems(
    search?: string,
    continuationToken?: string | null,
  ): Promise<Page<InventoryItem>>
  listContainerItems(
    containerId: string,
    search?: string,
    continuationToken?: string | null,
  ): Promise<Page<InventoryItem>>
  createContainer(input: ContainerInput): Promise<Container>
  updateContainer(id: string, input: ContainerInput): Promise<Container>
  deleteContainer(id: string): Promise<void>
  createItem(input: ItemInput): Promise<InventoryItem>
  updateItem(id: string, input: ItemInput): Promise<InventoryItem>
  deleteItem(id: string): Promise<void>
  assignItem(itemId: string, containerId: string): Promise<InventoryItem>
  acceptSuggestion(
    itemId: string,
    containerId: string,
  ): Promise<InventoryItem>
  unassignItem(itemId: string): Promise<InventoryItem>
  uploadPhoto(file: File, idempotencyKey: string): Promise<Photo>
  startAnalysis(
    photoId: string,
    idempotencyKey: string,
  ): Promise<Analysis>
  uploadAndAnalyze(
    containerId: string,
    file: File,
    idempotencyKey: string,
  ): Promise<Analysis>
  getAnalysis(analysisId: string): Promise<Analysis>
  cancelAnalysis(analysisId: string): Promise<Analysis>
  getItems(itemIds: string[]): Promise<InventoryItem[]>
}

export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number,
  ) {
    super(message)
  }
}

export class HttpInventoryApi implements InventoryApi {
  constructor(
    private readonly baseUrl: string,
    private readonly getAccessToken: () => Promise<string | null>,
  ) {}

  getSummary() {
    return this.request<Summary>('/api/v1/summary')
  }

  async listContainers() {
    const page = await this.request<Page<Container>>(
      '/api/v1/containers?pageSize=100',
    )
    return page.items
  }

  listItems(search = '', continuationToken?: string | null) {
    const query = new URLSearchParams({ pageSize: '100' })
    if (search.trim()) {
      query.set('search', search.trim())
    }
    if (continuationToken) {
      query.set('continuationToken', continuationToken)
    }
    return this.request<Page<InventoryItem>>(`/api/v1/items?${query}`)
  }

  listContainerItems(
    containerId: string,
    search = '',
    continuationToken?: string | null,
  ) {
    const query = new URLSearchParams({ pageSize: '100' })
    if (search.trim()) {
      query.set('search', search.trim())
    }
    if (continuationToken) {
      query.set('continuationToken', continuationToken)
    }
    return this.request<Page<InventoryItem>>(
      `/api/v1/containers/${containerId}/items?${query}`,
    )
  }

  createContainer(input: ContainerInput) {
    return this.request<Container>('/api/v1/containers', {
      method: 'POST',
      body: JSON.stringify(input),
    })
  }

  async updateContainer(id: string, input: ContainerInput) {
    const current = await this.getResource<Container>(
      `/api/v1/containers/${id}`,
    )
    return this.request<Container>(`/api/v1/containers/${id}`, {
      method: 'PATCH',
      headers: { 'If-Match': current.etag },
      body: JSON.stringify(input),
    })
  }

  async deleteContainer(id: string) {
    const current = await this.getResource<Container>(
      `/api/v1/containers/${id}`,
    )
    await this.request<void>(`/api/v1/containers/${id}`, {
      method: 'DELETE',
      headers: { 'If-Match': current.etag },
    })
  }

  createItem(input: ItemInput) {
    return this.request<InventoryItem>('/api/v1/items', {
      method: 'POST',
      body: JSON.stringify(input),
    })
  }

  async updateItem(id: string, input: ItemInput) {
    const current = await this.getResource<InventoryItem>(
      `/api/v1/items/${id}`,
    )
    return this.request<InventoryItem>(`/api/v1/items/${id}`, {
      method: 'PATCH',
      headers: { 'If-Match': current.etag },
      body: JSON.stringify(input),
    })
  }

  async deleteItem(id: string) {
    const current = await this.getResource<InventoryItem>(
      `/api/v1/items/${id}`,
    )
    await this.request<void>(`/api/v1/items/${id}`, {
      method: 'DELETE',
      headers: { 'If-Match': current.etag },
    })
  }

  async assignItem(itemId: string, containerId: string) {
    return this.setItemContainer(itemId, containerId, false)
  }

  async acceptSuggestion(itemId: string, containerId: string) {
    return this.setItemContainer(itemId, containerId, true)
  }

  async unassignItem(itemId: string) {
    const current = await this.getResource<InventoryItem>(
      `/api/v1/items/${itemId}`,
    )
    return this.request<InventoryItem>(
      `/api/v1/items/${itemId}/container`,
      {
        method: 'DELETE',
        headers: { 'If-Match': current.etag },
      },
    )
  }

  uploadPhoto(file: File, idempotencyKey: string) {
    const body = new FormData()
    body.append('file', file)
    return this.request<Photo>('/api/v1/photos', {
      method: 'POST',
      headers: { 'Idempotency-Key': idempotencyKey },
      body,
    })
  }

  startAnalysis(photoId: string, idempotencyKey: string) {
    return this.request<Analysis>(`/api/v1/photos/${photoId}/analyses`, {
      method: 'POST',
      headers: { 'Idempotency-Key': idempotencyKey },
    })
  }

  uploadAndAnalyze(
    containerId: string,
    file: File,
    idempotencyKey: string,
  ) {
    const body = new FormData()
    body.append('file', file)
    return this.request<Analysis>(
      `/api/v1/containers/${containerId}/photo-analyses`,
      {
        method: 'POST',
        headers: { 'Idempotency-Key': idempotencyKey },
        body,
      },
    )
  }

  getAnalysis(analysisId: string) {
    return this.request<Analysis>(`/api/v1/analyses/${analysisId}`)
  }

  async cancelAnalysis(analysisId: string) {
    const current = await this.getResource<Analysis>(
      `/api/v1/analyses/${analysisId}`,
    )
    return this.request<Analysis>(
      `/api/v1/analyses/${analysisId}/cancel`,
      {
        method: 'POST',
        headers: { 'If-Match': current.etag },
      },
    )
  }

  async getItems(itemIds: string[]) {
    return Promise.all(
      itemIds.map(async (itemId) => {
        const resource = await this.getResource<InventoryItem>(
          `/api/v1/items/${itemId}`,
        )
        return resource.value
      }),
    )
  }

  private async setItemContainer(
    itemId: string,
    containerId: string,
    acceptSuggestion: boolean,
  ) {
    const current = await this.getResource<InventoryItem>(
      `/api/v1/items/${itemId}`,
    )
    return this.request<InventoryItem>(
      `/api/v1/items/${itemId}/container`,
      {
        method: 'PUT',
        headers: { 'If-Match': current.etag },
        body: JSON.stringify({ containerId, acceptSuggestion }),
      },
    )
  }

  private async getResource<T>(path: string): Promise<Resource<T>> {
    const response = await this.send(path)
    return {
      value: (await response.json()) as T,
      etag: response.headers.get('ETag') ?? '',
    }
  }

  private async request<T>(path: string, init?: RequestInit): Promise<T> {
    const response = await this.send(path, init)
    if (response.status === 204) {
      return undefined as T
    }
    return (await response.json()) as T
  }

  private async send(path: string, init?: RequestInit) {
    const token = await this.getAccessToken()
    const headers = new Headers(init?.headers)
    if (token) {
      headers.set('Authorization', `Bearer ${token}`)
    }
    if (init?.body && !(init.body instanceof FormData)) {
      headers.set('Content-Type', 'application/json')
    }

    const response = await fetch(`${this.baseUrl}${path}`, {
      ...init,
      headers,
    })
    if (!response.ok) {
      const problem = (await response.json().catch(() => null)) as {
        detail?: string
        title?: string
      } | null
      throw new ApiError(
        problem?.detail ?? problem?.title ?? 'The request failed.',
        response.status,
      )
    }
    return response
  }
}
