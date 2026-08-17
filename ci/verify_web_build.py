#!/usr/bin/env python3
from pathlib import Path
import sys

if len(sys.argv) < 2: raise SystemExit('usage: verify_web_build.py <webgl-output>')
out=Path(sys.argv[1])
errors=[]
for f in ['index.html','manifest.webmanifest','service-worker.js','.nojekyll']:
    if not (out/f).is_file(): errors.append('missing '+f)
all_files=[p for p in out.rglob('*') if p.is_file()]
if not any(p.suffix=='.wasm' or '.wasm.' in p.name for p in all_files): errors.append('no WebAssembly output (.wasm)')
if not any(p.suffix=='.data' or '.data.' in p.name for p in all_files): errors.append('no Unity data output (.data)')
if not any(p.suffix=='.js' for p in all_files): errors.append('no JavaScript loader/framework output')
total=sum(p.stat().st_size for p in all_files)
if total > 120*1024*1024: errors.append(f'web build exceeds 120 MB budget: {total/1024/1024:.1f} MB')
if errors:
    print('\n'.join('ERROR: '+e for e in errors)); sys.exit(1)
print(f'EX360 WebGL verification: PASS ({len(all_files)} files, {total/1024/1024:.1f} MB)')
