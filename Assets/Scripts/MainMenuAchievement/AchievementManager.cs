using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using I2.Loc;
using SoundControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public string achievementID;
    public GameControl gameControl;
    public Localize achievementTypeText;
    public Localize achievementNameText;

    public Image achievementImg;
    public Sprite lockSprite;
    public Sprite unlockedSprite;

    public Animator achievementWayAnim;
    public Localize achievementWayText;

    private void OnEnable()
    {
        achievementTypeText.SetTerm($"Achievement/Type/{achievementID}");
        if (gameControl.achievementList.Contains(achievementID.Trim()))
        {
            achievementImg.sprite = unlockedSprite;
            achievementNameText.SetTerm($"Achievement/Name/{achievementID}");
//            SoundManager.PlayBgm("asd");

            achievementNameText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 255f / 255f);
            achievementTypeText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 255f / 255f);
        }
        else
        {
            achievementImg.sprite = lockSprite;
            achievementNameText.SetTerm("???");
            achievementNameText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 255f / 255f);
            achievementTypeText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 255f / 255f);
        }
    }

    public void OnAchievementClicked()
    {
        achievementWayText.SetTerm($"Achievement/Way/{achievementID}");
        achievementWayText.GetComponent<TextMeshProUGUI>().text =
            Localization.GetString($"Achievement/Way/{achievementID}");
        achievementWayAnim.SetTrigger("Show");
    }
}
