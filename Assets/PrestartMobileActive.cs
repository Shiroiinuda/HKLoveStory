using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestartMobileActive : MonoBehaviour
{
    [SerializeField] private List<GameObject> mobileList;

    private void Awake()
    {
#if !(UNITY_ANDROID || UNITY_IOS)
        foreach (var gameObject in mobileList)
        {
            gameObject.SetActive(false);    
        }
#else
        foreach (var gameObject in mobileList)
        {
            gameObject.SetActive(true);
        }
#endif
    }
}
