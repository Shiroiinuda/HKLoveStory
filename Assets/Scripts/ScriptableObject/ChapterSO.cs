using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterDataSO", menuName = "ScriptableObjects/Chapter", order = 1)]
public class ChapterSO : ScriptableObject
{
    public string chapterName;
    public string chapterBookMark;
    public string previousChapter1;
}
