using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Level3.scripts
{
    
public class UImanager : MonoBehaviour
{
    private const float INTERACT_COOLDOWN = 1.5f;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    // GameEnd canvas
    [SerializeField] private CanvasGroup gameEndCanvasGroup;
    [SerializeField] private Text winOrLoseText;
    [SerializeField] private Image whiteOrBlackImage;
    [SerializeField] private CanvasGroup pauseMenu;
    private Color _originalColor;
    
    public float fadeDuration = 0.1f;

    void Start()
    {
        // The game end canvas is invisible before an ending condition happens
        gameEndCanvasGroup.alpha = 0f;
        HidePauseMenu();
        SetCanvasGroupButtonInteractivity(gameEndCanvasGroup, false);
        
    }
    
    // Coroutine Wrapper
    public void FadeToBlack(string message)
    {
        SetCanvasGroupButtonInteractivity(gameEndCanvasGroup, true);
        winOrLoseText.text = message;
        winOrLoseText.color = Color.white;
        StartCoroutine(ScreenFade(1f, Color.black));
    }
    
    // Coroutine Wrapper
    public void FadeToWhite(string message) {
        SetCanvasGroupButtonInteractivity(gameEndCanvasGroup, true);
        winOrLoseText.text = message;
        winOrLoseText.color = Color.black; 
        StartCoroutine(ScreenFade(1f, Color.white));
    }
    
    // Coroutine to fade screen on game end
    private IEnumerator ScreenFade(float targetAlpha, Color backgroundColor)
    {
        whiteOrBlackImage.color = backgroundColor;
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
        SetCanvasGroupButtonInteractivity(pauseMenu,true);
    }
    
    public void HidePauseMenu() {
        pauseMenu.gameObject.SetActive(false);
        SetCanvasGroupButtonInteractivity(pauseMenu,false);
    }

    private void SetCanvasGroupButtonInteractivity(CanvasGroup canvasGroup, bool value) {
        Button button = canvasGroup.GetComponentInChildren<Button>();
        button.interactable = value;
    }
}

}