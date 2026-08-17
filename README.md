# ARABIA STRIKE V20.2 — NIGHT SIEGE DENSITY PASS

**ARABIA STRIKE / إريبيا سترايك**  
**EX™ — ENGINEERING HUMAN EXPERIENCES.**

A standalone original browser run-and-gun game packaged for direct hosting on
GitHub Pages.

## Play architecture

- `index.html` is the complete game.
- No Node.js.
- No npm.
- No build step.
- No CDN.
- No runtime network dependency.
- No ES-module server requirement.
- Desktop keyboard + touch controls.
- Designed to run directly from GitHub Pages.

## V20.2 highlights

- Old City → Night Market → Desert → Command Zone progression.
- Dense multi-level combat.
- 10 elevated combat surfaces.
- 10 rooftop threats.
- Air-raider entrance attacks.
- Air enemies can be hit before landing.
- Airborne downward fire.
- Civilian rescue exit animation.
- Rescue reward drops.
- HMG / grenade / shotgun reward rhythm.
- Hummer combat.
- Attack helicopter encounter.
- Multi-phase COMMAND MECH.
- V20/V20.1 art, FX, HUD and audio master work retained.
- Original ARABIA STRIKE visual and gameplay assets.

## Controls

| Action | Keyboard |
|---|---|
| Move left | `A` / Left Arrow |
| Move right | `D` / Right Arrow |
| Jump | `W` / Up Arrow / Space |
| Crouch | `S` / Down Arrow |
| Fire | `J` / `F` |
| Grenade | `K` / `G` |
| Use / Hummer enter-exit | `E` |
| Aim upward | `I` / `U` |
| Fire downward in air | `S/Down + Fire` |

Touch controls are included inside the game.

## Create a new GitHub repository

1. Create a **new empty repository** on GitHub.
2. Upload **all files and folders from this package to the repository root**.
3. Commit them to `main`.
4. Open **Settings → Pages**.
5. Under **Build and deployment**, choose **Deploy from a branch**.
6. Select:
   - Branch: `main`
   - Folder: `/(root)`
7. Save.
8. GitHub Pages will serve the game from the repository URL.

The root file must remain named:

```text
index.html
```

## Important

Do not place `index.html` inside another folder when you want the repository
root to be the GitHub Pages site.

The package intentionally does **not** use a Service Worker in order to avoid
stale-cache deployment problems during active development.

## Release

`V20.2 — NIGHT SIEGE DENSITY PASS`

See:

- `docs/RELEASE_NOTES.md`
- `docs/QA_REPORT.txt`
- `docs/DEPLOY_GITHUB_PAGES.md`
