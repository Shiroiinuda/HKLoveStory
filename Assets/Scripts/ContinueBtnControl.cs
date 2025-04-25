using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueBtnControl : MonoBehaviour
{

    [SerializeField] GameObject startButton;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameControl gameControl;
    [SerializeField] LoadSceneManager loadSceneManager;

    [SerializeField] private bool gamePlayed;

    // Start is called before the first frame update
    void Start()
    {
            startButton.SetActive(!(gameControl.currentBookmark > 0));
            continueButton.SetActive(gameControl.currentBookmark > 0);
            gamePlayed = gameControl.currentBookmark > 0;
    }

    public void OnBtnCliced()
    {
        if (gamePlayed == false)
        {
            gameControl.unLockedSavept.Add("Chapter1");
        }

        loadSceneManager.FadeToLevel(3);
    }
}
