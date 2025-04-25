using System.Collections;
using System.Collections.Generic;
using MyBox;
using SoundControl;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public List<GameObject> pages;
    public int pageNow =0;
    [Separator("UI")] public Button leftBtn;
    public Button rightBtn;
    protected virtual void OnEnable()
    {
        ChangePage(pageNow);
        leftBtn.onClick.RemoveAllListeners();
        leftBtn.onClick.AddListener(()=>ChangePage(--pageNow));
        rightBtn.onClick.RemoveAllListeners();
        rightBtn.onClick.AddListener(()=>ChangePage(++pageNow));
    }
    public void ChangePage(int pageNum)
    {
        leftBtn.interactable = !(pageNow <= 0);
        rightBtn.interactable = !(pageNow >= pages.Count - 1);
        
        for (int i = 0; i < pages.Count;i++)
        {
            var tmpint = i;
            
            pages[tmpint].SetActive(pageNum==tmpint);
            if (pageNum == tmpint)
            {
                Debug.Log($"{tmpint} : {pageNum}" );
            }
        }
        SoundManager.PlaySfx("nextbtn");
    }
}
