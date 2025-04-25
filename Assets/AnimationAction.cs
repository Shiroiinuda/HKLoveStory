using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationAction : MonoBehaviour
{
    public UnityEvent events;
    public void Endevent() => events.Invoke();
}
