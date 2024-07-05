using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    public void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        CloseCredits();
    }
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    
    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void OpenCredits()
    {
        creditsPanel.gameObject.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.gameObject.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}