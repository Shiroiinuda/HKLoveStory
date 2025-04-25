using System.Collections;
using System.Collections.Generic;
using EasierLocalization;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ShopManager : MonoBehaviour
{
    public GameControl gameControl;
    public AudioManager audioManager;
    public AudioClip smashselect;
    public GameObject infoPanel;

    //Refund
    private int tapTitleCount = 0;
    public GameObject refundPanel;

    public GameObject unlimitedPackageBlocker;
    public Button unlimitedPackageButton;
    public TextMeshProUGUI unlimitedPackageText;

    public TextMeshProUGUI adsRemainText;

    private void OnEnable()
    {
        
        UpdateAdsRestText();
        if (gameControl.boughtUnlimitedPackage)
        {
            unlimitedPackageBlocker.SetActive(true);
            unlimitedPackageButton.interactable = false;
            unlimitedPackageText.color = new Color(1f, 0f, 0f, 128f / 255f);
        }
        else
        {
            unlimitedPackageBlocker.SetActive(false);
            unlimitedPackageButton.interactable = true;
            unlimitedPackageText.color = new Color(255, 0, 0, 255);
        }
    }

    public void OnInfoClicked()
    {
        audioManager.PlayButtonSFX(smashselect);
        infoPanel.SetActive(true);
    }

    public void OnBackClicked(GameObject closeObj)
    {
        AudioClip backbutton = Resources.Load<AudioClip>("Sounds/sfx/backbutton");
        audioManager.PlayButtonSFX(backbutton);

        closeObj.SetActive(false);
    }

    public void OnTitleClicked()
    {
        tapTitleCount += 1;

        if(tapTitleCount >= 10)
        {
            refundPanel.SetActive(true);
            tapTitleCount = 0;
        }
    }

    public void UpdateAdsRestText()
    {
        Localization.ParametersText(adsRemainText,"TIMES", (10 - gameControl.todayAdsCount).ToString());
    }
}
