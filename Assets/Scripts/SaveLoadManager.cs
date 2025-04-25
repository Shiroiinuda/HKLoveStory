using System;
using System.Collections;
using System.Collections.Generic;
using SoundControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    private GameControl gameControl;
    public List<SaveBookMark> saveBookMarks;
    public List<SaveLoadData> saveLoadDatas;
    public UnityEvent saveLoadAction;
    public Button confirmBtn;
    public List<Button> returnBtns;
    public GameObject confirmPanel;

    
    public void OnEnable()
    {
        gameControl = GameControl.instance;
        saveBookMarks = gameControl.SaveBookMarks;
        for (int i = 0; i < saveBookMarks.Count; i++)
        {
            /*Debug.Log(saveBookMarks[i].audio);*/
            int temp = i; 
            Debug.Log(saveBookMarks[i]);
            saveLoadDatas[i].saveLoadManager = this;
            saveLoadDatas[i].DisplaySave(saveBookMarks[i],i);
            
        }
        confirmBtn.onClick.AddListener(saveLoadAction.Invoke);
        confirmBtn.onClick.AddListener(()=>saveLoadAction.RemoveAllListeners());
        
    }

    public void Start()
    {
        foreach (var btn in returnBtns)
        {
            btn.onClick.AddListener(()=>saveLoadAction.RemoveAllListeners());
            btn.onClick.AddListener(()=>confirmPanel.SetActive(false));
            btn.onClick.AddListener(()=>SoundManager.PlaySfx("smashselect"));
        }
    }

    public void PlayReturn()
    {
        SoundManager.PlaySfx("");
    }
    
}
