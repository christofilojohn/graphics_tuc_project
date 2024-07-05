using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    public List<DialogueElement> dialogueElements;

    public void AddDialogueElement(DialogueElement element)
    {
        if (dialogueElements == null)
        {
            dialogueElements = new List<DialogueElement>();
        }
        dialogueElements.Add(element);
    }

    public int CountElements()
    {
        return dialogueElements.Count;
    }
}