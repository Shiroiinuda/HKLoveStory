using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterMobileDisable : MonoBehaviour
{
    public List<GameObject> engLetterList;

    // Start is called before the first frame update

    void Start()
    {
#if !(UNITY_ANDROID || UNITY_IOS)
    foreach (GameObject engLetter in engLetterList)
            engLetter.SetActive(true);
#else
    foreach (GameObject engLetter in engLetterList)
            engLetter.SetActive(false);
#endif
    }
}
