using MyBox;
using UnityEngine;

public class CharacterEmote : MonoBehaviour
{
    public Transform target;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;

    [ButtonMethod]
    public void TestShake()
    {
        TriggerShake(shakeDuration, shakeMagnitude);
    }
    public void TriggerShake(float duration, float magnitude)
    {
        if (target == null) return;

        shakeDuration = duration;
        shakeMagnitude = magnitude;
        originalPosition = Vector3.zero;

        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private System.Collections.IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            target.localPosition = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localPosition = originalPosition;
    }
}