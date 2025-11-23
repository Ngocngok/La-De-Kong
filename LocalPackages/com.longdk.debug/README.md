# LongDK Debug

A lightweight, production-safe logging wrapper.

## Why use this?
Standard `Debug.Log` calls remain in your build (even if they don't print to the console, the string allocation happens). This system uses `[Conditional]` attributes to completely strip logging calls from Release builds, ensuring zero performance cost.

## Setup
To see logs in the Editor or Development Builds, you **must** define the symbol:
1. Go to **Project Settings** > **Player**.
2. Scroll to **Scripting Define Symbols**.
3. Add `LONGDK_DEBUG`.
4. Click **Apply**.

## Usage

```csharp
using LongDK.Debug;

public class Player : MonoBehaviour
{
    void Start()
    {
        // Standard Log
        Log.Msg("Player spawned.");
        
        // Warning
        Log.Warn("Health is low!", this); // Pass 'this' for context clicking
        
        // Error
        Log.Error("Missing weapon reference.");
    }
}
```

## API Reference
- `Log.Msg(object)`
- `Log.Warn(object)`
- `Log.Error(object)`
