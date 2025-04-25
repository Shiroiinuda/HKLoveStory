using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Investigation;
using SoundControl;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LetterSortingGame : MonoBehaviour
{
    public BossGameManager bossGameManager;
    public Transform letterParent;
    public List<BossGameButton> letters;
    public Button resetBtn;
    public Image binziSpirit; // Reference to your UI Image component
    public Sprite originalSprite; // Array to hold your sprites
    public Sprite finishSpiritSprites; // Array to hold your sprites

    public List<BossGameButton> reOrderedBtn;
    // This can be an array or list to hold the correct sequence
    public List<int> correctSequence;
    public List<int> enteredSequence;
    public bool letterSortGameWon = false;
    public List<KeyCode> keyBoardCode;
    public GameObject shadowObj;
    public Vector2 goalPosition; // Set this to the desired goal position
    
    // private bool inited = false;

    private void Start()
    {
        
        shadowObj.SetActive(false);
        originalSprite = binziSpirit.sprite;
        for (int i = 0; i < letters.Count; i++)
        {
            int num = i;
            var letterBtn = letters[num].GetComponent<BossGameButton>();
            letterBtn.Button.onClick.AddListener(()=>enteredSequence.Add(num));
            letterBtn.Button.onClick.AddListener(()=>letterBtn.Button.interactable = false);
            letterBtn.Button.onClick.AddListener(CheckSortCompleted);
            letterBtn.Button.onClick.AddListener(()=>SoundManager.PlaySfx("rope"));
        }
        resetBtn.onClick.AddListener(ResetButton);
        
        // inited = true;
        
        for (int i = 0; i < letters.Count; i++)
        {
            int num =  Random.Range(0, letters.Count);
            letters[num].transform.SetParent(gameObject.transform);
            letters[num].transform.SetParent(letterParent);
        }
        resetBtn.transform.SetParent(gameObject.transform);
        resetBtn.transform.SetParent(letterParent);
        reOrderedBtn = letterParent.GetComponentsInChildren<BossGameButton>().ToList();
        for (int i = 0; i < keyBoardCode.Count; i++)
        {
        reOrderedBtn[i].AssignLetter(keyBoardCode[i].ToString());
        }
    }

    private void OnEnable()
    {
        if (bossGameManager.sealedSpirit >= 5 && !bossGameManager.removedNameList.Contains(this.gameObject.name) && bossGameManager.removedNameList.Count + 5 < bossGameManager.spiritNum)
        {
            int randNum = Random.Range(0, 2);
            if (letterSortGameWon && randNum != 0)
            {
                shadowObj.SetActive(true);
                shadowObj.GetComponent<RectTransform>().DOAnchorPos(goalPosition, 0.25f).OnComplete(() =>
                              shadowObj.SetActive(false));
                binziSpirit.sprite = originalSprite;
                bossGameManager.removedNameList.Add(this.gameObject.name);
                ResetGame();
            }
        }

        /*if (!inited) return;
        for (int i = 0; i < letters.Count; i++)
        {
            int num =  Random.Range(0, letters.Count);
            letters[num].transform.SetParent(gameObject.transform);
            letters[num].transform.SetParent(letterParent);
        }
        resetBtn.transform.SetParent(gameObject.transform);
        resetBtn.transform.SetParent(letterParent);
        reOrderedBtn = letterParent.GetComponentsInChildren<Button>().ToList();*/
    }

    private void Update()
    {

        if (Keyboard.current[Key.C].wasPressedThisFrame)
        {
            if(reOrderedBtn[0].Button.interactable)
                reOrderedBtn[0].Button.onClick.Invoke();
        }
        if (Keyboard.current[Key.V].wasPressedThisFrame)
        {
            if(reOrderedBtn[1].Button.interactable)
                reOrderedBtn[1].Button.onClick.Invoke();
        }
        if (Keyboard.current[Key.B].wasPressedThisFrame)
        {
            if(reOrderedBtn[2].Button.interactable)
                reOrderedBtn[2].Button.onClick.Invoke();
        }
        if (Keyboard.current[Key.N].wasPressedThisFrame)
        {
            if(reOrderedBtn[3].Button.interactable)
                reOrderedBtn[3].Button.onClick.Invoke();
        }
        if (Keyboard.current[Key.M].wasPressedThisFrame)
        {
            if(reOrderedBtn[4].Button.interactable)
                reOrderedBtn[4].Button.onClick.Invoke();
        }
        if (Keyboard.current[Key.X].wasPressedThisFrame)
        {
            if(resetBtn.interactable)
                resetBtn.onClick.Invoke();
        }
        
    }

    public void CheckSortCompleted()
    {
        if (enteredSequence.Count != correctSequence.Count) return;
        bool isCorrect = true;
        for (int i = 0; i < correctSequence.Count; i++)
        {
            if (correctSequence[i] != enteredSequence[i])
            {
                isCorrect = false;
                break;
            }

        }
        enteredSequence.Equals(correctSequence);
        if (isCorrect)
        {
            bossGameManager.AddSealedSpirit();
            binziSpirit.sprite = finishSpiritSprites;
            letterSortGameWon = true;
        }
        else
        {
            enteredSequence.Clear();
            SoundManager.PlaySfx("backbutton");
        }
        SetLetters(!isCorrect);
    }

    public void ResetButton()
    {
        if (!resetBtn.interactable) return;
        enteredSequence.Clear();
        SetLetters(true);
        SoundManager.PlaySfx("backbutton");
    }
    public void ReleaseLetters()
    {
        SetLetters(true);
    }
    

    private void SetLetters(bool isActivate)
    {
        foreach (var letter in letters)
        {
            letter.GetComponent<BossGameButton>().Button.interactable = isActivate;
        }
        
            resetBtn.interactable = !letterSortGameWon;
    }

    private void ResetGame()
    {
        enteredSequence.Clear();
        binziSpirit.sprite = originalSprite;
        letterSortGameWon = false;
        SetLetters(true);
        
    }
}
