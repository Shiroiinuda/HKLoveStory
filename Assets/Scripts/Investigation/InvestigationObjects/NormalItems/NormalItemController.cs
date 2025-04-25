using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Investigation;
using EasierLocalization;
using UnityEngine;
using UnityEngine.UI;

// using UnityEngine.Localization.Components;

public class NormalItemController : MonoBehaviour
{
    [Header("Item ScriptableObject")] public NormalInvestigationItems normalItem;

    [Header("Localization")] [Space(2)]
    public List<string> _normalItemText = new List<string>();
    public List<string> _normalItemUnlockedText = new List<string>();

    [Space(5)] [Header("ItemControl")] public GameControl gameControl;
    public DialogueManager dialogueManager;
    public InventoryManager inventoryManager;
    public InvestigationControl investigationControl;
    public AudioManager audioManager;
    
    private Button itemBtn;
    private Image itemImage;

    //For Unlock and if have Puzzle Game
    public List<GameObject> unlockObj;
    public PuzzleGameControl puzzleGameControl;
    public string completedTask;
        
    private void Start()
    {
        itemBtn = GetComponent<Button>();
        itemBtn.onClick.AddListener(NormalItemClickedAction);

        itemImage = GetComponent<Image>();

        SetItemText(normalItem.name);
        SetItemUnlockText(normalItem.name);

    }

    private void NormalItemClickedAction()
    {
        if (normalItem.needKey)
        {
            if (inventoryManager.HasItem(normalItem.key))
            {
                UnlockItem();

                if (normalItem.triggerEnd)
                {
                    HandleEndTrigger();
                }
                else
                {
                    investigationControl.ShowSentences(_normalItemUnlockedText);
                }
            }
            else
            {
                //PlaySoundIfNotNull
                PlaySoundIfNotNull(normalItem.lockedSFx);
                investigationControl.ShowSentences(_normalItemText);
            }
        }
        else
        {
            if (puzzleGameControl != null)
            {
                puzzleGameControl.AddCompletedTask(completedTask);
            }

            //PlaySoundIfNotNull
            PlaySoundIfNotNull(normalItem.lockedSFx);

            if (normalItem.triggerEnd)
            {
                HandleEndTrigger();
                if (puzzleGameControl != null)
                    puzzleGameControl.isReturningFromDialogue = true;
            }
            else
            {
                if (_normalItemText.Count > 0)
                    investigationControl.ShowSentences(_normalItemText);
            }

            if (unlockObj.Count > 0)
            {
                foreach (GameObject obj in unlockObj)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

    private void UnlockItem()
    {
        //play item.unlockedSFx
        PlaySoundIfNotNull(normalItem.unlockedSFx);

        //Remove Key from inventory
        if (normalItem.delKey)
            inventoryManager.RemoveItem(normalItem.key);
        if (normalItem.triggerEnd == false)
        {
            if (unlockObj.Count > 0)
            {
                foreach (GameObject obj in unlockObj)
                {
                    obj.SetActive(true);
                }
            }

            if(puzzleGameControl != null)
            {
                puzzleGameControl.AddCompletedTask(completedTask);
            }

            this.gameObject.SetActive(false);

        }
    }

    private void PlaySoundIfNotNull(AudioClip clip)
    {
        if (clip != null)
            audioManager.PlaySFX(clip);
    }

    private void HandleEndTrigger()
    {
        if (normalItem.enalbleBtnAfterEnd == false)
        {
            itemBtn.enabled = false;
            itemImage.raycastTarget = false;
        }

        CheckJumpTo();
    }

    private void CheckJumpTo()
    {
        if (normalItem.jumpTo > 0)
        {
            dialogueManager.jumpMark = normalItem.jumpTo;

            if (normalItem.needKey)
            {
                if (normalItem.unlockedText.Count == 0)
                {
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
                else
                {
                    investigationControl.isEnd = true;
                    investigationControl.ShowSentences(_normalItemUnlockedText);
                }
            }
            else
            {
                if (normalItem.itemText.Count == 0)
                {
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
                else
                {
                    investigationControl.isEnd = true;
                    investigationControl.ShowSentences(_normalItemText);
                }
            }
        }
        else
        {
            if (normalItem.needKey)
            {
                if (normalItem.unlockedText.Count == 0)
                {
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
                else
                {
                    investigationControl.isEnd = true;
                    investigationControl.ShowSentences(_normalItemUnlockedText);
                }
            }
            else
            {
                if (normalItem.itemText.Count == 0)
                {
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
                else
                {
                    investigationControl.isEnd = true;
                    investigationControl.ShowSentences(_normalItemText);
                }
            }
        }
    }

    void SetItemText(string itemkeyvalue)
    {
        string itemText = Localization.GetString($"NormalItemText/{itemkeyvalue}");
        _normalItemText.Clear();
        string[] splitString = itemText.Trim().Split('&');
        if (!string.IsNullOrEmpty(itemText))
            _normalItemText = splitString.ToList();
    }

    void SetItemUnlockText(string itemkeyvalue)
    {
        string unlockedText = Localization.GetString($"NormalItemUnlockedText/{itemkeyvalue}");
        _normalItemUnlockedText.Clear();
        string[] splitString = unlockedText.Trim().Split('&');
        if (!string.IsNullOrEmpty(unlockedText))
            _normalItemUnlockedText = splitString.ToList();
    }
}