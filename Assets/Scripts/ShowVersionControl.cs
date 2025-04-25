using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowVersionControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "Version: v" + Application.version;
    }
}
