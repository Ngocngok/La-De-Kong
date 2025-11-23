using UnityEngine;
using UnityEngine.Audio;

namespace LongDK.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "LongDK/Audio/Config")]
    public class AudioConfig : ScriptableObject
    {
        [Header("Mixer Settings")]
        public AudioMixer Mixer;
        public string MasterVolumeParam = "MasterVolume";
        public string MusicVolumeParam = "MusicVolume";
        public string SFXVolumeParam = "SFXVolume";
        public string UIVolumeParam = "UIVolume";

        [Header("Mixer Groups")]
        public AudioMixerGroup MasterGroup;
        public AudioMixerGroup MusicGroup;
        public AudioMixerGroup SFXGroup;
        public AudioMixerGroup UIGroup;

        [Header("Pooling")]
        public int InitialPoolSize = 10;
        public int MaxPoolSize = 30;
    }
}