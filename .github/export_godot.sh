#!/usr/bin/env bash
# Because our add-on has memory leaks by default our build fails but the export completes fine. This script is a workaround for failing the build.
# this should be run in the root of the godot project (specifically where the project file is)

#apt-get update && apt-get install -y rsync
#zip -r filename.zip folder
#"/x/Projects/Godot/ThemedHorrorJam5"

echo "argument:0 = $0"
echo "argument:1 = $1"
echo "argument:2 = $2"

#if [ -z ${var+x} ]; then echo "var is unset"; else echo "var is set to '$var'"; fi

if ! [ -n "$1" ]; then 
    echo "Godot export missing required 'exportType' argument"
    exit 1
fi

exportType=$1
projectRootPath=$(pwd)

if [ -n "$2" ]; then 
    projectRootPath=$2
fi

echo "projectRootPath set to $projectRootPath"
cd $projectRootPath

echo "=============================================================="
echo "running Godot export with arg $exportType"
echo "=============================================================="
echo ""
echo "=============================================================="
echo "Creating export "
echo "=============================================================="
mkdir -p "$projectRootPath/export/$exportType"
ExportPath="$projectRootPath/export/$exportType"
echo ""

echo "=============================================================="
echo "Current Directory: "
echo "=============================================================="
pwd
echo "=============================================================="
echo "godot -v --export-pack $exportType "$ExportPath/$exportType.zip" --no-window --quiet || true"
echo "=============================================================="
echo "="
echo "="
# Run the build but, ignore the error code
godot -v --export $exportType --no-window --quiet || true

#if we have an index.html then huzah we have a successful export.
if test -f "$ExportPath/$exportType.zip"; then
    echo "Godot export: $exportType successfully export to $ExportPath/$exportType.zip"
else
    echo "Godot export: $exportType did not successfully export to $ExportPath/$exportType.zip"
    echo "godot -v --export $exportType --no-window --quiet || true"
    exit 1
fi

ls $ExportPath/