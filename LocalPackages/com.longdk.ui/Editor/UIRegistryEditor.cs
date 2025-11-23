using UnityEngine;
using UnityEditor;
using System.IO;
using LongDK.UI;

namespace LongDK.UI.Editor
{
    [CustomEditor(typeof(UIRegistry))]
    public class UIRegistryEditor : UnityEditor.Editor
    {
        private string _newEnumName = "";

        public override void OnInspectorGUI()
        {
            UIRegistry registry = (UIRegistry)target;

            GUILayout.Space(10);
            GUILayout.Label("UI ID Generator", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            _newEnumName = EditorGUILayout.TextField("New ID Name", _newEnumName);
            
            if (GUILayout.Button("Add & Generate", GUILayout.Width(120)))
            {
                if (string.IsNullOrEmpty(_newEnumName))
                {
                    EditorUtility.DisplayDialog("Error", "Enum name cannot be empty.", "OK");
                }
                else
                {
                    AddEnumAndEntry(registry, _newEnumName);
                    _newEnumName = ""; // Reset
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            base.OnInspectorGUI();
        }

        private void AddEnumAndEntry(UIRegistry registry, string enumName)
        {
            enumName = enumName.Replace(" ", "_");
            
            string[] guids = AssetDatabase.FindAssets("UIDefinitions");
            if (guids.Length == 0)
            {
                UnityEngine.Debug.LogError("Could not find UIDefinitions.cs file!");
                return;
            }
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            
            string content = File.ReadAllText(path);
            if (content.Contains(enumName + ","))
            {
                EditorUtility.DisplayDialog("Error", $"Enum '{enumName}' already exists!", "OK");
                return;
            }

            string marker = "// [GENERATED_ENUMS_END]";
            if (!content.Contains(marker))
            {
                UnityEngine.Debug.LogError("UIDefinitions.cs is missing the generation marker: " + marker);
                return;
            }

            string newEnumLine = $"        {enumName},\n";
            string newContent = content.Replace(marker, newEnumLine + "        " + marker);
            
            File.WriteAllText(path, newContent);
            AssetDatabase.Refresh();

            UnityEngine.Debug.Log($"Added '{enumName}' to UIDefinitions.cs. Wait for compilation...");
            
            UIEntry newEntry = new UIEntry();
            Undo.RecordObject(registry, "Add UI Entry");
            registry.Entries.Add(newEntry);
            EditorUtility.SetDirty(registry);
        }
    }
}