# LongDK Core

The foundation of the LongDK SDK. This package contains shared rules, patterns, and utilities used by all other modules.

## Features

### 1. MonoSingleton
A robust, safety-first implementation of the Singleton pattern for MonoBehaviours.
- **Strict Mode:** Does not lazy-instantiate. The object *must* exist in the scene.
- **Quit Safety:** Prevents "ghost object" creation during application shutdown.
- **Duplicate Protection:** Automatically destroys duplicate instances.

**Usage:**
```csharp
using LongDK.Core;

public class GameManager : MonoSingleton<GameManager>
{
    public void StartGame() { ... }
}

// Access
GameManager.Instance.StartGame();
```

### 2. Agent Manifest
Contains the `AGENT_MANIFEST.md`, a set of "Laws" for AI Agents contributing to this project. If you are using an LLM to write code for this SDK, feed it that file first.

## Installation
Add via Unity Package Manager using the Git URL:
`https://github.com/YourRepo/LongDK.git?path=/LocalPackages/com.longdk.core`
