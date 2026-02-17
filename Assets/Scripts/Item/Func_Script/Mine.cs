using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour, IItem
{
    [Header("Settings")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Mine Timing")]
    public float armDelay = 0.5f;       // Delay before it can be triggered
    public float explodeDelay = 2f;     // Countdown after player triggers it
    public float explosionRadius = 3f;  // Radius of explosion

    [Header("Collision")]
    public LayerMask playerLayer;       // Set this to your "Player" layer

    private bool isArmed = false;
    private bool triggered = false;
    private bool exploded = false;

    private void Start()
    {
        // Arm the mine after spawn
        Invoke(nameof(ArmMine), armDelay);
    }

    private void ArmMine()
    {
        isArmed = true;
        Debug.Log("Mine is armed");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isArmed || triggered || exploded)
            return;

        // Only trigger when a player touches it
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            triggered = true;
            Debug.Log("Mine triggered! Beep.");

            // Start countdown to explosion
            StartCoroutine(ExplodeAfterDelay(collision.gameObject));
        }
    }

    private IEnumerator ExplodeAfterDelay(GameObject triggeringPlayer)
    {
        float timer = 0f;
        float beepInterval = 0.5f;

        // Optional: visual/audio beep effect
        while (timer < explodeDelay)
        {
            Debug.Log("Beep!");
            timer += beepInterval;
            yield return new WaitForSeconds(beepInterval);
        }

        Explode(triggeringPlayer);
    }

    private void Explode(GameObject triggeringPlayer)
    {
        if (exploded)
            return;

        exploded = true;
        Debug.Log("Mine exploded!");

        // Find all players in explosion radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("hit player " + hit.gameObject.name);

            Destroy(hit.transform.root.gameObject); // One-hit kill other players in radius
        }

        // Destroy the mine itself
        Destroy(gameObject);
    }

    // Draw explosion radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
