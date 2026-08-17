# Deploy ARABIA STRIKE V20.2 on GitHub Pages

## Recommended deployment: branch root

This repository requires no build process.

### Step 1 — New repository

Create a new repository. It can be Public or Private depending on the GitHub
plan and Pages settings available to the account.

### Step 2 — Upload package

Upload the full contents of this package directly into the repository root.

After upload, the repository root should contain at least:

```text
index.html
README.md
VERSION.txt
.nojekyll
COPYRIGHT_NOTICE.md
docs/
```

### Step 3 — Enable GitHub Pages

Go to:

**Settings → Pages**

Set:

```text
Source: Deploy from a branch
Branch: main
Folder: /(root)
```

Save.

### Step 4 — Verify

Open the GitHub Pages URL.

The loading status should eventually display:

```text
V20.2 READY
```

Then press:

```text
START MISSION
```

## No extra tools required

You do not need:

- Node.js
- npm
- Python server
- PowerShell deploy script
- Vite
- Webpack
- GitHub Actions

## Cache note

This repository intentionally does not ship a Service Worker during the active
development phase. That reduces the risk that an old game version remains
cached after a repository update.
