using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Investigation;
using EasierLocalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// using UnityEngine.Localization.Components;

public class InventoryItemsController : MonoBehaviour
{
    [Header("Item ScriptableObject")] public InventoryItems inventoryItems;

    [Header("Localization")] [Space(2)]
    public List<string> _ItemText = new List<string>();

    [Space(5)] [Header("ItemControl")] [SerializeField]
    private InventoryManager inventoryManager;

    private Button inventoryItemBtn;

    [SerializeField] private InvestigationControl investigationControl;


    private void Start()
    {
        inventoryItemBtn = GetComponent<Button>();
        inventoryItemBtn.onClick.AddListener(InventoryItemClickedAction);

        SetItemText(inventoryItems.name);

        if (inventoryItems.collection)
        {
            if (inventoryManager.HasItem(inventoryItems))
                this.gameObject.SetActive(false);
        }
    }


    public void InventoryItemClickedAction()
    {
        PickItem();

        if (inventoryItems.triggerEnd)
        {
            if (inventoryItems.itemText == null)
            {
                inventoryManager.getItemPanel.SetActive(false);
                StartCoroutine(investigationControl.EndInvestigation());
            }
            else
            {
                investigationControl.isEnd = true;
                investigationControl.ShowSentences(_ItemText);
            }
        }
        else
            investigationControl.ShowSentences(_ItemText);
    }

    private void PickItem()
    {
        inventoryManager.SetGetItemPenel(inventoryItems);

        if (transform.childCount > 0)
        {
            GameObject childObject = transform.GetChild(0).gameObject; // get the first child game object
            childObject.SetActive(false); // set the child game object to inactive    
        }

        inventoryItemBtn.enabled = false;
        inventoryItemBtn.GetComponent<Image>().raycastTarget = false;

        if (inventoryManager.HasItem(inventoryItems) == false)
        {
            inventoryManager.AddItem(inventoryItems);
        }

        if (inventoryItems.collection && inventoryManager.collections.Count >= 19)
            GameControl.instance.OnUnlockAchievement("Collectibles");

    }

    void SetItemText(string itemkeyvalue)
    {
        string itemText = Localization.GetString($"ItemText/{itemkeyvalue}");
        _ItemText.Clear();
        string[] splitString = itemText.Trim().Split('&');
        if (!string.IsNullOrEmpty(itemText))
            _ItemText = splitString.ToList();
    }
}