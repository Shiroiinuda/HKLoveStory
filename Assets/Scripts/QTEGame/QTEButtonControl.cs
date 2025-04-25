using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Don't forget to include DoTween

public class QTEButtonControl : MonoBehaviour
{
    [SerializeField] private QTECircleControl qteCircleControl;
    [SerializeField] private QTEGame qteGame;
    
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;

    [SerializeField] private Sprite missImg;
    [SerializeField] private Sprite goodImg;

    [SerializeField] private Animator buttonAnimtion;
    [SerializeField] private Animator icePickAnimtion;

    [SerializeField] private Image missBloodImg;

    private bool cliicked;

    public void Start()
    {
        // Access the parent's parent GameObject
        GameObject parentObject = transform.parent.gameObject;

        // Get the MyScript component from the parent's parent GameObject
        qteGame = parentObject.GetComponent<QTEGame>();

        if (qteGame.isIcePickGame || qteGame.isFakeArgueQTE)
            missBloodImg = parentObject.transform.Find("missBloodImg").GetComponent<Image>();
    }

    public void OnButtonClick()
    {

#if UNITY_ANDROID || UNITY_IOS

        qteGame.marks += 1;
        if (qteGame.audioManager != null && qteGame.correctSFX != null)
            qteGame.audioManager.PlaySFX(qteGame.correctSFX);
        buttonImage.sprite = goodImg;
        buttonAnimtion.SetTrigger("FadeOut");
        if (icePickAnimtion != null)
            icePickAnimtion.SetTrigger("FadeOut");

        qteCircleControl.CheckClicked();

        button.enabled = false;

        qteGame.clickCount += 1;

        Invoke("CheckAfterShowingUI", 0.5f);
#else
        if (qteCircleControl.canClick == true)
        {
            qteGame.marks += 1;
            if (qteGame.audioManager != null && qteGame.correctSFX != null)
                qteGame.audioManager.PlaySFX(qteGame.correctSFX);
            buttonImage.sprite = goodImg;
            buttonAnimtion.SetTrigger("FadeOut");
            if (icePickAnimtion != null)
                icePickAnimtion.SetTrigger("FadeOut");
        }
        else
        {
            if (qteGame.audioManager != null && qteGame.missSFX != null)
                qteGame.audioManager.PlaySFX(qteGame.missSFX);
            buttonImage.sprite = missImg;
            buttonAnimtion.SetTrigger("FadeOut");
            if (icePickAnimtion != null)
                icePickAnimtion.SetTrigger("FadeOut");
            if (qteGame.isIcePickGame || qteGame.isFakeArgueQTE)
            {
                qteGame.missMark += 1;
                DoBloodFading();
            }
        }

        qteCircleControl.CheckClicked();

        button.enabled = false;

        qteGame.clickCount += 1;

        Invoke("CheckAfterShowingUI", 0.5f);
#endif
    }

    private void CheckAfterShowingUI()
    {
        qteGame.CheckClickCount();
    }

    private void DoBloodFading()
    {
        if (missBloodImg != null)
        {
            Debug.Log("DoBloodFading");

            missBloodImg.color = new Color(missBloodImg.color.r, missBloodImg.color.g, missBloodImg.color.b, 0); // Ensure it starts transparent
            missBloodImg.gameObject.SetActive(true); // Make sure it's active
            missBloodImg.DOFade(1, 0.2f).OnComplete(() =>
            {
                missBloodImg.DOFade(0, 0.2f).OnComplete(() =>
                {
                    missBloodImg.gameObject.SetActive(false); // Hide it after fade out
                });
            });
        }
    }
}
