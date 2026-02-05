using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement_AnimationTest : MonoBehaviour
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

    private Animator mAnimator;
    private bool ismoving;
    private Vector3 left;
    private Vector3 right;

    void Awake()
    {
        left = transform.localScale;
        right = transform.localScale;
        right.x *= -1;
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        Debug.Log(left);

    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        Debug.Log(gameObject.name + " MOVE: " + moveInput);
        if (moveInput.x < 0)
        {
            transform.localScale = right;
        }
        if (moveInput.x > 0)
        {
            transform.localScale = left;

        }

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
        ismoving = false;
        //Animaation Parameters
        if (moveInput.x != 0.0f)
        {
            ismoving = true;
        }
        else
        {
            ismoving = false;
        }
        mAnimator.SetBool("OnMoving", ismoving);

        //Flip the player
        
    }
}
