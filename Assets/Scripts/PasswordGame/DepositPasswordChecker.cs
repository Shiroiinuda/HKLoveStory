using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepositPasswordChecker : MonoBehaviour
{

    [SerializeField] AudioManager audioManager;

    [SerializeField] private Button[] numberButtons; // Assign buttons 1¡V9 in the Inspector
    [SerializeField] private Button enterButton; // Assign the Enter button in the Inspector
    [SerializeField] private string setPassword = "1234"; // Set your password here
    private string enteredPassword = ""; // Track what the player enters

    [SerializeField] private List<GameObject> delObjs;
    [SerializeField] private AudioClip clickPWSfx;
    [SerializeField] private AudioClip wrongWSfx;
    [SerializeField] private AudioClip openSfx;


    private void Start()
    {
        // Add listeners to the number buttons
        foreach (Button button in numberButtons)
        {
            button.onClick.AddListener(() => OnNumberButtonPressed(button.name));
        }

        // Add listener to the Enter button
        enterButton.onClick.AddListener(OnEnterButtonPressed);
    }

    private void OnNumberButtonPressed(string buttonName)
    {
        if (audioManager != null && clickPWSfx != null)
            audioManager.PlaySFX(clickPWSfx);

        if (enteredPassword.Length < setPassword.Length)
        {
            enteredPassword += buttonName; // Append button's name to entered password
        }
    }

    private void OnEnterButtonPressed()
    {
        if (enteredPassword == setPassword)
        {
            if (audioManager != null && openSfx != null)
                audioManager.PlaySFX(openSfx);

            foreach (GameObject obj in delObjs)
            {
                obj.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
        else
        {
            if (audioManager != null && wrongWSfx != null)
                audioManager.PlaySFX(wrongWSfx);

            enteredPassword = ""; // Reset entered password
        }
    }
}
