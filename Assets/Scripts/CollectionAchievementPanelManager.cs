using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionAchievementPanelManager : MonoBehaviour
{
    public List<GameObject> tabPanels;
    public List<Button> tabButtons;
    
    
    
    
    
    
    public GameControl gameControl;
    public AudioManager audioManager;

    public AudioClip clcikSfx;

    public GameObject collectionPanel;
    public GameObject achievementPanel;

    public List<GameObject> cgPages;
    public List<GameObject> achievementPages;
    public Button achievementBtn;
    public Button cgBtn;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        
    }

    public void Start()
    {
        for (var i = 0; i < tabButtons.Count; i++)
        {
            var iIndex = i;
            tabButtons[i].onClick.AddListener(()=> OnTabSelected(iIndex));
        }
        OnTabSelected(0);
    }
    private void OnTabSelected(int tabIndex)
    {
        Debug.Log(tabIndex);
        audioManager.PlayButtonSFX(clcikSfx);
        for (int i = 0; i < tabPanels.Count; i++)
        {
            if(i != tabIndex)
                tabButtons[i].animator.Play("Normal"); 
            tabPanels[i].SetActive(i == tabIndex);
        }
        tabButtons[tabIndex].animator.Play("Selected"); 
    }
    public void DisableOtherTabs()
    {
    }

    private void OpenCollectionPanel()
    {
        foreach(var p in cgPages)
        {
            if (p.name == "Page1Panel")
                p.SetActive(true);
            else
                p.SetActive(false);
        }

        collectionPanel.SetActive(true);
        achievementPanel.SetActive(false);

    }

    private void OpenAchievementPanel()
    {
        foreach (var p in achievementPages)
        {
            if (p.name == "Page1")
                p.SetActive(true);
            else
                p.SetActive(false);
        }

        collectionPanel.SetActive(false);
        achievementPanel.SetActive(true);
    }
}
