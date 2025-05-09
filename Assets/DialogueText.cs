using System.Collections;
using DG.Tweening;
using EasierLocalization;
using I2.Loc;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI narrativeText;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private Image dialogueBox;
    [SerializeField] private Image arrow;
    [SerializeField] private Sprite dialogueSprite; 
    [SerializeField] private Sprite narrtiveSprite;
    [Header("State")]
    [SerializeField, ReadOnly] private string currentSentence;
    [SerializeField, ReadOnly] private string currentSpeakerName;
    [SerializeField, ReadOnly] private Dialogue currentDialogue;
    [SerializeField, ReadOnly] private bool isTyping;

    private Coroutine displayLineCoroutine;
    private Coroutine currentlyDisplay;
    private Coroutine typingText;

    public bool IsTyping => isTyping;

    public void LoadDialogue(Dialogue dialogue)
    {
        
        currentDialogue = dialogue;
        currentSpeakerName = Localization.GetString($"Speaker/{currentDialogue.speakerName}");

        // Handle Speaker UI
        if (speakerName != null)
        {
            speakerName.text = currentSpeakerName;
            if (speakerName.transform.parent != null)
            {
                speakerName.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(currentSpeakerName));
            }
        }

        // Start typing the dialogue
        StartTyping(Localization.GetString($"Dialogue/{currentDialogue.bookMark}"));
    }

    private void StartUIAnimation()
    {
        arrow.gameObject.SetActive(false);
        switch (currentDialogue.speakerID)
        {
            case 800: 
                if (dialogueBox != null && narrtiveSprite != null)
                {
                    dialogueBox.sprite = narrtiveSprite;
                }
                break;

            case var id when id >= 0 && id <= 996:
                if (dialogueBox != null  && dialogueSprite != null)
                {
                    dialogueBox.sprite = dialogueSprite;
                }
                break;
        }
        dialogueBox.rectTransform.sizeDelta = new Vector2(dialogueBox.rectTransform.sizeDelta.x,33);
        dialogueBox.rectTransform.DOSizeDelta(new Vector2(dialogueBox.rectTransform.sizeDelta.x,400), 0.75f).SetEase(Ease.OutCubic);
    }
    public void SkipTyping()
    {
        if (!isTyping) return;

        
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
            displayLineCoroutine = null;
        }

        if (typingText != null)
        {
            StopCoroutine(typingText);
            typingText = null;
        }

        // Display full text immediately
        switch (currentDialogue.speakerID)
        {
            case 800: // Narrator
                if (narrativeText != null)
                {
                    narrativeText.text = currentSentence + "<sprite=15>";
                }
                break;
            case var id when id >= 0 && id <= 996: // Normal dialogue
                if (dialogueText != null && !string.IsNullOrEmpty(currentSentence))
                {
                    dialogueText.text = currentSentence + "<sprite=15>";
                }
                break;
        }
        isTyping = false;
        arrow.gameObject.SetActive(true);
    }
    public void SkipTyping(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        if (!isTyping) return;

        
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
            displayLineCoroutine = null;
        }

        if (typingText != null)
        {
            StopCoroutine(typingText);
            typingText = null;
        }

        // Display full text immediately
        switch (currentDialogue.speakerID)
        {
            case 800: // Narrator
                if (narrativeText != null)
                {
                    narrativeText.text = currentSentence + "<sprite=15>";
                }
                break;
            case var id when id >= 0 && id <= 996: // Normal dialogue
                if (dialogueText != null && !string.IsNullOrEmpty(currentSentence))
                {
                    dialogueText.text = currentSentence + "<sprite=15>";
                }
                break;
        }
        isTyping = false;
        arrow.gameObject.SetActive(true);
    }
    private void StartTyping(string dialogueString)
    {
        isTyping = true;
        currentSentence = dialogueString;

        // Stop any existing typing
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        // Clear and hide text components
        if (dialogueText != null)
        {
            dialogueText.text = "";
            dialogueText.gameObject.SetActive(false);
        }
        if (narrativeText != null)
        {
            narrativeText.text = "";
            narrativeText.gameObject.SetActive(false);
        }

        StartUIAnimation();
        displayLineCoroutine = StartCoroutine(TypeSentence(dialogueString));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        yield return new WaitForSeconds(0.25f);
        isTyping = true;
        yield return TypeDefaultDialogue(sentence);
        
        yield return new WaitForSeconds(0.1f); // Small pause at end
        isTyping = false;
        arrow.gameObject.SetActive(true);
    }

    private IEnumerator TypeDefaultDialogue(string sentence)
    {
        if (typingText != null)
        {
            StopCoroutine(typingText);
            typingText = null;
        }

        switch (currentDialogue.speakerID)
        {
            case 800: // Narrator
                if (narrativeText != null)
                {
                    yield return currentlyDisplay = typingText = StartCoroutine(TypeText(sentence, narrativeText, 0.05f));
                }
                break;

            case var id when id >= 0 && id <= 996: // Normal dialogue
                if (dialogueText != null && !string.IsNullOrEmpty(currentSentence))
                {
                    yield return currentlyDisplay = typingText = StartCoroutine(TypeText(sentence, dialogueText, 0.05f));
                }
                break;
        }
    }

    private IEnumerator TypeText(string sentence, TextMeshProUGUI textComponent, float delay)
    {
        textComponent.gameObject.SetActive(true);
        textComponent.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(delay);
        }

        // Add cursor sprite at end
        textComponent.text += "<sprite=15>";
        yield return new WaitForSeconds(delay);
    }
}
