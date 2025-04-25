using System;
using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Investigation
{
    public class AngerGameControl : MonoBehaviour
    {
        [Header("Scripts")]
        public DialogueManager dialogueManager;
        public InvestigationControl investigationControl;

        [Header("Game Settings")]
        public float gameDuration = 60f; // Time to win the game
        public float angerIncreaseRate = 0.1f; // Rate at which angerValue increases
        public float angerDecreaseAmount = 0.05f; // Amount angerValue decreases when the button is pressed
        public float angerMaxValue = 1f; // Maximum value of angerValue
        public float smoothDuration = 0.5f; // Duration for slider smoothing
        private int winJumpMark;
        private int loseJumpMark;

        [Header("UI Elements")]
        public Slider angerSlider; // Reference to the UI slider
        public Button reduceAngerButton; // Reference to the button
        public Image stageImage; // Image component to display sprites
        public Sprite[] sprites; // Array of sprites for different stages

        private float angerValue = 0f; // Current anger value
        private float elapsedTime = 0f; // Time elapsed since the start of the game
        private bool gameRunning = true; // To check if the game is still running
        private const string clickSfx = "Menu Click";

        private void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)DecreaseAnger();
            if(Keyboard.current.spaceKey.wasPressedThisFrame)DecreaseAnger();
        }

        private void Start()
        {
            if (dialogueManager.currentDialogue.roads.road1 != "")
                winJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);

            if (dialogueManager.currentDialogue.roads.road2 != "")
                loseJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);

            // Initialize slider
            angerSlider.maxValue = angerMaxValue;
            angerSlider.value = angerValue;

            // Bind button click event
            reduceAngerButton.onClick.AddListener(DecreaseAnger);

            // Start the game timer and anger increase mechanics
            InvokeRepeating(nameof(IncreaseAngerOverTime), 0f, 0.1f); // Increase anger every 0.1 seconds
            StartCoroutine(TrackElapsedTime()); // Start tracking elapsed time
        }

        private IEnumerator TrackElapsedTime()
        {
            while (gameRunning && !investigationControl.wonGame)
            {
                elapsedTime += Time.deltaTime;
                
                if (elapsedTime >= gameDuration)
                {
                    float segmentDuration = angerMaxValue / sprites.Length; // Divide the game duration into 4 equal segments

                    if (angerValue <= (sprites.Length/1) *segmentDuration)
                        WinGame();
                    else
                        LoseGame();
                }

                yield return null;
            }
        }

        private void IncreaseAngerOverTime()
        {
            if (!gameRunning && investigationControl.wonGame) return;

            SetAngerValue(angerValue + angerIncreaseRate);
            UpdateSprite();
            if (angerValue >= angerMaxValue)
            {
                LoseGame();
            }
        }

        private void DecreaseAnger()
        {
            if (!gameRunning && investigationControl.wonGame) return;
            SoundControl.SoundManager.PlaySfx(clickSfx);
            UpdateSprite();
            SetAngerValue(angerValue - angerDecreaseAmount);
        }

        private void SetAngerValue(float newValue)
        {
            newValue = Mathf.Clamp(newValue, 0, angerMaxValue);

            // Start a coroutine to smoothly update the slider
            StartCoroutine(SmoothSliderChange(angerSlider.value, newValue));
            angerValue = newValue;
        }

        private IEnumerator SmoothSliderChange(float startValue, float targetValue)
        {
            float elapsed = 0f;

            while (elapsed < smoothDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / smoothDuration;
                angerSlider.value = Mathf.Lerp(startValue, targetValue, t);
                yield return null;
            }

            angerSlider.value = targetValue; // Ensure the final value is accurate
        }

        private void UpdateSprite()
        {
            if (!gameRunning && investigationControl.wonGame) return;

            // Calculate which sprite to display

            float segmentDuration = angerMaxValue / sprites.Length;

            // Determine the sprite index based on angerValue
            int index = Mathf.Clamp((int)(angerValue / segmentDuration), 0, sprites.Length - 1);

            // Update the sprite
            stageImage.sprite = sprites[index];
        }

        private void WinGame()
        {
            if (!gameRunning && investigationControl.wonGame) return;

            gameRunning = false;
            CancelInvoke(nameof(IncreaseAngerOverTime));
            dialogueManager.jumpMark = winJumpMark;
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }

        private void LoseGame()
        {
            if (!gameRunning && investigationControl.wonGame) return;

            gameRunning = false;
            CancelInvoke(nameof(IncreaseAngerOverTime));
            dialogueManager.jumpMark = loseJumpMark;
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }
    }
}
