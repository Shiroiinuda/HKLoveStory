using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLocalzation : MonoBehaviour
{
    [AutoProperty] [ReadOnly] public Button button;
    public LocalizedString localize;

}
