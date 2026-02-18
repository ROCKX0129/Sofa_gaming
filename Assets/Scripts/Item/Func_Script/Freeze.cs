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

        // Item pickup mode (falls from sky)
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        col.isTrigger = true; // so player can pick it up
    }

    // Called by ItemCharacterManager when shooting
    public void Shoot(Vector2 direction)
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // physics active
        rb.gravityScale = 0f;                  // fly straight
        col.isTrigger = false;                 // collide with players
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            var controller = collision.gameObject.GetComponent<PlayerController>();
            if (controller != null)
                StartCoroutine(FreezePlayer(controller));
        }

        Destroy(gameObject); // destroy projectile after hitting anything
    }

    private IEnumerator FreezePlayer(PlayerController controller)
    {
        controller.enabled = false; // disable movement/jump
        yield return new WaitForSeconds(freezeDuration);
        controller.enabled = true;  // re-enable
    }
}


