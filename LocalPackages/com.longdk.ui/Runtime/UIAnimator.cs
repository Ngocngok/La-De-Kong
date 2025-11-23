using System;
using System.Collections;
using UnityEngine;

namespace LongDK.UI
{
    public abstract class UIAnimator : MonoBehaviour
    {
        public abstract float Duration { get; }
        public abstract IEnumerator AnimateShow();
        public abstract IEnumerator AnimateHide();
        public abstract void ResetState(); // Called on Awake to ensure correct starting state
    }
}