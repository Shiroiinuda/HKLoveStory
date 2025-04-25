using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossGameButton : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI TextMeshProUGUI;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    public void AssignLetter(string letter)
    {
        TextMeshProUGUI.text = letter;
    }
    
}
