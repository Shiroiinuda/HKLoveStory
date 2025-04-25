using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DelBookmark : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TextMeshProUGUI bookmarkTest;

    public void updateBookmark()
    {
        bookmarkTest.text = dialogueManager.currentDialogue.bookMark.ToString();
    }
}
