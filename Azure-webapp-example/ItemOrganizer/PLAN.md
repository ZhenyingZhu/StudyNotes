# Item Organizer — Plan

## Status

**Awaiting guidance. No implementation is authorized.**

## Goal

Plan an application that:

- Accepts photos.
- Allows containers to be entered.
- Uses AI to identify items in photos.
- Associates identified items with containers.
- Exposes web APIs for other tools.

## Approved technology stack

- **Frontend:** React with TypeScript
- **Backend/API:** ASP.NET Core Web API on .NET 10
- **Database:** Azure SQL with Entity Framework Core
- **Photo storage:** Azure Blob Storage
- **AI:** Azure OpenAI with `gpt-5.4-mini` for vision-based item identification and structured outputs
- **Authentication:** Microsoft Entra ID
- **API contract:** REST with OpenAPI/Swagger
- **Hosting:** Azure Static Web Apps Standard for the frontend; Azure App Service for the API
- **Testing:** xUnit for the backend; Vitest and Playwright for the frontend
- **Infrastructure as code:** Bicep

## Web API contract

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

The upload request contains a required `file` part and an optional `containerId` hint. Accepted formats, maximum size, and image dimensions will be finalized with the validation requirements. Azure Blob Storage remains private; the API never returns a permanent public blob URL.

### AI analyses

| Method | Route | Purpose |
| --- | --- | --- |
| `POST` | `/api/v1/photos/{photoId}/analyses` | Start analysis of an uploaded photo |
| `POST` | `/api/v1/containers/{containerId}/photo-analyses` | Upload and analyze one photo, then assign every detected item to the specified container |
| `GET` | `/api/v1/analyses/{analysisId}` | Poll analysis state and retrieve results when complete |
| `POST` | `/api/v1/analyses/{analysisId}/cancel` | Request cancellation of queued or running analysis |

Starting analysis returns `202 Accepted`, a `Location` header for the analysis resource, and:

```json
{
	"id": "analysis UUID",
	"photoId": "photo UUID",
	"status": "queued",
	"createdAt": "UTC timestamp"
}
```

Analysis states are `queued`, `running`, `completed`, `failed`, and `cancelled`. A completed analysis includes detected item IDs. A failed analysis includes a safe error code and message, but never provider credentials or raw internal exceptions.

`POST /api/v1/containers/{containerId}/photo-analyses` is a convenience workflow using `multipart/form-data` with a required `file` part. It validates the container, stores the photo, creates an analysis, and returns `202 Accepted` with the analysis resource in the same form as the standard analysis endpoint. When analysis completes, all items detected in that photo are assigned to the specified container with `assignmentStatus` set to `confirmed`. The explicit container selection in this request constitutes user confirmation; it is not an AI suggestion. Creation of the detected items and their assignments is committed atomically. If upload or analysis fails, no detected items or assignments are created; a stored photo may remain available for retry or deletion.

### Items and assignments

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/v1/items` | Search inventory; filter by `search`, `category`, `containerId`, `photoId`, or assignment state |
| `POST` | `/api/v1/items` | Manually create an item not detected from a photo |
| `GET` | `/api/v1/items/{itemId}` | Get an item, its source photo, and assignment details |
| `PATCH` | `/api/v1/items/{itemId}` | Correct an item's editable properties |
| `DELETE` | `/api/v1/items/{itemId}` | Remove an item from the catalog |
| `PUT` | `/api/v1/items/{itemId}/container` | Assign or move an item to a container |
| `DELETE` | `/api/v1/items/{itemId}/container` | Mark an item as unassigned |

An AI-detected item contains:

- `name`
- `description`
- `category`
- `quantity`
- `confidence` from `0` to `1`
- `photoId`
- `analysisId`
- `containerId`, when confirmed or assigned
- `suggestedContainerId`, when AI finds a likely match
- `assignmentStatus`: `unassigned`, `suggested`, or `confirmed`

AI suggestions do not silently become confirmed assignments. A client or user confirms them through `PUT /api/v1/items/{itemId}/container`.

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

The same application permissions may later be exposed for daemon tools. Tenant boundaries and ownership rules remain to be defined before implementation.

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

## Guidance to capture

The following decisions are intentionally open:

1. User experience and workflow
2. Azure OpenAI item-identification behavior
3. Container-association rules
4. Data model and storage details
5. Tenant boundaries, ownership, and detailed authorization rules
6. Photo validation and retention requirements
7. Deployment process
8. Testing and acceptance criteria

## Next step

Revise this plan from the user's guidance. Do not create application code, configuration, dependencies, or additional files until the plan is explicitly accepted.
