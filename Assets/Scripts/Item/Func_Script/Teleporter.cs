using UnityEngine;

public class Teleporter : MonoBehaviour, IItem
{
    [Header("Item Data")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    private GameObject ownerPlayer;
    public bool isPlaced = false; // has it been thrown
    public bool isPickedUp = false;

    [Header("Teleport Settings")]
    public float throwForce = 5f;   // how far it gets thrown
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        col.isTrigger = false;
    }

    public void PickUp(GameObject player)
    {
        ownerPlayer = player;
        isPickedUp = true;
        isPlaced = false;

        // Hide and stop physics while held
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        gameObject.SetActive(false);
    }

    public void Throw()
    {
        if (!isPickedUp || ownerPlayer == null) return;

        isPickedUp = false;
        isPlaced = true;

        // Determine throw start position slightly in front of the player
        float horizontalOffset = 1f; // adjust how far in front
        Vector3 startPosition = ownerPlayer.transform.position + new Vector3(horizontalOffset * ownerPlayer.transform.localScale.x, 0f, 0f);
        transform.position = startPosition;

        gameObject.SetActive(true);
        col.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        // Apply throw velocity forward
        Vector2 throwDir = new Vector2(ownerPlayer.transform.localScale.x, 0f).normalized;
        rb.linearVelocity = throwDir * throwForce;
    }

    public void TeleportOwner()
    {
        if (!isPlaced || ownerPlayer == null) return;

        ownerPlayer.transform.position = transform.position;

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}