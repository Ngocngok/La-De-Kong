using UnityEngine;
using LongDK.UI;
using LongDK.Debug;

public class TestWindow : UIFrame
{
    public override void OnShow()
    {
        Log.Msg($"Window {ID} Shown!");
    }

    public override void OnHide()
    {
        Log.Msg($"Window {ID} Hidden!");
    }

    public void Setup(string message)
    {
        Log.Msg($"Window {ID} Setup with message: {message}");
    }
}
