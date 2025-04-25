using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewarded : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IOS

    [Header("Scripts")]
    [SerializeField] private GameControl gameControl;
    [SerializeField] private CurrencyManager currencyManager;
    public ShopManager shopManager;
    public Initializeads initializeads;

    private void OnEnable()
    {
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
    }

    public void RewardAddJade()
    {
        if (gameControl.todayAdsCount < 10)
        {
            AudioManager.instance.adsStopBGM();
            IronSource.Agent.init(initializeads.appKey, IronSourceAdUnits.REWARDED_VIDEO);
            if (IronSource.Agent.isRewardedVideoAvailable())
                IronSource.Agent.showRewardedVideo();
            GameControl.instance.isWatchingAds = true;
        }
    }

    void RewardedVideoAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdClosedEvent With AdInfo " +
            adInfo.ToString());

        IronSource.Agent.init(initializeads.appKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);
        GameControl.instance.isWatchingAds = false;
        AudioManager.instance.ResetAudioAndPlay();
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" +
            placement.ToString() + "And AdInfo " + adInfo.ToString());


        gameControl.currency += 5;
        currencyManager.ShowCurrency();
        gameControl.todayAdsCount += 1;
        shopManager.UpdateAdsRestText();
    }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }

    void RewardedVideoOnAdUnavailable()
    {
    }

    private void OnDisable()
    {
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
    }

#endif
}
