using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSelfCount : MonoBehaviour
{
    [SerializeField] private CountItems countItems;

    private bool clicked;

    // Start is called before the first frame update
    void Start()
    {
        clicked = false;
    }

    public void OnItemClicked()
    {
        if (clicked == false)
        {
        //    countItems.CheckItemsNum();
            clicked = true;
        }
    }
}
