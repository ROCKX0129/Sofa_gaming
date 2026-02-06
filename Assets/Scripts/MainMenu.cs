using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    // settings panel is for now deactivated i'll work on it next week
    //it was implemented for 6.2
    public GameObject settingsPanel;

    public string levelSceneName = "level";

    private void Awake()
    {
        CleanupOtherScenes();
        ShowMainMenu();
    }

    // removing any other loaded scenes so the floor and other stuff doesnt appear
    private void CleanupOtherScenes()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene != activeScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame called");
        SceneManager.LoadScene(levelSceneName, LoadSceneMode.Single);
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings called");
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
