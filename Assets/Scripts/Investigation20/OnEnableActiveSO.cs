using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableActiveSO : MonoBehaviour
{
    public GameObject inventoryItemObj;
    private Button inventoryItemBtn;

    private void OnEnable()
    {
        inventoryItemBtn = inventoryItemObj.GetComponent<Button>();

        if (inventoryItemObj.transform.childCount > 0)
        {
            GameObject childObject = inventoryItemObj.transform.GetChild(0).gameObject; // get the first child game object
            childObject.SetActive(true); // set the child game object to activate    
        }

        inventoryItemBtn.enabled = true;
        inventoryItemBtn.GetComponent<Image>().raycastTarget = true;
    }
}
