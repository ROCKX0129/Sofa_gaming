using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLoader : MonoBehaviour
{
    public bool goForward;
    public bool isFinalPortal = false;
    public int winnerPlayer = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player1") && !other.CompareTag("Player2")) return;

        if (isFinalPortal)
        {
            if (winnerPlayer != 0)
            {
                PlayerPrefs.SetInt("Winner", winnerPlayer);
                if (winnerPlayer == 1)
                    PlayerPrefs.SetString("PlayerOneName", "Player 1");
                else if (winnerPlayer == 2)
                    PlayerPrefs.SetString("PlayerTwoName", "Player 2");
            }
            SceneManager.LoadScene("EndingScene");
            return;
        }
        int current = SceneManager.GetActiveScene().buildIndex;
        int target = goForward ? current + 1 : current - 1;

        if (target >= 0 && target < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(target);
    }
}