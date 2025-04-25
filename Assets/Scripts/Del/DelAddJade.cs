using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelAddJade : MonoBehaviour
{
    public GameControl gameControl;
    public CurrencyManager currencyManager;

    public void ADDJade()
    {
        gameControl.currency += 100;
        currencyManager.ShowCurrency();
    }
}
