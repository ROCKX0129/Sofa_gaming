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

    private SpriteRenderer spriteRenderer;
    private Animator animator;
   


   
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

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

            if  (spriteRenderer != null )
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.15f);
                spriteRenderer.color = Color.white;
            }
            
            yield return new WaitForSeconds(beepInterval - 0.15f);
            timer += beepInterval;
        }

        Explode();
    }

    private void Explode()
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

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        transform.localScale = Vector3.one * 0.8f;
        if (animator != null)
            animator.SetTrigger("explode");

        // Destroy the mine itself
        Destroy(gameObject, 1f);
    }

    // Draw explosion radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
