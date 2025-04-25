using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUnlockedChapter : MonoBehaviour
{
    /// <summary>
    /// If Chapter is unlocked, set active this object
    /// </summary>
    
    [SerializeField] private GameControl gameControl;
    [SerializeField] private List<string> chapterNameList;
    public bool isSetActive;
    // Start is called before the first frame update
    private void OnEnable()
    {
        for (int i = 0; i < chapterNameList.Count; i++)
        {
            string chapterName = chapterNameList[i];
            if (gameControl.stageClear.Contains(chapterName))
            {
                this.gameObject.SetActive(isSetActive);
                return;
            }
            else
                this.gameObject.SetActive(!isSetActive);
        }
    }
}
