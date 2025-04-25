using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace MainMenu
{
    public class MainMenuInteract : MonoBehaviour,IMainMenuInteraction
    {

        public SpriteRenderer sprite;
        
        
        public UnityEvent interaction;
        public void Interactions()
        {
            interaction?.Invoke();
        }

        public void FadeOut()
        {
            sprite.DOFade(0, 0.5f);
        }
    }
}
