# Milestone status and revision records

[High-level plan](../../PLAN.md) | [Environment revision archive](environment-revisions.md) | [Validation history](validation-history.md)

These records describe decisions and behavior at their recorded dates. The roadmap reports current status; design documents specify target behavior. Historical completion does not validate later changes.

## Milestone 0 - Product decision revisions

These decisions resolve the original open guidance so implementation can
proceed without revisiting core product and security questions. They include
the October 6 review revisions: selecting a container during upload does not
confirm unseen detections, and analysis completion creates review drafts,
not canonical inventory. Milestone 8's remaining measured live AI acceptance
requirements and go/no-go gate were waived on October 26, 2026; this does not
establish measured recognition quality or remove runtime safety requirements.

## Milestone 1 - Environment status

## Current revision status

The Windows/Ubuntu and native-source design was approved on November 27, 2026.
It replaces the earlier manual Ubuntu-terminal workflow, separately
provisioned WSL prerequisites, and universal host-only source adapter.

The existing host-only adapters and provisional Windows launchers do not
implement the complete design. The new Windows launcher test file has not
been run. Historical 42/55 mocked shell regressions do not establish runtime
correctness. Application and launcher implementation remains paused.

Before implementing prerequisite provisioning, verify the supported native
WSL/Ubuntu installation inputs and their source configurability. A native
installer's default downloads are not authorization to bypass external
source configuration.

No supported Ubuntu Docker Engine environment has been validated for this
revision. The remaining Windows/native Ubuntu runtime gates are blocked,
not passed; do not reconfigure the current host to bypass them.

**Current validation status (October 3, 2026):**

- **Prerequisite revision (November 4, 2026):** Docker Engine with `dockerd`
  replaces Docker Desktop in the planned setup. The historical results below
  are retained, but do not validate this revised prerequisite workflow.
  Clean-checkout bootstrap, launcher compatibility, port access, and the
  environment smoke test must be revalidated with Docker Engine in WSL 2.

- **Confirmed revision (November 27, 2026):** The approved environment and
  historical download-source revision below also requires native Ubuntu support, external
  file-only source selection and credentials, and the unified Linux startup
  lifecycle. Implementation and 42 mocked script regressions are complete;
  both-platform end-to-end validation remains blocked as documented below.
  The historical completion below is not validation of this revision.

- The Development Container image builds successfully on Windows ARM64.
- The pinned .NET SDK, EF Core CLI, Git, Node.js, pnpm, Azure CLI, and Bicep versions were verified inside the workspace image.
- PostgreSQL starts healthy and is reachable through its Compose service name.
- Azurite starts healthy and is reachable through its Compose service name on the Blob, Queue, and Table ports.
- The complete environment smoke test passes on Windows ARM64.
- On October 3, 2026, a second Windows AMD64 machine removed ignored files,
  containers, PostgreSQL and Azurite data, and package-cache volumes before
  running the documented bootstrap from the committed clean worktree.
- The second machine rebuilt the Development Container, verified the pinned
  tools, started healthy PostgreSQL and Azurite services, reached both by
  Compose service name, passed the smoke test, restored dependencies, applied
  migrations, and started the API and frontend successfully.
- Host requests returned HTTP 200 from the frontend, API readiness endpoint,
  and authenticated API summary endpoint. Milestone 1 therefore satisfies its
  exit criteria and is complete.

## Milestone 2 - Implementation records

**Current implementation status (September 18, 2026):**

- The current domain model covers containers, photos, analyses, items, assignments, outbox messages, and idempotency records.
- The initial EF Core migration defines PostgreSQL ownership keys, foreign keys, checks, uniqueness rules, indexes, and optimistic concurrency tokens.
- Domain tests cover analysis cancellation, completion conflicts, retries, assignment transitions, and deletion conflicts.
- PostgreSQL integration tests cover migration creation, ownership enforcement, idempotency uniqueness, atomic result persistence, rollback, and stale-version rejection.
- The database command and PowerShell wrapper provide deterministic reset, migration, and representative seed data.
- **Plan revision (October 6, 2026):** analysis detection drafts are now
  persisted separately, existing unreviewed AI items are migrated back to
  pending review, and confirmation transaction tests pass. This milestone
  remains reopened for reviewed bounding-box and private crop metadata,
  constraints, lineage, and cleanup persistence.

## Milestone 4 - Implementation records

**Current implementation status (September 25, 2026):**

- Container, manual-item, and assignment mutation routes enforce ownership,
  delegated write scope, optimistic concurrency, validation, and conflict
  responses.
- Manual items now have explicit persistence support without fabricated photo,
  analysis, or confidence values.
- The React and TypeScript frontend covers container and item creation,
  editing, deletion, inventory search, assignment, unassignment, and conflict
  feedback.
- Domain and API contract tests pass, and PostgreSQL integration tests passed
  during the September 23 validation. Frontend component tests cover container
  creation, inventory search, item editing, assignment, container inventory
  views, conflict feedback, and stale-record feedback.
- This historical validation used environment-specific frontend registry
  settings. The November 27 revision replaces source-selection environment
  variables with externally supplied source files; it does not change the
  package-access restrictions of any development environment.
- Validation on September 27, 2026 passed 9 domain tests, 7 PostgreSQL/Azurite
  integration tests, 26 API tests, 6 frontend component tests, the frontend
  type-check and production build, and the environment smoke test.

## Milestone 5 - Implementation records

**Current implementation status (September 25, 2026):**

- Multipart photo uploads validate the declared and decoded JPEG, PNG, or WebP
  format, the 10 MiB limit, 512-8000 pixel dimensions, malformed content, and
  multi-frame images before persistence.
- Uploads use server-generated blob names, private Blob Storage, per-owner
  quotas, and optional owner-scoped `Idempotency-Key` replay without persisting
  partial metadata after storage failure.
- Authorized content requests return short-lived read-only SAS URLs. Photo
  deletion blocks active analyses and referenced items, records pending
  deletion before touching storage, and safely retries failed or repeated
  deletion attempts.
- A background retention worker processes expired and pending deletions at a
  configurable interval of no more than 15 minutes.
- API and security tests pass against an in-memory storage test double. A
  dedicated integration test verifies private upload, SAS download, and
  idempotent deletion against Azurite.
- Validation on September 27, 2026 passed the complete backend,
  PostgreSQL/Azurite, frontend component, type-check, build, and environment
  smoke-test gates.

## Milestone 6 - Implementation records

**Current implementation status (September 28, 2026):**

- Analysis creation requires `ItemOrganizer.Analyze`, supports owner-scoped
  idempotency, and atomically persists the queued analysis and SQL outbox
  record before returning `202 Accepted`, `Location`, and `ETag`.
- A hosted dispatcher forwards analysis IDs to Azure Storage Queue. The worker
  processes deterministic structured results, validates schema and version
  metadata, persists items atomically, and handles cancellation before commit.
- Temporary failures use exponential retry through the sixth delivery attempt;
  permanent or exhausted failures use safe error details and dead-letter
  handling. Duplicate delivery does not recreate completed items.
- End-to-end validation on September 29, 2026 passed against the Docker Compose
  PostgreSQL and Azurite services. A real HTTP photo upload created a queued
  analysis and SQL outbox entry; the hosted dispatcher delivered it through
  Azure Storage Queue; the worker completed one delivery and atomically
  persisted two detected items with `suggested` and `unassigned` states.
- The analysis polling resource reached `completed`, PostgreSQL recorded the
  outbox as `dispatched`, the main queue drained, no dead-letter message was
  created, and all 8 PostgreSQL/Azurite integration tests passed. Milestone 6
  therefore satisfies its exit criteria and is complete.
- **Plan revision (October 6, 2026):** the pipeline now persists review drafts
  without creating inventory. This milestone remains reopened until the
  structured-output contract, deterministic fixtures, and validation cover
  optional normalized bounding boxes.

## Milestone 7 - Review validation

**Status: Complete, including the reopened scope; exit criteria passed on
October 7, 2026.**

**Implementation and validation history (October 1-7, 2026):**

- The web application uploads or captures JPEG, PNG, and WebP files, previews
  the selected source photo, starts standard or container-targeted analyses,
  polls queued and running work, displays failures and warnings, and permits
  cancellation.
- Completed analyses display detected names, categories, quantities,
  confidence, assignment status, and container suggestions. Users can accept
  or reject a suggestion or explicitly select a different container.
- `POST /api/v1/containers/{containerId}/photo-analyses` reuses the secure
  upload pipeline, validates owner-visible containers before storage, supports
  idempotent replay, creates the queued analysis and outbox record, and marks
  all completed detections as confirmed convenience-workflow assignments.
- API tests cover successful creation, invalid containers, duplicate
  submission, storage failure without analysis creation, and atomic confirmed
  result persistence. Frontend component tests cover standard upload,
  convenience upload, cancellation, and explicit suggestion acceptance and
  rejection.
- Live validation against PostgreSQL and Azurite completed a real convenience
  upload through the SQL outbox and queue worker, then returned two items
  confirmed in the selected container.
- Playwright Chromium validation resets the deterministic database, starts the
  development-authenticated API and Vite frontend, generates and uploads a
  valid 512-pixel PNG, polls through queue-worker completion, accepts an AI
  suggestion, explicitly assigns an unassigned item, refreshes the inventory,
  and verifies the container-targeted convenience workflow confirms every
  detected item.
- On October 1, 2026, both Playwright workflows, all 41 API tests, all 8
  PostgreSQL/Azurite integration tests, all 9 domain tests, all 11 frontend
  component tests, and the frontend production build passed. Milestone 7
  therefore satisfies its exit criteria and is complete.
- **Plan revision (October 6, 2026):** pending detections are now editable and
  only accepted detections are promoted to inventory through explicit
  confirmation.
- **Plan revision (October 6, 2026):** editable bounding-box review and
  confirmed derivative-crop storage were added to the milestone.
- **Completion validation (October 7, 2026):** the review UI overlays
  predicted boxes and permits numeric move, resize, creation, and removal;
  confirmation persists reviewed geometry and creates private PNG crops only
  for accepted detections. Crop upload or database failure creates no
  inventory, item deletion schedules crop cleanup, authorized inventory
  thumbnails load through the API, and manual items remain photo-free.
  All 10 domain tests, 48 API tests, 8 PostgreSQL/Azurite integration tests,
  13 frontend component tests, the frontend production build, and both
  Playwright Chromium workflows passed in the Docker Development Container.
  Milestone 7 therefore satisfies its reopened exit criteria and is complete.

## Milestone 8 - Feasibility waiver and live provider records

**Status (October 26, 2026):** The remaining feasibility work is **not needed**
by user direction. The gate is waived, not passed or validated, and no longer
blocks Milestone 8A, Milestone 9, or production-infrastructure work.

**Exit criteria:** No additional feasibility exit gate is required. Record this
milestone as waived rather than complete through measured validation.

**Current implementation status (October 1, 2026):**

- The API includes a configurable Azure OpenAI Responses provider using strict
  structured output, private blob download, managed identity or an explicitly
  supplied development API key, and safe transient, rejected, and malformed
  output classifications.
- A development `gpt-5.4-mini` deployment was provisioned with GlobalStandard
  capacity and passed an Entra-authenticated Responses API test using real
  image input and strict JSON-schema output.
- The default remains the deterministic provider so ordinary local and CI
  suites do not consume Azure resources.
- The representative dataset, acceptance thresholds, live measurements, and
  stakeholder go/no-go decision were not completed. These remaining requirements
  were subsequently marked not needed on October 26, 2026.

## Milestone 8A - Planning record

**Status:** Planned on October 25, 2026; no embedding deployment, vector
extension, indexing pipeline, or semantic search endpoint is implemented.
