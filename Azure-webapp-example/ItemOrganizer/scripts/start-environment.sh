#!/usr/bin/env bash

set +x
set -euo pipefail

declare -A sources=() credentials=()
docker_command=(docker)

fail() {
  printf 'Error: %s\n' "$*" >&2
  return 1
}

run() {
  local step="$1" recovery="$2"
  shift 2
  echo "> ${step}"
  if "$@"; then
    return 0
  else
    local status=$?
    printf '%s failed (exit %s).\nRecovery: %s\n' "$step" "$status" "$recovery" >&2
    return "$status"
  fi
}

read_settings() {
  local path="$1" name="$2" line key value number=0
  local -n settings="$name"
  settings=()
  [[ -r "$path" && -f "$path" ]] || { fail "Configuration file is unreadable."; return 1; }
  while IFS= read -r line || [[ -n "$line" ]]; do
    number=$((number + 1))
    line="${line%$'\r'}"
    [[ -z "$line" || "$line" == \#* ]] && continue
    [[ "$line" =~ ^[A-Z][A-Z0-9_]*= ]] ||
      { fail "Invalid configuration entry at line ${number}; use unquoted KEY=value."; return 1; }
    key="${line%%=*}"
    value="${line#*=}"
    [[ -n "$value" && ! -v "settings[$key]" ]] ||
      { fail "Empty or duplicate configuration key at line ${number}."; return 1; }
    settings["$key"]="$value"
  done < "$path"
}

load_sources() {
  local key value
  read_settings "$1" sources
  for key in "${!sources[@]}"; do
    case "$key" in
      WORKSPACE_BASE_IMAGE|POSTGRES_IMAGE|AZURITE_IMAGE)
        value="${sources[$key]}"
        [[ "$value" =~ ^[a-zA-Z0-9.-]+(:[0-9]+)?/[a-z0-9._/-]+(:[a-zA-Z0-9._-]+)?(@sha256:[a-f0-9]{64})?$ ]] &&
          [[ "${value%%/*}" == *.* || "${value%%/*}" == *:* || "${value%%/*}" == localhost ]] ||
          { fail "Image key ${key} must include an explicit registry."; return 1; }
        [[ "$value" == *@sha256:* || "${value##*/}" == *:* ]] ||
          { fail "Image key ${key} must be pinned by tag or digest."; return 1; }
        [[ "$value" != *:latest ]] || { fail "Image key ${key} must not use latest."; return 1; }
        ;;
      UBUNTU_APT_URI_AMD64|UBUNTU_SECURITY_URI_AMD64|UBUNTU_APT_URI_ARM64|UBUNTU_SECURITY_URI_ARM64|DOCKER_APT_URI|DOCKER_APT_KEY_URL|DEBIAN_APT_URI|DEBIAN_SECURITY_URI|AZURE_CLI_APT_URI|AZURE_CLI_KEY_URL|DOTNET_SDK_URL_AMD64|DOTNET_SDK_URL_ARM64|DOTNET_EF_PACKAGE_URL|BICEP_URL_AMD64|BICEP_URL_ARM64|NUGET_SOURCE|NPM_REGISTRY|PLAYWRIGHT_CHROMIUM_URL_AMD64|PLAYWRIGHT_CHROMIUM_URL_ARM64)
        value="${sources[$key]}"
        [[ "$value" == https://* && "${value#https://}" == */* &&
           "$value" != *[$' \t'\"\'\`\$\\]* && "$value" != *'@'* &&
           "$value" != *'?'* && "$value" != *'#'* ]] ||
          { fail "Source key ${key} requires an HTTPS URL without credentials, queries, or substitutions."; return 1; }
        ;;
      *) fail "Unknown source key ${key}."; return 1 ;;
    esac
  done
}

require_sources() {
  local key
  for key in "$@"; do
    [[ -n "${sources[$key]:-}" ]] ||
      { fail "Missing source key ${key}; no fallback is configured."; return 1; }
  done
}

load_credentials() {
  local key
  credentials=()
  [[ -n "$1" ]] || return 0
  read_settings "$1" credentials
  for key in "${!credentials[@]}"; do
    case "$key" in
      NUGET_USERNAME|NUGET_PASSWORD|NPM_USERNAME|NPM_PASSWORD) ;;
      *) fail "Unsupported credential entry; use the documented credential-file format."; return 1 ;;
    esac
  done
  for key in NUGET NPM; do
    if [[ -n "${credentials[${key}_USERNAME]:-}" || -n "${credentials[${key}_PASSWORD]:-}" ]]; then
      [[ -n "${credentials[${key}_USERNAME]:-}" && -n "${credentials[${key}_PASSWORD]:-}" ]] ||
        { fail "${key} credentials require both username and password."; return 1; }
    fi
  done
  if [[ -n "${credentials[NPM_USERNAME]:-}" ]]; then
    [[ "${credentials[NPM_USERNAME]}" =~ ^[a-zA-Z0-9._@-]+$ ]] ||
      { fail "Npm credential username contains unsupported characters."; return 1; }
  fi
}

external_file() {
  local path
  path="$(realpath -- "$1")"
  [[ -f "$path" && -r "$path" ]] || { fail "External file is missing or unreadable."; return 1; }
  [[ "$path" != "$repository_root" && "$path" != "$repository_root/"* ]] ||
    { fail "Source and credential files must be outside the Git repository."; return 1; }
  printf '%s\n' "$path"
}

protected_file() {
  local path mode
  path="$(external_file "$1")"
  mode="$(stat -c '%a' -- "$path")"
  (( (8#$mode & 077) == 0 )) ||
    { fail "Credential files must not be readable or writable by group/others; use chmod 600."; return 1; }
  printf '%s\n' "$path"
}

repository_root_for() {
  local directory="$1"
  while [[ "$directory" != / ]]; do
    if [[ -e "$directory/.git" ]]; then printf '%s\n' "$directory"; return 0; fi
    directory="$(dirname "$directory")"
  done
  fail "Run from a Git checkout so external-file boundaries can be verified."
}

require_ubuntu() {
  [[ "$(uname -s)" == Linux ]] ||
    { fail "Run this script in native Ubuntu or Ubuntu inside WSL 2, not Windows PowerShell."; return 1; }
  grep -qx 'ID=ubuntu' /etc/os-release ||
    { fail "The supported development host is Ubuntu."; return 1; }
}

docker_cmd() { "${docker_command[@]}" "$@"; }
compose() { docker_cmd compose "$@"; }

check_engine() {
  local server
  command -v docker >/dev/null || { fail "Docker CLI is missing; request administrator setup."; return 1; }
  server="$(docker_cmd info --format '{{.OperatingSystem}}|{{.OSType}}')" ||
    { fail "Cannot connect to Docker Engine; ask the administrator to start it and approve your access."; return 1; }
  [[ "$server" == *'|linux' && "$server" != *'Docker Desktop'* ]] ||
    { fail "Use standalone Linux Docker Engine, not Docker Desktop or Windows containers."; return 1; }
  run "Checking Compose v2" "Install the Compose v2 plugin through approved administrator setup." compose version
  run "Checking Buildx" "Install the Buildx plugin through approved administrator setup." docker_cmd buildx version
}

download() {
  local key="$1" output="$2" auth="${3:-}"
  local -a arguments=(--fail --silent --show-error --location --proto '=https' --proto-redir '=https')
  require_sources "$key"
  [[ -z "$auth" ]] || arguments+=(--netrc-file "$auth")
  curl "${arguments[@]}" "${sources[$key]}" --output "$output"
}

xml_escape() {
  local value="$1"
  value="${value//&/\&amp;}"
  value="${value//</\&lt;}"
  value="${value//>/\&gt;}"
  value="${value//\"/\&quot;}"
  value="${value//\'/\&apos;}"
  printf '%s' "$value"
}

write_package_configs() {
  local authenticated="$1" scope password
  mkdir -p "$HOME/.nuget/NuGet"
  touch "$HOME/.nuget/NuGet/NuGet.Config" "$HOME/.npmrc"
  chmod 600 "$HOME/.nuget/NuGet/NuGet.Config" "$HOME/.npmrc"
  {
    printf '<configuration><packageSources><clear/><add key="configured" value="%s"/></packageSources>' \
      "$(xml_escape "${sources[NUGET_SOURCE]}")"
    if [[ "$authenticated" == true && -n "${credentials[NUGET_PASSWORD]:-}" ]]; then
      printf '<packageSourceCredentials><configured><add key="Username" value="%s"/><add key="ClearTextPassword" value="%s"/></configured></packageSourceCredentials>' \
        "$(xml_escape "${credentials[NUGET_USERNAME]}")" "$(xml_escape "${credentials[NUGET_PASSWORD]}")"
    fi
    printf '</configuration>\n'
  } > "$HOME/.nuget/NuGet/NuGet.Config"
  printf 'registry=%s\nstrict-ssl=true\nupdate-notifier=false\n' "${sources[NPM_REGISTRY]}" > "$HOME/.npmrc"
  if [[ "$authenticated" == true && -n "${credentials[NPM_PASSWORD]:-}" ]]; then
    password="$(printf '%s' "${credentials[NPM_PASSWORD]}" | base64 | tr -d '\n')"
    scope="//${sources[NPM_REGISTRY]#https://}"
    scope="${scope%/}/"
    local -a scopes=("$scope")
    [[ "$scope" != */registry/ ]] || scopes+=("${scope%registry/}")
    for scope in "${scopes[@]}"; do
      printf '%s:username=%s\n%s:_password=%s\n%s:email=npm@example.invalid\n%s:always-auth=true\n' \
        "$scope" "${credentials[NPM_USERNAME]}" "$scope" "$password" "$scope" "$scope" >> "$HOME/.npmrc"
    done
  fi
}

prepare_browser() (
  local auth="$1" architecture key expected actual directory temporary executable
  architecture="$(dpkg --print-architecture)"
  case "$architecture" in amd64|arm64) ;; *) fail "Unsupported browser architecture."; exit 1 ;; esac
  key="PLAYWRIGHT_CHROMIUM_URL_${architecture^^}"
  require_sources "$key"
  expected="$(node -e "const fs=require('node:fs'),p=require('node:path'); const root=p.dirname(require.resolve('playwright-core/package.json')); console.log(JSON.parse(fs.readFileSync(p.join(root,'browsers.json'))).browsers.find(b => b.name === 'chromium').browserVersion)")"
  directory="$HOME/.cache/itemorganizer/chromium"
  if [[ -x "$directory/chrome" ]]; then
    actual="$("$directory/chrome" --version)"
    [[ "$actual" != *"$expected"* ]] || { echo "Configured Chromium version is already available."; exit 0; }
  fi
  temporary="$(mktemp -d)"
  trap 'rm -rf -- "$temporary"' EXIT
  download "$key" "$temporary/chromium.zip" "$auth"
  unzip -q "$temporary/chromium.zip" -d "$temporary/browser"
  executable="$(find "$temporary/browser" -type f -name chrome -print -quit)"
  [[ -n "$executable" && -x "$executable" ]] || { fail "Configured archive has no executable Chromium."; exit 1; }
  actual="$("$executable" --version)"
  [[ "$actual" == *"$expected"* ]] ||
    { fail "Configured Chromium archive does not match the locked Playwright browser version."; exit 1; }
  mkdir -p "$directory"
  cp -a "$temporary/browser/." "$directory/"
  ln -sfn "${executable#"$temporary/browser/"}" "$directory/chrome"
)

restore_packages() (
  local source_path="$1" credential_path="$2" auth="$3" browser="$4"
  umask 077
  load_sources "$source_path"
  require_sources NUGET_SOURCE NPM_REGISTRY
  load_credentials "$credential_path"
  cleanup_restore() {
    if ! write_package_configs false; then
      fail "Package credential cleanup failed; stop/remove workspace before sharing it."
      exit 1
    fi
  }
  trap cleanup_restore EXIT
  trap 'exit 130' INT
  trap 'exit 143' TERM
  unset NPM_CONFIG_REGISTRY npm_config_registry NPM_CONFIG_USERCONFIG npm_config_userconfig
  unset NPM_CONFIG_STRICT_SSL npm_config_strict_ssl NuGetPackageSourceCredentials_configured
  unset RestoreSources RestoreAdditionalProjectSources NODE_TLS_REJECT_UNAUTHORIZED
  write_package_configs true
  run "Backend package restore" "Check the configured NuGet source, credential file and container certificate trust." \
    dotnet restore ItemOrganizer.sln --configfile "$HOME/.nuget/NuGet/NuGet.Config" --nologo
  (
    cd web
    run "Frontend package restore" "Check the configured npm source, credential file and container certificate trust." \
      pnpm install --registry "${sources[NPM_REGISTRY]}" --strict-ssl=true \
        --frozen-lockfile --ignore-scripts --store-dir "$HOME/.local/share/pnpm/store"
    [[ "$browser" == false ]] || prepare_browser "$auth"
  )
)

stop_owned_servers() {
  local token="$1" directory="${2:-/tmp/itemorganizer}" name pid ticks current attempt
  [[ -f "$directory/owner" && "$(cat "$directory/owner")" == "$token" ]] || return 0
  for name in frontend api; do
    [[ -f "$directory/$name.pid" ]] || continue
    read -r pid ticks < "$directory/$name.pid"
    [[ "$pid" =~ ^[0-9]+$ && "$pid" -gt 1 && "$ticks" =~ ^[0-9]+$ ]] ||
      { fail "Invalid managed server PID record."; return 1; }
    if [[ -r "/proc/$pid/stat" ]]; then
      current="$(awk '{print $22}' "/proc/$pid/stat")"
      if [[ "$current" == "$ticks" ]] && kill -0 -- "-$pid" 2>/dev/null; then
        kill -TERM -- "-$pid"
        for attempt in {1..50}; do
          kill -0 -- "-$pid" 2>/dev/null || break
          sleep 0.1
        done
        if kill -0 -- "-$pid" 2>/dev/null; then kill -KILL -- "-$pid"; fi
      fi
    fi
    rm -f -- "$directory/$name.pid"
  done
  rm -f -- "$directory/owner" "$directory/signature"
}

stop_launcher() {
  local token="$1" directory="/tmp/$1" pid ticks current attempt
  [[ "$token" == itemorganizer-config.* && "$token" != */* ]] ||
    { fail "Invalid launcher ownership token."; return 1; }
  if [[ -d "$directory" ]]; then
    touch "$directory/cancelled"
    if [[ -f "$directory/launcher.pid" ]]; then
      read -r pid ticks < "$directory/launcher.pid"
      [[ "$pid" =~ ^[0-9]+$ && "$pid" -gt 1 && "$ticks" =~ ^[0-9]+$ ]] ||
        { fail "Invalid launcher PID record."; return 1; }
      if [[ -r "/proc/$pid/stat" ]]; then
        current="$(awk '{print $22}' "/proc/$pid/stat")"
        if [[ "$current" == "$ticks" ]] && kill -0 "$pid" 2>/dev/null; then
          kill -TERM "$pid"
          for attempt in {1..50}; do
            kill -0 "$pid" 2>/dev/null || break
            sleep 0.1
          done
        fi
      fi
    fi
  fi
  stop_owned_servers "$token"
}

wait_for_url() {
  local name="$1" url="$2" pid="$3" log="$4" deadline=$((SECONDS + 120))
  while ((SECONDS < deadline)); do
    if curl --fail --silent --show-error --max-time 2 "$url" >/dev/null 2>&1; then
      echo "${name} is ready."
      return 0
    fi
    if ! kill -0 "$pid" 2>/dev/null; then
      tail -n 50 "$log" >&2
      fail "${name} exited before becoming ready."
      return 1
    fi
    sleep 1
  done
  tail -n 50 "$log" >&2
  fail "${name} did not become ready within 120 seconds."
}

launch_applications() (
  local mode="$1" detached="$2" token="$3" endpoint="$4" deployment="$5"
  local directory="/tmp/itemorganizer" control="/tmp/$token" signature api_pid frontend_pid keep_running=false
  [[ -d "$control" && ! -f "$control/cancelled" ]] || { fail "Startup was cancelled."; exit 1; }
  local launcher_pid="$BASHPID"
  printf '%s %s\n' "$launcher_pid" "$(awk '{print $22}' "/proc/$launcher_pid/stat")" > "$control/launcher.pid"
  cleanup_launch() {
    if [[ "$keep_running" == false ]]; then stop_owned_servers "$token"; fi
    rm -f -- "$control/launcher.pid"
  }
  trap cleanup_launch EXIT
  trap 'exit 130' INT
  trap 'exit 143' TERM
  [[ ! -f "$control/cancelled" ]] || { fail "Startup was cancelled."; exit 1; }
  mkdir -p "$directory"
  exec 9>"$directory/start.lock"
  flock -n 9 || { fail "Another foreground startup owns the application servers."; exit 1; }
  signature="$(printf '%s\n' "$mode" "$endpoint" "$deployment" | sha256sum | cut -d ' ' -f 1)"
  if curl --fail --silent --max-time 2 http://localhost:5000/api/v1/health/ready >/dev/null 2>&1 ||
     curl --fail --silent --max-time 2 http://localhost:5173/ >/dev/null 2>&1; then
    [[ -f "$directory/signature" && "$(cat "$directory/signature")" == "$signature" &&
       -f "$directory/api.pid" && -f "$directory/frontend.pid" ]] ||
      { fail "Servers already run with another configuration; use docker compose stop before switching."; exit 1; }
    read -r api_pid _ < "$directory/api.pid"
    read -r frontend_pid _ < "$directory/frontend.pid"
  else
    printf '%s\n' "$token" > "$directory/owner"
    printf '%s\n' "$signature" > "$directory/signature"
    local provider=deterministic model=deterministic-mock
    if [[ "$mode" == live ]]; then provider=azure-openai; model="$deployment"; fi
    nohup setsid env "ASPNETCORE_URLS=http://0.0.0.0:5000" \
      "Analysis__Provider=$provider" "Analysis__Model=$model" "AzureOpenAI__Endpoint=$endpoint" \
      dotnet run --project src/ItemOrganizer.Api --no-restore > "$directory/api.log" 2>&1 </dev/null 9>&- &
    api_pid=$!
    printf '%s %s\n' "$api_pid" "$(awk '{print $22}' "/proc/$api_pid/stat")" > "$directory/api.pid"
    nohup setsid pnpm -C web dev > "$directory/frontend.log" 2>&1 </dev/null 9>&- &
    frontend_pid=$!
    printf '%s %s\n' "$frontend_pid" "$(awk '{print $22}' "/proc/$frontend_pid/stat")" > "$directory/frontend.pid"
  fi
  printf '%s\n' "$token" > "$directory/owner"
  wait_for_url API http://localhost:5000/api/v1/health/ready "$api_pid" "$directory/api.log"
  wait_for_url Frontend http://localhost:5173/ "$frontend_pid" "$directory/frontend.log"
  echo "Item Organizer (${mode}) is ready at http://localhost:5173."
  if [[ "$detached" == true ]]; then
    keep_running=true
    echo "Servers remain running. Use docker compose stop to stop the local services."
    exit 0
  fi
  echo "Press Ctrl+C to stop the API and frontend; database/storage containers remain running."
  while kill -0 "$api_pid" 2>/dev/null && kill -0 "$frontend_pid" 2>/dev/null; do sleep 1 9>&-; done
  fail "A development server exited unexpectedly; inspect /tmp/itemorganizer/*.log in workspace."
)

check_live() {
  local validate="$1" tenant="$2" resource="$3" group="$4" deployment="$5" state
  if ! compose exec -T workspace az account show --only-show-errors --output none; then
    [[ "$validate" == false ]] ||
      { fail "Workspace Azure CLI session is missing; validation does not sign in. Use normal live startup."; return 1; }
    local -a login=(compose exec -T -e AZURE_CORE_LOGIN_EXPERIENCE_V2=off workspace az login --use-device-code --output none)
    [[ -z "$tenant" ]] || login+=(--tenant "$tenant")
    run "Container Azure device-code login" "Complete browser sign-in with access to the configured Azure resource." "${login[@]}"
    run "Checking Azure login result" "Sign in inside workspace, not on the host." \
      compose exec -T workspace az account show --only-show-errors --output none
  fi
  state="$(compose exec -T workspace az cognitiveservices account deployment show \
    --name "$resource" --resource-group "$group" --deployment-name "$deployment" \
    --only-show-errors --query properties.provisioningState --output tsv)" ||
    { fail "Cannot read the live deployment; check resource names, Azure subscription and permissions."; return 1; }
  [[ "${state%$'\r'}" == Succeeded ]] ||
    { fail "Live deployment is not ready; resolve its provisioning state before startup."; return 1; }
}

copy_configuration() {
  local local_path="$1" remote_path="$2"
  compose exec -T workspace sh -c 'umask 077; cat > "$1"' sh "$remote_path" < "$local_path"
}

main() (
  local script_path project_root repository_root source_path="" credential_path="" download_auth="" apt_auth=""
  local docker_config="" client_directory="" custom_ca="" mode=mock detached=false no_build=false validate=false browser=false skip_start=false
  local tenant="" endpoint="" deployment="" resource="" group="" remote_directory="" token="" stop_on_exit=false launch_pid=""
  script_path="$(realpath -- "${BASH_SOURCE[0]}")"
  project_root="$(dirname "$(dirname "$script_path")")"
  while (($#)); do
    case "$1" in
      --sources|--credentials|--download-auth|--apt-auth|--docker-config|--custom-ca|--mode|--tenant-id|--endpoint|--deployment-name|--resource-name|--resource-group)
        [[ $# -ge 2 && -n "$2" && "$2" != --* ]] || { fail "Option $1 requires a value."; exit 1; }
        case "$1" in
          --sources) source_path="$2" ;; --credentials) credential_path="$2" ;;
          --download-auth) download_auth="$2" ;; --apt-auth) apt_auth="$2" ;;
          --docker-config) docker_config="$2" ;; --custom-ca) custom_ca="$2" ;;
          --mode) mode="$2" ;; --tenant-id) tenant="$2" ;; --endpoint) endpoint="$2" ;;
          --deployment-name) deployment="$2" ;; --resource-name) resource="$2" ;; --resource-group) group="$2" ;;
        esac
        shift 2 ;;
      --detach) detached=true; shift ;; --no-build) no_build=true; shift ;;
      --validate-only) validate=true; shift ;; --prepare-browser) browser=true; shift ;;
      --skip-application-start) skip_start=true; shift ;;
      --help)
        echo "Usage: bash scripts/start-environment.sh --sources /external/sources.conf [--mode mock|live] [--no-build] [--detach]"
        echo "Optional files: --credentials, --download-auth (netrc), --apt-auth, --docker-config, --custom-ca."
        echo "Live options: --endpoint, --deployment-name, --resource-name, --resource-group, --tenant-id."
        echo "Other options: --validate-only, --prepare-browser, --skip-application-start."
        exit 0 ;;
      *) fail "Unknown startup option $1."; exit 1 ;;
    esac
  done
  [[ -n "$source_path" ]] || { fail "--sources is required; no default sources are used."; exit 1; }
  [[ "$mode" == mock || "$mode" == live ]] || { fail "--mode must be mock or live."; exit 1; }
  [[ -z "$custom_ca" || "$no_build" == false ]] ||
    { fail "--custom-ca requires an image build; do not combine it with --no-build."; exit 1; }
  require_ubuntu
  repository_root="$(repository_root_for "$project_root")"
  source_path="$(external_file "$source_path")"
  load_sources "$source_path"
  require_sources WORKSPACE_BASE_IMAGE POSTGRES_IMAGE AZURITE_IMAGE NUGET_SOURCE NPM_REGISTRY
  if [[ -n "$credential_path" ]]; then
    credential_path="$(protected_file "$credential_path")"
    load_credentials "$credential_path"
    credentials=()
  fi
  [[ -z "$download_auth" ]] || download_auth="$(protected_file "$download_auth")"
  [[ -z "$apt_auth" ]] || apt_auth="$(protected_file "$apt_auth")"
  [[ -z "$custom_ca" ]] || custom_ca="$(external_file "$custom_ca")"
  if [[ -n "$docker_config" ]]; then
    docker_config="$(realpath -- "$docker_config")"
    protected_file "$docker_config/config.json" >/dev/null
    if grep -Eq '"(credsStore|credHelpers)"[[:space:]]*:' "$docker_config/config.json"; then
      fail "Use protected file-based Docker auths, not credential-helper programs."
      exit 1
    fi
    docker_command=(docker --config "$docker_config")
  else
    client_directory="$(mktemp -d /tmp/itemorganizer-client.XXXXXX)"
    docker_command=(docker --config "$client_directory")
    trap 'rm -rf -- "$client_directory"' EXIT
  fi
  if [[ "$mode" == live ]]; then
    [[ "$endpoint" == https://* && "$endpoint" != *'@'* && "$endpoint" != *'?'* &&
       "${endpoint#https://}" =~ ^[a-zA-Z0-9.-]+(:[0-9]+)?(/[^[:space:]]*)?$ &&
       -n "$deployment" && -n "$resource" && -n "$group" ]] ||
      { fail "Live mode requires --endpoint (HTTPS), --deployment-name, --resource-name and --resource-group."; exit 1; }
  fi
  check_engine
  cd "$project_root"
  if [[ ! -f .env ]]; then cp .env.example .env; echo "Created .env without modifying existing configuration."; fi
  if [[ ! -f web/.env.local ]]; then cp web/.env.example web/.env.local; fi
  local key image alias
  for key in POSTGRES_IMAGE AZURITE_IMAGE; do
    image="${sources[$key]}"
    if ! docker_cmd image inspect "$image" >/dev/null 2>&1; then
      run "Pulling configured ${key}" "Check the external image source, registry credentials and Docker certificate trust." docker_cmd pull "$image"
    fi
    case "$key" in POSTGRES_IMAGE) alias=itemorganizer-postgres:local ;; AZURITE_IMAGE) alias=itemorganizer-azurite:local ;; esac
    docker_cmd tag "$image" "$alias"
  done
  if [[ "$no_build" == false && "$validate" == false ]]; then
    require_sources DEBIAN_APT_URI DEBIAN_SECURITY_URI AZURE_CLI_APT_URI AZURE_CLI_KEY_URL DOTNET_EF_PACKAGE_URL
    local -a build=(docker_cmd build --file .devcontainer/Dockerfile --tag itemorganizer-workspace:local
      --build-arg "WORKSPACE_BASE_IMAGE=${sources[WORKSPACE_BASE_IMAGE]}"
      --build-arg "DOWNLOAD_SOURCES_HASH=$(sha256sum "$source_path" | cut -d ' ' -f 1)"
      --secret "id=download_sources,src=$source_path")
    [[ -z "$download_auth" ]] || build+=(--secret "id=download_auth,src=$download_auth")
    [[ -z "$apt_auth" ]] || build+=(--secret "id=apt_auth,src=$apt_auth")
    [[ -z "$custom_ca" ]] || build+=(--secret "id=custom_ca,src=$custom_ca")
    local line value
    for key in DOTNET_SDK_VERSION DOTNET_EF_VERSION NODE_VERSION PNPM_VERSION AZURE_CLI_VERSION BICEP_VERSION; do
      value=""
      while IFS= read -r line || [[ -n "$line" ]]; do
        line="${line%$'\r'}"
        [[ "$line" != "$key="* ]] || value="${line#*=}"
      done < .env
      if [[ "$value" == \"*\" || "$value" == \'*\' ]]; then value="${value:1:${#value}-2}"; fi
      [[ "$value" =~ ^v?[0-9]+(\.[0-9]+)+$ ]] ||
        { fail "Missing or invalid pinned tool version ${key} in .env."; exit 1; }
      build+=(--build-arg "$key=$value")
    done
    build+=(.)
    run "Building the local workspace image" "Check configured build sources and certificate trust; no source fallback is attempted." "${build[@]}"
  fi
  run "Checking local workspace image" "Run startup without --no-build to build the workspace from configured sources." \
    docker_cmd image inspect itemorganizer-workspace:local >/dev/null
  run "Validating Compose configuration" "Correct .env and the Compose configuration." compose config --quiet
  run "Starting Docker services" "Inspect docker compose ps and docker compose logs --tail 50." compose up -d --wait --no-build workspace
  run "Workspace smoke test" "Inspect tool versions and PostgreSQL/Azurite container connectivity." \
    compose exec -T workspace bash scripts/smoke-test.sh
  if [[ "$mode" == live ]]; then check_live "$validate" "$tenant" "$resource" "$group" "$deployment"; fi
  if [[ "$validate" == true || "$skip_start" == true ]]; then
    echo "Container prerequisites are valid; no package restore, migration or server startup was performed."
    exit 0
  fi
  remote_directory="$(compose exec -T workspace mktemp -d /tmp/itemorganizer-config.XXXXXX)"
  [[ "$remote_directory" == /tmp/itemorganizer-config.* && "$remote_directory" != *[$'\r\n ']* ]] ||
    { fail "Could not allocate a private container configuration directory."; exit 1; }
  token="${remote_directory##*/}"
  cleanup() {
    local status=$?
    trap - EXIT
    if [[ "$stop_on_exit" == true ]]; then
      if ! compose exec -T workspace bash -s -- --internal-stop "$token" < "$script_path"; then
        echo "Server cleanup failed; use docker compose stop." >&2
        status=1
      fi
    fi
    if [[ -n "$launch_pid" ]] && kill -0 "$launch_pid" 2>/dev/null; then
      kill -TERM "$launch_pid"
      if wait "$launch_pid"; then :; else echo "Stopped the startup Docker client."; fi
    fi
    if ! compose exec -T workspace sh -c 'rm -f -- "$1/sources.conf" "$1/credentials.conf" "$1/download.netrc" "$1/launcher.pid" "$1/cancelled"; rmdir -- "$1"' sh "$remote_directory"; then
      echo "Temporary configuration cleanup failed; stop/remove workspace before sharing it." >&2
      status=1
    fi
    if [[ -n "$client_directory" ]]; then rm -rf -- "$client_directory"; fi
    exit "$status"
  }
  trap cleanup EXIT
  trap 'exit 130' INT
  trap 'exit 143' TERM
  copy_configuration "$source_path" "$remote_directory/sources.conf"
  local remote_credentials="" remote_auth=""
  if [[ -n "$credential_path" ]]; then
    remote_credentials="$remote_directory/credentials.conf"
    copy_configuration "$credential_path" "$remote_credentials"
  fi
  if [[ -n "$download_auth" ]]; then
    remote_auth="$remote_directory/download.netrc"
    copy_configuration "$download_auth" "$remote_auth"
  fi
  compose exec -T workspace bash -s -- --internal-restore \
    "$remote_directory/sources.conf" "$remote_credentials" "$remote_auth" "$browser" < "$script_path"
  compose exec -T workspace sh -c 'rm -f -- "$1/sources.conf" "$1/credentials.conf" "$1/download.netrc"' sh "$remote_directory"
  run "Applying database migrations" "Check the database connection and migration error; startup does not reset data." \
    compose exec -T workspace dotnet run --project src/ItemOrganizer.Database --no-restore -- migrate
  stop_on_exit=true
  "${docker_command[@]}" compose exec -T workspace bash -s -- --internal-launch \
    "$mode" "$detached" "$token" "$endpoint" "$deployment" < "$script_path" &
  launch_pid=$!
  wait "$launch_pid"
  launch_pid=""
  if [[ "$detached" == true ]]; then stop_on_exit=false; fi
)

if [[ "${BASH_SOURCE[0]:-$0}" == "$0" ]]; then
  case "${1:-}" in
    --internal-restore) shift; restore_packages "$@" ;;
    --internal-launch) shift; launch_applications "$@" ;;
    --internal-stop) shift; stop_launcher "$@" ;;
    *) main "$@" ;;
  esac
fi
