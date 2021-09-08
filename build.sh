#!/bin/bash

dotnet tool restore

dotnet paket update -g wsbuild --no-install

. paket-files/wsbuild/github.com/dotnet-websharper/build-script/build.sh "$@"
