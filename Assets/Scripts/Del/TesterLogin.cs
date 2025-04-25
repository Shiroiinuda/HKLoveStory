using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TesterLogin : MonoBehaviour
{
    public GameControl gameControl;

    private string testerLoginPW = "GSTester";
    public TMP_InputField pwInput;
    public GameObject tickImg;
    public GameObject crossImg;

    public void OnEnterClicked()
    {
        if (pwInput.text.Trim() == testerLoginPW)
        {
            pwInput.text = "";
            gameControl.isTester = true;
            tickImg.SetActive(true);
            crossImg.SetActive(false);
        }
        else
        {
            pwInput.text = "";
            crossImg.SetActive(true);
            tickImg.SetActive(false);
        }

        if (pwInput.text.Trim() == "ResetTesterBool")
        {
            pwInput.text = "";
            gameControl.isTester = false;
            tickImg.SetActive(true);
            crossImg.SetActive(false);
        }
    }

}
