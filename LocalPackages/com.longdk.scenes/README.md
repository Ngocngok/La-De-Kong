# LongDK Scene System

A "Conductor" for scene transitions.

## Features
- **Smooth Transitions:** Automatically fades out music and fades in a loading screen.
- **Progress Tracking:** Reports async loading progress (0.0 to 1.0) to the UI.
- **Minimum Load Time:** Prevents "flickering" on fast PCs by enforcing a minimum duration.
- **Garbage Collection:** Forces `GC.Collect()` during the transition to minimize lag during gameplay.

## Quick Start
1. **UI:** Create a UI Prefab that implements `ILoadingScreen` (and `UIFrame`). Register it as `Loading_Simple`.
2. **Code:**
   ```csharp
   SceneLoader.Instance.LoadScene("MyGameScene");
   ```
