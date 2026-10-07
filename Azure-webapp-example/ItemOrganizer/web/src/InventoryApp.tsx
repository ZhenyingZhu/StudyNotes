import { FormEvent, useCallback, useEffect, useState } from 'react'
import {
  Analysis,
  AnalysisDetection,
  ApiError,
  Container,
  ContainerInput,
  InventoryApi,
  InventoryItem,
  ItemInput,
  DetectionReviewInput,
  Summary,
} from './api'

const emptyContainer: ContainerInput = {
  name: '',
  description: '',
  location: '',
  labels: [],
}

const emptyItem: ItemInput = {
  name: '',
  description: '',
  category: '',
  quantity: 1,
}

type DetectionReview = DetectionReviewInput & {
  confidence: number
  suggestedContainerId: string | null
  reviewStatus: AnalysisDetection['reviewStatus']
}

export function InventoryApp({ api }: { api: InventoryApi }) {
  const [summary, setSummary] = useState<Summary | null>(null)
  const [containers, setContainers] = useState<Container[]>([])
  const [items, setItems] = useState<InventoryItem[]>([])
  const [continuationToken, setContinuationToken] = useState<string | null>(
    null,
  )
  const [selectedContainerId, setSelectedContainerId] = useState<string | null>(
    null,
  )
  const [search, setSearch] = useState('')
  const [containerForm, setContainerForm] = useState(emptyContainer)
  const [itemForm, setItemForm] = useState(emptyItem)
  const [editingContainerId, setEditingContainerId] = useState<string | null>(
    null,
  )
  const [editingItemId, setEditingItemId] = useState<string | null>(null)
  const [error, setError] = useState('')
  const [notice, setNotice] = useState('')
  const [loading, setLoading] = useState(true)
  const [selectedPhoto, setSelectedPhoto] = useState<File | null>(null)
  const [photoPreviewUrl, setPhotoPreviewUrl] = useState('')
  const [targetContainerId, setTargetContainerId] = useState('')
  const [photoRequestKey, setPhotoRequestKey] = useState('')
  const [analysis, setAnalysis] = useState<Analysis | null>(null)
  const [analysisDetections, setAnalysisDetections] = useState<
    DetectionReview[]
  >([])
  const [analysisBusy, setAnalysisBusy] = useState(false)

  const load = useCallback(
    async (
      itemSearch: string,
      containerId: string | null = selectedContainerId,
    ) => {
      try {
        setLoading(true)
        const [nextSummary, nextContainers, nextPage] = await Promise.all([
          api.getSummary(),
          api.listContainers(),
          containerId
            ? api.listContainerItems(containerId, itemSearch)
            : api.listItems(itemSearch),
        ])
        setSummary(nextSummary)
        setContainers(nextContainers)
        setItems(nextPage.items)
        setContinuationToken(nextPage.continuationToken)
        setError('')
      } catch (caught) {
        showError(caught, setError)
      } finally {
        setLoading(false)
      }
    },
    [api, selectedContainerId],
  )

  useEffect(() => {
    void load('')
  }, [load])

  useEffect(() => {
    if (!selectedPhoto) {
      setPhotoPreviewUrl('')
      return
    }
    if (typeof URL.createObjectURL !== 'function') {
      return
    }

    const url = URL.createObjectURL(selectedPhoto)
    setPhotoPreviewUrl(url)
    return () => URL.revokeObjectURL(url)
  }, [selectedPhoto])

  useEffect(() => {
    if (!analysis || !['queued', 'running'].includes(analysis.status)) {
      return
    }

    const timer = window.setTimeout(async () => {
      try {
        const next = await api.getAnalysis(analysis.id)
        setAnalysis(next)
        if (next.status === 'completed') {
          setAnalysisDetections(toDetectionReviews(next))
        }
      } catch (caught) {
        showError(caught, setError)
      }
    }, 750)
    return () => window.clearTimeout(timer)
  }, [analysis, api, load, search])

  async function submitContainer(event: FormEvent) {
    event.preventDefault()
    await runMutation(async () => {
      if (editingContainerId) {
        await api.updateContainer(editingContainerId, containerForm)
        setNotice('Container updated.')
      } else {
        await api.createContainer(containerForm)
        setNotice('Container created.')
      }
      setContainerForm(emptyContainer)
      setEditingContainerId(null)
    })
  }

  async function submitItem(event: FormEvent) {
    event.preventDefault()
    await runMutation(async () => {
      if (editingItemId) {
        await api.updateItem(editingItemId, itemForm)
        setNotice('Item updated.')
      } else {
        await api.createItem(itemForm)
        setNotice('Item created.')
      }
      setItemForm(emptyItem)
      setEditingItemId(null)
    })
  }

  async function runMutation(action: () => Promise<void>) {
    try {
      setError('')
      await action()
      await load(search)
    } catch (caught) {
      showError(caught, setError)
    }
  }

  async function submitPhoto(event: FormEvent) {
      event.preventDefault()
      if (!selectedPhoto) {
        setError('Choose a photo before starting analysis.')
        return
      }

      try {
        setAnalysisBusy(true)
        setError('')
        setNotice('')
        setAnalysisDetections([])
        const key = photoRequestKey || createIdempotencyKey()
        setPhotoRequestKey(key)
        const started = targetContainerId
          ? await api.uploadAndAnalyze(
              targetContainerId,
              selectedPhoto,
              key,
            )
          : await api.startAnalysis(
              (await api.uploadPhoto(selectedPhoto, key)).id,
              key,
            )
        setAnalysis(started)
        if (started.status === 'completed') {
          setAnalysisDetections(toDetectionReviews(started))
        }
        setNotice('Photo uploaded and analysis queued.')
      } catch (caught) {
        showError(caught, setError)
      } finally {
        setAnalysisBusy(false)
      }
  }

  async function cancelAnalysis() {
      if (!analysis) {
        return
      }

      try {
        setAnalysisBusy(true)
        setError('')
        setAnalysis(await api.cancelAnalysis(analysis.id))
        setNotice('Analysis cancelled.')
      } catch (caught) {
        showError(caught, setError)
      } finally {
        setAnalysisBusy(false)
      }
  }

  async function confirmDetections() {
    if (!analysis) {
      return
    }

    try {
      setAnalysisBusy(true)
      setError('')
      const confirmed = await api.confirmAnalysis(
        analysis.id,
        analysisDetections.map((detection) => ({
          id: detection.id,
          accepted: detection.accepted,
          name: detection.name,
          description: detection.description,
          category: detection.category,
          quantity: detection.quantity,
          containerId: detection.containerId,
        })),
      )
      setAnalysis(confirmed)
      setAnalysisDetections(toDetectionReviews(confirmed))
      setNotice('Confirmed detections were added to inventory.')
      await load(search)
    } catch (caught) {
      showError(caught, setError)
    } finally {
      setAnalysisBusy(false)
    }
  }

  function updateDetection(
    detectionId: string,
    update: Partial<DetectionReview>,
  ) {
    setAnalysisDetections((current) =>
      current.map((detection) =>
        detection.id === detectionId
          ? { ...detection, ...update }
          : detection,
      ),
    )
  }

  async function loadMore() {
    if (!continuationToken) {
      return
    }

    try {
      setLoading(true)
      const page = selectedContainerId
        ? await api.listContainerItems(
            selectedContainerId,
            search,
            continuationToken,
          )
        : await api.listItems(search, continuationToken)
      setItems((current) => [...current, ...page.items])
      setContinuationToken(page.continuationToken)
      setError('')
    } catch (caught) {
      showError(caught, setError)
    } finally {
      setLoading(false)
    }
  }

  function editContainer(container: Container) {
    setEditingContainerId(container.id)
    setContainerForm({
      name: container.name,
      description: container.description ?? '',
      location: container.location ?? '',
      labels: container.labels,
    })
  }

  function editItem(item: InventoryItem) {
    setEditingItemId(item.id)
    setItemForm({
      name: item.name,
      description: item.description ?? '',
      category: item.category ?? '',
      quantity: item.quantity,
    })
  }

  return (
    <main>
      <header className="hero">
        <div>
          <p className="eyebrow">ITEM ORGANIZER</p>
          <h1>Know where everything lives.</h1>
          <p className="hero-copy">
            Build a searchable inventory, group items into containers, and keep
            every move explicit.
          </p>
        </div>
        <button className="secondary" onClick={() => void load(search)}>
          Refresh
        </button>
      </header>

      {error && (
        <div className="message error" role="alert">
          {error}
        </div>
      )}
      {notice && (
        <div className="message success" role="status">
          {notice}
        </div>
      )}

      <section className="summary-grid" aria-label="Inventory summary">
        <SummaryCard label="Containers" value={summary?.containers} />
        <SummaryCard label="Items" value={summary?.items} />
        <SummaryCard label="Unassigned" value={summary?.unassignedItems} />
        <SummaryCard
          label="Analyses running"
          value={summary?.analysesInProgress}
        />
      </section>

      <section className="panel analysis-workbench">
        <div className="section-heading">
          <div>
            <p className="eyebrow">PHOTO ANALYSIS</p>
            <h2>Turn a photo into inventory</h2>
          </div>
          {analysis && (
            <span className={`status ${analysis.status}`}>
              {analysis.status}
            </span>
          )}
        </div>

        <div className="analysis-layout">
          <form className="photo-editor" onSubmit={submitPhoto}>
            <label>
              Photo
              <input
                type="file"
                accept="image/jpeg,image/png,image/webp"
                onChange={(event) => {
                  setSelectedPhoto(event.target.files?.[0] ?? null)
                  setPhotoRequestKey(createIdempotencyKey())
                  setAnalysis(null)
                  setAnalysisDetections([])
                }}
              />
            </label>
            <label>
              Preselect a container for review (optional)
              <select
                value={targetContainerId}
                onChange={(event) =>
                  setTargetContainerId(event.target.value)
                }
              >
                <option value="">Use AI suggestions during review</option>
                {containers.map((container) => (
                  <option value={container.id} key={container.id}>
                    {container.name}
                  </option>
                ))}
              </select>
            </label>
            <p className="field-help">
              JPEG, PNG, or WebP; 512-8000 pixels; up to 10 MiB.
            </p>
            <div className="form-actions">
              <button type="submit" disabled={analysisBusy}>
                {analysisBusy ? 'Starting…' : 'Upload and analyze'}
              </button>
              {analysis &&
                ['queued', 'running'].includes(analysis.status) && (
                  <button
                    type="button"
                    className="secondary"
                    disabled={analysisBusy}
                    onClick={() => void cancelAnalysis()}
                  >
                    Cancel analysis
                  </button>
                )}
            </div>
          </form>

          <div className="photo-preview">
            {photoPreviewUrl ? (
              <img src={photoPreviewUrl} alt="Selected source" />
            ) : (
              <p className="empty-state">Choose a photo to preview it.</p>
            )}
          </div>
        </div>

        {analysis && (
          <div className="analysis-results" aria-live="polite">
            <h3>Analysis {analysis.status}</h3>
            {['queued', 'running'].includes(analysis.status) && (
              <p>Detection is running. This page refreshes automatically.</p>
            )}
            {analysis.status === 'failed' && (
              <p className="message error">
                {analysis.errorMessage || 'Analysis failed.'}
              </p>
            )}
            {analysis.status === 'cancelled' && (
              <p>The analysis was cancelled without creating items.</p>
            )}
            {analysis.warnings.map((warning) => (
              <p className="analysis-warning" key={warning}>
                {warning}
              </p>
            ))}
            {analysis.status === 'completed' &&
              analysisDetections.map((detection) => {
                const suggested = containers.find(
                  (container) =>
                    container.id === detection.suggestedContainerId,
                )
                return (
                  <article className="detection-card" key={detection.id}>
                    <div className="review-actions">
                      <label>
                        <input
                          type="checkbox"
                          checked={detection.accepted}
                          disabled={detection.reviewStatus !== 'pending'}
                          onChange={(event) =>
                            updateDetection(detection.id, {
                              accepted: event.target.checked,
                            })
                          }
                        />
                        Add to inventory
                      </label>
                      <label>
                        Name
                        <input
                          value={detection.name}
                          disabled={!detection.accepted}
                          onChange={(event) =>
                            updateDetection(detection.id, {
                              name: event.target.value,
                            })
                          }
                        />
                      </label>
                      <label>
                        Category
                        <input
                          value={detection.category}
                          disabled={!detection.accepted}
                          onChange={(event) =>
                            updateDetection(detection.id, {
                              category: event.target.value,
                            })
                          }
                        />
                      </label>
                      <label>
                        Quantity
                        <input
                          type="number"
                          min="1"
                          value={detection.quantity}
                          disabled={!detection.accepted}
                          onChange={(event) =>
                            updateDetection(detection.id, {
                              quantity: Number(event.target.value),
                            })
                          }
                        />
                      </label>
                      <label>
                        Description
                        <input
                          value={detection.description}
                          disabled={!detection.accepted}
                          onChange={(event) =>
                            updateDetection(detection.id, {
                              description: event.target.value,
                            })
                          }
                        />
                      </label>
                      <label>
                        Container
                        <select
                          aria-label={`Container for ${detection.name}`}
                          value={detection.containerId ?? ''}
                          disabled={!detection.accepted}
                          onChange={(event) =>
                            updateDetection(detection.id, {
                              containerId: event.target.value || null,
                            })
                          }
                        >
                          <option value="">Unassigned</option>
                          {containers.map((container) => (
                            <option value={container.id} key={container.id}>
                              {container.name}
                            </option>
                          ))}
                        </select>
                      </label>
                      <p>
                        Confidence {formatConfidence(detection.confidence)}
                        {suggested
                          ? ` · Suggested container: ${suggested.name}`
                          : ''}
                      </p>
                    </div>
                  </article>
                )
              })}
            {analysis.status === 'completed' &&
              analysisDetections.some(
                (detection) => detection.reviewStatus === 'pending',
              ) && (
                <button
                  disabled={analysisBusy}
                  onClick={() => void confirmDetections()}
                >
                  {analysisBusy ? 'Confirming…' : 'Confirm inventory'}
                </button>
              )}
            {analysis.status === 'completed' && !analysisDetections.length && (
              <p className="empty-state">
                Analysis completed without detected inventory items.
              </p>
            )}
          </div>
        )}
      </section>

      <section className="workspace">
        <div className="panel">
          <div className="section-heading">
            <div>
              <p className="eyebrow">PLACES</p>
              <h2>Containers</h2>
            </div>
            <span>{containers.length} total</span>
          </div>

          <form onSubmit={submitContainer} className="editor">
            <label>
              Name
              <input
                required
                value={containerForm.name}
                onChange={(event) =>
                  setContainerForm({
                    ...containerForm,
                    name: event.target.value,
                  })
                }
              />
            </label>
            <label>
              Location
              <input
                value={containerForm.location}
                onChange={(event) =>
                  setContainerForm({
                    ...containerForm,
                    location: event.target.value,
                  })
                }
              />
            </label>
            <label className="full-width">
              Description
              <textarea
                value={containerForm.description}
                onChange={(event) =>
                  setContainerForm({
                    ...containerForm,
                    description: event.target.value,
                  })
                }
              />
            </label>
            <label className="full-width">
              Labels
              <input
                placeholder="office, cables, seasonal"
                value={containerForm.labels.join(', ')}
                onChange={(event) =>
                  setContainerForm({
                    ...containerForm,
                    labels: event.target.value
                      .split(',')
                      .map((label) => label.trim())
                      .filter(Boolean),
                  })
                }
              />
            </label>
            <div className="form-actions full-width">
              <button type="submit">
                {editingContainerId ? 'Save container' : 'Add container'}
              </button>
              {editingContainerId && (
                <button
                  type="button"
                  className="secondary"
                  onClick={() => {
                    setEditingContainerId(null)
                    setContainerForm(emptyContainer)
                  }}
                >
                  Cancel
                </button>
              )}
            </div>
          </form>

          <div className="card-list">
            {containers.map((container) => (
              <article className="resource-card" key={container.id}>
                <div>
                  <h3>{container.name}</h3>
                  <p>{container.location || 'Location not set'}</p>
                  <div className="tag-row">
                    {container.labels.map((label) => (
                      <span className="tag" key={label}>
                        {label}
                      </span>
                    ))}
                  </div>
                </div>
                <div className="resource-meta">
                  <strong>{container.itemCount}</strong>
                  <span>items</span>
                  <button
                    className="text-button"
                    onClick={() => editContainer(container)}
                  >
                    Edit
                  </button>
                  <button
                    className="text-button"
                    onClick={() => {
                      setSelectedContainerId(container.id)
                      setSearch('')
                      void load('', container.id)
                    }}
                  >
                    View items
                  </button>
                  <button
                    className="text-button danger"
                    onClick={() =>
                      void runMutation(async () => {
                        await api.deleteContainer(container.id)
                        setNotice('Container deleted.')
                      })
                    }
                  >
                    Delete
                  </button>
                </div>
              </article>
            ))}
          </div>
        </div>

        <div className="panel">
          <div className="section-heading">
            <div>
              <p className="eyebrow">CATALOG</p>
              <h2>
                {selectedContainerId
                  ? containers.find(
                      (container) => container.id === selectedContainerId,
                    )?.name ?? 'Container'
                  : 'All inventory'}
              </h2>
            </div>
            <span>{items.length} shown</span>
          </div>

          {selectedContainerId && (
            <button
              className="secondary inventory-scope"
              onClick={() => {
                setSelectedContainerId(null)
                setSearch('')
                void load('', null)
              }}
            >
              View all inventory
            </button>
          )}

          <form
            className="search"
            onSubmit={(event) => {
              event.preventDefault()
              void load(search)
            }}
          >
            <input
              aria-label="Search inventory"
              placeholder="Search names and descriptions"
              value={search}
              onChange={(event) => setSearch(event.target.value)}
            />
            <button type="submit">Search</button>
          </form>

          <form onSubmit={submitItem} className="editor">
            <label>
              Name
              <input
                required
                value={itemForm.name}
                onChange={(event) =>
                  setItemForm({ ...itemForm, name: event.target.value })
                }
              />
            </label>
            <label>
              Category
              <input
                value={itemForm.category}
                onChange={(event) =>
                  setItemForm({ ...itemForm, category: event.target.value })
                }
              />
            </label>
            <label>
              Quantity
              <input
                type="number"
                min="1"
                required
                value={itemForm.quantity}
                onChange={(event) =>
                  setItemForm({
                    ...itemForm,
                    quantity: Number(event.target.value),
                  })
                }
              />
            </label>
            <label>
              Description
              <input
                value={itemForm.description}
                onChange={(event) =>
                  setItemForm({
                    ...itemForm,
                    description: event.target.value,
                  })
                }
              />
            </label>
            <div className="form-actions full-width">
              <button type="submit">
                {editingItemId ? 'Save item' : 'Add item'}
              </button>
              {editingItemId && (
                <button
                  type="button"
                  className="secondary"
                  onClick={() => {
                    setEditingItemId(null)
                    setItemForm(emptyItem)
                  }}
                >
                  Cancel
                </button>
              )}
            </div>
          </form>

          {loading ? (
            <p className="empty-state">Loading inventory…</p>
          ) : (
            <div className="inventory-table">
              {items.map((item) => (
                <article className="inventory-row" key={item.id}>
                  <div>
                    <h3>{item.name}</h3>
                    <p>
                      {item.category || 'Uncategorized'} · Quantity{' '}
                      {item.quantity}
                    </p>
                  </div>
                  <label>
                    Container
                    <select
                      aria-label={`Container for ${item.name}`}
                      value={item.containerId ?? ''}
                      onChange={(event) =>
                        void runMutation(async () => {
                          if (event.target.value) {
                            await api.assignItem(item.id, event.target.value)
                            setNotice('Item assigned.')
                          } else {
                            await api.unassignItem(item.id)
                            setNotice('Item unassigned.')
                          }
                        })
                      }
                    >
                      <option value="">Unassigned</option>
                      {containers.map((container) => (
                        <option value={container.id} key={container.id}>
                          {container.name}
                        </option>
                      ))}
                    </select>
                  </label>
                  <div className="row-actions">
                    <span className={`status ${item.assignmentStatus}`}>
                      {item.assignmentStatus}
                    </span>
                    <button
                      className="text-button"
                      onClick={() => editItem(item)}
                    >
                      Edit
                    </button>
                    <button
                      className="text-button danger"
                      onClick={() =>
                        void runMutation(async () => {
                          await api.deleteItem(item.id)
                          setNotice('Item deleted.')
                        })
                      }
                    >
                      Delete
                    </button>
                  </div>
                </article>
              ))}
              {!items.length && (
                <p className="empty-state">No inventory items found.</p>
              )}
              {continuationToken && (
                <button
                  className="secondary load-more"
                  onClick={() => void loadMore()}
                  disabled={loading}
                >
                  {loading ? 'Loading…' : 'Load 100 more'}
                </button>
              )}
            </div>
          )}
        </div>
      </section>
    </main>
  )
}

function SummaryCard({
  label,
  value,
}: {
  label: string
  value: number | undefined
}) {
  return (
    <article className="summary-card">
      <span>{label}</span>
      <strong>{value ?? '—'}</strong>
    </article>
  )
}

function showError(
  caught: unknown,
  setError: (message: string) => void,
) {
  if (caught instanceof ApiError && caught.status === 412) {
    setError('This record changed elsewhere. Refresh and try again.')
    return
  }
  if (caught instanceof ApiError && caught.status === 409) {
    setError(`Conflict: ${caught.message}`)
    return
  }
  setError(caught instanceof Error ? caught.message : 'Something went wrong.')
}

function createIdempotencyKey() {
  return globalThis.crypto?.randomUUID?.() ??
    `${Date.now()}-${Math.random().toString(16).slice(2)}`
}

function formatConfidence(confidence: number | null) {
  return confidence === null ? 'not provided' : `${Math.round(confidence * 100)}%`
}

function toDetectionReviews(analysis: Analysis): DetectionReview[] {
  return analysis.detections.map((detection) => ({
    id: detection.id,
    accepted: detection.reviewStatus !== 'rejected',
    name: detection.reviewedName ?? detection.name,
    description: detection.reviewedDescription ?? detection.description ?? '',
    category: detection.reviewedCategory ?? detection.category ?? '',
    quantity: detection.reviewedQuantity ?? detection.quantity,
    containerId:
      detection.selectedContainerId ??
      analysis.defaultContainerId ??
      detection.suggestedContainerId,
    confidence: detection.confidence,
    suggestedContainerId: detection.suggestedContainerId,
    reviewStatus: detection.reviewStatus,
  }))
}
