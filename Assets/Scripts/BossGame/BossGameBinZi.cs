using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem; // Make sure to include the DOTween namespace
using UnityEngine.UI;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class BossGameBinZi : MonoBehaviour
{
    public RectTransform sceneRectTranf;
    public GameObject inventoryBag;
    public BossGameManager bossGameControl;
    public int tapMaxTime = 5; // Set the maximum number of taps allowed
    public float moveDuration = 1f; // Duration of movement to goal position
    public Vector2 goalPosition; // Set this to the desired goal position
    public Vector2 originalPosition; // Original position to return to
    public Vector3 originalScale;
    public Vector3 targetScale = new Vector3(2f, 2f, 1f); // Scale to increase to

    /*public List<Button> buttonList; */

    private int tapCount;
    private bool isActive;
    private float randomTime;

    private RectTransform currentPos;

    public Image BinZiImg;
    public Sprite BinZiNormalSprite;
    public Sprite BinZiClickedSprite;

    public float backOffDistance = 30f; // The distance to move back when clicked
    public float scaleReductionFactor = 0.8f; // How much to shrink when tapped
    public float scaleDuration = 0.2f; // Duration of the scaling animation
    public PlayableDirector linkedTimeline;

    private void OnEnable()
    {
        BinZiImg = this.gameObject.GetComponent<Image>();
        BinZiImg.sprite = BinZiNormalSprite;
        currentPos = GetComponent<RectTransform>();
        tapCount = 0;
        originalPosition = GetComponent<RectTransform>().anchoredPosition; // Store the original position
        originalScale = GetComponent<RectTransform>().localScale;
        StartCoroutine(SpawnGhost());
    }

    private IEnumerator SpawnGhost()
    {
        randomTime = Random.Range(3f, 6f); // Random time range for appearance
        yield return new WaitForSeconds(randomTime);

        // Fade in the ghost
        SoundControl.SoundManager.PlaySfx("binzi_scream");
        BinZiImg.DOFade(1f, 0.5f); // Fade to full opacity over 0.5 seconds
        ActivateGhost();
    }

    public void ActivateGhost()
    {
        inventoryBag.SetActive(false);
        GetComponent<Button>().interactable = true;
        BinZiImg.sprite = BinZiNormalSprite;
        BinZiImg.raycastTarget = true;

        isActive = true;
        
        currentPos.DOAnchorPos(goalPosition, moveDuration).OnComplete(OnReachGoal);
        currentPos.DOScale(targetScale, moveDuration); // Scale to the target scale
    }

    private void OnDisable()
    {
        GetComponent<Button>().interactable = false;
        BinZiImg.raycastTarget = false;
        currentPos.DOKill();

        // Deactivate ghost logic
        isActive = false;
        tapCount = 0; // Reset tap count
        // Move back to the original position

        currentPos.localScale = originalScale;
        currentPos.anchoredPosition = originalPosition;
        BinZiImg.DOFade(0f, 0.5f);
        BinZiImg.raycastTarget = false;
    }

    private void SpawnFu()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log($"X:{mousePosition.x}, Y:{mousePosition.y}");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            sceneRectTranf,
            mousePosition,
            null, // or parentCanvas.worldCamera if Screen Space - Camera
            out Vector2 localPoint
        );
        GameObject spawnedObject = Instantiate(bossGameControl.Fu);
        spawnedObject.transform.SetParent(sceneRectTranf);
        spawnedObject.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }
    public void TapButtonClick()
    {
        bossGameControl.MinusFu();
        if (bossGameControl.fuNum > 0)
        {
#if !(UNITY_ANDROID || UNITY_IOS)
            SpawnFu();
#endif
            // Change the sprite to indicate the ghost has been tapped
            BinZiImg.sprite = BinZiClickedSprite;

            if (isActive)
            {
                
                tapCount++;

                // Stop the ongoing animations
                currentPos.DOKill();

                // Shrink the ghost a bit, then scale back to the target scale
                Vector3 shrunkenScale = targetScale * scaleReductionFactor;
                currentPos.DOScale(shrunkenScale, scaleDuration / 2)
                    .SetEase(Ease.OutCubic).OnComplete(() =>
                          currentPos.DOScale(targetScale, scaleDuration / 2).SetEase(Ease.OutCubic));

                // Move the ghost back a little
                Vector2 backOffPosition = currentPos.anchoredPosition - (goalPosition - currentPos.anchoredPosition).normalized * backOffDistance;
                currentPos.DOAnchorPos(backOffPosition, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    ActivateGhost();
                });

                // Check if the maximum number of taps has been reached
                if (tapCount >= tapMaxTime)
                    DeactivateGhost();
            }
        }
    }

    public void DeactivateGhost()
    {
        GetComponent<Button>().interactable = false;
        BinZiImg.raycastTarget = false;
        currentPos.DOKill();

        // Deactivate ghost logic
        isActive = false;
        tapCount = 0; // Reset tap count
                      // Move back to the original position

        currentPos.DOScale(originalScale, moveDuration/2); // Scale to the target scale
        currentPos.DOAnchorPos(originalPosition, moveDuration/2).OnComplete(() =>
        {
            if(gameObject.activeInHierarchy)
             StartCoroutine(SpawnGhost());
            /*if (buttonList.Count > 0)
            {
                foreach (Button btn in buttonList)
                {
                    if(btn !=null)
                    btn.interactable = true;
                }
            }*/
            BinZiImg.DOFade(0f, 0.5f);
            BinZiImg.raycastTarget = false;
        });
    }

    private void OnReachGoal()
    {
        if (isActive)
        {
            isActive = false;
            if (linkedTimeline != null)
                bossGameControl.gameOverEffect = linkedTimeline;
            bossGameControl.LoseGame();
        }
    }


}
