using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Portals")]
    public GameObject leftPortal;
    public GameObject rightPortal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find portals by name in the new scene
        leftPortal = GameObject.Find("LeftPortal");
        rightPortal = GameObject.Find("RightPortal");

        // Start with them disabled
        if (leftPortal != null) leftPortal.SetActive(false);
        if (rightPortal != null) rightPortal.SetActive(false);
    }

    
    public void PlayerDied(string playerTag)
    {
        Debug.Log(playerTag + " died!");

        
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
            player.SetActive(false);

        
        if (playerTag == "Player1")
        {
            
            if (rightPortal != null) rightPortal.SetActive(true);
            if (leftPortal != null) leftPortal.SetActive(false);
        }
        else if (playerTag == "Player2")
        {
            
            if (leftPortal != null) leftPortal.SetActive(true);
            if (rightPortal != null) rightPortal.SetActive(false);
        }
    }

    
    public void GoToNextSceneRight()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(currentIndex + 1);
    }

    public void GoToNextSceneLeft()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex > 0)
            SceneManager.LoadScene(currentIndex - 1);
    }
}

