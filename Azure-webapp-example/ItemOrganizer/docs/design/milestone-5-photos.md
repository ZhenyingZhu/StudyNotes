# Milestone 5 - Secure photo ingestion and storage

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Accept, validate, store, retrieve, and delete private photos safely.

## Requirements

- Valid multipart uploads create photo metadata and private blobs, returning `201 Created` and a `Location` header.
- Unsupported media, oversized files, invalid dimensions, malformed multipart requests, and upload quota violations return the approved errors without persisting partial records.
- Blob names and metadata do not expose tenant identifiers, credentials, or user-controlled path traversal.
- Authorized content retrieval uses a short-lived protected read mechanism; unauthorized callers cannot read the blob.
- Confirmed item crops use separate server-generated private blobs, retain source-photo lineage and reviewed coordinates, and cannot be created from an unreviewed AI box.
- Crop encoding, hash, dimensions, cleanup, storage-failure compensation, and authorization are covered by storage tests.
- Photo deletion is blocked while analysis is running and is idempotent where the contract requires it.
- Upload, download, deletion, retention, and storage-failure tests pass against Azurite, with explicitly documented emulator limitations.

## Upload pipeline

Authorize write access and validate any owner-visible container hint. Decode
the multipart file before persistence: accept JPEG, PNG or WebP, at most
10 MiB, with each dimension between 512 and 8000 pixels. Reject malformed,
animated or multi-frame content and enforce the configured per-owner quota.

Use server-generated blob names and private containers. Persist decoded
metadata/hash and owner-scoped idempotency information with the photo record.
A replay returns the original result; storage/database failure must not
leave a successful-looking partial photo resource.

## Content and derivative storage

Authorized content requests obtain short-lived, read-only access. Neither
resource representations nor normal logs contain permanent public blob URLs
or credentials. Crop blobs have separate server-generated identifiers and
retain source-photo and reviewed-coordinate lineage.

Only accepted review decisions with valid reviewed geometry create crops.
Decode the source, convert normalized coordinates to validated pixel bounds,
and encode a non-animated derivative. Store its dimensions, content type,
length and hash. Compensate storage writes if inventory promotion fails.

## Deletion and retention

Block photo deletion while analysis is active or inventory references the
photo. Record pending/soft deletion before asynchronous hard deletion; retry
cleanup idempotently and finish eligible user-requested deletion within
15 minutes. Item deletion schedules its crop cleanup independently.

Retain completed photos for 30 days by default and failed/cancelled photos
for 7 days, subject to active/reference restrictions. Cleanup keeps safe
audit metadata without retaining readable deleted content. Azurite verifies
local behavior; live Azure tests cover emulator limitations.

## Acceptance criteria

API integration and security tests pass for accepted and rejected uploads, authorization, cleanup, retry, and deletion behavior.
