using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioClip lockSFX;
    [SerializeField] AudioClip clickSFX;

    [SerializeField] InventoryUIManager inventoryUIManager;
    private InventoryItems _item;
    public InventoryItems item
    {
        get => _item;
        set
        {
            if (_item == null)
//                Debug.Log(value);
                imgObj.GetComponent<Image>().sprite = inventoryUIManager.emptyLine;
            _item =  value;
        }
    }
    public GameObject imgObj;


    public void OnSlotClicked()
    {
        if (item != null)
        {
            audioManager.PlayButtonSFX(clickSFX);
            inventoryUIManager.ShowItemContent(item);
        }
        else
            audioManager.PlayButtonSFX(lockSFX);
    }
}
