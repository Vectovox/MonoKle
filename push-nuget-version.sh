#!/bin/bash
push_package()
{
	PACKAGE=$1
	echo "Pushing package $PACKAGE"
	dotnet nuget push $PACKAGE -s https://nuget.pkg.github.com/Vectovox/index.json
}

# Main body
if [ -z "$1" ]
then
  echo "You need to provide a version (e.g. 0.3.5-alpha.001)"
else
  VERSION=$1
  echo "Pushing version: $VERSION"
  push_package ./MonoKle/bin/Release/MonoKle.$VERSION.nupkg
  push_package ./MonoKle.Engine/bin/Release/MonoKle.Engine.$VERSION.nupkg
  push_package ./MonoKle.Utilities/bin/Release/MonoKle.Utilities.$VERSION.nupkg
fi
