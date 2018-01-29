@echo off
setlocal

.paket\paket.exe restore -g build
if errorlevel 1 exit /b %errorlevel%

.paket\paket.exe restore -g wsbuild
if errorlevel 1 exit /b %errorlevel%

set NOT_DOTNET=true
paket-files\wsbuild\intellifactory\websharper\tools\WebSharper.Fake.cmd %*
