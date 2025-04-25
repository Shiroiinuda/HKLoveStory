using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonCycleAnimators : MonoBehaviour
{
    public Button button;
    public List<Animator> animators;
    public string triggerName = "Trigger";

    private int currentAnimatorIndex = 0;

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (animators != null && animators.Count > 0)
        {
            animators[currentAnimatorIndex].SetTrigger(triggerName);
            currentAnimatorIndex = (currentAnimatorIndex + 1) % animators.Count;
        }
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}
