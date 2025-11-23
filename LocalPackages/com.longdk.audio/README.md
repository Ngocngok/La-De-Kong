# LongDK Audio System

A professional audio architecture supporting Mixer groups, 3D spatial pooling, cross-fading music, and a type-safe Library system.

## Setup

1. **Create Mixer:** Create a Unity AudioMixer and expose parameters named `MasterVolume`, `MusicVolume`, `SFXVolume`, `UIVolume`.
2. **Create Config:** Right-click `Create > LongDK > Audio > Config`. Assign your Mixer and Groups.
3. **Create Library:** Right-click `Create > LongDK > Audio > Library`.
4. **Scene Setup:** Create a GameObject with the `AudioManager` component. Assign the Config and Library.

## Features & Usage

### 1. The Audio Library (Recommended)
Avoid "Magic Strings" by generating an Enum for your sounds.
1. Select your `AudioLibrary` asset.
2. In the Inspector, type a name (e.g., "Explosion") and select "SFX".
3. Click **Add & Generate**.
4. Wait for compilation.
5. Assign clips to the new entry in the list.

```csharp
// Play using the generated ID
AudioManager.Instance.PlaySFX(AudioID.Explosion);
```

### 2. Music Cross-Fading
Automatically fades out the current track and fades in the new one.
```csharp
// Play "BattleTheme" with a 2-second cross-fade, looping enabled
AudioManager.Instance.PlayMusic(AudioID.BattleTheme, fadeDuration: 2.0f, loop: true);

// Stop music
AudioManager.Instance.StopMusic(fadeDuration: 1.0f);
```

### 3. 3D Spatial Audio
Plays a sound at a specific location using a pooled AudioSource.
```csharp
// Returns the AudioSource in case you need to stop it manually (e.g., looping engine sound)
var source = AudioManager.Instance.PlaySFX(AudioID.CarEngine, transform.position);
```

### 4. Volume Control
Controls the Mixer groups (converts 0-1 linear value to Decibels).
```csharp
AudioManager.Instance.SetMusicVolume(0.8f);
AudioManager.Instance.SetSFXVolume(1.0f);
```
