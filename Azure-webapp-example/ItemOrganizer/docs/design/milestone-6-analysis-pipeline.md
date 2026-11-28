# Milestone 6 - Deterministic asynchronous analysis pipeline

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Process analyses asynchronously with a deterministic mock AI implementation before enabling live Azure OpenAI calls.

## Requirements

- Starting an analysis returns `202 Accepted`, a `Location` header, and the documented queued resource.
- The worker transitions analyses only through approved states and records safe failure information, retry count, timestamps, and correlation data.
- Fixed structured-output fixtures produce deterministic review drafts, confidence values, suggestions, source links, and optional normalized bounding boxes.
- Bounding-box validation rejects non-finite values, non-positive areas, values outside `0` to `1`, and boxes whose right or bottom edge exceeds the image boundary.
- Invalid, incomplete, unsafe, or schema-incompatible AI output is rejected without creating partial inventory data.
- Retries are bounded and idempotent; duplicate delivery does not duplicate review drafts.
- Cancellation works for queued and eligible running work, and cancellation races have deterministic results.
- Polling returns the documented state and result shape, including failure and cancellation responses.

## Dispatch and state transitions

```text
Analysis request -> SQL analysis + outbox transaction -> queue dispatcher
    -> Storage Queue -> worker -> validated review drafts -> completed
```

Creation requires analyze scope and supports owner-scoped idempotency.
Return `202 Accepted` with the analysis location and queued representation.
Queue messages carry only the analysis ID; PostgreSQL owns operation state.
The worker claims eligible work before provider invocation and checks state
on every delivery so completed work cannot create drafts twice.

States are queued, running, completed, failed and cancelled. Cancelling queued
work stores cancelled immediately. Running cancellation records a request;
the worker checks it before committing results, including after provider
success. Terminal analyses reject cancellation with conflict.

## Fixture and result processing

The deterministic provider consumes fixed structured-output fixtures and makes
no Azure calls. Validate schema/photo/prompt/model metadata, required fields,
confidence, quantity and optional normalized geometry before committing any
drafts. Invalid unsanitized output fails the operation without partial drafts.

Discard confidence below 0.35 with warnings. Below 0.80, retain valid drafts
without an AI container suggestion. Higher-confidence suggestions must refer
to an owner-visible container. Merge exact normalized duplicate detections
within one analysis according to the product model; do not merge them into
existing inventory automatically.

Bounding boxes must be finite, positive-area and inside the source image.
The live provider's optional-geometry sanitization is specified separately
in [Milestone 8](milestone-8-live-ai.md). Analysis completion never creates
canonical items or crops; confirmation owns that transition.

## Retry and failure behavior

Temporary failures use exponential backoff with five retries after the initial
delivery. The sixth failed delivery stores a safe failure and uses dead-letter
handling. Permanent validation failures do not retry. Duplicate messages and
retries check persisted state and unique constraints before writing results.
Polling exposes operation state, drafts, warnings and safe failure details,
not raw provider payloads.

## Acceptance criteria

Worker, transaction, retry, cancellation, fixture, and API integration tests pass without network access to Azure OpenAI.
