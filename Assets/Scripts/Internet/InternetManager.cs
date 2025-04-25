using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InternetManager : MonoBehaviour
{
    public string userID;
    public string userPW;

    public TMP_InputField inputID;
    public TMP_InputField inputPW;

    public GameControl gameControl;
    public LoadSceneManager loadSceneManager;

    private void Start()
    {
        inputID.text = userID;
    }

    private void OnEnable()
    {
        inputPW.text = "";
    }

    public void OnEnterClick()
    {
        if(inputPW.text == userPW)
        {
            inputPW.text = "";
            gameControl.currentBookmark = 5293;
            loadSceneManager.FadeToLevelWithoutLoadingScreen(3);
        }
        else
            inputPW.text = "";
    }

}
