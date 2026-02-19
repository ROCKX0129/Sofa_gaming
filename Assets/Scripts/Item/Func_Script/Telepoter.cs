using UnityEngine;

public class Teleporter : MonoBehaviour, IItem
{
    [Header("Item Data")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    private GameObject ownerPlayer;        // the player who threw it
    private bool isPlaced = false;         // has it been thrown/placed

    [Header("Teleport Settings")]
    public float teleportDelay = 0.1f;     // optional delay if needed
    public LayerMask groundLayer;          // ground detection

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

        // Start as pickup item
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        col.isTrigger = false;  // collide with ground
    }

    // Called by ItemCharacterManager when thrown
    public void SetOwner(GameObject player)
    {
        ownerPlayer = player;
        isPlaced = true;

        // After throw, let physics handle placement
        rb.gravityScale = 1f;
        col.isTrigger = false;
    }

    // Called by the player to teleport
    public void TeleportPlayer()
    {
        if (!isPlaced || ownerPlayer == null) return;

        // Teleport the owner to the teleporter's position
        ownerPlayer.transform.position = transform.position;

        // Optional: destroy teleporter after use
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop moving if hits ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic; // stay in place
        }
    }
}

