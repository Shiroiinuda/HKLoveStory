using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Investigation
{
    public class EngGameControl : MonoBehaviour
    {
        [SerializeField] private List<Password> pwClassList;
        [SerializeField] private List<Button> pwButtonList;
        [SerializeField] private string goalPassword;

        public bool needMoveAnim;
        public RectTransform moveObj;
        public float moveXDoorValue;
        public float moveYDoorValue;

        public float moveXDoorSec;
        private const string openSfx = "Sliding Door 01 Open";

        private void Start()
        {
            InitLock();
        }

        private void InitLock()
        {
            for (int i = 0; i < pwClassList.Count; i++)
            {
                int index = i;
                pwClassList[i].Init();
                pwButtonList[i].onClick.AddListener(() => pwClassList[index].CycleLetter());
                pwButtonList[i].onClick.AddListener(() => CheckUnlock());
            }
        }

        private bool CheckCorrectPW()
        {
            // Combine current letters and compare to target
            string currentPassword = string.Join("", pwClassList.ConvertAll(p => p.GetCurrentLetter()));
            return currentPassword == goalPassword;
        }

        private void CheckUnlock()
        {
            if (!CheckCorrectPW()) return;
            if (needMoveAnim)
            {
                moveObj.DOAnchorPos(new Vector2(moveXDoorValue, moveYDoorValue), moveXDoorSec);
            }

            SoundControl.SoundManager.PlaySfx(openSfx);

            gameObject.SetActive(false);

        }
    }


    [System.Serializable]
    public class Password
    {
        public List<char> providedChar;
        public TextMeshProUGUI pwText;
        public int currentNum = 0;
        private const string sfx = "turning_a_lock3";

        public Password()
        {

        }
        public void Init()
        {
            currentNum = 0;
            UpdateDisplay();
        }

        public void CycleLetter()
        {
            SoundControl.SoundManager.PlaySfx(sfx);

            currentNum = (currentNum + 1) % providedChar.Count;
            UpdateDisplay();
        }

        public char GetCurrentLetter()
        {
            return providedChar[currentNum];
        }

        private void UpdateDisplay()
        {
            Debug.Log($"{pwText} , {providedChar[currentNum]} ");
            pwText.text = providedChar[currentNum].ToString();
        }
    }
}
