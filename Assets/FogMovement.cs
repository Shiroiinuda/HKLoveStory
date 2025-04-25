using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Ensure DOTween is imported
using Random = UnityEngine.Random;

public class FogMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    public FogSpawner spawner;
    public string fogType;

    [Header("Click Logic")]
    public int clickTime;       // Number of clicks so far
    public int requireTime;     // Required clicks to return fog to the pool

    [Header("Scaling Settings")]
    public float shrinkFactor = 0.8f;       // Amount to shrink per click
    public float shrinkDuration = 0.2f;     // Time (seconds) for each shrink animation
    public float returnDuration = 0.5f;     // Time (seconds) to reset back to full size

    [Header("Reset Logic")]
    public float resetThreshold = 2f; // Time (seconds) after last click to reset size

    private Button buttonComponent;
    private bool isClickedRecently;
    private float timeSinceLastClick;

    private void Awake()
    {
        // Assign a random requiredTime within a specified range
        requireTime = Random.Range(2, 4);

        // Attach the click event to the Button
        buttonComponent = GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(OnFogClicked);
        }
    }

    private void Update()
    {
        // Move fog to the left
        transform.localPosition += Vector3.left * speed * Time.deltaTime;

        // Check if the fog is out of bounds and return it to the pool if so
        RectTransform parentRect = transform.parent as RectTransform;
        if (parentRect != null)
        {
            if (transform.localPosition.x < -parentRect.rect.width / 2 - 600)
            {
                spawner.ReturnFogToPool(gameObject, fogType);
            }
        }

        // If recently clicked, track elapsed time
        if (isClickedRecently)
        {
            timeSinceLastClick += Time.deltaTime;

            // Reset scale back to Vector3.one if no additional clicks occur within threshold
            if (timeSinceLastClick >= resetThreshold)
            {
                isClickedRecently = false;
                timeSinceLastClick = 0f;
                transform.DOScale(Vector3.one, returnDuration).SetEase(Ease.OutQuad);
            }
        }
    }
    private void OnEnable()
    {
        requireTime = Random.Range(2, 4);
        clickTime = 0;
    }
    private void OnFogClicked()
    {
        clickTime++;
        isClickedRecently = true;
        timeSinceLastClick = 0f;
        SoundControl.SoundManager.PlaySfx("Cloud");
        // Once the fog meets or exceeds required clicks, return to the pool
        if (clickTime >= requireTime)
        {
            spawner.ReturnFogToPool(gameObject, fogType);
            return;
        }

        // Shrink the fog a bit to indicate damage
        Vector3 newScale = transform.localScale * shrinkFactor;
        transform.DOScale(newScale, shrinkDuration).SetEase(Ease.OutQuad);
    }
}
