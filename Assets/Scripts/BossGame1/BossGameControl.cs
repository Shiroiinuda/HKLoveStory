using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.Playables;

public class BossGameControl : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;

    [SerializeField]
    private bool fromLeft;

    [SerializeField]
    private bool fromRight;

    public List<PlayableDirector> fongTimelineList = new List<PlayableDirector>();

    public PlayableDirector fongTimeline;

    public int targetNum;
    public int currentNum;

    private int timelineNum;

    private IEnumerator ghostAppearCoroutine;
    private bool startedCoroutine;

    //[SerializeField] private GameObject tutText;

    public bool istut;

    public PlayableDirector gameOverEffect;

    private void Start()
    {
        startedCoroutine = false;
        fromLeft = false;
        fromRight = false;
        currentNum = 0;
        OnWallEnable();
        StartCoroutine(CheckInvestigationWonGame());
    }

    //
    public void OnLeftClicked()
    {
        if (fromLeft)
        {
            BossGameOver();
        }
        else
        {
            if (fongTimeline != null)
            {
                fongTimeline.Stop();
                fongTimeline.time = 0;
                fongTimeline.Evaluate();
            }
            fromRight = false;
            OnWallEnable();
        }

    }

    public void OnRightClicked()
    {
        if (fromRight)
        {
            BossGameOver();
        }
        else
        {
            if (fongTimeline != null)
            {
                fongTimeline.Stop();
                fongTimeline.time = 0;
                fongTimeline.Evaluate();
            }
            fromLeft = false;
            OnWallEnable();
        }
    }

    public void OnWallEnable()
    {
        if (istut == false)
        {
            if (startedCoroutine)
            {
                StopCoroutine(ghostAppearCoroutine);
            }
            ghostAppearCoroutine = GhostAppear();
            StartCoroutine(ghostAppearCoroutine);
        }
    }

    //

    private IEnumerator GhostAppear()
    {
        startedCoroutine = true;
        yield return new WaitForSeconds(Random.Range(2, 5));
        timelineNum = Random.Range(0, 2);
        fongTimeline = fongTimelineList[timelineNum];
        if (fongTimeline != null)
        {
            fongTimeline.Play();
        }
    }

    private IEnumerator CheckInvestigationWonGame()
    {
        if(investigationControl.wonGame == true)
        {
            if (fongTimeline != null)
                fongTimeline.Stop();
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(CheckInvestigationWonGame());
        }

    }

    public void CheckWin()
    {
        if (currentNum == targetNum)
        {
            if (fongTimeline != null)
                fongTimeline.Stop();
            StartCoroutine(investigationControl.EndInvestigation());
        }
    }

    public void GhostFromLeft()
    {
        fromLeft = true;
    }

    public void GhostFromRight()
    {
        fromRight = true;
    }

    public void BossGameOver()
    {
        GameOver();
    }

    private void GameOver()
    {
        investigationControl.endGameBlocker.SetActive(true);
        if (gameOverEffect != null)
        {
            gameOverEffect.gameObject.SetActive(true);
            gameOverEffect.Play();
        }
    }
}
