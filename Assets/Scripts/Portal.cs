using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name + "touched the portal");

        if (GameManager.Instance.IsWinner(collision.tag))
        {
            Debug.Log("Winner touched the portal");
            GameManager.Instance.LoadNextLevel();
        }
    }
}
