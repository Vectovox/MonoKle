assert_git_clean()
{
    if [ -z "$(git status --porcelain)" ]; then 
        echo "Git clean, continuing..."
    else 
        echo "Git not clean! Aborting..."
        exit 1
    fi
}

assert_yes()
{
    read -p "(y/n) " -n 1 -r
    echo    # (optional) move to a new line
    if [[ $REPLY =~ ^[Yy]$ ]]
    then
        echo "Ok! Continuing..."
    else
        echo "Ok! Aborting..."
        exit 2
    fi
}

## Main script ##
echo "Checking that git is clean..."
assert_git_clean

echo 
echo "Displaying current version..."
nbgv get-version
VERSION_LINE=$(nbgv get-version | grep NpmPackageVersion)
VERSION=${VERSION_LINE:30}

echo
echo "Nuget version will be: $VERSION"
echo "Is this REALLY what you want to build and publish?"
assert_yes

echo 
echo "Cleaning..."
./clean_projects.sh

echo 
echo "Building..."
./build_projects.sh

echo 
echo "Pushing..."
./push-nuget-version.sh $VERSION