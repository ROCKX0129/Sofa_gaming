using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLoader : MonoBehaviour
{
    public bool goForward;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player1") && !other.CompareTag("Player2")) return;

        int current = SceneManager.GetActiveScene().buildIndex;
        int target = goForward ? current + 1 : current - 1;

        if (target >= 0 && target < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(target);
    }
}