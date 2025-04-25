using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using JetBrains.Annotations;

namespace Chapters
{
    [Serializable]
    public class ChapterData
    {
        public string ChapterId { get; set; }
        public int GoToBookmark { get; set; }
        public string PreviousChapter1 { get; set; }
        public string PreviousChapter2 { get; set; }
    }
    public class ChapterCSVReader : MonoBehaviour
    {


        public ChapterDataList chapterDataList = new ChapterDataList();

        [Serializable]
        public class ChapterDataList
        {
            public ChapterData[] dataChapter;
        }

        [Header("Declare Chapter")] public List<ChapterManager> chapters;
        public List<EndChapterManager> endChapterManagers;
        public TrueStoryManager trueStoryChapterManagers;
        public TextAsset chapter;

        private void Awake()
        {
            LoadChapterCsv();

            foreach (ChapterManager chapter in chapters)
            {
                foreach (var t in chapterDataList.dataChapter)
                {
                    if (chapter.chapterID != t.ChapterId) continue;
//                    Debug.Log($"{chapter.chapterID}, {t.ChapterId}");
                    chapter.bookmark = t.GoToBookmark;
                    if (!String.IsNullOrEmpty(t.PreviousChapter1))
                        chapter.previousChapter.Add(t.PreviousChapter1);
                    if (!String.IsNullOrEmpty(t.PreviousChapter2))
                        chapter.previousChapter.Add(t.PreviousChapter2);
                    chapter.chapterKeyName = chapter.chapterID;
                    break;
                }
            }

            foreach (var endChapterManager in endChapterManagers)
            {
                foreach (var t in chapterDataList.dataChapter)
                {
                    if (endChapterManager.chapterID != t.ChapterId) continue;
                    endChapterManager.goToBookmark = t.GoToBookmark;
                    endChapterManager.chapterKeyName = endChapterManager.chapterID;
                    break;
                }
            }
            
            foreach (var t in chapterDataList.dataChapter)
            {
                if (trueStoryChapterManagers == null|| t.ChapterId == null) continue;
                if (trueStoryChapterManagers.chapterID != t.ChapterId) continue;
                trueStoryChapterManagers.bookmark = t.GoToBookmark;
                trueStoryChapterManagers.chapterKeyName = trueStoryChapterManagers.chapterID;
                break;
            }
        }

        private void ReadCsv(TextAsset csv, [NotNull] ref ChapterData[] dataList, Func<string[], ChapterData> loadData)
        {
            if (dataList == null) throw new ArgumentNullException(nameof(dataList));
            string[] lines = csv.text.Split('\n');

            dataList = new ChapterData[lines.Length];

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
  //              Debug.Log(lines);
                string[] columns = line.Split(',');
                for (int j = 0; j < columns.Length; j++)
                {
//                    Debug.Log(columns[j]);
                }
                dataList[i - 1] = loadData(columns);
            }
        }

        void LoadChapterCsv()
        {
            if (chapter == null) return;
            ReadCsv(chapter, ref chapterDataList.dataChapter, LoadStoryData);

            ChapterData LoadStoryData(string[] columns)
            {
                return new ChapterData
                {
                    ChapterId = columns[0],
                    GoToBookmark = int.Parse(columns[1]),
                    PreviousChapter1 = columns[2],
                    PreviousChapter2 = columns[3]
                };
            }
        }
    }
}