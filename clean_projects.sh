#!/bin/bash
dotnet clean MonoKle -c Release
rm -rf MonoKle/bin
dotnet clean MonoKle.Engine -c Release
rm -rf MonoKle.Engine/bin
dotnet clean MonoKle.Utilities -c Release
rm -rf MonoKle.Utilities/bin
