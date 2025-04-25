using System;
using UnityEngine;

public class ResolutionInitScript : MonoBehaviour
{
#if !(UNITY_ANDROID || UNITY_IOS)
        private const string PrefWidth = "ResolutionWidth";
    private const string PrefHeight = "ResolutionHeight";
    private const string PrefRefresh = "ResolutionRefresh";
    private const string PrefFullscreen = "IsFullscreen";

    void Awake()
    {
        // Check if PlayerPrefs are set
        int savedWidth = PlayerPrefs.GetInt(PrefWidth, -1);
        int savedHeight = PlayerPrefs.GetInt(PrefHeight, -1);
        int savedRefresh = PlayerPrefs.GetInt(PrefRefresh, -1);
        int savedFullscreenInt = PlayerPrefs.GetInt(PrefFullscreen, -1);

        // If no saved prefs, initialize defaults
        if (savedWidth <= 0 || savedHeight <= 0 || savedRefresh <= 0 || savedFullscreenInt == -1)
        {
            InitDefaultResolution();
        }
        else
        {
            // Apply saved resolution
            bool savedFullscreen = savedFullscreenInt == 1;
            Screen.SetResolution(savedWidth, savedHeight, savedFullscreen, savedRefresh);
        }
    }
    void Start()
    {
        // Assuming you have a method GetDisplayRefreshRate() that returns the current refresh rate.
        int refreshRate = Screen.currentResolution.refreshRate;
        if (refreshRate >= 120)
        {
            QualitySettings.vSyncCount = 1; // Sync with display
        }
        else
        {
            QualitySettings.vSyncCount = 0; // Disable vSync
            Application.targetFrameRate = 120; // Cap at 120 FPS
        }
    }

    private void InitDefaultResolution()
    {
        // Example: use the first available resolution and fullscreen as default
        Resolution[] availableResolutions = Screen.resolutions;

        if (availableResolutions.Length > 0)
        {
            Resolution defaultRes = FindClosestResolution(Screen.width, Screen.height);
            bool defaultFullscreen = true; // Set default to fullscreen. Change as desired.

            Screen.SetResolution(defaultRes.width, defaultRes.height, defaultFullscreen, defaultRes.refreshRate);

            PlayerPrefs.SetInt(PrefWidth, defaultRes.width);
            PlayerPrefs.SetInt(PrefHeight, defaultRes.height);
            PlayerPrefs.SetInt(PrefRefresh, defaultRes.refreshRate);
            PlayerPrefs.SetInt(PrefFullscreen, defaultFullscreen ? 1 : 0);
            PlayerPrefs.Save();
        }
        else
        {
            // In case no resolutions are found, you could set a fallback:
            // This scenario is rare, but you can choose a safe default resolution here.
            Screen.SetResolution(1920, 1080, true, 60);
            PlayerPrefs.SetInt(PrefWidth, 1920);
            PlayerPrefs.SetInt(PrefHeight, 1080);
            PlayerPrefs.SetInt(PrefRefresh, 60);
            PlayerPrefs.SetInt(PrefFullscreen, 1);
            PlayerPrefs.Save();
        }
    }
    private Resolution FindClosestResolution(int desiredWidth, int desiredHeight)
    {
        Resolution[] availableResolutions = Screen.resolutions;

        // If no resolutions are available, pick a fallback
        if (availableResolutions.Length == 0)
        {
            Resolution fallback = new Resolution();
            fallback.width = 1920;
            fallback.height = 1080;
            fallback.refreshRate = 60;
            return fallback;
        }

        // Initialize comparison values
        Resolution closest = availableResolutions[0];
        int closestDiff = int.MaxValue;
        int desiredTotalPixels = desiredWidth * desiredHeight;

        // Loop through available resolutions to find the best match
        foreach (Resolution res in availableResolutions)
        {
            int resTotalPixels = res.width * res.height;
            int diff = Math.Abs(resTotalPixels - desiredTotalPixels);

            // Optionally consider aspect ratio or refresh rate as well:
            // e.g., add extra weighting if aspect ratios differ significantly

            if (diff < closestDiff)
            {
                closestDiff = diff;
                closest = res;
            }
        }

        return closest;
    }
#endif

}
