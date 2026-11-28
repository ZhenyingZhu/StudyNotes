# Milestone 8A - Hybrid semantic inventory search

[High-level plan](../../PLAN.md) | [API contract](api-contract.md)

## Scope

Find confirmed inventory by meaning as well as literal wording.
For example, "something to light up a tent" should retrieve a camping lantern
even when that exact phrase is absent from its name and description. Search
retrieves real inventory records; it does not generate answers or invent items.

## Dependencies and scope
 Complete the search gate before Milestone 9 and include its infrastructure
 and release requirements in Milestone 10. Photo-recognition benchmarking
 is not a dependency. Initial scope is
text-to-item search, not image similarity, conversational search, or automatic
container assignment.

## Retrieval and indexing design

- Use hybrid retrieval: keep literal matching for exact names, identifiers,
  and partial words; add PostgreSQL full-text retrieval and vector similarity
  for natural-language queries. Fuse ranked candidate lists using reciprocal
  rank fusion rather than combining incompatible raw scores. Tune candidate
  limits, relevance cutoffs, and lexical priority on the evaluation set so
  semantic neighbors do not displace clear exact matches or force irrelevant
  results for queries with no suitable item.
- Store text embeddings alongside relational metadata in the existing
  PostgreSQL database using `pgvector`, avoiding a separate search service
  initially. Adoption requires verifying extension availability,
  supported versions, EF Core/Npgsql integration, migration permissions, and
  local/Azure parity. Change development database images or
  tooling only inside the existing Development Container and Docker services.
- Use an approved Azure OpenAI text-embedding deployment, separate from the
  vision model. Select the model, dimensions, token limits, region, authentication,
  and cost budget through a small live feasibility evaluation; do not assume
  the current vision deployment can produce embeddings. Use the same model,
  dimensions, and normalization for indexed text and queries. Keep the default
  local/CI provider deterministic, with live Azure evaluation explicitly invoked.
- Build canonical embedding text from the item's current reviewed name,
  description, and category. Exclude raw photos, rejected/pending detections,
  and unreviewed predictions. Keep container/location metadata out of the
  initial embedding document so a move does not require re-embedding; return
  current container/location information for navigation, not semantic matching.
- Add an item-search document keyed by item ID, tenant, and owner, recording
  source revision/hash, embedding model and document-format versions, vector,
  indexing state, and timestamps. Index manually created items and accepted
  detections only. Persist item changes and durable indexing work atomically
  using the existing outbox/worker pattern; do not call the embedding provider
  inside the inventory write transaction.
- Make indexing idempotent, retry transient failures with bounded backoff, and
  expose terminal failures to operators. On updates, reject obsolete worker
  writes using the source revision and exclude stale vectors until refreshed.
  New or edited items remain immediately searchable through the lexical path.
  Backfill existing items in bounded, resumable batches; model/document changes
  require a versioned reindex and controlled cutover, never mixed vector spaces.
- Apply tenant, owner, soft-deletion, category, photo, assignment, and selected
  container constraints to every lexical/vector candidate query before ranking
  and pagination. Container scope includes confirmed assignments only. Join
  against current inventory state so deletions and moves affect results
  immediately even if asynchronous index cleanup is pending. Delete derived
  search documents with their inventory records.
- Start with exact vector distance within the authorized filtered inventory.
  Add an approximate index only if measured scale requires it; verify filtered
  recall so a global candidate limit cannot hide another owner's inventory
  from their own results. Record PostgreSQL query plans, index sizes, and
  latency at representative inventory sizes.
- Cache query embeddings only with bounded lifetime and tenant/owner/model
  isolation. Enforce query length, page size, request timeout, concurrency,
  and rate limits. Do not log raw queries, embedding inputs, vectors, or
  provider payloads; treat embeddings as private derived inventory data.
- Keep relevance pagination stable using an expiring server-side result snapshot
  and opaque continuation token bound to the user, normalized query, filters,
  model/index version, and page position. Recheck authorization and current
  item visibility on each page. Reject mismatched or expired tokens explicitly;
  do not reuse the existing creation-order offset token for ranked results.
- Add a feature-gated "Smart search" UI mode while retaining keyword search.
  Submit on Search/Enter rather than each keystroke, preserve container scope,
  show relevance ordering and indexing/degraded-search status, and provide
  useful empty/error states. If embedding retrieval times out or fails, return
  lexical results only with an explicit warning and safe dependency telemetry;
  if lexical retrieval also fails, return a standard error, not an empty success.
- Add deterministic unit, API, frontend, and PostgreSQL integration coverage
  for fusion, query validation, filters, cross-owner/cross-tenant denial,
  pending/rejected draft exclusion, stale writes, retries, deletion, backfill,
  model cutover, dependency failure, and continuation-token behavior. Cover
  exact names, partial words, natural-language intent, synonyms, misspellings,
  ambiguity, and no-match queries in a versioned human-labeled relevance set.
  Report limitations rather than assuming embeddings handle every case.

## Acceptance criteria

Before tuning, approve numeric targets for Recall@10,
NDCG@10, exact-name top-result accuracy, no-match behavior, p95 search latency,
indexing freshness, and embedding cost per query/item. Compare the unchanged
keyword baseline with hybrid search on the same relevance set. Hybrid search
must improve semantic-query relevance without violating the approved exact-match
regression budget. All authorization, lifecycle, pagination, outage, and UI
tests must pass end-to-end in the Development Container; a separately invoked
live Azure/PostgreSQL evaluation must meet the approved quality, latency,
freshness, and cost targets. Record evidence before enabling smart search by
default; until then, retain keyword search as the default.
