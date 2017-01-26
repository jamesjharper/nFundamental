#!/usr/bin/env bash

mkdir -p /usr/lib/mono/xbuild-frameworks/.NETFramework/v4.6
dotnet restore
dotnet build src/**/project.json
dotnet tests tests/nFundamental.Interface.Wasapi.Tests
dotnet tests tests/nFundamental.Tests