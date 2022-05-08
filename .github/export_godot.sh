#!/usr/bin/env bash
# Because our add-on has memory leaks by default our build fails but the export completes fine. This script is a workaround for failing the build. 
# this should be run in the root of the godot project (specifically where the project file is)
CURRENTDIR=`pwd`
EXPORTTYPE= "HTML5"
EXPORTFILEPATH= "export/index.html"
echo "current directory is $CURRENTDIR"

# Run the build but, ignore the error code
godot -v --export $EXPORTTYPE $EXPORTFILEPATH --verbose || true

#if we have an index.html then huzah we have a successful export.
if test -f $EXPORTFILEPATH; then
    echo "$EXPORTFILEPATH exists"
    exit 0
else
    echo "godot did not export successfully"
    echo "CMD: godot -v --export $EXPORTTYPE $EXPORTFILEPATH --verbose || true"
    exit 1
fi