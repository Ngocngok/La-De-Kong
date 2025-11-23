using System.IO;
using UnityEngine;
using LongDK.Debug;

namespace LongDK.Save
{
    public class FileStorage : IStorage
    {
        private readonly string _basePath;
        private const string EXTENSION = ".json";

        public FileStorage()
        {
            _basePath = Application.persistentDataPath;
        }

        private string GetPath(string key)
        {
            return Path.Combine(_basePath, key + EXTENSION);
        }

        public void Write(string key, string data)
        {
            string path = GetPath(key);
            try
            {
                File.WriteAllText(path, data);
                // Log.Msg($"Saved to: {path}"); // Verbose
            }
            catch (System.Exception e)
            {
                Log.Error($"Failed to write save file '{key}': {e.Message}");
            }
        }

        public string Read(string key)
        {
            string path = GetPath(key);
            if (!File.Exists(path))
            {
                Log.Warn($"Save file not found: {path}");
                return null;
            }

            try
            {
                return File.ReadAllText(path);
            }
            catch (System.Exception e)
            {
                Log.Error($"Failed to read save file '{key}': {e.Message}");
                return null;
            }
        }

        public bool Exists(string key)
        {
            return File.Exists(GetPath(key));
        }

        public void Delete(string key)
        {
            string path = GetPath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}