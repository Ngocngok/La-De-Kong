# LongDK Debug System

## Role
Provides a centralized logging mechanism that can be stripped from release builds using conditional compilation.

## Dependencies
- `com.longdk.core`

## Core Components
- `LongDK.Debug.Log`: The main static entry point for logging.

## Usage

### Basic Logging
```csharp
using LongDK.Debug;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Only executes if LONGDK_DEBUG is defined
        Log.Msg("System initialized.");
        Log.Warn("Something looks fishy.");
        Log.Error("Critical failure!");
    }
}
```

### Enabling Logs
To see logs in the console, you must define the symbol `LONGDK_DEBUG`.
1. Go to **Project Settings** > **Player**.
2. Navigate to **Scripting Define Symbols**.
3. Add `LONGDK_DEBUG`.
4. Click **Apply**.

## Anti-Patterns
- **DO NOT** use `UnityEngine.Debug.Log` directly in production code. It cannot be easily stripped.
- **DO NOT** perform expensive string concatenation inside the Log call if you expect it to be stripped.
  - *Bad:* `Log.Msg("Data: " + ExpensiveJsonConvert(data));` (The conversion still runs even if the call is stripped? No, actually `[Conditional]` strips the call site, so arguments are not evaluated. But be careful with side effects).
