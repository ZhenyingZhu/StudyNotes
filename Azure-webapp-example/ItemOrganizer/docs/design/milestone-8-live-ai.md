# Milestone 8 - Live AI provider

[High-level plan](../../PLAN.md) | [Product model](milestone-0-decisions.md) | [Analysis pipeline](milestone-6-analysis-pipeline.md)

## Scope

Provide live vision-based item identification through Azure OpenAI while
preserving the asynchronous review workflow. The provider returns candidate
detections, not canonical inventory or confirmed assignments.

A measured recognition/localization feasibility benchmark is outside this
milestone's required release gate. No labeled benchmark dataset, numeric
recognition thresholds, dedicated object-detector comparison or stakeholder
go/no-go benchmark approval is required. This does not remove provider safety,
privacy, explicit review or other milestones' acceptance criteria. Embedding
search has its own evaluation gate in Milestone 8A.

## Provider selection and inputs

`Analysis:Provider` selects `deterministic` or `azure-openai`. Local/default
tests select deterministic behavior; live mode is explicit. `Analysis:Model`
identifies the Azure deployment, independently of the model family.

The worker downloads the private source photo and sends image input to the
Azure OpenAI Responses API. Use the configured vision deployment and strict,
application-owned structured-output schema. Authentication uses managed
identity through `DefaultAzureCredential`, or an explicitly supplied
development key from an approved secret source. No provider key is committed.

## Output contract

Require schema version, photo ID, prompt version, model, items and warnings.
Each item requires name, quantity and confidence; nullable description,
category, container suggestion/reason and bounding box properties are present
with `null` when unavailable.

Validate required fields and version/photo metadata before draft persistence.
Store application build, schema, prompt and model versions on the analysis.
Reprocessing creates a new analysis; original predictions remain immutable.

Invalid optional box geometry is removed with a review warning so otherwise
valid detections remain reviewable without crops. Nonnumeric geometry and
invalid required fields fail validation. No invalid box enters persisted
drafts. User-reviewed geometry alone controls crop generation.

The shared pipeline applies confidence thresholds, owner-visible suggestions,
duplicate handling and atomic draft persistence. No provider output bypasses
explicit confirmation.

## Failures and privacy

Transient timeouts/throttling/provider failures map to
`AI_PROVIDER_TEMPORARY_FAILURE`; non-retryable request rejection maps to
`AI_PROVIDER_REJECTED_REQUEST`; malformed structured output maps to
`AI_OUTPUT_INVALID`. The queue worker applies bounded retries only to
temporary failures.

Client errors expose safe codes, actionable messages and correlation IDs.
Raw provider payloads, photo content, credentials and internal exceptions
are not persisted in ordinary logs. Live startup verifies authentication
and deployment readiness and warns about image transfer and Azure charges.

## Acceptance criteria

Test valid/invalid structured output, nullable/invalid geometry, private-photo
access, version tracking, safe failure classes, cancellation and absence of
partial drafts or inventory. Default local tests make no live Azure calls.
Explicit live tests validate the configured deployment and authentication
without implying measured recognition quality.
