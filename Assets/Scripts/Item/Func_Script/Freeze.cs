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
    public LayerMask playerLayer;
    private bool isShot = false;
    // Layer of players to freeze

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
         // so player can pick it up
    }

    // Called by ItemCharacterManager when shooting
    public void Shoot(Vector2 direction)
    {
        isShot = true;
        rb.bodyType = RigidbodyType2D.Dynamic; // physics active
        rb.gravityScale = 0f;                  // fly straight
        col.isTrigger = true;                 // collide with players
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isShot) return; // ignore all collisions until it’s shot

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            var controller = collision.gameObject.GetComponent<PlayerController>();
            if (controller != null)
                controller.StartCoroutine(controller.Freeze(freezeDuration));

        }

        Destroy(gameObject); // destroy only after hitting something post-shot
    }


    private IEnumerator FreezePlayer(PlayerController controller)
    {
        Debug.Log("Freeze started");

        Rigidbody2D playerRb = controller.GetComponent<Rigidbody2D>();

        controller.isFrozen = true;

        playerRb.linearVelocity = Vector2.zero;
        playerRb.angularVelocity = 0f;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(freezeDuration);

        Debug.Log("Freeze ended");

        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        controller.isFrozen = false;
    }


}


