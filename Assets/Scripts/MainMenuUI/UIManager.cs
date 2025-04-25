using System;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using SoundControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [Foldout("Log Button",true)]
    [SerializeField] private Button logBtn;
    [SerializeField] private GameObject logPanel;
    [Foldout("Log Button")]
    [SerializeField] private LogController logController;

    [Foldout("Skip Button")] [SerializeField]
    private Button skipBtn;
    [SerializeField]
    private Button autoBtn;
    [SerializeField]
    private Button autoBtnOff;

    [Foldout("Game Log Button", true)]
    [SerializeField] private Button logBtn_Game;
    [SerializeField] private GameObject logPanel_Game;
    [Foldout("Game Log Button")]
    [SerializeField] private LogController logController_Game;
    [Separator("Mobile")]
    [SerializeField] private List<GameObject> mobileList;

    public Button saveButton;

    private bool isHideUI;
    [SerializeField]private RectTransform tabs;
    public GameObject dialoguePanel;
    public Button hideUiBtn;
    
    private void Awake()
    {
        logBtn.onClick.AddListener(LogButtonFunc);
        skipBtn.onClick.AddListener(SkipButtonFunc);
        hideUiBtn.onClick.AddListener(HideUI);
        autoBtn.gameObject.SetActive(false);
        autoBtnOff.gameObject.SetActive(false);
        /*autoBtn.onClick.AddListener(()=>AutoButtonFunc(true));
        autoBtnOff.onClick.AddListener(() => AutoButtonFunc(false));*/
        logBtn_Game.onClick.AddListener(GameLogButtonFunc);
        #if !(UNITY_ANDROID || UNITY_IOS)
        foreach (var gameObject in mobileList)
        {
            gameObject.SetActive(false);    
        }
        hideUiBtn.gameObject.SetActive(true);

        #else
        foreach (var gameObject in mobileList)
        {
            gameObject.SetActive(true);
        }
        hideUiBtn.gameObject.SetActive(false);
        #endif
    }
    private Vector3 lastMousePosition;
    private float totalMovement;
    private void FixedUpdate()
    {
        if (!isHideUI) return;
        Vector3 currentMousePosition = Mouse.current.delta.ReadValue();
        float movementThisFrame = Vector3.Distance(currentMousePosition, lastMousePosition);

        if (movementThisFrame > 0f)
        {
            totalMovement += movementThisFrame;
            if (totalMovement > 50)
            {
                HideUI();
                totalMovement = 0;
            }
            // Debug.Log("Movement this FixedUpdate: " + movementThisFrame + " | Total Movement: " + totalMovement);
        }

        lastMousePosition = currentMousePosition;
    }

    private void HideUI()
    {
        isHideUI = !isHideUI;
        if (isHideUI)
        {
            tabs.DOAnchorPosY(270f,0.5f);
        }
        else
        {
            tabs.DOAnchorPosY(-40f,0.5f);
        }
        dialoguePanel.SetActive(!isHideUI);
    }

    private void LogButtonFunc()
    {
        logPanel.SetActive(true);
        logController.OnLogUpdate();
        SoundManager.PlaySfx("logbtn");
    }

    private void GameLogButtonFunc()
    {
        logPanel_Game.SetActive(true);
        logController_Game.OnLogUpdate();
        SoundManager.PlaySfx("logbtn");
    }

    private void AutoButtonFunc(bool isOn)
    {
        DialogueManager.Instance.autoPlay = isOn;
            DialogueManager.Instance.Auto(isOn);
            
        SoundManager.PlaySfx("autobtn");
    }

    private void SkipButtonFunc()
    {
        DialogueManager.Instance.SkipToNextImportantDialogue();
        SoundManager.PlaySfx("autobtn");
    }

    public void CheckSaveBtn()
    {
        saveButton.interactable = !GameControl.instance.isInvestigation &&
                                  DialogueManager.Instance.currentDialogue.mode != "InGame";
    }
}


