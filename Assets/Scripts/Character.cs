using System.Collections.Generic;
using JetBrains.Annotations;
using MyBox;
using UnityEditor;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        public int id;
        public SpriteRenderer body;
        public SpriteRenderer head;
        public SpriteMask bodyMask;
        public SpriteRenderer expression;
        public SpriteMask expressionMask;

        public Transform parent;
        /*public List<Sprite> bodySprites;
        public List<Sprite> headSprites;
        public List<Sprite> expressionSprites;*/
        public CharacterScriptableObject characterScriptableObject;
        public Animator anim;
        #if UNITY_EDITOR
        [ButtonMethod]
        public void SpriteQuickSetup()
        {
            var root = transform;
            body = root.Find("Character/Body").GetComponent<SpriteRenderer>();
            head = root.Find("Character/Head").GetComponent<SpriteRenderer>();
            bodyMask = root.Find("Character/Body").GetComponent<SpriteMask>();
            expression = root.Find("Character/Head/Expression").GetComponent<SpriteRenderer>();
            expressionMask = root.Find("Character/Head/Expression").GetComponent<SpriteMask>();
            anim = GetComponent<Animator>();
        }
        [ButtonMethod]
        public void QuickSetup()
        {
            CharacterScriptableObject asset = AssetDatabase.LoadAssetAtPath<CharacterScriptableObject>($"Assets/ScriptableObject/Characters/Character_{id}.asset");
            Debug.Log($"{asset}  Assets/ScriptableObject/Characters/Character_{id}.asset");
            if (asset == null) return;
            characterScriptableObject = asset;
            if (characterScriptableObject == null) return;
            body.sprite = characterScriptableObject.bodySprites[0];
            bodyMask.sprite = characterScriptableObject.bodySprites[0];
            head.sprite = characterScriptableObject.headSprites[0];
            expression.sprite = characterScriptableObject.expressionSprites[0];
            expressionMask.sprite = characterScriptableObject.expressionSprites[0];


        }
        #endif
        [ButtonMethod]
        public void TestShake()
        {
            GetComponent<CharacterEmote>().TestShake();
        }
        [ButtonMethod]
        public void TestJump()
        {
            anim.SetTrigger("Jump");
        }
        private void Awake()
        {
            if (anim == null)
                anim = GetComponent<Animator>();
        }

        public void ResetPos()
        {
            parent.localPosition = Vector3.zero;
            GetComponent<CharacterEmote>().target.localPosition = Vector3.zero;
        }
        public void ChangeCharacterFace([CanBeNull] string bodySpriteName, [CanBeNull] string headSpriteName, [CanBeNull] string expressionSpriteName)
        {
            ResetPos();
            if (characterScriptableObject == null) return;
            UpdateSprite(body, characterScriptableObject.bodySprites, bodySpriteName, bodyMask);
            UpdateSprite(head, characterScriptableObject.headSprites, headSpriteName);
            UpdateSprite(expression, characterScriptableObject.expressionSprites, expressionSpriteName, expressionMask);
        }

        public void ChangeCharacterFace()
        {
            UpdateSprite(body, characterScriptableObject.bodySprites, null, bodyMask);
            UpdateSprite(head, characterScriptableObject.headSprites);
            UpdateSprite(expression, characterScriptableObject.expressionSprites, null, expressionMask);
        }
        private void UpdateSprite(SpriteRenderer renderer, List<Sprite> spriteList, [CanBeNull]string spriteName= null,
            SpriteMask additionalRenderer = null)
        {
            if(renderer.sprite is null) return;
            if (spriteName == null)
            {
                renderer.sprite = spriteList[0];
                if (additionalRenderer is null) return;
                additionalRenderer.sprite =  spriteList[0];
            }
            if (renderer.sprite.name == spriteName) return;
            var sprite = spriteList.Find(s => s.name == spriteName);
            if (sprite is null)
            {
                Debug.Log($"No {spriteName} in {spriteList}");
                return;
            }

            renderer.sprite = sprite;
            if (additionalRenderer is null) return;
            additionalRenderer.sprite = sprite;
        }
    }
}