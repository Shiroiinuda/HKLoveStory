using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Don't forget to include DoTween

public class QTECircleControl : MonoBehaviour
{
    public GameObject button;
    [SerializeField] private Button buttonUI;

    public bool canClick;

    private bool clicked;

    [SerializeField] private Sprite missSprite;

    [SerializeField] private Animator buttonAnimtion;
    [SerializeField] private Animator icePickAnimtion;

    [SerializeField] private QTEGame qteGame;

    [SerializeField] private GameObject icePick;
    private Vector3 icePickTargetScale;

    [SerializeField] private Image missBloodImg;



    // Start is called before the first frame update
    void Start()
    {
        // Access the parent's parent GameObject
        GameObject parentObject = transform.parent.parent.gameObject;

        // Get the MyScript component from the parent's parent GameObject
        qteGame = parentObject.GetComponent<QTEGame>();

        StartCoroutine(ScaleButton());
        canClick = false;

        clicked = false;

        if (icePick != null)
        {
            icePickTargetScale = icePick.transform.localScale;
            icePick.transform.localScale = Vector3.zero; // Start from zero scale
        }
        if(qteGame.isIcePickGame || qteGame.isFakeArgueQTE)
            missBloodImg = parentObject.transform.Find("missBloodImg").GetComponent<Image>();
    }

    private IEnumerator ScaleButton()
    {
#if UNITY_ANDROID || UNITY_IOS
        float scaleSpeed = Random.Range(0.95f, 1f);
#else
        float scaleSpeed = Random.Range(1f, 3f);
#endif

        while (this.gameObject.transform.localScale.x > 1)
        {
            this.gameObject.transform.localScale -= new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, 0f);
            if (icePick != null)
                icePick.transform.localScale = Vector3.Lerp(icePick.transform.localScale, icePickTargetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        canClick = true;

        yield return new WaitForSeconds(0.5f);

        canClick = false;

        if (clicked == true)
        {

        }
        else
        {
            MissClick();
        }
        
        
    }

    private void MissClick()
    {
        buttonUI.enabled = false;
        buttonAnimtion.SetTrigger("FadeOut");
        if (icePickAnimtion != null)
            icePickAnimtion.SetTrigger("FadeOut");

        if (qteGame.isIcePickGame || qteGame.isFakeArgueQTE)
        {
            qteGame.missMark += 1;
            DoBloodFading();
        }

        if (qteGame.audioManager != null && qteGame.missSFX != null)
            qteGame.audioManager.PlaySFX(qteGame.missSFX);
        button.GetComponent<Image>().sprite = missSprite;

        qteGame.clickCount += 1;

        if (qteGame.isIcePickGame || qteGame.isFakeArgueQTE)
            return;
        qteGame.CheckClickCount();
    }

    public void CheckClicked()
    {
        clicked = true;
    }

    private void DoBloodFading()
    {
        if (missBloodImg != null)
        {
            missBloodImg.color = new Color(missBloodImg.color.r, missBloodImg.color.g, missBloodImg.color.b, 0); // Ensure it starts transparent
            missBloodImg.gameObject.SetActive(true); // Make sure it's active
            missBloodImg.DOFade(1, 0.2f).OnComplete(() =>
            {
                missBloodImg.DOFade(0, 0.2f).OnComplete(() =>
                {
                    missBloodImg.gameObject.SetActive(false); // Hide it after fade out
                    Invoke("QTEGameCheck", 0.5f);
                });
            });
        }
    }

    void QTEGameCheck()
    {
        qteGame.CheckClickCount();
    }
}
