using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandpaSpeakControl : MonoBehaviour
{
    public AudioManager audioManager;

    public List<AudioClip> grandpaTalks;

    public void OnGrandpaClick()
    {
        if (grandpaTalks != null)
        {
            if(grandpaTalks.Count > 0)
            {
                int randomIndex = Random.Range(0, grandpaTalks.Count);
                audioManager.PlayCSound(grandpaTalks[randomIndex]);
            }
        }
    }
}
