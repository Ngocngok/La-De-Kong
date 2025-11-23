using UnityEngine;
using LongDK.Core;
using LongDK.Debug;

namespace LongDK.Save
{
    public class SaveManager : MonoSingleton<SaveManager>
    {
        private ISerializer _serializer;
        private IStorage _storage;

        protected override void Awake()
        {
            base.Awake();
            
            // Default Configuration
            // Users can override this by calling SetSerializer/SetStorage in their own Awake (if execution order permits)
            // or via a custom Initialize method.
            if (_serializer == null) _serializer = new JsonSerializer();
            if (_storage == null) _storage = new FileStorage();
        }

        public void SetSerializer(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void SetStorage(IStorage storage)
        {
            _storage = storage;
        }

        public void Save<T>(string key, T data)
        {
            if (data == null)
            {
                Log.Error($"Cannot save null data for key: {key}");
                return;
            }

            string serialized = _serializer.Serialize(data);
            _storage.Write(key, serialized);
        }

        public T Load<T>(string key)
        {
            if (!_storage.Exists(key))
            {
                return default(T);
            }

            string serialized = _storage.Read(key);
            if (string.IsNullOrEmpty(serialized))
            {
                return default(T);
            }

            return _serializer.Deserialize<T>(serialized);
        }

        public bool Exists(string key)
        {
            return _storage.Exists(key);
        }

        public void Delete(string key)
        {
            _storage.Delete(key);
        }
    }
}