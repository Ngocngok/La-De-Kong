using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongDK.Save
{
    /// <summary>
    /// A wrapper for Dictionary that supports Unity serialization (JsonUtility).
    /// Usage: public class MyDict : SerializableDictionary<string, int> {}
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys = new List<TKey>();
        [SerializeField] private List<TValue> _values = new List<TValue>();

        // Save to List (Before Serialize)
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var pair in this)
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        // Load from List (After Deserialize)
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (_keys.Count != _values.Count)
            {
                UnityEngine.Debug.LogError($"[SerializableDictionary] Key count ({_keys.Count}) != Value count ({_values.Count}). Data corruption?");
                return;
            }

            for (int i = 0; i < _keys.Count; i++)
            {
                // Handle duplicate keys gracefully (though they shouldn't exist in a valid save)
                if (!this.ContainsKey(_keys[i]))
                {
                    this.Add(_keys[i], _values[i]);
                }
            }
        }
    }
}