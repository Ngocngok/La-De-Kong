# LongDK Scene System

## Role
Manages asynchronous scene loading, ensuring smooth transitions by coordinating with the UI (Loading Screens) and Audio (Music Fading) systems.

## Dependencies
- `com.longdk.core`
- `com.longdk.ui`
- `com.longdk.audio`

## Core Components
- `SceneLoader`: Singleton entry point.
- `ILoadingScreen`: Interface that any Loading UI must implement to receive progress updates.

## Usage

### 1. Setup
1. Ensure your scenes are added to **File > Build Settings**.
2. Create a Loading Screen UI Prefab:
   - Inherit from `UIFrame`.
   - Implement `ILoadingScreen`.
   - Add it to your `UIRegistry` with ID `Loading_Simple` (or your custom ID).
3. Ensure `SceneLoader` is in your scene (or created at runtime).

### 2. Loading a Scene
```csharp
// Load "Level_1" using the default loading screen
SceneLoader.Instance.LoadScene("Level_1");

// Load with a specific loading screen ID
SceneLoader.Instance.LoadScene("Level_1", UI_ID.Loading_Tips);
```

### 3. Creating a Loading Screen Script
```csharp
public class MyLoadingScreen : UIFrame, ILoadingScreen
{
    public Image ProgressBar;

    public void SetProgress(float progress)
    {
        ProgressBar.fillAmount = progress;
    }
}
```
