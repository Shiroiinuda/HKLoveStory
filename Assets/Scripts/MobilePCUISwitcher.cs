using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobilePCUISwitcher : MonoBehaviour
{
    public Sprite pcUI;
    public Sprite pcUI_hl;

    public Sprite mobileUI;
    public Sprite mobileUI_hl;

    // Start is called before the first frame update
    void Start()
    {
        Image targetImgage = gameObject.GetComponent<Image>();
        Button targetbtn = gameObject.GetComponent<Button>();
        SpriteState spriteState = new SpriteState();

#if !(UNITY_ANDROID || UNITY_IOS)
        targetImgage.sprite = pcUI;
        spriteState.highlightedSprite = pcUI_hl;
        spriteState.pressedSprite = pcUI_hl;
        targetbtn.spriteState = spriteState;
#else
        targetImgage.sprite = mobileUI;
        spriteState.highlightedSprite = mobileUI_hl;
        spriteState.pressedSprite = mobileUI_hl;
        targetbtn.spriteState = spriteState;
#endif

    }
}
