using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CSVReader : MonoBehaviour
{
    public GameControl gameControl;

    public TextAsset csvData_TW;

//     public List<Dialogue> Parse()
//     {
//         List<Dialogue> dialogues = new List<Dialogue>();
//
//         string[] csvLines;
//
//         csvLines = csvData_TW.text.Split('\n');
//
//         bool firstRow = true;
//         foreach (string line in csvLines)
//         {
//             if (firstRow)
//             {
//                 firstRow = false;
//                 continue;
//             }
//
//             string[] fields = line.Split(';');
//             if (fields.Length >= 44)
//             {
//                 string chapter = fields[0];
//                 string svaeChapter = fields[1];
//                 string chapterName = fields[2];
//                 int nextlv = int.Parse(fields[3]);
//                 int chapterState = int.Parse(fields[4]);
//                 int bookMark = int.Parse(fields[5]);
//                 int jumpMark = int.Parse(fields[6]);
//                 int speakerID = int.Parse(fields[7]);
//                 string isChoiceLoop = fields[8];
//                 string choiceKeyaPath = fields[9];
//                 string choiceKeyNeed = fields[10];
//                 string speakerName = fields[11];
//                 string dialogueMode = fields[12];
//                 string SpeakerPosition = fields[13];
//                 string bgFile = fields[14];
//                 string bgImg = fields[15];
//                 int fade = int.Parse(fields[16]);
//                 string bodyImg = fields[17];
//                 string headImg = fields[18];
//                 string expressionImg = fields[19];
//                 string dialogue = fields[20];
//                 string addItem = fields[21];
//                 string delItemPath = fields[22];
//                 string delItem = fields[23];
//                 string bgmFile = fields[24];
//
//                 string voiceFile = fields[25];
//                 string voiceName = fields[26];
//                 string sfxFile = fields[27];
//                 int vibrate = int.Parse(fields[28]);
//                 string cgFile = fields[29];
//                 string cgImg = fields[30];
//                 string dialogueBoxOnOff = fields[31];
//                 string animationEffect = fields[32];
//                 string ImgEffect = fields[33];
//                 string propName = fields[34];
//                 string propAnim = fields[35];
//                 string mode = fields[36];
//                 string investigationLable = fields[37];
//
//                 string callFunction = fields[38];
//                 string callFunction2 = fields[39];
//                 string road1 = fields[40];
//                 string road2 = fields[41];
//                 string road3 = fields[42];
//                 string mainMenuBG = fields[43];
//
//                 dialogues.Add(new Dialogue(chapter, svaeChapter, chapterName, nextlv, chapterState, bookMark, jumpMark, speakerID, isChoiceLoop, choiceKeyaPath, choiceKeyNeed, speakerName, dialogueMode, SpeakerPosition, bgFile, bgImg, fade, bodyImg, headImg, expressionImg, dialogue, addItem, delItemPath, delItem, bgmFile, voiceFile, voiceName, sfxFile, vibrate, cgFile ,cgImg, dialogueBoxOnOff, animationEffect, ImgEffect, propName, propAnim, mode, investigationLable, callFunction, callFunction2, road1, road2, road3, mainMenuBG));
//             }
//         }
//
//         return dialogues;
//     }
//
// }
// public struct Dialogue
// {
//     public string chapter;
//     public string saveChapter;
//     public string chapterName;
//     public int nextlv;
//     public int chapterState;
//     public int bookMark;
//     public int jumpMark;
//     public int speakerID;
//     public string choiceMode;
//
//     public string choiceKeyPath;
//     public string choiceKeyNeed;
//
//     public string speakerName;
//     public string dialogueMode;
//     public string SpeakerPosition;
//
//     public string bgFile;
//     public string bgImg;
//     public int fade;
//     public string bodyImg;
//     public string headImg;
//     public string expressionImg;
//     public string dialogue;
//     public string addItem;
//     public string delItemPath;
//     public string delItem;
//     public string bgmFile;
//
//     public string voiceFile;
//     public string voiceName;
//     public string sfxFile;
//     public int vibrate;
//
//     public string cgFile;
//     public string cgImg;
//     public string dialogueBoxOnOff;
//     public string animationEffect;
//     public string imgEffect;
//     public string propName;
//     public string propAnim;
//     public string mode;
//     public string investigationLable;
//
//     public string callFunction;
//     public string callFunction2;
//     public string road1;
//     public string road2;
//     public string road3;
//     public string mainMenuBG;
//
//
//
//
//     public Dialogue(string _chapter, string _svaeChapter, string _chapterName, int _nextlv, int _chapterState, int _bookMark, int _jumpMark, int _speakerID, string _ChoiceMode, string _choiceKeyaPath, string _choiceKeyNeed, string _speakerName, string _dialogueMode, string _SpeakerPosition, string _bgFile, string _bgImg, int _fade, string _bodyImg, string _headImg, string _expressionImg, string _dialogue, string _addItem, string _delItemPath, string _delItem, string _bgmFile, string _voiceFile, string _voiceName, string _sfxFile, int _vibrate, string _cgFile, string _cgImg, string _dialogueBoxOnOff, string _animationEffect, string _imgEffect, string _propName, string _propAnim, string _mode, string _investigationLabel, string _callFunction, string _callFunction2, string _road1, string _road2, string _road3, string _mainMenuBG)
//     {
//         chapter = _chapter;
//         saveChapter = _svaeChapter;
//         chapterName = _chapterName;
//         nextlv = _nextlv;
//         chapterState = _chapterState;
//         bookMark = _bookMark;
//         jumpMark = _jumpMark;
//         speakerID = _speakerID;
//         choiceMode = _ChoiceMode;
//
//         choiceKeyPath = _choiceKeyaPath;
//         choiceKeyNeed = _choiceKeyNeed;
//
//         speakerName = _speakerName;
//         dialogueMode = _dialogueMode;
//         SpeakerPosition = _SpeakerPosition;
//
//         bgFile = _bgFile;
//         bgImg = _bgImg;
//         fade = _fade;
//         bodyImg = _bodyImg;
//         headImg = _headImg;
//         expressionImg = _expressionImg;
//         dialogue = _dialogue;
//         addItem = _addItem;
//         delItemPath = _delItemPath;
//         delItem = _delItem;
//         bgmFile = _bgmFile;
//
//         voiceFile = _voiceFile;
//         voiceName = _voiceName;
//         sfxFile = _sfxFile;
//         vibrate = _vibrate;
//
//         cgFile = _cgFile;
//         cgImg = _cgImg;
//         dialogueBoxOnOff = _dialogueBoxOnOff;
//         animationEffect = _animationEffect;
//         imgEffect = _imgEffect;
//         propName = _propName;
//         propAnim = _propAnim;
//         mode = _mode;
//         investigationLable = _investigationLabel;
//
//         callFunction = _callFunction;
//         callFunction2 = _callFunction2;
//         road1 = _road1;
//         road2 = _road2;
//         road3 = _road3;
//         mainMenuBG = _mainMenuBG;
//     }
}
