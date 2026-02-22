using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum PortalSide { Left, Right }
    public PortalSide side; // Set in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only respond to players
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // Call the appropriate GameManager method
            if (side == PortalSide.Right)
            {
                GameManager.Instance.GoToNextSceneRight();
            }
            else if (side == PortalSide.Left)
            {
                GameManager.Instance.GoToNextSceneLeft();
            }
        }
    }
}