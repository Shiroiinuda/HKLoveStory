#if UNITY_STANDALONE_WIN
using Steamworks;
#endif
using UnityEngine;

public class SteamScript : MonoBehaviour
{
    private void Start()
    {
        #if !UNITY_STANDALONE_WIN
        #else
        if (!SteamManager.Initialized) return;
       // SteamUserStats.ResetAllStats(true );
        Debug.Log(SteamUserStats.RequestCurrentStats());
        #endif
    }

    public static void GiveAchievement(string achievementID)
    {
        #if !UNITY_STANDALONE_WIN
                return;
        #else
                //Debug.LogError($"Steam Achievement Detected {achievementID}");
                if (!SteamManager.Initialized) return;
                SteamUserStats.GetAchievement(achievementID,out bool achievementObtain);
                //Debug.Log(achievementObtain);
                if (!achievementObtain)
                {
                    SteamUserStats.SetAchievement(achievementID);
                    SteamUserStats.StoreStats(); // Store the achievement unlock
                    //Debug.Log("Achievement Unlocked: " + achievementID);
                }
                else
                {
                    //Debug.LogError("Failed to unlock achievement: " + achievementID);
                }
        #endif
    }
}

