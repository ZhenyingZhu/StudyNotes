#!/usr/bin/env bash

set -euo pipefail

run_directory="/tmp/itemorganizer"
mkdir -p "${run_directory}"

wait_for_url() {
  local name="$1"
  local url="$2"
  local pid="$3"
  local log_path="$4"

  for _ in $(seq 1 60); do
    if curl --fail --silent --show-error "${url}" >/dev/null 2>&1; then
      echo "${name} is ready."
      return
    fi

    if ! kill -0 "${pid}" 2>/dev/null; then
      echo "${name} stopped before becoming ready:" >&2
      tail -n 50 "${log_path}" >&2 || true
      exit 1
    fi

    sleep 1
  done

  echo "${name} did not become ready within 60 seconds:" >&2
  tail -n 50 "${log_path}" >&2 || true
  exit 1
}

start_api() {
  local readiness_url="http://localhost:5000/api/v1/health/ready"
  local log_path="${run_directory}/api.log"
  local pid_path="${run_directory}/api.pid"

  if curl --fail --silent "${readiness_url}" >/dev/null 2>&1; then
    echo "API is already running."
    return
  fi

  nohup env \
    "ASPNETCORE_URLS=${ASPNETCORE_URLS:-http://0.0.0.0:5000}" \
    "Authentication__AllowedTenantId=${Authentication__AllowedTenantId:-10000000-0000-0000-0000-000000000001}" \
    "DevelopmentAuthentication__Enabled=${DevelopmentAuthentication__Enabled:-true}" \
    "DevelopmentAuthentication__TenantId=${DevelopmentAuthentication__TenantId:-10000000-0000-0000-0000-000000000001}" \
    "DevelopmentAuthentication__OwnerObjectId=${DevelopmentAuthentication__OwnerObjectId:-20000000-0000-0000-0000-000000000001}" \
    dotnet run \
      --project src/ItemOrganizer.Api \
      --no-restore \
      >"${log_path}" 2>&1 </dev/null &

  local pid=$!
  printf '%s\n' "${pid}" >"${pid_path}"
  wait_for_url "API" "${readiness_url}" "${pid}" "${log_path}"
}

start_frontend() {
  local readiness_url="http://localhost:5173/"
  local log_path="${run_directory}/frontend.log"
  local pid_path="${run_directory}/frontend.pid"

  if curl --fail --silent "${readiness_url}" >/dev/null 2>&1; then
    echo "Frontend is already running."
    return
  fi

  nohup pnpm -C web dev >"${log_path}" 2>&1 </dev/null &

  local pid=$!
  printf '%s\n' "${pid}" >"${pid_path}"
  wait_for_url "Frontend" "${readiness_url}" "${pid}" "${log_path}"
}

cd /workspace
start_api
start_frontend
