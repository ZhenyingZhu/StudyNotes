# Milestone 4 - Container, item, and assignment workflows

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Deliver the manually managed inventory workflow before introducing AI processing.

## Requirements

- Authorized clients can create, list, update, and delete containers according to validation and empty-container rules.
- Authorized clients can create, list, update, and delete manually managed items.
- Items can be assigned, moved, or unassigned atomically through the documented endpoints.
- AI suggestions remain distinguishable from confirmed assignments and are not silently promoted.
- Invalid fields, missing resources, duplicate requests, stale `ETag`s, and non-empty-container deletion return the documented status and Problem Details responses.
- Frontend tests cover container creation, inventory search, item editing, assignment, and conflict messages.

## Mutation workflow

Container and item mutations require delegated write scope and owner-visible
resources. Validate fields and expected ETags before persistence. Empty
containers may be deleted; deleting a non-empty container returns conflict
until its items are moved or unassigned.

Manual creation writes a canonical item without photo/analysis metadata.
Edits correct its name, description, category and quantity without changing
ownership. Item deletion removes it from inventory and schedules private
crop cleanup when applicable; it does not delete a source photo.

Assignment is a single atomic replacement, not remove-then-add. Explicit
selection produces a confirmed assignment. Accepting an AI suggestion is
allowed only for the same suggested container. Unassignment clears both
container and suggestion and returns the item to the unassigned state.

## Frontend interaction

The inventory UI supports container/item creation, editing and deletion,
container-scoped browsing, keyword search and assignment changes. It sends
ETags on mutation and presents validation, missing-resource and stale-version
errors explicitly. A conflict must not silently discard user edits.

Suggestions are displayed separately from confirmed locations; UI navigation
and counts use canonical inventory. Keyboard operation, focus behavior and
accessible status/error messages cover the same operations as pointer input.

## Acceptance criteria

Backend unit, API contract, and frontend component tests pass for the complete non-AI workflow.
