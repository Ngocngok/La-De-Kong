using System.Collections;
using UnityEngine;

namespace LongDK.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SimpleFadeAnimator : UIAnimator
    {
        [SerializeField] private float _duration = 0.3f;
        private CanvasGroup _canvasGroup;

        public override float Duration => _duration;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void ResetState()
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public override IEnumerator AnimateShow()
        {
            float timer = 0;
            while (timer < _duration)
            {
                timer += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(0, 1, timer / _duration);
                yield return null;
            }
            _canvasGroup.alpha = 1;
        }

        public override IEnumerator AnimateHide()
        {
            float timer = 0;
            while (timer < _duration)
            {
                timer += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(1, 0, timer / _duration);
                yield return null;
            }
            _canvasGroup.alpha = 0;
        }
    }
}