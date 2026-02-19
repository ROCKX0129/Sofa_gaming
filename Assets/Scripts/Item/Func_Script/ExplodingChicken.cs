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

    private Rigidbody2D rb;
    private float direction;
    private float directionTimer;
    private float explosionTimer;

    private bool isThrown = false; // NEW FLAG

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        direction = Random.value > 0.5f ? 1f : -1f;
        explosionTimer = Random.Range(minExplosionTime, maxExplosionTime);

        // Initially stationary until thrown
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (!isThrown) return; // Only move after thrown

        // Random walking
        directionTimer += Time.deltaTime;
        if (directionTimer >= changeDirectionTime)
        {
            direction *= -1f;
            directionTimer = 0f;
        }

        rb.linearVelocity = new Vector2(direction * walkSpeed, 0);

        // Explosion timer
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0)
        {
            Explode();
        }
    }

    // Called by ItemCharacterManager when thrown
    public void Throw(Vector2 force)
    {
        isThrown = true;

        // Add initial push
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            Destroy(hit.gameObject); // instant death
        }

        Destroy(gameObject);
    }
}
