using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSprite", menuName = "ScriptableObjects/Characters", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public int id;
    public List<Sprite> bodySprites;
    public List<Sprite> headSprites;
    public List<Sprite> expressionSprites;
}
