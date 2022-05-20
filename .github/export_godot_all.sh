#!/usr/bin/env bash
# Because our add-on has memory leaks by default our build fails but the export completes fine. This script is a workaround for failing the build.
# this should be run in the root of the godot project (specifically where the project file is)

cd "/x/Projects/Godot/ThemedHorrorJam5/"
if test -f 'export'; then
    echo "=============================================================="
    echo "Removing export directory"
    echo "=============================================================="
    echo "="
    echo "="
    rm -r 'export'
fi

#apt-get update && apt-get install -y rsync
#zip -r filename.zip folder
exportData=("WindowsDesktop" "MacOSX" "Linux")

#EXPORTTYPE= ("HTML5" "WindowsDesktop" "MacOSX" "Linux/X11")
#EXPORTFILEPATH= "export/index.html"


#pwd
#ls -a
echo "=============================================================="
echo "ExportData: $exportData"
echo "=============================================================="
echo "="
echo "="

for exportType in ${exportData[@]}; do
    echo "="
    echo "="
    echo "=============================================================="
    echo "exportType: $exportType"
    echo "=============================================================="
    echo "="
    echo "="
    mkdir -p "./export/$exportType"
    ExportPath="./export/$exportType"
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
    godot -v --export-pack $exportType "$ExportPath/$exportType.zip" --no-window --quiet || true

    #if we have an index.html then huzah we have a successful export.
    if test -f "$ExportPath/$exportType.zip"; then
        echo "Godot export: $exportType successfully export to $ExportPath/$exportType.zip"
    else
        echo "Godot export: $exportType did not successfully export to $ExportPath/$exportType.zip"
        echo "godot -v --export-pack $exportType "$ExportPath/$exportType.zip" --no-window --quiet || true"
        exit 1
    fi
done