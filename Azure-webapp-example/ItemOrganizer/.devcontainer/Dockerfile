ARG WORKSPACE_BASE_IMAGE
FROM ${WORKSPACE_BASE_IMAGE}

ARG DOTNET_SDK_VERSION=10.0.100
ARG DOTNET_EF_VERSION=10.0.1
ARG NODE_VERSION=22.16.0
ARG PNPM_VERSION=10.11.0
ARG AZURE_CLI_VERSION=2.78.0
ARG BICEP_VERSION=v0.37.4
ARG TARGETARCH
ARG DOWNLOAD_SOURCES_HASH

ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH=/usr/share/dotnet:/usr/local/bin:${PATH}
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_CLI_WORKLOAD_UPDATE_NOTIFY_DISABLE=true

USER root
RUN --mount=type=secret,id=download_sources,required=true \
    --mount=type=secret,id=download_auth \
    --mount=type=secret,id=apt_auth \
    --mount=type=secret,id=custom_ca \
    --mount=type=bind,source=scripts/start-environment.sh,target=/tmp/start-environment.sh \
    bash -euo pipefail -c '\
    source /tmp/start-environment.sh; \
    test -n "$DOWNLOAD_SOURCES_HASH"; \
    load_sources /run/secrets/download_sources; \
    require_sources DEBIAN_APT_URI DEBIAN_SECURITY_URI AZURE_CLI_APT_URI AZURE_CLI_KEY_URL DOTNET_EF_PACKAGE_URL; \
    case "$TARGETARCH" in amd64|arm64) ;; *) fail "Unsupported container architecture."; exit 1 ;; esac; \
    require_sources "DOTNET_SDK_URL_${TARGETARCH^^}" "BICEP_URL_${TARGETARCH^^}"; \
    if [ -f /run/secrets/custom_ca ]; then \
      install -m 0644 /run/secrets/custom_ca /usr/local/share/ca-certificates/custom-ca.crt; \
      update-ca-certificates; \
    fi; \
    mkdir -p /tmp/itemorganizer-apt/lists/partial; \
    printf "Types: deb\nURIs: %s\nSuites: bookworm bookworm-updates\nComponents: main\nSigned-By: /usr/share/keyrings/debian-archive-keyring.gpg\n\nTypes: deb\nURIs: %s\nSuites: bookworm-security\nComponents: main\nSigned-By: /usr/share/keyrings/debian-archive-keyring.gpg\n" \
      "${sources[DEBIAN_APT_URI]}" "${sources[DEBIAN_SECURITY_URI]}" > /tmp/itemorganizer-apt/sources; \
    mv /tmp/itemorganizer-apt/sources /tmp/itemorganizer-apt/sources.sources; \
    apt_get() { apt-get -o Dir::Etc::sourcelist=/tmp/itemorganizer-apt/sources.sources \
      -o Dir::Etc::sourceparts=- -o Dir::State::lists=/tmp/itemorganizer-apt/lists \
      -o Dir::Etc::netrc=/run/secrets/apt_auth -o Dir::Etc::netrcparts=- "$@"; }; \
    apt_get update; \
    apt_get install -y --no-install-recommends ca-certificates curl git gnupg jq python3 tar unzip util-linux \
      libnss3 libnspr4 libatk1.0-0 libatk-bridge2.0-0 libdrm2 libxkbcommon0 libxcomposite1 \
      libxdamage1 libxfixes3 libxrandr2 libgbm1 libasound2 libatspi2.0-0 libx11-6 libxcb1 \
      libxext6 libcups2 libpango-1.0-0 libcairo2 fonts-liberation fonts-noto-color-emoji; \
    auth=""; if [ -f /run/secrets/download_auth ]; then auth=/run/secrets/download_auth; fi; \
    download AZURE_CLI_KEY_URL /tmp/microsoft.asc "$auth"; \
    gpg --batch --yes --dearmor --output /usr/share/keyrings/microsoft-archive-keyring.gpg /tmp/microsoft.asc; \
    printf "\nTypes: deb\nURIs: %s\nSuites: bookworm\nComponents: main\nArchitectures: %s\nSigned-By: /usr/share/keyrings/microsoft-archive-keyring.gpg\n" \
      "${sources[AZURE_CLI_APT_URI]}" "$TARGETARCH" >> /tmp/itemorganizer-apt/sources.sources; \
    apt_get update; \
    apt_get install -y --no-install-recommends "azure-cli=${AZURE_CLI_VERSION}-1~bookworm"; \
    download "DOTNET_SDK_URL_${TARGETARCH^^}" /tmp/dotnet.tar.gz "$auth"; \
    mkdir -p /usr/share/dotnet; tar -xzf /tmp/dotnet.tar.gz -C /usr/share/dotnet; \
    ln -s /usr/share/dotnet/dotnet /usr/local/bin/dotnet; \
    test "$(dotnet --version)" = "$DOTNET_SDK_VERSION"; \
    test "$(node --version)" = "v$NODE_VERSION"; test "$(pnpm --version)" = "$PNPM_VERSION"; \
    download "BICEP_URL_${TARGETARCH^^}" /usr/local/bin/bicep "$auth"; chmod +x /usr/local/bin/bicep; \
    mkdir -p /tmp/dotnet-tools; \
    download DOTNET_EF_PACKAGE_URL "/tmp/dotnet-tools/dotnet-ef.${DOTNET_EF_VERSION}.nupkg" "$auth"; \
    printf "<configuration><packageSources><clear/><add key=\"local\" value=\"/tmp/dotnet-tools\"/></packageSources></configuration>" > /tmp/ef.config; \
    dotnet tool install --tool-path /usr/local/bin dotnet-ef --version "$DOTNET_EF_VERSION" --configfile /tmp/ef.config; \
    install -d -o node -g node /home/node/.nuget/packages /home/node/.local/share/pnpm/store; \
    find /etc/apt/sources.list.d -maxdepth 1 -type f \( -name "*.list" -o -name "*.sources" \) -delete; \
    rm -f /etc/apt/sources.list; \
    rm -rf /tmp/itemorganizer-apt /tmp/dotnet-tools; \
    rm -f /tmp/microsoft.asc /tmp/dotnet.tar.gz /tmp/ef.config; \
    '
USER node
