#!/usr/bin/env bash

set -euo pipefail

required_tools=(
  "dotnet"
  "git"
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

assert_version() {
  local name="$1"
  local actual="$2"
  local expected="$3"

  if [[ "${actual}" != "${expected}" ]]; then
    echo "${name} version mismatch: expected ${expected}, found ${actual}" >&2
    exit 1
  fi
}

assert_version ".NET SDK" "$(dotnet --version)" "${DOTNET_SDK_VERSION}"
assert_version "EF Core CLI" "$(dotnet ef --version | tail -n 1)" "${DOTNET_EF_VERSION}"
assert_version "Node.js" "$(node --version)" "v${NODE_VERSION}"
assert_version "pnpm" "$(pnpm --version)" "${PNPM_VERSION}"
assert_version "Azure CLI" "$(az version --output json | jq -r '.["azure-cli"]')" "${AZURE_CLI_VERSION}"
assert_version "Bicep CLI" "$(bicep --version | awk '{print $4}')" "${BICEP_VERSION#v}"

for host_port in "postgres:5432" "azurite:10000" "azurite:10001" "azurite:10002"; do
  host="${host_port%%:*}"
  port="${host_port##*:}"

  if ! timeout 5 bash -lc "cat < /dev/null > /dev/tcp/${host}/${port}" 2>/dev/null; then
    echo "Unable to reach ${host} on port ${port}" >&2
    exit 1
  fi
done

if [[ "${ITEMORGANIZER_DATABASE_CONNECTION:-}" != *"Host=postgres"* ]]; then
  echo "ITEMORGANIZER_DATABASE_CONNECTION must use the postgres service name." >&2
  exit 1
fi

if [[ "${ITEMORGANIZER_STORAGE_CONNECTION:-}" != *"azurite"* ]]; then
  echo "ITEMORGANIZER_STORAGE_CONNECTION must use the azurite service name." >&2
  exit 1
fi

if [[ "${ITEMORGANIZER_DATABASE_CONNECTION:-}" == *"127.0.0.1"* ]] || [[ "${ITEMORGANIZER_STORAGE_CONNECTION:-}" == *"127.0.0.1"* ]]; then
  echo "Container-side dependencies must not use 127.0.0.1." >&2
  exit 1
fi

echo "Workspace toolchain and container-network smoke test passed."
