using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DOTweenAnimation : MonoBehaviour
{
    public RectTransform moveObj;

    public float originalDoorValueX;

    public float moveXDoorValue;
    public float moveYDoorValue;
    public float moveXDoorSec;
    public bool doorOpen;

    public bool dontReopen = false;
    public AudioManager audioManager;
    public AudioClip openSFX;
    public AudioClip closeSFX;

    private void Start()
    {
        originalDoorValueX = moveObj.anchoredPosition.x;
        Debug.Log("originalDoorValueX = " + originalDoorValueX);
    }

    public void ClickMoveXDoor()
    {
        if (dontReopen)
        {
            moveObj.DOAnchorPos(new Vector2(moveXDoorValue, moveYDoorValue), moveXDoorSec);
            this.gameObject.SetActive(false);
        }
        else
        {
            if (!doorOpen)
            {
                moveObj.DOAnchorPos(new Vector2(moveXDoorValue, moveYDoorValue), moveXDoorSec);
                if (openSFX != null)
                    audioManager.PlaySFX(openSFX);
                doorOpen = true;
            }
            else
            {
                moveObj.DOAnchorPos(new Vector2(originalDoorValueX, moveYDoorValue), moveXDoorSec);
                if (closeSFX != null)
                    audioManager.PlaySFX(closeSFX);
                doorOpen = false;
            }
        }
    }
}
