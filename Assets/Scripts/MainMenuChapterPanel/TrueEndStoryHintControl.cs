using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueEndStoryHintControl : MonoBehaviour
{
    public GameControl gameControl;
    public GameObject trueStoryHintObj;
    public GameObject trueEndHintObj;

    private void Start()
    {
        checkTruthHintEnable();
    }

    public void checkTruthHintEnable()
    {
        if (gameControl.stageClear.Contains("End4") || gameControl.stageClear.Contains("End5") || gameControl.stageClear.Contains("End6") || gameControl.stageClear.Contains("End7"))
        {
            if (gameControl.stageClear.Contains("Truth"))
            {
                trueStoryHintObj.SetActive(false);
                trueEndHintObj.SetActive(true);
            }
            else
            {
                trueStoryHintObj.SetActive(true);
                trueEndHintObj.SetActive(false);
            }
        }
        else
        {
            trueStoryHintObj.SetActive(false);
            trueEndHintObj.SetActive(false);
        }
    }
}
