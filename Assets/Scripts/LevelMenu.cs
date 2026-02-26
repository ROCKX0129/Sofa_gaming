using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
   
   void Awake()
    {
        
        Debug.Log(SceneManager.GetAllScenes().Count());
    }
    public string mainMenuScene = "MainMenu";

    public void GoToMainMenu()
    {
        Debug.Log("Loading menu called");
        SceneManager.LoadScene(mainMenuScene);
    }


}
