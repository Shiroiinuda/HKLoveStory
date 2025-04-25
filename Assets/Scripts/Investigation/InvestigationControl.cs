using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyBox;
using SoundControl;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Investigation
{
    public class InvestigationControl : MonoSingleton<InvestigationControl>
    {
        [SerializeField] public GameControl gameControl;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private LogController logController;
        public TextMeshProUGUI sentenceText;
        public GameObject dialoguePanel;

        public List<string> currentSentences;
        public int currentSentenceIndex;
        public List<string> currentSpeakers;
        public List<AudioClip> currentClips;
        private bool isTyping = false;
        private string currentSentence = "";
        private string currentSpeaker = "";
        private Coroutine currentCoroutine = null;

        [SerializeField] private GameObject autoOnBtn;
        [SerializeField] private GameObject autoOffBtn;
        private bool autoPlay;

        [SerializeField] public GameObject logText;
        [SerializeField] public GameObject logContent;

        [SerializeField] public SkipButtonControl skipButtonControl; 
    
        public bool isEnd = false;

        public bool wonGame = false;
        public bool inGame = false;
        public GameObject endGameBlocker;
        public UnityEvent endDialogue;
        [SerializeField] private List<PuzzleGameControl> puzzleGameControlList;  // Reference to PuzzleGameControl
        public bool isShowing;

        public GameObject speakerBox;

        public TextMeshProUGUI speakerTmp;
        // Start is called before the first frame update

        void Start()
        {
            OnPanelActivate();
        }

        [ButtonMethod]
        void HAHA()
        {
// Load all sprites from the Resources/backgrounds/ChengWing2 folder
            Sprite[] chengWingSprites = Resources.LoadAll<Sprite>("Backgrounds/ChengWing2");
            // Put them into a HashSet for quick membership checks
            HashSet<Sprite> chengWingSpriteSet = new HashSet<Sprite>(chengWingSprites);

            // Retrieve all Image components from this GameObject and its children
            Image[] images = GetComponentsInChildren<Image>(true);
            
            foreach (Image img in images)
            {
                if (img.sprite == null)
                {
                    Debug.Log("GameObject '" + img.gameObject.name);
                    img.gameObject.AddComponent<ColorInHierarchy>();
                }
                if (img.sprite != null && chengWingSpriteSet.Contains(img.sprite))
                {
                    Debug.Log("GameObject '" + img.gameObject.name + 
                              "' uses sprite: " + img.sprite.name);
                }
            }
        }
        public void OnPanelActivate()
        {
            wonGame = false;
            endGameBlocker.SetActive(false);

            sentenceText.text = currentSentence;
            SetActiveChildGameObject(dialogueManager.currentDialogue.investivationLabel.Trim());
            if (dialogueManager.currentDialogue.investivationLabel.Trim() != "InternetInvestigation")
                skipButtonControl.CheckCanSkip();
            StartCoroutine(CheckAuto());
            GameControl.instance.isInvestigation = true;
            inGame = true;
            foreach (PuzzleGameControl puzzleGameControl in puzzleGameControlList)
                puzzleGameControl.SetPanelActive(true);
        }


        public void ShowSentences(List<string> sentences, [CanBeNull] List<string> Speaker = null)
        {
            endDialogue.RemoveAllListeners();
            currentSentences = sentences;
            currentSpeakers = Speaker;
            currentSentenceIndex = 0;
            dialoguePanel.SetActive(true);

            if (currentSentences != null && currentSentences.Count > 0)
            {
                dialoguePanel.SetActive(true);
                currentSentence = currentSentences[currentSentenceIndex];
        
                // Handle currentSpeaker safely
                currentSpeaker = (currentSpeakers != null && currentSentenceIndex < currentSpeakers.Count) 
                    ? currentSpeakers[currentSentenceIndex] 
                    : null;

                currentCoroutine = StartCoroutine(TypeSentence(currentSentence, currentSpeaker));
            }
            else
            {
                HideDialogue();
            }
        }

        private AudioClip currentClip;
        public void ShowSentences(List<string> sentences, [CanBeNull] List<string> Speaker = null,[CanBeNull] List<AudioClip> speakerClips = null)
        {
            endDialogue.RemoveAllListeners();
            currentSentences = sentences;
            currentSpeakers = Speaker;
            currentClips = speakerClips;
            currentSentenceIndex = 0;
            dialoguePanel.SetActive(true);

            if (currentSentences != null && currentSentences.Count > 0)
            {
                dialoguePanel.SetActive(true);
                currentSentence = currentSentences[currentSentenceIndex];
        
                // Handle currentSpeaker safely
                currentSpeaker = (currentSpeakers != null && currentSentenceIndex < currentSpeakers.Count) 
                    ? currentSpeakers[currentSentenceIndex] 
                    : null;
                currentClip = (currentClips != null && currentSentenceIndex < currentClips.Count) 
                    ? currentClips[currentSentenceIndex] 
                    : null;
                currentCoroutine = StartCoroutine(TypeSentence(currentSentence, currentSpeaker,currentClip));
            }
            else
            {
                HideDialogue();
            }
        }
        public void NextSentence()
        {
            if (isTyping)
            {
                isTyping = false;
                StopCoroutine(currentCoroutine);
                sentenceText.text = currentSentence += "<sprite=15>";
                
            }
            else
            {
                // Move on to the next sentence
                currentSentenceIndex++;

                if (currentSentenceIndex < currentSentences.Count)
                {
                    currentSentence = currentSentences[currentSentenceIndex];
                    // Handle currentSpeaker safely
                    currentSpeaker = (currentSpeakers != null && currentSentenceIndex < currentSpeakers.Count)
                        ? currentSpeakers[currentSentenceIndex]
                        : null;
                    currentClip = (currentClips != null && currentSentenceIndex < currentClips.Count) 
                        ? currentClips[currentSentenceIndex] 
                        : null;
                    if(currentCoroutine !=null)
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                    currentCoroutine = StartCoroutine(TypeSentence(currentSentence,currentSpeaker,currentClip));
                }
                else
                {
                    HideDialogue();
                }
            }
        }

        public void OnNextBtnClicked()
        {
            AutoOff();
            NextSentence();
        }

        private IEnumerator TypeSentence(string sentence,string? speaker = null)
        {
            Debug.Log(speaker);
   
            speakerBox.SetActive(!string.IsNullOrEmpty(speaker));
            speakerTmp.text = speaker;
            AddToLog();
            sentenceText.text = "";
            isTyping = true;

            foreach (char letter in sentence)
            {
                sentenceText.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            isTyping = false;
            sentenceText.text += "<sprite=15>";
        }
        private IEnumerator TypeSentence(string sentence,string? speaker = null, [CanBeNull] AudioClip clip = null)
        {
            Debug.Log(speaker);
            if(clip !=null)
             AudioManager.Instance.PlayCSound(clip);
            speakerBox.SetActive(!string.IsNullOrEmpty(speaker));
            speakerTmp.text = speaker;
            AddToLog();
            sentenceText.text = "";
            isTyping = true;
            sentenceText.text = "";
            foreach (char letter in sentence)
            {
                sentenceText.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            isTyping = false;
            sentenceText.text += "<sprite=15>";
        }
        private void HideDialogue()
        {
            endDialogue.Invoke();
            isShowing = false;
            AutoOff();
            currentSentences = null;
            currentSentenceIndex = 0;
            sentenceText.text = "";
            dialoguePanel.SetActive(false);

            if (isEnd)
            {
                StartCoroutine(EndInvestigationThatJumpItself());
            }
        }



        public void SetActiveChildGameObject(string LvName)
        {
            // Find the child GameObject with the provided name
            Transform childTransform = transform.Find(LvName);
            if (childTransform != null)
            {
                // Deactivate all other child GameObjects
                foreach (Transform child in transform)
                {
                    if (child != childTransform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                addFuPanel.SetActive(true);
                // Activate the child GameObject with the provided name
                childTransform.gameObject.SetActive(true);
            }
            else
            {
            }
        }

        public void AutoOn()
        {
            autoOnBtn.SetActive(false);
            autoOffBtn.SetActive(false);
            autoPlay = true;

            if (isTyping == false)
            {
                NextSentence();
            }
        }

        public void AutoOff()
        {
            autoOnBtn.SetActive(false);
            autoOffBtn.SetActive(false);
            autoPlay = false;
        }

        IEnumerator CheckAuto()
        {
            if (autoPlay)
            {
                AutoOn();
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(CheckAuto());
        }

        private void AddToLog()
        {
            GameObject newlogText = Instantiate(logText, logContent.transform);
            Debug.Log(newlogText);
            newlogText.GetComponent<TextMeshProUGUI>().text = currentSentences[currentSentenceIndex];
            logController.OnLogUpdate();
        }

        public IEnumerator EndInvestigation()
        {
            GameControl.instance.isInvestigation = false;
            wonGame = true;
            endGameBlocker.SetActive(true);

            AutoOff();

            dialogueManager.jumpMark = dialogueManager.currentDialogue.jumpMark;
            dialogueManager.Auto(false);
            isEnd = false;
            inGame = false;
            dialogueManager.OnButtonClick();

            if (dialogueManager.currentDialogue.fade)
            {
                yield return new WaitForSeconds(1f);
            }

            if (puzzleGameControlList.Count > 0)
            {
                foreach (PuzzleGameControl puzzleGameControl in puzzleGameControlList)
                    puzzleGameControl.SetPanelActive(false);
            }

            // Deactivate all child GameObjects
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            this.gameObject.SetActive(false);
        }

        public IEnumerator EndInvestigationThatJumpItself()
        {
            GameControl.instance.isInvestigation = false;
            wonGame = true;
            endGameBlocker.SetActive(true);
            inGame = false;
            AutoOff();

            dialogueManager.OnButtonClick();
            dialogueManager.Auto(false);
            isEnd = false;

            if (dialogueManager.currentDialogue.fade)
            {
                yield return new WaitForSeconds(1f);
            }

            if (puzzleGameControlList.Count > 0)
            {
                foreach (PuzzleGameControl puzzleGameControl in puzzleGameControlList)
                    puzzleGameControl.SetPanelActive(false);
            }

            // Deactivate all child GameObjects
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }

        public void BattleFail()
        {
            dialogueManager.InvestiationLocalize.SetTerm("battle_fail");
            SoundManager.PlaySfx("battle_fail");
            dialogueManager.startInvestigationAnim.SetTrigger("Start");
        }
        public void BattleWin()
        {
            dialogueManager.InvestiationLocalize.SetTerm("battle_end");
            SoundManager.PlaySfx("Battle_end");
            dialogueManager.startInvestigationAnim.SetTrigger("Start");
        }

        public void ShowSentences(List<string> sentence)
        {

            endDialogue.RemoveAllListeners();
            currentSentences = sentence;
            currentSentenceIndex = 0;
            dialoguePanel.SetActive(true);

            if (currentSentences != null && currentSentences.Count > 0)
            {
                dialoguePanel.SetActive(true);
                currentSentence = currentSentences[currentSentenceIndex];

                // Handle currentSpeaker safely
                currentSpeaker = (currentSpeakers != null && currentSentenceIndex < currentSpeakers.Count)
                    ? currentSpeakers[currentSentenceIndex]
                    : null;

                currentCoroutine = StartCoroutine(TypeSentence(currentSentence, currentSpeaker));
            }
            else
            {
                HideDialogue();
            }
        }

        [Header("For GetFu")]
        public GameObject addFuPanel;
        private const string addFuSound = "tearing";

        public void AddFu()
        {
            gameControl.GetFu();
            addFuPanel.GetComponent<Animator>().SetTrigger("Click");
            SoundControl.SoundManager.PlaySfx(addFuSound);
        }
    }
}
