using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuBackGround : MonoBehaviour
    {
        public List<MainMenuBg> mainMenu;
        private SpriteRenderer bgSprite;
        private const string chapterPP = "CurrentChapter";
        private void Start()
        {
            bgSprite = GetComponent<SpriteRenderer>();
//            Debug.Log(GameControl.instance.playerSaveData.currentBookmark);
            var tempLoopInt = mainMenu.Count -1;
                if(FBPP.HasKey(chapterPP) && mainMenu.Any(item => item.chapter ==FBPP.GetString(chapterPP)))
                {
                    tempLoopInt = mainMenu.FindLastIndex(item => item.chapter == FBPP.GetString(chapterPP));
                }
                
//            Debug.Log(FBPP.GetString(chapterPP));
  //          Debug.Log(mainMenu[tempLoopInt].chapter);
    //        Debug.Log(tempLoopInt);
            for (int i = tempLoopInt; i >= 0; i--)
            {
                    if (GameControl.instance.playerSaveData.currentBookmark <= mainMenu[i].bookMark) continue;
                    /*if(!String.IsNullOrEmpty(mainMenu[i].chapter)) if (FBPP.GetString(chapterPP) != mainMenu[i].chapter) continue;*/
                bgSprite.sprite = mainMenu[i].backGround;
                if(mainMenu[i].active !=null)
                    mainMenu[i].active.SetActive(true);
                break;
            }
        }

        public void DelayBackGround(Sprite swapSprite)
        {
            StartCoroutine(ChangeBg(swapSprite));
        }

       private IEnumerator ChangeBg(Sprite swapSprite)
       {
           yield return new WaitForSeconds(1.5f);
           bgSprite.sprite = swapSprite;
       }
    }

    [System.Serializable]
    public struct MainMenuBg
    {
        public string chapter;
        public int bookMark;
        public Sprite backGround;
        public GameObject active;
    }
}
