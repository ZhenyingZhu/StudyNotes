#!/usr/bin/env bash

set -euo pipefail

project="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
temporary="$(mktemp -d)"
trap 'rm -rf -- "$temporary"' EXIT
mkdir -p "$temporary/project/scripts" "$temporary/project/web" "$temporary/project/.git" "$temporary/bin" "$temporary/external"
cp "$project/scripts/start-environment.sh" "$temporary/project/scripts/"
cp "$project/.env.example" "$temporary/project/.env"
cp "$project/web/.env.example" "$temporary/project/web/.env.local"
# Legacy startup fixture, independent of the current project source manifest.
cat > "$temporary/external/sources.conf" <<'SOURCES'
WORKSPACE_BASE_IMAGE=mcr.microsoft.com/devcontainers/javascript-node:1-22-bookworm@sha256:0d29e5fdc64f8397cd502223e0c4679f1e60877ca0fd2db4f2e2e0028e4271af
POSTGRES_IMAGE=docker.io/library/postgres:18.0-bookworm
AZURITE_IMAGE=mcr.microsoft.com/azure-storage/azurite:3.35.0
UBUNTU_APT_URI_AMD64=https://archive.ubuntu.com/ubuntu/
UBUNTU_SECURITY_URI_AMD64=https://security.ubuntu.com/ubuntu/
UBUNTU_APT_URI_ARM64=https://ports.ubuntu.com/ubuntu-ports/
UBUNTU_SECURITY_URI_ARM64=https://ports.ubuntu.com/ubuntu-ports/
DOCKER_APT_URI=https://download.docker.com/linux/ubuntu
DOCKER_APT_KEY_URL=https://download.docker.com/linux/ubuntu/gpg
DEBIAN_APT_URI=https://deb.debian.org/debian
DEBIAN_SECURITY_URI=https://deb.debian.org/debian-security
AZURE_CLI_APT_URI=https://packages.microsoft.com/repos/azure-cli/
AZURE_CLI_KEY_URL=https://packages.microsoft.com/keys/microsoft.asc
DOTNET_SDK_URL_AMD64=https://builds.dotnet.microsoft.com/dotnet/Sdk/10.0.100/dotnet-sdk-10.0.100-linux-x64.tar.gz
DOTNET_SDK_URL_ARM64=https://builds.dotnet.microsoft.com/dotnet/Sdk/10.0.100/dotnet-sdk-10.0.100-linux-arm64.tar.gz
DOTNET_EF_PACKAGE_URL=https://api.nuget.org/v3-flatcontainer/dotnet-ef/10.0.1/dotnet-ef.10.0.1.nupkg
BICEP_URL_AMD64=https://downloads.bicep.azure.com/v0.37.4/bicep-linux-x64
BICEP_URL_ARM64=https://downloads.bicep.azure.com/v0.37.4/bicep-linux-arm64
NUGET_SOURCE=https://api.nuget.org/v3/index.json
NPM_REGISTRY=https://registry.npmjs.org/
PLAYWRIGHT_CHROMIUM_URL_AMD64=https://cdn.playwright.dev/builds/cft/153.0.8010.12/linux64/chrome-linux64.zip
PLAYWRIGHT_CHROMIUM_URL_ARM64=https://cdn.playwright.dev/builds/cft/153.0.8010.12/linux-arm64/chrome-linux-arm64.zip
SOURCES
export TEST_CALLS="$temporary/calls" TEST_STATE="$temporary/state"
export PATH="$temporary/bin:$PATH"

cat > "$temporary/bin/docker" <<'MOCK'
#!/usr/bin/env bash
set -euo pipefail
printf '%s\n' "$*" >> "$TEST_CALLS"
if [[ "${1:-}" == --config ]]; then shift 2; fi
case "$*" in
  'info --format '*)
    case "${TEST_SCENARIO:-}" in
      engine) echo "Simulated engine diagnostic." >&2; exit 17 ;;
      desktop) echo 'Docker Desktop|linux' ;;
      windows) echo 'Windows|windows' ;;
      *) echo 'Ubuntu|linux' ;;
    esac
    exit 0 ;;
  'compose version') [[ "${TEST_SCENARIO:-}" != compose ]] || exit 17 ;;
  'buildx version') [[ "${TEST_SCENARIO:-}" != buildx ]] || exit 17 ;;
  'image inspect itemorganizer-workspace:local') [[ "${TEST_SCENARIO:-}" != missing-image ]] || exit 17 ;;
  'image inspect '*) exit 1 ;;
  'pull '*) [[ "${TEST_SCENARIO:-}" != pull ]] || { echo 'Simulated pull diagnostic.' >&2; exit 17; } ;;
  'build '*) [[ "${TEST_SCENARIO:-}" != build ]] || exit 17 ;;
  'compose config --quiet') [[ "${TEST_SCENARIO:-}" != config ]] || exit 17 ;;
  'compose up '*) [[ "${TEST_SCENARIO:-}" != up ]] || exit 17 ;;
  'compose exec -T workspace mktemp '*) echo '/tmp/itemorganizer-config.test'; exit 0 ;;
  *' az account show '*)
    if [[ "${TEST_SCENARIO:-}" == login-recheck ||
          ( ( "${TEST_SCENARIO:-}" == login || "${TEST_SCENARIO:-}" == login-failure ) && ! -f "$TEST_STATE" ) ||
          "${TEST_SCENARIO:-}" == no-session ]]; then exit 17; fi ;;
  *' az login --use-device-code '*)
    [[ "${TEST_SCENARIO:-}" != login-failure ]] || exit 17
    touch "$TEST_STATE" ;;
  *' az cognitiveservices '*)
    case "${TEST_SCENARIO:-}" in
      deployment) exit 17 ;; not-ready) echo Failed ;; *) echo Succeeded ;;
    esac
    exit 0 ;;
  *'--internal-restore '*)
    cat > /dev/null
    [[ "${TEST_SCENARIO:-}" != restore ]] || exit 17 ;;
  *'--internal-launch '*)
    cat > /dev/null
    if [[ "${TEST_SCENARIO:-}" == hold-launch ]]; then
      printf '%s\n' "$PPID" > "$TEST_STATE"
      while true; do sleep 0.1; done
    fi
    [[ "${TEST_SCENARIO:-}" != launch ]] || exit 17 ;;
  *'--internal-stop '*) cat > /dev/null ;;
  *'cat > '*) cat > /dev/null ;;
  *'-- migrate') [[ "${TEST_SCENARIO:-}" != migrate ]] || exit 17 ;;
esac
MOCK
chmod +x "$temporary/bin/docker"
if [[ "$(uname -s)" != Linux ]]; then
  cat > "$temporary/bin/stat" <<'MOCK'
#!/usr/bin/env bash
if [[ "$1" == -c && "$2" == %a ]]; then
  printf '%s\n' "${TEST_FILE_MODE:-600}"
else
  /usr/bin/stat "$@"
fi
MOCK
  chmod +x "$temporary/bin/stat"
  echo "NTFS mode checks are simulated; real Unix permission validation requires Ubuntu."
fi

passed=0
pass() { passed=$((passed + 1)); echo "PASS: $1"; }
assert_contains() { grep -Fq -- "$2" "$1" || { echo "Missing expected diagnostic: $2" >&2; cat "$1" >&2; exit 1; }; }
assert_absent() { if grep -Fq -- "$2" "$1"; then echo "Unexpected operation: $2" >&2; cat "$1" >&2; exit 1; fi; }

invoke() {
  : > "$TEST_CALLS"
  rm -f "$TEST_STATE"
  bash -c 'source "$1"; shift; require_ubuntu() { :; }; main "$@"' \
    bash "$temporary/project/scripts/start-environment.sh" "$@"
}

run_failure() {
  local scenario="$1" expected="$2"
  shift 2
  export TEST_SCENARIO="$scenario"
  if invoke "$@" > "$temporary/output" 2>&1; then
    echo "Unexpected success: $scenario" >&2
    exit 1
  fi
  assert_contains "$temporary/output" "$expected"
  pass "$scenario"
}

sources="$temporary/external/sources.conf"
run_failure no-source '--sources is required' --no-build
cp "$sources" "$temporary/project/inside.conf"
run_failure inside 'outside the Git repository' --sources "$temporary/project/inside.conf" --no-build
cp "$sources" "$temporary/external/duplicate.conf"
printf 'NPM_REGISTRY=unused\n' >> "$temporary/external/duplicate.conf"
run_failure duplicate 'duplicate configuration key' --sources "$temporary/external/duplicate.conf" --no-build
sed '/^NPM_REGISTRY=/d' "$sources" > "$temporary/external/missing.conf"
run_failure missing-source 'Missing source key NPM_REGISTRY' --sources "$temporary/external/missing.conf" --no-build
assert_absent "$TEST_CALLS" 'pull '
sed 's|^NPM_REGISTRY=.*|NPM_REGISTRY=https://user:password@invalid.example/|' "$sources" > "$temporary/external/unsafe.conf"
run_failure credentials-in-url 'without credentials' --sources "$temporary/external/unsafe.conf"
printf 'NPM_PASSWORD=synthetic-test-token\n' > "$temporary/external/credentials.conf"
chmod 644 "$temporary/external/credentials.conf"
export TEST_FILE_MODE=644
run_failure permissions 'chmod 600' --sources "$sources" --credentials "$temporary/external/credentials.conf"
chmod 600 "$temporary/external/credentials.conf"
export TEST_FILE_MODE=600
run_failure incomplete-credentials 'both username and password' --sources "$sources" --credentials "$temporary/external/credentials.conf"
run_failure invalid-mode 'mock or live' --sources "$sources" --mode invalid
run_failure conflicting-ca 'do not combine' --sources "$sources" --no-build --custom-ca "$sources"

for entry in \
  'engine|Cannot connect to Docker Engine' \
  'desktop|standalone Linux Docker Engine' \
  'windows|standalone Linux Docker Engine' \
  'compose|Checking Compose v2 failed' \
  'buildx|Checking Buildx failed' \
  'pull|Pulling configured POSTGRES_IMAGE failed' \
  'missing-image|Run startup without --no-build' \
  'config|Validating Compose configuration failed' \
  'up|Starting Docker services failed' \
  'restore|' \
  'migrate|Applying database migrations failed' \
  'launch|'; do
  scenario="${entry%%|*}"
  expected="${entry#*|}"
  run_failure "$scenario" "$expected" --sources "$sources" --no-build --detach
  if [[ "$scenario" == restore ]]; then
    assert_absent "$TEST_CALLS" '-- migrate'
    assert_contains "$TEST_CALLS" 'rmdir'
  fi
done
run_failure build 'Building the local workspace image failed' --sources "$sources" --detach
assert_absent "$TEST_CALLS" '--internal-restore'

live=(--sources "$sources" --no-build --mode live --endpoint https://resource.example.invalid/
  --resource-name resource --resource-group group --deployment-name deployment)
run_failure missing-live 'Live mode requires' --sources "$sources" --mode live
run_failure no-session 'validation does not sign in' "${live[@]}" --validate-only
assert_absent "$TEST_CALLS" ' az login '
assert_absent "$TEST_CALLS" '--internal-stop'
run_failure deployment 'Cannot read the live deployment' "${live[@]}" --validate-only
run_failure not-ready 'Live deployment is not ready' "${live[@]}" --validate-only
run_failure login-recheck 'Checking Azure login result failed' "${live[@]}" --detach
run_failure login-failure 'Container Azure device-code login failed' "${live[@]}" --detach

export TEST_SCENARIO=login
invoke "${live[@]}" --tenant-id selected-tenant --detach > "$temporary/output" 2>&1
assert_contains "$TEST_CALLS" '--tenant selected-tenant'
[[ "$(grep -c ' az account show ' "$TEST_CALLS")" == 2 ]]
assert_contains "$TEST_CALLS" '--internal-launch live true'
assert_absent "$TEST_CALLS" '--internal-stop'
pass 'Live login is rechecked and tenant is forwarded'

export TEST_SCENARIO=success
invoke --sources "$sources" --no-build > "$temporary/output" 2>&1
assert_contains "$TEST_CALLS" '--internal-launch mock false'
assert_contains "$TEST_CALLS" '--internal-stop'
assert_absent "$TEST_CALLS" 'reset-and-seed'
assert_absent "$TEST_CALLS" 'push '
pass 'Foreground default owns cleanup and never resets or publishes'
invoke --sources "$sources" --no-build --detach > "$temporary/output" 2>&1
assert_contains "$TEST_CALLS" '--internal-launch mock true'
assert_absent "$TEST_CALLS" '--internal-stop'
pass 'Detached success leaves servers running'
export TEST_SCENARIO=hold-launch
invoke --sources "$sources" --no-build > "$temporary/output" 2>&1 &
driver=$!
for attempt in {1..100}; do
  [[ ! -f "$TEST_STATE" ]] || break
  sleep 0.1
done
[[ -f "$TEST_STATE" ]] || { kill -TERM "$driver"; echo "Mock launcher did not start." >&2; exit 1; }
kill -TERM "$(cat "$TEST_STATE")"
status=0
wait "$driver" || status=$?
[[ "$status" != 0 ]]
assert_contains "$TEST_CALLS" '--internal-stop'
assert_contains "$TEST_CALLS" 'rmdir'
pass 'Foreground interruption stops the Docker client and cleans temporary configuration'
export TEST_SCENARIO=success
invoke "${live[@]}" --validate-only > "$temporary/output" 2>&1
assert_absent "$TEST_CALLS" '--internal-restore'
assert_absent "$TEST_CALLS" '-- migrate'
assert_absent "$TEST_CALLS" '--internal-launch'
pass 'Validation is non-interactive and does not restore, migrate or launch'

cat > "$temporary/bin/dotnet" <<'MOCK'
#!/usr/bin/env bash
printf 'dotnet %s\n' "$*" >> "$TEST_CALLS"
cp "$HOME/.nuget/NuGet/NuGet.Config" "$TEST_SNAPSHOT/NuGet.Config"
[[ "${TEST_SCENARIO:-}" != backend-failure ]]
MOCK
cat > "$temporary/bin/pnpm" <<'MOCK'
#!/usr/bin/env bash
printf 'pnpm %s\n' "$*" >> "$TEST_CALLS"
cp "$HOME/.npmrc" "$TEST_SNAPSHOT/npmrc"
[[ "${TEST_SCENARIO:-}" != frontend-failure ]]
MOCK
chmod +x "$temporary/bin/dotnet" "$temporary/bin/pnpm"
mkdir -p "$temporary/home" "$temporary/snapshot"
export TEST_SNAPSHOT="$temporary/snapshot"
printf '%s\n' 'NUGET_USERNAME=test' 'NUGET_PASSWORD=synthetic<&token' \
  'NPM_USERNAME=test' 'NPM_PASSWORD=synthetic-test-token' > "$temporary/external/credentials.conf"
chmod 600 "$temporary/external/credentials.conf"
for scenario in success backend-failure frontend-failure; do
  : > "$TEST_CALLS"
  export TEST_SCENARIO="$scenario"
  status=0
  HOME="$temporary/home" NPM_CONFIG_REGISTRY=https://ignored.example.invalid/ \
    bash -c 'source "$1"; cd "$2"; restore_packages "$3" "$4" "" false' \
    bash "$project/scripts/start-environment.sh" "$temporary/project" "$sources" \
    "$temporary/external/credentials.conf" > "$temporary/output" 2>&1 || status=$?
  if [[ "$scenario" == success ]]; then [[ "$status" == 0 ]]; else [[ "$status" != 0 ]]; fi
  assert_absent "$temporary/home/.npmrc" '_password'
  assert_absent "$temporary/home/.nuget/NuGet/NuGet.Config" 'ClearTextPassword'
  assert_absent "$TEST_CALLS" 'synthetic'
  assert_absent "$temporary/home/.npmrc" 'ignored.example'
  if [[ "$scenario" == backend-failure ]]; then assert_absent "$TEST_CALLS" 'pnpm install'; fi
  pass "File-based restore and credential cleanup: $scenario"
done
assert_contains "$TEST_SNAPSHOT/NuGet.Config" 'synthetic&lt;&amp;token'
assert_contains "$TEST_CALLS" '--ignore-scripts'
export TEST_SCENARIO=success
(
  cd "$temporary/project"
  HOME="$temporary/home" bash -s -- --internal-restore "$sources" \
    "$temporary/external/credentials.conf" "" false < "$project/scripts/start-environment.sh"
) > "$temporary/output" 2>&1
assert_absent "$temporary/home/.npmrc" '_password'
pass 'The same startup file dispatches container commands from stdin'

source "$project/scripts/start-environment.sh"
log="$temporary/server.log"
echo 'Synthetic server failure' > "$log"
curl() { return 1; }
kill() { return 1; }
if wait_for_url API unused 999999 "$log" > "$temporary/output" 2>&1; then exit 1; fi
assert_contains "$temporary/output" 'exited before becoming ready'
assert_contains "$temporary/output" 'Synthetic server failure'
pass 'Readiness failure retains server diagnostics'
unset -f curl kill
mkdir -p "$temporary/servers"
echo another-owner > "$temporary/servers/owner"
stop_owned_servers our-owner "$temporary/servers"
[[ -f "$temporary/servers/owner" ]]
pass 'Cleanup does not stop servers owned by another startup'

if bash "$project/scripts/install-prerequisites.sh" > "$temporary/output" 2>&1; then exit 1; fi
assert_contains "$temporary/output" 'Host installation is opt-in'
pass 'Installer never changes the host without explicit approval'
if bash "$project/scripts/reset-and-seed.sh" > "$temporary/output" 2>&1; then exit 1; fi
assert_contains "$temporary/output" 'explicit --confirm'
pass 'Reset requires explicit confirmation'

for script in "$project"/scripts/*.sh; do bash -n "$script"; done
awk '{sub(/\r$/, "")} /bash -euo pipefail -c / {body=1; next} body && /^    \047$/ {exit} body {print}' "$project/.devcontainer/Dockerfile" | bash -n
pass 'All scripts and the Dockerfile build shell parse'
echo "${passed} script regression cases passed; Docker and Azure were mocked."
