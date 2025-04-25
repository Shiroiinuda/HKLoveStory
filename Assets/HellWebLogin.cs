using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class HellWebLogin : MonoBehaviour
{
    private int passWord = 1113;
    public TMP_InputField passWordInput;
    public UnityEvent loginSucess;
    public RectTransform pWRect;
    public float duration = 0.5f;
    public float magnitude = 10f;
    public RectTransform title;
    public GameObject loginPanel;
    
    private void Start()
    {
        passWordInput.onEndEdit.AddListener((arg0)=>CheckPW(arg0));
    }

    private void OnEnable()
    {
        loginPanel.SetActive(true);
        passWordInput.text = "";
        title.anchoredPosition = new Vector2(0, 312);
        title.localScale = Vector3.one;
    }

    private void CheckPW(string pw)
    {

        int.TryParse(pw, out var A);
        if (passWord ==A )
        {
            loginPanel.SetActive(false);
            title.DOAnchorPos(new Vector2(-700, 425), 2);
            title.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 2).OnComplete(()=>loginSucess.Invoke());
        }
        else
        {
            StartCoroutine(ShakeRoutine());
        }
    }
    
    private IEnumerator ShakeRoutine()
    {
        Vector3 originalPosition = pWRect.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            pWRect.anchoredPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        pWRect.anchoredPosition = originalPosition;
        passWordInput.text = "";
    }
}
