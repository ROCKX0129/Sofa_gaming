using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float angle = 45f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Setup(bool facingRight)
    {
        float direction = facingRight ? 1f : -1f; ;
        float rad = angle * Mathf.Deg2Rad;

        float vx = Mathf.Cos(rad) * speed * direction;
        float vy = Mathf.Sin(rad) * speed;

        rb.linearVelocity = new Vector2 (vx, vy);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.transform.root.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (col.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
