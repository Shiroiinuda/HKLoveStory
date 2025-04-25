using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableLetter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public LetterSortingGame letterSortingGame;
    public GameObject targetObject;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private bool isInside = false;
    [SerializeField] private int sequenceIndex;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Awake()
    {
        originalPosition = GetComponent<RectTransform>().anchoredPosition; // Store the original position
    }

    // Called when the drag operation begins
    public void OnBeginDrag(PointerEventData eventData)
    {
        //sfkjsdl
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert the mouse position to world position
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the letter is inside the drop area
        if (!letterSortingGame.letterSortGameWon)
        {
            isInside = IsObjectInsideTarget();
            if (isInside == true)
            {
                if (sequenceIndex == letterSortingGame.correctSequence[letterSortingGame.enteredSequence.Count]) // Replace with actual sequence check
                {
                    // Correct placement logic here (e.g. snap to position)
                    letterSortingGame.enteredSequence.Add(sequenceIndex);
                    GetComponent<RectTransform>().anchoredPosition = originalPosition;
                    this.gameObject.SetActive(false);
                    letterSortingGame.CheckSortCompleted();
                }
                else
                {
                    // Clear the enteredSequence if the sequence is incorrect
                    letterSortingGame.enteredSequence.Clear();
                    // Snap to original position if sequence is incorrect
                    GetComponent<RectTransform>().anchoredPosition = originalPosition;
                    letterSortingGame.ReleaseLetters();
                }
            }
            else
            {
                // Snap back to original position if not in drop area
                GetComponent<RectTransform>().anchoredPosition = originalPosition;
            }
        }
    }

    private bool IsObjectInsideTarget()
    {
        // Create bounding boxes for each object
        Bounds targetBounds = targetObject.GetComponent<Collider2D>().bounds;
        Bounds thisBounds = this.gameObject.GetComponent<Collider2D>().bounds;

        // Check if this object's bounding box is fully contained inside the targetObject's bounding box
        return targetBounds.Contains(thisBounds.min) && targetBounds.Contains(thisBounds.max);
    }
}
