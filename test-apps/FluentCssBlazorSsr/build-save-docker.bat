@echo off
setlocal

rem Build the FluentCss static-SSR docs image and save it to a tar file, so it can
rem be copied to a server, loaded and started without a container registry.
rem
rem Usage:
rem   build-save-docker.bat            builds fluentcssdocs:latest
rem   build-save-docker.bat v1.2       builds fluentcssdocs:v1.2
rem
rem On the server:
rem   docker load --input fluentcssdocs-latest.tar
rem   docker compose up -d

set "IMAGE_NAME=fluentcssdocs"
if "%~1"=="" (set "IMAGE_TAG=latest") else (set "IMAGE_TAG=%~1")
set "IMAGE_REF=%IMAGE_NAME%:%IMAGE_TAG%"

for %%I in ("%~dp0..\..") do set "REPO_ROOT=%%~fI"
set "OUTPUT_FILE=%~dp0%IMAGE_NAME%-%IMAGE_TAG%.tar"

where docker >nul 2>&1
if errorlevel 1 (
    echo error: docker is not installed or not on PATH 1>&2
    exit /b 1
)

echo ==^> Building %IMAGE_REF%
docker build --file "%~dp0Dockerfile" --tag "%IMAGE_REF%" "%REPO_ROOT%"
if errorlevel 1 exit /b 1

echo ==^> Saving %IMAGE_REF% to %OUTPUT_FILE%
docker save "%IMAGE_REF%" --output "%OUTPUT_FILE%"
if errorlevel 1 exit /b 1

echo ==^> Done: %OUTPUT_FILE%
echo.
echo Copy the file to the server, then run:
echo   docker load --input %IMAGE_NAME%-%IMAGE_TAG%.tar
echo   docker compose up -d
