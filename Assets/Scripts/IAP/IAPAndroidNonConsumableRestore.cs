#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


public class IAPAndroidNonConsumableRestore : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_StoreController;
    IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

    private string unlimitedProductId = "unlimitedpackage_hs2";
    private string coin20ProductId = "coin20_hs2";
    private string coin100ProductId = "coin100_hs2";

    public IAPManager iapManager;

    void Start()
    {
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Non-Consumable
        builder.AddProduct(unlimitedProductId, ProductType.NonConsumable);

        //Consumables
        builder.AddProduct(coin20ProductId, ProductType.Consumable);
        builder.AddProduct(coin100ProductId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");

        m_StoreController = controller;
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        //UpdateUI();
    }

    public void Restore()
    {
        m_GooglePlayStoreExtensions.RestoreTransactions(OnRestore);

    }

    void OnRestore(bool success, string error)
    {
        var restoreMessage = "";
        if (success)
        {
            // This does not mean anything was restored,
            // merely that the restoration process succeeded.
            restoreMessage = "Restore Successful";
            UpdateUI();
        }
        else
        {
            // Restoration failed.
            restoreMessage = $"Restore Failed with error: {error}";
        }

        Debug.Log(restoreMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;

        Debug.Log($"Processing Purchase: {product.definition.id}");

        if (!iapManager.gameControl.boughtUnlimitedPackage && product.definition.id == unlimitedProductId)
        {
            if (iapManager.unlimitedPackageBlocker != null)
                iapManager.unlimitedPackageBlocker.SetActive(true);
            iapManager.gameControl.currency += 400;
            iapManager.currencyManager.ShowCurrency();
            iapManager.gameControl.boughtUnlimitedPackage = true;
            iapManager.unlimitedButton.interactable = false;
            iapManager.unlimitedPackageText.color = new Color(1f, 0f, 0f, 128f / 255f);
        }

        if (product.definition.id == coin20ProductId)
        {
            iapManager.gameControl.currency += 20;
            iapManager.currencyManager.ShowCurrency();
        }

        if (product.definition.id == coin100ProductId)
        {
            iapManager.gameControl.currency += 100;
            iapManager.currencyManager.ShowCurrency();
        }

        iapManager.gameControl.canRestore = false;
        iapManager.restoreButton_Android.GetComponent<Button>().interactable = false;

        return PurchaseProcessingResult.Complete;
    }

    void UpdateUI()
    {
        if (HasUnlimitedPackage() && !iapManager.gameControl.boughtUnlimitedPackage)
        {
            if (iapManager.unlimitedPackageBlocker != null)
                iapManager.unlimitedPackageBlocker.SetActive(true);
            iapManager.gameControl.currency += 400;
            iapManager.currencyManager.ShowCurrency();
            iapManager.gameControl.boughtUnlimitedPackage = true;
            iapManager.unlimitedButton.interactable = false;
            iapManager.unlimitedPackageText.color = new Color(1f, 0f, 0f, 128f / 255f);

            iapManager.gameControl.canRestore = false;
            iapManager.restoreButton_Android.GetComponent<Button>().interactable = false;
        }
    }

    bool HasUnlimitedPackage()
    {
        var unlimitedProduct = m_StoreController.products.WithID(unlimitedProductId);
        return unlimitedProduct != null && unlimitedProduct.hasReceipt;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }
}
#endif