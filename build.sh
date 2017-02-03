#!/usr/bin/env bash

mkdir -p /usr/lib/mono/xbuild-frameworks/.NETFramework/v4.6
dotnet restore
dotnet build src/**/project.json
dotnet test tests/nFundamental.Core.Tests -f netcoreapp1.0
dotnet test tests/nFundamental.Interface.Wasapi.Tests -f netcoreapp1.0