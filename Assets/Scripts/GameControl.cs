using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using EasierLocalization;
using MyBox;
using SoundControl;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    private bool _isInvestigation;

    public bool isInvestigation
    {
        get => _isInvestigation;
        set
        {
            Debug.Log($"On investigation: {value}");
            _isInvestigation = value;
        }
    }

    public AudioManager audioManager;
    public Animator getAchievementAnim;
    public TextMeshProUGUI achievementType;
    public TextMeshProUGUI achievementName;

    [ButtonMethod]
    private void SetLanguageEN()
    {
        Localization.SetLanguage("English");
    }

    [ButtonMethod]
    private void SetLanguageTW()
    {
        Localization.SetLanguage("Chinese (Traditional)");
    }

    [ButtonMethod]
    private void SetLanguageCN()
    {
        Localization.SetLanguage("Chinese (Simplified)");
    }

    public bool reopen;

    [Header("Player Data")] public PlayerData playerSaveData;

    public int currentBookmark
    {
        get
        {
            Debug.Log($"Get BookMark:{playerSaveData.currentBookmark}");
           return playerSaveData.currentBookmark;
        }
        set
        {
            Debug.Log($"Save BookMark:{value}");
            playerSaveData.currentBookmark = value;
            saveData.Save(SaveplayerData());
        }
    }

    public void CheatMode()
    {
        playerSaveData.isTester = !playerSaveData.isTester;
    }
    public List<SaveBookMark> SaveBookMarks
    {
        get
        {
            List<SaveBookMark> bookMarks = new List<SaveBookMark>(6);
            for (int i = 0; i < 6; i++)
            {
                bookMarks.Add(new SaveBookMark()
                {
hasSave = false
                });
            }
            Debug.Log(playerSaveData.SaveBookMarks.Count);
            if (playerSaveData.SaveBookMarks.Count != 6 || playerSaveData.SaveBookMarks.Count !=0)
            {

                for (int i = 0; i < playerSaveData.SaveBookMarks.Count; i++)
                {
                    if (playerSaveData.SaveBookMarks[i] == null) continue;
                            bookMarks[i] = playerSaveData.SaveBookMarks[i];
                }
            }
            else
            {
                bookMarks = playerSaveData.SaveBookMarks;
            }

            saveData.data.SaveBookMarks = bookMarks;
            return bookMarks;
        }
        set => playerSaveData.SaveBookMarks = value;
    }
    public List<string> stageClear
    {
        get => playerSaveData.stageClear;
        set => playerSaveData.stageClear = value;
    }

    public List<string> items
    {
        get => playerSaveData.items;
        set => playerSaveData.items = value;
    }

    public List<string> itemPath
    {
        get => playerSaveData.itemPath;
        set => playerSaveData.itemPath = value;
    }

    public List<string> collections
    {
        get => playerSaveData.collections;
        set => playerSaveData.collections = value;
    }

    public List<string> collectionPath
    {
        get => playerSaveData.collectionPath;
        set => playerSaveData.collectionPath = value;
    }

    public bool isChoiceLoop
    {
        get => playerSaveData.isChoiceLoop;
        set => playerSaveData.isChoiceLoop = value;
    }

    public int loopChoiceCounter
    {
        get => playerSaveData.loopChoiceCounter;
        set => playerSaveData.loopChoiceCounter = value;
    }

    public int loopChoiceJumpmark
    {
        get => playerSaveData.loopChoiceJumpmark;
        set => playerSaveData.loopChoiceJumpmark = value;
    }

    public int replayCount
    {
        get => playerSaveData.replayCount;
        set => playerSaveData.replayCount = value;
    }

    public string prevousGameName
    {
        get => playerSaveData.prevousGameName;
        set => playerSaveData.prevousGameName = value;
    }

    public string bgm
    {
        get => playerSaveData.bgm;
        set => playerSaveData.bgm = value;
    }

    public string mainMenuBG
    {
        get => playerSaveData.mainMenuBG;
        set => playerSaveData.mainMenuBG = value;
    }

    public int toldFan
    {
        get => playerSaveData.toldFan;
        set => playerSaveData.toldFan = value;
    }

    public int unlockTrueEndDrug
    {
        get => playerSaveData.unlockTrueEndDrug;
        set => playerSaveData.unlockTrueEndDrug = value;
    }

    public int unlockTrueEndLoveLetter
    {
        get => playerSaveData.unlockTrueEndLoveLetter;
        set => playerSaveData.unlockTrueEndLoveLetter = value;
    }

    public int ansHome
    {
        get => playerSaveData.ansHome;
        set => playerSaveData.ansHome = value;
    }

    public int ansSchool
    {
        get => playerSaveData.ansSchool;
        set => playerSaveData.ansSchool = value;
    }

    public int ansTheatre
    {
        get => playerSaveData.ansTheatre;
        set => playerSaveData.ansTheatre = value;
    }

    public int ansBreakUp
    {
        get => playerSaveData.ansBreakUp;
        set => playerSaveData.ansBreakUp = value;
    }

    public int unlockTrueEndStory
    {
        get => playerSaveData.unlockTrueEndStory;
        set => playerSaveData.unlockTrueEndStory = value;
    }

    public int unlockPic
    {
        get => playerSaveData.unlockPic;
        set => playerSaveData.unlockPic = value;
    }

    public int PressedResetButton
    {
        get => playerSaveData.PressedResetButton;
        set => playerSaveData.PressedResetButton = value;
    }

    [Space(5)] [Header("Player Data that won't be reset")]
    public string language;

    public List<string> CG
    {
        get => playerSaveData.CG;
        set => playerSaveData.CG = value;
    }

    public int currency
    {
        get => playerSaveData.currency;
        set
        {
            playerSaveData.currency = value;
            saveData.Save(SaveplayerData());
        }
    }

    public List<string> shopRecord;

    public List<string> unLockedSavept
    {
        get => playerSaveData.unLockedSavept;
        set => playerSaveData.unLockedSavept = value;
    }

    //Shop
    public int removedAds;
    public int unlimitedHP;

    [Space(5)] [Header("Script")] public static GameControl instance;
    public SaveData saveData;

    public string SteamLanguage
    {
        get => playerSaveData.steamLanguage;
        set => playerSaveData.steamLanguage = value;
    }

    public string UserLanguage
    {
        get => playerSaveData.userLanguage;
        set => playerSaveData.userLanguage = value;
    }

    //DrawGame
    public List<string> cardList
    {
        get => playerSaveData.cardList;
        set => playerSaveData.cardList = value;
    }

    //Refund
    public List<string> refundList
    {
        get => playerSaveData.refundList;
        set => playerSaveData.refundList = value;
    }

    //Tester
    public bool isTester
    {
        get => playerSaveData.isTester;
        set => playerSaveData.isTester = value;
    }

    //Shop
    public bool boughtUnlimitedPackage
    {
        get => playerSaveData.boughtUnlimitedPackage;
        set => playerSaveData.boughtUnlimitedPackage = value;
    }

    public bool canRestore
    {
        get => playerSaveData.canRestore;
        set => playerSaveData.canRestore = value;
    }

    //Achievement
    public List<string> achievementList
    {
        get => playerSaveData.achievementList;
        set => playerSaveData.achievementList = value;
    }

    public int todayAdsCount
    {
        get => playerSaveData.todayAdsCount;
        set => playerSaveData.todayAdsCount = value;
    }

    private int yesterdayDate
    {
        get
        {
            if (string.IsNullOrEmpty(playerSaveData.yesterdayDate))
            {
                playerSaveData.yesterdayDate = DateTime.Now.Day.ToString();
            }

            int a = DateTime.Now.Day;
            
            bool isint = int.TryParse(playerSaveData.yesterdayDate, out a);

            if (isint)
                return a;
            else
                return DateTime.Now.Day;
        }
        set => playerSaveData.yesterdayDate = value.ToString();
    }

    [Header("For Back Button")] public GameObject backButtonLastObj;
    public List<GameObject> pagesObj;
    public GameObject bufferPanel;
    private bool isPagesActive;
    public GameObject logOn;
    public GameObject logOff;
    public GameObject logOn_Game;
    public GameObject logOff_Game;

    public List<int> storedBookmark
    {
        get => playerSaveData.storedBookmark;
        set => playerSaveData.storedBookmark = value;
    }
    private const string BossGameFuPP = "BossFuAmount";

    public void GetFu()
    {
        var config = new FBPPConfig()
        {
            SaveFileName = "GameData.txt",
            AutoSaveData = true,
            ScrambleSaveData = true,
            EncryptionSecret = "HauntedHouse",
            SaveFilePath = Application.persistentDataPath
        };
        FBPP.Start(config);
        FBPP.SetInt(BossGameFuPP,FBPP.GetInt(BossGameFuPP)+10);
        if (FBPP.GetInt(BossGameFuPP) >= 50)
            OnUnlockAchievement("Amulet");
    }

    public bool isWatchingAds = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        var config = new FBPPConfig()
        {
            SaveFileName = "GameData.txt",
            AutoSaveData = true,
            ScrambleSaveData = true,
            EncryptionSecret = "HauntedHouse",
            SaveFilePath = Application.persistentDataPath
        };
        FBPP.Start(config);
//        Debug.Log(FBPP.HasKey("chengWingData"));
        if (!FBPP.HasKey(BossGameFuPP))
        {
            FBPP.SetInt(BossGameFuPP,0);
        }
        if (FBPP.HasKey("chengWingData"))
        {
            saveData.Load();
        }
        else
        {
            saveData.Initialization();
            saveData.Save(saveData.data);
        }

        reopen = false;
        playerSaveData = saveData.data;
    #if UNITY_ANDROID || UNITY_IOS
            int tempDateTime = yesterdayDate;
            if (int.TryParse(saveData.data.yesterdayDate, out tempDateTime))
            {
                Console.WriteLine("String as DateTime: " + tempDateTime);
            }
            else
            {
                Console.WriteLine("Invalid date string");
            }
    #endif
    #if (UNITY_STANDALONE_WIN ||UNITY_EDITOR)
            SteamLanguage = saveData.data.steamLanguage;
            UserLanguage = saveData.data.userLanguage;
            if (!SteamManager.Initialized) return;
    #if(UNITY_STANDALONE_WIN)
        if (String.IsNullOrEmpty(SteamLanguage) ||
           
            SteamLanguage != Steamworks.SteamApps.GetCurrentGameLanguage())
        {
            SteamLanguage = Steamworks.SteamApps.GetCurrentGameLanguage();
            Localization.SetLanguage(SteamLanguage);
        }
        else
        {
            Localization.SetLanguage(saveData.data.userLanguage);
        }
        #endif
#endif
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(playerSaveData.yesterdayDate)) return;
        
        int today = DateTime.Now.Day;
        int difference = today - yesterdayDate;
        if (difference != 0)
        {
            todayAdsCount = 0;
            yesterdayDate = DateTime.Now.Day;
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (bufferPanel != null)
                {
                    if (bufferPanel.activeSelf == false)
                    {
                        BackButtonAction();
                    }
                }
                else
                {
                    BackButtonAction();
                }
            }
        }
    }

    private void BackButtonAction()
    {
        isPagesActive = false;
        if (pagesObj != null && pagesObj.Count > 0)
        {
            foreach (GameObject page in pagesObj)
            {
                if (page.activeSelf)
                    isPagesActive = true;
            }

            if (isPagesActive == false)
            {
                SoundManager.PlaySfx("Menu Click");
                backButtonLastObj.SetActive(true);
            }
            else
            {
                SoundManager.PlaySfx("backbutton");
                for (int i = pagesObj.Count - 1; i >= 0; i--)
                {
                    if (pagesObj[i].activeSelf)
                    {
                        pagesObj[i].SetActive(false);

                        if (pagesObj[i].name == "LogPanel_Dialogue")
                        {
                            logOn?.SetActive(false);
                            logOff?.SetActive(true);
                        }

                        if (pagesObj[i].name == "LogPanel_Game")
                        {
                            logOn_Game.SetActive(!(logOn_Game is null));
                        }

                        break;
                    }
                }
            }
        }
    }

    public void ConfirmQuitClicked()
    {
        SoundManager.PlaySfx("smashselect");
        Application.Quit();
    }

    public void OnBackClicked(GameObject closeObj)
    {
        SoundManager.PlaySfx("backbutton");
        closeObj.SetActive(false);
    }

    public void OnUnlockAchievement(string aName)
    {
        aName = aName.Trim();
        
        #if(UNITY_STANDALONE_WIN)
                SteamScript.GiveAchievement(aName);
        #endif
#if !UNITY_EDITOR
        if (achievementList.Contains(aName)) return;
#endif
        achievementType.text = Localization.GetString($"Achievement/Type/{aName}");
        achievementName.text = Localization.GetString($"Achievement/Name/{aName}");
        getAchievementAnim.SetTrigger("ToLeft");
        achievementList.Add(aName);

    }

    private void OnDisable()
    {
        saveData.Save(SaveplayerData());
    }

    private void OnApplicationPause(bool pause)
    {
        saveData.Save(SaveplayerData());

        if (!pause && !isWatchingAds)
        {
            reopen = true;
            audioManager.ResetAudioAndPlay();
        }
    }

    private void OnApplicationQuit()
    {
        saveData.Save(SaveplayerData());
    }

    private PlayerData SaveplayerData()
    {
        saveData.data = playerSaveData;
        saveData.data.storedBookmark = storedBookmark;
        return saveData.data;
    }
    
}