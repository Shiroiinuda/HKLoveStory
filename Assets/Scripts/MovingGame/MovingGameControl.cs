using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;

public class MovingGameControl : MonoBehaviour
{
    public Button moveButton;
    public Image background;
    
    private bool isGameActive;

    private bool isReachHalf;

    [SerializeField] private InvestigationControl investigationControl;

    private void Start()
    {
        // Initialize game state
        isGameActive = true;
        isReachHalf = false;

        // Add button click event listener
        moveButton.onClick.AddListener(MoveButtonClicked);
    }

    
    private void MoveButtonClicked()
    {
        if (isGameActive)
        {
            // Move background image to the right (player moving to the left)
            background.transform.position -= new Vector3(20f, 0f, 0f);
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        if (isReachHalf == false && background.rectTransform.position.x <= 960)
        {
            isReachHalf = true;
            StartCoroutine(investigationControl.EndInvestigation());
        }

        // Check if the background image has moved completely to the right
        if (background.rectTransform.position.x <= 0) // Adjust the threshold as per your level design
        {
            StartCoroutine(investigationControl.EndInvestigation());
            isGameActive = false;
        }
    }
}
