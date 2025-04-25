using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ContinuousPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] PressGameControl pressGameControl;

    private bool isButtonPressed = false;
    private Coroutine countdownCoroutine;

    private GameObject bar;
    private Image progressBar;

    public GameObject[] nextButton;
    public bool isMyTurn;

    private float timeLeft;

    public List<GameObject> tutObj;

    void Awake()
    {
        timeLeft = pressGameControl.countTime;
        progressBar = transform.Find("progressBar").GetComponent<Image>();
        bar = transform.Find("Bar").gameObject;
        bar.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isMyTurn)
        {
            if (!isButtonPressed)
            {
                isButtonPressed = true;
                progressBar.fillAmount = 0f; // Reset the progress bar
                bar.SetActive(true);
                countdownCoroutine = StartCoroutine(WaitAndDestroy());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isMyTurn)
        {
            isButtonPressed = false;
            StopCoroutine(countdownCoroutine);
            progressBar.fillAmount = 0f; // Reset the progress bar
            bar.SetActive(false);
        }
    }

    IEnumerator WaitAndDestroy()
    {
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime / timeLeft;
            progressBar.fillAmount = progress;

            yield return null;
        }
        pressGameControl.AddCount();

        if (nextButton != null)
        {
            foreach(GameObject nextObject in nextButton)
            {
                nextObject.GetComponent<ContinuousPressButton>().isMyTurn = true;
                nextObject.GetComponent<Image>().raycastTarget = true;
            }
        }

        if(tutObj != null)
            foreach(GameObject obj in tutObj)
            {
                obj.SetActive(false);
            }

        this.gameObject.SetActive(false); // Destroy the button game object
    }
}