using UnityEngine;
using System.Collections;

public class Quicksand2D : MonoBehaviour
{
    public float jumpReduction = 0.3f;
    public float lingerTime = 3f;
    private bool playerAffected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerAffected) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerAffected = true;

            if (!player.gameObject.TryGetComponent<OriginalJump>(out _))
            {
                OriginalJump original = player.gameObject.AddComponent<OriginalJump>();
                original.value = player.jumpForce;
            }

            player.jumpForce *= jumpReduction;
        }
    }

        public class OriginalJump : MonoBehaviour
    {
        public float value;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && playerAffected)
        {
            StartCoroutine(RestoreJumpAfterDelay(player));
        }
    }

    private IEnumerator RestoreJumpAfterDelay(PlayerController player)
    {
        yield return new WaitForSeconds(lingerTime);

        OriginalJump original = player.GetComponent<OriginalJump>();
        if (original != null)
        {
            player.jumpForce = original.value;
            Destroy(original);
        }

        playerAffected = false;
    }
}
