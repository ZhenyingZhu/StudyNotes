#!/usr/bin/env bash

set -euo pipefail
source "$(dirname "$(realpath -- "${BASH_SOURCE[0]}")")/start-environment.sh"

reset_and_seed() (
  local confirmed=false docker_config="" project_root repository_root
  while (($#)); do
    case "$1" in
      --confirm) confirmed=true; shift ;;
      --docker-config)
        [[ $# -ge 2 ]] || { fail "--docker-config requires an external directory."; exit 1; }
        docker_config="$2"; shift 2 ;;
      --help)
        echo "Usage: bash scripts/reset-and-seed.sh --confirm [--docker-config /external/docker-client]"
        echo "Destructively resets the application database. Prepare the environment with startup first."
        exit 0 ;;
      *) fail "Unknown reset option $1."; exit 1 ;;
    esac
  done
  [[ "$confirmed" == true ]] || { fail "Database reset requires explicit --confirm."; exit 1; }
  require_ubuntu
  project_root="$(dirname "$(dirname "$(realpath -- "${BASH_SOURCE[0]}")")")"
  repository_root="$(repository_root_for "$project_root")"
  if [[ -n "$docker_config" ]]; then
    docker_config="$(realpath -- "$docker_config")"
    protected_file "$docker_config/config.json" >/dev/null
    docker_command=(docker --config "$docker_config")
  fi
  check_engine
  cd "$project_root"
  run "Starting existing local services" "Run start-environment.sh first; reset does not pull or build images." \
    compose up -d --wait --no-build workspace
  run "Resetting and seeding the application database" "Inspect database connectivity and the reset error." \
    compose exec -T workspace dotnet run --project src/ItemOrganizer.Database --no-restore -- reset-and-seed
)

reset_and_seed "$@"
