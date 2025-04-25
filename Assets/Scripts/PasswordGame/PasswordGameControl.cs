using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Investigation;
using TMPro;

public class PasswordGameControl : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public InvestigationControl investigationControl;
    public AudioManager audioManager;

    [SerializeField] protected int goalPassword;
    [SerializeField] protected List<TextMeshProUGUI> pwTextList;
    [SerializeField] protected List<int> pwNumList;

    [SerializeField] protected int inputPw = 0;

    public GameObject closeLid;
    public GameObject OpenLid;

    public bool needJumpBookMark;

    public AudioClip unlockedSfx;
    public AudioClip typingSfx;

    public List<GameObject> disableObjList;

    private void Start()
    {
        foreach (TextMeshProUGUI pwText in pwTextList)
        {
            pwText.text = "0";
        }
    }

    public virtual void OnPWButtonClicked(int num)
    {
        PlaySoundIfNotNull(typingSfx);
        pwNumList[num] += 1;
        if (pwNumList[num] == 10)
            pwNumList[num] = 0;

        pwTextList[num].text = pwNumList[num].ToString();
        if (needJumpBookMark == false)
            CheckCorrectPW();
    }

    public void OnEnterButtonClick()
    {
        CheckCorrectPW();
    }
    public void OnBackButtonClick()
    {
        if (dialogueManager != null)
        {

            dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);
            if (investigationControl != null)
            {
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
            }
        }
    }

    protected void CheckCorrectPW()
    {
        inputPw = 0;

        for (int i = 0; i < pwNumList.Count; i++)
        {
            int currentSum = 0;
            currentSum = pwNumList[i] * (int)Mathf.Pow(10, (pwNumList.Count - 1 - i));

            inputPw += currentSum;
        }

        if (inputPw == goalPassword)
        {
            PlaySoundIfNotNull(unlockedSfx);

            if (needJumpBookMark == false)
            {
                if (closeLid != null)
                    closeLid.SetActive(false);
                if (OpenLid != null)
                    OpenLid.SetActive(true);
                if (disableObjList.Count > 0)
                {
                    foreach (GameObject disableObj in disableObjList)
                        disableObj.SetActive(false);
                }
                gameObject.SetActive(false);
            }
            else
            {
                if (dialogueManager != null)
                {
                    dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);
                    if (investigationControl != null)
                    {
                        StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                    }
                }
            }
        }
    }

    protected void PlaySoundIfNotNull(AudioClip clip)
    {
        if (clip != null)
            audioManager.PlaySFX(clip);
    }
}
