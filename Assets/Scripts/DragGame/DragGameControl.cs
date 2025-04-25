using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.Playables;

public class DragGameControl : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;

    [SerializeField] private int goalNum;

    public int draggedNum;
    
    public PlayableDirector dragGameDirector;

    public void CheckWinGame()
    {
        draggedNum += 1;

        if (draggedNum >= goalNum)
        {
            WinGame();
        }

        if(dragGameDirector != null)
        {
            dragGameDirector.Play();
        }
    }

    private void WinGame()
    {
        StartCoroutine(investigationControl.EndInvestigation());
    }
}
