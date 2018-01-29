#!/bin/bash

set -e

if [ "$OS" = "Windows_NT" ]; then
    .paket/paket.exe restore -g build
    .paket/paket.exe update -g wsbuild
else
    mono .paket/paket.exe restore -g build
    mono .paket/paket.exe update -g wsbuild
fi

export NOT_DOTNET=true
paket-files/wsbuild/intellifactory/websharper/tools/WebSharper.Fake.sh "$@"
