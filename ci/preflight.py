#!/usr/bin/env python3
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
required = [
    'ProjectSettings/ProjectVersion.txt', 'Packages/manifest.json',
    'Assets/Scripts/Runtime/Core/GameBootstrap.cs', 'Assets/Editor/BuildWebGL.cs',
    'WebSupport/manifest.webmanifest', 'WebSupport/service-worker.js',
    '.github/workflows/webgl-ci.yml'
]
errors=[]
for f in required:
    if not (ROOT/f).is_file(): errors.append('missing '+f)
for forbidden in ['Library','Temp','Obj','UserSettings']:
    if (ROOT/forbidden).exists(): errors.append('generated Unity folder committed: '+forbidden)
for p in ROOT.rglob('*'):
    if p.is_file() and '.git' not in p.parts and p.stat().st_size > 50*1024*1024:
        errors.append(f'file exceeds 50 MB repo budget: {p.relative_to(ROOT)}')
text=(ROOT/'Assets/Editor/BuildWebGL.cs').read_text(encoding='utf-8')
if 'BuildTarget.WebGL' not in text or 'PerformBuild' not in text: errors.append('WebGL build method invalid')
if errors:
    print('\n'.join('ERROR: '+e for e in errors)); sys.exit(1)
print('EX360 preflight: PASS')
