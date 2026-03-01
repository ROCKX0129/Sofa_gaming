using UnityEngine;
using System.Collections;

public class Freeze : MonoBehaviour, IItem
{
    [Header("Item Data")]
    public Item_SO itemSO;
    public Item_SO ItemData => itemSO;

    [Header("Projectile Settings")]
    public float speed = 10f;             
    public float freezeDuration = 2f;     
    public LayerMask playerLayer;
    private bool isShot = false;
    

    private Rigidbody2D rb;
    private Collider2D col;

    private Animator animator;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();

        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
        if (col == null)
            col = gameObject.AddComponent<BoxCollider2D>();

        
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;


    }

    
    public void Shoot(Vector2 direction)
    {
        isShot = true;
        rb.bodyType = RigidbodyType2D.Dynamic; 
        rb.gravityScale = 0f;                  
        col.isTrigger = true;                 
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isShot) return; 

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            var controller = collision.gameObject.GetComponent<PlayerController>();
            if (controller != null)
                controller.StartCoroutine(controller.Freeze(freezeDuration));

        }

        StartCoroutine(PlayHitAndDestroy()); 
    }

    private IEnumerator PlayHitAndDestroy()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
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


