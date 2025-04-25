using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;
using I2.Loc;
using SoundControl;
using UnityEngine;
using UnityEngine.UI;

public class FunctionListener : MonoBehaviour
{
    private DialogueManager dialogueManager;

    private void Awake()=>dialogueManager = GetComponent<DialogueManager>();

    private void ClearInventory()=>dialogueManager.inventoryManager.ClearInventory();

    private void CharacterJump()=>dialogueManager.characterManager.currentCharater.TestJump();
    private void CharacterShake()=>dialogueManager.characterManager.currentCharater.TestShake();

    private void StartInvestigation()
    {
        dialogueManager.InvestiationLocalize.SetTerm("ImageUI/StartInvestigation");
        dialogueManager.startInvestigationAnim.SetTrigger("Start");
    }

    private void StartInvestigation2()
    {
        dialogueManager.InvestiationLocalize.SetTerm("ImageUI/StartInvestigation");
        dialogueManager.startInvestigationAnim.SetTrigger("Start2");
    }
    private void StartInvestigations() => Debug.Log("A");

    private void StartBattle()
    {
        dialogueManager.InvestiationLocalize.SetTerm("battle_start");
        SoundManager.PlaySfx("battle_start");
        dialogueManager.startInvestigationAnim.SetTrigger("Start");
    }
    
    private void GameOverPanel() => dialogueManager.GameOverPanels();

    private void ClearCharacter() => dialogueManager.characterManager.DefaultDisableCharacters();
    private const string PPrefArgue = "DialogueArgueScore";
    private void DecuctMarksFunction(int score) => PlayerPrefs.SetInt(PPrefArgue, PlayerPrefs.GetInt(PPrefArgue)+score);
    private void ResetMark() => PlayerPrefs.SetInt(PPrefArgue, 0);

    private void CountMark()
    { 
        var tmpScore =PlayerPrefs.GetInt(PPrefArgue);
        if (tmpScore >= 0)
        {
            dialogueManager.jumpMark = int.Parse(dialogueManager.currentDialogue.roads.road1);
        }
        else
        {
            dialogueManager.jumpMark =  int.Parse(dialogueManager.currentDialogue.roads.road2);
        }
    }

   // private void PostProcessingFilter(int arg0) => PostProcessingController.Instance.Filter(arg0);
    /*private void CharacterMovement(int pos,int characterId)
    {
        dialogueManager.characterManager.UpdatePosition(pos, dialogueManager.characterManager.characters[characterId]);
    }*/

    private void UnlockedChapter(string a)
    {
        string[] endSuffixes = a.Split('&');
        
        foreach (var suffix in endSuffixes)
        {
            if (!GameControl.instance.stageClear.Contains(suffix.Trim()))
            {
                GameControl.instance.stageClear.Add(suffix.Trim());
            }
        }
    }

    private void CheckUnlockedChapter(string b)
    {
        string[] endSuffixes = b.Split('&');

        bool found = false;
        foreach (var suffix in endSuffixes)
        {
            if (GameControl.instance.stageClear.Contains(suffix.Trim()))
            {
                found = true;
                break;
            }
        }

        dialogueManager.jumpMark = int.Parse(found ? dialogueManager.currentDialogue.roads.road1 : dialogueManager.currentDialogue.roads.road2);
    }

    private void AddCG(string c)
    {
        if (!GameControl.instance.CG.Contains(c))
            GameControl.instance.CG.Add(c);
    }

    private void ResetLastOpenedDoor()
    {
        Debug.Log("ResetLastOpenedDoor2");
        if (PlayerPrefs.HasKey("LastOpenedDoor"))
        {
            PlayerPrefs.SetInt("LastOpenedDoor", 0);
            PlayerPrefs.Save();
        }
    }

    private const string BossGameFuPP = "BossFuAmount";
    private void UnlockFuAchievement()
    {
        if (FBPP.GetInt(BossGameFuPP) >= 50)
            GameControl.instance.OnUnlockAchievement("Amulet");
    }

     // Main method to invoke any function
     public bool CallFunctionsByCell(string cellContent)
    {
        Debug.Log($"Func: { cellContent}");
        try
        {
            // Split the input string into separate function calls using newline or any delimiter
            string[] functions = cellContent.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string function in functions)
            {
                Debug.Log(function);
              return  CallFunctionByName(function.Trim());
            }


        }
        catch (Exception e)
        {
            Debug.LogError($"Error invoking functions: {e.Message}");
            return false;
        }
        return true;
    }

    public bool CallFunctionByName(string functionName)
    {
        Debug.Log(functionName);
        try
        {
            // Check if the function has parentheses (with or without arguments)
            int startIndex = functionName.IndexOf('(');
            int endIndex = functionName.IndexOf(')');

            if (startIndex == -1 || endIndex == -1)
            {
                // If no parentheses, treat it as a parameterless function
                InvokeFunction(functionName, null);
                return true;
            }

            // Extract method name
            string methodName = functionName.Substring(0, startIndex);

            // Extract arguments (if any)
            string arguments = functionName.Substring(startIndex + 1, endIndex - startIndex - 1);
            object[] parsedArgs = string.IsNullOrWhiteSpace(arguments) ? null : ParseArguments(arguments.Split('.'));

            // Invoke the function
            InvokeFunction(methodName, parsedArgs);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error invoking function: {e.Message}");
            return false;
        }

        return true;
    }

    private void InvokeFunction(string methodName, object[] args)
    {
        // Use reflection to find the method
        MethodInfo method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method == null)
        {
            throw new MissingMethodException($"Method {methodName} not found!");
        }

        // Check if the method requires parameters
        if (args == null && method.GetParameters().Length > 0)
        {
            throw new ArgumentException($"Method {methodName} requires parameters, but none were provided.");
        }

        // Invoke the method
        method.Invoke(this, args);
    }

    private object[] ParseArguments(string[] argArray)
    {
        object[] parsedArgs = new object[argArray.Length];
        for (int i = 0; i < argArray.Length; i++)
        {
            string arg = argArray[i].Trim();

            // Basic type inference (int, float, string)
            if (int.TryParse(arg, out int intValue))
            {
                parsedArgs[i] = intValue;
            }
            else if (float.TryParse(arg, out float floatValue))
            {
                parsedArgs[i] = floatValue;
            }
            else if (arg.StartsWith("\"") && arg.EndsWith("\"")) // Handles string literals
            {
                parsedArgs[i] = arg.Trim('"');
            }
            else
            {
                throw new ArgumentException($"Unsupported argument type: {arg}");
            }
        }
        return parsedArgs;
    }
}