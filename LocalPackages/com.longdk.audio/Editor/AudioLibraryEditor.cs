using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using LongDK.Audio;

namespace LongDK.Audio.Editor
{
    [CustomEditor(typeof(AudioLibrary))]
    public class AudioLibraryEditor : UnityEditor.Editor
    {
        private string _newEnumName = "";
        private int _selectedCategory = 1; // 0=Music, 1=SFX, 2=UI
        private string[] _categories = new string[] { "Music", "SFX", "UI" };

        public override void OnInspectorGUI()
        {
            AudioLibrary library = (AudioLibrary)target;

            GUILayout.Space(10);
            GUILayout.Label("Audio ID Generator", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            _newEnumName = EditorGUILayout.TextField("New ID Name", _newEnumName);
            _selectedCategory = EditorGUILayout.Popup(_selectedCategory, _categories);
            
            if (GUILayout.Button("Add & Generate", GUILayout.Width(120)))
            {
                if (string.IsNullOrEmpty(_newEnumName))
                {
                    EditorUtility.DisplayDialog("Error", "Enum name cannot be empty.", "OK");
                }
                else
                {
                    AddEnumAndEntry(library, _newEnumName, _selectedCategory);
                    _newEnumName = ""; // Reset
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Library Data", EditorStyles.boldLabel);
            
            base.OnInspectorGUI();
        }

        private void AddEnumAndEntry(AudioLibrary library, string enumName, int categoryIndex)
        {
            // 1. Validate Name
            enumName = enumName.Replace(" ", "_"); // Basic sanitization
            
            // 2. Find the AudioID.cs file
            string[] guids = AssetDatabase.FindAssets("AudioID");
            if (guids.Length == 0)
            {
                UnityEngine.Debug.LogError("Could not find AudioID.cs file!");
                return;
            }
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            
            // 3. Read and Modify
            string content = File.ReadAllText(path);
            if (content.Contains(enumName + ","))
            {
                EditorUtility.DisplayDialog("Error", $"Enum '{enumName}' already exists!", "OK");
                return;
            }

            string marker = "// [GENERATED_ENUMS_END]";
            if (!content.Contains(marker))
            {
                UnityEngine.Debug.LogError("AudioID.cs is missing the generation marker: " + marker);
                return;
            }

            string newEnumLine = $"        {enumName},\n";
            string newContent = content.Replace(marker, newEnumLine + "        " + marker);
            
            File.WriteAllText(path, newContent);
            AssetDatabase.Refresh();

            // 4. Add Entry to ScriptableObject (We need to wait for compilation? 
            // Actually, we can't add the entry with the *correct enum value* until compilation finishes.
            // But we can add it with a placeholder or just let the user know.)
            
            UnityEngine.Debug.Log($"Added '{enumName}' to AudioID.cs. Wait for compilation...");
            
            // Note: Because adding the Enum triggers a recompile, we can't immediately assign the new Enum value 
            // to the list in this frame. The user will see the new entry in the list, but the ID might default to 'None' 
            // until they select it, OR we can try to parse it next time.
            // For now, let's just add the entry to the list so the user doesn't have to click "+" manually.
            
            AudioData newData = new AudioData();
            // We can't set newData.ID = AudioID.NewName yet because it doesn't exist in the current domain.
            
            Undo.RecordObject(library, "Add Audio Entry");
            
            if (categoryIndex == 0) library.Music.Add(newData);
            else if (categoryIndex == 1) library.SFX.Add(newData);
            else library.UI.Add(newData);
            
            EditorUtility.SetDirty(library);
        }
    }
}