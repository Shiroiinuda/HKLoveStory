using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossGameFu : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.DOFade(0, 0.5f).SetEase(Ease.InBack).onComplete = ByeByeFu;
        transform.DOScale(Vector3.zero, 1);
    }

    private void ByeByeFu()
    {
        Destroy(this.gameObject);
    }
}
