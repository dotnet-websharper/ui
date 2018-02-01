@echo off
setlocal

.paket\paket.exe restore -g build
if errorlevel 1 exit /b %errorlevel%

.paket\paket.exe update -g wsbuild
if errorlevel 1 exit /b %errorlevel%

set NOT_DOTNET=true
call paket-files\wsbuild\github.com\dotnet-websharper\build-script\WebSharper.Fake.cmd %*
