using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MoveActive : MonoBehaviour
{
    public GameObject targetParentPage;
    public GameObject targetPage;
    public GameObject preciousPage;

    public bool isForward;
    public bool isLeft;
    public bool isRight;

    public List<GameObject> deactiveObj;
    public PlayableDirector bufferAnim;

    public List<GameObject> transitionObjL;
    public List<GameObject> transitionObjR;

    public void MoveGameObject()
    {
        if (isForward)
            preciousPage.GetComponent<Animator>().SetTrigger("Forward");

        bufferAnim.Play();
        StartCoroutine(MovePage());
    }

    private IEnumerator MovePage()
    {
        if (isForward)
        {
            yield return new WaitForSeconds(1);
            targetParentPage.SetActive(true);
            targetPage.SetActive(true);

            targetPage.GetComponent<Animator>().SetTrigger("FadeIn");
            yield return new WaitForSeconds(0.4f);
            foreach (GameObject obj in deactiveObj)
                obj.SetActive(false);
        }
        else if (isLeft)
        {
            if (transitionObjL != null && transitionObjL.Count > 0)
            {
                yield return new WaitForSeconds(0.3f);
                transitionObjL[0].SetActive(true);
                yield return new WaitForSeconds(0.3f);
                transitionObjL[1].SetActive(true);
                yield return new WaitForSeconds(0.3f);
                targetParentPage.SetActive(true);
                targetPage.SetActive(true);
                foreach (GameObject obj in deactiveObj)
                    obj.SetActive(false);
            }
        }
        else if (isRight)
        {
            if (transitionObjR != null && transitionObjR.Count > 0)
            {
                yield return new WaitForSeconds(0.3f);
                transitionObjR[0].SetActive(true);
                yield return new WaitForSeconds(0.3f);
                transitionObjR[1].SetActive(true);
                yield return new WaitForSeconds(0.3f);
                targetParentPage.SetActive(true);
                targetPage.SetActive(true);
                foreach (GameObject obj in deactiveObj)
                    obj.SetActive(false);
            }
        }
    }
}
