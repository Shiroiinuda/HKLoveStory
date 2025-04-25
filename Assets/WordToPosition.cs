using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;

public class WordToPosition : MonoBehaviour
{
    public float fadeInDuration = 1f;
    public float moveDuration = 5f;
    public Vector3 targetScale = new Vector3(3f, 3f, 3f); // Scale the text will grow to
    public Vector2 movementRange = new Vector2(1000f, 200f);

    private Vector3 initialScale;
    private Vector3 targetPosition;
    private float elapsedTime = 0f;
    public TextMeshProUGUI textMesh;
    public Localize localize;

void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;

        // Set a random position within the defined X and Y range in local space
        float randomX = Random.Range(-movementRange.x, movementRange.x);
        float randomY = Random.Range(movementRange.y/2, movementRange.y);
        targetPosition = new Vector3(randomX, randomY, rectTransform.localPosition.z);

        // Start with fully transparent text
        Color startColor = textMesh.color;
        startColor.a = 0f;
        textMesh.color = startColor;

        // Start the coroutine to handle the effect
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Fade in and scale up
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeInDuration;

            // Fade in
            Color color = textMesh.color;
            color.a = Mathf.Lerp(0f, 1f, progress);
            textMesh.color = color;

            yield return null; // Wait for the next frame
        }

        // Move to the target position within the defined range in local space
        elapsedTime = 0f;
        Vector3 startPosition = rectTransform.localPosition;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveDuration;
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            // Move towards the target position
            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);

            yield return null; // Wait for the next frame
        }

        // Destroy the object after the animation is done
        Destroy(gameObject);
    }
}
