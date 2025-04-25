using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemPanelControl : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public void OnLeaveClick()
    {
        if(dialogueManager.currentDialogue.itemModify.addItem != "")
        {
            dialogueManager.DisplayNextSentence();
        }
        
        this.gameObject.SetActive(false);
    }
}
