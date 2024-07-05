using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UImanager : MonoBehaviour
{
    private const float INTERACT_COOLDOWN = 1.5f;
    [SerializeField] private Button interactButton;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text timerText; 
    // GameEnd canvas
    [SerializeField] private CanvasGroup gameEndCanvasGroup;
    [SerializeField] private Text winOrLoseText;
    [SerializeField] private Image whiiteOrBlackImage;
    [SerializeField] private CanvasGroup pauseMenu;
    private float _nextInteractTime = 0f;
    private float totalTime = (int)(4.0f * 60);  
    private float timeRemaining; // Counting down
    private float timeToStayRed = 0.8f;
    private bool isPaused = false; // Timer pauses on certain events
    private Color _originalColor;
    private Coroutine fadeCoroutine;
    
    public float fadeDuration = 0.1f;

    void Start()
    {
        // The game end canvas is invisible before an ending condition happens
        gameEndCanvasGroup.alpha = 0f;
        HidePauseMenu();
        SetCanvasGroupButtonInteractivity(gameEndCanvasGroup, false);
        //timerText.gameObject.gameObject.SetActive(true);
        if (timerText != null)
        {
            timeRemaining = totalTime;
            UpdateTimerText();
            StartCoroutine(TimerCoroutine());
        }
        if (interactButton != null)
        {
            Image buttonImage = interactButton.GetComponent<Image>();
            Text buttonText = interactButton.GetComponentInChildren<Text>();
            // Set alpha to 0
            interactButton.GetComponent<Image>().color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0);
            buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 0);
        }
    }

    public void KillInteractElement()
    {
        interactButton.gameObject.SetActive(false);
    }
    public void ShowInteractElement()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeButton(0.5f));
    }
    
    public void HideInteractElement()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeButton(0));
    }
    
    IEnumerator FadeButton(float targetAlpha)
    {
        Image buttonImage = interactButton.GetComponent<Image>();
        Text buttonText = interactButton.GetComponentInChildren<Text>();

        Color imageColor = buttonImage.color;
        Color textColor = buttonText.color;

        float startAlphaImage = imageColor.a;

        float timer = 0f; // A local timer for the coroutine

        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlphaImage, targetAlpha, timer / fadeDuration);
            buttonImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            buttonText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        buttonImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, targetAlpha);
        buttonText.color = new Color(textColor.r, textColor.g, textColor.b, targetAlpha);
    }

    // Tell the player that this interaction can't happen by color and sound
    public void MakeInteractButtonRed()
    {
        if (Time.time < _nextInteractTime)
        {
            return; 
        }
        audioManager.PlayErrorSound();
        _nextInteractTime = Time.time + INTERACT_COOLDOWN;
        StartCoroutine(ChangeButtonColorCoroutine());
    }
    
    // Coroutine to change the interact button to red temporarily
    private IEnumerator ChangeButtonColorCoroutine()
    {
        Image buttonImage = interactButton.GetComponent<Image>();
        _originalColor = buttonImage.color;
        buttonImage.color = new Color(180f / 255f, 0f, 0f, _originalColor.a); // Set to a shade of red, no change in transparency
        yield return new WaitForSeconds(timeToStayRed);  
        buttonImage.color = _originalColor;
    }
    
    private IEnumerator TimerCoroutine()
    {
        while (timeRemaining > 1)
        {
            // This helps stop time on dialogues and other situations
            if (!isPaused)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            yield return null;
        }
        //Pause timer on 00:00
        PauseTimer();
        // Call gameManager to end the game
        gameManager.GameOver();
        
    }
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        //Both minutes and econds are displayed with double zeros in front
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void HideTimer()
    {
        timerText.gameObject.SetActive(false);
    }
    public void ShowTimer()
    {
       timerText.gameObject.SetActive(true);
    }
    
    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ContinueTimer()
    {
        isPaused = false;
    }

    // Handle extra time booster
    public void AddRemainingTime(float extraTime)
    {
        timeRemaining += extraTime;
    }
    
    // Coroutine Wrapper
    public void FadeToBlack(string message)
    {
        SetCanvasGroupButtonInteractivity(gameEndCanvasGroup, true);
        winOrLoseText.text = message;
        winOrLoseText.color = Color.white; // Set text color to white for visibility on black background
        StartCoroutine(ScreenFade(1f, Color.black));
    }
    
    // Coroutine Wrapper
    public void FadeToWhite(string message)
    {
        SetCanvasGroupButtonInteractivity(gameEndCanvasGroup, true);
        winOrLoseText.text = message;
        winOrLoseText.color = Color.black; // Set text color to black for visibility on white background
        StartCoroutine(ScreenFade(1f, Color.white));
    }
    
    // Coroutine to fade screen on game end
    private IEnumerator ScreenFade(float targetAlpha, Color backgroundColor)
    {
        whiiteOrBlackImage.color = backgroundColor;
        float fadeDuration = 2f; 
        float startAlpha = gameEndCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            gameEndCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        gameEndCanvasGroup.alpha = targetAlpha;
    }
    
    public void ShowPauseMenu() {
        pauseMenu.gameObject.SetActive(true);
        SetCanvasGroupButtonInteractivity(pauseMenu, true);
    }
    
    public void HidePauseMenu() {
        pauseMenu.gameObject.SetActive(false);
        SetCanvasGroupButtonInteractivity(pauseMenu, false);
    }
    
    private void SetCanvasGroupButtonInteractivity(CanvasGroup canvasGroup, bool value) {
        Button button = canvasGroup.GetComponentInChildren<Button>();
        button.interactable = value;
    }
    
}
