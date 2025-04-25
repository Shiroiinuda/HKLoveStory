using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;

    [SerializeField]
    public PlayerData data;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void Save(PlayerData newData)
    {
        data = newData;
        FBPP.SetString("chengWingData", JsonUtility.ToJson(data));
        FBPP.Save();
    }
    public void Load()
    {
        //Debug.Log(FBPP.GetString("chengWingData"));
        data = JsonUtility.FromJson<PlayerData>(FBPP.GetString("chengWingData"));
    }


    public void Initialization()
    {
        data.currentBookmark = 0;
        data.stageClear.Clear();
        data.items.Clear();
        data.itemPath.Clear();
        data.items.Add("JadeRing");
        data.itemPath.Add("AddItemInventoryItems");
        data.collections.Clear();
        data.collectionPath.Clear();
        data.isChoiceLoop = false;
        data.loopChoiceCounter = 0;
        data.replayCount = 0;
        data.prevousGameName = "";
        data.bgm = "";
        data.mainMenuBG = "Normal";
        #if UNITY_STANDALONE_WIN
        data.steamLanguage = Steamworks.SteamApps.GetCurrentGameLanguage();
        #endif
        data.toldFan = 0;
        for (int i = 0; i < 6; i++)
            data.SaveBookMarks.Add(new SaveBookMark()
            {
                hasSave = false
            });
        data.unlockTrueEndDrug = 0;
        data.unlockTrueEndLoveLetter = 0;
        data.ansHome = 0;
        data.ansSchool = 0;
        data.ansTheatre = 0;
        data.ansBreakUp = 0;
        data.unlockTrueEndStory = 0;
        data.unlockPic = 0;
        data.PressedResetButton = 0;

        data.CG.Clear();

        data.currency = 20;

        data.shopRecord.Clear();
        data.unLockedSavept.Clear();
        data.saveptDatas.Clear();

        data.cardList.Clear();

        data.refundList.Clear();
        data.isTester = false;
        data.boughtUnlimitedPackage = false;
        data.canRestore = true;

        data.achievementList.Clear();

        data.todayAdsCount = 0;
        
        data.yesterdayDate = DateTime.Now.Day.ToString();

        data.storedBookmark.Clear();
    }
}

[Serializable]
public class PlayerData
{
    public int currentBookmark;
    public List<string> stageClear;
    public List<string> items;
    public List<string> itemPath;
    public List<string> collections;
    public List<string> collectionPath;
    public List<SaveBookMark> SaveBookMarks;
    public bool isChoiceLoop;
    public int loopChoiceCounter;
    public int loopChoiceJumpmark;
    public int replayCount;
    public string prevousGameName;
    public string bgm;
    public string mainMenuBG;

    public int toldFan;

    public int unlockTrueEndDrug;
    public int unlockTrueEndLoveLetter;
    public int unlockTrueEndStory;

    public int ansHome;                     
    public int ansSchool;
    public int ansTheatre;
    public int ansBreakUp;
    public int unlockPic;
    public int PressedResetButton;

    public List<string> CG;
    
    public int currency;

    public List<string> shopRecord;

    public List<string> unLockedSavept;

    public List<string> saveptDatas;
    public List<string> cardList;
    public List<string> refundList;
    public bool isTester;
    public bool boughtUnlimitedPackage;
    public bool canRestore;
    public List<string> achievementList;
    public int todayAdsCount;
    public string yesterdayDate;
    public List<int> storedBookmark;
    public string steamLanguage;
    public string userLanguage;

}

[Serializable]
public class SaveBookMark
{

    public bool hasSave = false;
    public int bookMark;
    public string chapter;
    public string backGroundPath;
    public string date;
    public string audio;
    public int dialogueArgue;
}