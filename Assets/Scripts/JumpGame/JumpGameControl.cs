using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;
using UnityEngine.UI;


public class JumpGameControl : MonoBehaviour
{
    public AudioManager audioManager;
    public InvestigationControl investigationControl;
    public Collider2D targetCollider;
    public Collider2D myCollider;

    public int maxCount = 3;
    [SerializeField] private int count;

    public Animator jumpAnimation;

    [SerializeField]
    private Animator miisAnim;

    [SerializeField] private GameObject pos1;
    [SerializeField] private GameObject pos2;
    [SerializeField] private GameObject pos3;
    [SerializeField] private Vector3 targetPos;

    public List<AudioClip> jumpSound;
    public AudioClip missSound;

    //skip
    [SerializeField] private GameObject skipGameObg;
    [SerializeField] private int totalMiss;

    [SerializeField]
    private Button jumpButton;

    private void Start()
    {
        count = 0;
        totalMiss = 0;
        targetPos = pos1.transform.position;
        targetCollider.transform.position = targetPos;
    }

    public void onJumpClicked()
    {
        
        bool isInside = IsObjectInsideTarget();

        // Do something based on the result
        if (isInside)
        {
            count += 1;

            if(count == 1)
            {
                jumpAnimation.SetTrigger("Jump1");
                targetPos = pos2.transform.position;
                targetCollider.transform.position = targetPos;
            }
            else if(count == 2)
            {
                jumpAnimation.SetTrigger("Jump2");
                targetPos = pos3.transform.position;
                targetCollider.transform.position = targetPos;
            }
            else
            {
                jumpButton.enabled = false;
                jumpAnimation.SetTrigger("Jump3");
            }

            if (jumpSound != null)
            {
                int clickSoundNum = Random.Range(0, jumpSound.Count);
                audioManager.PlaySFX(jumpSound[clickSoundNum]);
            }
        }
        else
        {
            totalMiss += 1;
            if (totalMiss >= 10)
                skipGameObg.SetActive(true);

            count = 0;
            audioManager.StopSfx();
            audioManager.PlaySFX(missSound);
            targetPos = pos1.transform.position;
            targetCollider.transform.position = targetPos;
            miisAnim.SetTrigger("Miss");
        }
    }

    private bool IsObjectInsideTarget()
    {
        // Create bounding boxes for each object
        Bounds targetBounds = targetCollider.bounds;
        Bounds thisBounds = myCollider.bounds;

        // Check if this object's bounding box is fully contained inside the targetObject's bounding box
        return targetBounds.Contains(thisBounds.min) && targetBounds.Contains(thisBounds.max);
    }

    public void EndGame()
    {
        audioManager.StopSfx();
        StartCoroutine(investigationControl.EndInvestigation());
    }
}
