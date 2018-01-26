@echo off
setlocal

.paket\paket.exe restore -g build
if errorlevel 1 exit /b %errorlevel%

set NOT_DOTNET=true
paket-files\build\intellifactory\websharper\tools\WebSharper.Fake.cmd %*
