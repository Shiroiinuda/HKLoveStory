using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;

public class GetBabyAchievement : MonoBehaviour
{
    public PatientGameControl patientGameControl;
    public GameControl gameControl;
    public DialogueManager dialogueManager;
    public InvestigationControl investigationControl;
    public AudioManager audioManager;

    public AudioClip clickSfx;
    private int getBabyJumpMark;

    private void Start()
    {
        var roads = dialogueManager.currentDialogue.roads;
        if (roads.road1 != "")
            getBabyJumpMark = int.Parse(roads.road3);
    }

    public void OnGhostMotherCliced()
    {
        if (gameControl.items.Contains("Baby"))
        {
            dialogueManager.jumpMark = getBabyJumpMark;
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }
        else
        {
            audioManager.PlayButtonSFX(clickSfx);
            patientGameControl.OnMonitorClick();
        }
    }
}
