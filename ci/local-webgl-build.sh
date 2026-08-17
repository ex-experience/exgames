#!/usr/bin/env bash
set -euo pipefail
: "${UNITY_EDITOR:?Set UNITY_EDITOR to the Unity executable path}"
"$UNITY_EDITOR" -batchmode -quit -nographics -projectPath "$(pwd)" \
  -executeMethod EX360.Editor.BuildWebGL.PerformBuild -buildPath build/WebGL -logFile -
python3 ci/postprocess_web.py build/WebGL
python3 ci/verify_web_build.py build/WebGL
