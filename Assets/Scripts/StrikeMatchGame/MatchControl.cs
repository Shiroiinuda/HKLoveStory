using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public StrikeMatchControl strikeMatchControl;
    private Vector3 initialPosition;
    private Camera mainCamera;
    private RectTransform rectTransform;
    
    private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;

        // Make sure object have collider
        if (this.gameObject.GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Both objects must have Collider components!");
            enabled = false;
        }
    }

    
    // Called when the drag operation begins
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Calculate the offset between the RectTransform's position and the pointer position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition
        );
        offset = rectTransform.anchoredPosition - localPointerPosition;
    }

    // Called every time the object is dragged
    public void OnDrag(PointerEventData eventData)
    {
        // Convert the screen position to a local point in the RectTransform's parent
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition
        );

        // Apply the offset to the new position
        rectTransform.anchoredPosition = localPointerPosition + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //// Apply the offset to the original position if the match is on fire
        rectTransform.anchoredPosition = initialPosition;

        if (strikeMatchControl.isFired)
        {
            strikeMatchControl.AddFiredMatchToBag();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object that has a 2D collider
        if (collision.collider != null)
        {
            strikeMatchControl.StopResetStrike();

            if (collision.collider.name == "MatchCollider1")
            {
                strikeMatchControl.startStrike = true;
            }

            if (collision.gameObject.name == "MatchCollider2")
            {
                strikeMatchControl.endStrike = true;
            }

            strikeMatchControl.CheckResult();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        strikeMatchControl.StartResetStrike();
    }
}
