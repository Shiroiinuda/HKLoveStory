using System;
using System.Collections;
using System.IO;
using EasierLocalization;
using MyBox;
using SoundControl;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveLoadData : MonoBehaviour
{
    public SaveLoadManager saveLoadManager;
    public int saveNum;
    public GameObject haveSave;
    public bool hasSave;
    public TextMeshProUGUI chapter;
    public TextMeshProUGUI chapterName;
    public TextMeshProUGUI date;
    public Image backGround;
    private SaveBookMark saveData;
    private const string PPrefArgue = "DialogueArgueScore";

    private void OnEnable()
    {
        DisplaySave(saveLoadManager.saveBookMarks[saveNum], saveNum);
    }

    public void DisplaySave(SaveBookMark data,int Num)
    {
//        Debug.Log($"{Num}, Data:{data.audio}");
        saveData = data;
        Sprite sprite = null;
        saveNum = Num;
        hasSave = data.hasSave;
        haveSave.SetActive(hasSave);
        if (hasSave)
        {
            backGround.color = Color.white;
            chapter.text = Localization.GetString($"CR_Chap/{data.chapter}");
            chapterName.text = Localization.GetString($"CR_ChapName/{data.chapter}");
            date.text = data.date;
        }
        else
        {
            chapter.text = "Null";
            chapterName.text = "Null";
            date.text = "Null";
        }

        if(!string.IsNullOrEmpty(data.backGroundPath))
            sprite = LoadSpriteFromPath($"{Application.persistentDataPath}/Screenshots/SaveScreenshot{saveNum}.png");
        if (sprite is null) return;
            backGround.sprite = sprite;
    }
[ButtonMethod]
    public void CreateSave()
    {
        if (!saveLoadManager.saveBookMarks[saveNum].hasSave)
        {
            StartSave();
            return;
        }
        saveLoadManager.saveLoadAction.RemoveAllListeners();
        saveLoadManager.confirmPanel.SetActive(true);
        saveLoadManager.saveLoadAction.AddListener(StartSave);
        saveLoadManager.saveLoadAction.AddListener(()=>saveLoadManager.confirmPanel.SetActive(false));
    }

    public void StartSave()
    {
        StartCoroutine(ICreateSave());
    }
    IEnumerator ICreateSave()
    {
       // Debug.Log(DialogueManager.Instance.currentDialogue.bookMark);

     //   Debug.Log(DialogueManager.Instance.currentDialogue.bookMark);
            SaveBookMark temp = new SaveBookMark()
            {
                hasSave = true,
                chapter = DialogueManager.Instance.currentDialogue.saveChapter,
                bookMark = DialogueManager.Instance.currentDialogue.bookMark,
                date = DateTime.Now.ToString(),
                backGroundPath = $"{Application.persistentDataPath}/Screenshots/SaveScreenshot{saveNum}.png",
                audio = GameControl.instance.bgm,
                dialogueArgue = PlayerPrefs.GetInt(PPrefArgue)
            };
            Debug.Log($"{temp.bookMark}");
            saveLoadManager.saveBookMarks[saveNum] = temp;
            
            CaptureDisplay.Instance.CaptureScreens(saveNum);
            yield return new WaitForSeconds(1f);
            backGround.sprite =
                LoadSpriteFromPath($"{Application.persistentDataPath}/Screenshots/SaveScreenshot{saveNum}.png");
            DisplaySave(temp,saveNum);
            SoundManager.PlaySfx("Sound2");
           yield break; 
           
        
        
    }
    [ButtonMethod]
    public void Load()
    {
        Debug.Log("?");
        if (!hasSave) return;
        saveLoadManager.confirmPanel.SetActive(true);
        saveLoadManager.saveLoadAction.AddListener(LoadFunction);
            
    }

    public void LoadFunction()
    {
        Debug.Log("???");
        SoundManager.PlaySfx("smashselect");
        GameControl.instance.currentBookmark = saveLoadManager.saveBookMarks[saveNum].bookMark;
        Debug.Log($"{saveNum} Num : {saveData.audio}");
        GameControl.instance.bgm = saveLoadManager.saveBookMarks[saveNum].audio;
        SaveData.instance.data.bgm = saveLoadManager.saveBookMarks[saveNum].audio;
        Debug.Log($"Set Argue: {saveLoadManager.saveBookMarks[saveNum].dialogueArgue}");
        PlayerPrefs.SetInt(PPrefArgue,saveLoadManager.saveBookMarks[saveNum].dialogueArgue);
        SoundManager.PlaySfx("Sound3");
        LoadSceneManager.Instance.FadeToLevel(3);
    }
    public Sprite LoadSpriteFromPath(string filePath)
    {
        // Check if the file exists at the specified path
        if (File.Exists(filePath))
        {
            // Load the image data into a byte array
            byte[] imageData = File.ReadAllBytes(filePath);

            // Create a Texture2D from the image data
            Texture2D texture = new Texture2D(2, 2); // Temporary size, it will be overwritten by LoadImage
            texture.LoadImage(imageData);

            // Create a Sprite from the Texture2D
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            // Return the created sprite
            return newSprite;
        }
        else
        {
            Debug.LogError("File not found at path: " + filePath);
            return null; // Return null if the file doesn't exist
        }
    }
}
