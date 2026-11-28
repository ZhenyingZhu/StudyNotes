# Milestone 0 - Product and ownership model

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Approved technology stack

- **Frontend:** React with TypeScript
- **Backend/API:** ASP.NET Core Web API on .NET 10
- **Database:** Azure Database for PostgreSQL Flexible Server with Entity Framework Core and Npgsql
- **Photo storage:** Azure Blob Storage
- **AI:** Azure OpenAI with `gpt-5.4-mini` for vision-based item identification and structured outputs
- **Authentication:** Microsoft Entra ID
- **API contract:** REST with OpenAPI/Swagger
- **Hosting:** Azure Static Web Apps Standard for the frontend; Azure App Service for the API
- **Testing:** xUnit for the backend; Vitest and Playwright for the frontend
- **Infrastructure as code:** Bicep

## Scope

Define the inventory model, photo-to-review workflow, ownership boundaries,
assignment rules and safety constraints shared by the API and frontend.

## Requirements

- A written workflow describes photo upload, container creation, analysis polling, item review, assignment, correction, deletion, and retry behavior.
- The approved AI output schema defines required fields, confidence handling, malformed-output handling, prompt/version tracking, and provider failure behavior.
- Container-association rules distinguish explicit user confirmation from AI suggestions and define duplicate-item behavior.
- Tenant, ownership, delegated-scope, and resource-authorization rules are documented with allow and deny examples.
- Photo format, size, dimension, retention, deletion, and privacy requirements are documented.
- The queue/worker model, cancellation semantics, retry policy, idempotency behavior, and deployment mode are selected.
- Acceptance criteria are recorded for functional, security, performance, reliability, and accessibility requirements.

## Design approval

Product and technical owners approve the shared product model, ownership
rules and acceptance criteria before implementation.

## Product model

Selecting a container during upload provides a review default, not confirmation
of unseen detections. Analysis completion creates review drafts. Only explicit
review confirmation creates canonical inventory. Measured recognition-quality
benchmarking is outside the release gate; provider safety and review requirements
remain mandatory.

### Decision summary

- Initial release scope is a single Microsoft Entra tenant with per-user ownership boundaries.
- Production deployment uses Azure App Service's managed .NET runtime for the API rather than a custom production container.
- AI analysis uses Azure OpenAI structured output with an application-owned schema version and prompt version.
- Item-to-container suggestions remain suggestions; only explicit user actions create confirmed assignments.
- The asynchronous analysis pipeline uses Azure Storage Queue plus a SQL-backed outbox/operation record for reliable dispatch and retry handling.
- Photo storage remains private, with short-lived read access issued only through the API.

### End-to-end workflows

**Standard container workflow:**

1. A signed-in user creates a container with a required name and optional description, location, and labels.
2. The API stores the container with the caller's `tenantId` and `ownerObjectId`.
3. The user uploads one photo through `POST /api/v1/photos`.
4. The API validates media type, file size, image dimensions, and duplicate idempotency metadata before storing the private blob and photo record.
5. The user starts analysis through `POST /api/v1/photos/{photoId}/analyses`.
6. The API creates an `Analysis` record in `queued` state, writes an outbox entry, and returns `202 Accepted`.
7. The client polls `GET /api/v1/analyses/{analysisId}` until the analysis reaches `completed`, `failed`, or `cancelled`.
8. Successful analysis exposes pending review drafts with source photo, confidence, suggested container, and optional predicted bounding box; no inventory items are created.
9. The user corrects draft fields and geometry, accepts or rejects detections, and explicitly confirms through `POST /api/v1/analyses/{analysisId}/confirm`. Accepted drafts become inventory items, optionally assigned to a selected container, with private crops only from reviewed geometry.
10. Later assignment changes use `PUT /api/v1/items/{itemId}/container`. Photos cannot be deleted while analysis is running or while referenced by inventory, and non-empty containers cannot be deleted.

**Convenience workflow: upload directly to a known container:**

1. A signed-in user selects an existing container they own.
2. The user uploads a photo through `POST /api/v1/containers/{containerId}/photo-analyses`.
3. The target container is preselected for review; it does not count as confirmation of unseen detections.
4. Successful analysis creates review drafts only. The user reviews identities, quantities, assignments, and boxes before explicitly confirming accepted detections.
5. Confirmation commits accepted items and assignments atomically. Upload, queueing, analysis, or confirmation failure never commits unconfirmed inventory or partial assignments.
6. The stored photo may remain available for retry or deletion according to the retention rules below.

**Retry, correction, and deletion rules:**

- `Idempotency-Key` is supported on photo upload, analysis creation, and the convenience workflow.
- A retry with the same idempotency key and same authenticated owner returns the existing result instead of creating duplicate operations.
- Users may correct draft fields before confirmation and item name, description, category, quantity, and assignment afterward.
- Users may delete failed or cancelled photos immediately unless the photo is referenced by a completed item record that has not been removed.
- Deleting an item never deletes its source photo automatically; it schedules private crop cleanup.

### AI output schema and handling

The worker expects Azure OpenAI structured output matching the application
schema. An illustrative result is:

```json
{
  "schemaVersion": "item-organizer.analysis-result.v1",
  "photoId": "4d9e0ac7-4fdd-482d-b69e-a37d39fe8f2d",
  "promptVersion": "2026-09-15.m1",
  "model": "gpt-5.4-mini",
  "items": [
    {
      "name": "USB-C cable",
      "description": "Black braided cable",
      "category": "electronics",
      "quantity": 2,
      "confidence": 0.91,
      "suggestedContainerId": null,
      "suggestedContainerReason": null,
      "boundingBox": null
    }
  ],
  "warnings": []
}
```

**Required fields:**

- Top level: `schemaVersion`, `photoId`, `promptVersion`, `model`, `items`, and `warnings`.
- Per item: `name`, `quantity`, and `confidence` require values.
- Description, category, suggested container ID and reason, and bounding box are optional information. The live provider's strict schema requires these properties to be present with `null` when unavailable.
- Bounding boxes follow the normalized geometry and validation rules in the [API contract](api-contract.md).

**Confidence handling:**

- `confidence` must be numeric and between `0` and `1`, inclusive.
- Detections with `confidence < 0.35` are discarded and recorded as warnings.
- Detections with `0.35 <= confidence < 0.80` may become review drafts, but receive no AI container suggestion; the user may explicitly choose a container during confirmation.
- AI-generated `suggestedContainerId` is accepted only when `confidence >= 0.80` and the referenced container is visible to the detection's owner.
- No confidence threshold bypasses explicit review confirmation.

**Malformed and partial output handling:**

- Schema-validation failure transitions the analysis to `failed` with safe code `AI_OUTPUT_INVALID`.
- Raw provider payloads are not persisted in ordinary logs or client-visible errors.
- Malformed structured output commits no partial review drafts or inventory records.
- Discarding low-confidence detections from otherwise valid output may still produce a completed analysis with warnings.
- Invalid optional bounding-box geometry from the live provider is discarded
  with a review warning, preserving the detection without a crop. Nonnumeric
  geometry and invalid required fields still fail analysis validation.

**Prompt and model version tracking:**

- Every analysis stores `promptVersion`, `schemaVersion`, `model`, and the application build version.
- Schema changes require a new `schemaVersion`; prompt changes preserving the schema require a new `promptVersion`.
- Reprocessing a photo under a new prompt or schema creates a new analysis record; prior completed predictions remain immutable while review decisions are recorded separately.
- `Analysis:Provider` selects `deterministic` or `azure-openai`. The deterministic provider remains the default for local and CI tests.
- The live provider sends the private photo directly from Blob Storage to the Azure OpenAI Responses API and requires `Analysis:Model` to identify the deployment.
- Live authentication uses managed identity through `DefaultAzureCredential` by default. A development API key may be injected through an approved secret source but must not be committed.

**Provider failure behavior:**

- Timeouts, throttling, and transient provider errors surface as `AI_PROVIDER_TEMPORARY_FAILURE`.
- Non-retryable provider validation errors surface as `AI_PROVIDER_REJECTED_REQUEST`.
- Client-visible errors include only a safe code, a user-actionable message, and a correlation ID.

### Container association and duplicate-item rules

**Assignment states:**

- `unassigned` - no container has been selected.
- `suggested` - the AI proposed a visible container, but the user has not confirmed it.
- `confirmed` - the user explicitly selected or accepted the container.

**Assignment rules:**

- The convenience endpoint preselects a container but does not confirm assignments; confirmation requires reviewing the detections.
- Standard AI analyses never create `confirmed` inventory assignments automatically.
- `acceptSuggestion = true` on `PUT /api/v1/items/{itemId}/container` is allowed only when the requested `containerId` matches the current `suggestedContainerId`.
- Moving an item replaces its existing assignment atomically.
- Removing an assignment clears `containerId` and `suggestedContainerId` and sets `assignmentStatus = unassigned`.

**Duplicate handling:**

- Duplicate detections within one analysis are merged when normalized `name`, `category`, and effective `suggestedContainerId` match exactly; merged records sum `quantity` and keep the highest confidence.
- The system does not automatically merge newly detected items with pre-existing catalog items in other photos during v1.
- Users may manually consolidate duplicates later through item edits or administrative cleanup tooling introduced in a future milestone.

### Data model and storage choices

The logical persistence records are:

- `Container` - user-owned metadata about a physical storage place.
- `Photo` - uploaded image metadata, private storage pointer, hash, dimensions, retention state, and owner.
- `Analysis` - operation state, safe errors, prompt/model metadata, timestamps, and cancellation metadata.
- `AnalysisDetection` - pending, accepted, or rejected review draft preserving original predictions, reviewed fields and geometry, and resulting item linkage.
- `Item` - confirmed detected or manual inventory record, including private crop metadata when present.
- `ItemAssignment` - current container relationship, assignment source, and state.
- `OutboxMessage` - reliable queue-dispatch record for asynchronous analysis.

Azure Database for PostgreSQL Flexible Server stores metadata and relational
state. Azure Blob Storage stores original photos and confirmed crops privately.
Azure Storage Queue carries asynchronous analysis work items. Long-lived
public blob URLs are not used.

### Tenant, ownership, and authorization rules

**Tenant and ownership boundaries:**

- The initial release supports one Microsoft Entra tenant. Every user-owned record still stores `tenantId` to permit future multi-tenant deployment; tokens from unapproved tenants are denied.
- Every container, photo, analysis, and item is owned by exactly one user object ID.
- A delegated caller may access only resources whose `tenantId` and `ownerObjectId` match the access token.
- Background workers use application permissions internally but enforce the original resource owner on every state transition.

**Scope requirements:**

- `ItemOrganizer.Read` is required for all `GET` routes.
- `ItemOrganizer.Write` is required for container, photo, and item creation or mutation routes.
- `ItemOrganizer.Analyze` is required for starting or cancelling analyses.
- Mutating routes combining upload and analysis require both `ItemOrganizer.Write` and `ItemOrganizer.Analyze`.

**Allow examples:**

- A user with `ItemOrganizer.Read` can list their own containers.
- A user with `ItemOrganizer.Write` can upload a photo they will own.
- A user with both `ItemOrganizer.Write` and `ItemOrganizer.Analyze` can use `POST /api/v1/containers/{containerId}/photo-analyses` on a container they own.

**Deny examples:**

- A token from another Entra tenant receives `403 Forbidden`, even if scopes are present.
- A same-tenant user cannot read or mutate another user's containers, photos, analyses, or items.
- A user with `ItemOrganizer.Read` alone cannot upload photos or change assignments.
- A user with `ItemOrganizer.Write` but without `ItemOrganizer.Analyze` cannot start or cancel analysis.

### Photo validation, privacy, retention, and deletion

**Accepted formats and limits:**

- Accepted media types: `image/jpeg`, `image/png`, and `image/webp`.
- Maximum upload size: `10 MiB`.
- Minimum dimensions: `512 x 512`.
- Maximum dimensions: `8000 x 8000`.
- Animated image formats and multi-frame uploads are rejected.

**Privacy rules:**

- Photos are private by default and stored in non-public Blob Storage.
- The API returns only short-lived read access for authorized callers.
- AI prompts and logs must not include user names, access tokens, or container labels unrelated to the requested photo.
- Test artifacts use synthetic photos or approved fixtures only.

**Retention and deletion:**

- Completed photos are retained for `30 days` by default to support review and reassessment.
- Failed or cancelled analyses retain their photos for `7 days` by default to support retry.
- User-requested deletion soft-deletes metadata immediately and hard-deletes blob content asynchronously within `15 minutes`, subject to the active-analysis and item-reference restrictions above.
- Retention-deletion jobs are idempotent and leave a safe audit trail without preserving readable photo content.

### Queue, worker, cancellation, retry, and idempotency design

**Selected design:**

- The API writes an `Analysis` row and an `OutboxMessage` row in one SQL transaction.
- A dispatcher forwards pending outbox entries to Azure Storage Queue.
- A worker dequeues messages, claims an analysis if still `queued`, and processes it idempotently.

**Cancellation semantics:**

- Cancelling a `queued` analysis changes it directly to `cancelled`.
- Cancelling a `running` analysis records a request checked before result persistence.
- Cancellation after provider success but before commit discards results and stores `cancelled`.
- Completed, failed, or already-cancelled analyses reject further cancellation with `409 Conflict`.

**Retry policy:**

- Transient worker and provider failures use exponential backoff, permitting five retries after the initial delivery.
- The sixth failed delivery marks the analysis `failed` and moves the message to dead-letter handling.
- Permanent validation failures do not retry.

**Idempotency behavior:**

- Queue messages carry the analysis ID only; the database is the source of truth.
- Reprocessing checks persisted analysis state before acting.
- Completed analysis never creates drafts twice, and confirmation never promotes them twice; state transitions and unique database constraints guard persistence.

### Acceptance criteria

**Functional:**

- Users can create containers, upload photos, start analyses, poll status, review detections, confirm inventory and assignments, correct data, and delete allowed resources through the documented API.
- Every successful create route returns the canonical resource location and server-generated UUID.
- Every asynchronous analysis route returns a pollable analysis resource.

**Security:**

- All routes enforce delegated scopes and owner-based resource access.
- Permanent blob URLs, raw provider payloads, secrets, and tokens never appear in API responses or normal logs.
- Cross-tenant and cross-owner access attempts are denied and covered by tests.

**Performance:**

- A valid `10 MiB` photo upload is accepted or rejected within `5 seconds` under normal local-development conditions before AI processing starts.
- Read-only list endpoints return the first page within `1 second` against representative development data.
- Polling an existing analysis resource returns within `500 ms` on local development hardware when dependencies are healthy.

**Reliability:**

- Repeating an idempotent upload or analysis-start request with the same key does not create duplicate records.
- Worker retries do not create duplicate items or assignments.
- Dependency failures produce safe Problem Details responses and preserve recoverable state.

**Accessibility:**

- The frontend supports keyboard-only upload, polling, review, assignment, correction, and deletion workflows.
- Status changes and validation errors are exposed to assistive technologies.
- Color alone does not convey analysis or assignment state.
