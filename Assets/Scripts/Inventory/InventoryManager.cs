using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Localization")] [Space(2)]
    // public LocalizeStringEvent _itemNameStringEvent;
    public string _normalItemName;

    // public LocalizeStringEvent _itemDescriptionStringEvent;

    public string _normalItemDescription;


    [Space(5)] [Header("ItemControl")] [SerializeField]
    GameControl gameControl;

    [SerializeField] AudioManager audioManager;

    public AudioClip pickupSound;

    public List<InventoryItems> items = new List<InventoryItems>();
    public List<InventoryItems> collections = new List<InventoryItems>();

    public GameObject getItemPanel;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemImg;

    private void Start()
    {
        InitiallizeItem();
    }

    private void InitiallizeItem()
    {
        items = new List<InventoryItems>(gameControl.items.Count);
        collections = new List<InventoryItems>(gameControl.collections.Count);

        for (int i = 0; i < gameControl.items.Count; i++)
        {
            string savedItemsPath = (gameControl.itemPath[i]).Trim();
            string savedItemsName = (gameControl.items[i]).Trim();

            string itemPath = savedItemsPath + "/" + savedItemsName;
            itemPath = itemPath.Trim();

            InventoryItems addItem = Resources.Load<InventoryItems>(itemPath);

            if (addItem != null)
                items.Add(addItem);
        }

        for (int i = 0; i < gameControl.collections.Count; i++)
        {
            string savedCollectionPath = (gameControl.collectionPath[i]).Trim();
            string savedCollectionName = (gameControl.collections[i]).Trim();

            string collectionPath = savedCollectionPath + "/" + savedCollectionName;
            collectionPath = collectionPath.Trim();

            InventoryItems addCollection = Resources.Load<InventoryItems>(collectionPath);

            if (addCollection != null)
                collections.Add(addCollection);
        }
    }

    public bool AddItem(InventoryItems item)
    {
        if (item.inventory)
            items.Add(item);
        else if (item.collection)
            collections.Add(item);

        SavetoGameControl();

        return true;
    }

    public bool RemoveItem(InventoryItems item)
    {
        if (!items.Contains(item))
        {
            return false;
        }

        int index = items.IndexOf(item);
        items.Remove(item);

        SavetoGameControl();

        return true;
    }

    public bool HasItem(InventoryItems item)
    {
        if (item.inventory)
            return items.Contains(item);
        else if (item.collection)
            return collections.Contains(item);

        return false;
    }

    private void GetitemName(string itemkeyvalue)
    {
        //Debug.Log(itemkeyvalue);
        _normalItemName = Localization.GetString($"ItemName/{itemkeyvalue}");
        itemName.text = _normalItemName;
        _normalItemDescription = Localization.GetString($"ItemDescription/{itemkeyvalue}");
        itemDescription.text = _normalItemDescription;
    }

    public void SetGetItemPenel(InventoryItems items)
    {
        GetitemName(items.keyValue);
        //Pop up pick SFx
        audioManager.PlaySFX(pickupSound);

        getItemPanel.SetActive(true);
        itemImg.sprite = items.itemSprite;
    }


    public void ClearInventory()
    {
        InitiallizeItem();

        Debug.Log("clear");
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].needSave == false)
            {
                // Remove the item from the list
                items.RemoveAt(i);
            }
        }

        SavetoGameControl();
    }

    //GameControl items and collections = this items and collections
    private void SavetoGameControl()
    {
        gameControl.items.Clear();
        gameControl.itemPath.Clear();
        gameControl.collections.Clear();
        gameControl.collectionPath.Clear();

        gameControl.items = new List<string>(items.Count);
        gameControl.itemPath = new List<string>(items.Count);
        gameControl.collections = new List<string>(collections.Count);
        gameControl.collectionPath = new List<string>(collections.Count);

        for (int i = 0; i < items.Count; i++)
        {
            gameControl.items.Add((items[i].name).Trim());
            gameControl.itemPath.Add((items[i].path).Trim());
        }

        for (int j = 0; j < collections.Count; j++)
        {
            gameControl.collections.Add((collections[j].name).Trim());
            gameControl.collectionPath.Add((collections[j].path).Trim());
        }
    }
}