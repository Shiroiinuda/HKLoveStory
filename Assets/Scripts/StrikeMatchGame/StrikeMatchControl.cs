using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeMatchControl : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;
    [SerializeField] private InventoryManager inventoryManager;
    public AudioManager audioManager;

    public GameObject matchButton;

    public List<InventoryItems> matchesList; // List of all matches
    public GameObject draggableMatch; // Draggable match object
    private Image draggableMatchImg;
    public Sprite matchSprite; 
    public Sprite firedMatchSprite;
    public InventoryItems firedMatch;

    private InventoryItems inputMatch;
    public bool startStrike = false;
    public bool endStrike = false;
    private float maxStrikeTime = 2f;

    public bool isFired = false;

    private Coroutine resetStrikeCoroutine;
    public AudioClip fireMatchSfx;

    [SerializeField]private int strikeCount = 0;

    void Start()
    {
        draggableMatchImg = draggableMatch.GetComponent<Image>();
        matchButton.gameObject.SetActive(true);
        draggableMatch.SetActive(false);
    }

    public void OnMatchButtonClick()
    {
        strikeCount = 0;
        if (inventoryManager.items.Count > 0)
        {
            for (int i = 0; i < inventoryManager.items.Count; i++)
            {
                for (int j = 0; j < matchesList.Count; j++)
                {
                    if (inventoryManager.items[i] == matchesList[j])
                    {
                        inputMatch = inventoryManager.items[i];
                        draggableMatch.SetActive(true);
                        matchButton.SetActive(false);
                        return;
                    }
                }
            }
        }
    }

    public void StartResetStrike()
    {
        // Check if coroutine is already running to avoid starting it again
        if (resetStrikeCoroutine == null)
        {
            resetStrikeCoroutine = StartCoroutine(ResetStrike());
        }
    }

    // Call this method to stop the coroutine
    public void StopResetStrike()
    {
        if (resetStrikeCoroutine != null)
        {
            StopCoroutine(resetStrikeCoroutine);
            resetStrikeCoroutine = null;
        }
    }

    private IEnumerator ResetStrike()
    {
        yield return new WaitForSeconds(maxStrikeTime);
        startStrike = false;
        endStrike = false;

        resetStrikeCoroutine = null;
    }

    public void CheckResult()
    {
        PlaySoundIfNotNull(fireMatchSfx);

        if (startStrike && endStrike)
        {
            strikeCount++;
            bool result = Random.value > 0.5f; // Random chance for successful strike
            if (result)
            {
                if (strikeCount == 1)
                {
                    gameControl.OnUnlockAchievement("MatchBoy");
                }
                HandleSuccessfulStrike();
            }
            else
            {
                startStrike = false;
                endStrike = false;
            }
        }
    }

    private void HandleSuccessfulStrike()
    {
        draggableMatchImg.sprite = firedMatchSprite;
        isFired = true;
    }

    public void AddFiredMatchToBag()
    {
        // Remove inputMatch from bag and add firedMatch
        matchesList.Remove(inputMatch);
        inventoryManager.RemoveItem(inputMatch);

        inventoryManager.AddItem(firedMatch);
        inventoryManager.SetGetItemPenel(firedMatch);

        ResetGameValues();
    }

    private void ResetGameValues()
    {
        draggableMatchImg.sprite = matchSprite;
        matchButton.gameObject.SetActive(true);
        draggableMatch.SetActive(false);
        inputMatch = null;
        startStrike = false;
        endStrike = false;
        maxStrikeTime = 2f;
        isFired = false;
    }

    private void PlaySoundIfNotNull(AudioClip clip)
    {
        if (clip != null)
            audioManager.PlaySFX(clip);
    }
}
