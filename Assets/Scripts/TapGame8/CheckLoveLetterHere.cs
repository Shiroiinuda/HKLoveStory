using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLoveLetterHere : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;

    // Start is called before the first frame update
    private void OnEnable()
    {
            if (!gameControl.items.Contains("LoveLetter") && gameControl.items.Contains("HeartRoomKey"))
                this.gameObject.SetActive(true);
            else
                this.gameObject.SetActive(false);
    }
}
