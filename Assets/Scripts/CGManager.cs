using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class CGManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip lockSFX;
    [SerializeField] private AudioClip clickSFX;

    [SerializeField] private string cgName;
    [SerializeField] private Sprite cgLock;
    [SerializeField] private GameObject cgLockObj;
    [SerializeField] private GameObject cgViewer;
    [SerializeField] private Image cg_CGViewer;
    public Button cgBtn;
    private bool cgUnlocked;


    // Start is called before the first frame update
    void Start()
    {
        updateCG();
    }

    private void OnEnable()
    {
        cgBtn.onClick.AddListener(()=>OnCGClicked());
        updateCG();
    }

    private void OnDisable()
    {
        cgBtn.onClick.RemoveListener(()=>OnCGClicked());
    }

    private void updateCG()
    {
        string unLockedCG = null;
        if(GameControl.instance)
            unLockedCG = GameControl.instance.CG.Find(obj => obj == cgName);
        if (unLockedCG != null)    //CG unlocked
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"CG/ChengWing2/{cgName}");
            cgLockObj.SetActive(false);
            cgUnlocked = true;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = cgLock;
            cgLockObj.SetActive(true);
            cgUnlocked = false;
        }
    }

    public void OnCGClicked()
    {
        if (cgUnlocked)
        {
            audioManager.PlayButtonSFX(clickSFX);
            cgViewer.SetActive(true);
            cg_CGViewer.GetComponent<Image>().sprite = Resources.Load<Sprite>($"CG/ChengWing2/{cgName}");
        }
        else
        {
            audioManager.PlayButtonSFX(lockSFX);
        }
    }
}