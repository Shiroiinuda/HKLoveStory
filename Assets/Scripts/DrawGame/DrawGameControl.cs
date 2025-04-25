using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DrawGameControl : MonoBehaviour
{
    public AudioManager audioManager;

    public Button drawByJadeBtn;
    public Button drawByADSBtn;

    public GameControl gameControl;
    public GameObject confirmPayPanel;
    public GameObject shopPanel;
    [SerializeField] private Animator NoCurrencyNotice;

    public PlayableDirector drawAnimtion;
    public Image aniamtionCard;
    public Image cardInCardViewer;

    public GameObject cardBookPanel;
    public List<GameObject> cardBookPages;

    public GameObject afterPayObject;

    [SerializeField] private CurrencyManager currencyManager;

    public List<Sprite> cardPool;

    private bool drawByJade = false;

    public AudioClip shopSfx;
    public AudioClip drawClickClickSfx;
    public AudioClip confirmYesSfx;


    private void Start()
    {
        if (cardPool.Count <= 0)
        {
            drawByJadeBtn.interactable = false;
            drawByADSBtn.interactable = false;
        }
    }

    public void OnDrawByJadeClick()
    {
        if (gameControl.currency >= 1)
        {
            audioManager.PlayButtonSFX(drawClickClickSfx);
            confirmPayPanel.SetActive(true);
        }
        else
        {
            audioManager.PlayButtonSFX(shopSfx);
            shopPanel.SetActive(true);
            NoCurrencyNotice.SetTrigger("NoCurrency");
        }
    }


    public void OnPayButtonClick()
    {
        audioManager.PlayButtonSFX(confirmYesSfx);
        drawByJade = true;
        confirmPayPanel.SetActive(false);
        afterPayObject.SetActive(true);
    }


    public void OnDrawByADSClick()
    {
        afterPayObject.SetActive(true);
    }
    
    public void DrawCard()
    {
        if (drawByJade)
        {
            gameControl.currency -= 1;
            currencyManager.ShowCurrency();
            drawByJade = false;
        }

        afterPayObject.SetActive(false);

        //
        if (cardPool.Count > 0)
        {
            int randomIndex = Random.Range(0, cardPool.Count); // Randomly select a card from the pool
            Sprite drawnCard = cardPool[randomIndex];

            if (!gameControl.cardList.Contains(drawnCard.name.Trim()))
            {
                gameControl.cardList.Add(drawnCard.name); // Add the card to the card list
                Debug.Log("Card drawn: " + drawnCard.name);
            }

            aniamtionCard.sprite = drawnCard;
            cardInCardViewer.sprite = drawnCard;

            drawAnimtion.gameObject.SetActive(true);
            drawAnimtion.Play();
        }
    }


    public void OnAnimationEnd()
    {
        if (gameControl.cardList.Count == 100)
            gameControl.OnUnlockAchievement("DrawAllCards");
        drawAnimtion.gameObject.SetActive(false);
    }

    public void OnCardBookClicked()
    {
        foreach (GameObject page in cardBookPages)
        {
            if (page.name == "Page1Panel")
                page.SetActive(true);
            else
                page.SetActive(false);
        }

        cardBookPanel.SetActive(true);
    }

}
