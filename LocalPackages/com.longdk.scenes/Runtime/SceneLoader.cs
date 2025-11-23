using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using LongDK.Core;
using LongDK.Debug;
using LongDK.UI;
using LongDK.Audio;

namespace LongDK.Scenes
{
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        [Header("Config")]
        [SerializeField] private float _minLoadTime = 1.0f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private UI_ID _defaultLoadingScreenID = UI_ID.None; // Will be set to Loading_Simple by user

        private bool _isLoading = false;

        public void LoadScene(string sceneName, UI_ID loadingScreenID = UI_ID.None)
        {
            if (_isLoading)
            {
                Log.Warn("Scene load already in progress.");
                return;
            }

            if (loadingScreenID == UI_ID.None)
            {
                loadingScreenID = _defaultLoadingScreenID;
                if (loadingScreenID == UI_ID.None)
                {
                    // Fallback if user forgot to set default
                    // We try to find "Loading_Simple" if it exists in the enum, otherwise we just warn
                    // Since we can't check Enum existence easily without reflection/parsing, we just log.
                    Log.Warn("No loading screen ID provided and no default set. Transition might be abrupt.");
                }
            }

            StartCoroutine(LoadRoutine(sceneName, loadingScreenID));
        }

        private IEnumerator LoadRoutine(string sceneName, UI_ID loadingScreenID)
        {
            _isLoading = true;

            // 1. Block Input (Optional - usually handled by UI overlay blocking raycasts)
            
            // 2. Show Loading Screen
            ILoadingScreen loadingScreen = null;
            if (loadingScreenID != UI_ID.None)
            {
                UIFrame frame = UIManager.Instance.Show(loadingScreenID);
                loadingScreen = frame as ILoadingScreen;
                
                // Wait for UI to be ready
                yield return null; 
            }

            // 3. Fade out Music
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.StopMusic(_fadeDuration);
            }

            // Wait for fade
            yield return new WaitForSeconds(_fadeDuration);

            // 4. Load Scene
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            float timer = 0f;
            while (!op.isDone)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(op.progress / 0.9f);
                
                // Artificial delay
                if (timer >= _minLoadTime && progress >= 1f)
                {
                    op.allowSceneActivation = true;
                }

                if (loadingScreen != null)
                {
                    loadingScreen.SetProgress(progress);
                }
                
                yield return null;
            }

            // 5. Cleanup
            // Force GC
            System.GC.Collect();

            // 6. Hide Loading Screen
            if (loadingScreenID != UI_ID.None)
            {
                UIManager.Instance.Hide(loadingScreenID);
            }

            _isLoading = false;
        }
    }
}