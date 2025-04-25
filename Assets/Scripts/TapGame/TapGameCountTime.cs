using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TapGameCountTime : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InvestigationControl investigationControl;
    [SerializeField] private AudioManager audioManager;
    public int maxClickNum;
    public List<AudioClip> clickSound;
    public Button tapBtn;
    public Animator clickAnimation;

    public bool haveGameOver;

    public int winJumpMark;
    public int loseJumpMark;

    public int count;

    //count time
    public float maxTime;
    private float currentTime;
    public Slider slider;
    public float timeDecreaseRate = 0.1f; // Amount to decrease currentBreathTime per 0.1 second

    public PlayableDirector gameOverEffect;


    private void Start()
    {
        if (dialogueManager.currentDialogue.roads.road1 != "")
            winJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);

        if (dialogueManager.currentDialogue.roads.road2 != "")
            loseJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);

        currentTime = maxTime;
        //UIs
        if (slider != null)
        {
            slider.maxValue = maxTime;
            slider.value = maxTime;
        }

        count = 0;
        tapBtn.enabled = true;

        StartCoroutine(DecreaseTime());
    }

    public void OnButtonClicked()
    {
        count += 1;

        if (clickAnimation != null)
        {
            clickAnimation.SetTrigger("Click");
        }

        if (clickSound.Count > 0)
        {
            int clickSoundNum = Random.Range(0, clickSound.Count);
            audioManager.PlaySFX(clickSound[clickSoundNum]);
        }

        if (count >= maxClickNum)
        {
            audioManager.StopSfx();
            tapBtn.enabled = false;
            dialogueManager.jumpMark = winJumpMark;
            Debug.Log("dialogueManager.jumpMark = " + winJumpMark);
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }
    }

    private IEnumerator DecreaseTime()
    {
        if (investigationControl.wonGame == false)
        {
            // Decrease the currentBreathTime by breathDecreaseRate
            currentTime -= timeDecreaseRate;
            ShowCurrentTime();

            // Add any additional logic or checks you need here, e.g., game over condition if currentBreathTime <= 0
            if (currentTime <= 0f)
            {
                // Game over logic
                if (haveGameOver)
                    GameOver();
                else
                {
                    dialogueManager.jumpMark = loseJumpMark;
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
            }
            else
            {
                // Wait for 0.1 seconds before decreasing breath again
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(DecreaseTime());
            }
        }
    }

    private void GameOver()
    {
        investigationControl.endGameBlocker.SetActive(true);
        audioManager.StopSfx();
        if (gameOverEffect != null)
        {
            gameOverEffect.gameObject.SetActive(true);
            gameOverEffect.Play();
        }
    }

    public void ShowCurrentTime()
    {
        if (slider != null)
            slider.value = currentTime;
    }
}
