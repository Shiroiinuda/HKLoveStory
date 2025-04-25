using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "InventoryItem")]
public class InventoryItems : ScriptableObject
{
    public string keyValue;

    public List<string> itemText;

    public string itemName;
    public string itemDiscription;

    public Sprite itemSprite;

    public bool inventory;
    public bool collection;

    public bool needSave;

    public bool triggerEnd;

    public string path;
}
