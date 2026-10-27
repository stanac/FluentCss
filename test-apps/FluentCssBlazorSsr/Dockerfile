# syntax=docker/dockerfile:1

# ---------------------------------------------------------------------------
# Build stage
# The build context is the repository root (see docker-compose.yml / the
# build-save-docker.sh script), because the SSR app references both src/*
# and the shared FluentCss.Docs Razor Class Library.
# ---------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore layer: copy only the project files first so Docker can cache the
# NuGet restore and only re-run it when a project file changes.
COPY src/FluentCss.Core/FluentCss.Core.csproj src/FluentCss.Core/
COPY src/FluentCss/FluentCss.csproj src/FluentCss/
COPY test-apps/FluentCss.Docs/FluentCss.Docs.csproj test-apps/FluentCss.Docs/
COPY test-apps/FluentCssBlazorSsr/FluentCssBlazorSsr.csproj test-apps/FluentCssBlazorSsr/
RUN dotnet restore test-apps/FluentCssBlazorSsr/FluentCssBlazorSsr.csproj

# Copy the remaining sources and publish.
COPY . .
RUN dotnet publish test-apps/FluentCssBlazorSsr/FluentCssBlazorSsr.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ---------------------------------------------------------------------------
# Runtime stage
# ---------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production

# The .NET 10 base image already sets ASPNETCORE_HTTP_PORTS=8080.
EXPOSE 8080

COPY --from=build /app/publish .

# Run as the non-root user provided by the base image.
USER $APP_UID
ENTRYPOINT ["dotnet", "FluentCssBlazorSsr.dll"]
