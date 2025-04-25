using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EasierLocalization;
using MyBox;
using SoundControl;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuCharacters : MonoBehaviour,IMainMenuInteraction
    {
        public GameObject talkCloud;
        public TextMeshPro textMeshProText;
        private Coroutine dialogueCoroutine;
        public bool random;
        [ConditionalField(nameof(random))]
        public DialogueWarpper randomDialogue;
        [ConditionalField(nameof(random),true)]
        public DialogueWarpper fixedDialogue;
        [ConditionalField(nameof(random),true)]
        public bool unityEventBool;
       
        [ConditionalField(nameof(unityEventBool))]
        public UnityEvent AfterFixed;
        public string speakerName;
        private int _dialogueNum;
        public SpriteRenderer spriteRenderer;
        public Button uIBtn;
        
        public UnityEvent interactiveDialogue;

        private int dialogueNum
        {
            get => _dialogueNum;
            set => _dialogueNum = value;
        }

        private void Start()
        {
            uIBtn?.onClick.AddListener(OnPlayerClick);
                if(spriteRenderer == null)
            spriteRenderer = talkCloud.GetComponent<SpriteRenderer>();
            if (fixedDialogue != null)
            {
                _dialogueNum = 0;
            }
            talkCloud.SetActive(false);
            //Init
            var initColor = textMeshProText.color;
            initColor.a = 0;
            textMeshProText.color = initColor;
            
            //Init
            initColor = talkCloud.GetComponent<SpriteRenderer>().color;
            initColor.a = 0;
            talkCloud.GetComponent<SpriteRenderer>().color = initColor;
        }

        void OnPlayerClick()
        {
            if(dialogueCoroutine != null) return;
                dialogueCoroutine = StartCoroutine(ShowDialogue());
            
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ShowDialogue()
        {
            talkCloud.SetActive(true);
            AudioClip tmpAudioClip = null;
            if (!random)
            {
//                Debug.Log($"{fixedDialogue.Value.Length}, {dialogueNum}");
                if (fixedDialogue.Value.Length <= dialogueNum) yield return null;
                textMeshProText.text = Localization.GetString($"MainMenu/{speakerName}/{fixedDialogue.Value[dialogueNum].dialogue}");
                Debug.Log($"MainMenu/{speakerName}/{fixedDialogue.Value[dialogueNum].dialogue}");
                tmpAudioClip = SoundManager.GetCSound("MainMenu",fixedDialogue.Value[dialogueNum].soundClipName);

                if (fixedDialogue.Value[dialogueNum].changeSprite)
                    ;
                if(fixedDialogue.Value[dialogueNum].doAction)
                    interactiveDialogue.Invoke();
                dialogueNum++;

            }
            else
            {
                var randomDialogeNum = Random.Range(0, randomDialogue.Value.Length);
                textMeshProText.text = Localization.GetString($"MainMenu/{speakerName}/{randomDialogue.Value[randomDialogeNum].dialogue}");
              tmpAudioClip = SoundManager.GetCSound("MainMenu",randomDialogue.Value[randomDialogeNum].soundClipName);
            }
            Fade(1);
            yield return new WaitForSeconds(1f);
            float holdLength = 2;
            if (tmpAudioClip != null)
                holdLength = tmpAudioClip.length + 1f;
            yield return new WaitForSeconds(holdLength);
            Fade(0);
            if (fixedDialogue.Value.Length <= dialogueNum && unityEventBool)
            {
                    
                AfterFixed.Invoke();
                Debug.Log($"{speakerName}, Function call later");
            }
            yield return new WaitForSeconds(1f);
            talkCloud.SetActive(false);
            yield return dialogueCoroutine = null;
        }

        private void Fade(float InOut)
        {
            spriteRenderer.DOFade(InOut, 1f).SetEase(Ease.InOutQuad);
            textMeshProText.DOFade(InOut, 1f).SetEase(Ease.InOutQuad);
        }

        public void Interactions()
        {
            OnPlayerClick();
        }
    }

    [System.Serializable]
    public class MainMenuDialogue
    {
        public string dialogue;
        public string soundClipName;
        public bool doAction;
        public Sprite changeSprite;
    }

    [System.Serializable]
    public class DialogueWarpper : CollectionWrapper<MainMenuDialogue>{}
}
