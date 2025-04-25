using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using Google.Play.Review;
#endif

public class CommentBoxController : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip clickSfx;
    public AudioClip backSfx;

    public GameObject commentBox1;

#if UNITY_ANDROID
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
#endif

    public void OnCommentBtnClicked()
    {
        audioManager.PlayButtonSFX(clickSfx);

#if UNITY_IOS
            Device.RequestStoreReview();
#elif UNITY_ANDROID
        StartCoroutine(AndroidRequestReview());
#endif

        commentBox1.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnFirstNoClicked()
    {
        audioManager.PlayButtonSFX(backSfx);
        commentBox1.SetActive(false);
    }

    public void OnSecondNoClicked()
    {
        audioManager.PlayButtonSFX(backSfx);
        commentBox1.SetActive(true);
        this.gameObject.SetActive(false);
    }

#if UNITY_ANDROID
    IEnumerator AndroidRequestReview()
    {
        _reviewManager = new ReviewManager();

        //Request a ReviewInfo Obj
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.GenuineStudio.HauntedHouse2&pcampaignid=web_share&pli=1");
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();

        //Launch the Review Flow
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.GenuineStudio.HauntedHouse2&pcampaignid=web_share&pli=1");
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
#endif
}
