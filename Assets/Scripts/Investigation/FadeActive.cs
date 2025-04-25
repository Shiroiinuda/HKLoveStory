using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class FadeActive : MonoBehaviour
{
    public GameObject fadeInObj;
    public List<GameObject> fadeOutObj;
    public PlayableDirector fadeAnim;


    // Start is called before the first frame update

    public void fadeGameObject()
    {
        fadeAnim.Play();
        StartCoroutine(fadeObj());
    }

    private IEnumerator fadeObj()
    {
        yield return new WaitForSeconds(1);
        fadeInObj.SetActive(true);
        foreach (GameObject obj in fadeOutObj)
            obj.SetActive(false);
    }
}
