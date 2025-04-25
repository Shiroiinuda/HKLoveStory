using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Investigation;
using SoundControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class SimplerVideoController : MonoBehaviour
{
    [SerializeField] InvestigationControl investigationControl;
    [SerializeField] private float playSpeed;
    public VideoPlayer videoPlayer;
    public Image firstFrame;
    public int targetTap;
    private int tapCount;
    
    private PlayerInput playerInput;
    public Button leftBtn;
    public Button rightBtn;
    private bool isEndGame;

    private void Awake()
    {
        isEndGame = false;
        playerInput = new PlayerInput();
        playerInput.ChaseGame.Left.performed += ctx => LeftBtnClick();
        playerInput.ChaseGame.Right.performed += ctx => RightBtnClick();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    private void Start()
    {
        videoPlayer.time = 1;
        tapCount = 0;
    }

    private void LeftBtnClick()
    {
        NextFrame();
        BtnAnimation(leftBtn);
        SoundManager.PlaySfx("leftfoot");
        
    }
    private void RightBtnClick()
    {
        NextFrame();
        BtnAnimation(rightBtn);
        SoundManager.PlaySfx("rightfoot");
    }

    private async void BtnAnimation(Button Btn)
    {
        if (Btn is null) return;
        Btn.GetComponent<Image>().sprite = Btn.spriteState.pressedSprite;
        await Task.Delay(100);
        Btn.GetComponent<Image>().sprite = Btn.spriteState.disabledSprite;
    }
    public void NextFrame()
    {
        if (!isEndGame)
        {
            if(firstFrame !=null)
                firstFrame.gameObject.SetActive(false);
            tapCount += 1;
            videoPlayer.time += playSpeed;
            CheckVideoEnd();
        }
    }
    
    private void CheckVideoEnd()
    {
        if (videoPlayer.time + playSpeed >= videoPlayer.length || tapCount == targetTap)
        {
            tapCount = 0;
            isEndGame = true;
            StartCoroutine(investigationControl.EndInvestigation());
        }
    }
}
