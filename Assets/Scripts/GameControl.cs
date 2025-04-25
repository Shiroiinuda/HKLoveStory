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
    

    public int currentBookmark
    {
        get
        {
            Debug.Log($"Get BookMark:{SaveData.Instance.data.currentBookmark}");
           return SaveData.Instance.data.currentBookmark;
        }
        set
        {
            Debug.Log($"Save BookMark:{value}");
            SaveData.Instance.data.currentBookmark = value;
            saveData.Save(SaveplayerData());
        }
    }

    public void CheatMode()
    {
        SaveData.Instance.data.isTester = !SaveData.Instance.data.isTester;
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
            Debug.Log(SaveData.Instance.data.saveBookMarks.Count);
            if (SaveData.Instance.data.saveBookMarks.Count != 6 || SaveData.Instance.data.saveBookMarks.Count !=0)
            {

                for (int i = 0; i < SaveData.Instance.data.saveBookMarks.Count; i++)
                {
                    if (SaveData.Instance.data.saveBookMarks[i] == null) continue;
                            bookMarks[i] = SaveData.Instance.data.saveBookMarks[i];
                }
            }
            else
            {
                bookMarks = SaveData.Instance.data.saveBookMarks;
            }

            saveData.data.saveBookMarks = bookMarks;
            return bookMarks;
        }
        set => SaveData.Instance.data.saveBookMarks = value;
    }
    public List<string> stageClear
    {
        get => SaveData.Instance.data.stageClear;
        set => SaveData.Instance.data.stageClear = value;
    }

    public List<string> items
    {
        get => SaveData.Instance.data.items;
        set => SaveData.Instance.data.items = value;
    }

    public List<string> itemPath
    {
        get => SaveData.Instance.data.itemPath;
        set => SaveData.Instance.data.itemPath = value;
    }

    public List<string> collections
    {
        get => SaveData.Instance.data.collections;
        set => SaveData.Instance.data.collections = value;
    }

    public List<string> collectionPath
    {
        get => SaveData.Instance.data.collectionPath;
        set => SaveData.Instance.data.collectionPath = value;
    }

    public bool isChoiceLoop
    {
        get => SaveData.Instance.data.isChoiceLoop;
        set => SaveData.Instance.data.isChoiceLoop = value;
    }

    public int loopChoiceCounter
    {
        get => SaveData.Instance.data.loopChoiceCounter;
        set => SaveData.Instance.data.loopChoiceCounter = value;
    }

    public int loopChoiceJumpmark
    {
        get => SaveData.Instance.data.loopChoiceJumpmark;
        set => SaveData.Instance.data.loopChoiceJumpmark = value;
    }

    public int replayCount
    {
        get => SaveData.Instance.data.replayCount;
        set => SaveData.Instance.data.replayCount = value;
    }

    public string prevousGameName
    {
        get => SaveData.Instance.data.prevousGameName;
        set => SaveData.Instance.data.prevousGameName = value;
    }

    public string bgm
    {
        get => SaveData.Instance.data.bgm;
        set => SaveData.Instance.data.bgm = value;
    }

    public string mainMenuBG
    {
        get => SaveData.Instance.data.mainMenuBG;
        set => SaveData.Instance.data.mainMenuBG = value;
    }

    [Space(5)] [Header("Player Data that won't be reset")]
    public string language;

    public List<string> CG
    {
        get => SaveData.Instance.data.CG;
        set => SaveData.Instance.data.CG = value;
    }

    public int currency
    {
        get => SaveData.Instance.data.currency;
        set
        {
            SaveData.Instance.data.currency = value;
            saveData.Save(SaveplayerData());
        }
    }

    public List<string> shopRecord;

    public List<string> unLockedSavept
    {
        get => SaveData.Instance.data.unLockedSavept;
        set => SaveData.Instance.data.unLockedSavept = value;
    }

    //Shop
    public int removedAds;
    public int unlimitedHP;

    [Space(5)] [Header("Script")] public static GameControl instance;
    public SaveData saveData;

    public string SteamLanguage
    {
        get => SaveData.Instance.data.steamLanguage;
        set => SaveData.Instance.data.steamLanguage = value;
    }

    public string UserLanguage
    {
        get => SaveData.Instance.data.userLanguage;
        set => SaveData.Instance.data.userLanguage = value;
    }

    //DrawGame
    public List<string> cardList
    {
        get => SaveData.Instance.data.cardList;
        set => SaveData.Instance.data.cardList = value;
    }

    //Refund
    public List<string> refundList
    {
        get => SaveData.Instance.data.refundList;
        set => SaveData.Instance.data.refundList = value;
    }

    //Tester
    public bool isTester
    {
        get => SaveData.Instance.data.isTester;
        set => SaveData.Instance.data.isTester = value;
    }

    //Shop
    public bool boughtUnlimitedPackage
    {
        get => SaveData.Instance.data.boughtUnlimitedPackage;
        set => SaveData.Instance.data.boughtUnlimitedPackage = value;
    }

    public bool canRestore
    {
        get => SaveData.Instance.data.canRestore;
        set => SaveData.Instance.data.canRestore = value;
    }

    //Achievement
    public List<string> achievementList
    {
        get => SaveData.Instance.data.achievementList;
        set => SaveData.Instance.data.achievementList = value;
    }

    public int todayAdsCount
    {
        get => SaveData.Instance.data.todayAdsCount;
        set => SaveData.Instance.data.todayAdsCount = value;
    }

    private int yesterdayDate
    {
        get
        {
            if (string.IsNullOrEmpty(SaveData.Instance.data.yesterdayDate))
            {
                SaveData.Instance.data.yesterdayDate = DateTime.Now.Day.ToString();
            }

            int a = DateTime.Now.Day;
            
            bool isint = int.TryParse(SaveData.Instance.data.yesterdayDate, out a);

            if (isint)
                return a;
            else
                return DateTime.Now.Day;
        }
        set => SaveData.Instance.data.yesterdayDate = value.ToString();
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
        get => SaveData.Instance.data.storedBookmark;
        set => SaveData.Instance.data.storedBookmark = value;
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
        SaveData.Instance.data = saveData.data;
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
        if (string.IsNullOrEmpty(SaveData.Instance.data.yesterdayDate)) return;
        
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
            SaveData.Instance.data.storedBookmark = storedBookmark;
        return SaveData.Instance.data;
    }
    
}