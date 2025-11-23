using UnityEngine;
using LongDK.UI;
using LongDK.Debug;

public class UITest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Log.Msg("Showing MainMenu with Setup...");
            UIManager.Instance.Show<TestWindow>(UI_ID.MainMenu, (window) => 
            {
                window.Setup("Hello from Setup!");
            });
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Log.Msg("Showing GameplayHUD...");
            UIManager.Instance.Show(UI_ID.GameplayHUD);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Log.Msg("Back...");
            UIManager.Instance.Back();
        }
    }
}