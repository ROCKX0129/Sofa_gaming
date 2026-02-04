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

        float distance = dir.magnitude;

        float arc = 5f;
        Vector2 velocity = new Vector2(dir.x, dir.y + arc).normalized * speed;
        rb.linearVelocity = velocity;
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
