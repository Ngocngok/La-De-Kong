# LongDK SDK

A modular, agent-compatible Unity SDK designed for rapid development and scalability.

## Overview
LongDK is built as a **UPM Monorepo**. It consists of several independent packages that can be installed individually or as a set.

### Modules
- **Core (`com.longdk.core`):** Foundation, Rules, Singletons.
- **Debug (`com.longdk.debug`):** Performance-safe logging (strippable).
- **Audio (`com.longdk.audio`):** Mixer-based architecture, 3D Pooling, Library System.
- **Save (`com.longdk.save`):** Modular persistence (JSON/File) with Dictionary support.
- **UI (`com.longdk.ui`):** Stack-based, Layered UI with generic setup callbacks.
- **Scenes (`com.longdk.scenes`):** Transition orchestration with loading screens.

## Installation

### Option 1: Local Development (Clone)
1. Clone this repository.
2. Open the project in Unity.
3. The packages are located in `LocalPackages/` and are auto-loaded via `Packages/manifest.json`.

### Option 2: Install into Another Project (Git URL)
You can install specific packages directly from GitHub into your Unity project's Package Manager.

**Core (Required by all):**
`https://github.com/Ngocngok/La-De-Kong.git?path=/LocalPackages/com.longdk.core`

**Debug:**
`https://github.com/Ngocngok/La-De-Kong.git?path=/LocalPackages/com.longdk.debug`

**Audio:**
`https://github.com/Ngocngok/La-De-Kong.git?path=/LocalPackages/com.longdk.audio`

**Save:**
`https://github.com/Ngocngok/La-De-Kong.git?path=/LocalPackages/com.longdk.save`

**UI:**
`https://github.com/Ngocngok/La-De-Kong.git?path=/LocalPackages/com.longdk.ui`

**Scenes:**
`https://github.com/Ngocngok/La-De-Kong.git?path=/LocalPackages/com.longdk.scenes`

## For AI Agents
If you are an AI Agent working on this project, please read `PROJECT_ARCHITECT.md` and `LocalPackages/com.longdk.core/AGENT_MANIFEST.md` before writing any code.
