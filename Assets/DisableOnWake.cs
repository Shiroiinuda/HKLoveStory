using System;
using UnityEngine;

public class DisableOnWake : MonoBehaviour
{
    private void Start() => gameObject.SetActive(false);
}