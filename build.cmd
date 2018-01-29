@echo off
setlocal

.paket\paket.exe restore -g build
if errorlevel 1 exit /b %errorlevel%

.paket\paket.exe update -g wsbuild
if errorlevel 1 exit /b %errorlevel%

paket-files\wsbuild\intellifactory\websharper\tools\WebSharper.Fake.cmd %*
