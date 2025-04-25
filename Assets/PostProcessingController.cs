using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoSingleton<PostProcessingController>
{
    private bool _disabled;
    public bool disabled
    {
        get => _disabled;
        set
        {
            volume.enabled = !value;
            _disabled = value;
        }
    }
    public bool isManual;
    public List<PostProcessProfile> postProcessProfiles;
    public int currentPostProcessNum;
    public PostProcessVolume volume;
    private const string PrefPPIndex = "PostProcessProfileIndex";
    private const string PrefDisable = "PPDisable";
    private const string PrefSystem = "PPSystem";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(PrefDisable)) PlayerPrefs.SetInt(PrefDisable, 0);
        if (!PlayerPrefs.HasKey(PrefPPIndex)) PlayerPrefs.SetInt(PrefPPIndex, 0);
        if (!PlayerPrefs.HasKey(PrefSystem)) PlayerPrefs.SetInt(PrefSystem, 0);
        ChangeFilter(PlayerPrefs.GetInt(PrefPPIndex));

        isManual = PlayerPrefs.GetInt(PrefDisable) != 0;
        disabled = PlayerPrefs.GetInt(PrefPPIndex, 0) != 0;
        Filter(PlayerPrefs.GetInt(PrefSystem));
    }

    public void ChangeFilter(int currentNum)
    {
//        Debug.Log(currentNum);
        PlayerPrefs.SetInt(PrefPPIndex, currentNum);
        currentNum -= 1;

        if (currentNum < 0) // Handle "Auto"
        {
            //Debug.Log("Auto");
            currentPostProcessNum = 0;
            isManual = false;
            Filter(PlayerPrefs.GetInt(PrefSystem));
            
            return;
        }

        if (currentNum >= 0 && currentNum < postProcessProfiles.Count)
        {
            //Debug.Log("noAuto : (");
            isManual = true;
            currentPostProcessNum = currentNum;
            volume.profile = postProcessProfiles[currentNum];
            Debug.Log(postProcessProfiles[currentNum].name);
        }
    }

    public void Filter(int currentNum)
    {
        if (PlayerPrefs.GetInt(PrefSystem) != currentNum)
            PlayerPrefs.SetInt(PrefSystem, currentNum);
        if (isManual || disabled)
        {
            /*Debug.Log($"{disabled} , {isManual}");
            Debug.Log("No Change Filter");*/
            return;
        }
            currentPostProcessNum = currentNum;
        if (volume.profile == postProcessProfiles[currentNum]) return;
            volume.profile = postProcessProfiles[currentNum];
    }
}