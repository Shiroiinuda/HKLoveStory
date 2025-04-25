using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImagePasswordGameControl : PasswordGameControl
{
    public List<Sprite> sprite;
    private void Start()
    {
        foreach (TextMeshProUGUI pwText in pwTextList)
        {
            pwText.GetComponentInParent<Image>().sprite= sprite[0];
        }
    }
    public override void OnPWButtonClicked(int num)
    {
        PlaySoundIfNotNull(typingSfx);
        pwNumList[num] ++;
            pwNumList[num] %=3;

        pwTextList[num].GetComponentInParent<Image>().sprite= sprite[pwNumList[num]];
        if (needJumpBookMark == false)
            CheckCorrectPW();
    }
}
