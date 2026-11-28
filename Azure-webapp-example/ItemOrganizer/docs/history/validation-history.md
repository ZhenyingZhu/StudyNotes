# Recorded validation history

[High-level plan](../../PLAN.md) | [Milestone records](milestone-status.md) | [Environment revision archive](environment-revisions.md)

These are dated historical records, not evidence that the newly approved environment design has passed.

## Recorded validation history

These records describe the behavior at the time of each validation, not the
current review contract. The October 6 review revisions and October 7
completion validation are recorded in [Milestone records](milestone-status.md). Environment validation
on Windows ARM64 and the October 3 clean-checkout Windows AMD64 validation are
recorded in [Milestone records](milestone-status.md) and the [environment revision archive](environment-revisions.md).

**September 16, 2026:**

- The Development Container image built on Windows ARM64, pinned workspace
  tool versions were available, and PostgreSQL and Azurite became healthy and
  were reachable by service name.
- The complete environment smoke test passed on Windows ARM64.
- Compose configuration and local-secret ignore rules were valid.

**September 27, 2026:**

- The Development Container smoke test, 9 domain tests, 7 PostgreSQL/Azurite
  integration tests, 26 API tests, 6 frontend component tests, and the frontend
  type-check and production build passed.

**September 29, 2026:**

- 9 domain tests, 41 API/analysis-pipeline tests, and 11 frontend component
  tests passed.
- All 8 PostgreSQL/Azurite integration tests passed, including private Blob
  Storage and Azure Queue delivery/dead-letter behavior.
- A real HTTP upload started an analysis through the SQL outbox and Azurite
  queue, reached `completed`, and persisted two detected items atomically.
- A real convenience upload reached `completed` and persisted two confirmed
  assignments in the explicitly selected container.
- The complete .NET solution tests and frontend type-check and production
  build passed.

**October 1, 2026:**

- All 9 domain, 8 PostgreSQL/Azurite integration, 41 API/pipeline, and 11
  frontend component tests passed.
- The frontend type-check and production build passed.
- Both Playwright Chromium workflows passed against the real local frontend,
  API, PostgreSQL database, private Azurite Blob Storage, SQL outbox, Azurite
  Queue, and deterministic analysis worker.
- The standard workflow uploaded a generated valid PNG, reached completed
  analysis, accepted the suggested container for one item, explicitly assigned
  the unassigned item, refreshed, and retained both confirmed assignments.
- The convenience workflow completed with both detected items confirmed in the
  explicitly selected container.

**October 17, 2026 startup diagnostics validation:**

- The live AI launcher now retains failed command output, identifies the
  failed prerequisite, and provides recovery instructions. Prerequisite
  failures and `-ValidateOnly` no longer stop existing development servers.
- Normal startup now runs device-code Azure login in the Docker workspace
  when its session check fails, streams the sign-in instructions, and
  rechecks the session before continuing. Optional `-TenantId` targets a
  specific tenant. `-ValidateOnly` remains non-interactive.
- All 17 isolated launcher regression cases passed using
  `.\tests\Scripts\start-live-ai.Tests.ps1`, covering Docker, Compose,
  workspace Azure login, deployment readiness, frontend packages, backend
  restore, database migrations, successful prerequisite validation, automatic
  login continuation, tenant forwarding, login failure, non-interactive
  validation, and post-login session verification.
- A real `.\scripts\start-live-ai.ps1 -ValidateOnly` run reached the workspace
  Azure login check and reported the underlying `Please run 'az login'`
  error with the container device-login command. Actual browser sign-in,
  deployment access, and end-to-end live analysis were not validated; the
  user must complete browser authentication when prompted during startup.

**October 23, 2026 live bounding-box validation fix:**

- Reproduced an intermittent live-analysis failure caused by model-generated
  bounding boxes extending beyond the photo. The live provider now removes
  invalid optional geometry and records a review warning instead of discarding
  otherwise valid detections. Required-field validation remains unchanged.
- All 20 targeted Azure OpenAI provider and analysis-pipeline tests passed
  inside the Development Container, including valid edge-aligned boxes,
  out-of-bounds and nonpositive geometry, null boxes, and malformed geometry.
- Restarted the live API and reanalyzed the same uploaded photo through the
  real HTTP API, SQL outbox, Azurite queue, and Azure OpenAI deployment. The
  analysis completed with nine review detections; the API and frontend were
  responsive. This does not establish the Milestone 8 recognition thresholds.

**October 24, 2026 graphical crop review:**

- Replaced numeric crop-coordinate fields with a source-photo editor for each
  detected item. Labeled boxes can be dragged to move them and resized with a
  corner handle; keyboard arrow controls, crop creation, and removal are
  available without entering coordinates.
- Movement and resizing keep geometry inside the photo. Rejected and confirmed
  detections cannot edit crop geometry. Photo canvases match the rendered
  image dimensions, including portrait images limited by preview height.
- All 17 frontend component tests, the frontend production build, and all
  three Playwright Chromium workflows passed in the Development Container.
  Browser coverage includes dragging, resizing, submitted review geometry,
  confirmed private-crop display, mobile portrait alignment, and adding and
  removing boxes. Missing Chromium and browser system dependencies were
  installed only inside the existing workspace container.
