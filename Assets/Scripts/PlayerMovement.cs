using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;

    private Vector2 moveInput;
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
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, groundLayer);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else
        {
            jumpPressed = false;
        }
    }

    
}
