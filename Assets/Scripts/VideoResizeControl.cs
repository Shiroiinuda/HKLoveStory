using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoResizeControl : MonoBehaviour
{
    public GameObject videoClip;

    // Start is called before the first frame update
    void Start()
    {
        if (videoClip != null)
        {
            float displayWidth = Display.main.systemWidth;
            float displayHeight = Display.main.systemHeight;

            float displayRatio = displayWidth / displayHeight;

            float targetRatio = 16.0f / 9.0f;

            if (displayRatio > targetRatio)
            {
                float height = (1920f / displayWidth) * displayHeight;
                float width = (height / 1080f) * 1920;

                videoClip.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            }
        }
    }

}
