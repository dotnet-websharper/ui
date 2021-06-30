@echo off
setlocal
    
dotnet tool restore
dotnet paket update -g wsbuild --no-install
    
call paket-files\wsbuild\github.com\dotnet-websharper\build-script\update.cmd

dotnet paket restore
dotnet restore

call paket-files\wsbuild\github.com\dotnet-websharper\build-script\build.cmd %*
