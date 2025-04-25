using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUnlockTruthStory : MonoBehaviour
{
    public GameControl gameControl;
    private const string clickSfx = "Menu Click";

    public void OnChapterOpen()
    {
        /*gameObject.SetActive(gameControl.unlockTrueEndStory == 1 && !gameControl.stageClear.Contains("End2"));*/
    }

    public void OnResetButtonClicked()
    {
        /*SoundControl.SoundManager.PlaySfx(clickSfx);
        gameControl.unlockTrueEndStory = 0;*/
    }
}
