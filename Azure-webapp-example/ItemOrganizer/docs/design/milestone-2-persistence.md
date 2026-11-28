# Milestone 2 - Domain model and persistence foundation

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Implement the approved inventory, photo, analysis-review, assignment, and idempotency persistence model.

## Requirements

- EF Core migrations create the approved schema in a clean local PostgreSQL database.
- Constraints and indexes enforce identifier, ownership, assignment, status, uniqueness, and timestamp rules.
- Valid and invalid state transitions are covered by unit tests, including analysis cancellation, completion, failure, retry, and deletion conflicts.
- Transaction tests demonstrate atomic review-draft persistence at analysis completion and atomic inventory creation only after explicit confirmation.
- Concurrent update tests demonstrate stale `ETag` protection and prevent lost changes.
- A reset-and-seed procedure produces representative data deterministically.

## Relational model

PostgreSQL stores metadata through EF Core/Npgsql. Every user-owned record
has a server-generated UUID, tenant ID, owner object ID and UTC timestamps.
Owner-aware keys and foreign keys prevent relationships across ownership
boundaries; indexes support owner-filtered collections and operation lookup.

| Record | State and relationships |
|---|---|
| Container | Name, description, physical location and searchable labels |
| Photo | Private blob reference, hash, decoded dimensions and deletion/retention state |
| Analysis | Photo/owner, operation state, cancellation, safe errors, retries and provider versions |
| AnalysisDetection | Immutable prediction, editable review decision/geometry and accepted item linkage |
| Item | Canonical manual or accepted inventory; optional photo/analysis/crop lineage |
| ItemAssignment | Current container, suggestion, source and assignment state |
| OutboxMessage | Durable dispatch work committed with the operation that produces it |
| Idempotency record | Owner/action/key, request identity and replayable result reference |

Manual items have no fabricated photo, analysis or confidence. Pending and
rejected detections are not items. Only accepted detections link to promoted
inventory. Original and reviewed boxes are separate; crop metadata records
source lineage, reviewed geometry, dimensions, content type, length and hash.

## Transactions and concurrency

Commit analysis creation and its outbox message together. Commit completed
review drafts atomically; validation/cancellation failure writes no partial
drafts. Confirmation creates accepted items and assignments and records all
review decisions in one transaction. Unique constraints and persisted state
make replay and duplicate queue delivery safe.

Use optimistic concurrency tokens exposed as ETags. Mutation requires the
expected version; stale updates fail rather than overwrite another change.
Persist domain invariants as database constraints where possible, including
valid states, quantities, ownership, uniqueness and assignment relationships.

Crop writes occur outside SQL's atomic boundary. Storage failure prevents
promotion; database failure removes newly written crops. Persist cleanup work
so deletion and retention are retryable. The reset-and-seed operation is
explicit and separate from migration-only application startup.

## Acceptance criteria

Persistence unit and integration tests pass against the local PostgreSQL service, including rollback and concurrency cases.
