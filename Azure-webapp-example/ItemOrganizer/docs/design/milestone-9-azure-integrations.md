# Milestone 9 - Entra ID, remaining live Azure integrations, and observability

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Integrate Entra authentication, managed Azure resource access and
privacy-preserving telemetry. Vision and embedding provider configuration
comes from the corresponding provider/search designs.

## Requirements

- Entra ID tokens with each approved scope produce the documented allow/deny behavior.
- The deployed API accesses Azure Database for PostgreSQL Flexible Server, private Blob Storage, and configured Azure OpenAI vision and embedding deployments through managed identity or the approved secret mechanism; no credentials are present in source or ordinary configuration.
- Live integration tests are explicitly invoked, isolated from the default local test run, and clean up their test data.
- Application Insights and Log Analytics capture correlation IDs, dependency failures, analysis failures, throttling, latency, and authorization failures without recording photo content or secrets.
- Readiness and liveness checks, alerts, retention settings, rate limits, and upload limits are verified in a non-production environment.

## Identity and dependency access

The frontend obtains delegated access tokens through its configured Entra
application. The API validates issuer/approved tenant and required read,
write and analyze scopes, then enforces tenant and owner resource boundaries.
Combined upload/analysis operations require both write and analyze scope.

Use managed identity for supported database, storage and Azure OpenAI access.
Grant resource-specific permissions rather than embedding keys in ordinary
configuration. Approved secret mechanisms handle any dependency that cannot
use managed identity; identifiers and non-secret deployment settings remain
environment-specific configuration.

## Operational signals

Liveness checks the API process; readiness checks required dependency access.
Application Insights/Log Analytics correlate requests and operations through
safe IDs and capture dependency failures, latency, authorization failures,
analysis failures and throttling. They do not capture raw photos, queries,
provider payloads, tokens or secrets.

Configure alerts and limits for HTTP failures, dependency outages, provider
throttling, failed analyses and request/upload load. Retention processing
remains idempotent and observable without preserving deleted content.

## Live test isolation

Invoke live tests explicitly against a dedicated non-production environment.
Use authorized users/resources, approved synthetic fixtures and bounded test
data cleanup. Verify allowed/denied scopes and owners, managed-resource access,
dependency failure reporting, telemetry privacy and alert behavior separately
from deterministic local tests.

## Acceptance criteria

The live integration and observability checklist passes in a dedicated Azure development environment with evidence retained for review.
