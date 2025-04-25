using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;

public class TapGameControl : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;
    [SerializeField] private AudioManager audioManager;
    public int maxClickNum;
    public List<AudioClip> clickSound;
    public Button tapBtn;
    public Animator clickAnimation;

    public Image moveImg;
    [SerializeField] private Vector3 currentScale;
    [SerializeField] private float moveRangeX;
    [SerializeField] private float moveRangeY;
    [SerializeField] private float reduceValue;

    public bool keepPlaySFX;
    public bool waitForSFX = false;

    public int count;

    private void Start()
    {
        if (moveImg != null)
            currentScale = moveImg.transform.localScale;

        count = 0;
        tapBtn.enabled = true;
    }

    private void OnEnable()
    {
        if (moveImg != null)
            currentScale = moveImg.transform.localScale;

        count = 0;
        tapBtn.enabled = true;
    }

    public void OnButtonClicked()
    {
        count += 1;

        if(clickAnimation != null)
        {
            clickAnimation.SetTrigger("Click");
        }

        if(moveImg != null)
        {
            moveImg.transform.position += new Vector3(moveRangeX, moveRangeY, 0f);
            if (reduceValue > 0)
                ReduceNum();
        }

        if (clickSound.Count > 0)
        {
            int clickSoundNum = Random.Range(0, clickSound.Count);
            AudioClip selectedClip = clickSound[clickSoundNum];
            audioManager.PlaySFX(selectedClip);

            if (waitForSFX)
            {
                disableButton();
                Invoke(nameof(enableButton), selectedClip.length);
            }
        }

        if (count == maxClickNum)
        {
            if(!keepPlaySFX)
                audioManager.StopSfx();
            tapBtn.enabled = false;
            StartCoroutine(investigationControl.EndInvestigation());
        }

    }

    public void ReduceNum()
    {
            currentScale.x -= reduceValue;
            currentScale.y -= reduceValue;

            // Make sure the scale doesn't go below 0
            currentScale.x = Mathf.Max(0f, currentScale.x);
            currentScale.y = Mathf.Max(0f, currentScale.y);

            moveImg.transform.localScale = currentScale;
    }

    private void disableButton()
    {
        tapBtn.gameObject.SetActive(false);
    }

    private void enableButton()
    {
        tapBtn.gameObject.SetActive(true);
    }
}
