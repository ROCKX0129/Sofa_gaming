using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float arcHeight = 45f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Setup(Vector2 targetPosition)
    {
        Vector2 start = transform.position;
        Vector2 dir = targetPosition - start;

        float directionX = Mathf.Sign(dir.x);

        float fixedArcY = arcHeight;
        rb.linearVelocity = new Vector2(directionX * speed, fixedArcY);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int groundLayer = LayerMask.NameToLayer("Ground");

        if (col.gameObject.layer == playerLayer)
        {
            Combat targetCombat = col.transform.root.GetComponent<Combat>();
            if (targetCombat != null && targetCombat.blockCollider != null && targetCombat.blockCollider.enabled)
            {
                Debug.Log("Projectile blocked!");
                Destroy(gameObject); 
                return;
            }

            targetCombat.Die();
            Destroy(gameObject);
            return;
        }
        
        
        Destroy(gameObject);
        
    }


}
