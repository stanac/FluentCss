#!/usr/bin/env bash
#
# Build the FluentCss static-SSR docs image and save it to a tar file, so it can
# be copied to a server, loaded and started without a container registry.
#
# Usage:
#   ./build-save-docker.sh            # builds fluentcssdocs:latest
#   ./build-save-docker.sh v1.2       # builds fluentcssdocs:v1.2
#
# On the server:
#   docker load --input fluentcssdocs-latest.tar
#   docker compose up -d
#
set -euo pipefail

IMAGE_NAME="fluentcssdocs"
IMAGE_TAG="${1:-latest}"
IMAGE_REF="${IMAGE_NAME}:${IMAGE_TAG}"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
OUTPUT_FILE="${SCRIPT_DIR}/${IMAGE_NAME}-${IMAGE_TAG}.tar"

if ! command -v docker >/dev/null 2>&1; then
    echo "error: docker is not installed or not on PATH" >&2
    exit 1
fi

echo "==> Building ${IMAGE_REF}"
docker build \
    --file "${SCRIPT_DIR}/Dockerfile" \
    --tag "${IMAGE_REF}" \
    "${REPO_ROOT}"

echo "==> Saving ${IMAGE_REF} to ${OUTPUT_FILE}"
docker save "${IMAGE_REF}" --output "${OUTPUT_FILE}"

SIZE="$(du -h "${OUTPUT_FILE}" | cut -f1)"
echo "==> Done: ${OUTPUT_FILE} (${SIZE})"
echo
echo "Copy the file to the server, then run:"
echo "  docker load --input $(basename "${OUTPUT_FILE}")"
echo "  docker compose up -d"
