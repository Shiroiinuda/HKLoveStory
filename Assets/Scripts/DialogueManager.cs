using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasierLocalization;
using I2.Loc;
using Investigation;
using MyBox;
using SoundControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    private const string chapterPP = "CurrentChapter";
    [Separator("Current Dialogue")] [ReadOnly] [SerializeField]
    string currentSentence;

    [ReadOnly] [SerializeField] string currentSpeakerName;

    public DialogueData dataPrefabs;
    [Separator("Scripts")] public CharacterManager characterManager;
    public GameControl gameControl;
    public AudioManager audioManager;
    public ImgEffectControl imgEffectControl;
    public LogController logController;
    public InvestigationControl investigationControl;
    public InventoryManager inventoryManager;
    public DialogueText dialogueTextScript;
    [Space(10)] [Separator("DialogueUIs and Controls")]
    
    
    public GameObject speakerNameBox;
    

    public GameObject dialoguePanel;
    public GameObject settingButton;
    public GameObject inventoryButton;
    
    public PlayableDirector centerTextFadeIn;

    public GameObject bubbleDialoguePanel;
    public GameObject dialogueBubble;
    public List<RectTransform> bubbleSpawnPts;
    public TextMeshProUGUI bubbleText;
    public Animator dialogueBubbles_anim;
    // public Button bubbleContinueButton;

    public Button continueButton;
    public Queue<Dialogue> dialogueQueue;

    [Space(5)] private Coroutine displayLineCoroutine;
    private Coroutine currentlyDisplay;
    private bool isTyping;
    private bool isPaused;
    private bool isEndCSound;

    public Dialogue currentDialogue;

    private Dialogue previousDialogue;

    public Dialogue checkNextLvDialogue;

    private bool firstSentence;

    public int jumpMark = -1;

    [Space(10)] [Separator("Choices")] public GameObject choicePanel;
    [SerializeField] private Choices choices1;
    [SerializeField] private Choices choices2;
    [SerializeField] private Choices choices3;
    [SerializeField] private int loopChoiceJumpmark;
    public bool isChoiceLooping;
    public int choicesClicked;
    public int loopChoiceCounter;


    public GameObject logText;
    public GameObject logContent;

    public Button autoOffBtn;
    public Button autoOnBtn;
    public bool autoPlay;

    bool characterNeedFade = true;

    public int savedBookmark;

    [Space(10)] [Separator("Chapters and Stage Clear")]
    public PlayableDirector chapterTimeline;

    public GameObject chapterPanel;
    public TextMeshProUGUI chapterTextUI;
    public TextMeshProUGUI chapterNameUI;
    public GameObject stageClearPanel;
    public GameObject endClearPanel;
    public TextMeshProUGUI endNum;
    public TextMeshProUGUI endName;
    public GameObject nextLvPanel;
    public GameObject nextLvCost;

    public int storedID;

    public GameObject gamePanel;

    [Space(10)] [Separator("Skip Dialogue Stuff")]
    public Button skipDialogueBtn;

    private bool isSkipToEnd;
    private bool canPressSpace;

    [Space(10)] [Separator("Other Stuff")] public Animator startInvestigationAnim;
    public Localize InvestiationLocalize;
    public GameObject purchasePack;
    public GameObject commentBox;
    public GameObject gameOverPannel;
    public GameOverPanelControl gameOverPanelControl;

    public Animator propAnim;
    public Image propImg;

    public Animator dialogueVibrateAnim;

    public GameObject skipVideoButton;

    public string previousDialogueMode;
    private Coroutine autoCoroutine;

    private void Start()
    {
        if (dataPrefabs == null && Resources.Load<DialogueData>("DialogueDataPrefab"))
            dataPrefabs = Resources.Load<DialogueData>("DialogueDataPrefab");

        previousDialogueMode = "";

        firstSentence = true;
        //TODO get the data in prefab
        dialogueQueue = new Queue<Dialogue>();

        if (gameControl.bgm != "")
        {
            SoundManager.PlayBgm(gameControl.bgm.Trim());
        }


        savedBookmark = gameControl.currentBookmark;
        isChoiceLooping = gameControl.isChoiceLoop;
        loopChoiceCounter = gameControl.loopChoiceCounter;
        loopChoiceJumpmark = gameControl.loopChoiceJumpmark;

        isSkipToEnd = false;
        canPressSpace = true;
        characterManager.DefaultDisableCharacters();
        PrepareDialogue();
        StartDialogue();
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if(continueButton.interactable && continueButton.isActiveAndEnabled)
                continueButton.onClick.Invoke();
        }
    }

    public void PrepareDialogue()
    {
        foreach (Dialogue dialogue in dataPrefabs.dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }

        if (savedBookmark > 0)
        {
            foreach (Dialogue dialogue in dialogueQueue)
            {
                //                Debug.Log("dialogue.bookMark = " + dialogue.bookMark);

                if (dialogue.bookMark == savedBookmark - 1)
                {
                    // Clear the jumpMark to prevent further jumps.
                    jumpMark = -1;
                    DequeueDialoguesUntil(dialogue);
                    return;
                }
            }
        }

        Debug.Log(currentDialogue.bookMark);

        dialogueTextScript.LoadDialogue(currentDialogue);
    }

    private void StartDialogue()
    {
        BackgroundManager.Instance.ShowImage(currentDialogue);

        DisplayNextSentence();
        /*if(autoCoroutine == null)
        autoCoroutine= StartCoroutine(CheckAuto());*/
    }

    private void EnableSkipAndAuto()
    {
    }

    private void NextDialogue()
    {
        
        var callFunc1 = currentDialogue.callFuncs.func1.Trim();
        var peekFunc1 = dialogueQueue.Peek().callFuncs.func1.Trim();

        if (currentDialogue.chapterState < 0 || currentDialogue.mode != "" || currentDialogue.speakerID >= 997 ||
            dialogueQueue.Peek().speakerID >= 997 || dialogueQueue.Peek().mode != "" || previousDialogue.mode != "" ||
            previousDialogue.speakerID >= 997 ||
            callFunc1 == "CheckTrueEnd" ||
            peekFunc1 == "CheckTrueEnd" ||
            callFunc1 == "LastEndDetermination" ||
            peekFunc1 == "LastEndDetermination" || dialogueQueue.Count == 0 ||
            isChoiceLooping)
        {
            skipDialogueBtn.interactable = false;
            autoOffBtn.interactable = false;
        }
        else
        {
            skipDialogueBtn.interactable = true;
            autoOffBtn.interactable = true;
        }

        CheckSkipDialogueEnable();

        /*if (currentDialogue.itemModify.addItem != "")
        {
            InventoryItems inventoryItems =
                Resources.Load<InventoryItems>($"AddItemInventoryItems/{currentDialogue.itemModify.addItem.Trim()}");
            if (inventoryItems != null)
            {
                if (inventoryManager.HasItem(inventoryItems) == false)
                {
                    inventoryManager.AddItem(inventoryItems);
                }

                if (currentDialogue.fade)
                    StartCoroutine(CountTimeandGetItem(inventoryItems));
                else
                    inventoryManager.SetGetItemPenel(inventoryItems);
            }
        }*/

        if (currentDialogue.itemModify.delItem != "")
        {
            InventoryItems inventoryItems =
                Resources.Load<InventoryItems>(
                    $"{currentDialogue.itemModify.delItemPath}/{currentDialogue.itemModify.delItem.Trim()}");

            if (inventoryItems != null)
            {
                if (inventoryManager.HasItem(inventoryItems))
                {
                    inventoryManager.RemoveItem(inventoryItems);
                }
            }
        }

        //Play Sounds
        PlaySounds();
        //Dialogue Active
        DialogueBoxState(currentDialogue.dialogueBoxOnOff);
        //Start Game
        if (currentDialogue.mode != "" && currentDialogue.mode != "InGame")
        {
            if (gameControl.prevousGameName != currentDialogue.investivationLabel)
            {
                gameControl.prevousGameName = currentDialogue.investivationLabel;
                gameControl.replayCount = 0;
            }

            UpdateCharacters();
            Debug.Log("CALL FUNC1");
            /*if (currentDialogue.fade)
                FadeInOut.Instance.fadeAction.AddListener(CallFunctions);
            else*/

            if (!(currentDialogue.fade))
            {
                CallFunctions();
            }


            ContinueButtonState(false);
            Auto(false);

            if (!currentDialogue.fade)
                ShowDialogueVisuals();
            gamePanel.SetActive(true);
            InvestigationControl.Instance.OnPanelActivate();
        }
        else
        {
            ContinueButtonState(true);
            UpdateCharacters();

            if (currentDialogue.chapterState == 1)
            {
                StartCoroutine(ShowChapter());
            }
            else if (currentDialogue.chapterState == -1 || currentDialogue.chapterState == -2)
            {
                ContinueButtonState(false);
                StartCoroutine(ShowStageClear());
                DialogueBoxState(false);
                return;
            }
            Debug.Log("BB");
            if (currentDialogue.speakerID >= 0 && currentDialogue.speakerID <= 996)
            {
                Debug.Log("CC");
                speakerNameBox.SetActive(true);
                  if (currentDialogue.speakerID == 800 || currentDialogue.speakerID == 900)
                  {
                      speakerNameBox.SetActive(false);
                  }
                // if (!currentDialogue.fade)
                // {
                //     ShowDialogueVisuals();
                // }
//                Debug.Log(Localization.GetString($"Speaker/{currentDialogue.speakerName}"));
                /*speakerNameText.text = Localization.GetString($"Speaker/{currentDialogue.speakerName}");
                StartTyping(Localization.GetString($"Dialogue/{currentDialogue.bookMark}"));*/
                dialogueTextScript.LoadDialogue(currentDialogue);
                AddToLog();
                HandleVibration();
                Debug.Log("DD");
                if (isChoiceLooping)
                {
                    if (currentDialogue.choicesData.isLoop == "loopjump")
                    {
                        jumpMark = loopChoiceJumpmark;
                    }
                }
                else
                {
                    jumpMark = currentDialogue.jumpMark;
                }
                Debug.Log("EE");
                Debug.Log("CALL FUNC2");

                /*if (currentDialogue.fade)
                    FadeInOut.Instance.fadeAction.AddListener(CallFunctions);
                else*/
                if (!(currentDialogue.fade))
                {
                    CallFunctions();
        
                }
            }
            else if (currentDialogue.speakerID >= 997 && currentDialogue.speakerID <= 999)
            {
                DialogueBoxState(true);

                /*speakerNameText.text = Localization.GetString($"Speaker/{currentDialogue.speakerName}");
                StartTyping(Localization.GetString($"Dialogue/{currentDialogue.bookMark}"));*/
                dialogueTextScript.SkipTyping(previousDialogue);
                ShowChoices();
                /*if (firstSentence)
                {
                    if (previousDialogueMode != "DialogueCloud")
                    {
                        if (previousDialogue.speakerID == 800 || previousDialogue.speakerID == 900)
                            speakerNameText.text = "";
                        else if (previousDialogue.speakerID >= 0 && previousDialogue.speakerID <= 996)
                            speakerNameText.text = Localization.GetString($"Speaker/{currentDialogue.speakerName}");
                        ;


                        if (previousDialogue.speakerID == 800)
                            narratorText.text = Localization.GetString($"Dialogue/{currentDialogue.bookMark}");
                        // LocalizeStringUpdate(narratorLocalizeString, currentDialogue.bookMark.ToString(),
                        //     "ChengWIngDialogue");
                        // Debug.Log("test");
                        else if (previousDialogue.speakerID == 900)
                        {
                            centerText.text = Localization.GetString($"Dialogue/{currentDialogue.bookMark}");
                        }
                        else if (previousDialogue.speakerID >= 0 && previousDialogue.speakerID <= 996)
                        {
                            Debug.Log("A");
                            dialogueText.text = Localization.GetString($"Dialogue/{currentDialogue.bookMark}");
                        }
                    }
                }*/
            }

            firstSentence = false;

            if (isChoiceLooping)
            {
                FBPP.SetString(chapterPP,currentDialogue.saveChapter);
                gameControl.currentBookmark = loopChoiceJumpmark - 1;
            }
            else
            {
                if (currentDialogue.mode == "")
                {
                    if (currentDialogue.speakerID >= 0 && currentDialogue.speakerID <= 996 && currentDialogue.callFuncs.func1 != "GameOverPanel()")
                    {
                        FBPP.SetString(chapterPP,currentDialogue.saveChapter);
                        gameControl.currentBookmark = currentDialogue.bookMark;

                        if (!gameControl.storedBookmark.Contains(currentDialogue.bookMark))
                            gameControl.storedBookmark.Add(currentDialogue.bookMark);
                    }
                }
            }

            gameControl.reopen = false;
            gameControl.isChoiceLoop = isChoiceLooping;
            gameControl.loopChoiceCounter = loopChoiceCounter;
            gameControl.loopChoiceJumpmark = loopChoiceJumpmark;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public async Task DisplayNextSentence()
    {
        /*Debug.Log($"HEHE {IsNormalDialogue()}");
        autoOffBtn.interactable = IsNormalDialogue();*/
        audioManager.StopCSound();
//        Debug.LogError($"This Sentence{currentDialogue.bookMark}");
        skipVideoButton.SetActive(false);

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Jump to next Dialogue
        CheckJump();

        if (!currentDialogue.dialogueBoxOnOff || currentDialogue.fade)
            DialogueBoxState(false);
        //Fade
        if (currentDialogue.fade)
        {
            ContinueButtonStates(false);
            Debug.Log("Fade");
            FadeInOut.Instance.fadeOutAction.AddListener(NextDialogue);
            FadeInOut.Instance.fadeOutAction.AddListener(() => ContinueButtonStates(true));
            FadeInOut.Instance.fadeOutAction.AddListener(() => continueButton.enabled = true);
            FadeInOut.Instance.fadeAction.AddListener(ShowDialogueVisuals);
            FadeInOut.Instance.fadeAction.AddListener(CallFunctions);
            
            await FadeInOut.Instance.StartFadeSequence();
            return;
        }

        if (!currentDialogue.fade)
            NextDialogue();
    }

    private void ContinueButtonStates(bool onOff) => continueButton.gameObject.SetActive(onOff);

    private void DialogueBoxState(bool TurnOn)
    {
        if (currentDialogue.mode == "DialogueCloud")
        {
            bubbleDialoguePanel.SetActive(TurnOn);
            dialoguePanel.SetActive(!TurnOn); // Ensure the other panel is deactivated
        }
        else
        {
            bubbleDialoguePanel.SetActive(!TurnOn); // Ensure the other panel is deactivated
            dialoguePanel.SetActive(TurnOn);
        }

        settingButton.SetActive(TurnOn);
        inventoryButton.SetActive(TurnOn);
    }

    public void PlaySounds(Dialogue nowDialogue)
    {
        //bgm
        var tmpBgm = nowDialogue.audioData.bgm.Trim();
        if (!string.IsNullOrEmpty(tmpBgm) && tmpBgm != "Stop")
        {
            SoundManager.PlayBgm(tmpBgm);

            gameControl.bgm = tmpBgm;
        }

        if (tmpBgm == "Stop")
        {
            audioManager.StopBGM();
        }

        if (string.IsNullOrEmpty(tmpBgm))
        {
            if (gameControl.reopen)
            {
                if (gameControl.bgm != "")
                {
                    AudioClip bgm = Resources.Load<AudioClip>($"Sounds/BGM/{gameControl.bgm.Trim()}");
                    if (bgm != null)
                    {
                        audioManager.PlayMusic(bgm);
                    }
                }
            }
        }

        var tmpsfx = nowDialogue.audioData.sfxFile.Trim();
        //sfx
        if (tmpsfx != "")
        {
            AudioClip sfx = Resources.Load<AudioClip>($"Sounds/sfx/{tmpsfx}");
            if (sfx != null)
            {
                audioManager.PlaySFX(sfx);
            }
        }

        //NPCVoices
        var tmpvoice = nowDialogue.audioData.voiceName.Trim();
        if (!string.IsNullOrEmpty(tmpvoice))
        {
            string soundFileName = nowDialogue.audioData.voiceFile.Trim();

            AudioClip npcVoice =
                SoundManager.GetCSound(soundFileName, tmpvoice);
            if (npcVoice != null)
            {
                SoundManager.PlayCSound(soundFileName, tmpvoice);
                isEndCSound = false;
            }
            else
            {
                audioManager.StopCSound();
            }
        }
        else
            audioManager.StopCSound();
    }


    public void PlaySounds()
    {
        //bgm
        var tmpBgm = currentDialogue.audioData.bgm.Trim();
        if (!string.IsNullOrEmpty(tmpBgm) && tmpBgm != "Stop")
        {
            SoundManager.PlayBgm(tmpBgm);

            gameControl.bgm = tmpBgm;
        }

        if (tmpBgm == "Stop")
        {
            audioManager.StopBGM();
        }

        if (string.IsNullOrEmpty(tmpBgm))
        {
            if (gameControl.reopen)
            {
                if (gameControl.bgm != "")
                {
                    SoundManager.PlayBgm(gameControl.bgm.Trim());
                }
            }
        }

        var tmpsfx = currentDialogue.audioData.sfxFile.Trim();
        //sfx
        if (tmpsfx != "")
        {
            AudioClip sfx = Resources.Load<AudioClip>($"Sounds/sfx/{tmpsfx}");
            if (sfx != null)
            {
                audioManager.PlaySFX(sfx);
            }
        }

        Debug.Log(Localization.NpcVoiceLan());
        //NPCVoices
        var tmpvoice = currentDialogue.audioData.voiceName.Trim();
        if (!string.IsNullOrEmpty(tmpvoice))
        {
            string soundFileName = currentDialogue.audioData.voiceFile.Trim();
            AudioClip npcVoice =
                SoundManager.GetCSound(soundFileName, tmpvoice);
            if (npcVoice != null)
            {
                audioManager.PlayCSound(npcVoice);
                isEndCSound = false;
            }
        }
        else
            audioManager.StopCSound();
    }

    private void UpdateCharacters()
    {
        if (firstSentence)
        {
            ShowCharacterImage();

            characterNeedFade = true;

            if (currentDialogue.dialogueMode == "DialogueCloud")
            {
                dialoguePanel.SetActive(false);
                /*if (previousDialogueMode == "SmallIconBox")
                    smallIconTexture.SetActive(false);*/

                ShowDialogueCloud();
            }
        }
        else
        {
            if (currentDialogue.speakerID < 997) // Normal Mode
            {
                if (currentDialogue.dialogueMode == "Default")
                {
                    //Debug.Log("A");
                    if (previousDialogueMode != "Default")
                    {
                        //Debug.Log("B");
                        if (previousDialogueMode == "DialogueCloud")
                        {
                            dialoguePanel.SetActive(true);
                            dialogueBubbles_anim.SetTrigger("FadeOut");
                            bubbleDialoguePanel.SetActive(false);
                            characterNeedFade = true;
                        }
                        /*if (previousDialogueMode == "SmallIconBox")
                        {
                            smallIconTexture.SetActive(false);
                        }*/
                    }
                    else
                    {
                        //Debug.Log("D");
                        if (currentDialogue.speakerID > 0 && currentDialogue.speakerID <= 800)
                        {
                            if (previousDialogue.speakerID == currentDialogue.speakerID)
                            {
                                if (previousDialogue.speakerPosition == "NoShow")
                                {
                                    characterNeedFade = true;
                                }
                                else
                                {
                                    characterNeedFade = false;
                                }
                            }
                            else
                            {
                                //characterManager.DefaultDisableCharacters();
                                characterNeedFade = true;
                            }
                        }
                        else
                            characterNeedFade = false;
                    }
                }
                else
                {
                    //Debug.Log("B");
                    if (previousDialogueMode == "Default")
                        Debug.Log("LastDefault");
                    if (currentDialogue.dialogueMode == "SmallIconBox")
                    {
                        if (previousDialogueMode == "DialogueCloud")
                        {
                            dialoguePanel.SetActive(true);
                            dialogueBubbles_anim.SetTrigger("FadeOut");
                            bubbleDialoguePanel.SetActive(false);
                        }
                    }
                    else if (currentDialogue.dialogueMode == "DialogueCloud")
                    {
                        dialoguePanel.SetActive(false);
                        /*if (previousDialogueMode == "SmallIconBox")
                            smallIconTexture.SetActive(false);*/

                        ShowDialogueCloud();
                    }
                }
            }
            else if (currentDialogue.speakerID >= 997 && currentDialogue.speakerID <= 999) // Choice Mode
            {
                characterNeedFade = false;
                storedID = previousDialogue.speakerID;
            }
        }
    }


    private void ShowDialogueVisuals()
    {
        continueButton.enabled = !currentDialogue.fade;
        // bubbleContinueButton.enabled = !currentDialogue.fade;

        if (currentDialogue.imgEffect != previousDialogue.imgEffect || currentDialogue.imgEffect == "")
        {
            imgEffectControl.DeactivateImgEffect();
        }

        imgEffectControl.DeactivateAnimation();

        // SetImageSize();
        BackgroundManager.Instance.ShowImage(currentDialogue);
        ShowCharacterImage();
    }




    private void ShowCharacterImage()
    {
//        Debug.Log($"{currentDialogue.speakerPosition},{previousDialogue.speakerPosition},{currentDialogue.dialogueMode}");
        if (currentDialogue.dialogueMode == "Default")
        {
            // defaultCharacterTexture.SetActive(true);
            // smallIconTexture.SetActive(false);
            if (currentDialogue.speakerID > 0 && currentDialogue.speakerID <= 800)
            {
                if (currentDialogue.fade)
                    characterManager.DefaultDisableCharacters();

                if (currentDialogue.speakerPosition != "NoShow")
                {
//                    Debug.LogWarning("ShowActive");
                    characterManager.DefaultActivateCharacter(currentDialogue.speakerID,
                        currentDialogue.character.body,
                        currentDialogue.character.head, currentDialogue.character.expression, characterNeedFade,
                        currentDialogue.speakerPosition);
                }
                else
                { 
                    //                    Debug.Log("Noshow");
                    if (currentDialogue.speakerPosition == "NoShow")
                        characterManager.DefaultDisableCharacters();
                }
            }
            else
            {
                if (currentDialogue.bookMark <= 0 || firstSentence)
                {
                    characterManager.DefaultDisableCharacters();
                    return;
                }

                if (previousDialogue.speakerID > 0 && previousDialogue.speakerID <= 800)
                {
                    characterManager.DefaultDisableCharacters();
                }
            }
        }
        else if (currentDialogue.dialogueMode == "SmallIconBox")
        {
            Debug.Log(currentDialogue.speakerID);
            characterManager.SmallIconBoxActivateCharacter(currentDialogue.speakerID, currentDialogue.character.body,
                currentDialogue.character.head, currentDialogue.character.expression);
        }
    }


    private IEnumerator CheckPortraitFadeOutEnd()
    {
        if (characterManager.textureAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            characterManager.DefaultActivateCharacter(currentDialogue.speakerID, currentDialogue.character.body,
                currentDialogue.character.head, currentDialogue.character.expression, characterNeedFade,
                currentDialogue.speakerPosition);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(CheckPortraitFadeOutEnd());
        }
    }

    private void AddToLog()
    {
        currentSpeakerName = Localization.GetString($"Speaker/{currentDialogue.speakerName}");

        if (!string.IsNullOrEmpty(currentSentence) && currentDialogue.dialogueBoxOnOff)
        {
            GameObject newlogText = Instantiate(logText, logContent.transform);
            newlogText.GetComponent<TextMeshProUGUI>().text = currentSpeakerName + ": " + currentSentence;
            logController.OnLogUpdate();
        }
    }

    private void HandleVibration()
    {
        if (currentDialogue.isVibrate)
        {
#if !UNITY_STANDALONE
            Handheld.Vibrate();
#endif
            dialogueVibrateAnim.SetTrigger("Vibrate");
        }
    }
    

    /*private void StartTyping(string dialogueString)
    {
        currentSentence = dialogueString;
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
            //  Debug.Log("Still have DisplayLine");
        }

        //    Debug.Log(dialogueText.text);
        dialogueText.text = "";
        narratorText.text = "";
        centerText.text = "";

        bubbleText.text = "";
        dialogueText.gameObject.SetActive(false);
        narratorText.gameObject.SetActive(false);
        centerText.gameObject.SetActive(false);
        bubbleText.gameObject.SetActive(false);

        displayLineCoroutine = StartCoroutine(TypeSentence(dialogueString));
    }*/

    
    /*IEnumerator TypeSentence(string sentence)
    {
        //if (currentDialogue.fade)
        //yield return new WaitForSeconds(1);

        isTyping = true;
        switch (currentDialogue.mode)
        {
            case "DialogueCloud":
                yield return TypeText($"{sentence}<sprite=15>", bubbleText, 0.1f);
                if (isPaused)
                {
                    isPaused = false;
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    continueButton.gameObject.SetActive(!(currentDialogue.speakerID >= 997));
                    bubbleContinueButton.gameObject.SetActive(!(currentDialogue.speakerID >= 997));
                }

                break;
            default:
                yield return TypeDefaultDialogue(sentence, currentDialogue);
                isTyping = false;

                break;
        }

        //     Debug.Log("TypeA");
        yield return new WaitForSeconds(3f);
        isTyping = false;
        // Debug.Log("TypeB");
        if (autoPlay) autoCoroutine ??= StartCoroutine(CheckAuto());
    }

    private Coroutine typingText;

    IEnumerator TypeDefaultDialogue(string sentence, Dialogue dialogue)
    {
//        Debug.Log("Typing");
        if (typingText != null)
        {
            StopCoroutine(typingText);
            typingText = null;
        }

        switch (dialogue.speakerID)
        {
            case 800:
                yield return currentlyDisplay =
                    typingText = StartCoroutine(TypeText($"{sentence}", narratorText, 0.05f));
                break;

            case 900:
                typingText = StartCoroutine(TypeText($"{sentence}", centerText, 0f));
                /*centerText.text = sentence;#1#

                centerTextFadeIn.Play();
                break;

            case var id when id >= 0 && id <= 996:
                if (!string.IsNullOrEmpty(currentSentence))
                    yield return currentlyDisplay =
                        typingText = StartCoroutine(TypeText($"{sentence}", dialogueText, 0.05f));
                break;
            case var id when id >= 997:
                // DialogueBoxState(false);
                dialogueText.text = LocalizationManager.GetTranslation($"Dialogue/{currentDialogue.bookMark}");
                // LocalizeStringUpdate(dialogueLocalizeString, previousDialogue.bookMark.ToString());
                dialogueText.text += sentence;
                break;
        }
    }

    IEnumerator TypeText(string sentence, TextMeshProUGUI textComponent, float delay)
    {
        //    Debug.Log(sentence);
        textComponent.gameObject.SetActive(true);
        textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(delay);
        }
        if(currentDialogue.speakerID != 900)
            textComponent.text += "<sprite=15>";
        yield return new WaitForSeconds(delay);
    }*/

    private void ShowChoices()
    {
        int firstChoiceBookmark = currentDialogue.bookMark;
        bool checkLoop = isChoiceLooping == false && currentDialogue.choicesData.isLoop == "Loop";

        switch (currentDialogue.speakerID)
        {
            case 997:
                // Display choice 1 and set it to jump to dialogue ID 10

                ContinueButtonStates(false);

                //Show choicePanel and 997ChoiceBox
                choicePanel.SetActive(true);
                Choice(choices1);

                if (checkLoop)
                    loopChoiceCounter++;

                if (dialogueQueue.Peek().speakerID == 998)
                {
                    currentDialogue = dialogueQueue.Dequeue(); // remove the dialogue with ID 998 from the queue

                    Choice(choices2);

                    if (checkLoop)
                        loopChoiceCounter++;
                }

                if (dialogueQueue.Peek().speakerID == 999)
                {
                    currentDialogue = dialogueQueue.Dequeue(); // remove the dialogue with ID 999 from the queue

                    Choice(choices3);

                    if (checkLoop)
                        loopChoiceCounter++;
                }

                break;

            case 998:
                // Display choice 1 and set it to jump to dialogue ID 10

                continueButton.gameObject.SetActive(false);
                // bubbleContinueButton.gameObject.SetActive(false);

                //Show choicePanel and 998ChoiceBox
                choicePanel.SetActive(true);
                Choice(choices2);

                if (checkLoop)
                    loopChoiceCounter++;

                //Show999ChoiceBox
                if (dialogueQueue.Peek().speakerID == 999)
                {
                    currentDialogue = dialogueQueue.Dequeue(); // remove the dialogue with ID 999 from the queue

                    Choice(choices3);
                    if (checkLoop)
                        loopChoiceCounter++;
                }

                break;

            case 999:
                // Display choice 1 and set it to jump to dialogue ID 10

                continueButton.gameObject.SetActive(false);
                // bubbleContinueButton.gameObject.SetActive(false);

                //Show choicePanel and 997ChoiceBox
                choicePanel.SetActive(true);
                Choice(choices3);

                if (checkLoop)
                    loopChoiceCounter++;

                break;
        }

        if (isChoiceLooping == false)
        {
            if (currentDialogue.choicesData.isLoop == "loop")
            {
                isChoiceLooping = true;
                loopChoiceJumpmark = firstChoiceBookmark;
            }
        }

        JumpToBookmarkInDialogueQueue(firstChoiceBookmark);

        void Choice(Choices choice)
        {
            choice.gameObject.SetActive(true);
            choice.jumpMark = currentDialogue.jumpMark;
            choice.choiceMode = currentDialogue.choicesData.isLoop;
            choice.UpdateLocalize(currentDialogue.bookMark.ToString());
        }
    }


    public void EndDialogue()
    {
        // speakerNameText.text = "";
        // dialogueText.text = "";
        // narratorText.text = "";
        // centerText.text = "";

        bubbleText.text = "";

        ContinueButtonState(false);
    }

    public void ContinueButtonState(bool state)
    {
        Debug.Log($"ContinueButtonState{state}");
        continueButton.enabled = state;
        // bubbleContinueButton.enabled = state;
    }

    public bool investDialogue;

    public void OnButtonClick()
    {
        Auto(false);
        if (dialogueTextScript.IsTyping)
        {
            isPaused = true;
            dialogueTextScript.SkipTyping();
        }
        else
        {
            ContinueButtonState(false);
            Debug.Log($"{!gameControl.isInvestigation} || {!investDialogue}");
            if (!gameControl.isInvestigation && !investDialogue && !investigationControl.inGame)
                DisplayNextSentence();
        }
    }

    private void CheckJump()
    {
        if (isSkipToEnd)
            return;

        if (currentDialogue.speakerID < 997 && !firstSentence)
            previousDialogueMode = currentDialogue.dialogueMode;

        previousDialogue = currentDialogue;

        if (jumpMark == -1)
        {
            currentDialogue = dialogueQueue.Dequeue();
        }
        else if (jumpMark > -1)
        {
            JumpToBookmarkInDialogueQueue(jumpMark);
        }
    }

    private void JumpToBookmarkInDialogueQueue(int bookmark)
    {
        if (bookmark < 0)
        {
            Debug.LogWarning("Invalid bookmark. Cannot jump to a negative bookmark.");
            return;
        }

        if (bookmark < currentDialogue.bookMark)
        {
            dialogueQueue.Clear();

            for (int i = 0; i < dataPrefabs.dialogues.Count; i++)
            {
                if (dataPrefabs.dialogues[i].bookMark >= bookmark)
                {
                    dialogueQueue.Enqueue(dataPrefabs.dialogues[i]);
                }
            }

            if (dialogueQueue.Count > 0)
            {
                currentDialogue = dialogueQueue.Dequeue();
            }
            else
            {
                Debug.LogWarning("No dialogues available after the specified bookmark.");
            }

            return;
        }

        foreach (var dialogue in dialogueQueue)
        {
            if (dialogue.bookMark == bookmark)
            {
                jumpMark = -1;
                DequeueDialoguesUntil(dialogue);
                return;
            }
        }

        Debug.LogWarning("Bookmark not found in the current dialogue queue.");
    }

    private void DequeueDialoguesUntil(Dialogue targetDialogue)
    {
        while (dialogueQueue.Count > 0 && !dialogueQueue.Peek().Equals(targetDialogue))
        {
            currentDialogue = dialogueQueue.Dequeue();
        }

        if (dialogueQueue.Count > 0 && dialogueQueue.Peek().Equals(targetDialogue))
        {
            currentDialogue = dialogueQueue.Dequeue();
        }
        else
        {
            Debug.LogWarning("Target dialogue not found during dequeue operation.");
        }
    }

    IEnumerator ShowChapter()
    {
        // chapterNameLocalizeString.StringReference.TableEntryReference = currentDialogue.saveChapter;
        // chapterLocalizeString.StringReference.TableEntryReference = currentDialogue.saveChapter;
        chapterTextUI.text = Localization.GetString($"CR_Chap/{currentDialogue.saveChapter}");
        chapterNameUI.text = Localization.GetString($"CR_ChapName/{currentDialogue.saveChapter}");
        ;

        if (currentDialogue.fade)
        {
            yield return new WaitForSeconds(1.3f);
        }

        chapterPanel.SetActive(true);

        chapterTimeline.Play();
    }


    //Check the next lv chapter
    private void CheckNextLv(int bookmark)
    {
        // Dequeue all dialogues until the target dialogue is at the top of the queue.
        foreach (Dialogue dialogue in dialogueQueue)
        {
            if (dialogue.bookMark == bookmark)
            {
                // Clear the jumpMark to prevent further jumps.
                jumpMark = -1;

                while (!dialogueQueue.Peek().Equals(dialogue))
                {
                    checkNextLvDialogue = dialogueQueue.Dequeue();
                }

                // Dequeue the target dialogue.
                checkNextLvDialogue = dialogueQueue.Dequeue();

                return;
            }
        }
    }

    IEnumerator ShowStageClear()
    {
        // fadeAnim.Play();
        //SetFadeRaycast();
        FadeInOut.Instance.StartFadeSequence();
        continueButton.gameObject.SetActive(false);
        //Check the next lv chapter
        CheckNextLv(currentDialogue.nextLv);

        if (dialogueQueue.Count == 0 || currentDialogue.nextLv == -1)
        {
            nextLvPanel.SetActive(false);
            nextLvCost.SetActive(false);
        }
        else
        {
            nextLvPanel.SetActive(true);

            // If a checkSavePt object with bookmark was found, disable btn
            if (gameControl.unLockedSavept.Contains(checkNextLvDialogue.saveChapter))
            {
                nextLvCost.SetActive(false);
            }
            else
            {
                nextLvCost.SetActive(true);
            }
        }

        yield return new WaitForSeconds(1);
    // SoundManager.PlaySfx("stageclear");
        switch (currentDialogue.chapterState)
        {
            case -1:
                stageClearPanel.SetActive(true);
                break;
            case -2:
                gameControl.OnUnlockAchievement(currentDialogue.saveChapter.Trim());
                Debug.Log("End Stage");
                endNum.text = Localization.GetString($"CR_Chap/{currentDialogue.saveChapter}");
                endName.text = Localization.GetString($"CR_ChapName/{currentDialogue.saveChapter}");
                endClearPanel.SetActive(true);
                break;
        }

        if (!gameControl.stageClear.Contains(currentDialogue.saveChapter))
        {
            gameControl.stageClear.Add(currentDialogue.saveChapter);
        }

        isSkipToEnd = false;
    }

    public void Auto(bool isOn)
    {
        Debug.Log($"Auto: isTyping {isTyping}");


        /*autoPlay = isOn;*/
        autoOnBtn.gameObject.SetActive(false);
        autoOffBtn.gameObject.SetActive(false);
        return;
        if (!isOn)
        {
            if (autoCoroutine != null)
                StopCoroutine(autoCoroutine);

        }
        Debug.Log(isOn);
        //Debug.Log($"{isOn}, {isTyping}, {!audioManager.CSoundSource.isPlaying}");
        if (!isOn) return;
        Debug.Log(IsNormalDialogue());
        if (IsNormalDialogue())
        {
            autoCoroutine ??= StartCoroutine(CheckAuto());
        }
        else
        {
            autoOnBtn.gameObject.SetActive(false);
            autoOffBtn.gameObject.SetActive(true);
            Auto(false);
        }


    }

    private bool IsNormalDialogue()
    {
     var   nextdialogues =  dialogueQueue.Peek();
     Debug.Log($"Auto 997,800,900:{(nextdialogues.speakerID < 997 || nextdialogues.speakerID == 800 || nextdialogues.speakerID == 900)}");
     Debug.Log($"Auto item: {nextdialogues.itemModify.addItem == ""}");
     Debug.Log($"Auto ChapterState{nextdialogues.chapterState == 0}");
     Debug.Log($"Auto Fade: {!nextdialogues.fade}");
     Debug.Log($"Auto Animation{!String.IsNullOrWhiteSpace(nextdialogues.animation) && !String.IsNullOrWhiteSpace(currentDialogue.animation)}");
     
        return ((nextdialogues.speakerID < 997 || nextdialogues.speakerID == 800 ||
                 nextdialogues.speakerID == 900) && nextdialogues.itemModify.addItem == "" &&
                nextdialogues.chapterState == 0 && !currentDialogue.fade /*&& !nextdialogues.fade && string.IsNullOrWhiteSpace(nextdialogues.animation) && string.IsNullOrWhiteSpace(currentDialogue.animation)*/);
    }

    public IEnumerator DisplayNext()
    {
        bool waiting = true;
        while (waiting)
        {
            isEndCSound = !audioManager.CSoundSource.isPlaying;


            if (!isTyping && isEndCSound)
            {
                waiting = false;
                yield return DisplayNextSentence();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CheckAuto()
    {
            Debug.Log(autoPlay);
        while (autoPlay) // Keep the coroutine running indefinitely
        {
            // Update isEndCSound based on whether the sound is playing
            isEndCSound = !audioManager.CSoundSource.isPlaying;

             Debug.Log(FadeInOut.Instance.isFading);
            if (isEndCSound && !isTyping)
            {
                if (!isTyping && !audioManager.CSoundSource.isPlaying &&continueButton.interactable && continueButton.isActiveAndEnabled)
                    if (!IsNormalDialogue())
                        Auto(false);
                        DisplayNextSentence();

            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SkipToNextImportantDialogue()
    {
        canPressSpace = false;
        CheckSkip();
        canPressSpace = true;
    }

    private void CheckSkip()
    {
        if (dialogueQueue.Count <= 0) return;


        Auto(false);

        if (currentDialogue.speakerID > 0 && currentDialogue.speakerID <= 800)
        {
            switch (currentDialogue.dialogueMode)
            {
                case "Default":
                {
                    if (currentDialogue.speakerPosition != "NoShow")
                        characterManager.DefaultDisableCharacters();
                    break;
                }
                case "DialogueCloud":
                    bubbleDialoguePanel.SetActive(false);
                    break;
            }
        }

        imgEffectControl.DeactivateAll();

        Queue<Dialogue> tempQueue = new Queue<Dialogue>();
        foreach (Dialogue dialogue in dialogueQueue)
        {
            tempQueue.Enqueue(dialogue);
        }

        Debug.LogError(tempQueue.Peek().bookMark);
        tempQueue.Dequeue();

        Dialogue nextDialogue = tempQueue.Peek();

        while (dialogueQueue.Count > 0)
        {
            if (currentDialogue.jumpMark == -1)
            {
                if (nextDialogue.mode != "" || dialogueQueue.Peek().mode != "")
                    break;
                previousDialogue = currentDialogue;
                currentDialogue = dialogueQueue.Dequeue();
                tempQueue.Dequeue();
                nextDialogue = tempQueue.Peek();
            }
            else if (currentDialogue.jumpMark > -1)
            {
                if (currentDialogue.jumpMark <= currentDialogue.bookMark)
                    break;
                int targetJumpmark = currentDialogue.jumpMark;
                int loopTime = targetJumpmark - currentDialogue.bookMark;

                for (int i = 0; i < loopTime; i++)
                {
                    if (i == loopTime - 1 && (nextDialogue.mode != "" || dialogueQueue.Peek().mode != ""))
                        break;
                    previousDialogue = currentDialogue;
                    currentDialogue = dialogueQueue.Dequeue();
                    tempQueue.Dequeue();
                    nextDialogue = tempQueue.Peek();
                }
            }

            jumpMark = currentDialogue.jumpMark;

            if (currentDialogue.audioData.bgm != "")
                gameControl.bgm = currentDialogue.audioData.bgm;

            if (!gameControl.storedBookmark.Contains(nextDialogue.bookMark))
            {
                break;
            }

            if (nextDialogue.speakerID >= 997)
            {
                break;
            }

            if (nextDialogue.mode != "" || dialogueQueue.Peek().mode != "")
            {
                break;
            }

            if (currentDialogue.chapterState == -1 || currentDialogue.chapterState == -2)
            {
                isSkipToEnd = true;
                break;
            }

            var peekfunc = dialogueQueue.Peek().callFuncs.func1.Trim();
            if (peekfunc == "LastEndDetermination" ||
                peekfunc == "CheckTrueEnd")
            {
                break;
            }
        }

        if (!string.IsNullOrEmpty(gameControl.bgm))
        {
            SoundManager.PlayBgm(gameControl.bgm.Trim());
        }

        /*if (isTyping)
        {
            isPaused = true;
            StopCoroutine(displayLineCoroutine);

            switch (currentDialogue.speakerID)
            {
                case 800:
                    narratorText.text = currentSentence + "<sprite=15>";
                    break;
                case 900:
                    centerText.text = currentSentence;
                    break;
                default:
                {
                    if (currentDialogue.speakerID >= 0 && currentDialogue.speakerID <= 996)
                    {
                        if (!string.IsNullOrEmpty(currentSentence))
                        {
                            dialogueText.text = currentSentence + "<sprite=15>";
                        }
                    }

                    break;
                }
            }

            isTyping = false;
        }*/

        DisplayNextSentence();
    }
    
    private void CallFunctions()
    {
     Debug.Log("Called Func");
        string func1 = currentDialogue.callFuncs.func1.Trim();
        string func2 = currentDialogue.callFuncs.func2.Trim();

        if (func1 == "UnlockAchievement")
        {
            gameControl.OnUnlockAchievement(func2);
            return;
        }

        if (func1 == "SetTruthData")
        {
            var propertyInfo = gameControl.GetType().GetProperty(func2);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(gameControl, 1);

                if (propertyInfo.Name == "unlockTrueEndLoveLetter")
                {
                    gameControl.OnUnlockAchievement("BinZiFakeHair");

                }
                if (propertyInfo.Name == "unlockTrueEndDrug")
                {
                    gameControl.OnUnlockAchievement("BinZiRolex");
                }
                if (propertyInfo.Name == "unlockTrueEndStory")
                {
                    gameControl.OnUnlockAchievement("BinZiGirl");
                }
            }

            return;
        }

        if (func1 == "ResetEndData")
        {
            var propertyInfo = gameControl.GetType().GetProperty(func2);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(gameControl, 0);
            }

            return;
        }
        
        //

        if (func1 == "CheckTruthData")
        {
            // Split func2 by '&' to get multiple property names
            var propertyNames = func2.Split('&');
            bool allTrue = true; // Assume all values are true initially

            foreach (var propertyName in propertyNames)
            {
                var trimmedPropertyName = propertyName.Trim(); // Trim whitespace
                var valueInfo = gameControl.GetType().GetProperty(trimmedPropertyName); // Get each property

                if (valueInfo != null)
                {
                    // Get the value of the property and check if it's 1
                    int value = (int)valueInfo.GetValue(gameControl);

                    // If any value is not 1, set allTrue to false
                    if (value != 1)
                    {
                        allTrue = false;
                        break; // No need to check further
                    }
                }
                else
                {
                    allTrue = false; // If property is missing, treat it as false
                    break; // No need to check further
                }
            }

            // Set jumpMark based on the evaluation
            if (allTrue)
                jumpMark = int.Parse(currentDialogue.roads.road1);
            else
                jumpMark = int.Parse(currentDialogue.roads.road2);

            return;
        }

        if (func1 == "CheckTruthItem")
        {
            if (gameControl.items.Contains(func2))
                jumpMark = int.Parse(currentDialogue.roads.road1);
            else
                jumpMark = int.Parse(currentDialogue.roads.road2);
            if(func2 == "EarthenJar")
                gameControl.OnUnlockAchievement("SetFireToTheRoom");
            return;
        }

        if (func1 == "ResetLastOpenedDoor")
        {
            if (PlayerPrefs.HasKey("LastOpenedDoor"))
            {
                PlayerPrefs.SetInt("LastOpenedDoor", 0);
                PlayerPrefs.Save();
            }
        }

        ExecuteFunction(func1);
        ExecuteFunction(func2);

        void ExecuteFunction(string func)
        {
//            Debug.Log(func);
            if (string.IsNullOrEmpty(func)) return;
            if (GetComponent<FunctionListener>().CallFunctionsByCell(func)) return;
     //       Debug.Log("yeah");
            switch (func)
            {
                case "PurchasePack":
                    if (!gameControl.boughtUnlimitedPackage)
                        purchasePack.SetActive(true);
                    break;

                case "CommentBox":
                    commentBox.SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }

    public void GameOverPanels()
    {
        //SoundManager.PlaySfx("gameover");
        gameOverPannel.SetActive(true);
        if (!string.IsNullOrEmpty(currentDialogue.roads.road1))
            gameOverPanelControl.replayMark = int.Parse(currentDialogue.roads.road1);
    }

    private IEnumerator CountTimeandGetItem(InventoryItems item)
    {
        yield return new WaitForSeconds(2f);
        inventoryManager.SetGetItemPenel(item);
    }

    private void CheckSkipDialogueEnable()
    {
        if (gameControl.storedBookmark.Contains(currentDialogue.bookMark))
        {
            if (currentDialogue.jumpMark > -1)
            {
                skipDialogueBtn.gameObject.SetActive(gameControl.storedBookmark.Contains(currentDialogue.jumpMark));
            }
            else
            {
                skipDialogueBtn.gameObject.SetActive(
                    gameControl.storedBookmark.Contains(dialogueQueue.Peek().bookMark));
            }

            return;
        }

        skipDialogueBtn.gameObject.SetActive(false);
    }

    private void ShowDialogueCloud()
    {
        bubbleDialoguePanel.SetActive(true);
        dialoguePanel.SetActive(false);

        int num = int.Parse(currentDialogue.speakerPosition);
        dialogueBubble.GetComponent<RectTransform>().pivot = bubbleSpawnPts[num].pivot;
        dialogueBubble.GetComponent<RectTransform>().position = bubbleSpawnPts[num].position;

        if (previousDialogueMode != "DialogueCloud")
        {
            dialogueBubbles_anim.SetTrigger("FadeIn");
        }
    }
}