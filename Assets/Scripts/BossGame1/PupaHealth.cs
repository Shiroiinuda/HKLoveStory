using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupaHealth : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private BossGameControl bossGameControl;

    public float maxHealth = 10;
    private float currentHealth;

    [SerializeField]private Vector3 targetScale;

    [SerializeField] private float reduceValue;

    [SerializeField] private ParticleSystem waterparticle;

    //SFXs
    [SerializeField]
    private List<AudioClip> waterSound;

    [SerializeField]
    private AudioClip pupaDamage;

    private Animator pupaVibrate;

    public GameObject waterSpray;

    public GameObject pupaGameObj;

    private void Start()
    {
        currentHealth = maxHealth;
        reduceValue = 1 / maxHealth;
        pupaVibrate = pupaGameObj.GetComponent<Animator>();
    }

    public void OnPupaClicked()
    {
        int waterNum = Random.Range(0, waterSound.Count);

        if (waterSound.Count != 0)
            audioManager.PlaySFX(waterSound[waterNum]);

        pupaVibrate.ResetTrigger("Vibrate");
        pupaVibrate.SetTrigger("Vibrate");

        if (currentHealth > 0 && currentHealth <= maxHealth)
        {
            //pupaAnimator.SetTrigger("Shake");
            currentHealth -= 1;

            RotateWaterSpray();

            targetScale = pupaGameObj.transform.localScale;
            targetScale.x -= reduceValue;
            targetScale.y -= reduceValue;

            StartCoroutine(ReduceScale());
        }

        if (currentHealth <= 0)
        {
            if (pupaDamage != null)
                audioManager.PlaySFX(pupaDamage);
            bossGameControl.currentNum += 1;
            this.gameObject.SetActive(false);
            bossGameControl.CheckWin();
        }

        waterparticle.Play();
    }

    private IEnumerator ReduceScale()
    {
        Vector3 currentScale = pupaGameObj.transform.localScale;

        if(currentScale != targetScale)
        {
            currentScale.x -= reduceValue;
            currentScale.y -= reduceValue;

            // Make sure the scale doesn't go below 0
            currentScale.x = Mathf.Max(0f, currentScale.x);
            currentScale.y = Mathf.Max(0f, currentScale.y);

            pupaGameObj.transform.localScale = currentScale;

            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ReduceScale());
        }

    }

    private void RotateWaterSpray()
    {
        if (currentHealth % 2 == 1)
        {
            waterSpray.SetActive(true);
            RotateObject(-20f);
        }
        else
        {
            waterSpray.SetActive(true);
            RotateObject(20f);
        }

        // Reset the state after 0.5 seconds
        Invoke("ResetState", 0.5f);
    }

    private void RotateObject(float angle)
    {
        Vector3 rotation = waterSpray.transform.rotation.eulerAngles;
        rotation.z += angle;
        waterSpray.transform.rotation = Quaternion.Euler(rotation);
    }

    private void ResetState()
    {
        waterSpray.SetActive(false);
    }
}
