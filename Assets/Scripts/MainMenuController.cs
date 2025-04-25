using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Separator("Window only")] public List<Button> windowBtn;
    [Separator("Mobile only")] public List<Button> mobileBtn;
    [Separator("MainMenu Stuff")]
    public List<GameObject> mainMenuList;
    public GameControl gameControl;
    public GameObject Loadpanel;
   // public Button zStore;

    private void Start()
    {
        ChangeMainMenu();
//            zStore.gameObject.SetActive(LocalizationManager.CurrentLanguage != "English");
    }

    private void ChangeMainMenu()
    {
        if (mainMenuList.Count != 0)
        {
            ActivateGameObject(gameControl.mainMenuBG.Trim());
        }

        ;
#if UNITY_STANDALONE_WIN
        foreach (var objects in windowBtn)
        {
            objects.gameObject.SetActive(true);
        }
        foreach (var objects in mobileBtn)
        {
            objects.gameObject.SetActive(false);
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
        foreach (var objects in windowBtn)
        {
            objects.gameObject.SetActive(false);
        }
        foreach (var objects in mobileBtn)
        {
            objects.gameObject.SetActive(true);
        }
#endif
    }

    private void ActivateGameObject(string targetName)
    {
        foreach (GameObject obj in mainMenuList)
        {
            if (obj.name == targetName)
            {
                obj.SetActive(true);
            }
        }
    }
}