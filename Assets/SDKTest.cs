using UnityEngine;
using LongDK.Debug;

public class SDKTest : MonoBehaviour
{
    void Start()
    {
        Log.Msg("SDK Test: Core and Debug systems are working!");
        Log.Warn("SDK Test: This is a warning.");
        Log.Error("SDK Test: This is an error.");
        
        UnityEngine.Debug.Log("If you don't see the [LongDK] logs above, make sure 'LONGDK_DEBUG' is added to Scripting Define Symbols.");
    }
}