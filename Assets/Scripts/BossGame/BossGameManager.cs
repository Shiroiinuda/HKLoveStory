using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Investigation;
using SoundControl;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using Unity.IO.LowLevel.Unsafe;

public class BossGameManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public InvestigationControl investigationControl;
    public PlayableDirector gameOverEffect;
    public TextMeshProUGUI fuNumText;
    private int _fuNum;
    public GameObject Fu;
    private bool fullFu = false;

    public int fuNum
    {

        get => _fuNum;
        set
        {
            fuNumText.text = value.ToString();
            if (value < _fuNum && value != 0)
                SpawnText();
            _fuNum = value;

        }
    }

    public int startFuNum =30;
    
    public int spiritNum;
    public int sealedSpirit;

    public List<string> removedNameList;

    public RectTransform spawnposition;
    public RectTransform prefab;
    public RectTransform targetRect;

    // Shake settings
    public float shakeDuration = 0.5f;  // Total time to shake
    public float shakeIntensity = 10f;  // Maximum offset from original position
    public float shakeFrequency = 25f;  // How fast the position changes

    public void StartShake()
    {
        // Start the shake coroutine
        StartCoroutine(ShakeRectRoutine());
    }

    private IEnumerator ShakeRectRoutine()
    {
        // Capture the original anchoredPosition
        Vector2 originalPos = targetRect.anchoredPosition;
        
        // Track how long we've been shaking
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Random offset around the original position
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            
            // Apply offset
            targetRect.anchoredPosition = originalPos + new Vector2(offsetX, offsetY);

            // Wait a small fraction of a second to create a stutter effect
            yield return new WaitForSeconds(1f / shakeFrequency);

            elapsed += 1f / shakeFrequency;
        }

        // Return the RectTransform to the original position
        targetRect.anchoredPosition = originalPos;
    }
    private const string BossGameFuPP = "BossFuAmount";
    private void Start()
    {
        fuNum = startFuNum + FBPP.GetInt(BossGameFuPP);
        fullFu = (fuNum >= 80);
        InvestigationControl.Instance.skipButtonControl.Skipping.AddListener(IsWin);
    }

    public void SpawnText()
    {
        var MinusText=  Instantiate(prefab, spawnposition);
        MinusText.SetParent(spawnposition);
        MinusText.anchoredPosition = Vector2.zero;
        MinusText.DOAnchorPos(new Vector2(0, 100), 1);
        MinusText.GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
    }
    public void MinusFu()
    {
        Debug.Log("Clicked");
        
        if (fuNum > 0)
        {
            fuNum -= 1;
            SoundManager.PlaySfx("spell");
        }
        else
        {
            SoundManager.PlaySfx("empty");
            if (fullFu)
                GameControl.instance.OnUnlockAchievement("EatShitLaNei");
        }

        StartShake();
    }

    public void AddSealedSpirit()
    {
        sealedSpirit++;
        CheckWin();
    }

    public void LoseGame()
    {
        InvestigationControl.Instance.skipButtonControl.Skipping.RemoveAllListeners();
        dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);
        if (gameOverEffect != null)
        {
            gameOverEffect.gameObject.SetActive(true);
            gameOverEffect.Play();
        }
    }

    private void CheckWin()
    {
        if (sealedSpirit >= spiritNum)
        {
            IsWin();
        }
    }

    private void IsWin()
    {
        InvestigationControl.Instance.skipButtonControl.Skipping.RemoveAllListeners();
        dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);
        StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
    }
    public void BossGameEffectEnd()
    {
        dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);
        StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
    }

}
