using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HellWebTab : MonoBehaviour
{
    public Button leftBtn;
    public Button rightBtn;
    public List<HellWebDeadsFile> tabs;
    private int currentTabNum;
    public Image image;
    public Localize textLocalize;
    public Localize SecondLocalize;
    public Localize nameLocalize;
    public string page = "DeadFile";


    public void OnEnable()
    {
        ChangeTab(0);
        if(leftBtn !=null)        
        leftBtn.onClick.AddListener(() => ChangeTab(-1));
        if(rightBtn !=null)
        rightBtn.onClick.AddListener(() => ChangeTab(1));
    }

    private void OnDisable()
    {
        if(leftBtn !=null)
        leftBtn.onClick.RemoveAllListeners();
        if(rightBtn !=null)
        rightBtn.onClick.RemoveAllListeners();
    }

    private void ChangeTab(int value)
    {
        currentTabNum += value;
        if (currentTabNum < 0)
        {
            currentTabNum = tabs.Count;
        }

        currentTabNum %= tabs.Count;
        Debug.Log(currentTabNum);
        if(leftBtn != null)
            leftBtn.interactable = currentTabNum != 0;
        if (image != null)
        {
            image.sprite = tabs[currentTabNum].image;
        }
        
        textLocalize.SetTerm($"HellWeb/{page}/{tabs[currentTabNum].name}");
        nameLocalize?.SetTerm($"Speaker/{tabs[currentTabNum].name}");
        SecondLocalize?.SetTerm($"HellWeb/{page}/{tabs[currentTabNum].name}2");
        
    }
}