# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/devcontainers/javascript-node:1-22-bookworm@sha256:0d29e5fdc64f8397cd502223e0c4679f1e60877ca0fd2db4f2e2e0028e4271af

ARG DOTNET_SDK_VERSION=10.0.100
ARG DOTNET_EF_VERSION=10.0.0
ARG NODE_VERSION=22.16.0
ARG PNPM_VERSION=10.11.0
ARG AZURE_CLI_VERSION=2.78.0
ARG BICEP_VERSION=v0.37.4
ARG TARGETARCH

ENV DEBIAN_FRONTEND=noninteractive
ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH=/usr/share/dotnet:/usr/local/bin:${PATH}

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        apt-transport-https \
        ca-certificates \
        curl \
        git \
        gnupg \
        jq \
        python3 \
        python3-pip \
        python3-venv \
        tar \
        unzip \
    && rm -rf /var/lib/apt/lists/*

RUN --mount=type=secret,id=custom_ca,required=false \
    if [ -f /run/secrets/custom_ca ]; then \
        install -m 0644 /run/secrets/custom_ca /usr/local/share/ca-certificates/custom-ca.crt; \
        update-ca-certificates; \
    fi

RUN mkdir -p /usr/share/dotnet \
    && curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh \
    && bash /tmp/dotnet-install.sh --version ${DOTNET_SDK_VERSION} --install-dir /usr/share/dotnet \
    && rm /tmp/dotnet-install.sh

RUN test "$(node --version)" = "v${NODE_VERSION}" \
    && test "$(pnpm --version)" = "${PNPM_VERSION}"

RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc \
        | gpg --dearmor -o /usr/share/keyrings/microsoft-archive-keyring.gpg \
    && echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/repos/azure-cli/ bookworm main" \
        > /etc/apt/sources.list.d/azure-cli.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends azure-cli=${AZURE_CLI_VERSION}-1~bookworm \
    && rm -rf /var/lib/apt/lists/*

RUN case "${TARGETARCH}" in \
        amd64) BICEP_ARCH=x64 ;; \
        arm64) BICEP_ARCH=arm64 ;; \
        *) echo "Unsupported container architecture: ${TARGETARCH}" >&2; exit 1 ;; \
    esac \
    && curl -fsSL https://downloads.bicep.azure.com/${BICEP_VERSION}/bicep-linux-${BICEP_ARCH} -o /usr/local/bin/bicep \
    && chmod +x /usr/local/bin/bicep

RUN mkdir -p /tmp/dotnet-tools \
    && curl -fsSL "https://www.nuget.org/api/v2/package/dotnet-ef/${DOTNET_EF_VERSION}" \
        -o "/tmp/dotnet-tools/dotnet-ef.${DOTNET_EF_VERSION}.nupkg" \
    && dotnet tool install --tool-path /usr/local/bin dotnet-ef \
        --version ${DOTNET_EF_VERSION} \
        --source /tmp/dotnet-tools \
    && ln -s /usr/share/dotnet/dotnet /usr/local/bin/dotnet \
    && rm -rf /tmp/dotnet-tools
