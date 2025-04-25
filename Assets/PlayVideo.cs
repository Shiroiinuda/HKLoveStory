using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class PlayVideo : MonoBehaviour
{
    public VideoClip clip;
    private RenderTexture rt;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public Vector2 size = new Vector2(1920, 1080);
    public bool dontAutoPlay;


    [ButtonMethod]
    void Init()
    {
        if (rawImage == null)
            rawImage=GetComponent<RawImage>();
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
        clip = videoPlayer.clip;
        videoPlayer.targetTexture = null;
        videoPlayer.clip = null;
    }
    void  Awake()
    {

#if UNITY_ANDROID || UNITY_IOS
        size = new Vector2(clip.width,clip.height);
#endif
        rt = new RenderTexture((int)size.x, (int)size.y, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        // Add code here to work on the render texture
        videoPlayer.targetTexture = rt;
        rawImage.texture = rt;
        videoPlayer.clip = clip;
        videoPlayer.Play();

        if (!dontAutoPlay) return;
        videoPlayer.time = 1;
        videoPlayer.Pause();
    }
    

    private void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
            if(!videoPlayer.isPlaying)
                videoPlayer.Play();
    }
}
