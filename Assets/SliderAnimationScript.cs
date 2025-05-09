using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAnimationScript : MonoBehaviour
{
    Slider slider;
    public RectTransform rectTransform;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Scale(slider.value);
        slider.onValueChanged.AddListener((arg0)=>Scale(arg0));
    }

    private void Scale(float scaling)
    {
        float scalingF = Mathf.Lerp( 1, 2,scaling);
        rectTransform.localScale = new Vector3(scalingF, scalingF, scalingF);
    }
}
