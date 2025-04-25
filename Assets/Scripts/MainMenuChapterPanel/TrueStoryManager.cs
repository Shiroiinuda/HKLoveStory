using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Chapters
{
    public class TrueStoryManager : ChapterMono
    {
        public string chapterID;

        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioClip clickSFX;

        [SerializeField] private LoadSceneManager loadSceneManager;
        [SerializeField] private GameControl gameControl;

        public int bookmark;

        [SerializeField] private GameObject lockImg;

        private bool canClick;

        public GameObject trueEndHintPanel;

        protected override void CheckStageClear()
        {
            /*bool isClear = gameControl.unlockTrueEndDrug == 1 && gameControl.unlockTrueEndLoveLetter == 1;
            chapterUI.enabled = isClear;
            chapterNameUI.enabled = isClear;
            lockImg.SetActive(!isClear);
            canClick = isClear;
            gameObject.GetComponent<Button>().interactable = isClear;
            trueEndHintPanel.SetActive(!isClear);*/
        }

        public void OnChapterClicked()
        {
            unlockEffect();
        }

        void unlockEffect()
        {
            gameControl.replayCount = 0;
            audioManager.PlayButtonSFX(clickSFX);
            gameControl.currentBookmark = bookmark;
            loadSceneManager.FadeToLevel(3);
            if (!gameControl.unLockedSavept.Contains(chapterKeyName))
                gameControl.unLockedSavept.Add(chapterKeyName);
        }
    }
}
