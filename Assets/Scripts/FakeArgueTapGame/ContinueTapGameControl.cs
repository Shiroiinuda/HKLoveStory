using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Investigation;
using EasierLocalization;

namespace Investigation
{
    public class ContinueTapGameControl : MonoBehaviour
    {
        public GameControl gameControl;
        public InvestigationControl investigationControl;
        public AudioManager audioManager;
        public TapButton[] buttons;
        private int currentButtonIndex = 0;

        void Start()
        {
            SetupButtons();
        }

        void SetupButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i; // Capture index for lambda
                buttons[i].button.onClick.AddListener(() => OnButtonTapped(index));
                buttons[i].button.gameObject.SetActive(i == 0); // Only the first button is visible
            }
        }

        void OnButtonTapped(int index)
        {
            if (index != currentButtonIndex) return; // Ignore taps on inactive buttons

            buttons[index].currentTaps++;
            if (buttons[index].buttonSfx != null) audioManager.PlayButtonSFX(buttons[index].buttonSfx);
            if (buttons[index].currentTaps >= buttons[index].tapRequirement)
                HandleButtonComplete(index);
        }

        void HandleButtonComplete(int index)
        {
            float delay = 0f;
                        
            if (buttons[index].buttonSfx != null)
            {
                audioManager.PlayButtonSFX(buttons[index].buttonSfx);
                delay = buttons[index].buttonSfx.length;
            }

            if (buttons[index].animator != null)
            {
                buttons[index].animator.SetTrigger(buttons[index].setTriggerName);
                delay = 0.5f;
            }

            if (Localization.NpcVoiceLan() == "CN")
            {
                if (buttons[index].sfx_CN != null)
                {
                    audioManager.PlaySFX(buttons[index].sfx_CN);
                    delay = buttons[index].sfx_CN.length;
                }
            }
            else
            {
                if (buttons[index].sfx != null)
                {
                    audioManager.PlaySFX(buttons[index].sfx);
                    delay = buttons[index].sfx.length;
                }
            }


            buttons[index].button.gameObject.SetActive(false); // Hide completed button
            currentButtonIndex++;

            Invoke("AfterPlaySound", delay);
        }

        void WinGame()
        {
            gameControl.OnUnlockAchievement("SingBossGame");
            StartCoroutine(investigationControl.EndInvestigation());
        }

        void AfterPlaySound()
        {
            if (currentButtonIndex < buttons.Length)
            {
                buttons[currentButtonIndex].button.gameObject.SetActive(true); // Show next button
            }
            else
            {
                WinGame(); // All buttons completed
            }
        }
    }

    [System.Serializable]
    public class TapButton
    {
        public Button button;  // The UI button
        public int tapRequirement;  // Number of taps required
        public Animator animator;  // Animator for animations
        public string setTriggerName;
        public AudioClip buttonSfx;  // Sound effect
        public AudioClip sfx;  // Sound effect
        public AudioClip sfx_CN;  // Sound effect
        [HideInInspector] public int currentTaps = 0;
    }
}
