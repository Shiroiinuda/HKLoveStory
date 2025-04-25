using System.Collections;
using Investigation;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] InvestigationControl investigationControl;
    [SerializeField] private float playSpeed;
    public VideoPlayer videoPlayer;

    public int endTime;
    private int remainningTime;
    public int goalPress;
    [SerializeField] private int countPress;

    [SerializeField]private int winJumpMark;
    [SerializeField]private int loseJumpMark;
    [SerializeField] private DialogueManager dialogueManager;

    private int seconds = 3;

    [SerializeField] private GameObject tutText;

    public Slider slider;

    public TextMeshProUGUI timerCountText;

    private void Start()
    {
        NextFrame();
        tutText.SetActive(true);
        countPress = 0;
        remainningTime = endTime;
        timerCountText.text = endTime.ToString();

        //UIs
        slider.maxValue = goalPress;
        slider.value = countPress;
        var roads = dialogueManager.currentDialogue.roads;
        winJumpMark = int.Parse(roads.road1);
        loseJumpMark = int.Parse(roads.road2);

        StartCoroutine(TimeCountDown());
    }

    private IEnumerator TimeCountDown()
    {
        yield return new WaitForSeconds(1f);
        remainningTime -= 1;
        timerCountText.text = remainningTime.ToString();

        if (countPress >= 1)
            seconds -= 1;

        if (remainningTime <= 0)
        {
            if(countPress >= goalPress)
            {
                dialogueManager.jumpMark = winJumpMark;
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
            }
            else
            {
                dialogueManager.jumpMark = loseJumpMark;
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
            }
        }
        else
            StartCoroutine(TimeCountDown());


        if (seconds <= 0)
        {
            seconds = 3;
            dialogueManager.jumpMark = loseJumpMark;
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }
    }

    public void NextFrame()
    {
        tutText.SetActive(false);
        seconds = 3;
        countPress += 1;
        videoPlayer.time += playSpeed;
        slider.value = countPress;
        CheckVideoEnd();
    }

    private void CheckVideoEnd()
    {
        if (videoPlayer.time + playSpeed >= videoPlayer.length)
        {
            videoPlayer.time = 0;
        }
    }
}
