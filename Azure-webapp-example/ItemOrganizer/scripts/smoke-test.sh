#!/usr/bin/env bash

set -euo pipefail

required_tools=(
  "dotnet"
  "node"
  "pnpm"
  "az"
  "bicep"
)

for tool in "${required_tools[@]}"; do
  if ! command -v "${tool}" >/dev/null 2>&1; then
    echo "Missing required tool: ${tool}" >&2
    exit 1
  fi
done

for host_port in "sqlserver:1433" "azurite:10000" "azurite:10001" "azurite:10002"; do
  host="${host_port%%:*}"
  port="${host_port##*:}"

  if ! timeout 5 bash -lc "cat < /dev/null > /dev/tcp/${host}/${port}" 2>/dev/null; then
    echo "Unable to reach ${host} on port ${port}" >&2
    exit 1
  fi
done

if [[ "${ITEMORGANIZER_SQL_CONNECTION:-}" != *"sqlserver"* ]]; then
  echo "ITEMORGANIZER_SQL_CONNECTION must use the sqlserver service name." >&2
  exit 1
fi

if [[ "${ITEMORGANIZER_STORAGE_CONNECTION:-}" != *"azurite"* ]]; then
  echo "ITEMORGANIZER_STORAGE_CONNECTION must use the azurite service name." >&2
  exit 1
fi

if [[ "${ITEMORGANIZER_SQL_CONNECTION:-}" == *"127.0.0.1"* ]] || [[ "${ITEMORGANIZER_STORAGE_CONNECTION:-}" == *"127.0.0.1"* ]]; then
  echo "Container-side dependencies must not use 127.0.0.1." >&2
  exit 1
fi

echo "Workspace toolchain and container-network smoke test passed."
