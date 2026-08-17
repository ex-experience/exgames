#!/usr/bin/env python3
from pathlib import Path
import shutil, sys, json, hashlib, time

if len(sys.argv) < 2:
    raise SystemExit('usage: postprocess_web.py <webgl-output>')
out = Path(sys.argv[1]).resolve()
root = Path(__file__).resolve().parents[1]
if not (out/'index.html').is_file():
    raise SystemExit('index.html missing from Unity build output')
for name in ['manifest.webmanifest','service-worker.js','icon-192.png','icon-512.png']:
    shutil.copy2(root/'WebSupport'/name, out/name)
(out/'.nojekyll').write_text('', encoding='utf-8')
index=(out/'index.html').read_text(encoding='utf-8', errors='ignore')
head="\\n<meta name='viewport' content='width=device-width,initial-scale=1,viewport-fit=cover,user-scalable=no'>\\n<meta name='theme-color' content='#090b0e'>\\n<link rel='manifest' href='manifest.webmanifest'>\\n"
boot="\\n<script>if('serviceWorker' in navigator){window.addEventListener('load',()=>navigator.serviceWorker.register('./service-worker.js').catch(console.error));}</script>\\n"
if 'manifest.webmanifest' not in index:
    index=index.replace('</head>', head+'</head>')
if 'serviceWorker.register' not in index:
    index=index.replace('</body>', boot+'</body>')
(out/'index.html').write_text(index, encoding='utf-8')
meta={'product':'ARABIA STRIKE 360','generated_utc':int(time.time()),'pipeline':'Unity C# -> WebGL -> WASM -> GitHub Pages'}
(out/'build-meta.json').write_text(json.dumps(meta, indent=2)+'\n', encoding='utf-8')
print('EX360 web postprocess: PASS')
