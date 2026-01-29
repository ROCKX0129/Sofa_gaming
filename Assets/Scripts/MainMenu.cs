using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    public string mainScene = "MainScene";

    private void Start()
    {
        ShowMainMenu();
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void ExitGame()
    {
            Application.Quit();
       
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        ShowMainMenu();
    }


    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
