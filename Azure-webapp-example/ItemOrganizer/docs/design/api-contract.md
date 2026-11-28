# API contract

[High-level plan](../../PLAN.md) | [Product decisions](milestone-0-decisions.md)

### Conventions

- Base path: `/api/v1`
- Request and response format: JSON, except photo upload content
- Authentication: Microsoft Entra ID bearer access tokens
- Identifiers: server-generated UUIDs
- Dates and times: UTC in ISO 8601 format
- API definition: OpenAPI 3.1, exposed through Swagger UI in development
- Collection paging: `pageSize` and `continuationToken`
- Optional collection filtering: resource-specific query parameters
- Long-running AI analysis: asynchronous operation returning `202 Accepted`
- Successful creation: `201 Created` with a `Location` header
- Successful deletion: `204 No Content`
- Errors: RFC 9457 Problem Details (`application/problem+json`)
- Concurrent updates: `ETag` and `If-Match` headers to prevent lost changes
- Retried create and action requests: optional `Idempotency-Key` header

### System

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/health/live` | Confirm that the API process is running |
| `GET` | `/api/v1/health/ready` | Confirm access to required dependencies |
| `GET` | `/api/v1/summary` | Return counts of containers, photos, items, analyses in progress, and unassigned items |

### Containers

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/containers` | List containers; filter by `search` or `location` |
| `POST` | `/api/v1/containers` | Create a container |
| `GET` | `/api/v1/containers/{containerId}` | Get one container and its item count |
| `PATCH` | `/api/v1/containers/{containerId}` | Update selected container fields |
| `DELETE` | `/api/v1/containers/{containerId}` | Delete an empty container |
| `GET` | `/api/v1/containers/{containerId}/items` | List items assigned to a container |

Container creation fields:

- `name` — required display name
- `description` — optional description used by AI when suggesting assignments
- `location` — optional physical location
- `labels` — optional searchable labels

Deleting a non-empty container returns `409 Conflict`. Its items must first be moved or explicitly unassigned.

### Photos

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/photos` | List photos; filter by `status` or upload date |
| `POST` | `/api/v1/photos` | Upload one photo as `multipart/form-data` and create its record |
| `GET` | `/api/v1/photos/{photoId}` | Get photo metadata and processing status |
| `GET` | `/api/v1/photos/{photoId}/content` | Return a short-lived redirect or read URL for authorized viewing |
| `DELETE` | `/api/v1/photos/{photoId}` | Delete a photo when no analysis is running |

The upload request contains a required `file` part and an optional `containerId` hint. Accepted formats are JPEG, PNG, and WebP, with a maximum size of `10 MiB` and dimensions from `512 x 512` to `8000 x 8000`; animated and multi-frame uploads are rejected. Azure Blob Storage remains private; the API never returns a permanent public blob URL.

### AI analyses

| Method | Route | Purpose |
| --- | --- | --- |
| `POST` | `/api/v1/photos/{photoId}/analyses` | Start analysis of an uploaded photo |
| `POST` | `/api/v1/containers/{containerId}/photo-analyses` | Upload and analyze one photo with the specified container preselected for review |
| `GET` | `/api/v1/analyses/{analysisId}` | Poll analysis state and retrieve results when complete |
| `POST` | `/api/v1/analyses/{analysisId}/cancel` | Request cancellation of queued or running analysis |
| `POST` | `/api/v1/analyses/{analysisId}/confirm` | Confirm reviewed detections and atomically create canonical inventory items |

Starting analysis returns `202 Accepted`, a `Location` header for the analysis resource, and:

```json
{
	"id": "analysis UUID",
	"photoId": "photo UUID",
	"status": "queued",
	"createdAt": "UTC timestamp"
}
```

Analysis states are `queued`, `running`, `completed`, `failed`, and `cancelled`. A completed analysis contains persisted review drafts, not inventory item IDs. Drafts preserve the original AI prediction and remain excluded from inventory search, summaries, container counts, and assignment workflows until a user explicitly confirms them. A failed analysis includes a safe error code and message, but never provider credentials or raw internal exceptions.

`POST /api/v1/containers/{containerId}/photo-analyses` is a convenience workflow using `multipart/form-data` with a required `file` part. It validates the container, stores the photo, creates an analysis, and returns `202 Accepted` with the analysis resource in the same form as the standard analysis endpoint. The selected container is only a review default because the user has not yet seen or confirmed the detected identities. Analysis completion creates review drafts only.

During review, the user may correct a draft's name, description, category, quantity, and container; reject false positives; and leave valid items unassigned. `POST /api/v1/analyses/{analysisId}/confirm` accepts the complete review decision. In one transaction it creates inventory items and assignments only for accepted drafts, records rejected drafts without creating items, and marks the analysis review complete. Confirmation is idempotent and cannot partially promote a review. If analysis or confirmation fails, no unconfirmed detection appears in inventory.

Each AI detection may also include a proposed normalized bounding box:

```json
{
	"x": 0.18,
	"y": 0.32,
	"width": 0.41,
	"height": 0.12
}
```

`x` and `y` identify the top-left corner, and all four values are decimal fractions of the decoded source-photo width and height in the inclusive range `0` to `1`. `x + width` and `y + height` must not exceed `1`. The provider is instructed to return the smallest axis-aligned rectangle containing the visible portion of the item. Missing or uncertain localization is represented by `null`, not fabricated coordinates. Invalid boxes must not enter persisted drafts. The live provider removes invalid optional geometry with a review warning; nonnumeric geometry or invalid required fields reject the output.

Bounding boxes are AI proposals rather than authoritative geometry. The review UI overlays each proposed box on the source photo and allows the user to move, resize, remove, or create a box before confirmation. The original predicted box and the reviewed box are stored separately. Only the reviewed box may be used to generate an item crop.

### Items and assignments

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/items` | Search inventory; filter by `search`, `category`, `containerId`, `photoId`, or assignment state |
| `POST` | `/api/v1/items` | Manually create an item not detected from a photo |
| `GET` | `/api/v1/items/{itemId}` | Get an item, its source photo, and assignment details |
| `GET` | `/api/v1/items/{itemId}/crop` | Return a short-lived authorized URL for the confirmed item crop, when present |
| `PATCH` | `/api/v1/items/{itemId}` | Correct an item's editable properties |
| `DELETE` | `/api/v1/items/{itemId}` | Remove an item from the catalog |
| `PUT` | `/api/v1/items/{itemId}/container` | Assign or move an item to a container |
| `DELETE` | `/api/v1/items/{itemId}/container` | Mark an item as unassigned |

**Hybrid search contract (Milestone 8A):**

- Preserve `GET /api/v1/items?search=...` and container item listing as the
  existing case-insensitive name/description substring search, including their
  current ordering and pagination.
- Add `POST /api/v1/items/search` for relevance-ranked hybrid search. The
  request contains `query`, optional `category`, `containerId`, `photoId`, and
  `assignmentStatus` filters, `pageSize`, and an optional `continuationToken`.
  Apply the existing read authorization policy. A POST body avoids putting
  natural-language queries into URLs; request-body logging must also be disabled.
- Return existing item representations in a paged response plus the effective
  search mode and an explicit degradation warning when semantic retrieval is
  unavailable. OpenAPI describes the response schema and contract tests verify it.
  Invalid or overlong queries return the standard validation problem response;
  blank UI input continues to use the existing inventory listing.

An AI detection review draft contains:

- `name`
- `description`
- `category`
- `quantity`
- `confidence` from `0` to `1`
- `photoId`
- `analysisId`
- `suggestedContainerId`, when AI finds a likely match
- `predictedBoundingBox`, when the provider can localize the item
- `reviewedBoundingBox`, after the user accepts or adjusts the proposal
- `reviewStatus`: `pending`, `accepted`, or `rejected`
- `resultingItemId`, only after an accepted draft has been promoted

Only manually created items and explicitly accepted detection drafts are inventory items. AI suggestions never appear in `GET /api/v1/items`, summary counts, search results, or container contents before confirmation. Container assignment remains explicit; an accepted detection may be confirmed as unassigned.

When an accepted draft has a reviewed bounding box, confirmation crops the decoded source image using validated pixel coordinates, encodes the derivative in an approved non-animated format, and writes it to private Blob Storage under a server-generated name. PostgreSQL stores only crop metadata and the private blob reference, including source photo ID, reviewed normalized coordinates, pixel dimensions, content type, length, and hash. Crop creation and inventory promotion must have deterministic compensation behavior: a database failure removes the newly written crop, while a storage failure creates no item. Crop authorization, retention, and deletion follow the source item and photo ownership rules; deleting an item schedules its crop for deletion without silently deleting the source photo.

### Assignment request

```json
{
	"containerId": "container UUID",
	"acceptSuggestion": true
}
```

The assignment endpoint returns the updated item. Moving an item is atomic; no separate removal request is needed.

### Authorization policy

Initial delegated scopes:

- `ItemOrganizer.Read` — view containers, photos, analyses, and items
- `ItemOrganizer.Write` — upload photos and create or edit containers and items
- `ItemOrganizer.Analyze` — start or cancel AI analyses

The same application permissions may later be exposed for daemon tools. The initial release supports one approved Entra tenant and requires matching `tenantId` and `ownerObjectId` for delegated resource access. Combined upload-and-analysis routes require both `ItemOrganizer.Write` and `ItemOrganizer.Analyze`. Detailed allow and deny examples are recorded in the [approved product decisions](milestone-0-decisions.md).

### Standard errors

Expected responses include:

- `400 Bad Request` — malformed request or invalid state transition
- `401 Unauthorized` — missing or invalid access token
- `403 Forbidden` — token lacks the required scope or resource access
- `404 Not Found` — resource does not exist or is not visible to the caller
- `409 Conflict` — duplicate, non-empty container deletion, or incompatible operation state
- `412 Precondition Failed` — stale `ETag`
- `413 Content Too Large` — photo exceeds the configured limit
- `415 Unsupported Media Type` — unsupported photo format
- `422 Unprocessable Content` — semantically invalid fields
- `429 Too Many Requests` — caller or AI quota exceeded; includes `Retry-After`
- `500 Internal Server Error` — unexpected server failure with a correlation ID
- `503 Service Unavailable` — required Azure dependency is unavailable
