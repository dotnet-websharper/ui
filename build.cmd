@echo off
setlocal

set PATH=%GitToolPath%;%PATH%
dotnet restore
if errorlevel 1 exit /b %errorlevel%

paket-files\build\intellifactory\websharper\tools\WebSharper.Fake.cmd %*
