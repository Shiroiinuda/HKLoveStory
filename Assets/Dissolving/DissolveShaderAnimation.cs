using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DissolveShaderAnimation : MonoBehaviour
{
    public float dissolveDuration;

    [Range(0, 1)]
    public float dissolvePercentage;
    [Range(0, 1)]
    public int dissolveDefault = 1;

    public bool auto;
    private SpriteRenderer image;
    private Material material;

    private Coroutine dissolveCoroutine;

   public void OnEnable()
    {
        image = GetComponent<SpriteRenderer>();
        material = Instantiate(image.material);
        image.material.CopyPropertiesFromMaterial(material);

        image.material = material;

        if (auto)
        {
            StartDisolve();
        }
    }

   public void StartDisolve()
   {
       if (dissolveDefault == 0)
       {
           dissolveCoroutine = StartCoroutine(Countup(dissolveDuration));
       }
       else
       {
           dissolveCoroutine = StartCoroutine(Countdown(dissolveDuration));
       }
   }
    IEnumerator Countdown(float inputTime)
    {
        float normalizedTime = dissolveDefault; //1

        while (normalizedTime > 0)
        {
            normalizedTime -= Time.deltaTime / inputTime; 
            material.SetFloat("_Level", normalizedTime);
            yield return null; // Wait for the next frame
        }
        
        material.SetFloat("_Level", dissolvePercentage);
    }
    IEnumerator Countup(float inputTime)
    {
        float normalizedTime = dissolveDefault;//0

        while (normalizedTime < 1)
        {
            Debug.Log(normalizedTime);
            normalizedTime += Time.deltaTime / inputTime; 
            material.SetFloat("_Level", normalizedTime);
            yield return null; // Wait for the next frame
        }
        
        material.SetFloat("_Level", dissolvePercentage);
    }
    void OnDestroy()
    {
        Destroy(material);
    }
}