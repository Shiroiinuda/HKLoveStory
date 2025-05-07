using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Characters;
using UnityEngine.Analytics;
//using UnityEngine.iOS;

public class CharacterManager : MonoBehaviour
{
    enum position
    {
        left =0,
        middle = 1,
        right = 2
    }
    public List<Character> characters; // Array of character prefabs
    public Camera smallIconCamera;
    public Vector3 smallIconVector3;
    [Header("Dialogue Mode - Default")] 
    public Transform left;
    public Transform middle;
    public Transform right;
    public List<Character> characterPosition = new List<Character>(3);
    public Animator textureAnim;
    private static readonly int IsSpeaking = Animator.StringToHash("isSpeaking");
    private static readonly int Actived = Animator.StringToHash("isActivated");
    public Character currentCharater;

    private void Awake()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if(characters[i] !=null) continue;
                characters.RemoveAt(i);
        }
    }

    public void DefaultActivateCharacter(int characterId, string bodySpriteName, string headSpriteName,
        string expressionSpriteName, bool needFade, string position)
    {
        Character currentCharacter = null;
        foreach (Character targerCharacter in characters)
        {

            if (targerCharacter.id == characterId && !characterPosition.Contains(targerCharacter))
            {
                currentCharacter = targerCharacter;
                targerCharacter.gameObject.SetActive(true);
                
                targerCharacter.ChangeCharacterFace(bodySpriteName, headSpriteName, expressionSpriteName);
            }
            else if (characterPosition.Contains(targerCharacter) && targerCharacter.id == characterId)
            {
                currentCharacter = targerCharacter;
                targerCharacter.ChangeCharacterFace(bodySpriteName, headSpriteName, expressionSpriteName);
                
            }
            else if(!characterPosition.Contains(targerCharacter))
            {
                targerCharacter.gameObject.SetActive(false);
            }
            
        }
//        Debug.Log(currentCharacter);

        if (currentCharacter is null)
            return;
        currentCharater = currentCharacter;
        switch (position)
        {
            case "Left":
                UpdatePosition(0,currentCharacter, left.position);
                break;
            case "Center":
                UpdatePosition(1,currentCharacter, middle.position);
                break;
            case "Right":
                UpdatePosition(2,currentCharacter, right.position);
                break;
        }
        foreach (var character in characterPosition)
        {
            character?.anim.SetBool(IsSpeaking,false);
            if (character != null)
            {
//                Debug.Log($"{character.id} , {character.id == characterId}");
                character?.anim.SetBool(IsSpeaking,character.id == characterId);
                character?.anim.SetBool(Actived,true);
                
            }
        }
        if (!needFade || characterId == currentCharacter.id) return;
        
        
        

    }

    public void DefaultDisableCharacters()
    {
        //Debug.Log("Fadeout");
        Debug.Log("AA");
        for (int i= 0;i < characterPosition.Count;i++)
        {
            characterPosition[i]?.anim.SetBool(Actived,false);
            if (characterPosition[i] == null)
                continue;
            characterPosition[i].gameObject.SetActive(false);
            characterPosition[i] = null;
        }
        //Debug.Log("A");
        foreach (Character targerCharacter in characters)
        {
            targerCharacter.gameObject.SetActive(false);
        }
    }

    public void UpdatePosition(int posNum,Character character , Vector3 pos)
    {
        if (character != characterPosition[posNum])
        {
            characterPosition[posNum]?.anim.SetBool(Actived,false);
            characterPosition[posNum]?.gameObject.SetActive(false); 
        }
        characterPosition[posNum] = character;
        characterPosition[posNum].transform.position = pos;
        
    }
    public void UpdatePosition(int posNum,Character character )
    {
        if (character != characterPosition[posNum])
        {
            characterPosition[posNum]?.anim.SetBool(Actived,false);
            characterPosition[posNum]?.gameObject.SetActive(false); 
        }
        characterPosition[posNum] = character;
        Transform pos = posNum switch
        {
            0 => left,
            1 => middle,
            2 => right,
            _ => left
        };
        characterPosition[posNum].transform.position = pos.position;
        
    }

    public void SmallIconBoxActivateCharacter(int id,string bodySpriteName, string headSpriteName,
        string expressionSpriteName)
    {
        if (string.IsNullOrEmpty(DialogueManager.Instance.currentDialogue.speakerPosition))
        {
            Debug.Log("B");
            foreach (var targetCharacter in characters)
            {
                if(targetCharacter.id != id)
                {
                    targetCharacter.gameObject.SetActive(false);
                }
            }
            EnableCharacter(id).transform.position= smallIconVector3;
            smallIconCamera.transform.position = smallIconVector3;
            EnableCharacter(id).anim.SetBool(IsSpeaking,true);
        }
        else
        {
            Debug.Log("A");
            foreach (var targetCharacter in characters)
            {
                if (targetCharacter.id == id && !characterPosition.Contains(targetCharacter))
                {
                    targetCharacter.gameObject.SetActive(true);
                    targetCharacter.ChangeCharacterFace(bodySpriteName, headSpriteName, expressionSpriteName);
                }else if(!characterPosition.Contains(targetCharacter))
                {
                    targetCharacter.gameObject.SetActive(false);
                }
            }

            
            EnableCharacter(id).ChangeCharacterFace(bodySpriteName, headSpriteName, expressionSpriteName);
            EnableCharacter(id).transform.position= smallIconVector3;
            smallIconCamera.transform.position = EnableCharacter(id).head.transform.position;
            EnableCharacter(id).anim.SetBool(Actived,true);
            EnableCharacter(id).anim.SetBool(IsSpeaking,true);
        }
        foreach (var character in characterPosition)
        {
            character?.anim.SetBool(IsSpeaking,false);
            if (character != null)
            {
//                Debug.Log($"{character.id} , {character.id == characterId}");
                character?.anim.SetBool(IsSpeaking,character.id == id);
                character?.anim.SetBool(Actived,true);
                
            }
        }
        currentCharater = EnableCharacter(id);
    }

    private Character EnableCharacter(int id)
    {
        foreach (var targetCharacter in characters)
        {
            if (targetCharacter.GetComponent<Character>().id == id)
            {
                targetCharacter.gameObject.SetActive(true);
               return targetCharacter;
            }
        }

        return null;
    }
}