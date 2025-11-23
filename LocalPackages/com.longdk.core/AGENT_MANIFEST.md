# LongDK Agent Manifest

## 1. Core Philosophy
You are contributing to **LongDK**, a modular Unity SDK designed for high portability and agent-compatibility.
- **Modular:** Systems must be self-contained. Dependencies should be explicit via `package.json`.
- **Agent-First:** Code must be clean, well-documented, and follow the patterns described here so other agents can understand it without deep context.
- **Performance:** Zero-allocation in hot paths (Update loops).

## 2. Coding Conventions

### Namespace
- All code must be wrapped in the `LongDK` root namespace.
- Sub-systems use `LongDK.[SystemName]`.
  - Example: `LongDK.Core`, `LongDK.Debug`, `LongDK.UI`.

### Class Naming
- **Managers/Services:** Suffix with `Manager` or `Service` (e.g., `AudioManager`).
- **Data:** Suffix with `Data` or `Config` (e.g., `AudioConfig`).
- **Interfaces:** Prefix with `I` (e.g., `ILogger`).

### Code Style
- **Braces:** K&R style (opening brace on the same line is NOT standard C#, but for this project we use **Allman style** - opening brace on new line).
  ```csharp
  // Correct
  public void DoSomething()
  {
      if (true)
      {
          // ...
      }
  }
  ```
- **Fields:**
  - `private int _memberVariable;` (underscore prefix)
  - `public int PublicVariable;` (PascalCase)
- **Serialization:**
  - Use `[SerializeField] private` instead of `public` for Inspector variables.

## 3. Architecture Patterns

### Singleton
- Inherit from `MonoSingleton<T>` for MonoBehaviour-based singletons.
- **Strict Mode:** The singleton MUST be present in the scene. It will not be automatically created.
- **Quit Safety:** Accessing `Instance` during `OnApplicationQuit` or after destruction returns `null`.
- **Usage:**
  ```csharp
  public class GameManager : MonoSingleton<GameManager> { ... }
  ```

### Conditional Compilation
- Use `[Conditional("SYMBOL")]` for debug-only logic to ensure it is stripped from release builds.

## 4. Documentation
- Every public method must have an XML summary `/// <summary>`.
- Every system must have an `AGENT_USAGE.md` in its package root (for AI Agents).
- Every system must have a `README.md` in its package root (for Human Developers), containing setup instructions and code examples.
