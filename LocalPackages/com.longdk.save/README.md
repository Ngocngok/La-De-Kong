# LongDK Save System

A modular, interface-driven persistence system.

## Features
- **Modular:** Swap Serializers (JSON, Binary) or Storage (File, Cloud) without changing game code.
- **Simple API:** Key-based Save/Load.
- **Debuggable:** Default implementation uses pretty-printed JSON.

## Usage

### Basic Save/Load
```csharp
using LongDK.Save;

[Serializable]
public class GameData
{
    public int Level;
    public int Coins;
}

public class GameController : MonoBehaviour
{
    void SaveGame()
    {
        var data = new GameData { Level = 5, Coins = 100 };
        SaveManager.Instance.Save("slot_1", data);
    }

    void LoadGame()
    {
        if (SaveManager.Instance.Exists("slot_1"))
        {
            var data = SaveManager.Instance.Load<GameData>("slot_1");
            Debug.Log("Loaded Level: " + data.Level);
        }
    }
}
```

### Saving Dictionaries
Unity's `JsonUtility` does not support Dictionaries. Use the provided `SerializableDictionary` wrapper.

1. Define a concrete class inheriting from `SerializableDictionary`.
2. Use that class in your data structure.

```csharp
// 1. Define concrete type
[Serializable]
public class InventoryDict : SerializableDictionary<string, int> { }

[Serializable]
public class PlayerData
{
    // 2. Use it
    public InventoryDict Inventory = new InventoryDict();
}

// Usage
data.Inventory.Add("Gold", 100);
```

### Advanced: Custom Backend
You can inject custom implementations in your initialization phase.

```csharp
public class MyCloudStorage : IStorage 
{ 
    // Implement Read/Write for Steam/AWS/PlayFab...
}

void Awake()
{
    SaveManager.Instance.SetStorage(new MyCloudStorage());
}
```
