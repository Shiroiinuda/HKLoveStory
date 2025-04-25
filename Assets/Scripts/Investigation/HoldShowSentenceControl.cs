using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Investigation;
using UnityEngine.EventSystems;
using I2.Loc;
using DG.Tweening;  // Add this to use DoTween
using SoundControl;

public class HoldShowSentenceControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Dependencies")]
    public GameObject displayPanel;
    public TextMeshProUGUI sentenceDisplay;
    public GameObject dialogueDataPrefab;
    public InvestigationControl investigationControl;
    public DialogueManager dialogueManager;
    public NormalInvestigationItems normalItem;
    public PuzzleGameControl puzzleGameControl;

    public int groupId; // Group ID to share progress among specific buttons

    public bool isObjNeedDisable;
    public List<GameObject> disableObjectList;

    [SerializeField] private Coroutine displayCoroutine;
    [SerializeField] private bool isPressing;
    [SerializeField] private CanvasGroup displayPanelCanvasGroup;

    // Static dictionary to store shared progress by group
    private static Dictionary<int, int> sharedJumpToByGroup = new Dictionary<int, int>();

    private void Start()
    {
        // Initialize shared progress for the group
        sharedJumpToByGroup[groupId] = normalItem.jumpTo;
        ResetSentence();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        displayPanelCanvasGroup.DOKill();
        isPressing = true;
        displayPanel.SetActive(true);
        Debug.Log("true");
        // Fade in the display panel
        displayPanelCanvasGroup.DOFade(1f, 0.5f);

        // Start displaying sentences if not already started
        if (displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplaySentencesCoroutine());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        displayPanelCanvasGroup.DOKill();
        ResetSentence();
        isPressing = false;
        
        // Stop SFX and Character Sound
        AudioManager.Instance.StopSfx();
        AudioManager.Instance.StopCSound();

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }

        // Fade out the display panel and disable it after fade completes
        displayPanelCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            if(!isPressing)
            displayPanel.SetActive(false);
            Debug.Log("false");
        });
    }

    private IEnumerator DisplaySentencesCoroutine()
    {
        while (isPressing)
        {

            int currentJumpTo = sharedJumpToByGroup[groupId];
            string sentence = LocalizationManager.GetTranslation($"Dialogue/{currentJumpTo}");
            sentenceDisplay.text = sentence;

            // Retrieve SFX and NPC Voice from the current dialogue data
            var dialogueData = dialogueDataPrefab.GetComponent<DialogueData>().dialogues[currentJumpTo];
            string voiceName = dialogueData.audioData.voiceName.Trim();
            string sfxName = dialogueData.audioData.sfxFile.Trim();

            AudioClip npcVoice = null;
            AudioClip sfx = null;

            if (!string.IsNullOrEmpty(voiceName))
            {
                string voicePath = dialogueData.audioData.voiceFile.Trim();

                npcVoice = SoundManager.GetCSound(voicePath, voiceName);

                if (npcVoice != null)
                {
                    SoundManager.PlayCSound(voicePath, voiceName);
                }
            }

            // Load SFX if available
            if (!string.IsNullOrEmpty(sfxName))
            {
                sfx = Resources.Load<AudioClip>($"Sounds/sfx/{sfxName}");
                SoundManager.PlaySfx(sfxName);
            }

            // Determine the longer duration
            float npcVoiceLength = npcVoice != null ? npcVoice.length : 1f;
            float sfxLength = sfx != null ? sfx.length : 0f;
            float waitTime = Mathf.Max(npcVoiceLength, sfxLength) + 1f;

            // If both are null, set a default wait time
            if (npcVoice == null && sfx == null)
            {
                waitTime = 1f;
            }

            yield return new WaitForSeconds(waitTime);

            sharedJumpToByGroup[groupId] += 1;

            int readyJumpMark = dialogueDataPrefab.GetComponent<DialogueData>().dialogues[currentJumpTo].jumpMark;

            if (readyJumpMark != -1)
            {
                if (puzzleGameControl != null)
                {
                    puzzleGameControl.isReturningFromDialogue = true;
                    puzzleGameControl.AddCompletedTask(normalItem.name);
                }
                displayPanel.SetActive(false);
                Debug.Log("false");

                if (dialogueDataPrefab.GetComponent<DialogueData>().dialogues[readyJumpMark].investivationLabel == "")
                {
                    dialogueManager.jumpMark = dialogueDataPrefab.GetComponent<DialogueData>().dialogues[currentJumpTo].jumpMark;
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
                else
                {
                    if (puzzleGameControl != null)
                        puzzleGameControl.CheckComplete();
                }

                if(disableObjectList.Count > 0)
                {
                    foreach (GameObject obj in disableObjectList)
                    {
                        obj.SetActive(false);
                    }
                }

                if (isObjNeedDisable)
                    this.gameObject.SetActive(false);

                // Initialize shared progress for the group
                sharedJumpToByGroup[groupId] = normalItem.jumpTo;
                ResetSentence();
            }
        }
    }

    private void ResetSentence()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }

        displayPanelCanvasGroup.alpha = 0;
        displayPanel.SetActive(false);
        Debug.Log("false");
        isPressing = false;

    }
}
