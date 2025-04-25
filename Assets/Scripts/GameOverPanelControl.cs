using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelControl : MonoBehaviour
{
    public LoadSceneManager loadSceneManager;
    public GameControl gameControl;
    public int replayMark = -1;

    public void OnMenuClicked()
    {
        gameControl.replayCount = 0;
        loadSceneManager.FadeToLevel(2);
    }

    public void OnRplayClicked()
    {
        gameControl.replayCount += 1;
        if (replayMark >= 0)
            gameControl.currentBookmark = replayMark;
        loadSceneManager.FadeToLevel(3);
    }
}
