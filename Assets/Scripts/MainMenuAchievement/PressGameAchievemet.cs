using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressGameAchievemet : MonoBehaviour
{
    public AudioManager audioManager;
    public GameControl gameControl;

    private int needPress = 5;
    [SerializeField]private int countPress;
    public List<AudioClip> kwanPressSound;

    private void Start()
    {
        countPress = 0;
    }

    public void OnKwanPressed()
    {
        if (countPress <= needPress-1)
        {
            if (kwanPressSound != null && kwanPressSound.Count == needPress)
                audioManager.PlayCSound(kwanPressSound[countPress]);
            countPress += 1;
        }

        if(countPress == needPress)
        {
            gameControl.OnUnlockAchievement("NotHelpKwan");
        }
    }
}
