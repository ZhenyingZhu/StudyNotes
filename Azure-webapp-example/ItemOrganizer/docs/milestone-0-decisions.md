# Milestone 0 Decisions

This document resolves the open decisions listed in `PLAN.md` so implementation can proceed without revisiting core product and security questions.

## Decision summary

- Initial release scope is a single Microsoft Entra tenant with per-user ownership boundaries.
- Production deployment uses Azure App Service's managed .NET runtime for the API rather than a custom production container.
- AI analysis uses Azure OpenAI structured output with an application-owned schema version and prompt version.
- Item-to-container suggestions remain suggestions; only explicit user actions create confirmed assignments.
- The asynchronous analysis pipeline uses Azure Storage Queue plus a SQL-backed outbox/operation record for reliable dispatch and retry handling.
- Photo storage remains private, with short-lived read access issued only through the API.

## End-to-end workflow

### Standard container workflow

1. A signed-in user creates a container with a required name and optional description, location, and labels.
2. The API stores the container with the caller's `tenantId` and `ownerObjectId`.
3. The user uploads one photo through `POST /api/v1/photos`.
4. The API validates media type, file size, image dimensions, and duplicate idempotency metadata before storing the private blob and photo record.
5. The user starts analysis through `POST /api/v1/photos/{photoId}/analyses`.
6. The API creates an `Analysis` record in `queued` state, writes an outbox entry, and returns `202 Accepted`.
7. The client polls `GET /api/v1/analyses/{analysisId}` until the analysis reaches `completed`, `failed`, or `cancelled`.
8. When the worker completes successfully, the API exposes AI-detected items with `assignmentStatus` of either `unassigned` or `suggested`.
9. The user reviews the detected items, corrects fields when needed, and explicitly confirms assignments through `PUT /api/v1/items/{itemId}/container`.
10. Deletion follows ownership and state rules: photos cannot be deleted while analysis is running, and non-empty containers cannot be deleted.

### Convenience workflow: upload directly to a known container

1. A signed-in user selects an existing container they own.
2. The user uploads a photo through `POST /api/v1/containers/{containerId}/photo-analyses`.
3. The explicit target container counts as user confirmation for all newly created item assignments.
4. When the analysis completes successfully, all created items are committed atomically with `assignmentStatus = confirmed`.
5. If upload, queueing, or analysis result persistence fails, no items or assignments are committed.
6. The stored photo may remain available for retry or deletion according to the retention rules below.

### Retry, correction, and deletion rules

- `Idempotency-Key` is supported on photo upload, analysis creation, and the convenience workflow.
- A retry with the same idempotency key and same authenticated owner returns the existing result instead of creating duplicate operations.
- Users may correct item name, description, category, quantity, and assignment after analysis completes.
- Users may delete failed or cancelled photos immediately unless the photo is referenced by a completed item record that has not been removed.
- Deleting an item never deletes its source photo automatically.

## AI output schema and handling

### Structured output contract

The worker expects Azure OpenAI structured output that matches the application schema exactly:

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
      "suggestedContainerReason": null
    }
  ],
  "warnings": []
}
```

### Required fields

- Top level: `schemaVersion`, `photoId`, `promptVersion`, `model`, and `items`
- Per item: `name`, `quantity`, and `confidence`
- Optional per item: `description`, `category`, `suggestedContainerId`, and `suggestedContainerReason`

### Confidence handling

- `confidence` must be a numeric value between `0` and `1`, inclusive.
- Items with `confidence < 0.35` are discarded and recorded as warnings in the analysis result.
- Items with `0.35 <= confidence < 0.80` may be created, but they remain `unassigned` unless explicitly confirmed later by the user.
- AI-generated `suggestedContainerId` is accepted only when `confidence >= 0.80` and the referenced container is visible to the item's owner.

### Malformed and partial output handling

- If the provider response fails schema validation, the analysis transitions to `failed` with safe code `AI_OUTPUT_INVALID`.
- The raw provider payload is not persisted in ordinary logs or client-visible errors.
- No partial item records are committed when the structured output is malformed.
- If some low-confidence items are discarded but the overall schema is valid, the analysis may still complete successfully and report warnings.

### Prompt and model version tracking

- Every analysis stores `promptVersion`, `schemaVersion`, `model`, and the application build version.
- Schema changes require a new `schemaVersion`; prompt changes that preserve the same schema require a new `promptVersion`.
- Reprocessing a photo under a new prompt or schema creates a new analysis record; prior completed analyses remain immutable.

### Provider failure behavior

- Timeouts, throttling, and transient provider errors surface as `AI_PROVIDER_TEMPORARY_FAILURE`.
- Non-retryable provider validation errors surface as `AI_PROVIDER_REJECTED_REQUEST`.
- Client-visible errors include only a safe code, a user-actionable message, and a correlation ID.

## Container association and duplicate-item rules

### Assignment states

- `unassigned` - no container has been selected
- `suggested` - the AI proposed a visible container, but the user has not confirmed it
- `confirmed` - the user explicitly selected the container, including the convenience workflow

### Rules

- The convenience endpoint is treated as explicit user confirmation, not an AI suggestion.
- Standard AI analyses never create `confirmed` assignments automatically.
- `acceptSuggestion = true` on `PUT /api/v1/items/{itemId}/container` is allowed only when the requested `containerId` matches the current `suggestedContainerId`.
- Moving an item replaces the existing assignment atomically.
- Removing an assignment clears `containerId`, clears `suggestedContainerId`, and sets `assignmentStatus = unassigned`.

### Duplicate handling

- Duplicate detections within the same analysis are merged when their normalized `name`, `category`, and `suggestedContainerId` match exactly; merged records sum `quantity` and keep the highest confidence.
- The system does not auto-merge newly detected items with pre-existing catalog items in other photos during v1.
- Users may manually consolidate duplicates later through item edits or administrative cleanup tooling introduced in a future milestone.

## Data model and storage choices

The initial persistence model uses these logical records:

- `Container` - user-owned metadata about a physical storage place
- `Photo` - uploaded image metadata plus storage pointer, hash, dimensions, retention state, and owner
- `Analysis` - operation state, safe error details, prompt/model metadata, timestamps, and cancellation metadata
- `Item` - normalized detected or manual inventory record
- `ItemAssignment` - current container relationship, assignment source, and state
- `OutboxMessage` - reliable queue-dispatch record for asynchronous analysis

Storage choices:

- Azure SQL stores metadata and relational state.
- Azure Blob Storage stores original uploaded photos in a private container.
- Azure Storage Queue carries asynchronous analysis work items.
- Long-lived public blob URLs are not used.

## Tenant, ownership, and authorization rules

### Tenant boundary

The initial release supports one Microsoft Entra tenant, but every user-owned record still stores `tenantId` so a future multi-tenant deployment remains possible. Tokens from any unapproved tenant are denied.

### Ownership boundary

- Every container, photo, analysis, and item is owned by exactly one user object ID.
- A delegated caller may access only resources whose `tenantId` and `ownerObjectId` match the access token.
- Background workers use application permissions internally but must enforce the original resource owner on every state transition.

### Scope requirements

- `ItemOrganizer.Read` is required for all `GET` routes.
- `ItemOrganizer.Write` is required for container, photo, and item creation or mutation routes.
- `ItemOrganizer.Analyze` is required for starting or cancelling analyses.
- Mutating routes that combine upload and analysis require both `ItemOrganizer.Write` and `ItemOrganizer.Analyze`.

### Allow examples

- A user with `ItemOrganizer.Read` can list their own containers.
- A user with `ItemOrganizer.Write` can upload a photo they will own.
- A user with both `ItemOrganizer.Write` and `ItemOrganizer.Analyze` can use `POST /api/v1/containers/{containerId}/photo-analyses` on a container they own.

### Deny examples

- A token from another Entra tenant receives `403 Forbidden`, even if scopes are present.
- A same-tenant user cannot read or mutate another user's containers, photos, analyses, or items.
- A user with `ItemOrganizer.Read` alone cannot upload photos or change assignments.
- A user with `ItemOrganizer.Write` but without `ItemOrganizer.Analyze` cannot start or cancel analysis.

## Photo validation, privacy, retention, and deletion

### Accepted formats and limits

- Accepted media types: `image/jpeg`, `image/png`, `image/webp`
- Maximum upload size: `10 MiB`
- Minimum dimensions: `512 x 512`
- Maximum dimensions: `8000 x 8000`
- Animated image formats and multi-frame uploads are rejected.

### Privacy rules

- Photos are private by default and stored in non-public Blob Storage.
- The API returns only short-lived read access for authorized callers.
- AI prompts and logs must not include user names, access tokens, or container labels unrelated to the requested photo.
- Test artifacts must use synthetic photos or approved fixtures only.

### Retention and deletion

- Completed photos are retained for `30 days` by default to support review and reassessment.
- Failed or cancelled analyses retain their photos for `7 days` by default to support retry.
- User-requested deletion performs a soft-delete in metadata immediately and a hard-delete of blob content asynchronously within `15 minutes`.
- Retention-deletion jobs must be idempotent and leave a safe audit trail without preserving readable photo content.

## Queue, worker, cancellation, retry, and idempotency design

### Selected design

- The API writes an `Analysis` row and an `OutboxMessage` row in one SQL transaction.
- A dispatcher process forwards pending outbox entries to Azure Storage Queue.
- A worker service dequeues analysis messages, claims the analysis if it is still `queued`, and processes one analysis idempotently.

### Cancellation semantics

- Cancelling a `queued` analysis changes it directly to `cancelled`.
- Cancelling a `running` analysis records a cancellation request; the worker checks for cancellation before persisting results.
- If cancellation arrives after the provider call succeeds but before commit, the worker discards results and stores `cancelled`.
- Completed, failed, or already-cancelled analyses reject further cancellation with `409 Conflict`.

### Retry policy

- Transient worker and provider failures retry with exponential backoff for up to `5` delivery attempts.
- The sixth failure marks the analysis `failed` and moves the message to dead-letter handling.
- Permanent validation failures do not retry.

### Idempotency behavior

- Queue messages carry the analysis ID only; the database remains the source of truth.
- The worker can safely reprocess the same queue message because it first checks the persisted analysis state.
- A completed analysis never creates items twice; persistence is guarded by analysis-state transitions and unique database constraints.

## Selected production deployment mode

The initial production deployment will use Azure App Service's managed .NET runtime, not a custom API container. This keeps early delivery focused on application behavior and Azure integrations while preserving the option to adopt a production container later if operational needs justify it.

## Acceptance criteria

### Functional

- Users can create containers, upload photos, start analyses, poll status, review items, confirm assignments, correct data, and delete allowed resources through the documented API.
- Every successful create route returns the canonical resource location and server-generated UUID.
- Every asynchronous analysis route returns a pollable analysis resource.

### Security

- All routes enforce delegated scopes and owner-based resource access.
- Permanent blob URLs, raw provider payloads, secrets, and tokens never appear in API responses or normal logs.
- Cross-tenant and cross-owner access attempts are denied and covered by tests.

### Performance

- A valid `10 MiB` photo upload is accepted or rejected within `5 seconds` under normal local-development conditions before AI processing starts.
- Read-only list endpoints return the first page within `1 second` against representative development data.
- Polling an existing analysis resource returns within `500 ms` on local development hardware when dependencies are healthy.

### Reliability

- Repeating an idempotent upload or analysis-start request with the same idempotency key does not create duplicate records.
- Worker retries do not create duplicate items or assignments.
- Dependency failures produce safe Problem Details responses and preserve recoverable state.

### Accessibility

- The future frontend must support keyboard-only operation for upload, polling, review, assignment, correction, and deletion workflows.
- Status changes and validation errors must be exposed to assistive technologies.
- Color alone must not convey analysis or assignment state.
