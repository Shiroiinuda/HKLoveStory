using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using I2.Loc;
using UnityEngine;
// using UnityEngine.Localization.Components;
using TMPro;

namespace Chapters
{
    public class ChapterMono : MonoBehaviour
    {
        public string chapterKeyName;

        [Space(10)] [Header("Localization")] [Space(10)] [Header("Chapter")] [SerializeField]
        protected Localize chapterLoc;

        [SerializeField] private Localize chapterNameLoc;
        [SerializeField] protected TextMeshProUGUI chapterUI;
        [SerializeField] protected TextMeshProUGUI chapterNameUI;

        protected virtual void Start()
        {
            CheckStageClear();
            chapterUI.text = Localization.GetString($"CR_Chap/{chapterKeyName}");
            chapterNameUI.text = Localization.GetString($"CR_ChapName/{chapterKeyName}");
            chapterLoc.SetTerm($"CR_Chap/{chapterKeyName}");
            chapterNameLoc.SetTerm($"CR_ChapName/{chapterKeyName}");

        }
        protected void OnEnable()
        {
            CheckStageClear();
            chapterUI.text = Localization.GetString($"CR_Chap/{chapterKeyName}");
            chapterNameUI.text = Localization.GetString($"CR_ChapName/{chapterKeyName}");
            chapterLoc.SetTerm($"CR_Chap/{chapterKeyName}");
            chapterNameLoc.SetTerm($"CR_ChapName/{chapterKeyName}");
        }
        protected virtual void CheckStageClear()
        {

        }
    }
}
