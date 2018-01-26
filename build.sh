#!/bin/bash

set -e

if [ "$OS" = "Windows_NT" ]; then
    .paket/paket.exe restore -g build
else
    mono .paket/paket.exe restore -g build
fi

export NOT_DOTNET=true
paket-files/build/intellifactory/websharper/tools/WebSharper.Fake.sh "$@"
