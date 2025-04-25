using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableGameObject : MonoBehaviour
{
    public GameControl gameControl;
    [SerializeField] private DragGameControl dragGameControl;

    private bool isDragging = false;
    private Vector3 initialPosition;
    private Vector3 previousPosition;

    [SerializeField] private GameObject goalPosition;
    private Collider2D targetCollider; // Collider component of the targetObject
    private Collider2D thisCollider; // Collider component of this object


    [SerializeField] private GameObject goalImg;

    [SerializeField] private Image imgObg;

    [SerializeField] private Sprite stillImg;
    [SerializeField] private Sprite draggingImg;
    [SerializeField] private Sprite shakeImg;
    
    [SerializeField] private bool needShake;
    private float shakeThreshold = 300f; // Minimum shake threshold to trigger shake detection

    private int shakeCount;

    public GameObject[] nextButton;
    public bool isMyTurn;

    void Start()
    {
        previousPosition = transform.position;
        initialPosition = transform.position;
        imgObg.sprite = this.gameObject.GetComponent<Image>().sprite;

        shakeCount = 0;

        // Get the colliders of both objects
        targetCollider = goalPosition.GetComponent<Collider2D>();
        thisCollider = GetComponent<Collider2D>();

        // Make sure both objects have colliders
        if (targetCollider == null || thisCollider == null)
        {
            Debug.LogError("Both objects must have Collider components!");
            enabled = false; // Disable the script if colliders are missing
        }
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;
            touchPosition.z = 0f;

            if (isMyTurn)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        // Check if the touch position is on the object
                        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                        if (hit.collider != null && hit.collider.gameObject == gameObject)
                        {
                            isDragging = true;
                            imgObg.sprite = draggingImg;
                        }
                        break;

                    case TouchPhase.Moved:

                        if (isDragging)
                        {
                            float movementDelta = Vector3.Distance(touchPosition, previousPosition);

                            if (needShake && movementDelta >= shakeThreshold)
                            {
                                shakeCount += 1;

                                if (shakeCount >= 10)
                                {
                                    if (shakeImg != null)
                                        imgObg.sprite = shakeImg;
                                    if (gameControl != null)
                                        gameControl.OnUnlockAchievement("PlayFong");
                                }
                            }

                            previousPosition = touchPosition;

                            // Update the position of the object based on finger movement
                            transform.position = touchPosition;
                        }
                        break;

                    case TouchPhase.Ended:

                        if (isDragging)
                        {
                            // Check if this object is inside the targetObject
                            bool isInside = IsObjectInsideTarget();

                            // Do something based on the result
                            if (isInside)
                            {
                                // Object is in the correct position
                                dragGameControl.CheckWinGame();
                                goalImg.SetActive(true);
                                if (nextButton != null)
                                {
                                    foreach (GameObject nextObject in nextButton)
                                    {
                                        nextObject.GetComponent<DraggableGameObject>().isMyTurn = true;
                                        nextObject.GetComponent<BoxCollider2D>().enabled = true;
                                    }
                                }
                                this.gameObject.SetActive(false);
                            }
                            else
                            {
                                // Object is not in the correct position, reset its position
                                transform.position = initialPosition;
                                imgObg.sprite = stillImg;
                            }

                            // Reset shake detection flag
                            shakeCount = 0;
                            isDragging = false;

                        }
                        break;
                }
            }
        }
    }

    private bool IsObjectInsideTarget()
    {
        // Create bounding boxes for each object
        Bounds targetBounds = targetCollider.bounds;
        Bounds thisBounds = thisCollider.bounds;

        // Check if this object's bounding box is fully contained inside the targetObject's bounding box
        return targetBounds.Contains(thisBounds.min) && targetBounds.Contains(thisBounds.max);
    }
}
