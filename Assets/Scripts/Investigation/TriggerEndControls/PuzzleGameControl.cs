using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PuzzleGameControl : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;

    //[SerializeField] private int itemsNumNeed;
    //private int itemsCount;

    public List<string> needTasks;
    [SerializeField] private List<string> completedTasks;

    public List<Image> changeStranges;

    public bool isReturningFromDialogue = false;
    [SerializeField]private bool isPanelActive = false;

    [SerializeField] private bool noNeedJumpOut;

    public void AddCompletedTask(string taskName)
    {
        if (!completedTasks.Contains(taskName) && needTasks.Contains(taskName))
            completedTasks.Add(taskName);

        if (changeStranges.Count > 0)
        {
            StartCoroutine(ActiveStrange());
        }

        if (noNeedJumpOut)
            CheckComplete();
    }

    public void SetPanelActive(bool isActive)
    {
        isPanelActive = isActive;

        if (isPanelActive && isReturningFromDialogue)
        {
            CheckComplete();
            isReturningFromDialogue = false; // Reset for the next interaction
        }
    }

    public void CheckComplete()
    {
        if (completedTasks.Count >= needTasks.Count)
        {
            StartCoroutine(investigationControl.EndInvestigation());
        }
    }

    private IEnumerator ActiveStrange()
    {
        foreach (Image changeStrange in changeStranges)
        {
            bool result = Random.value > 0.5f;

            if (!result)
            {
                changeStrange.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);
                changeStrange.DOFade(0f, 4f);
            }
        }
    }
}
