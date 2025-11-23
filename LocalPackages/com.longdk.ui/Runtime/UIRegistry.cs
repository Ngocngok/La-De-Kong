using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongDK.UI
{
    [Serializable]
    public class UIEntry
    {
        public UI_ID ID;
        public UIFrame Prefab;
    }

    [CreateAssetMenu(fileName = "UIRegistry", menuName = "LongDK/UI/Registry")]
    public class UIRegistry : ScriptableObject
    {
        public List<UIEntry> Entries = new List<UIEntry>();

        public UIFrame GetPrefab(UI_ID id)
        {
            var entry = Entries.Find(x => x.ID == id);
            return entry != null ? entry.Prefab : null;
        }
    }
}