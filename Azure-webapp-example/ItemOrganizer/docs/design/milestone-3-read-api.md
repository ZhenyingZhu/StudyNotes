# Milestone 3 - Read-only API and authorization

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Expose secure read operations and system health endpoints using the approved API conventions.

## Requirements

- Health, summary, container, photo, analysis, and item read routes use the `/api/v1` base path and documented response shapes.
- OpenAPI 3.1 describes routes, schemas, security requirements, paging, `ETag`, and RFC 9457 Problem Details responses.
- Requests without a token receive `401`; requests without the required read scope receive `403`.
- A caller cannot read another tenant's or owner's resources under the approved authorization model.
- Collection paging, filtering, UTC timestamps, correlation IDs, and cache/concurrency headers behave as documented.
- Readiness reports dependency failure without exposing secrets or internal exception details.

## Request processing

Route all resources under `/api/v1`. Validate the bearer token, approved tenant
and delegated read scope before querying user-owned state. Apply tenant and
owner predicates to every resource query and collection; a foreign-owned
resource is not exposed through another user's endpoint.

Project response representations rather than exposing internal persistence
entities. Summary and container counts include canonical inventory only,
excluding pending/rejected detections. Collection filters and continuation
tokens follow the [API contract](api-contract.md); ordinary inventory search
matches names/descriptions rather than generating answers.

Return server UTC timestamps, ETags and correlation IDs consistently.
Authorization failures use `401`/`403`; missing or invisible resources use
the contract's not-found behavior. Errors use RFC 9457 Problem Details and
safe messages, not internal exception/provider details.

## Health and API description

Liveness describes the API process. Readiness checks required dependencies
and reports unavailable services without exposing connection strings or
credentials. Neither check resets data or creates application work.

OpenAPI 3.1 describes schemas, delegated security requirements, pagination,
concurrency headers and error responses. Development Swagger UI consumes
that contract; production exposure follows the release configuration.

## Acceptance criteria

Contract tests pass for successful reads, authorization failures, not-found behavior, paging, headers, and Problem Details.
