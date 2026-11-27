#!/usr/bin/env bash

set -euo pipefail
source "$(dirname "$(realpath -- "${BASH_SOURCE[0]}")")/start-environment.sh"

install_prerequisites() (
  local approved=false source_path="" download_auth="" apt_auth="" temporary=""
  local project_root repository_root architecture codename package
  while (($#)); do
    case "$1" in
      --install) approved=true; shift ;;
      --sources|--download-auth|--apt-auth)
        [[ $# -ge 2 && -n "$2" && "$2" != --* ]] || { fail "$1 requires a file path."; exit 1; }
        case "$1" in
          --sources) source_path="$2" ;; --download-auth) download_auth="$2" ;; --apt-auth) apt_auth="$2" ;;
        esac
        shift 2 ;;
      --help)
        echo "Usage: sudo bash scripts/install-prerequisites.sh --install --sources /external/sources.conf"
        echo "Optional protected external files: --download-auth (netrc), --apt-auth (apt auth.conf)."
        echo "Installs/starts standard Docker Engine on Ubuntu; never grants user permissions."
        exit 0 ;;
      *) fail "Unknown installer option $1."; exit 1 ;;
    esac
  done
  [[ "$approved" == true && -n "$source_path" ]] ||
    { fail "Host installation is opt-in: provide --install and --sources."; exit 1; }
  [[ "$EUID" == 0 ]] || { fail "Docker Engine installation requires administrator permission."; exit 1; }
  require_ubuntu
  [[ -d /run/systemd/system ]] ||
    { fail "Enable systemd through administrator-managed Ubuntu/WSL setup before installation."; exit 1; }
  project_root="$(dirname "$(dirname "$(realpath -- "${BASH_SOURCE[0]}")")")"
  repository_root="$(repository_root_for "$project_root")"
  source_path="$(external_file "$source_path")"
  load_sources "$source_path"
  [[ -z "$download_auth" ]] || download_auth="$(protected_file "$download_auth")"
  [[ -z "$apt_auth" ]] || apt_auth="$(protected_file "$apt_auth")"
  architecture="$(dpkg --print-architecture)"
  case "$architecture" in amd64|arm64) ;; *) fail "Supported host architectures are amd64 and arm64."; exit 1 ;; esac
  codename="$(. /etc/os-release; printf '%s' "$VERSION_CODENAME")"
  [[ "$codename" =~ ^[a-z]+$ ]] || { fail "Ubuntu release codename is unavailable."; exit 1; }
  require_sources "UBUNTU_APT_URI_${architecture^^}" "UBUNTU_SECURITY_URI_${architecture^^}" DOCKER_APT_URI DOCKER_APT_KEY_URL
  for package in docker.io docker-compose docker-compose-v2 docker-doc podman-docker containerd runc; do
    if dpkg-query -W -f='${Status}' "$package" 2>/dev/null | grep -q 'install ok installed'; then
      fail "Conflicting package ${package} is installed. Ask the administrator to resolve it; nothing is uninstalled automatically."
      exit 1
    fi
  done
  temporary="$(mktemp -d /tmp/itemorganizer-install.XXXXXX)"
  trap 'rm -rf -- "$temporary"' EXIT
  chmod 755 "$temporary"
  mkdir -p "$temporary/lists/partial"
  printf 'Types: deb\nURIs: %s\nSuites: %s %s-updates %s-backports\nComponents: main universe restricted multiverse\nSigned-By: /usr/share/keyrings/ubuntu-archive-keyring.gpg\n\nTypes: deb\nURIs: %s\nSuites: %s-security\nComponents: main universe restricted multiverse\nSigned-By: /usr/share/keyrings/ubuntu-archive-keyring.gpg\n' \
    "${sources[UBUNTU_APT_URI_${architecture^^}]}" "$codename" "$codename" "$codename" \
    "${sources[UBUNTU_SECURITY_URI_${architecture^^}]}" "$codename" > "$temporary/sources.sources"
  apt_get() {
    local -a arguments=(-o "Dir::Etc::sourcelist=$temporary/sources.sources"
      -o Dir::Etc::sourceparts=- -o "Dir::State::lists=$temporary/lists"
      -o Dir::Etc::netrcparts=-)
    if [[ -n "$apt_auth" ]]; then arguments+=(-o "Dir::Etc::netrc=$apt_auth"); else arguments+=(-o Dir::Etc::netrc=-); fi
    apt-get "${arguments[@]}" "$@"
  }
  run "Updating configured Ubuntu package indexes" "Check approved Ubuntu repositories and administrator-configured TLS trust." apt_get update
  run "Installing Docker setup prerequisites" "Check configured Ubuntu repositories." \
    apt_get install -y --no-install-recommends ca-certificates curl
  download DOCKER_APT_KEY_URL "$temporary/docker.asc" "$download_auth"
  install -d -m 0755 /etc/apt/keyrings
  install -m 0644 "$temporary/docker.asc" /etc/apt/keyrings/docker.asc
  printf '\nTypes: deb\nURIs: %s\nSuites: %s\nComponents: stable\nArchitectures: %s\nSigned-By: /etc/apt/keyrings/docker.asc\n' \
    "${sources[DOCKER_APT_URI]}" "$codename" "$architecture" >> "$temporary/sources.sources"
  run "Updating configured Docker package indexes" "Check the configured signed Docker repository." apt_get update
  run "Installing Docker Engine and plugins" "Check approved Docker/Ubuntu repositories and package compatibility." \
    apt_get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
  run "Starting Docker Engine" "Inspect systemctl status docker and administrator policy." systemctl enable --now docker
  docker version
  docker compose version
  docker buildx version
  echo "Docker Engine is installed. Administrator-managed user access is still required."
  echo "No groups, socket permissions, editor settings, or project SDKs were changed."
  echo "Future Docker updates must also use administrator-approved configured sources."
)

install_prerequisites "$@"
