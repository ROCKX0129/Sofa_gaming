using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Setup")]
    public string[] levels; 

    [Header("Portals")]
    public GameObject portalLeft;
    public GameObject portalRight;

    private string winnerTag;
    private int currentLevelIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    
    public void PlayerDied(string deadPlayerTag)
    {
        
        winnerTag = deadPlayerTag == "Player1" ? "Player2" : "Player1";

       
        if (winnerTag == "Player1")
        {
            if (portalRight != null) portalRight.SetActive(true);
            if (portalLeft != null) portalLeft.SetActive(false);
        }
        else if (winnerTag == "Player2")
        {
            if (portalLeft != null) portalLeft.SetActive(true);
            if (portalRight != null) portalRight.SetActive(false);
        }
    }

    
    public bool IsWinner(string playerTag)
    {
        return playerTag == winnerTag;
    }

    
    public void LoadNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex < levels.Length)
        {
            SceneManager.LoadScene(levels[currentLevelIndex]);
        }
        else
        {
            Debug.Log("No more levels!");
        }
    }
}
