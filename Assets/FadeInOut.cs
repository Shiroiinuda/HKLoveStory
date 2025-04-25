using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
using MyBox;
using UnityEngine.Events;

public class FadeInOut : MonoSingleton<FadeInOut>
{
    public Image uiImage;  // The UI Image to fade
    public float fadeDuration = 0.4f;
    public float fadeMidtime = 0.2f;
    public UnityEvent fadeAction;
    public UnityEvent fadeOutAction;
    public bool isFading;
    private async Task FadeInOutAsync()
    {
        DialogueManager.Instance.continueButton.enabled = false;
        isFading = true;
        await Fade(1f);  
        await MiddleAction();
        // Fade out
        await Fade(0f);
        isFading = false;
        fadeOutAction.Invoke();
        fadeAction.RemoveAllListeners();
        fadeOutAction.RemoveAllListeners();
    }
    private async Task Fade(float targetAlpha)
    {
        float startAlpha = uiImage.color.a;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            Color color = uiImage.color;
            color.a = newAlpha;
            uiImage.color = color;
            await Task.Yield(); 
        }
        
        Color finalColor = uiImage.color;
        finalColor.a = targetAlpha;
        uiImage.color = finalColor;
    }
    
    private async Task MiddleAction()
    {
        
        //Debug.Log("Middle Action Executing...");
        await Task.Delay((int)(fadeMidtime * 750));
        fadeAction.Invoke();
        await Task.Delay((int)(fadeMidtime * 250));
        //Debug.Log("Middle Action Completed.");

    }
    [ButtonMethod]
    public async Task StartFadeSequence()
    {
        await FadeInOutAsync();
    }
}