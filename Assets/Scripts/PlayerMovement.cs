using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;

    public Vector2 moveInput;
    private bool jumpPressed;

    // Movement
    public float moveSpeed;
    public float jumpForce;

    // Ground Check
    public Transform groundCheck;
    public float groundradius;
    public LayerMask groundLayer;

    private bool isGrounded;

    public int facing = 1;

    public bool isFrozen = false;
    void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (moveInput.x != 0)
        {
            facing = (int)Mathf.Sign(moveInput.x);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpPressed = true;
    }
    void FixedUpdate()
    {
        if (!isFrozen)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, groundLayer);

        if (!isFrozen && jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else
        {
            jumpPressed = false;
        }

        if (moveInput.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public IEnumerator Freeze(float duration)
    {
        if (isFrozen) yield break;

        isFrozen = true;

        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(duration);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isFrozen = false;
    }


}
