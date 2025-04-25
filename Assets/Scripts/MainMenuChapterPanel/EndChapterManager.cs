using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Chapters
{
    public class EndChapterManager : ChapterMono
    {
        public string chapterID;

        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioClip lockSFX;
        [SerializeField] private AudioClip clickSFX;

        [SerializeField] private GameControl gameControl;
        [SerializeField] private LoadSceneManager loadSceneManager;

        public int goToBookmark;
        [SerializeField] private GameObject lockImg;
        private bool canClick;


        protected override void CheckStageClear()
        {
                canClick = gameControl.isTester;
                bool hasMatch = CheckForMatch();
                chapterUI.enabled = hasMatch;
                chapterNameUI.enabled = hasMatch;
                lockImg.SetActive(!hasMatch);
                canClick = hasMatch;
                this.gameObject.GetComponent<Button>().interactable = hasMatch;
        }
        bool CheckForMatch()=> gameControl.stageClear.Contains(chapterKeyName);
        
        public void OnChapterClicked()
        {
            if (canClick)
            {
                audioManager.PlayButtonSFX(clickSFX);
                gameControl.currentBookmark = goToBookmark;
                loadSceneManager.FadeToLevel(3);
            }
            else
                audioManager.PlayButtonSFX(lockSFX);
        }
    }
}
