using UnityEngine.UI;

[System.Serializable]
public class DialogueElement
// In this version of the game the portrait indexes are redundant
{
    public string text;
    public int leftPortraitIconIndex;
    public int rightPortraitIconIndex;
    public bool isRightSpeaking;

    public DialogueElement(string text, int leftPortraitIconIndex, int rightPortraitIconIndex, bool isRightSpeaking)
    {
        this.text = text;
        this.leftPortraitIconIndex = leftPortraitIconIndex;
        this.rightPortraitIconIndex = rightPortraitIconIndex;
        this.isRightSpeaking = isRightSpeaking;
    }
}