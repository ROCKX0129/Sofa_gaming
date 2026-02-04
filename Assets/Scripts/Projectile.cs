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
    public void Setup(bool facingRight)
    {
        float direction = facingRight ? 1f : -1f; ;
        
        rb.linearVelocity = new Vector2(direction * speed, arcHeight);
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
