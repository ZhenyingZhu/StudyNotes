FROM mcr.microsoft.com/devcontainers/base:ubuntu-24.04

ARG DOTNET_SDK_VERSION=10.0.100
ARG DOTNET_EF_VERSION=10.0.0
ARG NODE_VERSION=22.17.1
ARG PNPM_VERSION=10.15.0
ARG AZURE_CLI_VERSION=2.78.0
ARG BICEP_VERSION=v0.37.4

ENV DEBIAN_FRONTEND=noninteractive
ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH=/usr/share/dotnet:/home/vscode/.dotnet/tools:/usr/local/bin:${PATH}

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
        xz-utils \
    && rm -rf /var/lib/apt/lists/*

RUN mkdir -p /usr/share/dotnet \
    && curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh \
    && bash /tmp/dotnet-install.sh --version ${DOTNET_SDK_VERSION} --install-dir /usr/share/dotnet \
    && rm /tmp/dotnet-install.sh

RUN curl -fsSL https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.xz -o /tmp/node.tar.xz \
    && tar -xJf /tmp/node.tar.xz -C /usr/local --strip-components=1 \
    && rm /tmp/node.tar.xz \
    && corepack enable \
    && npm install --global pnpm@${PNPM_VERSION}

RUN python3 -m venv /opt/azure-cli \
    && /opt/azure-cli/bin/pip install --no-cache-dir --upgrade pip \
    && /opt/azure-cli/bin/pip install --no-cache-dir azure-cli==${AZURE_CLI_VERSION} \
    && ln -s /opt/azure-cli/bin/az /usr/local/bin/az

RUN curl -fsSL https://github.com/Azure/bicep/releases/download/${BICEP_VERSION}/bicep-linux-x64 -o /usr/local/bin/bicep \
    && chmod +x /usr/local/bin/bicep

USER vscode

RUN dotnet tool install --global dotnet-ef --version ${DOTNET_EF_VERSION}
