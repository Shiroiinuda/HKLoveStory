using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCardManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip lockSFX;
    [SerializeField] private AudioClip clickSFX;

    [SerializeField] private Sprite cardsprite;
    [SerializeField] private Sprite cardLock;
    [SerializeField] private GameObject cardViewer;
    [SerializeField] private Image card_CardViewer;
    private bool cardUnlocked;


    // Start is called before the first frame update
    void Start()
    {
        updateCard();
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnCardClicked);
    }

    private void OnEnable()
    {
        updateCard();
    }

    private void updateCard()
    {
        string unLockedCG = GameControl.instance.cardList.Find(obj => obj == cardsprite.name.Trim());
        if (unLockedCG != null)    //CG unlocked
        {
            this.gameObject.GetComponent<Image>().sprite = cardsprite;
            cardUnlocked = true;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = cardLock;
            cardUnlocked = false;
        }
    }

    public void OnCardClicked()
    {
        if (cardUnlocked)
        {
            audioManager.PlayButtonSFX(clickSFX);
            cardViewer.SetActive(true);
            card_CardViewer.GetComponent<Image>().sprite = cardsprite;
        }
        else
        {
            audioManager.PlayButtonSFX(lockSFX);
        }
    }
}
