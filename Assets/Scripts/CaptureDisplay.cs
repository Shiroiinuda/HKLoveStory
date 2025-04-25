using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CaptureDisplay : MonoSingleton<CaptureDisplay>
{
    public List<GameObject> objectToHide;
    public Coroutine ScreenShot;
    public void CaptureScreens(int number)
    {
        ScreenShot =  StartCoroutine(CaptureScreen(number));
    }
    
    public IEnumerator CaptureScreen(int num)
    {
        
        // Capture the entire screen
        var directoryPath = $"{Application.persistentDataPath}/Screenshots";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        var filePath = $"{directoryPath}/SaveScreenshot{num}.png";
        
        // Hide the object before capturing the screenshot
        if (objectToHide != null)
        {
            foreach (GameObject objs in objectToHide)
                objs.SetActive(false);
        }

        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log("Screenshot saved at: " + filePath);

        yield return new WaitForSeconds(0.025f);
        // Hide the object before capturing the screenshot
        if (objectToHide == null) yield break;
        {
            foreach (GameObject objs in objectToHide)
                objs.SetActive(true);
        }
        yield return new WaitForSeconds(0.025f);
        if (ScreenShot == null) yield return null;
        ScreenShot = null;
    }
}
