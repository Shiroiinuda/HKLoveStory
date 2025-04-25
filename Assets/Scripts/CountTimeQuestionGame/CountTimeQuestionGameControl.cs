using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;
using DG.Tweening;

namespace Investigation
{
    public class CountTimeQuestionGameControl : MonoBehaviour
    {
        [Header("Script")]
        public InvestigationControl investigationControl;
        public DialogueManager dialogueManager;

        [Header("Game Objects")]
        public GameObject dialogueDataPrefab;
        public List<GameChoice> gameChoiceList;

        [Header("Variables")]
        public float maxTime = 30;
        private bool gameEnd;

        [Header("UIs")]
        public RectTransform timeSlider;

        private const string sfx = "Menu Click";

        private void OnEnable()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            gameEnd = false;

            setChoicesValue();

            //Reset Time Slider
            
            timeSlider.localScale = new Vector3(1,1,1);
            timeSlider.DOScaleX(0, maxTime);

            //Start Count Time
            Invoke("TimesUpJump", maxTime);
        }

        private void OnChoiceButtonClick(int choiceNim)
        {
            if (!gameEnd)
            {
                dialogueManager.jumpMark = gameChoiceList[choiceNim].jumpMark;
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                CancelInvoke("TimesUpJump");
                DOTween.Kill(timeSlider);
                gameEnd = true;
            }
        }

        private void TimesUpJump()
        {
            if (!gameEnd)
            {
                dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road3);
                StartCoroutine(investigationControl.EndInvestigationThatJumpItself());
                CancelInvoke("TimesUpJump");
                DOTween.Kill(timeSlider);
                gameEnd = true;
            }
        }

        private void setChoicesValue()
        {
            //Set Choices Values
            int bookmarkNum = dialogueManager.currentDialogue.bookMark + 1;

            for (int i = 0; i < gameChoiceList.Count; i++)
            {
                var dialogueData = dialogueDataPrefab.GetComponent<DialogueData>().dialogues[bookmarkNum];

                if (dialogueData.speakerID >= 997 && dialogueData.speakerID <= 998)
                {
                    gameChoiceList[i].choiceButton.transform.parent.gameObject.SetActive(true);

                    int choiceJumpMark;

                    if (i == 0)
                        choiceJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);
                    else
                        choiceJumpMark = int.Parse(dialogueManager.currentDialogue.roads.road2);

                    string ansSentence = LocalizationManager.GetTranslation($"Dialogue/{dialogueData.bookMark}");

                    gameChoiceList[i].Init(ansSentence, choiceJumpMark);
                }
                else
                    gameChoiceList[i].choiceButton.transform.parent.gameObject.SetActive(false);

                bookmarkNum++;
            }

            foreach (GameChoice gameChoice in gameChoiceList)
            {
                gameChoice.choiceButton.onClick.RemoveAllListeners();
                gameChoice.choiceButton.onClick.AddListener(() => OnChoiceButtonClick(gameChoice.choiceNum));
            }

        }
    }

    [System.Serializable]
    public class GameChoice
    {
        public int choiceNum;
        public Button choiceButton;
        public TextMeshProUGUI ansText;
        public int jumpMark = 0;

        public GameChoice()
        {
        }
        public void Init(string ans, int num)
        {
            ansText.text = ans;
            jumpMark = num;
        }
    }
}
