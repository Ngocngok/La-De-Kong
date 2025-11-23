using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LongDK.Core;
using LongDK.Debug;

namespace LongDK.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioConfig _config;
        [SerializeField] private AudioLibrary _library; // Optional library reference

        private AudioSource _musicSourceA;
        private AudioSource _musicSourceB;
        private bool _isUsingSourceA = true;
        
        private AudioSource _uiSource;
        private Queue<AudioSource> _sfxPool;
        private Transform _poolRoot;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            if (_config == null)
            {
                Log.Error("AudioConfig is missing on AudioManager!", this);
                return;
            }

            // Create Music Sources (Double buffering for cross-fade)
            _musicSourceA = CreateSource("MusicSource_A", _config.MusicGroup, true);
            _musicSourceB = CreateSource("MusicSource_B", _config.MusicGroup, true);

            // Create UI Source (2D only)
            _uiSource = CreateSource("UI_Source", _config.UIGroup, false);

            // Initialize SFX Pool
            _poolRoot = new GameObject("SFX_Pool").transform;
            _poolRoot.SetParent(transform);
            _sfxPool = new Queue<AudioSource>();

            for (int i = 0; i < _config.InitialPoolSize; i++)
            {
                CreatePoolObject();
            }
        }

        private AudioSource CreateSource(string name, UnityEngine.Audio.AudioMixerGroup group, bool loop)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(transform);
            AudioSource source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = group;
            source.loop = loop;
            source.playOnAwake = false;
            return source;
        }

        private AudioSource CreatePoolObject()
        {
            GameObject go = new GameObject("Pooled_SFX");
            go.transform.SetParent(_poolRoot);
            AudioSource source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _config.SFXGroup;
            source.playOnAwake = false;
            source.spatialBlend = 1.0f; // 3D sound by default
            go.SetActive(false);
            _sfxPool.Enqueue(source);
            return source;
        }

        private AudioSource GetFromPool()
        {
            if (_sfxPool.Count == 0)
            {
                if (_poolRoot.childCount < _config.MaxPoolSize)
                {
                    return CreatePoolObject();
                }
                else
                {
                    Log.Warn("Audio Pool exhausted! Stealing oldest sound (not implemented yet, just returning null for safety).");
                    return null;
                }
            }

            AudioSource source = _sfxPool.Dequeue();
            source.gameObject.SetActive(true);
            return source;
        }

        private void ReturnToPool(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
            _sfxPool.Enqueue(source);
        }

        #region Music API

        public void PlayMusic(AudioClip clip, float fadeDuration = 1.0f, bool loop = true)
        {
            if (clip == null) return;

            AudioSource activeSource = _isUsingSourceA ? _musicSourceA : _musicSourceB;
            AudioSource nextSource = _isUsingSourceA ? _musicSourceB : _musicSourceA;

            // If already playing this clip, just update loop status
            if (activeSource.isPlaying && activeSource.clip == clip)
            {
                activeSource.loop = loop;
                return;
            }

            StartCoroutine(CrossFadeRoutine(activeSource, nextSource, clip, fadeDuration, loop));
            _isUsingSourceA = !_isUsingSourceA;
        }

        public void StopMusic(float fadeDuration = 1.0f)
        {
            AudioSource activeSource = _isUsingSourceA ? _musicSourceA : _musicSourceB;
            if (activeSource.isPlaying)
            {
                StartCoroutine(FadeOutRoutine(activeSource, fadeDuration));
            }
        }

        private IEnumerator CrossFadeRoutine(AudioSource current, AudioSource next, AudioClip newClip, float duration, bool loop)
        {
            next.clip = newClip;
            next.loop = loop;
            next.volume = 0;
            next.Play();

            float timer = 0;
            float startVolume = current.volume;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;

                current.volume = Mathf.Lerp(startVolume, 0, t);
                next.volume = Mathf.Lerp(0, 1, t);

                yield return null;
            }

            current.Stop();
            current.volume = 0; // Reset for next time
            next.volume = 1;
        }

        private IEnumerator FadeOutRoutine(AudioSource source, float duration)
        {
            float timer = 0;
            float startVolume = source.volume;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                source.volume = Mathf.Lerp(startVolume, 0, t);
                yield return null;
            }

            source.Stop();
            source.volume = 0;
        }

        #endregion

        #region SFX API

        public void PlaySFX(AudioClip clip)
        {
            PlaySFX(clip, 1.0f, 1.0f);
        }

        public void PlaySFX(AudioClip clip, float volume, float pitch)
        {
            // 2D SFX (UI or Global)
            if (clip == null) return;
            _uiSource.pitch = pitch;
            _uiSource.PlayOneShot(clip, volume);
            _uiSource.pitch = 1.0f; // Reset
        }

        public void PlaySFX(AudioClip clip, Vector3 position)
        {
            PlaySFX(clip, position, 1.0f, 1.0f);
        }

        public AudioSource PlaySFX(AudioClip clip, Vector3 position, float volume, float pitch)
        {
            // 3D Spatial SFX
            if (clip == null) return null;

            AudioSource source = GetFromPool();
            if (source == null) return null;

            source.transform.position = position;
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();

            StartCoroutine(ReturnToPoolRoutine(source, clip.length));
            return source;
        }

        private IEnumerator ReturnToPoolRoutine(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnToPool(source);
        }

        #endregion

        #region Library API

        public void PlaySFX(AudioID id)
        {
            if (_library == null) return;
            AudioData data = _library.Get(id);
            if (data == null) return;

            AudioClip clip = data.GetRandomClip();
            float vol = data.Volume + Random.Range(-data.VolumeVariation, data.VolumeVariation);
            float pit = data.Pitch + Random.Range(-data.PitchVariation, data.PitchVariation);

            PlaySFX(clip, vol, pit);
        }

        public AudioSource PlaySFX(AudioID id, Vector3 position)
        {
            if (_library == null) return null;
            AudioData data = _library.Get(id);
            if (data == null) return null;

            AudioClip clip = data.GetRandomClip();
            float vol = data.Volume + Random.Range(-data.VolumeVariation, data.VolumeVariation);
            float pit = data.Pitch + Random.Range(-data.PitchVariation, data.PitchVariation);

            return PlaySFX(clip, position, vol, pit);
        }

        public void PlayMusic(AudioID id, float fadeDuration = 1.0f, bool loop = true)
        {
            if (_library == null) return;
            AudioData data = _library.Get(id);
            if (data == null) return;

            PlayMusic(data.GetRandomClip(), fadeDuration, loop);
        }

        #endregion

        #region Volume API

        public void SetMusicVolume(float volume01)
        {
            SetMixerVolume(_config.MusicVolumeParam, volume01);
        }

        public void SetSFXVolume(float volume01)
        {
            SetMixerVolume(_config.SFXVolumeParam, volume01);
        }

        private void SetMixerVolume(string paramName, float volume01)
        {
            if (_config.Mixer == null) return;
            
            // Convert 0-1 to Decibels (-80db to 0db)
            float db = volume01 <= 0.001f ? -80f : Mathf.Log10(volume01) * 20f;
            _config.Mixer.SetFloat(paramName, db);
        }

        #endregion
    }
}