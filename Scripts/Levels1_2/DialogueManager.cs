using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private const float IN_FOCUS_OPACITY = 1f;
    private const float OUT_OF_FOCUS_OPACITY = 0.4f;
    private const int AFTER_PILLARS_DIALOGUE_INDEX = 11;
    private const int AFTER_CHESS_DIALOGUE_INDEX = 17;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private UImanager UImanager;
    [SerializeField] private Text uiText; // Reference to the UI Text component
    private readonly float _typeWriterDelay = 0.03f; 
    [SerializeField] private List<Sprite> portraitSprites;
    [SerializeField] private GameObject DialogueGameObject;
    [SerializeField] private GameObject leftPortraitReference;
    [SerializeField] private GameObject rightPortraitReference;
    [SerializeField] private DialogueData dialogueData;
    public bool dialogueProgressSignal;
    private Image _leftPortrait;
    private Image _rightPortrait;
    private bool _canProgress = true;
    private bool _exitCondition = false;
    private string _textChunk;
    private int _currentDialogueElementIndex = 0;

    void Start()
    {
        HideDialogueUI();
        UImanager.ShowTimer();
        // May be unecessary
        if (uiText == null)
        {
            uiText = GetComponent<Text>();
        }
        uiText.text = "";
        _leftPortrait = leftPortraitReference.GetComponent<Image>();
        _rightPortrait = rightPortraitReference.GetComponent<Image>();
    }

    public void Update()
    {
        //Debug.Log(_currentDialogueElementIndex);
        if (!_canProgress || !dialogueProgressSignal || dialogueData.dialogueElements[_currentDialogueElementIndex] == null) return;
        DialogueElement nextDialogueElement = GetNextDialogueElement();

        // Set the signal that button is pressed to false
        dialogueProgressSignal = false;
        _textChunk = nextDialogueElement.text; // Store the full text
        uiText.text = ""; // Clear the text
        // Give focus to the speaker by modifying the portrait opacity
        ShowDialogueUI();
        if (nextDialogueElement.isRightSpeaking)
        {
            _leftPortrait.color = new Color(_leftPortrait.color.r, _leftPortrait.color.g, _leftPortrait.color.b, OUT_OF_FOCUS_OPACITY );
            _rightPortrait.color = new Color(_rightPortrait.color.r, _rightPortrait.color.g, _rightPortrait.color.b, IN_FOCUS_OPACITY );
        }
        else
        {
            _leftPortrait.color = new Color(_leftPortrait.color.r, _leftPortrait.color.g, _leftPortrait.color.b, IN_FOCUS_OPACITY);
            _rightPortrait.color = new Color(_rightPortrait.color.r, _rightPortrait.color.g, _rightPortrait.color.b, OUT_OF_FOCUS_OPACITY );
        }
        StartCoroutine(TypeText());

    }

    public bool ExitCondition()
    {
        return _exitCondition;
    }

    public void SetExitCondition(bool value)
    {
        _exitCondition = value;
    }
    private DialogueElement GetNextDialogueElement()
    {
        if (dialogueData == null || dialogueData.CountElements() == 0)
        {
            //Debug.LogWarning("No dialogue elements available");
            return null;
        }
        if (_currentDialogueElementIndex >= dialogueData.CountElements()) return null;
        DialogueElement nextElement = dialogueData.dialogueElements[_currentDialogueElementIndex];
        // Conditional Dialogue progress
        if (_currentDialogueElementIndex == 10)
        {
            _currentDialogueElementIndex = 7;
            _exitCondition = true; 
        } else if (_currentDialogueElementIndex == 16)
        {
            // Spawn Pillar with chespiece
            _currentDialogueElementIndex = 14;
            _exitCondition = true; 
        } else if (_currentDialogueElementIndex == 19)
        {
            // Open chest
            _exitCondition = true; 
        }
        else
        {
            _currentDialogueElementIndex++;
        }
           
        return nextElement;

    }

    public void PillarsComplete()
    {
        _currentDialogueElementIndex = AFTER_PILLARS_DIALOGUE_INDEX;
    }
    public void ChessComplete()
    {
        _currentDialogueElementIndex = AFTER_CHESS_DIALOGUE_INDEX;
    }
    public void ShowDialogueUI()
    {
        UImanager.PauseTimer();
        DialogueGameObject.SetActive(true);
        //uiText.gameObject.SetActive(true);
    }
    
    public void HideDialogueUI()
    {
        UImanager.ContinueTimer();
        DialogueGameObject.SetActive(false);
        //uiText.gameObject.SetActive(true);
    }
    
    
    // Coroutine to type text letter by letter
    IEnumerator TypeText()
    {
        foreach (char c in _textChunk.ToCharArray())
        {
            uiText.text += c;
            yield return new WaitForSeconds(_typeWriterDelay);
        } 

        if (_exitCondition)
        {
            StartCoroutine(WaitForKeyToHide(KeyCode.E));
        }
       
        // Text has finished printing
        _canProgress = true;
    }
    IEnumerator WaitForKeyToHide(KeyCode key)
    {
        //Debug.Log("Waiting for key press...");

        // Wait until the specified key is pressed
        while (!Input.GetKeyDown(key))
        {
            yield return null; // Wait for the next frame
        }
        HideDialogueUI();
        UImanager.ShowTimer();

    }
}
