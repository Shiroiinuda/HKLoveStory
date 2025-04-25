using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuConditions : MonoBehaviour
{
    public int condition;
    private int currentCondition;
    public UnityEvent doneAction;
    public void CheckDone()
    {
        currentCondition++;
        if (condition == currentCondition)
        {
            doneAction.Invoke();
        }
    }
}
