using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterButtonController : MonoBehaviour
{
    public string url_Google;
    public string url_Apple;

    public void InputStoreURL()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (url_Google != null || url_Google != "")
                Application.OpenURL(url_Google);
        }
        else
        {
            if (url_Apple != null || url_Apple != "")
                Application.OpenURL(url_Apple);
        }
    }
}
