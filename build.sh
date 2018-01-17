#!/bin/bash

set -e

dotnet restore
paket-files/build/intellifactory/websharper/tools/WebSharper.Fake.sh "$@"
