using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFadeImageControl : MonoBehaviour
{
    public IEnumerator SetRayCast()
    {
        this.gameObject.GetComponent<Image>().raycastTarget = true;
        yield return new WaitForSeconds(2);
        this.gameObject.GetComponent<Image>().raycastTarget = false;
    }
}
