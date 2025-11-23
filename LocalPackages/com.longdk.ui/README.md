# LongDK UI System

A professional, stack-based UI architecture for Unity UGUI.

## Features
- **Layered Sorting:** Automatically sorts UI into Background, Gameplay, Windows, Popups, and System layers.
- **Navigation Stack:** Built-in "Back" functionality.
- **Decoupled Animation:** Plug-and-play animation components (`SimpleFadeAnimator`, etc.).
- **Type-Safe:** Uses generated Enums (`UI_ID`) for referencing windows.

## Quick Start

1. **Registry:** Create a `UIRegistry` asset. Add a new ID "MainMenu".
2. **Prefab:** Create a UI Panel. Add a script that inherits from `UIFrame`. Add a `SimpleFadeAnimator` component.
3. **Link:** Assign the Prefab to the "MainMenu" slot in the Registry.
4. **Manager:** Add `UIManager` to your scene. Assign the Registry.
5. **Code:**
   ```csharp
   // Simple
   UIManager.Instance.Show(UI_ID.MainMenu);
   
   // With Setup
   UIManager.Instance.Show<MainMenu>(UI_ID.MainMenu, (menu) => 
   {
       menu.SetUsername("Player1");
   });
   ```
