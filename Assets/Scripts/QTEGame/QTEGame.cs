using System.Collections;
using Investigation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class QTEGame : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject gameArea;
    public int numberOfButtons = 5;
    public int requiredMarks = 4;
    public int marks;
    public int clickCount;

    public int missMark;

    private bool gameEnded;

    public bool haveGameOver;
    public bool isIcePickGame;
    public bool isFakeArgueQTE;

    public PlayableDirector gameOverEffect;

    private int winJumpMark;
    private int loseJumpMark;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InvestigationControl investigationControl;
    public AudioManager audioManager;

    public AudioClip spwanBtnSfx;
    public AudioClip winSound;
    public AudioClip missSFX;
    public AudioClip correctSFX;

    private const string PPrefArgue = "DialogueArgueScore";

    private void Start()
    {
        marks = 0;
        if (dialogueManager.currentDialogue.roads.road1 != "")
            winJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);

        if (dialogueManager.currentDialogue.roads.road2 != "")
            loseJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);
        StartCoroutine(StartQTEGame());
    }

    private IEnumerator StartQTEGame()
    {
        gameEnded = false;
        float screenWidth = 1920;
        float screenHeight = 1080;
        screenWidth = screenWidth / 2 * 0.8f -buttonPrefab.GetComponent<RectTransform>().rect.width;
        screenHeight = screenHeight / 2 * 0.8f - buttonPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < numberOfButtons; i++)
        {
            // Randomly determine position within game area
            Vector2 randomPosition = new Vector2(
                Random.Range(-screenWidth, screenWidth),
                Random.Range(-screenHeight, screenHeight));

            // Instantiate button prefab at the random position
            GameObject button = Instantiate(buttonPrefab, gameArea.transform.position, Quaternion.identity, gameArea.transform);
            if(audioManager != null && spwanBtnSfx != null)
                audioManager.PlaySFX(spwanBtnSfx);
            Debug.Log($"{randomPosition.x}, {randomPosition.y}");
            button.GetComponent<RectTransform>().anchoredPosition = randomPosition;

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }

        while (!gameEnded)
        {
            yield return null;
        }


    }

    public void CheckClickCount()
    {
        if (isIcePickGame && missMark >= (numberOfButtons - requiredMarks))
        {
            dialogueManager.jumpMark = loseJumpMark;
            StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
        }

        if (clickCount >= numberOfButtons)
        {
            gameEnded = true;
            if (isIcePickGame)
            {
                dialogueManager.jumpMark = winJumpMark;
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
            }
            else
                CheckWinLose();
        }
    }

    private void CheckWinLose()
    {
        if (investigationControl.wonGame == false)
        {
            if (haveGameOver)
            {
                // Count marks and determine game result
                //int requiredMarks = Mathf.CeilToInt(numberOfButtons / 2f);

                if (marks >= requiredMarks)
                {
                    int resultJump = winJumpMark;
                    dialogueManager.jumpMark = resultJump;
                    StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                }
                else
                {
                    investigationControl.endGameBlocker.SetActive(true);
                    if (gameOverEffect != null)
                    {
                        gameOverEffect.gameObject.SetActive(true);
                        gameOverEffect.Play();
                    }
                }
            }
            else
            {
                // Count marks and determine game result
                //int requiredMarks = Mathf.CeilToInt(numberOfButtons / 2f);

                if (marks >= requiredMarks)
                {
                    if (audioManager != null && winSound != null)
                        audioManager.PlaySFX(winSound);
                }

                int resultJump = (marks >= requiredMarks) ? winJumpMark : loseJumpMark;
                int resultscore = (marks >= requiredMarks) ? 0 : -1;
                Debug.Log("resultscore = " + resultscore);

                PlayerPrefs.SetInt(PPrefArgue, PlayerPrefs.GetInt(PPrefArgue) + resultscore);

                Debug.Log("resultscore: PPrefArgue = " + PlayerPrefs.GetInt(PPrefArgue));

                Debug.Log("resultJumpmark = " + resultJump);

                dialogueManager.jumpMark = resultJump;
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
            }
        }
    }
}
