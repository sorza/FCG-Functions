ARG BUILD_VERSION=1.0.0
ARG BUILD_DATE
ARG VCS_REF

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_VERSION
WORKDIR /src

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

COPY ["FCG-Functions.csproj", "./"]
RUN dotnet restore "FCG-Functions.csproj"

COPY . .
RUN dotnet publish "FCG-Functions.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:Version=${BUILD_VERSION}

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS runtime
ARG BUILD_VERSION
ARG BUILD_DATE
ARG VCS_REF

WORKDIR /home/site/wwwroot

COPY --from=build /app/publish .

LABEL org.opencontainers.image.version="${BUILD_VERSION}" \
      org.opencontainers.image.created="${BUILD_DATE}" \
      org.opencontainers.image.revision="${VCS_REF}" \
      org.opencontainers.image.title="FCG Email Function" \
      org.opencontainers.image.description="Azure Function for sending welcome emails via SendGrid" \
      org.opencontainers.image.authors="Alexandre Zordan <alexandre.zordan@outlook.com>" \
      org.opencontainers.image.source="https://github.com/alexandresorza/fcg-functions"

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:80/api/health || exit 1
