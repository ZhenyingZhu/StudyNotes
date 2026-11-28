# Milestone 7 - AI-assisted review and convenience workflow

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Require explicit human confirmation before any AI detection becomes canonical inventory.

## Requirements

- Completed analyses display pending review drafts with source photo, confidence, category, quantity, and container suggestions.
- Completed analyses overlay proposed bounding boxes on the source photo and remain usable when a provider returns no box.
- Users can correct draft fields, accept valid detections, reject false positives, choose or remove container assignments, and create, move, resize, or remove bounding boxes before confirmation.
- Pending and rejected detections never appear in inventory search, summaries, container counts, or item routes.
- One explicit confirmation operation creates inventory items and private crops only for accepted drafts and preserves original predictions plus reviewed values and geometry for evaluation.
- The convenience upload-and-analyze endpoint validates the container, stores the photo, queues analysis, and returns the same asynchronous contract.
- The convenience workflow preselects the requested container for review but does not bypass item confirmation.
- Retry, refresh, duplicate submission, stale `ETag`, invalid geometry, crop-storage failure, and partial dependency failure scenarios are covered by API and frontend tests.
- Playwright verifies upload, bounding-box review, rejection, correction, confirmation, crop display, and inventory persistence.

## Review interaction

The frontend uploads/captures a photo, starts standard or container-targeted
analysis, polls queued/running state, and exposes cancellation, warnings and
safe failures. Completed analysis displays drafts with source photo,
confidence, category, quantity and container suggestions.

Users edit draft identities and quantities, accept/reject detections, and
select or remove assignments. A container-targeted upload preselects that
container only as a review default. Pending/rejected drafts remain outside
item routes, search, summaries and container counts.

Overlay labeled normalized boxes on the rendered source image. Provide
pointer move/resize and keyboard controls, plus box creation/removal.
Rendering must preserve alignment for portrait and mobile layouts. A missing
AI box does not prevent reviewing an item. Original predictions are immutable;
user changes apply to reviewed fields and geometry.

## Confirmation transaction

Submit the complete review decision with the expected analysis version.
Validate all decisions, quantities, selected containers and reviewed geometry
before promotion. Rejected drafts create no items; accepted drafts create
canonical inventory and any explicit assignments atomically.

Reviewed boxes, not unreviewed AI proposals, determine private crops. Crop
storage failure prevents promotion; SQL failure removes newly written crops.
Record accepted/rejected decisions and resulting item linkage together.
Confirmation is idempotent and cannot partially promote a review.

After confirmation, refresh canonical inventory and load authorized crop
thumbnails through the API. Stale versions, repeated submissions, invalid
geometry and dependency failures produce explicit UI errors rather than
silently accepting an incomplete review.

## Acceptance criteria

The full local photo-to-review-to-inventory workflow passes with the deterministic mock AI, tests prove that no AI detection or crop enters canonical inventory before explicit confirmation, and confirmed crop lifecycle tests pass against Azurite.
