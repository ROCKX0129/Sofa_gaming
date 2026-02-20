using UnityEngine;

public class ExplodingChicken : MonoBehaviour, IItem
{
    [Header("Item data")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Movement & Explosion")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float changeDirectionTime = 2f;
    [SerializeField] private float minExplosionTime = 3f;
    [SerializeField] private float maxExplosionTime = 6f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float direction;
    private float directionTimer;
    private float explosionTimer;

    private bool grounded = false; // Only start walking when true

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Chicken falls naturally
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 10f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        direction = Random.value > 0.5f ? 1f : -1f;
        explosionTimer = Random.Range(minExplosionTime, maxExplosionTime);

        // Do not move yet
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        // Only move if grounded
        if (!grounded) return;

        // Random walking
        directionTimer += Time.deltaTime;
        if (directionTimer >= changeDirectionTime)
        {
            direction *= -1f;
            directionTimer = 0f;
        }

        // Move horizontally while keeping physics intact
        rb.linearVelocity = new Vector2(direction * walkSpeed, rb.linearVelocity.y);

        // Explosion countdown
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0f)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only trigger grounded if it hits the ground layer
        if (!grounded && ((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            grounded = true;

            // Stop vertical movement and let it walk
            rb.linearVelocity = new Vector2(0f, 0f);
        }
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            Destroy(hit.gameObject); // Instant kill
        }
        Destroy(gameObject);
    }

    // Called externally if you want to throw it
    public void Throw(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}