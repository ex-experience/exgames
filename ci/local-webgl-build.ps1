param([Parameter(Mandatory=$true)][string]$UnityEditor)
$ErrorActionPreference = 'Stop'
& $UnityEditor -batchmode -quit -nographics -projectPath (Get-Location) -executeMethod EX360.Editor.BuildWebGL.PerformBuild -buildPath build/WebGL -logFile -
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
python ci/postprocess_web.py build/WebGL
python ci/verify_web_build.py build/WebGL
