using UnityEngine;
using LongDK.Core;
using LongDK.Debug;

public class TestManager : MonoSingleton<TestManager>
{
    public void SayHello()
    {
        Log.Msg("Hello from TestManager Singleton!");
    }
}

public class SingletonTester : MonoBehaviour
{
    void Start()
    {
        if (TestManager.Instance != null)
        {
            TestManager.Instance.SayHello();
        }
        else
        {
            Log.Error("TestManager instance not found! (This is expected if you didn't put it in the scene)");
        }
    }
}