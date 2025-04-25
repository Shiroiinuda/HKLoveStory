using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using UnityEngine;
// using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class Choices : MonoBehaviour
{
    public TextMeshProUGUI choiceTxt;
    public Button choiceBtn;
    
    public ChoiceManager choiceManager;
    public int jumpMark;
    public bool clicked;
    public string choiceMode;
    public InventoryItems unlockChoiceItem;

    private void Start() =>
        choiceBtn.onClick.AddListener(() => choiceManager.OnChoiceButtonClicked(this));
    
    public void UpdateLocalize(string key) =>
        choiceTxt.text = Localization.GetString($"Dialogue/{key}");
}