using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;

public class PressGameControl : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;
    [SerializeField] private AudioManager audioManager;
    public int maxClickNum;
    public AudioClip clickSound;

    private int count;

    public float countTime;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    public void AddCount()
    {
        count += 1;
        CheckCount();
    }

    private void CheckCount()
    {
        if (clickSound != null)
            audioManager.PlaySFX(clickSound);


        if (count == maxClickNum)
        {
            StartCoroutine(investigationControl.EndInvestigation());
        }
    }
}
