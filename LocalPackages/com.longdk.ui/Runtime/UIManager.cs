using System;
using System.Collections.Generic;
using UnityEngine;
using LongDK.Core;
using LongDK.Debug;

namespace LongDK.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private UIRegistry _registry;
        [SerializeField] private Transform _rootCanvasTransform; // Optional: Parent for all UI

        private Dictionary<UI_ID, UIFrame> _instances = new Dictionary<UI_ID, UIFrame>();
        private Stack<UIFrame> _stack = new Stack<UIFrame>();

        // Track the currently active window (top of stack)
        public UIFrame CurrentWindow => _stack.Count > 0 ? _stack.Peek() : null;

        /// <summary>
        /// Shows a UI window by ID.
        /// </summary>
        public UIFrame Show(UI_ID id)
        {
            return Show<UIFrame>(id, null);
        }

        /// <summary>
        /// Shows a UI window by ID with a setup callback.
        /// </summary>
        public T Show<T>(UI_ID id, Action<T> onSetup = null) where T : UIFrame
        {
            if (_registry == null)
            {
                Log.Error("UIRegistry is missing!");
                return null;
            }

            // 1. Get Instance
            UIFrame instance = GetOrCreateInstance(id);
            if (instance == null) return null;

            T typedInstance = instance as T;
            if (typedInstance == null)
            {
                Log.Error($"UI ID {id} is not of type {typeof(T).Name}!");
                return null;
            }

            // 2. Setup (Before Show)
            onSetup?.Invoke(typedInstance);

            // 3. Handle Stack Logic
            if (instance.Layer == UILayer.Window)
            {
                // If we are opening a new window, push it to stack
                if (!_stack.Contains(instance))
                {
                    _stack.Push(instance);
                }
            }

            // 4. Show
            instance.Show();
            return typedInstance;
        }

        public void Hide(UI_ID id)
        {
            if (_instances.TryGetValue(id, out var instance))
            {
                instance.Hide();
                // Note: If it was in the stack, we should probably remove it?
                // But removing from middle of stack is messy. 
                // For now, Hide(ID) is mostly used for Popups/Overlays (Loading Screens) which are not in the stack.
            }
        }

        public void Back()
        {
            if (_stack.Count == 0) return;

            UIFrame top = _stack.Pop();
            top.Hide();

            // If there is a window below, make sure it's active/interactive
            if (_stack.Count > 0)
            {
                UIFrame next = _stack.Peek();
                // next.OnFocus(); // Optional: Add OnFocus hook
            }
        }

        public void HideAll()
        {
            foreach (var kvp in _instances)
            {
                kvp.Value.Hide();
            }
            _stack.Clear();
        }

        private UIFrame GetOrCreateInstance(UI_ID id)
        {
            if (_instances.TryGetValue(id, out var existing))
            {
                return existing;
            }

            UIFrame prefab = _registry.GetPrefab(id);
            if (prefab == null)
            {
                Log.Error($"UI Prefab not found for ID: {id}");
                return null;
            }

            UIFrame instance = Instantiate(prefab, _rootCanvasTransform != null ? _rootCanvasTransform : transform);
            instance.ID = id;
            instance.gameObject.SetActive(false); // Start hidden
            _instances.Add(id, instance);
            return instance;
        }
    }
}