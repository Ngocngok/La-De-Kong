using System;
using System.Collections;
using UnityEngine;
using LongDK.Debug;

namespace LongDK.UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public class UIFrame : MonoBehaviour
    {
        [Header("Config")]
        public UILayer Layer = UILayer.Window;
        public bool KeepUnderlyingVisible = false; // If false, might hide windows below (optimization) - Not fully implemented in this MVP

        private UIAnimator _animator;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        public UI_ID ID { get; set; }

        protected virtual void Awake()
        {
            _animator = GetComponent<UIAnimator>();
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_animator != null) _animator.ResetState();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            // Ensure sorting order
            if (_canvas != null) _canvas.sortingOrder = (int)Layer;

            OnShow();

            if (_animator != null)
            {
                StopAllCoroutines();
                StartCoroutine(_animator.AnimateShow());
            }
            else
            {
                // Default behavior if no animator
                if (_canvasGroup) _canvasGroup.alpha = 1;
            }
        }

        public void Hide(Action onComplete = null)
        {
            OnHide();

            if (_animator != null)
            {
                StopAllCoroutines();
                StartCoroutine(HideRoutine(onComplete));
            }
            else
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            }
        }

        private IEnumerator HideRoutine(Action onComplete)
        {
            yield return _animator.AnimateHide();
            gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        // Virtual hooks for subclasses
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public virtual void OnBack() 
        {
            UIManager.Instance.Back();
        }
    }
}