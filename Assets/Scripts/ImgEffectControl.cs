using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ImgEffectControl : MonoSingleton<ImgEffectControl>
{

    public DialogueManager dialogueManager;

    public List<PlayableDirector> timlineEffect = new List<PlayableDirector>();
    public List<GameObject> effectImg;
    public PlayableDirector directorToPlay;
    public GameObject bufferPanel;
    public GameObject skipVideoButton;
    public GameObject currencyPanel;

    public void PlayEffect(string animationName)
    {
        // Find the PlayableDirector in the list with matching name
        if(animationName != "")
            directorToPlay = timlineEffect.Find(x => x.name == animationName);

        // If the PlayableDirector with the given name is found, play it
        if (directorToPlay != null)
        {
            directorToPlay.gameObject.SetActive(true);
            directorToPlay.Play();
        }
    }

    public void ShowImgEffect(string effectName)
    {
        ActivateByName(effectName);
    }


    public void DeactivateImgEffect()
    {
        foreach (GameObject obj in effectImg)
        {
            obj.SetActive(false);
        }

    }

    public void DeactivateAnimation()
    {
        bufferPanel.SetActive(false);
        foreach (PlayableDirector playableDirector in timlineEffect)
        {
            playableDirector.gameObject.SetActive(false);
        }
    }

    public void DeactivateAll()
    {
        DeactivateImgEffect();
        DeactivateAnimation();
    }
    public void ActivateByName(string objectName)
    {
        foreach (GameObject obj in effectImg)
        {
            if (obj.name == objectName)
            {
                obj.SetActive(true);
                foreach (Transform child in obj.transform)
                {
                    if(child.GetComponent<PlayVideo>() != null)
                    {
                        currencyPanel.SetActive(false);
                    }
                }
            }
        }

        if(objectName == "CreditVideo" || objectName == "Trailer")
            skipVideoButton.SetActive(true);
        else
            skipVideoButton.SetActive(false);
    }

    public void EndAnimation()
    {
        StartCoroutine(dialogueManager.DisplayNext());
        currencyPanel.SetActive(true);
    }
}
