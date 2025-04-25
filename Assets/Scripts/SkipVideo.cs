using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipVideo : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject bufferPanel;
    public GameObject currencyPanel;

    public void OnSkipClicked()
    {
        if (!currencyPanel.activeSelf)
            currencyPanel.SetActive(true);
        bufferPanel.SetActive(false);
        dialogueManager.OnButtonClick();
    }

}
