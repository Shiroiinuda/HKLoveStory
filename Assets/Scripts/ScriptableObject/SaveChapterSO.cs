using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SaveData(Chapter)", menuName = "ScriptableObjects/Save/Chapter", order = 1)]
public class SaveChapterSO : ScriptableObject
{
    public List<ChapterSave> chapters;
}
[System.Serializable]
public struct ChapterSave
{
    public ChapterSO chapter;
    public bool played;
    public npcFavorability favorability;
}
