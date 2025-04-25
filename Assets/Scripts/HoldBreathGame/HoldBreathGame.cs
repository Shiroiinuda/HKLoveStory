using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class HoldBreathGame : MonoBehaviour
{
    public AudioManager audioManager;

    public InvestigationControl investigationControl;

    private float currentAirAmount;
    public float maxAirAmount = 10f;
    public Slider slider;

    public float breathDecreaseRate = 0.1f; // Amount to decrease currentBreathTime per 0.1 second

    [SerializeField] private bool ghostCome = false;
    [SerializeField] private bool isPressing = false;

    public PlayableDirector fongLegDirector;

    public List<AudioClip> releaseSound;

    private bool firstHold = false;
    public Animator HoldingBreathAnimation;

    public PlayableDirector gameOverEffect;

    private void Start()
    {
        currentAirAmount = maxAirAmount;
        //UIs
        slider.maxValue = maxAirAmount;
        slider.value = maxAirAmount;
    }

    public void ActivateGhostCome()
    {
        ghostCome = true;
        StartCoroutine(CheckPressing());
    }

    public void DeactivateGhostCome()
    {
        ghostCome = false;
    }

    public IEnumerator CheckPressing()
    {
        if (investigationControl.wonGame == false)
        {
            if (ghostCome)
            {
                if (isPressing)
                {
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(CheckPressing());
                }
                else
                {
                    GameOver();
                }

            }
        }
        else
            fongLegDirector.Stop();
    }

    // This method is called when the UI button is pressed and held down continuously
    public void OnButtonHold()
    {
        if(firstHold == false)
        {
            HoldingBreathAnimation.SetTrigger("Up");
            firstHold = true;
        }

        isPressing = true;
        StartCoroutine(DecreaseBreathOverTime());
    }

    // This method is called when the UI button is released
    public void OnButtonRelease()
    {
        HoldingBreathAnimation.SetTrigger("Down");
        firstHold = false;
        if (releaseSound != null)
        {
            int clickSoundNum = Random.Range(0, releaseSound.Count);
            audioManager.PlaySFX(releaseSound[clickSoundNum]);
        }
        isPressing = false;
        StartCoroutine(AddBreathOverTime());
    }

    private IEnumerator DecreaseBreathOverTime()
    {
        if (investigationControl.wonGame == false)
        {
            if (isPressing == true)
            {
                // Decrease the currentBreathTime by breathDecreaseRate
                currentAirAmount -= breathDecreaseRate;
                ShowCurrentAirAmount();

                // Add any additional logic or checks you need here, e.g., game over condition if currentBreathTime <= 0
                if (currentAirAmount <= 0f)
                {
                    // Game over logic
                    GameOver();
                }

                // Wait for 0.1 seconds before decreasing breath again
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(DecreaseBreathOverTime());
            }
        }
    }

    private IEnumerator AddBreathOverTime()
    {
        if (investigationControl.wonGame == false)
        {
            if (isPressing == false)
            {
                if (currentAirAmount < maxAirAmount)
                {
                    // Decrease the currentBreathTime by breathDecreaseRate
                    currentAirAmount += breathDecreaseRate;
                    ShowCurrentAirAmount();

                    // Wait for 0.1 seconds before decreasing breath again
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(AddBreathOverTime());
                }
            }
        }
    }

    public void ShowCurrentAirAmount()
    {
        slider.value = currentAirAmount;
    }

    private void GameOver()
    {
        investigationControl.endGameBlocker.SetActive(true);
        audioManager.StopSfx();
        fongLegDirector.Stop();
        if (gameOverEffect != null)
        {
            gameOverEffect.gameObject.SetActive(true);
            gameOverEffect.Play();
        }
    }

    public void EndGame()
    {
        audioManager.StopSfx();
        StartCoroutine(investigationControl.EndInvestigation());
    }
}
