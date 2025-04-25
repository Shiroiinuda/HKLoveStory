using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PressMoveGameControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private InvestigationControl investigationControl;
    [SerializeField] private AudioManager audioManager;

    public AudioClip moveSound;

    public RectTransform moveImg;
    public RectTransform targetPositionRect;

    [SerializeField] private float moveDuration = 0.3f; // Duration of movement

    private bool isPressing = false; // Track pressing state
    private bool isWin = false;
    private Tween currentTween; // Reference to the active tween
    [SerializeField] private float remainingTime; // Time left for the tween
    private float pressStartTime; // Time when the press started
    private Coroutine moveSoundCoroutine;

    private void Start()
    {
        remainingTime = moveDuration;
    }

    // Unity EventSystem interface to detect pointer down
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isWin) return; // If already won, ignore further input

        isPressing = true;
        pressStartTime = Time.time; // Record the time when pressing starts

        // Play sound effect
        if (moveSound != null && moveSoundCoroutine == null)
        {
            moveSoundCoroutine = StartCoroutine(LoopMoveSound());
        }

        StartCoroutine(PressAndMoveCoroutine());
    }

    // Unity EventSystem interface to detect pointer up
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPressing) return; // If not pressing, ignore

        isPressing = false;

        CheckTime();

        // Stop looping the move sound
        if (moveSoundCoroutine != null)
        {
            StopCoroutine(moveSoundCoroutine);
            moveSoundCoroutine = null;
        }

        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Pause();
        }
    }

    private IEnumerator PressAndMoveCoroutine()
    {
        while (isPressing && !isWin)
        {
            MoveObjectStep();
            yield return new WaitForSeconds(remainingTime); // Prevent spamming moves
        }
    }

    private void MoveObjectStep()
    {
        CheckTime();

        // Stop any ongoing tween to prevent overlapping tweens
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // Move to target position with DOTween
        currentTween = moveImg.DOAnchorPos(targetPositionRect.anchoredPosition, remainingTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => OnWin());
    }

    private void OnWin()
    {
        isWin = true;
        if (currentTween != null)
        {
            currentTween.Kill(); // Stop any ongoing tween
        }
        StartCoroutine(investigationControl.EndInvestigation());
    }

    private IEnumerator LoopMoveSound()
    {
        while (isPressing)
        {
            audioManager.PlaySFX(moveSound);
            yield return new WaitForSeconds(moveSound.length); // Wait for the sound to finish before replaying
        }
    }

    private void CheckTime()
    {
        float pressDuration = Time.time - pressStartTime;
        remainingTime = Mathf.Max(0, remainingTime - pressDuration); // Ensure remaining time is non-negative

        if (remainingTime <= 0)
            OnWin();
    }
}