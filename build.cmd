@echo off
setlocal
set PATH=%GitToolPath%;%PATH%

cls

dotnet restore
if errorlevel 1 (
  exit /b %errorlevel%
)

packages\build\FAKE\tools\FAKE.exe build.fsx %*
