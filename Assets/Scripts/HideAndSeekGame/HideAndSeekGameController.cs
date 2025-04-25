using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class HideAndSeekGameController : MonoBehaviour
{
    public AudioManager audioManager;

    public GameObject ghost;
    //public Sprite ghostLookingSprite;
    //public Sprite ghostNormalSprite;

    public Button moveButton;
    public Image background;

    //private bool isGhostLooking;
    private bool isGameActive;


    public PlayableDirector gameOverEffect;

    //UIs
    public Slider slider;
    public float maxSoundAmount = 10f;
    public float SoundDecreaseRate = 0.1f;
    private float currentSoundAmount;

    [SerializeField] private InvestigationControl investigationControl;

    public AudioClip FootSFX;

    private int targetClick = 46;
    [SerializeField]private int clickCount;

    private void Start()
    {
        clickCount = 0;
        currentSoundAmount = 0;
        //UIs
        slider.maxValue = maxSoundAmount;
        slider.value = maxSoundAmount;

        // Initialize game state
        //isGhostLooking = false;
        isGameActive = true;

        // Add button click event listener
        moveButton.onClick.AddListener(MoveButtonClicked);

        // Start the meter update coroutine
        //StartCoroutine(UpdateMeter());
        StartCoroutine(DecreaseVolumeOverTime());
    }

    //private IEnumerator UpdateMeter()
    //{
    //    if (investigationControl.wonGame == false)
    //    {
    //        while (isGameActive)
    //        {
    //            // Randomly check if ghost is looking at the player
    //            if (Random.Range(0, 50) < 20) // Adjust the probability as per your preference
    //            {
    //                isGhostLooking = true;
    //                ghost.GetComponent<Image>().sprite = ghostLookingSprite;
    //            }
    //            else
    //            {
    //                isGhostLooking = false;
    //                ghost.GetComponent<Image>().sprite = ghostNormalSprite;
    //            }

    //            yield return new WaitForSeconds(1);
    //        }
    //    }
    //}

    private void MoveButtonClicked()
    {
        clickCount += 1;
        audioManager.PlayButtonSFX(FootSFX);

        if (investigationControl.wonGame == false)
        {
            if (isGameActive)
            {
                currentSoundAmount += 1;
                ShowCurrentAirAmount();

                // Check if the meter value exceeds the threshold
                if (currentSoundAmount >= maxSoundAmount)
                {
                    // Game over condition
                    GameOver();
                    isGameActive = false;
                }
                else
                {
                    // Move background image to the right (player moving to the left)
                    background.transform.position += new Vector3(-40f, 0f, 0f);
                    CheckWinCondition();
                }
            }
        }
    }

    private void CheckWinCondition()
    {

        // Check if the background image has moved completely to the right
        if (clickCount >= targetClick) // Adjust the threshold as per your level design
        {
            StartCoroutine(investigationControl.EndInvestigation());
            isGameActive = false;
        }
    }

    private IEnumerator DecreaseVolumeOverTime()
    {
        if (investigationControl.wonGame == false)
        {
            if (currentSoundAmount <= 0)
            {
                currentSoundAmount = 0;
            }
            else
            {
                // Decrease the currentBreathTime by breathDecreaseRate
                currentSoundAmount -= SoundDecreaseRate;
            }

            ShowCurrentAirAmount();

            // Wait for 0.1 seconds before decreasing breath again
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DecreaseVolumeOverTime());
        }
    }

    public void ShowCurrentAirAmount()
    {
        slider.value = currentSoundAmount;
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
