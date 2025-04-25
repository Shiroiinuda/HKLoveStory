using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearManager : MonoBehaviour
{
    public LoadSceneManager loadSceneManager;
    public GameControl gameControl;
    public DialogueManager dialogueManager;
    public CurrencyManager currencyManager;

    public GameObject confirmPayPanel;
    public GameObject shopPanel;
    public Animator NoCurrencyNotice;

    public Button payButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Button backButton;
    public GameObject cost;
    private const string chapterPP = "CurrentChapter";

    private void OnEnable()
    {
        if(cost ==null) return;
#if !(UNITY_ANDROID || UNITY_IOS)
        cost.SetActive(false);
#else
        if (!gameControl.unLockedSavept.Contains(dialogueManager.checkNextLvDialogue.saveChapter))
            cost.SetActive(true);
        else
            cost.SetActive(false);

        if (dialogueManager.currentDialogue.saveChapter == "Chapter4")
            dialogueManager.commentBox.SetActive(true);
        else
            dialogueManager.commentBox.SetActive(false);

#endif
    }

    public void OnMenuClicked()
    {
        Debug.Log(dialogueManager.currentDialogue.saveChapter);
        FBPP.SetString(chapterPP,dialogueManager.currentDialogue.saveChapter);
        loadSceneManager.FadeToLevel(2);
    }

    public void OnNextLvClicked()
    {
        payButton?.onClick.RemoveListener(PayUnlockChapter);

        // If a checkSavePt object with saveChapter was found, check enough currency and do the purchasing
        if (!gameControl.unLockedSavept.Contains(dialogueManager.checkNextLvDialogue.saveChapter))    //Savept not yet unlocked
        {
#if !(UNITY_ANDROID || UNITY_IOS)
                gameControl.unLockedSavept.Add(dialogueManager.checkNextLvDialogue.saveChapter);
                Debug.Log($"Next Chapter : {dialogueManager.currentDialogue.nextLv}");
                gameControl.currentBookmark = dialogueManager.currentDialogue.nextLv;
                SaveData.Instance.data.currentBookmark = dialogueManager.currentDialogue.nextLv;
                loadSceneManager.FadeToLevel(3);
#else
            if (gameControl.currency >= 10)
                {
                    confirmPayPanel.SetActive(true);
                    payButton.onClick.AddListener(PayUnlockChapter);
                    noButton.onClick.AddListener(OnConfirmPayPanelClose);
                    backButton.onClick.AddListener(OnConfirmPayPanelClose);
                }
                else
                {
                    shopPanel.SetActive(true);
                    NoCurrencyNotice.SetTrigger("NoCurrency");
                }
            #endif
            
        }
        else                           
        {
            //SavePt unlocked
            Debug.Log($"Next Chapter : {dialogueManager.currentDialogue.nextLv}");
            gameControl.currentBookmark = dialogueManager.currentDialogue.nextLv;
            SaveData.Instance.data.currentBookmark = dialogueManager.currentDialogue.nextLv;
            loadSceneManager.FadeToLevel(3);
        }
    }

    private void PayUnlockChapter()
    {
        gameControl.currency -= 10;
        currencyManager.ShowCurrency();

        gameControl.unLockedSavept.Add(dialogueManager.checkNextLvDialogue.saveChapter);

        gameControl.currentBookmark = dialogueManager.currentDialogue.nextLv;
        loadSceneManager.FadeToLevel(3);
    }

    public void OnConfirmPayPanelClose()
    {
        payButton.onClick.RemoveListener(PayUnlockChapter);
    }
}
