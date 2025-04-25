using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ResolutionSetting : MonoBehaviour
{
#if !(UNITY_ANDROID || UNITY_IOS)
    public TMP_Dropdown resolutionDropdown;
  public Toggle fullscreenToggle;
  public TMP_Dropdown fullScreenDropDown;

    private Resolution[] availableResolutions;

    private const string PrefWidth = "ResolutionWidth";
    private const string PrefHeight = "ResolutionHeight";
    private const string PrefRefresh = "ResolutionRefresh";
    private const string PrefFullscreen = "IsFullscreen";

    void Start()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogError("No dropdown assigned to ResolutionScript. Please assign a UI Dropdown in the Inspector.");
            return;
        }

        if (fullscreenToggle == null)
        {
            Debug.LogError("No fullscreen toggle assigned to ResolutionScript. Please assign a UI Toggle in the Inspector.");
            return;
        }
        

        var bestResolutions = Screen.resolutions
            .GroupBy(r => (r.width, r.height))
            .Select(g => g.OrderByDescending(r => r.refreshRate).First())
            .ToList();


        foreach (var res in bestResolutions)
        {
            Debug.Log($"{res.width} x {res.height} @ {res.refreshRate}");
        }

        availableResolutions = bestResolutions.ToArray();
        resolutionDropdown.ClearOptions();
        
        var options = bestResolutions
            .Select(res => $"{res.width} x {res.height}")
            .ToList();
        
        resolutionDropdown.AddOptions(options);

        // Load saved settings
        int savedWidth = PlayerPrefs.GetInt(PrefWidth, Screen.currentResolution.width);
        int savedHeight = PlayerPrefs.GetInt(PrefHeight, Screen.currentResolution.height);
        int savedRefresh = PlayerPrefs.GetInt(PrefRefresh, Screen.currentResolution.refreshRate);
        bool savedFullscreen = PlayerPrefs.GetInt(PrefFullscreen, Screen.fullScreen ? 1 : 0) == 1;

        int currentResolutionIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == savedWidth &&
                availableResolutions[i].height == savedHeight &&
                availableResolutions[i].refreshRate == savedRefresh)
            {
                currentResolutionIndex = i;
                break;
            }
        }

        // Apply the loaded resolution settings
        
        Screen.SetResolution(savedWidth, savedHeight, savedFullscreen, savedRefresh);
        
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullScreenDropDown.value = savedFullscreen ? 0 : 1;
        

        resolutionDropdown.onValueChanged.AddListener(SetResolutionFromDropdown);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        fullScreenDropDown.onValueChanged.AddListener((arg0)=>SetFullscreen(arg0 == 0));
    }

    void SetResolutionFromDropdown(int index)
    {
        Resolution chosenRes = availableResolutions[index];
        bool isFullscreen = PlayerPrefs.GetInt(PrefFullscreen)==1;
        Screen.SetResolution(chosenRes.width, chosenRes.height, isFullscreen, chosenRes.refreshRate);

        SavePreferences(chosenRes.width, chosenRes.height, chosenRes.refreshRate, isFullscreen);
    }

    void SetFullscreen(bool isFullscreen)
    {
        Resolution chosenRes = availableResolutions[resolutionDropdown.value];
        Screen.SetResolution(chosenRes.width, chosenRes.height, isFullscreen, chosenRes.refreshRate);

        SavePreferences(chosenRes.width, chosenRes.height, chosenRes.refreshRate, isFullscreen);
    }

    void SavePreferences(int width, int height, int refreshRate, bool fullscreen)
    {
        PlayerPrefs.SetInt(PrefWidth, width);
        PlayerPrefs.SetInt(PrefHeight, height);
        PlayerPrefs.SetInt(PrefRefresh, refreshRate);
        PlayerPrefs.SetInt(PrefFullscreen, fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
#endif
}