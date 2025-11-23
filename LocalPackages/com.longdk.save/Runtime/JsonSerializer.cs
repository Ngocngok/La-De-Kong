using UnityEngine;

namespace LongDK.Save
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T data)
        {
            return JsonUtility.ToJson(data, true); // Pretty print for debug
        }

        public T Deserialize<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }
    }
}