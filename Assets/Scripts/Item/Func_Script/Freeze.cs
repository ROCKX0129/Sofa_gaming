using UnityEngine;
using System.Collections;

public class Freeze : MonoBehaviour, IItem
{
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Projectile Settings")]
    public float speed = 10f;
    public float freezeDuration = 2f;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private Collider2D col;
    private bool isShot = false; // <-- add this flag

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();

        // Start as pickup: falls naturally
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        col.isTrigger = false; // collides with ground
    }

    // Called by manager when shooting
    public void Shoot(Vector2 direction)
    {
        isShot = true; // now this is a projectile
        rb.gravityScale = 0f;
        rb.linearVelocity = direction.normalized * speed;
        col.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isShot) return; // Ignore collisions before being shot

        // Freeze player if hit
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerControls controls = collision.gameObject.GetComponent<PlayerControls>();
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

