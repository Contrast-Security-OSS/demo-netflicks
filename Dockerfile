# MULTI STAGE BUILD
ARG CONTRAST_AGENT_VERSION=latest
# Build stage for building the Netflicks application and it's dependencies
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG TARGETARCH

WORKDIR /src

# Copy project files and restore dependencies (leveraging Docker layer caching)
COPY *.sln .
COPY DotNetFlicks.Accessors/*.csproj ./DotNetFlicks.Accessors/
COPY DotNetFlicks.Common/*.csproj ./DotNetFlicks.Common/
COPY DotNetFlicks.Engines/*.csproj ./DotNetFlicks.Engines/
COPY DotNetFlicks.Managers/*.csproj ./DotNetFlicks.Managers/
COPY DotNetFlicks.ViewModels/*.csproj ./DotNetFlicks.ViewModels/
COPY DotNetFlicks.Web/*.csproj ./DotNetFlicks.Web/

# RUN dotnet restore "DotNetFlicks.Web/Web.csproj" --arch $TARGETARCH
RUN dotnet restore "DotNetFlicks.Web/Web.csproj" /p:Platform=$TARGETARCH

# Copy the rest of the source code and build
COPY ./DotNetFlicks.Accessors ./DotNetFlicks.Accessors
COPY ./DotNetFlicks.Common ./DotNetFlicks.Common 
COPY ./DotNetFlicks.Engines ./DotNetFlicks.Engines
COPY ./DotNetFlicks.Managers ./DotNetFlicks.Managers
COPY ./DotNetFlicks.ViewModels ./DotNetFlicks.ViewModels
COPY ./DotNetFlicks.Web ./DotNetFlicks.Web 
COPY ./DotNetFlicks.sln ./DotNetFlicks.sln
RUN dotnet publish "DotNetFlicks.Web/Web.csproj" \
    # --arch $TARGETARCH \
    /p:Platform=$TARGETARCH \
    --configuration Release \ 
    --no-restore \
    --output /app \
    --self-contained false

# Runtime stage for running the application without the Contrast agent
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

RUN apt-get update && \
    apt-get install --assume-yes --no-install-recommends \
        libnss3-tools \
        curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "DotNetFlicks.Web.dll"]

# Contrast agent image for .NET Core applications

FROM contrast/agent-dotnet-core:${CONTRAST_AGENT_VERSION} AS contrast-agent


# Final stage for running the Netflicks application with the Contrast agent
FROM runtime AS runtime-with-contrast
ARG TARGETARCH

# Copy the agent from the contrast agent image
COPY --from=contrast-agent /contrast /opt/contrast

# Workaround for architecture naming differences between .NET Core and Contrast
RUN ln -s /opt/contrast/runtimes/linux-x64 /opt/contrast/runtimes/linux-amd64

# Needs to be linux-arm64 or linux-x64 or win-x64 or win-x86
ENV CORECLR_PROFILER_PATH_64=/opt/contrast/runtimes/linux-$TARGETARCH/native/ContrastProfiler.so \
    CORECLR_PROFILER={8B2CE134-0948-48CA-A4B2-80DDAD9F5791} \
    CORECLR_ENABLE_PROFILING=1 \
    CONTRAST_CORECLR_LOGS_DIRECTORY=/opt/contrast
