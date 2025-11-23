using UnityEngine;
using LongDK.Scenes;
using LongDK.UI;
using LongDK.Debug;

public class SceneTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Log.Msg("Loading SampleScene...");
            // Note: In a real project, you'd load a DIFFERENT scene. 
            // Loading the same scene works for testing the transition UI.
            SceneLoader.Instance.LoadScene("SampleScene", UI_ID.None); 
        }
    }
}