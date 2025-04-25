using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> saveload;
    private void OnEnable()
    {
        #if UNITY_ANDROID || UNITY_IOS
        foreach (var objects in saveload)
        {
            objects.SetActive(false);
        }
        #endif
    }
}
