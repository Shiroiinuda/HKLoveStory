using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Investigation
{
    public class OpenEyeGameControl : MonoBehaviour
    {
        [Header("Scripts")]
        public InvestigationControl investigationControl;

        public Image bar;
        public RectTransform leftEye, rightEye;
        public Button shoutButton;

        public float barDecreaseSpeed = 0.2f;
        public float barIncreaseAmount = 0.05f;

        private float barValue = 0f;
        private Vector2 leftStartPos, rightStartPos;
        private Vector2 leftTargetPos, rightTargetPos;
        private bool gameRunning = true; // To check if the game is still running

        void Start()
        {
            leftStartPos = leftEye.anchoredPosition;
            rightStartPos = rightEye.anchoredPosition;

            leftTargetPos = leftStartPos + new Vector2(-1250, -730);
            rightTargetPos = rightStartPos + new Vector2(1250, 730);

            shoutButton.onClick.AddListener(Shout);
            StartCoroutine(DecreaseBarOverTime());
        }

        IEnumerator DecreaseBarOverTime()
        {
            while (gameRunning)
            {
                if (barValue > 0)
                {
                    barValue -= barDecreaseSpeed * Time.deltaTime;
                    bar.fillAmount = barValue;
                    UpdateEyePosition();
                }
                yield return null;
            }
        }

        void UpdateEyePosition()
        {
            float progress = bar.fillAmount;
            leftEye.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, progress);
            rightEye.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, progress);
        }

        void Shout()
        {
            if (gameRunning)
            {
                barValue += barIncreaseAmount;
                barValue = Mathf.Clamp(barValue, 0f, 1f);
                bar.fillAmount = barValue;
                UpdateEyePosition();
                if (barValue >= 1)
                    WinGame();
            }
        }

        private void WinGame()
        {
            if (!gameRunning) return;

            gameRunning = false;
            StartCoroutine(investigationControl.EndInvestigation());
        }
    }
}