using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    [AutoProperty(AutoPropertyMode.Children)] [ReadOnly][SerializeField]private Image image;
    [AutoProperty(AutoPropertyMode.Children)] [ReadOnly][SerializeField]private TextMeshProUGUI textMeshProUGUI;
    [Separator("Before")]
    public Color textBefore = Color.white;
    public Color imageBefore = Color.white;
    public Sprite imageSpriteBefore;
    [Separator("After")]
    public Color textAfter = Color.black;
    public Color imageAfter = Color.white;
    public Sprite imageSpriteAfter;
    void OnValidate()
    {
        BtnNormal();
    }
    public void BtnNormal()
    {
        textMeshProUGUI.color = textBefore;
        image.color = imageBefore;
        if (imageSpriteBefore is null) return;
        image.sprite = imageSpriteBefore;
    }
    public void BtnSelect()
    {
        textMeshProUGUI.color = textAfter;
        image.color = imageAfter;
        if (imageSpriteAfter is null) return;
        image.sprite = imageSpriteAfter;
    }
}
