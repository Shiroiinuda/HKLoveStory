using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Normal Item")]

public class NormalInvestigationItems : ScriptableObject
{
    public string keyValue;

    public List<string> itemText;

    public bool needKey;
    public InventoryItems key;
    public bool delKey;

    public List<string> unlockedText;

    public AudioClip unlockedSFx;
    public AudioClip lockedSFx;

    public bool triggerEnd;
    public bool enalbleBtnAfterEnd;
    public int jumpTo;
}
