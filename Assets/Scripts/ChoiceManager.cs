using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public List<Choices> choicesList;
    public GameControl gameControl;
    public GameObject choicePanel;
    public Action resetClick;
    private Action deActive;

    private void Awake()
    {
        foreach (var choices in choicesList)
        {
            choices.choiceManager = this;
            resetClick += () => choices.clicked = false;
            deActive += () => choices.gameObject.SetActive(false);
        }
        deActive += () => choicePanel.SetActive(false);
    }

    public void OnChoiceButtonClicked(Choices choices)
    {

        dialogueManager.jumpMark = choices.jumpMark;

        if (dialogueManager.isChoiceLooping)
        {
            if (!choices.clicked)
            {
                dialogueManager.choicesClicked++;
                choices.clicked = true;
            }

            if (dialogueManager.choicesClicked == dialogueManager.loopChoiceCounter)
            {
                dialogueManager.isChoiceLooping = false;
                dialogueManager.choicesClicked = 0;
                dialogueManager.loopChoiceCounter = 0;
                resetClick();
            }
        }

        deActive();

        dialogueManager.continueButton.gameObject.SetActive(true);
        dialogueManager.dialogueText.text = "";

        dialogueManager.bubbleContinueButton.gameObject.SetActive(true);
        dialogueManager.bubbleText.text = "";

        dialogueManager.DisplayNextSentence();
    }
}
