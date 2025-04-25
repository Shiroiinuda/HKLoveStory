using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializeads : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IOS

#if UNITY_ANDROID
    public string appKey = "2121d408d";
#elif UNITY_IOS
       public string appKey = "2121ed345";
#else
       public string appKey = "unexpected_platform";
#endif

    public static Initializeads instance;
    public static Initializeads Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAd();

        IronSourceEvents.onSdkInitializationCompletedEvent +=
            SdkInitializationCompletedEvent;
    }

    private void InitializeAd()
    {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void SdkInitializationCompletedEvent()
    {
        Debug.Log("IronSource Sdk Initialization Completed");
    }
#endif
}
