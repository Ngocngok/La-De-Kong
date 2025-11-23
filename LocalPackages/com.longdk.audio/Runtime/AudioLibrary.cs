using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongDK.Audio
{
    [Serializable]
    public class AudioData
    {
        public AudioID ID;
        public List<AudioClip> Clips = new List<AudioClip>();
        [Range(0f, 1f)] public float Volume = 1.0f;
        [Range(0.1f, 3f)] public float Pitch = 1.0f;
        [Range(0f, 0.5f)] public float PitchVariation = 0.0f; // Random +/- pitch
        [Range(0f, 0.2f)] public float VolumeVariation = 0.0f; // Random +/- volume

        public AudioClip GetRandomClip()
        {
            if (Clips == null || Clips.Count == 0) return null;
            return Clips[UnityEngine.Random.Range(0, Clips.Count)];
        }
    }

    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "LongDK/Audio/Library")]
    public class AudioLibrary : ScriptableObject
    {
        public List<AudioData> Music = new List<AudioData>();
        public List<AudioData> SFX = new List<AudioData>();
        public List<AudioData> UI = new List<AudioData>();

        private Dictionary<AudioID, AudioData> _lookup;

        public void Initialize()
        {
            _lookup = new Dictionary<AudioID, AudioData>();
            AddToLookup(Music);
            AddToLookup(SFX);
            AddToLookup(UI);
        }

        private void AddToLookup(List<AudioData> list)
        {
            foreach (var item in list)
            {
                if (!_lookup.ContainsKey(item.ID))
                {
                    _lookup.Add(item.ID, item);
                }
            }
        }

        public AudioData Get(AudioID id)
        {
            if (_lookup == null) Initialize();
            
            if (_lookup.TryGetValue(id, out var data))
            {
                return data;
            }
            
            // Fallback if dictionary is out of sync (e.g. in Editor)
            return FindInLists(id);
        }

        private AudioData FindInLists(AudioID id)
        {
            var data = SFX.Find(x => x.ID == id);
            if (data != null) return data;

            data = Music.Find(x => x.ID == id);
            if (data != null) return data;

            data = UI.Find(x => x.ID == id);
            return data;
        }
    }
}