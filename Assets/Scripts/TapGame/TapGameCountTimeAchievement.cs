using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;

public class TapGameCountTimeAchievement : MonoBehaviour
{
    public GameControl gameControl;

    public InvestigationControl investigationControl;
    public float maxTime;
    private float currentTime;
    public float timeDecreaseRate = 0.1f;
    
    private void Start()
    {
        currentTime = maxTime;
        StartCoroutine(DecreaseTime());
    }

    private IEnumerator DecreaseTime()
    {
        if (investigationControl.wonGame == false)
        {
            // Decrease the currentBreathTime by breathDecreaseRate
            currentTime -= timeDecreaseRate;

            // Add any additional logic or checks you need here, e.g., game over condition if currentBreathTime <= 0
            if (currentTime <= 0f)
            {
                gameControl.OnUnlockAchievement("NotGiveLunch");
            }
            else
            {
                // Wait for 0.1 seconds before decreasing breath again
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(DecreaseTime());
            }
        }
    }
}
