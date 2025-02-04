@echo off
setlocal

dotnet tool restore

dotnet paket update -g wsbuild --no-install
if errorlevel 1 exit /b %errorlevel%

dotnet fsi ./build.fsx -t %*