using System;
using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class QuitPanel : MonoBehaviour
{
    public Button[] returnBtn;
    public Button yesBtn;
    public Button noBtn;
    public Button contentBtn;
    public TextMeshProUGUI contentText;
    public int messageHitCount;
    private void Start()
    {
        messageHitCount = 0;
        contentText.text = Localization.GetString($"EasterEgg/QuitPanel1");
        foreach (var btn in returnBtn)
        {
            btn.onClick.AddListener(()=>GameControl.instance.OnBackClicked(this.gameObject));
        }
        yesBtn.onClick.AddListener(GameControl.instance.ConfirmQuitClicked);
        noBtn.onClick.AddListener(() => GameControl.instance.OnBackClicked(this.gameObject));
        contentBtn.onClick.AddListener(ChangeMessage);
    }

    private void ChangeMessage()
    {
        contentText.text = Localization.GetString($"EasterEgg/QuitPanel{Random.Range(2, 10)}");
        messageHitCount++;
        
    }
}
