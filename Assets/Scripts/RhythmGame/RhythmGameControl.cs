using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Investigation
{
    public class RhythmGameControl : MonoBehaviour
    {
        public AudioManager audioManager;
        public int rounds;
        public List<AudioClip> rhythmSound;
        public Button rhythmButton;
        public float beatInterval = 1f;
        public int targetScore = 20;

        public GameObject hand1;
        public GameObject hand2;

        private int score = 0;
        private bool canPress = false; // Only allows pressing on rhythm
        private float nextBeatTime = 0f;

        // Start is called before the first frame update
        void Start()
        {
            // Start the rhythm loop
            StartCoroutine(PlayRhythm());

            // Subscribe to button click event
            rhythmButton.onClick.AddListener(OnButtonPress);
        }

        IEnumerator PlayRhythm()
        {
            while (score < targetScore)
            {
                int randomIndex = Random.Range(0, rhythmSound.Count);
                AudioClip randomClip = rhythmSound[randomIndex];

                canPress = true; // Allow button press
                nextBeatTime = Time.time + beatInterval;

                while (audioManager.SFXSource.isPlaying)
                {
                    yield return null; // Wait until the next frame to check again
                }

                // Once the sound ends, disable button press
                canPress = false;

                // Wait for the beat interval
                yield return new WaitForSeconds(beatInterval);
            }
        }

        void OnButtonPress()
        {
            if (!canPress)
            {
                LoseGame();
                return;
            }

            score++;

            if (score >= targetScore)
            {
                WinGame();
            }


            bool activeFirstHand = hand2.activeSelf;

            hand1.SetActive(activeFirstHand);
            hand2.SetActive(!activeFirstHand);
        }

        void LoseGame()
        {
            StopAllCoroutines();
            Debug.Log("You lose! You missed the rhythm.");
            // Add additional lose logic here (e.g., restart, show message)
        }

        void WinGame()
        {
            StopAllCoroutines();
            Debug.Log("You win! You matched the rhythm correctly 20 times.");
            // Add additional win logic here (e.g., load next level, show message)
        }
    }
}