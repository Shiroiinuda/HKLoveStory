using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInventory : MonoBehaviour
{
    public GameControl gameControl;
    public InventoryManager inventoryManager;

    // Start is called before the first frame update

    private void OnEnable()
    {
        this.GetComponent<TextMeshProUGUI>().text = "";

        foreach (string gameControlItem in gameControl.items)
        {
            this.GetComponent<TextMeshProUGUI>().text += (gameControlItem + "(Gamecontrol Item), ");
        }
        foreach (string gameControlCollection in gameControl.collections)
        {
            this.GetComponent<TextMeshProUGUI>().text += (gameControlCollection + "(Gamecontrol Collection), ");
        }

        this.GetComponent<TextMeshProUGUI>().text += "\n";

        foreach (InventoryItems inventoryItem in inventoryManager.items)
        {
            this.GetComponent<TextMeshProUGUI>().text += (inventoryItem.name + "(inventoryManager Item), ");
        }
        foreach (InventoryItems inventoryCollection in inventoryManager.collections)
        {
            this.GetComponent<TextMeshProUGUI>().text += (inventoryCollection.name + "(inventoryManager Collection), ");
        }
    }

}
