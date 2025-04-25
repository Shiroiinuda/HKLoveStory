using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.Events;

public class SkipButtonControl : MonoBehaviour
{
    public GameControl gameControl;
    public InvestigationControl investigationControl;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private List<string> cantSkipGameList;

    public UnityEvent Skipping;

    public void OnSkipClicked()
    {
        gameControl.replayCount = 0;
        if (Skipping.GetPersistentEventCount() == 0)
        {
            StartCoroutine(investigationControl.EndInvestigation());
        }
        else
        {
            Skipping.Invoke();
        }
    }

    public void CheckCanSkip()
    {
        if (gameControl.replayCount >= 2 || gameControl.isTester == true || gameControl.stageClear.Contains("End4") || gameControl.stageClear.Contains("End5") || gameControl.stageClear.Contains("End6") || gameControl.stageClear.Contains("End7"))
        {
            gameObject.SetActive(!HasRestrictedActiveChild());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public bool HasRestrictedActiveChild()
    {
        // Iterate through all children of gamePanel
        foreach (Transform child in gamePanel.transform)
        {
            // Check if the child is active and its name matches any name in the list
            if (child.gameObject.activeSelf && cantSkipGameList.Contains(child.name))
            {
                return true; // Match found
            }
        }
        return false; // No matches found
    }
}
