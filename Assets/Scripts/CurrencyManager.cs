using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public GameControl gameControl;

    private void Start()
    {
        ShowCurrency();
    }

    public void ShowCurrency()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = gameControl.currency.ToString();
    }

}
