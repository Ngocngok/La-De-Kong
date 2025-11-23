using UnityEngine;
using UnityEngine.UI;
using LongDK.UI;

namespace LongDK.Scenes
{
    public class SimpleLoadingScreen : UIFrame, ILoadingScreen
    {
        [Header("References")]
        [SerializeField] private Image _progressBar;
        [SerializeField] private Text _progressText; // Optional

        public void SetProgress(float progress)
        {
            if (_progressBar != null)
            {
                _progressBar.fillAmount = progress;
            }

            if (_progressText != null)
            {
                _progressText.text = $"{(int)(progress * 100)}%";
            }
        }
    }
}