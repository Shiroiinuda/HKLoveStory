using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckActive : MonoBehaviour
{

    [SerializeField] private GameControl gameControl;
    [SerializeField] private InventoryItems inventoryItems;
    [SerializeField] private bool checkOnlyItemButton = false;
    [SerializeField] private Button checkButton;
    [SerializeField] private bool needActiveObj = false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (checkOnlyItemButton)
        {
            if (checkButton != null)
                this.gameObject.SetActive(checkButton.isActiveAndEnabled);
        }
        else
        {
            if (gameControl.items.Contains(inventoryItems.name))
                this.gameObject.SetActive(needActiveObj);
            else
                this.gameObject.SetActive(!needActiveObj);
        }
    }
}
