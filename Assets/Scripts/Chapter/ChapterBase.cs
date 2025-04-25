using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter
{
    public class ChapterBase : MonoBehaviour
    {
        public int ChapterId { get; set; }
        public int GoToBookmark { get; set; }
        public string PreviousChapter1 { get; set; }
        public string PreviousChapter2 { get; set; }
    }
}

