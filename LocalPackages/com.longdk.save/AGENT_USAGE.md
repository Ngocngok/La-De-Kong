# LongDK Save System

## Role
Provides a generic, key-based persistence layer. It is decoupled from specific file formats or storage locations via `ISerializer` and `IStorage` interfaces.

## Dependencies
- `com.longdk.core`
- `com.longdk.debug`

## Core Components
- `SaveManager`: The main entry point.
- `ISerializer`: Interface for converting objects to/from string/bytes.
- `IStorage`: Interface for writing/reading data to a medium (Disk, Cloud, etc.).

## Usage

### 1. Basic Saving & Loading
The default setup uses **JSON Serialization** and **File Storage** (Application.persistentDataPath).

```csharp
[Serializable]
public class PlayerData
{
    public int Health;
    public string Name;
}

// Save
var data = new PlayerData { Health = 100, Name = "Hero" };
SaveManager.Instance.Save("player_save", data);

// Load
if (SaveManager.Instance.Exists("player_save"))
{
    var loadedData = SaveManager.Instance.Load<PlayerData>("player_save");
    Debug.Log(loadedData.Name);
}
```

### 2. Custom Configuration
You can swap the backend in `Awake` before using the system, or create a custom initializer.

```csharp
// Example: Swapping to a Binary Serializer (if you implemented one)
SaveManager.Instance.SetSerializer(new MyBinarySerializer());
```

## Architecture
- **Key-Based:** Data is stored under a unique string key (which usually becomes the filename).
- **Synchronous:** The current implementation is synchronous (blocking). For large datasets, consider implementing an Async storage backend.
- **Dictionaries:** Use `SerializableDictionary<TKey, TValue>` to save dictionary data, as `JsonUtility` does not support native Dictionaries. You MUST create a concrete class (e.g., `class MyDict : SerializableDictionary<string, int> {}`) for it to serialize correctly.
