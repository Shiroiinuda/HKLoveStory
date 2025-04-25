using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDrawCard : MonoBehaviour
{
    /*public AudioManager audioManager;

    [Header("Scripts")]
    public DrawGameControl drawGameControl;
    public Initializeads initializeads;

    public AudioClip enterMoneySfx;
    // Start is called before the first frame update
    private void OnEnable()
    {
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailableDrawCard;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailableDrawCard;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedDrawCardVideoAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedDrawCardEvent;
    }

    public void WatchAdsDrawCard()
    {
        audioManager.StopBGM();
        IronSource.Agent.init(initializeads.appKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.showRewardedVideo();
    }

    void RewardedDrawCardVideoAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdClosedEvent With AdInfo " +
            adInfo.ToString());

        IronSource.Agent.init(initializeads.appKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);
        audioManager.ResetAudioAndPlay();

    }

    void RewardedVideoOnAdRewardedDrawCardEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" +
            placement.ToString() + "And AdInfo " + adInfo.ToString());

        drawGameControl.OnDrawByADSClick();
        audioManager.PlayButtonSFX(enterMoneySfx);
    }

    void RewardedVideoOnAdAvailableDrawCard(IronSourceAdInfo adInfo)
    {
    }

    void RewardedVideoOnAdUnavailableDrawCard()
    {
    }

    private void OnDisable()
    {
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailableDrawCard;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailableDrawCard;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedDrawCardVideoAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedDrawCardEvent;
        audioManager.ResetAudioAndPlay();
    }*/

}
