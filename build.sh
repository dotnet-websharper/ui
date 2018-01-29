#!/bin/bash

set -e

if [ "$OS" = "Windows_NT" ]; then
    .paket/paket.exe restore -g build
    .paket/paket.exe update -g wsbuild
    .paket/paket.exe restore -g build
else
    mono .paket/paket.exe restore -g build
    mono .paket/paket.exe update -g wsbuild
    mono .paket/paket.exe restore -g build
fi

paket-files/wsbuild/intellifactory/websharper/tools/WebSharper.Fake.sh "$@"
