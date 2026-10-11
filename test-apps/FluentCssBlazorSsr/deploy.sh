#!/usr/bin/env bash
#
# Deploy a saved FluentCss image on the server.
# Copy this script next to docker-compose.yml and run it there.
#
set -euo pipefail

# ---------------------------------------------------------------------------
# CHANGE ME: path to the image archive on the server.
# ---------------------------------------------------------------------------
TAR_FILE="/path/to/fluentcssdocs-latest.tar"

# Image reference used by docker-compose.yml.
IMAGE_NAME="fluentcssdocs"
COMPOSE_REF="${IMAGE_NAME}:latest"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if ! command -v docker >/dev/null 2>&1; then
    echo "error: docker is not installed or not on PATH" >&2
    exit 1
fi

if ! docker compose version >/dev/null 2>&1; then
    echo "error: the docker compose plugin is not available" >&2
    exit 1
fi

if [ ! -f "${TAR_FILE}" ]; then
    echo "error: image archive not found: ${TAR_FILE}" >&2
    exit 1
fi

echo "==> Loading ${TAR_FILE}"
LOAD_OUTPUT="$(docker load --input "${TAR_FILE}")"
echo "${LOAD_OUTPUT}"

# The archive may be tagged with a version (fluentcssdocs:v1.2). Point the tag
# that docker-compose.yml uses (fluentcssdocs:latest) at the freshly loaded image.
LOADED_REF="$(printf '%s\n' "${LOAD_OUTPUT}" | sed -n 's/^Loaded image: //p' | head -n 1)"
if [ -n "${LOADED_REF}" ] && [ "${LOADED_REF}" != "${COMPOSE_REF}" ]; then
    echo "==> Tagging ${LOADED_REF} as ${COMPOSE_REF}"
    docker tag "${LOADED_REF}" "${COMPOSE_REF}"
fi

echo "==> Recreating compose stack"
cd "${SCRIPT_DIR}"
docker compose up --detach --force-recreate --remove-orphans

# Drop the previous image, now untagged, to reclaim disk space on the server.
docker image prune --force >/dev/null

echo
echo "==> Deployed. Status:"
docker compose ps
