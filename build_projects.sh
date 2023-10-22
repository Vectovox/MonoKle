#!/bin/bash
if ! dotnet build MonoKle -c Release
then
	echo "Build error! Aborting..."
	exit 1
fi

if ! dotnet build MonoKle.Engine -c Release
then
	echo "Build error! Aborting..."
	exit 1
fi
