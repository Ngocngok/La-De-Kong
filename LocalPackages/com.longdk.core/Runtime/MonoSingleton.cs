using UnityEngine;

namespace LongDK.Core
{
    /// <summary>
    /// A strict MonoBehaviour Singleton.
    /// - Does NOT lazy instantiate (must be in the scene).
    /// - Returns null if accessed during Application Quit.
    /// - Does NOT handle DontDestroyOnLoad (assumes external management).
    /// </summary>
    /// <typeparam name="T">The type of the singleton class.</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        private static bool _isQuitting;

        /// <summary>
        /// Gets the singleton instance.
        /// Returns null if the instance is not found in the scene or if the application is quitting.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_isQuitting)
                {
                    // Optional: Log a warning if you want to debug "who is calling me during destroy?"
                    // Debug.LogWarning($"[MonoSingleton] Instance of '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    
                    if (_instance == null)
                    {
                        // Strict mode: We do NOT create it. We just complain.
                        UnityEngine.Debug.LogError($"[MonoSingleton] Instance of '{typeof(T)}' is needed in the scene, but none was found.");
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Returns true if the instance is currently valid and active.
        /// Useful for checking existence without triggering the "Error" log in the getter.
        /// </summary>
        public static bool HasInstance => _instance != null && !_isQuitting;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                UnityEngine.Debug.LogError($"[MonoSingleton] Duplicate instance of '{typeof(T)}' detected on GameObject '{gameObject.name}'. Destroying duplicate.");
                Destroy(this);
                return;
            }

            _instance = (T)this;
        }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}