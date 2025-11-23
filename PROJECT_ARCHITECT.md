# LongDK SDK - Project Architect Documentation

## Project Overview
LongDK is a modular, agent-compatible Unity SDK designed to be "plug and play" across different projects. The core philosophy is **Agent-First Design**, meaning every system is documented and structured specifically to be understood and implemented by AI Agents.

## Architecture
The project follows a **UPM (Unity Package Manager) Monorepo** structure.
- **Location:** All source code resides in `LocalPackages/`.
- **Format:** Each system is a standalone Unity Package (`package.json`, `asmdef`, etc.).
- **Documentation:**
    - **Global Rules:** `LocalPackages/com.longdk.core/AGENT_MANIFEST.md` contains the coding conventions, architectural patterns, and "laws" of the SDK.
    - **System Rules:** Each package contains an `AGENT_USAGE.md` file describing its specific API, dependencies, and usage patterns.

## Current Status
- **Core Package (`com.longdk.core`):** Established. Contains the `AGENT_MANIFEST.md`.
- **Debug Package (`com.longdk.debug`):** Implemented. A lightweight logging wrapper that supports conditional compilation stripping for release builds.

## For Future Agents
If you are picking up this project:
1. **Read `LocalPackages/com.longdk.core/AGENT_MANIFEST.md`** first. This is your "system prompt" for writing code in this SDK.
2. **Check `AGENT_USAGE.md`** in the specific package you are working on.
3. **Maintain Modularity:** Do not introduce tight coupling between packages unless absolutely necessary. Use Interfaces or Events defined in Core.
4. **Update Documentation:** If you change an API, you **MUST** update the corresponding `AGENT_USAGE.md`.

## Namespace Convention
- Root: `LongDK`
- Systems: `LongDK.[SystemName]` (e.g., `LongDK.Debug`, `LongDK.UI`)

## Build & Release
- The Debug system uses `[Conditional("LONGDK_DEBUG")]`. Ensure this symbol is defined in `Project Settings > Player > Scripting Define Symbols` for development builds if you want logs.
