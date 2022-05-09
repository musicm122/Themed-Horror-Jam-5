#!/usr/bin/env bash
# Because our add-on has memory leaks by default our build fails but the export completes fine. This script is a workaround for failing the build.
# this should be run in the root of the godot project (specifically where the project file is)

cd "/x/Projects/Godot/ThemedHorrorJam5/"
if test -f 'export'; then
    echo "=============================================================="
    echo "Removing export directory"
    echo "=============================================================="
    rm -r 'export'
fi