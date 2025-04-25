using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuChanging : MonoBehaviour
{
    [SerializeField] private List<Sprite> bG;
    private int currentbG;
    [SerializeField] private SpriteRenderer renderer;
    private int countClickTime;
    [SerializeField] private string achievementName;
    public void ChangeBG()
    {
        currentbG++;
        countClickTime++;
        currentbG %= bG.Count;
        renderer.sprite = bG[currentbG];
        if (countClickTime >= 12)
        {
            GameControl.instance.OnUnlockAchievement(achievementName);
        }
    }
}
