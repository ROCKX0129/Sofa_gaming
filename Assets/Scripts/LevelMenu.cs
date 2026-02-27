using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public string mainMenuScene = "MainMenu";
    public string gameScene = "GameScene";

    // -------------------------
    // PAUSE
    // -------------------------
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // -------------------------
    // CONTINUE
    // -------------------------
    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // -------------------------
    // NEW GAME
    // -------------------------
    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameScene);
    }

    // -------------------------
    // BACK TO MAIN MENU
    // -------------------------
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}