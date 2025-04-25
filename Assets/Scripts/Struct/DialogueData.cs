
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DialogueDatas
{
    
}

public class DialogueData : MonoBehaviour
{
    public List<Dialogue> dialogues;
}

[System.Serializable]
public struct ChoicesData
{
    public string key;
    public string isLoop;
    public string keyPath;
}

[System.Serializable]
public struct BackgroundData
{
    public string file;
    public string image;
}

[System.Serializable]
public struct CharacterData
{
    public string body;
    public string head;
    public string expression;
}

[System.Serializable]
public struct ItemModify
{
    public string addItem;
    public string delItemPath;
    public string delItem;
}

[System.Serializable]
public struct AudioData
{
    public string bgm;
    public string voiceName;
    public string voiceFile;
    public string sfxFile;
}

[System.Serializable]
public struct CgData
{
    public string file;
    public string cG;
}

[System.Serializable]
public struct Prop
{
    public string name;
    public string anim;
}

[System.Serializable]
public struct CallFunc
{
    public string func1;
    public string func2;
}

[System.Serializable]
public struct Road
{
    public string road1;
    public string road2;
    public string road3;
}
[System.Serializable]
public struct Dialogue
{
    public int bookMark;
    public string saveChapter;
    public int nextLv;
    public int chapterState;
    public int jumpMark;
    public int speakerID;
    public string speakerName;
    public string speakerPosition;
    public ChoicesData choicesData;
    public string dialogueMode;
    // public string dialogueSentence;
    public BackgroundData backgroundData;
    public bool fade;
    public CharacterData character;
    public ItemModify itemModify;
    public AudioData audioData;
    public bool isVibrate;
    public CgData cgData;
    public bool dialogueBoxOnOff;
    public string animation;
    public string imgEffect;
    public Prop prop;
    public string mode;
    public string investivationLabel;
    public CallFunc callFuncs;
    public Road roads;
    public string mainMenu;

    public Dialogue(string filePath)
    {
        
        string[] columns = filePath.Split(',');
        Debug.Log($"<color=#ffee00>Loading {columns[0]}: </color> <color=#00FFFF>{columns.Length}</color> columns loaded");
        Debug.Log(RemoveOuterQuotes(columns[35]));
        
        bookMark = int.Parse(columns[0]);
        saveChapter = columns[1];
        nextLv = ParseInt(columns[2], -1);
        chapterState = ParseInt(columns[3], 0);
        jumpMark = ParseInt(columns[4], -1);
        speakerID = ParseInt(columns[5], 0);
        speakerName = columns[6];
        speakerPosition = columns[7];
        choicesData = new ChoicesData()
        {
            isLoop = columns[8],
            keyPath = columns[9],
            key = columns[10]
        };
        dialogueMode = columns[11];
// dialogueSentence is removed
        backgroundData = new BackgroundData()
        {
            file = columns[12],
            image = columns[13],
        };
        fade = ParseBool(columns[14]);
        character = new CharacterData()
        {
            body = columns[15],
            head = columns[16],
            expression = columns[17]
        };
        itemModify = new ItemModify()
        {
            addItem = columns[18],
            delItemPath = columns[19],
            delItem = columns[20]
        };
        audioData = new AudioData()
        {
            bgm = columns[21],
            voiceFile = columns[22],
            voiceName = columns[23],
            sfxFile = columns[24]
        };
        isVibrate = ParseBool(columns[25]);

        cgData = new CgData()
        {
            file = columns[26],
            cG = columns[27],
        };
        dialogueBoxOnOff = ParseBool(columns[28]);
        animation = columns[29];
        imgEffect = columns[30];
        Debug.Log($"{dialogueBoxOnOff}, {columns[0]}");
        prop = new Prop()
        {
            name = columns[31],
            anim = columns[32]
        };
        mode = columns[33];
        investivationLabel = columns[34];
        callFuncs = new CallFunc()
        {
            func1 = RemoveOuterQuotes(columns[35]),
            func2 = RemoveOuterQuotes(columns[36]),
        };
        roads = new Road()
        {
            road1 = columns[37],
            road2 = columns[38],
            road3 = columns[39]
        };
        mainMenu = columns[40];
        return;

        int ParseInt(string numberString, int defaultNum)
        {
            return string.IsNullOrEmpty(numberString) ? defaultNum : int.Parse(numberString);
        }
        bool ParseBool(string numberString)
        {
            
            bool parseSuccess = int.TryParse(numberString, out int result);
            if (!parseSuccess)
            {
                return false;
            }

            return result != 0;
        }
    }
    public static string RemoveOuterQuotes(string input)
    {
        if (string.IsNullOrEmpty(input)) return null;
        if (input.Length > 1 && input[0] == '"' && input[input.Length - 1] == '"')
            return input.Substring(1, input.Length - 2);
        return input;
    }

}