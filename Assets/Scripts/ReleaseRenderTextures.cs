using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ReleaseRenderTextures : MonoBehaviour
{
    private RenderTexture rt;
    public RawImage rawImage;

    void  Awake()
    {
            rt = GetComponent<VideoPlayer>().targetTexture;
        rt = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
        rt.Create();
        
        if(rawImage !=null && rt !=null)
        rawImage.texture = rt;

        // Release the hardware resources used by the render texture
        rt.Release();
    }
}
