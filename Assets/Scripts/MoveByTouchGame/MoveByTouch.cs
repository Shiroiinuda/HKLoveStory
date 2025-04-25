using Investigation;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveByTouch : MonoBehaviour,IDragHandler, IBeginDragHandler
{
    [SerializeField] private InvestigationControl investigationControl;
    public GameObject lightBG;
    public GameObject targetObject;
    private bool gameWon = false;
    private bool isInside = false;
    private Camera mainCamera;
    private RectTransform rectTransform;

    private void WinGame()
    {
        gameWon = true;
        StartCoroutine(investigationControl.EndInvestigation());
    }

    private bool IsObjectInsideTarget()
    {
        // Create bounding boxes for each object
        Bounds targetBounds = targetObject.GetComponent<Collider2D>().bounds;
        Bounds thisBounds = this.gameObject.GetComponent<Collider2D>().bounds;

        // Check if this object's bounding box is fully contained inside the targetObject's bounding box
        return targetBounds.Contains(thisBounds.min) && targetBounds.Contains(thisBounds.max);
    }

    private Vector2 offset;

    private void Start()
    {
        // Get the main camera reference
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
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
        rectTransform.anchoredPosition = localPointerPosition;
        // Apply the offset to the new position
        if (!gameWon)
        {

            lightBG.GetComponent<RectTransform>().anchoredPosition = new Vector3(-(this.gameObject.GetComponent<RectTransform>().anchoredPosition.x), -(this.gameObject.GetComponent<RectTransform>().anchoredPosition.y), 0);

            isInside = IsObjectInsideTarget();
            if (isInside == true)
                WinGame();
        }
    }
}
