using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Localization")] [Space(2)]
    [SerializeField][ReadOnly] private string itemNameTxt;
    [SerializeField][ReadOnly] private string itemDescriptionTxt;

    [Space(5)] [Header("ItemControl")] [SerializeField]
    private GameControl gameControl;

    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemImg;
    [SerializeField] private Button itemImgBtn;
    [SerializeField] private Image CollectionImage;


    [SerializeField] private List<GameObject> itemSlots = new List<GameObject>();
    [SerializeField] private List<GameObject> collectionSlots = new List<GameObject>();

    [SerializeField] private GameObject inventorySlotContent;
    [SerializeField] private Vector3 inventorySlotPos;

    [SerializeField] private GameObject collectionSlotContent;
    [SerializeField] private Vector3 collectionSlotPos;
    public GameObject inventoryPanel;

    public GameObject inventorySlotBar;
    public GameObject collectionSlotBar;

    public Animator inventoryImg;
    public Animator collectionImg;

    public TextMeshProUGUI collectionCountText;

    public Sprite emptyLine;

    void Start()
    {

        inventorySlotPos = inventorySlotContent.transform.position;
        collectionSlotPos = collectionSlotContent.transform.position;
        ReloadInventoryUI();
        PopulateInventory();
    }

    public void DisableInvent()
    {
        inventoryPanel.SetActive(false);
    }
    private void GetitemName(string itemkeyvalue)
    {
        itemNameTxt = Localization.GetString($"ItemName/{itemkeyvalue}");
        itemName.text = itemNameTxt ;
        itemDescriptionTxt = Localization.GetString($"ItemDescription/{itemkeyvalue}");
        itemDescription.text = itemDescriptionTxt;
    }

    private void ReloadInventoryUI()
    {
        itemName.text = "";
        itemDescription.text = "";
        itemImg.enabled = false;

        // Clear all item slot images immediately
        foreach (var slot in itemSlots)
        {
            slot.GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite = emptyLine;
        }

        foreach (var slot in collectionSlots)
        {
            slot.GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite = emptyLine;
        }

        inventorySlotContent.transform.position = inventorySlotPos;
        collectionSlotContent.transform.position = collectionSlotPos;
    }

    private void PopulateInventory()
    {
        ClearInventory();

        for (int i = 0; i < inventoryManager.items.Count; i++)
        {
            itemSlots[i].GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite = emptyLine;
            itemSlots[i].GetComponent<ItemSlot>().item = inventoryManager.items[i];
            if (inventoryManager.items[i].itemSprite == null)
            {
                itemSlots[i].GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite = emptyLine;
                continue;
            }
            itemSlots[i].GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite =
                inventoryManager.items[i].itemSprite;
            itemSlots[i].GetComponent<ItemSlot>().imgObj.SetActive(true);
        }

        for (int i = 0; i < inventoryManager.collections.Count; i++)
        {
            collectionSlots[i].GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite = emptyLine;
            collectionSlots[i].GetComponent<ItemSlot>().item = inventoryManager.collections[i];
                
                if (inventoryManager.collections[i].itemSprite == null)
                {
                    collectionSlots[i].GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite = emptyLine;
                    continue;
                }
                
            collectionSlots[i].GetComponent<ItemSlot>().imgObj.GetComponent<Image>().sprite =
                inventoryManager.collections[i].itemSprite;
            collectionSlots[i].GetComponent<ItemSlot>().imgObj.SetActive(true);
        }
    }

    private void ClearInventory()
    {
        foreach (GameObject item in itemSlots)
        {
            item.GetComponent<ItemSlot>().item = null;
        }

        foreach (GameObject item in collectionSlots)
        {
            item.GetComponent<ItemSlot>().item = null;
        }
    }

    public void ShowItemContent(InventoryItems item)
    {
        if (item != null)
        {
            GetitemName(item.keyValue);
            itemName.text = itemNameTxt;
            itemDescription.text = itemDescriptionTxt;

            itemImg.sprite = item.itemSprite;
            itemImg.enabled = true;

            if (item.collection)
            {
                itemImgBtn.interactable = true;
                CollectionImage.sprite = item.itemSprite;
            }
            else
                itemImgBtn.interactable = false;
        }
    }

    public void OnBagClicked()
    {
        inventorySlotBar.SetActive(true);
        collectionSlotBar.SetActive(false);
        collectionCountText.text = "";

        collectionImg.Play("Normal");
        inventoryImg.Play("Selected");

        ReloadInventoryUI();
        PopulateInventory();
    }

    public void OnInventoryPageClicked()
    {
        inventorySlotBar.SetActive(true);
        collectionSlotBar.SetActive(false);
        collectionCountText.text = "";
        collectionImg.Play("Normal");
        inventoryImg.Play("Selected");


        ReloadInventoryUI();
        PopulateInventory();
    }

    public void OnCollectionPageClicked()
    {
        inventorySlotBar.SetActive(false);
        collectionSlotBar.SetActive(true);
        collectionCountText.text = inventoryManager.collections.Count + "/19";
        collectionImg.Play("Selected");
        inventoryImg.Play("Normal");


        ReloadInventoryUI();
        PopulateInventory();
    }
}