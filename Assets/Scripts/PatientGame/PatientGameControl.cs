using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class PatientGameControl : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Button CantPressbtn;
    public PlayableDirector grabKey;

    private int winJumpMark;
    private int loseJumpMark;

    private bool gameOver = false;

    private void Start()
    {
       var road = dialogueManager.currentDialogue.roads;
        if (road.road1 != "")
            winJumpMark = int.Parse(road.road1);

        if (road.road2 != "")
            loseJumpMark = int.Parse(road.road2);
    }

    public void OnMonitorClick()
    {
        if (gameOver == false)
        {
            gameOver = true;
            CantPressbtn.enabled = false;
            GameOver();
        }
    }

    public void OnAnimationEnd()
    {
        if (gameOver == false)
        {
            CantPressbtn.enabled = false;
            dialogueManager.jumpMark = winJumpMark;
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }
    }

    private void GameOver()
    {
        investigationControl.endGameBlocker.SetActive(true);
        gameOver = true;
        grabKey.Stop();

        dialogueManager.jumpMark = loseJumpMark;
        StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
    }
}
