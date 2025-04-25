using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasierLocalization;
using Investigation;
using MyBox;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TestArgueGame : MonoBehaviour
{
    public RectTransform leftPanel; // Reference to the left panel
    public RectTransform rightPanel; // Reference to the right panel
    [Range(0, 10)] public float currentScore = 5;
    private float aimScore;
    public int maxScore = 10;
    public List<ArgueGameDialogue> round;
    public bool isEnd;
    public GameObject prefab;
    public Transform spawnParent;
    public UnityEvent argueBtns;
    private List<Rect> ButtonRect;
    public int buttonPadding = 10;
    public int currentNum = 0;
    public Animator womenAnim;
    public List<Sprite> characterSprite;
    [Separator("Words")] public Transform textPrefab;
    public int spawnRate = 1;
    public Transform wordsParent;
    public Image characterImage;
    private int winJumpMark;
    private int loseJumpMark;
    public bool isSpeaking;

    public void Start()
    {
        characterImage.sprite = characterSprite[0];
        var dialogueManager = DialogueManager.Instance;
        if (dialogueManager.currentDialogue.roads.road1 != "")
            winJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);
        if (dialogueManager.currentDialogue.roads.road2 != "")
            loseJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);
        ButtonRect = new List<Rect>();
        UpdatePanelSizes();
        aimScore = currentScore;

        StartCoroutine(ReduceScore());
        StartCoroutine(SpawningText());
        SpawnChoices();
    }

    private void SpawnChoices()
    {
        Debug.Log("Sapwn Choice");
        for (int i = 0; i < round[currentNum].argueChoices.Count; i++)
        {
            GameObject button = Instantiate(prefab, transform.position, quaternion.identity);
            button.transform.SetParent(spawnParent);
            Debug.Log(i % 2 == 0);
            button.GetComponent<ArgueGameBtn>().Init(this, Localization.GetString($"ArgueGame/{round[currentNum].argueChoices[i].choiceText}"),
                Resources.Load($"MiniGames/ArgueGame/audio/Button/{Localization.NpcVoiceLan()}/{round[currentNum].argueChoices[i].audio.name}") as
                    AudioClip, round[currentNum].argueChoices[i].scoreChange,
                wordsParent.localPosition, i % 2 == 0);

            int retries = 150;
            bool positionFound = false;
            Rect newButtonRect = default;
            for (int attempt = 0; attempt < retries; attempt++)
            {
                Vector2 buttonSize = prefab.GetComponent<RectTransform>().sizeDelta;
                var position = Vector3.zero;
#if UNITY_ANDROID || UNITY_IOS
                Debug.Log(-Screen.height / 2);
                                position = new Vector3(Random.Range(-658, 531), Random.Range(-Screen.height/2 + buttonSize.y, -200+ buttonSize.y), 0);
                #else
                                 position = new Vector3(Random.Range(-658, 531), Random.Range(-450, -200), 0);
                #endif
                
                newButtonRect = new Rect(
                    position.x - buttonSize.x / 2 - buttonPadding,
                    position.y - buttonSize.y / 2 - buttonPadding,
                    buttonSize.x + buttonPadding * 2,
                    buttonSize.y + buttonPadding * 2
                );

                // Check if it overlaps with any already spawned buttons
                if (!IsOverlapping(newButtonRect))
                {
                    positionFound = true;
                    break;
                }
            }


            if (positionFound)
            {
                button.GetComponent<RectTransform>().anchoredPosition = newButtonRect.center;
                button.transform.localScale = Vector3.one;
                ButtonRect.Add(newButtonRect);
            }
        }
    }

    bool IsOverlapping(Rect newRect)
    {
        foreach (Rect existingRect in ButtonRect)
        {
            if (newRect.Overlaps(existingRect))
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        UpdatePanelSizes();
    }

    [ButtonMethod]
    public void UpdatePanelSizes()
    {
        aimScore = Mathf.Lerp(aimScore, currentScore, Time.deltaTime);
//        Debug.Log($"{Screen.width}");
        float leftPanelWidth = (aimScore / maxScore) * 1920;
        float rightPanelWidth = -((aimScore - maxScore) / maxScore) * 1920;

        leftPanel.sizeDelta = new Vector2(leftPanelWidth, leftPanel.sizeDelta.y);
        rightPanel.sizeDelta = new Vector2(rightPanelWidth, rightPanel.sizeDelta.y);
    }

    private IEnumerator ReduceScore()
    {
        while (!isEnd)
        {
            if (!isSpeaking)
            {
                currentScore -= 0.1f;
                int spriteScore = (int)(currentScore / 3.33f);
                Debug.Log(spriteScore);
                /*if(characterSprite[spriteScore] !=null)
                    characterImage.sprite = characterSprite[spriteScore];*/
            }

            yield return new WaitForSeconds(1f);
            if (currentScore <= 0)
            {
                isEnd = true;
                GameOver(false);
            }
        }
    }

    public void GameOver(bool isWin)
    {
        isEnd = true;
        int resultJump = (maxScore / 2) < currentScore ? winJumpMark : loseJumpMark;
        if ((maxScore / 2) < currentScore)
        {
            InvestigationControl.Instance.BattleWin();
        }
        else
        {
            InvestigationControl.Instance.BattleFail();
        }

        DialogueManager.Instance.jumpMark = resultJump;
        StartCoroutine(InvestigationControl.Instance.EndInvestigationThatJumpItself());
    }

    public void ChangeScore(int score)
    {
        currentScore += score;
        if (currentScore >= maxScore)
        {
            StartCoroutine(IChangeScore());
        }
    }

    IEnumerator IChangeScore()
    {
        yield return new WaitForSeconds(2f);
        currentScore = maxScore;
        GameOver(true);
    }
    public void EndArgue()
    {
        Debug.Log("A");
        List<string> tmp = new List<string>();
        List<string> speakerTmp = new List<string>();
        List<AudioClip> clipTmp = new List<AudioClip>();
        Debug.Log(round[currentNum].dialogueNum);
        for (int i = 0; i < round[currentNum].dialogueNum; i++)
        {
            int tmpInt = i;
            Debug.Log($"ArgueGame/{currentNum + 1}_{tmpInt}");
            tmp.Add(Localization.GetString($"ArgueGame/{currentNum + 1}_{tmpInt}"));
            speakerTmp.Add(Localization.GetString($"ArgueGame/{currentNum + 1}_{tmpInt}_SpeakerName"));
            clipTmp.Add( //{Localization.NpcVoiceLan()}
                Resources.Load($"MiniGames/ArgueGame/audio/Dialogue/HK/talkgameBDialogue_{currentNum + 1}_{tmpInt}") as
                    AudioClip);
            Debug.Log(
                Resources.Load($"MiniGames/ArgueGame/audio/Dialogue/HK/talkgameBDialogue_{currentNum + 1}_{tmpInt}") as
                    AudioClip);
            Debug.Log($"MiniGames/ArgueGame/audio/Dialogue/HK/talkgameBDialogue_{currentNum + 1}_{tmpInt}");
        }

        currentNum++;

        if (currentNum >= round.Count)
        {
            Debug.Log($"Argue Game End");
            GameOver(true);
            return;
        }

        argueBtns.Invoke();
        argueBtns.RemoveAllListeners();
        ButtonRect.Clear();
        isSpeaking = true;
        StartCoroutine(PlayArgueDialogue(tmp, speakerTmp, clipTmp));
        spawnTextNum = 0;
    }

    IEnumerator PlayArgueDialogue(List<string> tmp,List<string> speakerTmp,List<AudioClip> clipTmp)
    {
        yield return new WaitForSeconds(2f);
        InvestigationControl investigationControl = GetComponentInParent<InvestigationControl>();
        investigationControl.ShowSentences(tmp, speakerTmp, clipTmp);
        investigationControl.endDialogue.AddListener(isEndArgue);
        investigationControl.endDialogue.AddListener(AudioManager.Instance.StopCSound);
        investigationControl.endDialogue.AddListener(() => isSpeaking = false);
       
    }

    public void isEndArgue()
    {
        characterImage.sprite = characterSprite[currentNum];
        SpawnChoices();
    }

    private int spawnTextNum = 0;

    IEnumerator SpawningText()
    {
        while (!isEnd)
        {
            if (!isSpeaking)

            {
                spawnTextNum %= round[currentNum].dialogue.Count;
                Transform wordObject = Instantiate(textPrefab, this.transform);
                //wordObject.GetComponent<WordToPosition>().localize.mTerm = $"SceneDescription/ArgueGame{tempInt}";
                wordObject.SetParent(wordsParent);
                wordObject.transform.localPosition = Vector3.zero;
                wordObject.GetComponent<WordToPosition>().textMesh.text = Localization.GetString($"ArgueGame/{round[currentNum].dialogue[spawnTextNum]}");

                wordObject.GetComponent<AudioSource>().clip =
                    Resources.Load($"MiniGames/ArgueGame/audio/HK/talkgameB_{currentNum}_{spawnTextNum}") as AudioClip;
                wordObject.GetComponent<AudioSource>().PlayDelayed(1.5f);
                spawnTextNum++;
                //Localization.GetString($"SceneDescription/ArgueGame{tempInt}");
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
}

[Serializable]
public struct ArgueGameDialogue
{
    public List<ArgueChoice> argueChoices;
    public List<string> dialogue;
    public int dialogueNum;
}

[Serializable]
public struct ArgueChoice
{
    public String choiceText;
    public AudioClip audio;
    public int scoreChange;
}