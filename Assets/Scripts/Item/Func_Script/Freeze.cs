using UnityEngine;
using System.Collections;

public class Freeze : MonoBehaviour, IItem
{
    [Header("Item Data")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Projectile Settings")]
    public float speed = 10f;             // Speed when shot
    public float freezeDuration = 2f;     // How long the freeze lasts
    public LayerMask playerLayer;         // Layer of players to freeze

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
        if (col == null)
            col = gameObject.AddComponent<BoxCollider2D>();

        // Start as pickup: falls from sky naturally
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        col.isTrigger = true; // so player can pick it up
    }

    // Called by manager when shooting
    public void Shoot(Vector2 direction)
    {
        // Make it move like a projectile
        rb.bodyType = RigidbodyType2D.Dynamic; // allow physics
        rb.gravityScale = 0f;                  // fly straight
        col.isTrigger = false;                 // collide with players
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            var controls = collision.gameObject.GetComponent<PlayerControls>();
            if (controls != null)
                StartCoroutine(FreezePlayer(controls));
        }

        Destroy(gameObject); // destroy after hitting anything
    }

    private IEnumerator FreezePlayer(PlayerControls controls)
    {
        controls.Disable();
        yield return new WaitForSeconds(freezeDuration);
        controls.Enable();
    }
}


