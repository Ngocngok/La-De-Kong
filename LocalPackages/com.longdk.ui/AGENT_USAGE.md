# LongDK UI System

## Role
Manages UI instantiation, layering, navigation history (stack), and decoupled animations.

## Dependencies
- `com.longdk.core`
- `com.longdk.debug`

## Core Components
- `UIManager`: Singleton entry point.
- `UIFrame`: Base class for all UI panels.
- `UIAnimator`: Component for handling Show/Hide animations.

## Usage

### 1. Setup
1. Create a `UIRegistry` asset (Create > LongDK > UI > Registry).
2. Use the custom inspector to add UI IDs (e.g., "MainMenu", "Inventory").
3. Create your UI Prefabs. They MUST have a script inheriting from `UIFrame`.
4. Assign the prefabs to the Registry.
5. Drag the `UIManager` prefab into your scene (or ensure it's created at runtime).

### 2. Creating a Window
```csharp
public class InventoryWindow : UIFrame
{
    public override void OnShow()
    {
        // Refresh data
    }
}
```

### 3. Controlling UI
```csharp
// Open a window (adds to stack)
UIManager.Instance.Show(UI_ID.Inventory);

// Open with Setup Data (Type-Safe)
UIManager.Instance.Show<InventoryWindow>(UI_ID.Inventory, (window) => 
{
    window.SetupData(myData);
});

// Go back (closes top window)
UIManager.Instance.Back();

// Show a popup (does not affect stack of underlying windows)
UIManager.Instance.Show(UI_ID.ConfirmationPopup);
```

### 4. Animations
Add a `SimpleFadeAnimator` (or custom `UIAnimator`) component to your UI Prefab. The `UIFrame` will automatically find it and use it for Show/Hide transitions.
