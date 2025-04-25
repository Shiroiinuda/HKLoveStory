using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour
{
    public Button qBoxButton;
    public string url_Android;
    public string url_IOS;
    public string url_PC;

    private void Start()
    {
        if (qBoxButton != null)
            qBoxButton.onClick.AddListener(InputURL);
    }

    public void InputURL()
    {
#if UNITY_ANDROID
        Application.OpenURL(url_Android);
#elif UNITY_IOS
                Application.OpenURL(url_IOS);
#else
                        Application.OpenURL(url_PC);
#endif
    }
}
