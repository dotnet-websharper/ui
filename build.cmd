@echo off
setlocal

.paket\paket.exe restore -g build
if errorlevel 1 exit /b %errorlevel%

paket-files\build\intellifactory\websharper\tools\WebSharper.Fake.cmd %*
