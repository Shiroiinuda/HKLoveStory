using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

namespace Chapters
{
    public class ChapterManager : ChapterMono
    {
        public string chapterID;

        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioClip lockSFX;
        [SerializeField] private AudioClip clickSFX;

        [SerializeField] private LoadSceneManager loadSceneManager;
        [SerializeField] private GameControl gameControl;
        [SerializeField] private CurrencyManager currencyManager;

        [SerializeField] private GameObject confirmPayPanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private Animator NoCurrencyNotice;

        [SerializeField] private GameObject costPanel;

        [SerializeField] private Button payButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button backButton;


        public int bookmark;
        public List<string> previousChapter;

        [SerializeField] private GameObject lockImg;

        private bool canClick;


        /*public RawImage rawImage;
        public VideoPlayer videoPlayer;*/

        protected override void Start()
        {
            base.Start();
#if !(UNITY_ANDROID || UNITY_IOS)
            costPanel.SetActive(false);
#endif
            /*if (rawImage != null && videoPlayer != null)
            {
                rawImage.texture = videoPlayer.texture;
            }*/
        }

        protected override void CheckStageClear()
        {
            canClick = gameControl.isTester;

            if (bookmark > 0)
            {
                foreach (var chapter in previousChapter)
                {
                    bool hasMatch = CheckForMatch();
                    chapterUI.enabled = hasMatch;
                    chapterNameUI.enabled = hasMatch;
                    lockImg.SetActive(!hasMatch);
                    canClick = hasMatch;
                    gameObject.GetComponent<Button>().interactable = hasMatch;
                }
            }
            else if (bookmark == 0)
            {
                canClick = true;
                chapterUI.enabled = true;
                chapterNameUI.enabled = true;
                lockImg.SetActive(!true);
                canClick = true;
            }
#if UNITY_ANDROID || UNITY_IOS
            if (gameControl.unLockedSavept.Contains(chapterKeyName))
                costPanel.SetActive(false);
            else
            {
                if (bookmark > 0)
                    costPanel.SetActive(true);
                else
                    costPanel.SetActive(false);
            }
#endif
        }


        public void OnChapterClicked()
        {
            gameControl.replayCount = 0;

            if (bookmark == 0)
            {
                audioManager.PlayButtonSFX(clickSFX);
                gameControl.currentBookmark = 0;

                gameControl.isChoiceLoop = false;
                gameControl.loopChoiceCounter = 0;
                gameControl.loopChoiceJumpmark = 0;

                loadSceneManager.FadeToLevel(3);
                return;
            }
            else
            {
                // If a checkSavePt object with bookmark was found, check enough currency and do the purchasing
                if (!gameControl.unLockedSavept.Contains(chapterKeyName)) //Savept not yet unlocked and can click
                {
                    audioManager.PlayButtonSFX(lockSFX);

                    if (canClick)
                    {
#if !(UNITY_ANDROID || UNITY_IOS)
                        UnlockChapter();
#else
                        if (gameControl.currency >= 10)
                        {
                            confirmPayPanel.SetActive(true);
                            payButton.onClick.AddListener(UnlockChapter);
                            noButton.onClick.AddListener(OnConfirmPayPanelClose);
                            backButton.onClick.AddListener(OnConfirmPayPanelClose);
                        }
                        else
                        {
                            shopPanel.SetActive(true);
                            NoCurrencyNotice.SetTrigger("NoCurrency");
                        }
#endif
                    }
                }
                else //Savept unlocked
                {
                    audioManager.PlayButtonSFX(clickSFX);
                    gameControl.currentBookmark = bookmark;

                    gameControl.isChoiceLoop = false;
                    gameControl.loopChoiceCounter = 0;
                    gameControl.loopChoiceJumpmark = 0;

                    loadSceneManager.FadeToLevel(3);
                    return;
                }
            }
        }


        private void UnlockChapter()
        {
            audioManager.PlayButtonSFX(clickSFX);
#if UNITY_ANDROID || UNITY_IOS
            gameControl.currency -= 10;
            currencyManager.ShowCurrency();
#endif
            gameControl.unLockedSavept.Add(chapterKeyName);

            gameControl.currentBookmark = bookmark;

            gameControl.isChoiceLoop = false;
            gameControl.loopChoiceCounter = 0;
            gameControl.loopChoiceJumpmark = 0;

            loadSceneManager.FadeToLevel(3);
        }

        bool CheckForMatch()
        {
            if (gameControl.isTester)
                return true;
            
            foreach (string chapter in previousChapter)
            {
                if (gameControl.stageClear.Contains(chapter))
                {
                    return true; // Match found
                }
            }

            return false; // No match found
        }

        public void OnConfirmPayPanelClose()
        {
            payButton.onClick.RemoveListener(UnlockChapter);
        }
    }
}