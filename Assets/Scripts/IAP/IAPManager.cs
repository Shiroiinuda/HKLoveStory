using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class IAPManager : MonoBehaviour
{
    public GameControl gameControl;
    public CurrencyManager currencyManager;

    private string unlimitedPackage = "unlimitedpackage_hs2";
    private string coin20ProductId = "coin20_hs2";
    private string coin100ProductId = "coin100_hs2";

    public GameObject restoreButton;
    public GameObject restoreButton_Android;


    public GameObject unlimitedPackageBlocker;
    public Button unlimitedButton;
    public TextMeshProUGUI unlimitedPackageText;

    private void Start()
    {
        if (restoreButton != null)
        {
#if UNITY_ANDROID
            restoreButton.SetActive(false);
#elif UNITY_IOS
                restoreButton.SetActive(true);
#endif

            if (gameControl.canRestore)
                restoreButton.GetComponent<Button>().interactable = true;
            else
                restoreButton.GetComponent<Button>().interactable = false;
        }

#if UNITY_ANDROID
        if (restoreButton_Android != null)
        {
            restoreButton_Android.SetActive(true);
            if (gameControl.canRestore)
                restoreButton_Android.GetComponent<Button>().interactable = true;
            else
                restoreButton_Android.GetComponent<Button>().interactable = false;
        }
#elif UNITY_IOS
        if (restoreButton_Android != null)
        {
            restoreButton_Android.SetActive(false);
        }
#endif
    }

    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == unlimitedPackage)
        {
            if (unlimitedPackageBlocker != null)
                unlimitedPackageBlocker.SetActive(true);
            gameControl.currency += 400;
            currencyManager.ShowCurrency();
            gameControl.boughtUnlimitedPackage = true;
            unlimitedButton.interactable = false;
            unlimitedPackageText.color = new Color(1f, 0f, 0f, 128f / 255f);
        }

        if (product.definition.id == coin20ProductId)
        {
            gameControl.currency += 20;
            currencyManager.ShowCurrency();
        }

        if (product.definition.id == coin100ProductId)
        {
            gameControl.currency += 100;
            currencyManager.ShowCurrency();
        }

        gameControl.canRestore = false;
        restoreButton.GetComponent<Button>().interactable = false;
    }

    public void OnTransactionsRestored(bool success, string error)
    {
        Debug.Log($"TransactionsRestored: {success} {error}");
        if (success)
        {
            gameControl.canRestore = false;
            restoreButton.GetComponent<Button>().interactable = false;
        }
    }
}
